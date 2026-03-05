<template>
  <div class="edit-container">
    <div class="edit-form-header">
      <div class="edit-form-header-table-name">
        <el-icon>
          <Edit />
        </el-icon>
        <div class="name">{{ $ts(tableCNName) }}{{ stateName }}</div>
        <slot name="title"></slot>
      </div>
      <div class="edit-form-buttons form-buttons">
        <slot name="btn"></slot>
        <template v-for="(btn, index) in buttons" :key="index">
          <el-dropdown v-if="btn.drop" :type="btn.type" :color="btn.color" @click="() => { }">
            <el-button :plain="btn.plain" :type="btn.type" :color="btn.color" size="small">
              {{ $ts(item.name) }}<i class="el-icon-arrow-down el-icon-right"></i>
            </el-button>
            <template #dropdown>
              <el-dropdown-menu>
                <el-dropdown-item @click="() => { }" :name="item.name" v-show="!item.hidden"
                  v-for="(item, dIndex) in btn.data" :key="dIndex">
                  <i :class="item.icon"></i>
                  {{ $ts(item.name) }}</el-dropdown-item>
              </el-dropdown-menu>
            </template>
          </el-dropdown>
          <el-button v-else :disabled="btn.readonly" :plain="btn.plain" :type="btn.type" :color="btn.color"
            v-show="!btn.hidden" @click="btnClick(btn)">
            <i :class="btn.icon"></i>
            {{ $ts(btn.name) }}
          </el-button>
        </template>
      </div>
    </div>
    <el-scrollbar style="flex: 1; height: 0; padding: 12px">
      <slot name="header"></slot>
      <div class="edit-form">
        <!-- -->
        <vol-form :load-key="false" ref="form" :label-position="labelPosition" :label-width="labelWidth"
          :formRules="formOptions" :formFields="formFields" @dicInited="dicInited" @tabClick="tabClick"
          :select2Count="select2Count"></vol-form>
      </div>

      <slot name="content"></slot>

      <!-- 一對多明细表 -->
      <div class="detail-table">
        <detail-table borderCard="" ref="detailsRef" @loadBefore="loadDetailTableBefore"
          @loadAfter="loadDetailTableAfter" @rowChange="detailRowOnChange" @rowClick="rowClick" @tabsClick="tabsClick"
          v-if="details.length" :main-table="tableName" :height="detailHeight" :data="details" @onSortEnd="onSortEnd">
          <template #detailsBtn="{ data }">
            <slot name="detailsBtn" :data="data"></slot>
          </template>
        </detail-table>
      </div>

      <!-- 一對多三级明细表 -->
      <div class="detail-table">
        <detail-table borderCard="" ref="subDetailsRef" @loadBefore="loadSubDetailTableBefore"
          @loadAfter="loadSubDetailTableAfter" @rowChange="detailRowOnChange" @rowClick="rowClick"
          @tabsClick="tabsClick" v-if="subDetails.length" :main-table="tableName" :height="detailHeight"
          :data="subDetails" @onSortEnd="onSortEnd">
          <template #detailsBtn="{ data }">
            <slot name="detailsBtn" :data="data"></slot>
          </template>
        </detail-table>
      </div>

      <!-- 主從表 -->
      <div v-if="detail.columns.length" class="edit-form edit-detail">
        <div class="edit-form-header-title">
          <div class="edit-form-header-table-name">
            <div class="border"></div>
            <div class="name">{{ $ts(detail.cnName) }}</div>
            <slot name="detailContent"></slot>
          </div>
          <div class="edit-form-buttons">
            <template v-for="(btn, index) in detailButtons" :key="index">
              <el-dropdown v-if="btn.drop" @click="() => { }">
                <el-button link plain size="small">
                  {{ $ts(item.name) }}<i class="el-icon-arrow-down el-icon-right"></i>
                </el-button>
                <template #dropdown>
                  <el-dropdown-menu>
                    <el-dropdown-item @click="() => { }" :name="item.name" v-show="!item.hidden"
                      v-for="(item, dIndex) in btn.data" :key="dIndex">
                      <i :class="item.icon"></i>
                      {{ $ts(item.name) }}</el-dropdown-item>
                  </el-dropdown-menu>
                </template>
              </el-dropdown>
              <el-button v-else link v-show="!btn.hidden" @click="btnClick(btn)">
                <i :class="btn.icon"></i>
                {{ $ts(btn.name) }}
              </el-button>
            </template>
          </div>
        </div>
        <vol-table ref="table" @loadBefore="loadDetailTableBefore" @loadAfter="loadDetailTableAfter"
          @rowClick="rowClick" :url="detail.url" :load-key="true" :index="true" :columns="detail.columns"
          :pagination="detail.pagination" :height="detailHeight" :pagination-hide="!!detail.paginationHide"
          :beginEdit="beginEdit" :endEditBefore="endEditBefore" @dicInited="
            (dicData) => {
              dicInited(dicData, detail.table)
            }
          " :summary="true" :column-index="columnIndex" :ck="ck" :text-inline="textInline" :sortable="sortable"
          @onSortEnd="onSortEnd"></vol-table>
      </div>

      <slot name="footer"></slot>
    </el-scrollbar>

    <ViewGridAudit @auditClick="saveAudit" :option="autitTableOptions" ref="audit"> </ViewGridAudit>
  </div>
  <ViewGridPrint ref="printRef"></ViewGridPrint>
</template>
<script setup lang="jsx">
import {
  defineComponent,
  ref,
  reactive,
  watch,
  computed,
  getCurrentInstance,
  defineAsyncComponent
} from 'vue'
import VolEditProps from './VolEditProps.js'
import { loadFormData } from './VolEditLoadFormData.js'
import { loadDicData } from './VolEditLoadDicData.js'
import { saveData } from './VolEditSave.js'
import { ElMessage, ElMessageBox } from 'element-plus'
import { useRouter, useRoute } from 'vue-router'
import store from '@/store/index'
import VolForm from '@/components/basic/VolForm.vue'
import VolTable from '@/components/basic/VolTable.vue'
import DetailTable from '../ViewGrid/ViewGridDetails.vue'
const ViewGridAudit = defineAsyncComponent(() => import('../ViewGrid/ViewGridAudit'))
const ViewGridPrint = defineAsyncComponent(() => import('../ViewGrid/ViewGridPrint.vue'))
const props = defineProps(VolEditProps())

const emit = defineEmits([
  'dicInited',
  'beginEdit',
  'endEditBefore',
  'loadFormAfter',
  'initButtons',
  'initDetailButtons',
  'initDetailColumns',
  'initSubDetailColumns',
  'detailRowChange',
  'onSortEnd',
  'tabsClick',//一對多切换事件
  'tabClick'//表單分组點擊事件
])

const { proxy } = getCurrentInstance()
const router = useRouter()

const printRef = ref()
const route = useRoute()
const id = ref(null)
let isAdd = true
if (props.value || route.query.id) {
  isAdd = false
  id.value = props.value || route.query.id
}
if (!props.detail.url) {
  props.detail.url = `api/${props.tableName}/getDetailPage`
}
if (!props.detail.height) {
  props.detail.height = 500
}
const stateName = computed(() => {
  if (id.value && !route.query.copy) {
    isAdd = false
    return '(' + proxy.$ts('編輯') + ')'
  }
  isAdd = true
  return '(' + proxy.$ts('新建') + ')'
})

//主表按鈕
const buttons = reactive([])
//明细表按鈕
const detailButtons = reactive([])

const detailsRef = ref(null)

//三级表配置
const subDetails = ref([])
//三级表table對象
const subDetailsRef = ref(null)

const form = ref(null)

//明细表refs
const table = ref(null)
const getTable = (_tableName) => {
  if (props.details.length) {
    //二级明细表
    if (
      props.details.some((c) => {
        return c.table == _tableName
      })
    ) {
      return detailsRef.value.$refs[_tableName][0]
    }
    //三级明细表
    return subDetailsRef.value.$refs[_tableName][0]
  }
  return table.value
}

//刷新主表數據
const reloadMainTable = () => {
  proxy.base.getItem[props.tableName + '_edit']?.()
}

//保存
const save = () => {
  form.value.validate((result) => {
    if (!result) return
    saveData(
      proxy,
      props,
      id,
      isAdd,
      delKeys,
      currentRow,
      getTable,
      detailsRef,
      subDetails,
      saveExecting
    )
  })
}
const reloadDetail = () => {
  //刷新一對多表數據
  if (props.details.length) {
    props.details.forEach((item) => {
      if (!id.value) {
        getTable(item.table).rowData.splice(0)
      } else {
        getDetailData(item)
      }
    })
    if (subDetails.value.length && !id.value) {
      subDetails.value.forEach((item) => {
        getTable(item.table).rowData.splice(0)
      })
    }
    return
  }
  if (!id.value) {
    table.value && table.value.rowData.splice(0)
    return
  }
  table.value && table.value.load()
}
const saveExecting = (formData) => {
  const saveUrl = `api/${props.tableName}/${isAdd ? 'add' : 'update'}`
  proxy.http.post(saveUrl, formData, true).then((x) => {
    ElMessage({
      type: x.status ? 'success' : 'error',
      message: x.message
    })
    if (!x.status) {
      return
    }

    reloadMainTable()

    proxy[isAdd ? 'addAfter' : 'updateAfter'](x, (result) => {
      if (result === false)return;
      //刷新列表頁面
      const fn = proxy.base.getItem(`${props.tableName}_edit`)
      fn && fn()
      delKeys.splice(0)
      if (isAdd) {
        //新建后變更為編輯狀態
        if (props.editChange) {
          id.value = JSON.parse(x.data).data[props.keyField]
          isAdd = false
          getData()
          reloadDetail()
          return
        }
        if (x.data) {
          form.value.reset()
        }
        //重置明细表數據
        // table.value && table.value.reset()
        reloadDetail()
        return
      }
      reloadDetail()
    })
  })
}
//删除
const delClick = () => {
  if (isAdd) {
    return
  }
  ElMessageBox.confirm('確定要删除此數據吗?', '提示', {
    confirmButtonText: '確定',
    cancelButtonText: '取消',
    center: true,
    type: 'warning'
  }).then(() => {
    const delUrl = `api/${props.tableName}/del`
    proxy.http.post(delUrl, [id.value], true).then((x) => {
      ElMessage({
        type: x.status ? 'success' : 'error',
        message: x.message
      })
      if (!x.status) {
        return
      }
      reloadMainTable()
      proxy.$tabs.close(route.path)
    })
  })
}

//初始明细表配置
const initDetailButtons = (_buttons, item) => {
  if (!item) {
    item = {}
  }
  _buttons.push({
    name: '刷新',
    hidden: false,
    icon: 'el-icon-refresh',
    onClick: () => {
      getDetailData(item)
    }
  })

  _buttons.push({
    name: '删除行',
    hidden: false,
    readonly: false,
    icon: 'el-icon-delete',
    onClick: () => {
      delRow(item)
    }
  })

  _buttons.push({
    name: '添加行',
    readonly: false,
    hidden: false,
    icon: 'el-icon-plus',
    onClick: () => {
      detailAddRow(item)
    }
  })
  //主從表按鈕
  emit('initDetailButtons', _buttons)
}

let permission = proxy.base.getButtons('/' + props.tableName, null, props.tableName)
if (permission.length == 1 && permission[0].value == 'Search') {
  let routePermission = proxy.base.getButtons(route.path)
  if (routePermission.length > 1) {
    permission = routePermission
  }
}

if (!isAdd && permission.some((x) => x.value == 'Print')) {
  buttons.push({
    name: '打印',
    hidden: false,
    readonly: false,
    icon: 'el-icon-printer',
    plain: true,
    value: 'Print',
    type: 'primary',
    // color:"#F56C6C",
    onClick: () => {
      let ids = [id.value]
      const rows = [currentRow]
      if (!props.printBefore(rows)) {
        return
      }
      const table = props.tableName
      printRef.value.open({ ids, table, rows })
    }
  })
}
const hasBtn = permission.some((x) => x.value == 'Add' || x.value == 'Update')

//初始明细表、多级明细表權限按鈕
const initPermissionButtons = () => {
  if (hasBtn) {
    buttons.push({
      name: '保存',
      hidden: false,
      readonly: false,
      icon: 'el-icon-check',
      plain: true,
      value: 'Save',
      type: 'primary',
      // color:"#F56C6C",
      onClick: () => {
        save()
      }
    })
    if (proxy.details.length) {
      proxy.details.forEach((item) => {
        // item.pagination = { total: 0, size: 100, sortName: item.sortName }
        // item.paginationHide = false
        // item.columns.forEach((col) => {
        //   if (col.link) {
        //     col.link = false
        //   }
        // })
        // //設置默認加載明细表數據
        // if (item.load === undefined) {
        //   item.load = true
        // }
        initDetailButtons(item.buttons, item)
      })
    } else {
      initDetailButtons(detailButtons, {})
    }
  } else {
    //没有新建與編輯權限的設置為只讀
    props.formOptions.forEach((options) => {
      options.forEach((op) => {
        op.readonly = true
      })
    })
    proxy.details.forEach((item) => {
      if (item.columns) {
        item.columns.forEach((op) => {
          op.readonly = true
        })
      }
    })
    if (proxy.detail.columns) {
      proxy.detail.columns.forEach((op) => {
        op.readonly = true
      })
    }
  }
  if (
    permission.some((x) => {
      return x.value == 'Audit'
    }) &&
    !isAdd
  ) {
    buttons.unshift({
      name: '審核',
      readonly: false,
      hidden: false,
      value: 'Audit',
      icon: 'el-icon-edit-outline',
      onClick: () => {
        auditClick()
      }
    })
  }
  if (id.value) {
    if (
      permission.some((x) => {
        return x.value == 'Delete'
      })
    ) {
      buttons.unshift({
        name: '删除',
        readonly: false,
        hidden: false,
        plain: true,
        value: 'Delete',
        type: 'danger',
        color: '#F56C6C',
        icon: 'el-icon-delete',
        onClick: () => {
          delClick()
        }
      })
    }
  }
  buttons.unshift({
    name: '關閉',
    readonly: false,
    hidden: false,
    plain: true,
    value: 'Close',
    //  type: 'info',
    //  color: "#fcfffa",
    icon: 'el-icon-switch-button',
    onClick: () => {
      proxy.$tabs.close(route.path)
    }
  })
}

//初始化二级明细表配置
const initDetailColumns = () => {
  if (props.details.length) {
    if (props.detail && Array.isArray(props.detail.columns)) {
      props.detail.columns = []
    }
    props.details.forEach((item) => {
      item.pagination = Object.assign(
        { total: 0, size: 100, sortName: item.sortName },
        item.pagination
      )
      item.paginationHide = false
      item.columns.forEach((col) => {
        if (col.link) {
          col.link = false
        }
      })
      //設置默認加載明细表數據
      if (item.load === undefined) {
        item.load = true
      }

      //三级明细表
      if (item.detail) {
        //固定明细表高度
        // this.detailOptions.height = 200
        item.detail.height = 200
        /// this.detailHeight = 220
        item.single = true
        item.detail.columnIndex = false
        subDetails.value.push(item.detail)
      }
    })
  } else {
    if (!props.detail.pagination) {
      props.detail.pagination = { total: 0, size: 100 } //, sortName: item.sortName }
    }
  }
}
//初始化三级明细表配置
const initSubDetailColumns = () => {
  subDetails.value.forEach((item) => {
    item.paginationHide = false
    item.pagination = { total: 0, size: 100, sortName: '' }
    if (item.load === undefined) {
      item.load = false
    }
    if (hasBtn) {
      item.buttons.push(
        {
          name: '添加行',
          icon: 'el-icon-plus',
          type: 'primary',
          hidden: false,
          plain: true,
          onClick: (table, item, index) => {
            detailAddRow(item, true)
          }
        },
        {
          type: 'danger',
          plain: true,
          name: '删除行',
          hidden: false,
          icon: 'el-icon-delete',
          onClick: (table, item, index) => {
            delRow(item, true)
          }
        }
      )
    } else {
      item.columns.forEach((x) => {
        x.readonly = true
      })
    }
  })
}

// buttons.unshift({
//   name: '刷新',
//   hidden: false,
//   icon: 'el-icon-refresh',
//   click: () => {}
// });

//明细表選擇加載三级明细表
const detailRowOnChange = (row, item) => {
  //獲取三级明细子表
  if (!item || !item.detail) {
    return
  }
  emit('detailRowChange', row, item)

  let table = item.detail.table
  //三级表對象
  let subRef = getTable(table)
  if (!subRef) {
    return
  }

  if (!row[table]) {
    row[table] = []
  }
  //三级明细表數據
  let subRows = row[table]
  subRef.rowData = row[table]
  // let subRows = row[table];
  // if (subRows) {
  //   subRef.rowData.splice(0);
  //   subRef.rowData.push(...subRows);
  //   // subRef.rowData = subRows;
  //   return;
  // }

  // subRows = [];
  // row[table] = subRows;
  // subRef.rowData.push(...subRows);
  // //處于編輯狀態時没有明细表數據，從后台加載數據
  if (!isAdd && !subRows.length) {
    //從接口添加數據
    subRef.load(null, true)
  }
}

//明细(二)表加載前
const loadDetailTableBefore = (param, callback, table, item) => {
  //新建不添加數據
  if (!id.value) {
    callback(false)
    return
  }
  param.value = id.value

  if (table) {
    param.tableName = table
  }
  resetDelKeys(item)
  props.loadTableBefore(
    param,
    (x) => {
      callback(x)
    },
    table,
    item
  )
}

//三级明细表加載前
const loadSubDetailTableBefore = (param, callback, table, item) => {
  //獲取二级明细表配置
  if (item) {
    param.table = item.table
    param.detailTable = item.secondTable
    param.tableName = item.table
    let detailRows = getTable(item.secondTable).getSelected()
    if (!detailRows || detailRows.length == 0) {
      callback(false)
      return
    }
    param.value = detailRows[0][item.secondKey]
  }
  let rows
  if (!param.value) {
    //獲取三级表當前選中的明细行數據,並且設置二级明细表的id作為查詢條件
    let secondKey = item.secondKey
    //獲取二级明细表選中的行
    // rows = this.getTableRef(item.secondTable).getSelected();
    rows = getCurrentDetailSelectRows(item.secondTable)
    if (!rows) {
      callback(false)
      return
    }
    param.value = rows[0][secondKey]
  }

  //三级明细查詢前
  //新建時禁止加載明细
  if (isAdd) {
    callback(false)
    return false
  }
  props.loadTableBefore(
    param,
    (x) => {
      callback(x)
    },
    table,
    item
  )
}

//明细表加載后
const loadDetailTableAfter = (rows, result, table, item) => {
  if (typeof result == 'function') {
    result(true)
  }

  // //複制數據功能后清空明细表的主鍵值
  // if (id.value && !route.query.copy) {
  //   rows.forEach((row) => {
  //     row[props.table.key] = undefined
  //     if (!item) {
  //       row[dataConfig.detailOptions.key] = undefined
  //     } else {
  //       row[item.key] = undefined
  //     }
  //   })
  // }

  console.log(table)
  props.loadTableAfter(rows, result, (x) => { }, table, item)
  return true
}

//三级明细表加載后
const loadSubDetailTableAfter = (rows, result, table, item) => {
  item.keys = []
  //给二级明细表設置值
  let row = getTable(item.secondTable).getSelected()[0]
  row[item.table] = rows
  props.loadTableAfter(rows, result, (x) => { }, table, item)
  return true
}
//二级表切换時設置三级表選中
const tabsClick = (table) => {
  let obj = props.details.find((x) => {
    return x.table == table
  })
  emit('tabsClick', table)
  //設置三级明细表選中
  if (obj) {
    if (subDetailsRef.value) {
      if (obj.detail) {
        subDetailsRef.value.setTable(obj.detail.table)
      } else {
        subDetailsRef.value.setTable('')
      }
    }
    return
  }
  obj = subDetails.value.find((x) => {
    return x.table == table
  })
  //設置二级明细表選中
  if (obj) {
    detailsRef.value.setTable(obj.secondTable)
  }
}

const rowClick = () => { }

//開始編輯
const beginEdit = (row, column, index) => {
  emit('beginEdit', row, column, index)
  return true
}

//结束編輯前
const endEditBefore = (row, column, index) => {
  emit('endEditBefore', row, column, index)
  return true
}

//獲取二级或者三级明细表中文名稱
const getTableName = (table) => {
  //獲取明细表
  if (!props.details.length) {
    return '未配置二级或三级明细表'
  }

  let ops = props.details.find((x) => {
    return x.table == table
  })
  if (ops) {
    return ops.cnName
  }

  return subDetails.value.find((x) => {
    return x.table == table
  }).cnName
}

//獲取二级或者三级明细表中選中的行
const getCurrentDetailSelectRows = (table) => {
  let rows = getTable(table).getSelected()
  if (!rows.length) {
    proxy.$message.error(
      proxy.$ts('請選中明细表數據') + ':【' + proxy.$ts(getTableName(table)) + '】'
    )
    return null
  }
  return rows
}
//添加行
const detailAddRow = (item, isSub) => {
  //三级表添加行時必須選中二级表數據
  if (isSub && !getCurrentDetailSelectRows(item.secondTable)) {
    return
  }
  if (!item) {
    item = {}
  }
  const tableRef = getTable(item.table)

  let _row = props.addRow(item.table, item)
  //二级明细表
  if (props.details) {
    //二级表添加行，並且有三级明细表,添加三级明细表默認值
    if (!isSub && item.detail && item.detail.table) {
      _row[item.detail.table] = []
    }
  }
  // getCurrentDetailSelectRows(item.secondTable)[0][item.table].push(_row || {});
  // //getTable(item.secondTable).push(_row || {});
  if (proxy.$global.table.insertFirstRow) {
    tableRef.rowData.unshift(_row || {})
  } else {
    tableRef.rowData.push(_row || {})
  }

  //單獨處理設置值，這里數據不是响應式的
  if (isSub) {
    getTable(item.secondTable).getSelected()[0][item.table] = tableRef.rowData
  }
}

const delKeys = []
//一、二、三级明细删除操作
const delRow = (item, isSub) => {
  if (!item) {
    item = {}
  }
  const selectRows = getTable(item.table).getSelected()

  if (!selectRows.length) {
    proxy.$message.error(proxy.$ts('請選擇行數據'))
    return
  }
  //這里二级與三级的table不能响應，待處理
  let secondRows = []
  if (item.secondTable) {
    secondRows = getTable(item.secondTable).getSelected()
    if (!secondRows) {
      return
    }
  }

  if (!props.delRow(selectRows, item)) {
    return
  }
  ElMessageBox.confirm(proxy.$ts('確定要删除選中的行吗?'), proxy.$ts('提示'), {
    confirmButtonText: proxy.$ts('確定'),
    cancelButtonText: proxy.$ts('取消'),
    center: true,
    type: 'warning'
  }).then(() => {
    let key = item.table ? item.key : props.detail.key
    //記錄删除的行數據
    const _keys = selectRows
      .filter((x) => {
        return x.hasOwnProperty(key) && x[key]
      })
      .map((v) => {
        return v[key]
      })
    // console.log(getTable("Demo_ProductColor").rowData);
    // console.log(getTable(item.table).rowData);
    //reloadMainTable()
    resetDelKeys(item, _keys)
    getTable(item.table).delRow()
    //這里二级與三级的table不能响應，待處理
    if (secondRows && secondRows.length) {
      secondRows[0][item.table] = getTable(item.table).rowData
    }
    updateDetailTableSummaryTotal(item.table, _keys.length)
  })
}

const updateDetailTableSummaryTotal = (table, len) => {
  //删除或新增行時重新設置顯示的總行數
  //第一頁才刷新總數量
  if (refTable.paginations.size <= 1) {
    if (len > 0 && refTable.paginations.total - len >= 0) {
      refTable.paginations.total = refTable.paginations.total - len
    } else {
      const refTable = getTable(table)
      refTable.paginations.total = refTable.rowData.length
    }
  }
  //重新設置合計
  if (refTable.summary) {
    refTable.columns.forEach((column) => {
      if (column.summary) {
        refTable.getInputSummaries(null, null, null, column)
      }
    })
  }
}

const workFlowSteps = reactive([])
const audit = ref(null)
//獲取審核節點信息
const auditClick = () => {
  let _row = JSON.parse(JSON.stringify(props.formFields))
  _row[props.keyField] = id.value
  console.log(_row)
  audit.value.open([_row], null, '', false)
}
const saveAudit = (params, rows, callback) => {
  //保存審核
  let keys = rows.map((x) => {
    return x[props.keyField]
  })
  // if (!this.auditBefore(keys, rows)) {
  //   return;
  // }
  let url = `api/${props.tableName}/audit?auditReason=${params.reason}&auditStatus=${params.value}`
  proxy.http.post(url, keys, 'loading....').then((x) => {
    // if (!this.auditAfter(x, keys)) {
    //   return;
    // }
    if (!x.status) return proxy.$message.error(x.message)

    callback && callback(x)
    proxy.$message.success(x.message)
    //this.refresh();
  })
}
//當前操作的行數據
const currentRow = reactive({})

//編輯頁面加載表單數據
const getData = async () => {
  await loadFormData(props, proxy, emit, dicInfo, files, id, buttons, currentRow)
}

const getDetailData = (item, row) => {
  if (!id.value) {
    return
  }
  getTable(item.table).load({}, true)
}

const resetDelKeys = (item, _keys) => {
  if (props.details.length) {
    if (!item.delKeys) {
      item.delKeys = []
    }
    if (_keys && _keys.length) {
      item.delKeys.push(..._keys)
    } else {
      item.delKeys = []
    }
    return
  }
  if (_keys && _keys.length) {
    delKeys.push(..._keys)
  } else {
    delKeys.length = 0
  }
}

//圖片與文件上傳字段
const files = []
const dicInfo = reactive({})

const initDic = async () => {
  await loadDicData(props, proxy, route, emit, dicInfo, files)
  getData()
}
initDic()

const btnClick = (item) => {
  if (item.click) {
    item.click()
  } else if (item.onClick) {
    item.onClick()
  }
}
const autitTableOptions = reactive({
  key: props.keyField,
  tableName: props.tableName,
  url: props.tableName,
  tableCNName: props.tableCNName
})

//表單字典加載后方法
const dicInited = (dicData, table) => {
  emit('dicInited', dicData, table)
}

const loadDetail = (table) => {
  getDetailData({ table: table })
}
//重置表單
const reset = () => {
  form.value && form.value.reset()
  //重置明细表數據
  table.value && table.value.reset()
  getData()
}

//初始化表單、明细表權限按鈕
initPermissionButtons()
//初始化主從子表、二级表配置
initDetailColumns()
//初始化三级明细表配置
initSubDetailColumns()

//向edit.vue頁面傳遞初始化信息
emit('initButtons', buttons)

//二级表配置
if (props.details.length) {
  emit('initDetailColumns', props.details)
}
//三级表配置
if (subDetails.value.length) {
  emit('initSubDetailColumns', subDetails.value)
}
const onSortEnd = (rows, newIndex, oldIndex) => {
  emit('onSortEnd', rows, newIndex, oldIndex)
}

const tabClick = (name) => {
  emit('tabClick', name)
}

// 監聽contentId
watch(
  () => props.value,
  (newValue, oldValue) => {
    if (route.query.id) {
      return
    }
    if (oldValue && newValue) {
      id.value = newValue
      isAdd = false
      getData()
      return
    }
    if (!newValue) {
      isAdd = true
    }
  },
  { deep: true, immediate: true }
)

defineExpose({
  id,
  dicInfo,
  getData,
  buttons,
  detailButtons,
  tabsClick,
  rowClick,
  beginEdit,
  endEditBefore,
  form,
  table,
  detailsRef,
  detailAddRow,
  workFlowSteps,
  currentRow,
  btnClick,
  autitTableOptions,
  audit,
  saveAudit,
  getTable,
  save,
  getDetailData,
  loadDetail,
  dicInited,
  subDetails,
  subDetailsRef,
  reset,
  detailRowOnChange,
  onSortEnd,
  stateName
})
</script>
<style lang="less" scoped>
@import './edit.less';
</style>
