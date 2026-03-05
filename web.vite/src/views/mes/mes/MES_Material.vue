<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/mes/mes/MES_Material.jsx或MES_Material.vue文件編寫
 *新版本支持vue或【表.jsx]文件編寫業務,文檔見:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
 -->
<template>
  <div class="tree">
    <div class="left">
      <material-tree ref="treRef" @node-click="nodeClick"></material-tree>
    </div>
    <div class="right">
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
    </div>
  </div>
  <div class="test-ref">
    <MaterialCatalog class="test-ref" ref="catalogRef"></MaterialCatalog>
  </div>
</template>
<script setup lang="jsx">
import QrcodeVue from 'qrcode.vue'
import MaterialTree from './MES_Material/MES_MaterialTree.vue'
import MaterialCatalog from './MES_MaterialCatalog.vue'
import extend from '@/extension/mes/mes/MES_Material.jsx'
import viewOptions from './MES_Material/options.js'
import { ref, reactive, getCurrentInstance, nextTick, watch, onMounted } from 'vue'
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

const catalogRef = ref()


let gridRef //對應[表.jsx]文件中this.使用方式一樣
//生成對象屬性初始化
const onInit = async ($vm) => {
  gridRef = $vm
  gridRef.load = false //設置默認不加載數據，左邊樹點擊的時候加載

  gridRef.buttons.splice(3, 0, {
    name: '添加物料', //按鈕名稱
    icon: 'el-icon-document', //按鈕圖標:组件示例->圖標
    //primary、success、warning、error、info、text、danger
    type: 'primary',
    plain: true,
    onClick: () => {
      catalogRef.value.$refs.grid.add()
    }
  })

  //設置顯示所有查詢條件
  gridRef.setFixedSearchForm(true)

  //配置顯示二維碼
  gridRef.columns.splice(4, 1, {
    title: '物料二維碼',
    field: '物料二維碼',
    width: 100,
    align: 'center',
    tip: {
      text: '表格動態生成二維碼'
    },
    render: (h, { row, column, index }) => {
      return (
        <div style="padding-top:4px">
          {' '}
          <QrcodeVue value={row.CatalogCode} size={40} />
        </div>
      )
    }
  })
}
//生成對象屬性初始化后,操作明细表配置用到
const onInited = async () => {}

let catalogIds
let nodes
//左邊樹點擊事件
const nodeClick = (ids, nodes) => {
  //左邊樹節點點擊事件
  //左邊樹節點的甩有子節點，用于查詢數據
  catalogIds = ids.join(',')
  //左側樹選中節點的所有父節點,用于新建時設置级聯的默認值
  nodes = nodes
  nextTick(() => {
    //調用查詢方法
    gridRef.search()
  })
}
//左邊點擊的時候，設置查詢條件
const searchBefore = async (param) => {
  //界面查詢前,可以给param.wheres添加查詢参數
  //返回false，則不會執行查詢
  if (catalogIds) {
    param.wheres.push({
      name: 'CatalogID', //查詢分類字段，注意字段大小寫
      value: catalogIds,
      displayType: 'selectList'
    })
  }
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

defineExpose({})
</script>
<style scoped lang="less">
.tree {
  display: flex;
  .left {
    width: 200px;
  }
  .right {
    flex: 1;
    width: 0;
  }
}
.test-ref ::v-deep(.layout-container) {
  display: none;
}
</style>
