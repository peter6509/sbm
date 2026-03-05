<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/sys/system/Sys_Dictionary.jsx或Sys_Dictionary.vue文件編寫
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
    :detailSortEnd="detailSortEnd"
  >
    <!-- 自定義组件數據槽擴展，更多數據槽slot見文檔 -->
    <template #gridBody>
      <el-alert
        style="margin-bottom: 8px"
        title="所有功能數據源下拉框、级聯請在此頁面維护"
        class="alert-primary"
        :closable="false"
      ></el-alert>

    </template>
  </view-grid>
</template>
<script setup lang="jsx">
import extend from '@/extension/sys/system/Sys_Dictionary.jsx'
import viewOptions from './Sys_Dictionary/options.js'
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

let gridRef
//生成對象屬性初始化
const onInit = async ($vm) => {
  gridRef = $vm
  initDb()
  if (proxy.$route.path=='/coderV3') {
      gridRef.load=false;
  }
  gridRef.text = '表單、表格數據字典維护'
  //啟用排序
  gridRef.detailOptions.sortable = true
  //保存后不關閉編輯框
  gridRef.continueAdd = true

  gridRef.paginationHide = true
  gridRef.lazy = false
  //   gridRef.defaultExpandAll = true
  //樹形结點的id字段
  gridRef.rowKey = 'Dic_ID'
  //父级id字段
  gridRef.rowParentField = 'ParentId'

  gridRef.columns.forEach((x) => {
    if (x.field == 'ParentId' || x.field == 'OrderNo') {
      x.hidden = true
    }
  })
  initTableButtons()
  const parentOption = proxy.base.getFormOption(editFormOptions, 'ParentId')

  parentOption.required = false
  parentOption.type = 'cascader'
  parentOption.changeOnSelect = true
  parentOption.data = []
  parentOption.orginData = []
}
const getParentOption = () => {
  return proxy.base.getFormOption(editFormOptions, 'ParentId') || { data: [] }
}
//生成對象屬性初始化后,操作明细表配置用到
const onInited = async () => {
        // gridRef.boxOptions.width=900
  gridRef.detailOptions.pagination.sortName = 'OrderNo,CreateDate' //明细表排序字字段
  gridRef.detailOptions.pagination.order = 'desc' //明细表排序方式desc或者asc
  gridRef.detailOptions.columns.forEach((x) => {
    if (x.field == 'OrderNo') {
      x.tip = {
        text: '數據源按從大小到小降序排序,值越大越靠前顯示'
      }
    }else if (x.field=='Modifier'||x.field=='Creator') {
      x.width=80;
    }
  })
  gridRef.boxOptions.width = 1000
}
const searchBefore = async (param) => {
  //界面查詢前,可以给param.wheres添加查詢参數
  //返回false，則不會執行查詢
  return true
}
let parentCascader
const searchAfter = async (rows, result) => {
  parentCascader = rows.map((x) => {
    return {
      id: x.Dic_ID,
      key: x.Dic_ID,
      label: x.DicName,
      value: x.Dic_ID,
      parentId: x.ParentId
    }
  })
  return true
}
const addBefore = async (formData) => {
  //新建保存前formData為對象，包括明细表，可以给给表單設置值，自己輸出看formData的值
  return saveBefore(formData)
}
const updateBefore = async (formData) => {
  //編輯保存前formData為對象，包括明细表、删除行的Id
  return saveBefore(formData)
}
const saveBefore = (formData) => {
  if (!formData.mainData.ParentId) {
    formData.mainData.ParentId = 0
  } else {
    formData.mainData.ParentId = (formData.mainData.ParentId + '').split(',').pop()
  }
  if (
    editFormFields.DbSql &&
    (editFormFields.DbSql.indexOf('value') == -1 || editFormFields.DbSql.indexOf('key') == -1)
  ) {
    proxy.$message.error(
      'sql語句必須包括key/value字段,如:select orderType as key,orderName as value from order'
    )
    return false
  }
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
    initParentData()
  //彈出框打開后方法,設置表單默認值,按鈕操作等
}

const initParentData = (rows) => {
  let data = parentCascader
  const option = gridRef.getFormOption('ParentId')
  option.orginData = data
  const treeData = proxy.base.convertTree(
    JSON.parse(JSON.stringify(data)),
    (node, item, isRoot) => {}
  )
  option.data = treeData
}

//明细表拖動排序
const detailSortEnd = (rows, newIndex, oldIndex, table) => {
  let orderNo = rows.length * 10
  rows.forEach((x) => {
    orderNo = orderNo - 10
    x.OrderNo = orderNo
  })
}
const addBtnClick = (row) => {
  grid.value.add()
}
defineExpose({})
const initDb = () => {
  const dbSqlOption = proxy.base.getFormOption(editFormOptions, 'DbSql')
  dbSqlOption.labelRender = (h, {}) => {
    return (
      <div>
        <el-popover placement="top-start" title="自定義sql數據源配置" width="720" trigger="hover">
          {{
            default: () => {
              return (
                <div>
                  <div style="font-size:12px;line-height:2">
                    1. 如果需要從數據庫返回數據源sql為：SELECT key字段 as 'key',名稱字段 as 'value'
                    FROM table{' '}
                  </div>
                  <div style="font-size:12px;line-height:2">
                    2. 如果是级聯,數據源sql為： SELECT key字段 AS id,父级id字段 as parentId, key字段
                    AS 'key',名稱字段 AS 'value' FROM table{' '}
                  </div>
                  <div style="font-size:12px;line-height:2">
                    3.
                    注意级聯sql配置：id,parentId,key,value是必須的,返回的大小寫與這幾個字段必須一致{' '}
                  </div>
                  <div style="font-size:12px;line-height:2">
                    4.
                    如果需要根據用户信息加載數據源,這里正常配置sql,再修改后台DictionaryHandler.cs類GetCustomDBSql方法{' '}
                  </div>
                </div>
              )
            },
            reference: () => {
              return (
                <div style="color:#1598ff">
                  <span class="el-icon-warning-outline"></span> 自定義sql數據源
                </div>
              )
            }
          }}
        </el-popover>
      </div>
    )
  }
}
const initTableButtons = () => {
  columns.push({
    title: '操作',
    field: '操作',
    width: 100,
    fixed: 'right',
    align: 'center',
    render: (h, { row, column, index }) => {
      return (
        <div>
          <el-button
            onClick={($e) => {
              addBtnClick(row)
            }}
            type="primary"
            link
            icon="Plus"
          ></el-button>
          <el-button
            onClick={($e) => {
              gridRef.edit(row)
            }}
            type="success"
            link
            icon="Edit"
          ></el-button>
          <el-button
            link
            onClick={($e) => {
              gridRef.del(row)
            }}
            type="danger"
            icon="Delete"
          ></el-button>
        </div>
      )
    }
  })
}
</script>
