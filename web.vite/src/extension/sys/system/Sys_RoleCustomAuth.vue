<template>
  <vol-box v-model="model" :padding="15" :title="title" :width="610">
    <el-alert style="margin-bottom: 5px" :title="$ts('指定自定義值、用户、角色、部門數據權限過滤')" class="alert-primary"
      :closable="false"></el-alert>
    <el-radio-group v-model="radioValue" style="margin-bottom: 10px">
      <!-- <el-radio :value="0" :label="$ts('自定義值')"></el-radio>
      <el-radio :value="1" :label="$ts('當前用户')"></el-radio>
      <el-radio :value="2" :label="$ts('當前角色')"></el-radio>
      <el-radio :value="3" :label="$ts('當前部門')"></el-radio> -->
      <el-radio :value="item.value" :label="$ts(item.label)" v-for="(item, index) in radioData" :key="index"></el-radio>
      <el-button type="primary" @click="addClick" size="small" plain>+ {{ $ts("添加") }}</el-button>
    </el-radio-group>
    <div style="min-height: 250px">
      <div class="field-item fx" v-for="(item, index) in filters" :key="index">
        <!-- <div>{{ $ts("字段") }}：</div> -->
        <label style="margin-right: 8px">{{ index + 1 }}. </label>
        <el-select style="width: 150px" @change="
          (field) => {
            fieldChange(field, index);
          }
        " v-model="item.field" :placeholder="$ts('請選擇字段')">
          <el-option v-for="data in fieldsOptions" :key="data.field" :label="data.title" :value="data.field" />
        </el-select>
        <!-- <label>{{ $ts("條件") }}：</label> -->
        <el-select style="width: 120px; margin: 0 10px" v-model="item.filterType" :placeholder="$ts('請選擇條件')">
          <el-option v-for="data in filterType" :key="data.value" :label="data.name" :value="data.value" />
        </el-select>
        <!-- <label>{{ $ts("過滤值") }}：</label> -->
        <div style="width: 250px">
          <div v-if="item.valueType > 0">
            <!-- {{ $ts(getValueText(item)) }} -->
            <el-select v-model="item.value" :placeholder="$ts('請選擇')">
              <el-option v-for="data in authField" :key="data.field" :label="$ts(data.name)" :value="data.field" />
            </el-select>
          </div>
          <el-select v-else-if="item.data" multiple v-model="item.value" :placeholder="$ts('請選擇')">
            <el-option v-for="data in getDicData(item)" :key="data.key" :label="data.value" :value="data.key" />
          </el-select>
          <el-input-number v-else-if="
            item.type == 'int' || item.type == 'number' || item.type == 'decimal'
          " v-model="item.value" :placeholder="$ts('請輸入值')" controls-position="right" />

          <el-date-picker v-else-if="['date', 'datetime', 'month', 'year'].indexOf(item.type) != -1" clearable
            v-model="item.value" :type="item.type" :placeholder="$ts('請選擇')" :value-format="getDateFormat(item)">
          </el-date-picker>
          <el-input v-else :placeholder="$ts('請輸入值,多值以逗號隔開')" v-model="item.value"></el-input>
        </div>
        <el-button @click="delItem(index)" link icon="Delete" style="margin-left: 10px"></el-button>
      </div>
    </div>
    <template #footer>
      <div class="btns">
        <el-button type="default" size="small" icon="Close" @click="model = false">{{
          $ts("關閉")
        }}</el-button>
        <el-button type="primary" plain size="small" icon="Check" @click="save">{{
          $ts("確定")
        }}</el-button>
      </div>
    </template>
  </vol-box>
</template>
<script setup>
import { getDateFormat } from "@/components/basic/VolForm/VolFormDate.js";
import { ref, getCurrentInstance, computed } from "vue";
const { proxy } = getCurrentInstance();
const radioValue = ref(0);

const authField = ref([])

const radioData = ref([
  {
    value: 0,
    label: "自定義值",
  },
  {
    value: 1,
    label: "當前用户字段",
  },
  // {
  //   value: 2,
  //   label: "當前角色",
  // },
  // {
  //   value: 3,
  //   label: "當前部門",
  // },
]);

const title = ref("");
const fieldsOptions = ref([]);

const filters = ref([{ valueType: "", field: "", value: null, data: null }]);

const model = ref(false);
let tableName = "";

const dicInfo = ref([]);
const getDic = () => {
  const keys = fieldsOptions.value
    .filter((x) => {
      return x.key;
    })
    .map((x) => {
      return x.key;
    });
  if (!keys.length) {
    return;
  }
  proxy.http.post("api/Sys_Dictionary/GetVueDictionary", keys, false).then((x) => {
    dicInfo.value = x;

  });
};

const getFields = async (item) => {
  const url = `api/role/getFields?menuId=${item.id}&roleId=${item.Role_Id}`;
  await proxy.http.post(url, {}, false).then((x) => {
    fieldsOptions.value = x.data;
    authField.value = x.authField;
  });
};
let menuId = 0;
const currentItem = ref({});
const open = async (id, item) => {
  radioValue.value = 0;
  currentItem.value = item;
  if (
    item.authMenuData &&
    typeof item.authMenuData == "string" &&
    item.authMenuData.indexOf(",") != -1
  ) {
    filters.value = JSON.parse(item.authMenuData);
    filters.value.forEach((item) => {
      if (item.arr && item.value) {
        item.value = (item.value + "").split(",");
        item.data = [];
      }
    });
  } else {
    filters.value = [{ valueType: "", field: "", value: null, data: null }];
  }
  title.value = proxy.$ts("自定義數據權限") + "(" + proxy.$ts(item.text) + ")";
  model.value = true;
  if (menuId == id) {
    return;
  }
  menuId = id;
  await getFields(item);
  getDic();
};
// open();

const getDicData = (item) => {
  item = fieldsOptions.value.find((x) => {
    return item.field == x.field;
  });
  if (!item || !item.key) {
    return [];
  }
  const dicItem = dicInfo.value.find((x) => {
    return x.dicNo == item.key;
  });
  if (!dicItem) {
    return [];
  }
  return dicItem.data;
};

const getValueText = (item) => {
  let data =
    radioData.value.find((c) => {
      return c.value == item.valueType;
    }) || {};
  return data.label;
};

const filterType = ref([
  { name: "等于(=)", value: "=" },
  { name: "不等于(!=)", value: "!=" },
  { name: "大于(>)", value: ">" },
  { name: "大于等于(>=)", value: ">=" },
  { name: "小于(<)", value: "<" },
  { name: "小于等于(<=)", value: "<=" },
  { name: "包括(in)", value: "in" },
  // { name: "不包括(not in)", value: "notin" },
  { name: "模糊匹配(like)", value: "like" },
  { name: "或者(or)", value: "or" },
]);

const fieldChange = (field, index) => {
  let option = fieldsOptions.value.find((x) => {
    return x.field == field;
  });
  filters.value[index].type = option.type;
  filters.value[index].field = option.field;
  filters.value[index].value = option.key ? [] : null;
  filters.value[index].data = option.key ? [] : null;
};

const addClick = () => {
  filters.value.push({ filterType: null, valueType: radioValue.value, value: null });
};

const delItem = (index) => {
  filters.value.splice(index, 1);
};

const save = () => {
  const list = filters.value.filter((x) => {
    return x.field || x.value || x.valueType;
  });
  let index = list.findIndex((x) => {
    return !x.field;
  });
  if (index != -1) {
    proxy.$message.error(proxy.$ts(`請選擇第${index + 1}行的字段`));
    return;
  }
  index = list.findIndex((x) => {
    return !x.value && !x.valueType;
  });
  if (index != -1) {
    proxy.$message.error(proxy.$ts(`請輸入第${index + 1}行的值`));
    return;
  }
  currentItem.value.authMenuData = JSON.stringify(
    list.map((x) => {
      return {
        field: x.field,
        value: Array.isArray(x.value) ? x.value.join(",") : x.value,
        valueType: x.valueType, //值的類型，0自定義,1當前用户,2當前角色,3當前部門
        filterType: x.filterType,
        arr: Array.isArray(x.value) ? 1 : 0,
      };
    })
  );
  model.value = false;
};

defineExpose({
  open,
});
</script>
<style scoped lang="less">
.fx {
  display: flex;
  align-items: center;

  label {
    display: inline-block;
    margin-left: 10px;
    font-size: 13px;
    color: #000;
  }

  label:first-child {
    margin-left: 0;
  }
}

.field-item {
  margin-bottom: 10px;
}

.btns {
  text-align: center;
}
</style>
