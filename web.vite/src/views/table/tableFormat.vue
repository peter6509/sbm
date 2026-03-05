<template>
  <div class="table-item">
    <div class="table-item-header">
      <vol-title icon="edit" title="格式化+背景颜色+提示">
        功能:單元格格式化、單元格颜色、整行背景颜色、文字提示</vol-title
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
    ></vol-table>
  </div>
</template>
<script setup>
import VolTable from "@/components/basic/VolTable.vue";
import { ref, reactive, getCurrentInstance } from "vue";
const { proxy } = getCurrentInstance();
const tableRef = ref();
const formRef = ref();
const maxHeight = ref(0);
//計算表格高度:屏幕高度减去其他標簽高度
maxHeight.value = document.body.clientHeight - 250;
const url = ref("api/Demo_Order/getPageData");
const search = () => {
  tableRef.value.load();
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
    title: "自定義數據源tag颜色",
    titleStyle: "color:#0085ee", //標題樣式
    width: 120,
    type: "int",
    bind: { key: "訂單類型", data: [] },
    tip: {
      //自定義表頭提示
      text: "自定義數據源tag颜色",
      icon: "el-icon-edit-outline",
      color: "#0085ee", //圖標颜色
      click: () => {
        //圖標點擊事件
        proxy.$message.success("點擊了表頭");
      },
    },
    //設置table有數據源的列為正常顯示(並設置自定義文字颜色)
    normal: true,
    getStyle: (row, column) => {
      //自定義數據源tag颜色
      let color;
      if (row.OrderType == 1) {
        color = "#ff4949";
      } else if (row.OrderType == 2) {
        color = "rgb(1 197 8)";
      } else if (row.OrderType == 3) {
        color = "#F44336";
      } else if (row.OrderType == 4) {
        color = "#ff9a0f";
      } else {
        color = "#673AB7";
      }

      return {
        color: color,
        // background: "rgb(255 242 242)",
        border: "1px solid " + color,
        padding: "1px 2px",
        fontSize: "12px",
        "border-radius": "3px",
      };
    },
  },
  {
    field: "TotalPrice",
    title: "格式化+點擊事件",
    type: "decimal",
    summary: true,
    width: 120,
    formatter: (row, column) => {
      //格式化要顯示的内容
      //格式化配置
      let color = "";
      if (row.TotalPrice > 5000) {
        color = "#f01d0d";
      } else {
        color = "#2196f3";
      }
      return `<a style='color:${color}'>¥${(row.TotalPrice + "").replace(
        /\d{1,3}(?=(\d{3})+(\.\d*)?$)/g,
        "$&,"
      )}</a>`;
    },
    click: (row, column) => {
      //绑定點擊事件
      proxy.$message.success("點擊了" + row.TotalPrice);
    },
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
  {
    field: "PhoneNo",
    title: "手機",
    type: "string",
    width: 110,
    cellStyle: (row, rowIndex, columnIndex) => {
      //指定某個字段設置背景颜色
      //這里可以根據row.字段来判断設置整行的樣式
      //if (row.字段=='值') {
      return { background: "#2196F3", color: "#fff" }; //, color: "#fff" 字體颜色
      //}
    },
  },
  { field: "CreateDate", title: "創建時間", type: "datetime", width: 150 },
]);

//指定整行設置背景颜色
// columns.forEach((x) => {
//   //設置過的不再設置
//   if (x.cellStyle) {
//     return;
//   }
//   x.cellStyle = (row, rowIndex, columnIndex) => {
//     //這里可以根據row.字段来判断設置整行的樣式
//     if (row.elementIndex === 1) {
//       return { background: "#2196F3", color: "#fff" }; //, color: "#fff" 字體颜色
//     }
//   };
// });
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
