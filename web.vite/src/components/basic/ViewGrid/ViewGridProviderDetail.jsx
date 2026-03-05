//明细表有些方法需要對外開放，内部使用可能需要變更名稱，否則會出現原有代碼無法訪問方法

import { getGridTableRef, getDetailTableRef } from "./ViewGridRef.js";
import action from "./Action.js";

const checkRowsFalse = (rows) => {
  return rows === false;
};

export const initDetailOptions = (proxy, props, dataConfig) => {
  //明细表排序
  const detailOnSortEnd = (rows, newIndex, oldIndex, table) => {
    proxy.detailSortEnd.call(proxy, rows, newIndex, oldIndex, table);
    props.detailSortEnd(rows, newIndex, oldIndex, table);
  };
  const detailAddRowBefore = (table, item) => {
    const rows = [];
    let row = proxy.detailAddRowBefore.call(proxy, table, item);
    if (checkRowsFalse(row)) {
      return;
    }
    if (row) {
      if (Array.isArray(row)) {
        rows.push(...row);
      } else {
        rows.push(row);
      }
    }
    row = props.detailAddRowBefore(table, item);
    if (checkRowsFalse(row)) {
      return;
    }
    if (row) {
      if (Array.isArray(row)) {
        rows.push(...row);
      } else {
        rows.push(row);
      }
    }
    return rows;
  };
  //添加行
  const addRow = () => {
    const tableRef = proxy.getTable();
    const rows = detailAddRowBefore();
    if (!rows) {
      return;
    }
    if (!rows.length) {
      rows.push({});
    }
    tableRef.addRow(rows);
    tableRef.edit.rowIndex = -1;
    updateTableSummaryTotal(proxy, props, dataConfig);
  };
  //二级明细表添加行
  const addSecondRow = (table, item, index) => {
    const rows = detailAddRowBefore(table, item);
    if (checkRowsFalse(rows)) {
      return;
    }
    if (rows.length) {
      proxy.getTable(table).addRow(rows);
      return;
    }

    //默認默認的添加行數據
    let row = proxy.addDetailRow.call(proxy, table, item, index);
    if (!row) {
      row = props.addDetailRow(table, item, index) || {};
    }
    if (item.detail && item.detail.table) {
      row[item.detail.table] = [];
    }
    proxy.getTable(table).addRow(row);
  };
  //三级明细表添加行
  const addSubRow = (table, item, index, newRows) => {
    let rows = getCurrentDetailSelectRows(
      proxy,
      props,
      dataConfig,
      item.secondTable
    );
    if (!rows) {
      return;
    }
    const subRows = detailAddRowBefore(table, item);
    if (checkRowsFalse(subRows)) {
      return;
    }
    if (subRows.length) {
      newRows = subRows;
    }
    if (newRows) {
      if (!Array.isArray(newRows)) {
        newRows = [newRows];
      }
    } else {
      //默認默認的添加行數據
      let row = proxy.addDetailRow.call(proxy, table, item, index);
      if (!row) {
        row = props.addDetailRow(table, item, index) || {};
      }
      newRows = [row];
    }
    //给二级明细添加表數據
    if (!rows[0][table]) {
      rows[0][table] = newRows;
    } else {
      rows[0][table].push(...newRows);
    }
    //三级明细表數據指向二级明细表行數據
    proxy.getTableRef(item.table || table).rowData = rows[0][table];
  };

  //明细表删除
  const delRow = async (table, item, index, isSubDetail) => {
    //一對多明细的添加行

    let rows;
    if (isSubDetail) {
      rows = proxy.getTable(table).getSelected();
    } else if (typeof table == "string" && dataConfig.isMultiple.value) {
      rows = proxy.getTable(table).getSelected();
    } else {
      rows = proxy.getTable().getSelected();
    }

    if (!rows || rows.length == 0) {
      return proxy.$error(proxy.$ts("請選擇要删除的行!"));
    }
    if (!(await proxy.delDetailRow.call(proxy, rows, table))) {
      return;
    }
    if (!(await props.delDetailRow(rows, table))) {
      return;
    }
    if (!(await proxy.delRowBefore.call(proxy, rows, table, item, index))) {
      return;
    }
    if (!(await props.delRowBefore(rows, table, item, index))) {
      return;
    }
    let tigger = false;
    proxy
      .$confirm(proxy.$ts("確認要删除選擇的數據吗?"), proxy.$ts("警告"), {
        confirmButtonText: proxy.$ts("確定"),
        cancelButtonText: proxy.$ts("取消"),
        type: "warning",
        center: true,
      })
      .then(() => {
        if (tigger) return;
        tigger = true;
        //二、三级删除行
        if (dataConfig.isMultiple.value || isSubDetail) {
          let refDetail = proxy.getTable(table);
          refDetail.delRow();
          if (isSubDetail) {
            //這里分配二级明细表的三级表格數據后不能實現共同内存地址，問題待查
            const subRows = proxy.getTable(table).rowData;
            (proxy.getTable(item.secondTable).getSelected()[0] || {})[table] =
              subRows;
          }
          //記錄删除的明细
          rows.forEach((x) => {
            if (x.hasOwnProperty(item.key) && x[item.key]) {
              item.delKeys.push(x[item.key]);
            }
          });
          delRowAfter(proxy, props, rows, table, item, index);
          return;
        }
        rows = proxy.getTable().delRow();

        if (!delRowAfter(proxy, props, rows, table, item, index)) {
          return;
        }
        let key = dataConfig.detailOptions.key;
        //記錄删除的行數據
        rows.forEach((x) => {
          if (x.hasOwnProperty(key) && x[key]) {
            dataConfig.detailOptions.delKeys.push(x[key]);
          }
        });
        updateTableSummaryTotal(proxy, props, dataConfig, rows.length);
      });
  };

  //刷新明细表
  const refreshRow = (table) => {
    reloadDetail(table);
  };
  //刷新明细表
  const reloadDetail = (table) => {
    resetDetailTable(
      proxy,
      props,
      dataConfig,
      null,
      dataConfig.currentAction.value,
      table
    );
  };

  //一對明细表tabs點擊事件
  const detailTabsOnClick = (table) => {
    let obj = props.details.find((x) => {
      return x.table == table;
    });
    props.detailTabsClick(table);
    proxy.detailTabsClick.call(proxy, table);
    //設置三级明细表選中
    if (obj) {
      const subDetailsRef = proxy.$refs.subDetailsRef;
      if (subDetailsRef) {
        if (obj.detail) {
          subDetailsRef.setTable(obj.detail.table);
        } else {
          subDetailsRef.setTable("");
        }
      }
      return;
    }
    obj = dataConfig.subDetails.value.find((x) => {
      return x.table == table;
    });
    //設置二级明细表選中
    if (obj) {
      proxy.$refs.detailsRef.setTable(obj.secondTable);
    }
  };

  //明细表加載前方法
  const loadDetailTableBefore = async (param, callBack, table, item) => {
    //加載明细表數據之前,需要設定查詢的主表的ID
    //每次只要加載明细表格數據就重置删除明细的值
    const detailOptions = dataConfig.detailOptions;
    if (detailOptions.delKeys.length > 0) {
      detailOptions.delKeys = [];
    }
    const currentRow = dataConfig.currentRow.value;
    let key = props.table.key;
    if (currentRow && currentRow.hasOwnProperty(key)) {
      param.value = currentRow[key];
    }
    if (dataConfig.isMultiple.value) {
      if (!param.tableName) {
        param.value = currentRow[props.table.key];
        param.tableName = table;
      }
    }
    //明细查詢前
    //新建時禁止加載明细
    if (dataConfig.currentAction.value == action.ADD && !param.isCopyClick) {
      await callBack(false);
      return false;
    }
    let status = await proxy.searchDetailBefore.call(proxy, param, table, item);
    if (status) {
      status = await props.searchDetailBefore(param, table, item);
    }
    callBack(status);
    return status;
  };
  //明细查詢后
  const loadDetailTableAfter = async (data, callBack, table, item) => {
    let status = await proxy.searchDetailAfter.call(proxy, data, table, item);
    if (status) {
      status = await props.searchDetailAfter(data, table, item);
    }
    //2024.05.23增加複制數據功能后清空明细表的主鍵值
    if (dataConfig.currentAction.value == action.ADD) {
      data.forEach((row) => {
        row[props.table.key] = undefined;
        if (!item) {
          row[dataConfig.detailOptions.key] = undefined;
        } else {
          row[item.key] = undefined;
        }
      });
    }
    callBack(status);
  };
  //明细表複選框選擇事件
  const detailRowOnChange = (row, item) => {
    //獲取三级明细子表
    if (!item || !item.detail) {
      return;
    }
    proxy.detailRowChange.call(proxy, row, item);
    props.detailRowChange(row, item);
    let table = item.detail.table;
    //三级表對象
    let subRef = getDetailTableRef(proxy, props, table);
    if (!subRef) {
      return;
    }

    //三级明细表數據
    let subRows = row[table];
    if (subRows) {
      subRef.rowData = subRows;
      return;
    }
    if (!subRows) {
      subRows = [];
      row[table] = subRows;
    }
    subRef.rowData = subRows;
    //處于編輯狀態時没有明细表數據，從后台加載數據
    if (dataConfig.currentAction.value != action.ADD && !subRows.length) {
      //從接口添加數據
      subRef.load(null, true);
    }
  };
  //三级明细表加載前
  const loadSubDetailTableAfter = (rows, callBack, table, item) => {
    item.keys = [];
    //给二级明细表設置值
    let row = proxy.getTableRef(item.secondTable).getSelected()[0];
    row[item.table] = rows;
    //三级明细查詢后
    let status = props.searchSubDetailAfter(rows, table, item);
    if (!status) {
      status = proxy.searchSubDetailAfter.call(proxy, rows, table, item);
    }
    callBack(status);
  };
  //三级明细查詢前2023.09.17
  const loadSubInternalDetailTableBefore = (param, callBack, table, item) => {
    //獲取二级明细表配置
    if (item) {
      param.table = item.table;
      param.detailTable = item.secondTable;
      param.tableName = item.table;
      let detailRows = proxy.getTable(item.secondTable).getSelected();
      if (!detailRows || detailRows.length == 0) {
        callBack(false);
        return;
      }
      param.value = detailRows[0][item.secondKey];
    }
    let rows;
    if (!param.value) {
      //獲取三级表當前選中的明细行數據,並且設置二级明细表的id作為查詢條件
      let secondKey = item.secondKey;
      //獲取二级明细表選中的行
      rows = getCurrentDetailSelectRows(
        proxy,
        props,
        dataConfig,
        item.secondTable
      );
      if (!rows) {
        callBack(false);
        return;
      }
      param.value = rows[0][secondKey];
    }

    //三级明细查詢前
    //新建時禁止加載明细
    if (dataConfig.currentAction.value == action.ADD) {
      callBack(false);
      return false;
    }
    let status = props.searchSubDetailBefore(rows, table, item);
    if (!status) {
      status = proxy.searchSubDetailBefore.call(proxy, rows, table, item);
    }
    callBack(status);
  };

  //明细表行點擊事件
  const detailRowOnClick = ({ row, column, event, item }) => {
    proxy.detailRowClick.call(proxy, { row, column, event, item });
    props.detailRowClick({ row, column, event, item });
  };
  //明细表是否可以選中
  const detailOnSelectable = (row, index, item) => {
    if (!proxy.detailSelectable.call(proxy, row, index, item)) {
      return false;
    }
    return props.detailSelectable(row, index, item);
  };

  return {
    refreshRow,
    reloadDetail,
    addRow,
    addSubRow,
    delRow,
    addSecondRow,
    loadDetailTableBefore,
    loadDetailTableAfter,
    detailRowOnChange,
    detailRowOnClick,
    detailOnSelectable,
    detailOnSortEnd,
    detailTabsOnClick,
    loadSubDetailTableAfter,
    loadSubInternalDetailTableBefore,
  };
};
//三级表删除異常
//重置明细表
export const resetDetailTable = (
  proxy,
  props,
  dataConfig,
  row,
  isAdd,
  table
) => {
  //刷新指定明细表(編輯時有效)
  if (table) {
    const tableRef = getDetailTableRef(proxy, props, table);
    tableRef.reset();
    detailTableLoad(props, dataConfig, row, tableRef);
    return;
  }

  //重置三级明细
  if (dataConfig.subDetails && dataConfig.subDetails.value.length) {
    dataConfig.subDetails.value.forEach((x) => {
      x.delKeys = [];
      let tableRef = getGridTableRef(proxy, props, x.table);
      tableRef && tableRef.reset();
    });
  }
  //新建或編輯時重置明细表
  //一對多
  if (dataConfig.isMultiple.value) {
    proxy.$nextTick(() => {
      if (proxy.$refs.detailsRef) {
        props.details.forEach((item) => {
          if (item.columns) {
            let refTable = getDetailTableRef(proxy, props, item.table);
            refTable && refTable.reset();
            if (refTable && !isAdd) {
              // refTable.load();
              detailTableLoad(props, dataConfig, row, refTable, item.table);
            }
          }
        });
      }
    });
    return;
  }
  const detailOptions = dataConfig.detailOptions;

  //編輯和查看明细時重置從表數據
  if (!detailOptions.columns || detailOptions.columns.length == 0) {
    return;
  }
  //let key = table.key;
  // let query = { value: row ? row[key] : currentRow[key] };
  const isCopyClick = dataConfig.isCopyClick.value;
  proxy.$nextTick(() => {
    const detailRef = getDetailTableRef(proxy, props);
    if (detailRef) {
      detailRef.reset();
      //$refs.detail.load(query);
      detailTableLoad(props, dataConfig, row, detailRef, null, isCopyClick);
    }
  });
};

const detailTableLoad = (
  props,
  dataConfig,
  row,
  refTable,
  table,
  isCopyClick
) => {
  //一對多明细表加載數據
  if (refTable) {
    let query = {
      value: row
        ? row[props.table.key]
        : dataConfig.currentRow.value[props.table.key],
      tableName: table,
      isCopyClick: isCopyClick,
    };
    refTable.load(query);
  }
};

const updateTableSummaryTotal = (proxy, props, dataConfig, delTotal) => {
  //2021.09.25增加明细表删除、修改時重新計算行數與汇總
  //2021.12.12增加明细表判断(强制刷新合計時會用到)
  if (dataConfig.isMultiple.value) {
    props.details.forEach((c) => {
      if (!table || c.table === table) {
        let tableRef = proxy.getTable(c.table);
        tableRef.paginations.total = tableRef.rowData.length;
        //重新設置合計
        if (tableRef.summary) {
          tableRef.columns.forEach((column) => {
            if (column.summary) {
              tableRef.getInputSummaries(null, null, null, column);
            }
          });
        }
      }
    });
    return;
  }
  const detailRef = proxy.$refs.detail;
  if (!detailRef) return;
  //删除或新增行時重新設置顯示的總行數
  //第1頁后的數據删除不重置分頁總數，否則不能删除數據
  if (detailRef.paginations.page <= 1) {
    if (delTotal && detailRef.paginations.total - delTotal >= 0) {
      detailRef.paginations.total = detailRef.paginations.total - delTotal;
    } else {
      detailRef.paginations.total = detailRef.rowData.length;
    }
  }
  //重新設置合計
  if (detailRef.summary) {
    detailRef.columns.forEach((column) => {
      if (column.summary) {
        detailRef.getInputSummaries(null, null, null, column);
      }
    });
  }
};

const getCurrentDetailSelectRows = (proxy, props, dataConfig, table) => {
  let rows = proxy.getTableRef(table).getSelected();
  if (!rows.length) {
    proxy.$error(
      proxy.$ts("請選中明细表數據") +
        ":【" +
        proxy.$ts(getTableName(props, dataConfig, table)) +
        "】"
    );
    return null;
  }
  return rows;
};
//獲取二级或者三级明细表中文名稱
const getTableName = (props, dataConfig, table) => {
  //獲取明细表
  if (!props.details.length) {
    return "未配置二级或三级明细表";
  }

  let ops = props.details.find((x) => {
    return x.table == table;
  });
  if (ops) {
    return ops.cnName;
  }

  return dataConfig.subDetails.value.find((x) => {
    return x.table == table;
  }).cnName;
};

const delRowAfter = (proxy, props, rows, table, item, index) => {
  if (!proxy.delRowAfter.call(proxy, rows, table, item, index)) {
    return;
  }
  if (!props.delRowAfter(rows, table, item, index)) {
    return;
  }
  return true;
};
