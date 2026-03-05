<template>
  <div class="table-item">
    <div class="table-item-header">
      <div class="table-item-border"></div>
      <vol-title icon="edit" title="多级表頭">
        功能：多级表頭(目前多级表只有基礎顯示功能，不能編輯)</vol-title
      >
      <div class="table-item-buttons">
        <div>
          <el-input
            style="width: 140px; margin-right: 10px"
            v-model="orderNo"
            placeholder="訂單編號"
          ></el-input>
          <el-button type="primary" @click="search" color="#95d475" plain>查詢</el-button>
        </div>
      </div>
    </div>
    <vol-table
      @loadBefore="loadBefore"
      @loadAfter="loadAfter"
      ref="tableRef"
      :url="url"
      index
      :columns="columns"
      :max-height="maxHeight"
      :pagination-hide="false"
      :load-key="true"
      :ck="false"
      :column-index="true"
    ></vol-table>
  </div>
</template>
<script lang="jsx" setup>
import VolTable from "@/components/basic/VolTable.vue";
import { ref, reactive, getCurrentInstance } from "vue";
const { proxy } = getCurrentInstance();
const tableRef = ref();
//接口返回數據，可以框架生成的接口getPageData
//如果是自定義的接口，需要返回的數據格式：{total:100,rows:[]}
const url = ref("api/Demo_Order/getPageData");
//計算表格高度
const maxHeight = ref(0);
maxHeight.value = document.body.clientHeight - 250;
const orderNo = ref();

const loadBefore = (params, callBack) => {
  //調用后台接口前處理
  //設置查詢條件
  params.wheres.push({
    name: "OrderNo",
    value: orderNo.value,
    displayType: "like", //模糊查詢
  });

  //也可以给value設置值，后台自己解析
  // params.value=orderNo.value

  callBack(true); //false不會調用后台接口
};
//查詢后方法
const loadAfter = (rows, callBack, result) => {
  callBack(true);
};

const search = () => {
  tableRef.load({}, true);
  proxy.$message.success("查詢成功");
};

const columns = reactive([
  {
    field: "基礎信息",
    title: "基礎信息",
    type: "string",
    align: "center",
    children: [
      { field: "OrderNo", title: "訂單編號", type: "string", width: 130 },
      {
        field: "OrderType",
        title: "訂單類型",
        type: "int",
        bind: { key: "訂單類型", data: [] },
        width: 70,
      },
      {
        field: "TotalPrice",
        title: "總價",
        type: "decimal",
        width: 60,
        align: "center",
      },
      {
        field: "TotalQty",
        title: "總數量",
        type: "int",
        width: 80,
        align: "center",
      },
      { field: "OrderDate", title: "訂單日期", type: "date", width: 95 },
    ],
  },
  {
    field: "狀態",
    title: "狀態",
    type: "string",
    align: "center",
    children: [
      {
        field: "OrderType",
        title: "訂單類型",
        type: "int",
        bind: { key: "訂單類型", data: [] },
        width: 80,
      },
      {
        field: "OrderStatus",
        title: "訂單狀態",
        type: "int",
        bind: { key: "訂單狀態", data: [] },
        width: 80,
      },
    ],
  },
  {
    field: "創建人信息",
    title: "創建人信息",
    align: "center",
    children: [
      { field: "Creator", title: "創建人", type: "string", width: 90 },
      { field: "CreateDate", title: "創建時間", type: "datetime", width: 110 },
    ],
  },
  {
    field: "修改人信息",
    title: "修改人信息",
    type: "string",
    align: "center",
    children: [
      { field: "Modifier", title: "修改人", type: "string", width: 90 },
      { field: "ModifyDate", title: "修改時間", type: "datetime", width: 110 },
    ],
  },
]);
</script>
<style lang="less" scoped>
.table-item-header {
  display: flex;
  padding: 6px 0;
  .table-item-buttons {
    margin-left: auto;
  }
}
</style>
