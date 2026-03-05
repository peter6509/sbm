import { getGridTableRef } from './ViewGridRef.js'
import { getUrl } from './ViewGridProvider.jsx'
import action from './Action.js'
import { checkFormReadonly } from '@/uitils/formEditAuditCheck.js'
export const ViewGridAuditConfig = (proxy, props, ctx, dataConfig) => {
  return {
    signAfter() {
      //加簽后方法
      proxy.search()
    },
    initAuditColumn: (init) => {
      if (!dataConfig.showTableAudit.value) {
        return
      }
      if (!init && (!proxy.$global.table || !proxy.$global.table.showAudit)) {
        return
      }
      const b = props.columns.some((x) => {
        return x.field && x.field.toLowerCase() === 'auditstatus'
      })
      if (
        !b &&
        !dataConfig.buttons.value.some((x) => {
          return x.value == 'Audit'
        })
      )
        return

      props.columns.push({
        title: '流程', //按鈕名稱
        field: 'audit_view',
        fixed: 'right',
        width: 90,
        align: 'center',
        render: (h, { row, column, index }) => {
          return (
            <div>
              <el-button
                onClick={($e) => {
                  $e.stopPropagation()
                  auditTabelOnClick(proxy, props, dataConfig, row)
                }}
                text
                size="small"
                type="primary"
                plain
              >
                {proxy.$ts('查看流程')}
                {/* icon={hasAduit?'Check':'Document'} */}
                {/* { $ts(hasAduit? '流程審批' : '>.Net8')} */}
              </el-button>
            </div>
          )
        }
      })
    }
  }
}
export const showAudit = async (proxy, props, dataConfig, rows, isAnti, view) => {
  if (rows) {
    if (!Array.isArray(rows)) {
      rows = [rows]
    }
  } else {
    //審核彈出框
    rows = getGridTableRef(proxy, props, true).getSelected()
  }
  if (rows.length === 0) return proxy.$message.error(proxy.$ts('請選擇要審核的行!'))
  console.log(rows[0])
  let auditStatus = Object.keys(rows[0]).find((x) => {
    return x.toLowerCase() === 'auditstatus'
  })

  if (!auditStatus) {
    return proxy.$message.error(proxy.$ts(`表必須包括審核字段【AuditStatus】,並且是int類型`))
  }

  if (!(await proxy.auditModelOpenBefore.call(proxy, rows, isAnti, view))) {
    return
  }
  if (!(await props.auditModelOpenBefore(rows, isAnti, view))) {
    return
  }

  if (isAnti) {
    if (rows.length > 1) {
      return proxy.$message.error(proxy.$ts('只能選擇一行數據反審!'))
    }
    let status = rows[0][auditStatus]
    if (!status || status === 90 || status === 100) {
      return proxy.$message.error(proxy.$ts('只能選擇已審核數據!'))
    }
  }
  const auditTable =
    props.getAuditTable(rows, isAnti, view) || proxy.getAuditTable.call(proxy, rows, isAnti, view)
  dataConfig.auditInited.value = true
  await proxy.$nextTick()
  await proxy.$nextTick()
  proxy.$nextTick(() => {
    if (proxy.$refs.auditRef) {
      proxy.$refs.auditRef.open(rows, null, auditTable, isAnti, view)
      return
    }
    setTimeout(() => {
      proxy.$refs.auditRef.open(rows, null, auditTable, isAnti, view)
    }, 300)
  })
}

export const saveAuditClick = async (proxy, props, params, rows, attachFile, callback) => {
  //保存審核
  let keys = rows.map((x) => {
    return x[props.table.key]
  })
  if (!(await props.auditBefore(keys, rows))) {
    return
  }
  if (!(await proxy.auditBefore.call(proxy, keys, rows))) {
    return
  }
  let url = `${getUrl(action.AUDIT, null, props.table)}?auditReason=${params.reason}&auditStatus=${
    params.value
  }`
  if (attachFile) {
    url = url + '&attach=' + attachFile
  }
  proxy.http.post(url, keys, 'loading....').then(async (x) => {
    if (!(await props.auditAfter(x, rows))) {
      return
    }
    if (!(await proxy.auditAfter.call(proxy, x, rows))) {
      return
    }
    if (!x.status) return proxy.$error(x.message)
    callback && callback(x)
    proxy.$success(x.message)
    proxy.refresh()
  })
}

export const auditTabelOnClick = (proxy, props, dataConfig, row) => {
  if (!proxy.$global.table || !proxy.$global.table.boxAudit) {
    //待完
    showAudit(proxy, props, dataConfig, [row], false, true)
    return
  }
  dataConfig.isAuditClick.value = true
  const auditBtn = dataConfig.boxButtons.value.find((x) => {
    return x.value === 'Audit'
  })
  if (!auditBtn) {
    dataConfig.boxButtons.value.push({
      name: proxy.$ts('審批'),
      value: 'Audit',
      type: 'primary',
      hidden: false,
      icon: 'el-icon-check',
      plain: true,
      onClick: async () => {
        const currentRow = dataConfig.currentRow.value
        if ((await proxy.boxAuditOptionOpenBefore.call(proxy, currentRow)) === false) {
          return
        }
        if ((await props.boxAuditOptionOpenBefore(currentRow)) === false) {
          return
        }
        proxy.audit([currentRow], false, false)
      }
    })
  } else {
    auditBtn.hidden = false
  }
  proxy.edit(row)
}

//記錄默認只讀字段

export const initDefaultFormReadonlyFields = (proxy, props, dataConfig) => {
  const b = props.columns.some((x) => {
    return x.field == proxy.$global.auditStausField || 'AuditStatus'
  })
  if (!b) {
    return
  }
  props.editFormOptions.forEach((ops) => {
    ops.forEach((x) => {
      if (!x.readonly && !x.disabled) {
        dataConfig.defaultFormReadonlyFields.value.push(x.field)
      }
    })
  })
}

export const setFormAuditReadonly = (tableInfo, editFormFields, editFormOptions, dataConfig) => {
  if (!dataConfig.defaultFormReadonlyFields.value.length) {
    return
  }

  const b =
    dataConfig.currentAction.value == 'Add' || dataConfig.isCopyClick.value
      ? false
      : checkFormReadonly(dataConfig.currentRow.value, tableInfo, editFormFields, editFormOptions)
  editFormOptions.forEach((ops) => {
    ops.forEach((x) => {
      if (dataConfig.defaultFormReadonlyFields.value.includes(x.field)) {
        x.readonly = b
      }
    })
  })
  dataConfig.boxButtons.value.forEach((x) => {
    if (x.value == 'save') {
      x.hidden = b
    }
  })
  dataConfig.detailOptions.buttons.forEach((x) => {
    x.hidden = b
  })
}
