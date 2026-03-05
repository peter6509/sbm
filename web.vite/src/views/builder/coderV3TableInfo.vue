<template>
  <div class="form-content">
    <vol-form :label-width="90" ref="formRef" label-position="top" :form-rules="formOptions"
      :form-fields="formFields"></vol-form>
  </div>
  <div class="table-content">
    <div class="fx">
      <vol-title icon="Monitor" title="表结構信息"> </vol-title>
      <div class="desc"> <span style="color: #0247de; font-size: 13px;" class="el-icon-info">修改后數據庫表字段，請點擊同步表结構、生成model、生成頁面</span>
      </div>
      <div class="btns">
        <el-button link @click="delRow"><i class="el-icon-delete"></i> 删除行數據</el-button>
        <el-button link @click="syncTable">
          <i class="el-icon-refresh"></i>同步表结構</el-button>
      </div>
    </div>
    <vol-table :sortable="true" ref="tableRef" :paginationHide="true" @onSortEnd="onSortEnd" :table-data="tableData"
      :max-height="tableHeight" :columns="columns" :color="false" :index="true"></vol-table>
  </div>
</template>
<script lang="jsx" setup>
import { ref, getCurrentInstance } from "vue";
const { proxy } = getCurrentInstance();
const formRef = ref();
const tableRef = ref();
const tableHeight = ref(0);
tableHeight.value = document.body.clientHeight - 443;
if (tableHeight.value <= 260) {
  tableHeight.value = 260;
}

const pros = defineProps({
  formOptions: {
    type: Array,
    default: () => {
      return [];
    },
  },
  formFields: {
    type: Object,
    default: () => {
      return {};
    },
  },
  columns: {
    type: Array,
    default: () => {
      return [];
    },
  },
  tableData: {
    type: Array,
    default: () => {
      return [];
    },
  },
  syncClick: {
    type: Function,
    default: () => { },
  },
});

const validate = async () => {
  return await formRef.value.validate();
};

const delRow = () => {
  proxy
    .$confirm("删除警告?", "確認要删除選擇的數據吗", {
      confirmButtonText: "確定",
      cancelButtonText: "取消",
      type: "warning",
      center: true,
    })
    .then(() => {
      tableRef.value.delRow();
      proxy.$message.success("删除成功,點擊保存后或生成頁面才會生成效");
    });
};

const syncTable = () => {
  if (!pros.formFields.tableName) return proxy.$message.error("請選模塊");
  const url = "api/builder/syncTable?tableName=" + pros.formFields.tableName;
  proxy.http.post(url, {}, true).then((x) => {
    if (!x.status) {
      return proxy.$message.error(x.message);
    }
    proxy.$message.success(x.message);
    syncClick(pros.formFields.table_Id);
  });
};

const onSortEnd = (rows) => {
  let orderNo = 10000;
  rows.forEach((x) => {
    orderNo = orderNo - 50;
    x.orderNo = orderNo;
  });
};
defineExpose({
  validate,
});
</script>
<style lang="less" scoped>
@import "./style/coderV3Content.less";
.desc{
  display: flex;
    align-items: center;
}
</style>
