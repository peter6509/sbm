
export const resetPage = (paginations) => {
  // 重置查詢分頁
  // this.paginations.rows = 30;
  paginations.page = 1
}
export const loadData = async (
  props,
  proxy,
  vnode,
  tableData,
  emit,
  loading,
  paginations,
  query,
  isResetPage,
  isPageLoad,
  randomTableKey
) => {
  // isResetPage重置分頁數據
  if (!props.url) return
  if (isResetPage) {
    resetPage(paginations)
  }
  let param = {
    page: paginations.page,
    rows: props.paginationHide ? 1000 : paginations.rows,
    sort: paginations.sort,
    order: paginations.order,
    wheres: [] // 查詢條件，格式為[{ name: "字段", value: "xx" }]
  }
  let status = true
  // 合並查詢信息(包查詢分頁、排序、查詢條件等)
  if (query) {
    if (Array.isArray(query)) {
      param.wheres = query;
    } else {
      param = Object.assign(param, query)
    }
  }
  if (!(await props.loadBeforeAsync(param))) {
    return
  }
  let eventName = 'loadBefore'
  let onEventName = `on${eventName[0].toUpperCase() + eventName.slice(1)}`
  if (vnode.props && onEventName in vnode.props) {
    // console.log('参數長度',vnode.props[onEventName].length);
    //兼容自定義頁面早期寫法問題
    if (vnode.props[onEventName]?.length <= 1) {
      emit(eventName, param);
    } else {
      const loadBefore = () => {
        return new Promise((resolve) => {
          emit(eventName, param, resolve)
        })
      }
      status = await loadBefore()
    }
  }
  if (!status) return

  if (param.wheres && Array.isArray(param.wheres)) {
    param.wheres = JSON.stringify(param.wheres)
  }
  loading.value = true
  let url = param.url || props.url
  param.url = undefined
  let data = await proxy.http.post(url, param).then(
    (res) => {
      return res
    },
    (error) => {
      console.log(error)
      loading.value = false
    }
  )
  if (Array.isArray(data)) {
    data = { rows: data, total: data.length }
  }
  //2021.06.04修複tree不刷新的問題
  if (props.rowKey) {
    randomTableKey.value++
    tableData.splice(0)
  }
  loading.value = false

  eventName = 'loadAfter'
  onEventName = `on${eventName[0].toUpperCase() + eventName.slice(1)}`
  if (vnode.props && onEventName in vnode.props) {
    // console.log('参數長度',vnode.props[onEventName].length);
    //兼容自定義頁面早期寫法問題
    if (vnode.props[onEventName]?.length <= 1) {
      emit(eventName, param);
    } else {
      const loadAfter = () => {
        return new Promise((resolve) => {
          emit(eventName, data.rows, resolve, data)
        })
      }
      status = await loadAfter()
    }
  }

  //if (!status) return
  // this.GetTableDictionary(data.rows)
  let rows = data.rows || []
  if (props.rowParentField) {
    rows = proxy.base.convertTree(rows, null, props.rowKey, props.rowParentField)
  }
  if (tableData.length !== rows.length) {
    isPageLoad.value = true
  }
  tableData.splice(0)
  tableData.push(...rows)
  //  this.rowData = rows
  paginations.total = data.total || 0

  return data
}
