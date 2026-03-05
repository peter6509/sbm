import { getGridTableRef, getDetailTableRef } from './ViewGridRef.js'
import { resetSearch, setFixedSearch, initOntinueAdd, searchExec } from './ViewGridProvider.jsx'
//组件内部方法
export const initMethods = (proxy, props, dataConfig) => {
  //查詢
  const search = (params, resetPage) => {
    searchExec(proxy, params, resetPage)
  }
  //刷新
  const refresh = () => {
    search(null, true)
  }
  const getSearchParameters = () => {
    return proxy.base.getSearchParameters(proxy, props.searchFormFields, props.searchFormOptions)
  }

  //重置查詢
  const onResetSearch = () => {
    resetSearch(proxy, props)
  }
  //高级查詢
  const advancedSearch = () => {
    dataConfig.searchBoxShow.value = false
    search()
  }
  //快捷搜索
  const quickSearchKeyPress = ($event) => {
    //查詢字段為input時，按回车查詢
    if ($event.keyCode === 13) {
      search()
    }
  }
  //設置顯示所有查詢條件
  const setFiexdSearchForm = (visiable) => {
    setFixedSearchForm(visiable)
  }
  const setFixedSearchForm = (visiable) => {
    setFixedSearch(proxy, props, dataConfig, visiable)
  }

  //獲取編輯表單配置
  const getFormOption = (field) => {
    return proxy.base.getFormOption(props.editFormOptions, field)
  }
  //獲取編輯表單配置
  const getSearchFormOption = (field) => {
    return proxy.base.getFormOption(props.searchFormOptions, field)
  }
  //編輯表單分组事件
  const editFormTabClick = (name) => {
    //表單分组切换事件
    proxy.tabClick.call(proxy, name)
    props.tabClick(name)
  }
  //獲取voltable對象,table=true獲取主表table
  const getTable = (table) => {
    return getGridTableRef(proxy, props, table)
  }
  const getTableRef = (table) => {
    return getGridTableRef(proxy, props, table)
  }
  //獲取原生el table组件
  const getElementTableRef = (table) => {
    return getGridTableRef(proxy, props, table).getTable()
  }
  //主表點擊行選中當前行
  const toggleRowSelection = (row) => {
    getGridTableRef(proxy, props, true).toggleRowSelection(row);
  }
   //清除選中的行
  const clearSelection=()=>{
    getGridTableRef(proxy, props, true).clearSelection();
  }

  //獲取主表選中的行
  const getSelectRows = () => {
    //獲取選中的行
    return getGridTableRef(proxy, props, true).getSelected()
  }
  //獲取主表選中的行
  const getSelected = () => {
    return getSelectRows()
  }
  //獲取明细表選中的行
  const getDetailSelectRows = (table) => {
    return getDetailTableRef(proxy, props, table).getSelected()
  }
  //獲取主表數據
  const rowData = () => {
    return getDetailTableRef(proxy, props, table).rowData
  }
  const getCurrentAction = () => {
    if (dataConfig.currentReadonly.value) return ''
    return `(${proxy.$ts(dataConfig.currentAction.value === action.ADD ? '新增' : '編輯')})`
  }
  //刷新明细表合計
  const updateDetailTableSummaryTotal = (fields, table) => {
    getTableRef(table).updateSummary(fields)
  }
  //設置\取消只讀
  const setFormReadonly = (readonly) => {
    props.editFormOptions.forEach((x) => {
      x.forEach((ops) => {
        ops.readonly = !!readonly
      })
    })
    dataConfig.detailOptions.columns.forEach((x) => {
      x.readonly = !!readonly
    })
    dataConfig.detailOptions.buttons.forEach((x) => {
      x.hidden = !!readonly
    })
    if (props.details.length) {
      props.details.forEach((x) => {
        if (x.columns) {
          x.columns.forEach((col) => {
            col.readonly = !!readonly
          })
          x.buttons.forEach((btn) => {
            btn.hidden = !!readonly
          })
        }
      })
    }
  }

  return {
    editFormTabClick,
    getSelectRows,
    getSelected,
    getDetailSelectRows,
    getTable,
    getTableRef,
    getElementTableRef,
    rowData,
    getFormOption,
    getSearchFormOption,
    search,
    refresh,
    getSearchParameters,
    onResetSearch,
    quickSearchKeyPress,
    advancedSearch,
    setFiexdSearchForm,
    setFixedSearchForm,
    getCurrentAction,
    setFormReadonly,
    updateDetailTableSummaryTotal,
    toggleRowSelection,
    clearSelection
  }
}
