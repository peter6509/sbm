<template>
  <div class="table-item">
    <div class="table-item-header">
      <vol-title icon="edit" title="绑定接口">功能:自動分頁、表格數據加載前后事件、文本超出提示、跨頁面選擇數據(解决多頁數據選擇問題)</vol-title>
      <div class="table-item-buttons">
        <div>
          <el-button type="primary" @click="getSelected" plain>獲取所有分頁選中行</el-button>
          <el-button type="primary" @click="clearSelection" plain>清除所有分頁選中行</el-button>
          <el-button type="primary" @click="search" color="#95d475" plain>刷新</el-button>
        </div>
      </div>
    </div>
    <!-- 
       1、必須設置row-key與reserveSelection屬性后才能分頁選擇數據
       2、row-key為表格數據的唯一值字段(尽量是主鍵id字段)
       3、reserveSelection是否顯示分頁選中的數據
     -->
    <!--*********如果不需要跨頁面選擇數據，請去掉 row-key、reserveSelection 屬性******-->
    <vol-table row-key="Id" :reserveSelection="true" ref="tableRef" :url="url" index :columns="columns"
      :height="maxHeight" :pagination-hide="false" :load-key="true" :column-index="true" @loadBefore="loadBefore"
      @loadAfter="loadAfter" :pagination="pagination"></vol-table>
  </div>
</template>
<script lang="jsx" setup>
import VolTable from "@/components/basic/VolTable.vue";
import { ref, reactive, getCurrentInstance } from "vue";
const { proxy } = getCurrentInstance();
const tableRef = ref();
//計算表格高度
const maxHeight = ref(0);

//分頁信息
const pagination = ref({
  sizes: [15, 20, 30, 40, 60, 90], //默認分頁條大小
  size: 20, //默認分頁大小(每頁大小)
  sortName: '', //排序字段
  order: 'desc' //排序方式desc或者asc
})

maxHeight.value = document.body.clientHeight - 250;
//使用生成的接口返回數據，也可以自定義接口返回，見接口代碼
const url = ref("api/sys_log/getPageData");
//獲取選中
const getSelected = () => {
  const rows = tableRef.value.getSelected();
  if (!rows.length) {
    return proxy.$message.error("請選中行");
  }
  proxy.$message.success(`共選中【${rows.length}】行`);
};
//清除選中
const clearSelection = () => {
  tableRef.value.clearSelection();
  proxy.$message.success("清除成功");
};
//表格數據加載前事件
const loadBefore = async (param, callBack) => {
  //這里可以異步調用接口后再接着后面的操作
  // await proxy.http
  //   .post("api/sys_log/getpagedata", { rows: 200, page: 1 }, true)
  //   .then((x) => {});

  //設置查詢條件
  let wheres = [
    {
      name: "字段名1",
      value: "查詢的值",
      displayType: "like",
      //displayType查詢類型可選值：
      //like        模糊查詢
      //selectList  多選,對應sql的where xxx in (1,2,3)
      //thanorequal 大于等于
      //gt          大于
      //lessorequal 小于等于
      //lt          小于
      //in          對應sql的where xxx in (1,2,3)
      //notIn       對應sql的where xxx not in (1,2,3)
      //其他類型請在后台的查詢方法設置
    },
    {
      name: "字段名2",
      value: "value1,value2", //多選查詢值是逗號隔開
      displayType: "selectList",
    },
  ];
  param.wheres.push(...wheres);
  // param.url='api/xx/xx';這里也可以重新設置url
  param.value = "xx"; //也可以自定義任何值，在后表[表service類]中重寫查詢getPageData方法options.value直接獲取
  await callBack(true); //返回false，界面上不會顯示
};
//表格數據加載后事件
const loadAfter = (rows, callBack, result) => {
  //rows返回的數據
  //注意不要設置rows=xx，這樣會改變數據的内存地址導致修改的值無效
  callBack(true); //返回false，界面上不會顯示
};
const search = () => {
  tableRef.value.load();
};

//接口返回數據，可以框架生成的接口getPageData
//如果是自定義的接口，需要返回的數據格式：{total:100,rows:[]}
const columns = reactive([
  {
    field: "Id",
    title: "Id",
    hidden: true,
  },
  {
    field: "BeginDate",
    title: "開始時間",
    width: 120,
    sort: true,
  },
  {
    field: "UserName",
    title: "用户名稱",
    width: 90,
  },
  {
    field: "Url",
    title: "請求地址",
    width: 200,
    showOverflowTooltip: true, //設置超出鼠標放上去提示
  },
  {
    field: "Success",
    title: "响應狀態",
    bind: { key: "restatus", data: [] },
    width: 80,
  },
  { field: "ElapsedTime", title: "時長", type: "int", width: 60 },
  {
    field: "RequestParameter",
    title: "請求参數",
    width: 150,
    showOverflowTooltip: true, //設置超出鼠標放上去提示
  },
  { field: "UserIP", title: "用户IP", width: 90 },
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
