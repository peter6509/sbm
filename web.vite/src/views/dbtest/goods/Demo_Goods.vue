<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/dbtest/goods/Demo_Goods.jsx或Demo_Goods.vue文件編寫
 *新版本支持vue或【表.jsx]文件編寫業務,文檔見:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
 -->
<template>
  <view-grid
    ref="grid"
    :columns="columns"
    :detail="detail"
    :details="details"
    :editFormFields="editFormFields"
    :editFormOptions="editFormOptions"
    :searchFormFields="searchFormFields"
    :searchFormOptions="searchFormOptions"
    :table="table"
    :extend="extend"
    :onInit="onInit"
    :onInited="onInited"
    :searchBefore="searchBefore"
    :searchAfter="searchAfter"
    :addBefore="addBefore"
    :updateBefore="updateBefore"
    :rowClick="rowClick"
    :modelOpenBefore="modelOpenBefore"
    :modelOpenAfter="modelOpenAfter"
  >
    <!-- 自定義组件數據槽擴展，更多數據槽slot見文檔 -->
    <template #gridHeader> </template>
  </view-grid>
</template>
<script setup lang="jsx">
import QrcodeVue from 'qrcode.vue'
import extend from '@/extension/dbtest/goods/Demo_Goods.jsx'
import viewOptions from './Demo_Goods/options.js'
import { ref, reactive, getCurrentInstance,  watch, onMounted } from 'vue'
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

const qrCodeOptions = ref({
  width: 100, // 二維碼的宽度
  height: 100, // 二維碼的高度
  colorDark: '#000000', // 二維碼的深色部分颜色
  colorLight: '#ffffff' // 二維碼的浅色部分颜色
})

let gridRef //對應[表.jsx]文件中this.使用方式一樣
//生成對象屬性初始化
const onInit = async ($vm) => {
  gridRef = $vm
  //與jsx中的this.xx使用一樣，只需將this.xx改為gridRef.xx
  //更多屬性見：https://doc.volcore.xyz/docs/view-grid

  gridRef.columns.splice(4, 1, {
    title: '二維碼',
    field: '二維碼',
    width: 70,
    align: 'center',
    tip:{
        text:"表格動態生成二維碼"
    },
    render: (h, { row, column, index }) => {
      return (
        <div style="padding-top:4px">
          {' '}
          <QrcodeVue value={row.GoodsCode} size={60} />
        </div>
      )
    }
  })
}
//生成對象屬性初始化后,操作明细表配置用到
const onInited = async () => {}
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
  //查詢界面點擊行事件
  // grid.value.toggleRowSelection(row); //單擊行時選中當前行;
}
const modelOpenBefore = async (row) => {
  //彈出框打開后方法
  return true //返回false，不會打開彈出框
}
const modelOpenAfter = (row) => {
  //彈出框打開后方法,設置表單默認值,按鈕操作等
}
//監聽表單輸入，做實時計算
//watch(() => editFormFields.字段,(newValue, oldValue) => {	})
//對外暴露數據
defineExpose({})
</script>
