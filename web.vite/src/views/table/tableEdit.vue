<template>
  <div class="table-item">
    <div class="table-item-header">
      <vol-title icon="edit" title="表格編輯">
        功能：按條件編輯、編輯事件、api加載數據、自動分頁、圖片顯示及文件上傳、下拉框、輸入回车框事件、自定義按鈕等</vol-title>
      <div class="table-item-buttons">
        <div>
          <el-button type="primary" @click="addRow" plain>添加行</el-button>
          <el-button type="primary" @click="delRow" color="#f89898" plain>删除行</el-button>
          <el-button type="primary" @click="getRow" plain>獲取選中行</el-button>
          <el-button type="primary" @click="reload" color="#95d475" plain>刷新</el-button>
        </div>
      </div>
    </div>
    <!-- 
        ck：顯示複選框
        column-index：顯示行號
        table-data:表格數據
        columns：表格配置
        max-height:表格最大高(也可以設置為:height="maxHeight"固定高度)
     -->
    <vol-table ref="tableRef" :url="url" :ck="true" :columns="columns" :max-height="maxHeight" :pagination-hide="false"
      :load-key="true" :column-index="false" :beginEdit="beginEdit" :endEditBefore="endEditBefore"
      :table-data="tableData"></vol-table>
  </div>
</template>
<script lang="jsx" setup>
import VolTable from "@/components/basic/VolTable.vue";
import { ref, reactive, getCurrentInstance } from "vue";
const { proxy } = getCurrentInstance();
const tableRef = ref();
//計算表格高度
const maxHeight = ref(0);
maxHeight.value = document.body.clientHeight - 250;

const tableData = reactive([]);
//接口返回數據，可以框架生成的接口getPageData
//如果是自定義的接口，需要返回的數據格式：{total:100,rows:[]}
const url = ref("api/Demo_Order/getPageData");
//點擊行進入編輯事件
const beginEdit = (row, column, index) => {
  //進入編輯時，這里可以根據編輯的row行數據，設置表格字段只讀或編輯
  // columns.forEach(x=>{
  //   if (x.field=='字段') {
  //      x.readonly=row.判断字段=='值'
  //   }
  // })
  return true; //false不會進入編輯
};
const endEditBefore = (row, column, index) => {
  //结束編輯時
  proxy.$message.success("第" + (index + 1) + "行结束編輯");
  return true; //false不會结束編輯
};

const getRow = () => {
  const rows = tableRef.value.getSelected();
  if (!rows.length) {
    proxy.$message.error("請選中行");
    return;
  }
  proxy.$message.success(JSON.stringify(rows));
};
const addRow = () => {
  tableRef.value.addRow({ OrderNo: "D2022040600009" });
  //如果批量添加行。請使用：
  //tableRef.value.rowData.push(...[{ OrderNo: "D2022040600009" },{ OrderNo: "D2022040600009" }])
};
const delRow = () => {
  tableRef.value.delRow();
  proxy.$message.success("删除成功");
};
const reload = () => {
  tableRef.value.load(null, true);
  proxy.$message.success("刷新成功");
};

const columns = reactive([
  { field: "Order_Id", title: "Order_Id", type: "guid", width: 110, hidden: true }, //hidden隐藏字段
  {
    field: "OrderNo",
    title: "條件(默認)編輯",
    type: "string",
    width: 130,
    checkEdit: (row, column, index) => {
      //可根據row.字段的值返回true/false是否可以編輯
      // return row.字段=='值'
      return index % 2 == 1
    },
    tip: {
      //自定義表頭提示
      text: "checkEdit中判断row的值是否可以編輯",
      icon: "el-icon-warning-outline",
      color: "#0085ee", //圖標颜色
      click: () => {
        //圖標點擊事件
        proxy.$message.success("點擊了表頭");
      },
    },
    edit: { type: "text", keep: true }, //keep: true始终顯示編輯,可以去掉這個屬性
    onKeyPress: (row, column, $e) => {
      //輸入框輸入事件
      if ($e && $e.keyCode == 13) {
        proxy.$message.success("按了回车");
        //按回车跳轉到下一行編輯
        //tableRef.value.toNextCell(row, "OrderNo", true);
        return;
      }
      //輸的值
    },
    focus: (row, column, $event) => {
      //輸入框獲取焦點事件
      console.log("獲取焦點");
    },
    blur: (row, column, $event) => {
      //輸入框失去獲取焦點事件
      console.log("失去焦點");
    },
  },
  {
    field: "OrderType",
    title: "下拉框",
    type: "int",
    tip: {
      //自定義表頭提示
      text: "點擊表格自動進入編輯",
      icon: "el-icon-warning-outline",
      color: "#0085ee", //圖標颜色
      click: () => {
        //圖標點擊事件
        proxy.$message.success("點擊了表頭");
      },
    },
    bind: { key: "訂單狀態", data: [] },
    width: 90,
    edit: { type: "select" },
    //row:行數據，column字段配置，isClear是否清除，value:選中的值
    onChange: (row, column, isClear, value) => {
      //清除事件
      if (isClear) {
        return;
      }
      //獲取選擇的文本
      const txt = proxy.base.getColumnDicItem(columns, column.field, value).value;
      proxy.$message.success(`請選擇了[${txt}]`);
      //下拉框選擇時可以同時给其他字段設置：row.xx=row.字段；
    },
  },
  {
    field: "TotalQty",
    title: "按條件編輯",
    precision: 2, //precision小數位數
    type: "int",
    width: 80,
    align: "center",
    edit: { type: "number" }, //編輯類型number整數，decimal带小數
    onKeyPress: (row, column, $e) => {
      //绑定輸入事件
    },
    //extra: { text: "%", style: "margin-left:5px;padding-top:4px;" },
  },
  {
    field: "OrderDate",
    title: "日期",
    type: "date", //year/month/datetime/date/time  編輯類型，年、月、日期時間分秒、日期、時間
    width: 95,
    edit: { type: "date" }, //year/month/datetime/date/time  編輯類型，年、月、日期時間分秒、日期、時間
    onChange: (row, column, isClear, value) => {
      //绑定日期選擇事件
    },
  },
  {
    field: "OrderStatus",
    title: "單選",
    type: "switch",
    width: 95,
    edit: { type: "switch" },
    activeText: "取消",
    inactiveText: "正常",
    onChange: (value, row, column) => {
      //注意接口返回的這個字段必須要有值，否則會異常
      //proxy.$message.info(value + "");
    },
  },
  {
    field: "ImgUrl", //圖片、文件上傳事件
    title: "圖片",
    type: "img", //上傳類型可選img、excel、file
    width: 95,
    url: "api/Demo_Order/upload", //配置圖片上傳地址(默認api/表/upload即可)
    edit: {
      type: "img", //可選img、excel、file
      //fileTypes:['jpg','png','pdf'],//指定文件上傳類型
      multiple: true, //多文件上傳
      maxFile: 2, //最多可以上傳3張照片
      maxSize: 5, //文件大小限制5M
      autoUpload: true, //自動上傳
    },
    //選擇文件時
    onChange: (files) => {
      console.log("選擇文件事件");
      //此處不返回true，會中断代碼執行
      return true;
    },
    uploadBefore: (files, params) => {
      //上傳前方法
      console.log("上傳前");
      //上傳前可以自定義参數
      params.xx = "123";
      //上傳前可以自定義参數,從編輯表單、或者當前編輯的行數據中獲取

      // //從編輯的行數據中獲取参數
      // params.xx =
      //后台重寫【表service】類的中upload方法獲取参數:
      //string val= Utilities.HttpContext.Current.Request.Query["xx"]
      return true;
    },
    //上傳后方法
    uploadAfter: (files) => {
      console.log("上傳后");
      return true;
    },
  },
  {
    field: "Customer",
    title: "下拉table",
    edit: { type: "selectTable" }, //下拉table组件
    width: 100,
    url: "api/Demo_Customer/getPageData", //如果是自定義接口見：http://doc.volcore.xyz/web/detailSelectTable.html
    columns: [
      {
        field: "Customer_Id",
        title: "Customer_Id",
        type: "int",
        width: 110,
        hidden: true,
      },
      //設置search:true,則字段可以搜索
      { field: "Customer", title: "客户", type: "string", width: 80, search: true }, //search是否開啟表格上方的字段搜索
      {
        field: "PhoneNo",
        title: "手機",
        type: "string",
        width: 110,
        search: false,
      },
      {
        field: "Province",
        title: "省",
        type: "string",
        bind: { key: "省", data: [] },
        width: 80,
        search: false,
      },
      { field: "DetailAddress", title: "詳细地址", type: "string", width: 120 },
    ],
    //選拔數據后回顯
    onSelect: (editRow, rows) => {
      editRow.Customer = rows[0].Customer;
      editRow.PhoneNo = rows[0].PhoneNo;
    },
    loadBefore: (editRow, params, callback) => {
      //方式1、手動設置查詢條件
      // param.wheres.push({
      //       name:"GoodsName",
      //       value:row.GoodsName,
      //       displayType:"like"
      // })
      //方式2、给param.value設置值，后台手動處理查詢條件
      params.value = editRow.GoodsName;
      callback(true);
    },
    paginationHide: false, //顯示分頁
    height: 137, //表格高度
    single: true, //單選
  },
  {
    field: "PhoneNo",
    title: "下拉(聯動)",
    width: 90,
  },
  {
    field: "Price",
    title: "级聯",
    type: "treeSelect",
    width: 130,
    multiple:true,//是否可以多選
    edit: { type: "treeSelect" },
    bind: {
      key: "tree_roles",
      data: [],
    },
  },
  {
    title: "操作",
    field: "操作",
    width: 80,
    align: "center", // 'center',
    render: (h, { row, column, index }) => {
      return (
        <div>
          <el-button
            link
            onClick={($e) => {
              //啟表格編輯
              tableRef.value.setEdit(index);
            }}
            type="primary"
          >
            編輯
          </el-button>
          <el-button
            link
            onClick={($e) => {
              //结束編輯
              tableRef.value.setEdit(-1);
              //proxy.http調用接口保存數據
            }}
            type="primary"
          >
            保存
          </el-button>
        </div>
      );
    },
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
