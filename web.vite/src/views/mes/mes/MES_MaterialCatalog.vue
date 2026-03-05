<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/mes/mes/MES_MaterialCatalog.jsx或MES_MaterialCatalog.vue文件編寫
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
    :addAfter="addAfter"
    :updateAfter="updateAfter"
    :delAfter="delAfter"
    :rowClick="rowClick"
    :modelOpenBefore="modelOpenBefore"
    :modelOpenAfter="modelOpenAfter"
  >
    <!-- 自定義组件數據槽擴展，更多數據槽slot見文檔 -->
    <template #gridHeader> </template>
  </view-grid>
</template>
<script setup lang="jsx">

import extend from '@/extension/mes/mes/MES_MaterialCatalog.jsx'
import viewOptions from './MES_MaterialCatalog/options.js'
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

let gridRef //對應[表.jsx]文件中this.使用方式一樣
//生成對象屬性初始化
const onInit = async ($vm) => {
  gridRef = $vm
  gridRef.queryFields=['CatalogName','CatalogCode','Enable']
  //開啟樹形结構
  gridRef.paginationHide = false
  gridRef.lazy = false
  //樹形结點的id字段
  gridRef.rowKey = 'CatalogID'
  //父级id字段
  gridRef.rowParentField = 'ParentId'

}
let addCurrnetRow
//生成對象屬性初始化后,操作明细表配置用到
const onInited = async () => {
  //绑定表格操作按鈕
  let hasUpdate, hasDel, hasAdd
  gridRef.buttons.forEach((x) => {
    if (x.value == 'Update') {
      x.hidden = true
      hasUpdate = true
    } else if (x.value == 'Delete') {
      hasDel = true
      x.hidden = true //隐藏按鈕
    } else if (x.value == 'Add') {
      x.type = 'primary'
      hasAdd = true
    }
  })
  if (!(hasUpdate || hasDel || hasAdd)) {
    return
  }
  gridRef.columns.push({
    title: '操作',
    field: '操作',
    width: 100,
    fixed: 'right',
    align: 'center',
    render: (h, { row, column, index }) => {
      return (
        <div>
          {hasAdd ? (
            <el-button
              onClick={($e) => {
                addCurrnetRow = row
                gridRef.add()
              }}
              type="primary"
              link
              icon="Plus"
            ></el-button>
          ) : (
            ''
          )}
          {hasUpdate ? (
            <el-button
              onClick={($e) => {
                gridRef.edit(row)
              }}
              type="success"
              link
              icon="Edit"
            ></el-button>
          ) : (
            ''
          )}
          {hasDel ? (
            <el-button
              link
              onClick={($e) => {
                gridRef.del(row)
              }}
              type="danger"
              icon="Delete"
            ></el-button>
          ) : (
            ''
          )}
        </div>
      )
    }
  })
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
  //查詢界面點擊行事件
  // grid.value.toggleRowSelection(row); //單擊行時選中當前行;
}
const modelOpenBefore = async (row) => {
  //彈出框打開后方法
  return true //返回false，不會打開彈出框
}
const modelOpenAfter = (row) => {
  //彈出框打開后方法,設置表單默認值,按鈕操作等
  //點擊行上的添加按鈕事件
  if (addCurrnetRow) {
    //獲取當前组織構架的所有父级id,用于設置新建時父级id的默認值
    //獲取數據數據源
    let data = []
    editFormOptions.forEach((options) => {
      options.forEach((option) => {
        if (option.field == 'ParentId') {
          data = option.orginData
        }
      })
    })
    let parentIds = proxy.base.getTreeAllParent(addCurrnetRow.CatalogID, data).map((x) => {
      return x.id
    })
    //設置編輯表單上级组織的默認值
    editFormFields.ParentId = parentIds
    addCurrnetRow = null
  }
}
//刷新字典
const addAfter = () => {
  gridRef.initDicKeys()
  return true
}
const updateAfter = () => {
  gridRef.initDicKeys()
  return true
}
const delAfter = () => {
  gridRef.initDicKeys()
  return true
}

//監聽表單輸入，做實時計算
//watch(() => editFormFields.字段,(newValue, oldValue) => {	})
//對外暴露數據
defineExpose({})
</script>
