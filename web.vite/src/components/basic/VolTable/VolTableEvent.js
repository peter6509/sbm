export const switchChange = (val, row, column) => {
  if (row[column.field] === null) {
    return
  }
  if (column.onChange) {
    column.onChange(val, row, column)
  }
}
//輸入事件
export const inputChange = (row, column, $event) => {
  column.onKeypress && column.onKeypress(row, column, $event)
  column.onKeyPress && column.onKeyPress(row, column, $event)
}
export const extraClick = (row, column, tableData) => {
  column.extra.click && column.extra.click(row, column, tableData)
}
//下拉框選擇或清除事件
export const selectChange = (row, column, value, isClear) => {
  if (isClear) {
    row[column.field] = column.edit.type == 'select' ? null : []
  }
  column.onChange && column.onChange(row, column, isClear, value)
}
export const onBlur = (row, column, $event) => {
  column.blur && column.blur(row, column, $event)
}
export const onFocus = (row, column, $event) => {
  column.focus && column.focus(row, column, $event)
}
const initReserveSelectionRows = (
  props,
  tableData,
  selection,
  reserveSelectionRows,
  isPageLoad
) => {
  if (!(props.reserveSelection && props.rowKey)) return
  //分頁或者刷新時不重置選中的數據
  if (isPageLoad.value) return
  //獲取當前頁選中的行數據並且未添加的數據
  const selectNewRows = selection.filter((c) => {
    return !reserveSelectionRows.some((x) => {
      return x[props.rowKey] === c[props.rowKey]
    })
  })
  //當前面新添加的數據
  reserveSelectionRows.push(...selectNewRows)
  //獲取當前頁移除(未選中)的數據
  let unselectIds = tableData
    .filter((x) => {
      return !selection.some((c) => {
        return x[props.rowKey] === c[props.rowKey]
      })
    })
    .map((c) => {
      return c[props.rowKey]
    })
  const selectRows = reserveSelectionRows.filter((c) => {
    return !unselectIds.some((x) => x === c[props.rowKey])
  })
  reserveSelectionRows.splice(0)
  reserveSelectionRows.push(...selectRows)
  //  console.log(this.selectRows);
}
//複選框選擇事件
export const selectionRowChange = (
  props,
  emit,
  tableData,
  selection,
  tableRef,
  reserveSelectionRows,
  isPageLoad
) => {
  initReserveSelectionRows(props,tableData, selection, reserveSelectionRows, isPageLoad)
  if (props.single) {
    if (selection.length == 1) {
      emit('rowChange', selection[0])
    }
    if (selection.length > 1) {
      tableRef.toggleRowSelection(selection[0])
    }
  } else {
    emit('rowChange', selection)
  }
  // 將selectionchange暴露出去
  emit('selectionChange', selection)
}
//樹形结構展開事件
export const expandTreeChange = (props, row, expandedRows) => {
  //	當用户對某一行展開或者關閉的時
  if (!props.defaultExpandAll && !props.lazy) {
    if (expandedRows) {
      if (props.expandRowKeys.indexOf(row[props.rowKey]) == -1) {
        props.expandRowKeys.push(row[props.rowKey] + '')
      }
      return
    }
    let _index = props.expandRowKeys.findIndex((x) => {
      return x == row[props.rowKey]
    })
    if (_index != -1) {
      props.expandRowKeys.splice(_index, 1)
    }
  }
}
