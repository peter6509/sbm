<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/mes/mes/MES_ProductOutbound.jsx或MES_ProductOutbound.vue文件編寫
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
import extend from '@/extension/mes/mes/MES_ProductOutbound.jsx'
import viewOptions from './MES_ProductOutbound/options.js'
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
columns.forEach((x) => {
  if (x.field == 'OutboundQuantity') {
    x.summary = true
  }
})
let gridRef //對應[表.jsx]文件中this.使用方式一樣
//生成對象屬性初始化
const onInit = async ($vm) => {
  gridRef = $vm
  gridRef.setFixedSearchForm(true)
  editFormOptions.forEach((x) => {
    x.forEach((item) => {
      if (item.field == 'MaterialCode' || item.field == 'SpecificationModel') {
        item.readonly = true
      } else if (item.field == 'MaterialName') {
        item.type = 'selectTable'
        item.url = 'api/MES_Material/getPageData'
        item.columns = [
          { field: 'MaterialName', title: '物料名稱', width: 90, search: true },
          { field: 'MaterialCode', title: '物料編碼', width: 120, search: true },
          {
            field: 'CatalogID',
            title: '物料分類',
            bind: { key: '物料分類', data: [] },
            width: 80
          },
          { field: 'Specification', title: '規格型號', width: 110 },
          {
            field: 'Unit',
            title: '單位',
            bind: { key: '物料單位', data: [] },
            width: 70
          },
          { field: 'Remarks', title: '備註信息', width: 150 }
        ]
        item.selectWidth = 760
        item.height = 280
        item.onSelect = (rows) => {
          //给表單字段設置值
          editFormFields.MaterialName = rows[0].MaterialName
          editFormFields.MaterialCode = rows[0].MaterialCode
          editFormFields.SpecificationModel = rows[0].Specification
        }
        item.loadBefore = (param, callback) => {
          //表單没有輸入查詢的走表格輸入框的查詢條件
          const b = param.wheres.some((c) => {
            return c.name == 'MaterialName'
          })
          if (!b) {
            param.wheres.push({
              name: 'MaterialName',
              value: editFormFields.MaterialName,
              displayType: 'like'
            })
          }
          callback(true)
        }
        item.loadAfter = (rows, callback, result) => {
          callback(true)
        }
      }
    })
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
