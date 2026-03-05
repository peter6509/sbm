<template>
  <div class="node-filter-container">
    <div class="ef-node-pmenu-item">
      <div style="flex: 1">
        <span class="name"><i class="el-icon-news"></i>{{ $ts("條件設置") }}</span>
      </div>
      <div>
        <el-button link size="small" @click="addItem" type="primary">
          + {{ $ts("添加字段") }}</el-button>
      </div>
    </div>

    <div>
      <table>
        <thead>
          <tr>
            <td>{{ $ts("字段") }}</td>
            <td style="width: 90px">{{ $ts("條件") }}</td>
            <td class="value">{{ $ts("值") }}</td>
            <td style="width: 40px">{{ $ts("操作") }}</td>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, index) in filters" :key="index">
            <td>
              <el-select @change="
                (field) => {
                  fieldChange(field, index);
                }
              " size="small" v-model="item.field" :placeholder="$ts('請選擇')">
                <el-option v-for="data in fieldsOptions" :key="data.field" :label="data.name" :value="data.field" />
              </el-select>
            </td>
            <td style="width: 90px">
              <el-select size="small" v-model="item.filterType" :placeholder="$ts('請選擇')">
                <el-option v-for="data in filterType" :key="data.value" :label="data.name" :value="data.value" />
              </el-select>
            </td>
            <td>
              <template v-if="item.data">
                <el-select v-if="item.data.length >= 300" multiple size="small" v-model="item.value"
                  :placeholder="$ts('請選擇')">
                  <el-option v-for="data in item.data" :key="data.key" :label="data.value" :value="data.key" />
                </el-select>
                <el-select-v2 style="width: 100%" v-else multiple size="small" :options="item.data" v-model="item.value"
                  :placeholder="$ts('請選擇')">
                </el-select-v2>
              </template>
              <el-input v-else v-model="item.value" size="small"></el-input>
            </td>
            <td @click="delItem(index)" class="item-del">
              <i class="el-icon-delete"></i>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
<script setup>
import { ref, watch, onMounted, getCurrentInstance } from 'vue';
const { proxy } = getCurrentInstance();
const props = defineProps({
  tableName: {
    type: String,
    default: "",
  },
  filters: {
    type: Array,
    default: () => [],
  },
})
const filter = ref(proxy.$store.getters.data().flowTable);
const fieldsOptions = ref([]);
const t = ref([]);

const filterType = [
  { name: "等于(=)", value: "=" },
  { name: "不等于(!=)", value: "!=" },
  { name: "大于(>)", value: ">" },
  { name: "大于等于(>=)", value: ">=" },
  { name: "小于(<)", value: "<" },
  { name: "小于等于(<=)", value: "<=" },
  { name: "包括(in)", value: "in" },
  { name: "模糊匹配(like)", value: "like" },
  { name: "或者(or)", value: "or" },
];

// 方法定義
const delItem = (index) => {
  proxy.$confirm("確認要删除字配置條件配置吗?", "警告", {
    confirmButtonText: "確定",
    cancelButtonText: "取消",
    type: "warning",
    center: true,
  }).then(() => {
    props.filters.splice(index, 1);
  });
};

const addItem = () => {
  props.filters.push({ field: "", value: "", filterType: "", data: null });
};

const fieldChange = (field, index) => {
  let option = fieldsOptions.value.find((x) => {
    return x.field == field;
  });
  props.filters[index].field = option.field;
  props.filters[index].value = option.data ? [] : null;
  props.filters[index].data = option.data;
};

const getOptions = (tableName) => {
  const url = `api/Sys_WorkFlow/getFields?table=${tableName}`;
  proxy.http.post(url, {}, false).then((result) => {
    result.forEach((c) => {
      if (c.data && c.data.length < 300) {
        c.data = c.data.map((x) => ({
          value: x.key,
          label: x.value,
          key: x.key,
        }));
      }
    });
    fieldsOptions.value = result;
  });
};

// 監聽
watch(
  () => filter.value.WorkTable,
  (newValue) => {
    if (newValue) {
      getOptions(newValue);
    } else {
      //fieldsOptions.value = [];
    }
  },
  { immediate: true } // 初始時立即執行
);

onMounted(() => {
});
defineExpose({
  getOptions
})
</script>

<style lang="less" scoped>
.node-filter-container {
  margin-top: 15px;

  table {
    width: 100%;
    padding-left: 6px;

    td {
      font-size: 13px;
      padding: 5px;
    }

    tr:first-child {
      font-size: 12px;
      font-weight: bolder;
    }

    .item-del {
      text-align: center;
      color: rgb(226, 4, 4);
      cursor: pointer;
    }

    .value {
      width: 150px;
    }
  }

  .add-btn {
    text-align: right;
    padding-right: 10px;
    border-bottom: 1px solid #e8e8e8;
    padding-bottom: 5px;
  }

  .node-filter-item {}
}

.ef-node-pmenu-item {
  display: flex;
  color: #000;
  align-items: center;
  height: 40px;
  padding: 10px;
  background: #f8f8f8;
  border: 1px solid #eee;

  .name {
    cursor: pointer;
    margin-right: 15px;
  }

  .active {
    color: #0659e8;
  }
}
</style>
