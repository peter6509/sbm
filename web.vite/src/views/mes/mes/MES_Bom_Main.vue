<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/mes/mes/MES_Bom_Main.jsx或MES_Bom_Main.vue文件編寫
 *新版本支持vue或【表.jsx]文件編寫業務,文檔見:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
 -->
<template>
  <view-grid ref="grid" :columns="columns" :detail="detail" :details="details" :editFormFields="editFormFields"
    :editFormOptions="editFormOptions" :searchFormFields="searchFormFields" :searchFormOptions="searchFormOptions"
    :table="table" :extend="extend" :onInit="onInit" :onInited="onInited" :searchBefore="searchBefore"
    :searchAfter="searchAfter" :addBefore="addBefore" :updateBefore="updateBefore" :rowClick="rowClick"
    :modelOpenBefore="modelOpenBefore" :modelOpenAfter="modelOpenAfter" :detailSortEnd="detailSortEnd">
    <!-- 自定義组件數據槽擴展，更多數據槽slot見文檔 -->
    <template #gridHeader> </template>
    <template #gridFooter>
      <div class="bom-detail">
        <bom-detail ref="bomDetailRef"></bom-detail>
      </div>
    </template>
    <template #modelBody>
      <el-alert style="margin-bottom: -1px" title="可拖動明细表對【序號】自動完成排序" class="alert-primary"
        :closable="false"></el-alert>
    </template>
  </view-grid>
</template>
<script setup lang="jsx">
import bomDetail from './MES_Bom_Detail.vue'
import extend from '@/extension/mes/mes/MES_Bom_Main.jsx'
import viewOptions from './MES_Bom_Main/options.js'
import { ref, reactive, getCurrentInstance, watch, onMounted } from 'vue'
const grid = ref(null)
const { proxy } = getCurrentInstance()
//http請求，proxy.http.post/get
const {
  table,
  editFormFields,
  editFormOptions,
  searchFormFields,
  searchFormOptions,
  columns,
  detail,
  details
} = reactive(viewOptions())

const bomDetailRef = ref();
let gridRef //對應[表.jsx]文件中this.使用方式一樣
//生成對象屬性初始化
const onInit = async ($vm) => {
  gridRef = $vm
  gridRef.dragPosition = 'bottom' //設置拖動改變表格高度
  //缓存主表方法，返回主表選中的行，在下面的bom明细表[MES_Bom_Detail]中會調用
  proxy.base.setItem('getBomSelectRow', () => {
    return gridRef.getTable(true).getSelected();
  })

  //設置彈出框編輯宽度
  gridRef.boxOptions.width = 1100
  //啟用明细表排序
  gridRef.detailOptions.sortable = true

  gridRef.detailOptions.pagination.sortName = "Sequence";  //明细表排序字字段
  gridRef.detailOptions.pagination.order = "asc"; //明细表排序方式desc或者asc
}
//生成對象屬性初始化后,操作明细表配置用到
const onInited = async () => {
  gridRef.height = gridRef.height - 310;
  if (gridRef.height < 200) {
    gridRef.height = 200;
  }
}
const searchBefore = async (param) => {
  //界面查詢前,可以给param.wheres添加查詢参數
  //返回false，則不會執行查詢
  return true
}
const searchAfter = async (rows, result) => {
  return true
}
const addBefore = async (formData) => {
  //新建保存前formData為對象，包括明细表，可以给给表單設置值，自己輸出看formData的值
  return true
}
const updateBefore = async (formData) => {
  //編輯保存前formData為對象，包括明细表、删除行的Id
  return true
}
const rowClick = ({ row, column, event }) => {
  //點擊行清除選中的行(用于下面明细表判断)
  grid.value.clearSelection();
  //點擊行默認選中
  grid.value.toggleRowSelection(row); //單擊行時選中當前行;
  //加載明细表
  bomDetailRef.value.$refs.grid.search();
}
const modelOpenBefore = async (row) => {
  //彈出框打開后方法
  return true //返回false，不會打開彈出框
}
const modelOpenAfter = (row) => {
  //彈出框打開后方法,設置表單默認值,按鈕操作等
}
const detailSortEnd = (rows, newIndex, oldIndex, table) => {
  rows.forEach((x, index) => {
    x.Sequence = index + 1
  })
}
//監聽表單輸入，做實時計算
//watch(() => editFormFields.字段,(newValue, oldValue) => {	})
//對外暴露數據
defineExpose({})
</script>
<style lang="less" scoped>
.bom-detail {
  margin-top: 13px;
  border-top: 7px solid #eee;
}
</style>
