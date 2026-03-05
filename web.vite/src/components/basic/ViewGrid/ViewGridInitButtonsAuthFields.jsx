import { getUrl, resetSearch } from "./ViewGridProvider.jsx";
import action from "./Action.js";
import apiButtons from "@/api/buttons.js";
export const getButtons = (proxy, props, ctx, dataConfig) => {
  //生成ViewGrid界面的操作按鈕及更多選項
  const buttons = dataConfig.buttons.value;
  let searchIndex = buttons.findIndex((x) => {
    return x.value === "Search";
  });
  //添加高级查詢
  let hasOneFormItem =
    props.searchFormOptions.length === 1 &&
    props.searchFormOptions[0].length === 1;
  if (searchIndex !== -1 && !hasOneFormItem && !proxy.$global.fixedSearch) {
    buttons.splice(searchIndex + 1, 0, {
      icon: dataConfig.fixedSearchForm.value
        ? "el-icon-refresh-left"
        : "el-icon-search",
      name: dataConfig.fixedSearchForm.value ? "重置" : "高级查詢",
      plain: true,
      value: dataConfig.fixedSearchForm.value ? "" : "advanced",
      type: buttons[searchIndex].type,
      onClick: () => {
        if (dataConfig.fixedSearchForm.value) {
          return resetSearch(proxy, props);
        }
        //顯示、隐藏高级查詢
        dataConfig.searchBoxShow.value = !dataConfig.searchBoxShow.value;
      },
    });
  }
  if (props.searchFormOptions.length === 1) {
    const option = props.searchFormOptions[0];
    if (
      option.length == 1 &&
      option[0].type != "radio" &&
      option[0].type != "checkbox"
    ) {
      option[0].itemStyle = "width:200px";
    }
  }
  if (hasOneFormItem) {
    dataConfig.fixedSearchForm.value = false;
  }
  dataConfig.maxBtnLength.value += searchIndex === -1 ? 0 : 1;
};

export const initButtonsAuthFields = (
  proxy,
  props,
  dataConfig,
  $route,
  hiddenFields
) => {
  initAuthFields(proxy, props, hiddenFields);
  const buttons = dataConfig.buttons.value;
  // const boxButtons=dataConfig.boxButtons;
  const upload = dataConfig.upload;
  const detailOptions = dataConfig.detailOptions;
  const extend = props.extend || {};
  const details = props.details;
  //初始化ViewGird與彈出框/明细表按鈕
  //通過菜單獲取用户所對應菜單需要顯示的按鈕
  let permissionButtons;
  //預覽
  if (props.priview) {
    permissionButtons = apiButtons.filter((x) => {
      return ["Search", "Add", "Update", "Export","Delete"].includes(x.value);
    });
  } else {
    permissionButtons = proxy.base.getButtons(
      $route.path,
      null,
      extend.tableAction,
      props.table.name
    );
  }

  //console.log(permissionButtons);
  if (permissionButtons) {
    //2020.03.31添加深拷贝按鈕组
    permissionButtons.forEach((p) => {
      let _obj = {};
      for (const key in p) {
        _obj[key] = p[key];
      }
      buttons.push(_obj);
    });
    // this.buttons.push(...permissionButtons);
  }
  if (!extend.buttons) {
    extend.buttons = {};
  }
  //查詢界面擴展按鈕(擴展按鈕可自行通過設置按鈕的Index屬性顯示到具體位置)
  if (extend.buttons.view) {
    extendBtn(buttons, extend.buttons.view);
  }

  //彈出框按鈕
  let boxButtons = [];

  let saveBtn = buttons.some((x) => {
    if (
      x.value &&
      (x.value.toLowerCase() === action.ADD.toLowerCase() ||
        x.value.toLowerCase() === action.EDIT.toLowerCase())
    )
      return true;
  });
  dataConfig.currentReadonly.value = !saveBtn;
  //主從明细表格操作按鈕
  let detailGridButtons = {
    name: "刷新",
    type: "info",
    value: "refresh",
    icon: "el-icon-refresh",
    onClick() {
      //如果明细表當前的狀態為新建時，禁止刷新
      if (dataConfig.currentAction.value === action.ADD) {
        return;
      }
      proxy.refreshRow();
    },
  };

  if (details) {
    dataConfig.isMultiple.value = details.length > 0;
    if (dataConfig.isMultiple.value) {
      details.forEach((item) => {
        item.paginationHide = false;
        if (item.columnIndex === undefined) {
          item.columnIndex = false;
          item.columns.forEach((col) => {
            if (col.link) {
              col.link = false;
            }
          });
        }
      });
    }
  }

  let importExcel = buttons.some((x) => {
    if (x.value === action.IMPORT) return true;
  });
  //如果有導入權限,則需要初始化導入组件
  if (importExcel) {
    initImportOptions(proxy, props, dataConfig);
  }

  // disabled
  //如果當前角色没有編輯或新建功能，查看明细時字段設置全部只讀
  //只有明细表，將明细表也設置為不可能編輯，並且不顯示添加行、删除行
  if (!saveBtn) {
    props.editFormOptions.forEach((row) => {
      row.forEach((x) => {
        x.readonly = true;
      });
    });
    //没有新增編輯權限的，彈出框都設置為只讀
    if (props.detail.columns) {
      props.detail.columns.forEach((column) => {
        if (column.hasOwnProperty("edit")) {
          column.readonly = true;
        }
      });
    }
    //彈出框擴展按鈕
    extendBtn(boxButtons, extend.buttons.box);

    //彈出彈框按鈕(2020.04.21),没有編輯或新建權限時，也可以通過buttons屬性添加自定義彈出框按鈕
    boxButtons.push(...boxButtons);
    detailOptions.buttons.push(detailGridButtons);
    detailOptions.buttons.forEach((button) => {
      if (!button.hasOwnProperty("hidden")) {
        button.hidden = false;
      }
    });

    //彈出框擴展明细表按鈕
    extendBtn(detailOptions.buttons, extend.buttons.detail);
    initMultipleTables(proxy, props, dataConfig, false);
    return boxButtons;
  }

  detailOptions.edit = true;
  boxButtons.push(
    ...[
      {
        name: "保存",
        icon: "el-icon-check",
        type: "danger",
        disabled: false,
        value: "save",
        onClick() {
          proxy.save();
        },
      },
    ]
  );

  //從表表格操作按鈕,待完
  detailOptions.buttons.push(
    ...[
      {
        name: "添加行",
        icon: "el-icon-plus",
        type: "primary",
        hidden: false,
        plain: true,
        onClick() {
          proxy.addRow();
        },
      },
      {
        type: "danger",
        plain: true,
        name: "删除行",
        hidden: false,
        icon: "el-icon-delete",
        onClick() {
          proxy.delRow();
        },
      },
      //2022.01.08增加明细表導入導出功能
      //注意需要重寫后台明细表接口的導入與下載模板、導出的權限,Sys_DictionaryListController.cs/SellOrderListController.cs
      {
        type: "danger",
        plain: true,
        name: "導入",
        value: "import",
        hidden: false,
        icon: "el-icon-upload2",
        onClick() {
          upload.url = `${proxy.http.ipAddress}api/${props.detail.table}/${action.IMPORT}?table=1`;
          upload.template.url = `${proxy.http.ipAddress}api/${props.detail.table}/${action.DOWNLOADTEMPLATE}`;
          //定義下載模板的文件名
          upload.template.fileName = props.detail.cnName;
          upload.excel = true;
        },
      },
      {
        type: "danger",
        plain: true,
        name: "導出",
        value: "export",
        icon: "el-icon-download",
        hidden: false,
        onClick() {
          proxy.export(true);
        },
      },
    ]
  );
  detailOptions.buttons.forEach((button) => {
    if (button.hasOwnProperty("hidden")) {
      button.hidden = false;
    }
  });
  //彈出框擴展按鈕
  extendBtn(boxButtons, extend.buttons.box);

  //彈出框擴展明细表按鈕
  detailOptions.buttons.push(detailGridButtons);
  extendBtn(detailOptions.buttons, extend.buttons.detail);
  //彈出彈框按鈕
  dataConfig.boxButtons.value.push(...boxButtons);

  initMultipleTables(proxy, props, dataConfig, true);
};

const initAuthFields = (proxy, props, hiddenFields) => {
  let arr = props.table.name.split("/");
  let tableName = (
    (props.extend.tableAction || arr[arr.length - 1]) + ""
  ).toLowerCase();
  let _auth = (proxy.base.getItem("authFields") || []).find((c) => {
    return (c.name + "").toLowerCase() === tableName;
  });
  initDetailAuthFields(proxy, props);
  if (!_auth || !_auth.fields.length) {
    return;
  }
  if (!hiddenFields.value) {
    hiddenFields.value = [];
  }
  props.columns.forEach((col) => {
    if (!col.hidden) {
      col.hidden = _auth.fields.indexOf(col.field) === -1;
    }
  });

  props.editFormOptions.forEach((options) => {
    options.forEach((op) => {
      op.hidden = _auth.fields.indexOf(op.field) === -1;
      if (op.hidden) {
        hiddenFields.value.push(op.field);
      }
    });
  });

  props.searchFormOptions.forEach((options) => {
    options.forEach((op) => {
      op.hidden = _auth.fields.indexOf(op.field) === -1;
    });
  });
};
const initMultipleTables = (proxy, props, dataConfig, hasBtn) => {
  // console.log('initMultipleTables')
  if (dataConfig.isMultiple.value) {
    initMultipleButtons(proxy, props, dataConfig, hasBtn);
    initSubDetailButtons(proxy, props, dataConfig, hasBtn);
  }
};

const initMultipleButtons = (proxy, props, dataConfig, hasBtn) => {
  //明细操作待完
  props.details.forEach((item) => {
    if (item.columns) {
      item.pagination = { total: 0, size: 100, sortName: item.sortName };
      if (hasBtn) {
        item.buttons.push(
          {
            name: "添加行",
            icon: "el-icon-plus",
            type: "primary",
            hidden: false,
            plain: true,
            onClick: (table, item, index) => {
              proxy.addSecondRow(table, item, index);
            },
          },
          {
            type: "danger",
            plain: true,
            name: "删除行",
            hidden: false,
            icon: "el-icon-delete",
            onClick: (table, item, index) => {
              proxy.delRow(table, item, index);
            },
          }
        );
      } else {
        item.columns.forEach((x) => {
          x.readonly = true;
        });
      }
    }
  });
};

const initSubDetailButtons = (proxy, props, dataConfig, hasBtn) => {
  props.details.forEach((item) => {
    if (item.detail) {
      dataConfig.detailOptions.height = 200;
      item.detail.height = 200;
      dataConfig.detailHeight.value = 220;
      item.single = true;
      item.detail.columnIndex = false;
      dataConfig.subDetails.value.push(item.detail);
    }
  });

  dataConfig.subDetails.value.forEach((item) => {
    if (item.columns) {
      item.paginationHide = false;
      item.pagination = { total: 0, size: 100, sortName: "" };
      if (hasBtn) {
        item.buttons.push(
          {
            name: "添加行",
            icon: "el-icon-plus",
            type: "primary",
            hidden: false,
            plain: true,
            onClick: (table, item, index) => {
              proxy.addSubRow(table, item, index);
            },
          },
          {
            type: "danger",
            plain: true,
            name: "删除行",
            hidden: false,
            icon: "el-icon-delete",
            onClick: (table, item, index) => {
              proxy.delRow(table, item, index, true);
            },
          }
        );
      } else {
        item.columns.forEach((x) => {
          x.readonly = true;
        });
      }
    }
  });
};

const extendBtn = (btns, source) => {
  //btns權限按鈕，source為擴展按鈕
  if (!btns || !(source && source instanceof Array)) {
    return;
  }
  //source通過在表的擴展js文件中buttons對應按鈕的屬性index决定按鈕所放位置
  source.forEach((x) => {
    //通過按鈕的Index屬性，放到指定的位置
    btns.splice(x.index === undefined ? btns.length : x.index, 0, x);
  });
};

const initDetailAuthFields = (proxy, props) => {
  const detail = props.detail;
  try {
    if (!detail.columns || !detail.columns.length) {
      return;
    }
    let _auth = (proxy.base.getItem("authFields") || []).find((c) => {
      return (c.name + "").toLowerCase() === (detail.table + "").toLowerCase();
    });
    if (!_auth || !_auth.fields.length) {
      return;
    }
    detail.columns.forEach((col) => {
      if (!col.hidden) {
        col.hidden = _auth.fields.indexOf(col.field) === -1;
      }
    });
  } catch (error) {
    console.log(error);
  }
};

//上傳参數
export const initImportOptions = (proxy, props, dataConfig) => {
  const upload = dataConfig.upload;
  upload.url = getUrl(action.IMPORT, null, props.table);
  //定義下載模板的文件名
  upload.template.fileName = props.table.cnName;
  //定義下載模板的Url路径
  upload.template.url =
    proxy.http.ipAddress + getUrl(action.DOWNLOADTEMPLATE, true, props.table);
};
