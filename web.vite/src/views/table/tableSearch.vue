<template>
  <div class="table-item">
    <div style="padding-top: 10px">
      <vol-form
        :labelWidth="90"
        ref="formRef"
        :formFields="formFields"
        :formRules="formRules"
      >
      </vol-form>
    </div>

    <div class="table-item-header">
      <vol-title icon="edit" title="表格編輯">
        功能:自定義頁面使用框架表單volform做查詢條件,内部自動绑定數據源,並調用生成頁面接口,默認不需要寫后台代碼，减少大量工作</vol-title
      >
      <div class="table-item-buttons">
        <div>
          <el-button type="primary" @click="search" plain>查詢</el-button>
        </div>
      </div>
    </div>
    <vol-table
      ref="tableRef"
      :ck="false"
      column-index
      :columns="columns"
      :max-height="maxHeight"
      :url="url"
      :pagination-hide="false"
      @loadBefore="loadBefore"
      :spanMethod="spanMethod"
    ></vol-table>
  </div>
</template>
<script setup>
import VolTable from "@/components/basic/VolTable.vue";
import VolForm from "@/components/basic/VolForm.vue";
import { ref, reactive, getCurrentInstance, nextTick } from "vue";
const { proxy } = getCurrentInstance();
const tableRef = ref();
const formRef = ref();
const maxHeight = ref(0);
//計算表格高度:屏幕高度减去其他標簽高度
maxHeight.value = document.body.clientHeight - 320;
const url = ref("api/Demo_Order/getPageData");

const search = () => {
  tableRef.value.load();
};
//getSearchParameters
//表格數據加載前設置查詢條件
const loadBefore = (param, callBack) => {
  //1.自動生成查詢條件
  param.wheres = proxy.base.getSearchParameters(proxy, formFields, formRules);
  //2.手動設生成詢條件
  // let wheres = [
  //   {
  //     name: "OrderNo",
  //     value: formFields.OrderNo,
  //     displayType: "like",
  //     //displayType查詢類型可選值：
  //     //like        模糊查詢
  //     //selectList  多選,對應sql的where xxx in (1,2,3)
  //     //thanorequal 大于等于
  //     //gt          大于
  //     //lessorequal 小于等于
  //     //lt          小于
  //     //in          對應sql的where xxx in (1,2,3)
  //     //notIn       對應sql的where xxx not in (1,2,3)
  //     //其他類型請在后台的查詢方法設置
  //   }
  // ];
  // param.wheres.push(...wheres);
  callBack(true); //返回false，界面上不會顯示
};

//合並單元格，自己根據rows計算要合並的行與列坐標
const spanMethod = ({ row, column, rowIndex, columnIndex }, rows) => {
  //spanMethod放在[表.js]文件中，與onInit方法同级
  //根據rowIndex, columnIndex 值按需要合並
  if (rowIndex % 3 === 0) {
    if (columnIndex === 7) {
      return [1, 2];
    } else if (columnIndex === 5) {
      return [0, 0];
    }
  }
  if (columnIndex === 1) {
    if (rowIndex % 2 === 0) {
      return {
        rowspan: 2,
        colspan: 1,
      };
    } else {
      return {
        rowspan: 0,
        colspan: 0,
      };
    }
  }
};

const columns = reactive([
  { field: "Order_Id", title: "Order_Id", type: "guid", width: 110, hidden: true },
  {
    field: "OrderNo",
    title: "訂單編號",
    filterData: true, //開啟篩選
    showOverflowTooltip: true, //内容超出時鼠標放上去提示文本
    align: "center", //居中顯示：center,居左顯示:left，靠右顯示:right
    fixed: true, //固定列, 如果固定到右邊，true改為"right"
    width: 100,
  },
  {
    field: "OrderType",
    title: "訂單類型",
    titleStyle: "color:#0085ee", //標題樣式
    tip: {
      //自定義表頭提示
      text: "自定表頭提示",
      icon: "el-icon-edit-outline",
      color: "#0085ee", //圖標颜色
      click: () => {
        //圖標點擊事件
        proxy.$message.success("點擊了表頭");
      },
    },
    type: "int",
    bind: { key: "訂單類型", data: [] },
    width: 70,
  },
  {
    field: "TotalPrice",
    title: "總價",
    type: "decimal",
    summary: true,
    width: 60,
    sort: true, //排序
    align: "center",
    summaryFormatter: (val, col, data, summaryData) => {
      //自定義格式化合計顯示
      if (!val) {
        return "";
      }
      summaryData[0] = "汇總+"; //自定義合計名稱
      //自定義合計值的格式
      val = (val + "").replace(/\d{1,3}(?=(\d{3})+(\.\d*)?$)/g, "$&,");
      return "¥" + val;
    },
  },
  {
    field: "TotalQty",
    title: "總數量",
    type: "int",
    summary: true, //開啟合計
    numberLength: 2, //保留兩位小數
    width: 80,
    align: "center",
  },
  { field: "OrderDate", title: "訂單日期", type: "date", width: 95 }, //如果要顯示日期時分秒改為type: "datetime"
  { field: "Customer", title: "客户", type: "string", width: 80 },
  { field: "PhoneNo", title: "手機", type: "string", width: 110 },
  { field: "CreateDate", title: "創建時間", type: "datetime", width: 150 },
]);

//表單查詢配置
//更多表單配置信息，見表單示例
const formFields = reactive({
  OrderNo: null,
  OrderType: null,
  TotalQty: [null, null],
  OrderDate: [null, null],
  OrderStatus: null,
});
const formRules = reactive([
  [
    { title: "訂單編號", field: "OrderNo", type: "like" }, //like設置為模糊查詢
    {
      dataKey: "訂單類型",
      data: [],
      title: "訂單類型",
      field: "OrderType",
      type: "select",
    },
    { title: "總數量", field: "TotalQty", type: "number", range: true },
    { title: "訂單日期", range: true, field: "OrderDate", type: "datetime" },
    {
      dataKey: "訂單狀態",
      data: [],
      title: "訂單狀態",
      field: "OrderStatus",
      type: "select",
    },
  ],
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
.small-text {
  font-size: 12px;
  color: #2196f3;
  padding-top: 10px;
  position: relative;
  top: 2px;
}
</style>
