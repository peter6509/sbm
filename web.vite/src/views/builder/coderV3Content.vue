<template>
  <div class="coder-content">
    <div class="coder-content-header">
      <div class="fx-1">
        <vol-title icon="" title="代碼生成配置">
          <!-- <span class="desc">支持拖拽配置、實時預覽、動態生成;</span> -->
        </vol-title>
      </div>

      <div class="corder-content-btns">
        <el-popover title="操作说明" :width="420">
          <template #reference>
            <span style="color: #0247de; font-size: 13px;" class="el-icon-info">操作说明</span>
          </template>
          <div style="line-height:2;">
            <div>1.點擊新建，填寫數據表信息</div>
            <div>2.配置表结構信息</div>
            <div>3.配置查詢表單</div>
            <div>4.配置編輯表單</div>
            <div>支持批量生成：可一鍵生成100張表全部后台代碼</div>
          </div>
        </el-popover>

        <el-button :type="btn.type" :color="btn.color" v-for="(btn, index) in buttons" :key="index"
          @click="btnClick(btn)">
          <el-icon>
            <component :is="btn.icon" />
          </el-icon>
          {{ $ts(btn.name) }}
        </el-button>

      </div>
    </div>
    <div class="coder-tabs">
      <el-tabs v-model="activeName" @tab-click="tabsClick">
        <el-tab-pane v-for="(item, index) in tabs" :key="index" :label="item.label" :name="item.name"></el-tab-pane>
      </el-tabs>
      <div class="desc">
        <div class="ck">
          <el-checkbox :true-value="1" :false-value="0" v-model="formFields.dyPage">動態渲染頁面
            <el-popover title="" :width="380">
              <template #reference>
                <span style="color: #0247de; font-size: 13px;" class="el-icon-info"></span>
              </template>
              <div style="line-height: 2;">
                1. 動態渲染頁面：前端不需要發布、編譯。
                <div>2. 支持代碼生成器寫js脚本處理基礎業務。</div>
                <div>3. 點擊保存后頁面實時生效。</div>
              </div>
            </el-popover>
          </el-checkbox>

        </div>
        <div class="ck">
          <el-checkbox :true-value="1" :false-value="0" v-model="formFields.fixedSearch">顯示所有查詢條件</el-checkbox>
        </div>
        <span class="el-icon-info">支持拖拽配置、實時預覽、動態加載</span>
      </div>
    </div>

    <div class="coder-content-el">
      <div v-show="activeName == 'info'">
        <coderV3TableInfo :formOptions="formOptions" :formFields="formFields" :tableData="tableData" :columns="columns"
          :syncClick="nodeClick" ref="tableInfoRef">
        </coderV3TableInfo>
      </div>
      <div v-show="activeName == 'search'">
        <coderV3Form current-action="search" :height="height" ref="formRef">
        </coderV3Form>
      </div>
      <div v-show="activeName == 'edit'">
        <coderV3Form current-action="edit" :height="height" ref="editRef">
        </coderV3Form>
      </div>
      <div v-show="activeName == 'app'">
        <codeV3App :height="height" ref="appRef">
        </codeV3App>
      </div>

      <div v-show="activeName == 'scriptText'">
        <codeV3Script :height="height" ref="scriptRef">
        </codeV3Script>
      </div>

    </div>
  </div>
  <vol-box ref="add" :width="650" title="代碼生成" :padding="0" v-model="addModel">
    <div style="padding: 0">
      <el-alert title="如果只是創建目錄，父级id填0,其他随便填寫;" class="alert-primary" :closable="false"></el-alert>
    </div>
    <div class="add-model">
      <vol-form ref="addFormRef" label-position="left" :formRules="addOptions" :formFields="addFields">
      </vol-form>
    </div>
    <template #footer>
      <div>
        <el-button type="primary" size="small" @click="addClick"><i class="el-icon-plus"></i> 確定</el-button>
      </div>
    </template>
  </vol-box>
</template>
<script lang="jsx" setup>
import { ref, getCurrentInstance, computed, defineAsyncComponent } from "vue";
import coderV3TableInfo from "./coderV3TableInfo.vue";
const coderV3Form = defineAsyncComponent(() => import("./coderV3Form.vue"));
const codeV3Script = defineAsyncComponent(() => import("./codeV3Script.vue"));
const codeV3App = defineAsyncComponent(() => import("./coderV3App.vue"));
const scriptRef = ref();

const { proxy } = getCurrentInstance();

const props = defineProps({
  parentData: {
    type: Array,
    default: () => {
      return [];
    },
  },
});
const emit = defineEmits(["loadTableAfter", "loadTableInfo"]);
const activeName = ref("info");// ref("search");
const tabs = ref([
  { name: "info", label: "基礎信息" },
  { name: "search", label: "查詢表單" },
  { name: "edit", label: "編輯表單" },
  { name: "scriptText", label: "動態脚本" },
  { name: "app", label: "移動端(app/小程序)" },
]);

const addModel = ref(false);

import { tableOptions } from "./coderV3Options.jsx";
const { formFields, formOptions, columns, addOptions, addFields } = tableOptions();

const dicFields = ref([]);

const initDicFields = () => {
  dicFields.value = tableData.value.map((x) => {
    return {
      key: x.columnName,
      value: x.columnCnName ? "(" + x.columnCnName + ")" + x.columnName : x.columnName, // x.columnCnName,
    };
  });
  dicFields.value.unshift({ key: "", value: "請選擇" });
};

formOptions.value.forEach((x) => {
  x.forEach((c) => {
    if (c.field == "expressField") {
      // formFields.expressField
      c.type = "select";
      c.data = dicFields;
    } else if (c.field == "sortName") {
      c.type = "select";
      c.data = dicFields;
    }
  });
});


const initDic = async () => {
  proxy.http.post("api/Sys_Dictionary/GetBuilderDictionary", {}, true).then((dic) => {
    let column = columns.value.find((x) => {
      return x.field == "dropNo";
    });
    if (!column) return;

    column.bind.data = dic.map((x) => {
      return { key: x, value: x };
    });
    column.bind.data.unshift({ key: "", value: "請選擇" });
    setTimeout(() => {
      proxy.base.setItem('coderDicData', column.bind.data)
    }, 1000);
    // console.log('dic',proxy.base.getItem('coderDicData'))
  });
}
initDic();
proxy.base.setItem('coder:initDic', initDic)

formFields.value.vuePath = localStorage.getItem("vuePath");
formFields.value.appPath = localStorage.getItem("appPath");

const treeOrginData = ref([]);
const addFormRef = ref();
const tableInfoRef = ref();
const formRef = ref()
const editRef = ref();

const getCurrentFormOptions = () => {
  return activeName.value == 'search' ? searchFormOptions.value : editFormOptions.value
}


const initDraggableFields = (init) => {
  if (activeName.value == 'search' || init) {
    formRef.value.initDraggableFields(fieldOptions.value, searchFormOptions.value, activeName.value);
  }
  if (activeName.value == 'edit' || init) {
    editRef.value.initDraggableFields(fieldOptions.value, editFormOptions.value, activeName.value);
  }
}

const tabsClick = (item) => {
  activeName.value = tabs.value[item.index].name;
  // initDraggableFields();

};

const fieldOptions = ref([]);
const searchFormOptions = ref([])
const editFormOptions = ref([])


const sortForm = (a, b) => {
  // 先按rowNo排序
  if (a.rowNo !== b.rowNo) {
    return a.rowNo - b.rowNo;
  }

  // rowNo相同則按colNo排序
  // 處理null值，將null视為较大值排在后面
  const colA = a.colNo !== null ? a.colNo : Infinity;
  const colB = b.colNo !== null ? b.colNo : Infinity;

  return colA - colB;
}

const initFieldOptions = () => {
  fieldOptions.value = tableData.value.map((x) => {
    return {
      title: x.columnCnName || x.columnName, //名字
      field: x.columnName, //字段
      formType: x.columnType == "DateTime" || x.columnType == "datetime" ? 'date' : null, //編輯類型
      // searchRowNo: x.searchRowNo, //查詢行
      // searchColNo: x.searchColNo || x.dropNo, //查詢列
      // searchType: x.columnType == "DateTime" && !x.searchType ? 'date' : (x.searchType || ''), //查詢類型
      // searchDicNo: x.searchDicNo || '',
      // searchColSize:x.searchColSize,
      // editType: x.columnType == "DateTime" && !x.editType ? 'date' : (x.editType || ''), //編輯類型
      // editRowNo: x.editRowNo, //編輯行 
      // editColNo: x.editColNo, //編輯列
      // dropNo: x.dropNo || x.searchColNo, //數據源
      isNull: x.isNull,
      isReadDataset: x.isReadDataset, //是否只讀
      // colSize: x.colSize,
      // columnType: x.columnType
    };
  });

  const initWidth = (data) => {
    const rowNoCount = {};
    data.forEach(item => {
      const rowNo = item.rowNo;
      if (rowNoCount[rowNo]) {
        rowNoCount[rowNo]++;
      } else {
        rowNoCount[rowNo] = 1;
      }
    });

    console.log(Object.values(rowNoCount))
    const count = Math.max(...Object.values(rowNoCount)) || 1;

    data.forEach(item => {

      item.width = (100 / count).toFixed(3) * 1.0
    });
  }

  searchFormOptions.value = tableData.value.filter(x => { return x.searchRowNo > 0 }).map((x) => {
    return {
      title: x.columnCnName || x.columnName, //名字
      field: x.columnName, //字段
      rowNo: x.searchRowNo, //查詢行
      colNo: x.searchColNo, //查詢列
      formType: x.columnType == "DateTime" && !x.searchType ? 'date' : (x.searchType || ''), //查詢類型
      dropNo: x.searchDropNo || x.dropNo,//數據源
      // colSize: x.searchColSize//查詢宽度
      width: x.searchColSize
    };
  }).sort(sortForm);

  // sortForm(searchFormOptions.value);
  //這里還有加上默認編輯行排序
  editFormOptions.value = tableData.value.filter(x => { return x.editRowNo > 0 }).map((x) => {
    return {
      title: x.columnCnName || x.columnName, //名字
      field: x.columnName, //字段
      rowNo: x.editRowNo, //查詢行
      colNo: x.editColNo, //查詢列
      formType: x.columnType == "DateTime" && !x.editType ? 'date' : (x.editType || ''), //編輯類型
      dropNo: x.dropNo || x.searchDropNo,//數據源
      //  colSize: x.colSize,//編輯宽度
      width: x.colSize == 12 ? 100 : x.colSize,
      isReadDataset: x.isReadDataset, //是否只讀
      isNull: x.isNull,
    };
  }).sort(sortForm);

  //重新設置宽度，兼容舊的代碼生成器
  let haswd = editFormOptions.value.some(x => { return x.width })
  //没有設置過宽度的
  if (!haswd) {
    initWidth(editFormOptions.value);
  }
  haswd = searchFormOptions.value.some(x => { return x.width })
  //没有設置過宽度的
  console.log(searchFormOptions.value)
  if (!haswd) {
    initWidth(searchFormOptions.value);
  }

  console.log(editFormOptions.value)
  //  sortForm(editFormOptions.value);
  //  formRef.value.initDraggableFields(fieldOptions.value, getCurrentFormOptions());
  initDraggableFields(true);
};

const height = ref(0);
height.value = document.body.clientHeight - 117;
const tableData = ref([]);
const tableHeight = ref(0);
tableHeight.value = document.body.clientHeight - 443;
if (tableHeight.value <= 260) {
  tableHeight.value = 260;
}

const nodeClick = (id) => {
  const url = "/api/builder/LoadTableInfo?table_Id=" + id + "&isTreeLoad=true";
  // localStorage.setItem("vuePath", proxy.layOutOptins.fields.vuePath || "");
  proxy.http.post(url, {}, true).then((x) => {
    x.data = x.data || {};
    if (!x.data.tableTrueName) {
      x.data.tableTrueName = x.data.tableName;
    }
    const _fields = ["sortable", "isNull", "isReadDataset", "isColumnData", "isDisplay"];
    x.data.tableColumns.forEach((item) => {
      for (let index = 0; index < _fields.length; index++) {
        item[_fields[index]] = item[_fields[index]] || 0;
      }
    });

    tableData.value = x.data.tableColumns;
    initDicFields();
    initFieldOptions();
    x.data.vuePath = localStorage.getItem("vuePath");
    x.data.appPath = localStorage.getItem("appPath");
    proxy.base.resetForm(formFields, formOptions, x.data);
    formFields.value.dyPage = (x.data.dyPage || 0);
    formFields.value.fixedSearch = (x.data.fixedSearch || 0);

  });
};

// nodeClick(25);

const btnClick = (btn) => {
  btn?.click();
};

const addClick = async () => {
  const b = await addFormRef.value.validate();
  if (!b) {
    return;
  }
  if (!addFields.value.dbServer) {
    return proxy.$message.error("請選擇數據庫");
  }
  if (addFields.value.tableTrueName) {
    addFields.value.tableTrueName = addFields.value.tableName;
  }

  let parentId = addFields.value.parentId;
  if (Array.isArray(parentId)) {
    parentId = parentId.pop();
  }

  let queryParam =
    "parentId=" +
    parentId +
    "&tableName=" +
    addFields.value.tableName +
    "&columnCNName=" +
    addFields.value.columnCNName +
    "&nameSpace=" +
    addFields.value.namespace +
    "&foldername=" +
    addFields.value.folderName +
    "&isTreeLoad=false&dbServer=" +
    addFields.value.dbServer;
  const url = "/api/builder/loadTableInfo?" + queryParam;
  proxy.http.post(url, {}, true).then((x) => {
    if (!x.status) {
      proxy.$message.error(x.message);
      return;
    }
    x.data = x.data || {};
    addModel.value = false;
    emit("loadTableAfter", {
      id: x.data.table_Id,
      pId: x.data.parentId,
      parentId: x.data.parentId,
      name: x.data.columnCNName,
      orderNo: x.data.orderNo,
    });
    // }
    if (!x.data.tableTrueName) {
      x.data.tableTrueName = x.data.tableName;
    }
    x.data.vuePath = localStorage.getItem("vuePath");
    x.data.appPath = localStorage.getItem("appPath");
    proxy.base.resetForm(formFields, formOptions, x.data);
    tableData.value = x.data.tableColumns;
    initFieldOptions();
    initDicFields();
  });
};

const validateTableInfo = async (callback) => {
  //console.log("validateTableInfo");

  const b = await tableInfoRef.value.validate(); //await formRef.value.validate(); //
  if (!b) {
    return b;
  }
  if (tableData.value.length) {
    // proxy.$message.error({ message: "請先加載數據", offset: 100, duration: 2000 });
    // return false;
    let keyInfo = tableData.value.find((x) => {
      return x.isKey;
    });
    if (!keyInfo) {
      proxy.$message.error({ message: "請勾選設置主鍵", offset: 100, duration: 2000 });
      return false;
    }
    if (keyInfo.isNull == 1) {

      proxy.$message.error({ message: "主鍵【可為空】必須設置為否", offset: 100, duration: 2000 });
      return false;
    }
    if (
      keyInfo.columnType != "int" &&
      keyInfo.columnType != "bigint" &&
      !formFields.value.sortName
    ) {
      proxy.$message.error({ message: "主鍵非自增類型,請選擇上面表單【默認排序字段】", offset: 100, duration: 2000 });
      return false;
    }
  }


  return true;
};



const save = async () => {
  const vuePath = formFields.value.vuePath || "";
  localStorage.setItem("vuePath", vuePath);
  localStorage.setItem("appPath", formFields.value.appPath || "");
  if (!vuePath.endsWith("\\views") && !vuePath.endsWith("/views")) {
    return proxy.$message.error(
      { message: "Vue路径只能填寫到前端項目views目錄,如E:\\xxx\\web.vite\\scr\\views", offset: 100, duration: 2000 }
    );
  }

  if (
    columns.value.filter((x) => {
      return x.isKey == 1;
    }).length > 1
  ) {
    return proxy.$message.error({ message: '表结構只能勾選一個主鍵字段', offset: 100, duration: 2000 });
  }
  if (!(await validateTableInfo())) {
    return;
  }

  const param = getParam();

  proxy.http.post("/api/builder/Save", param, true).then((x) => {
    if (!x.status) {
      proxy.$message.error({ message: x.message, offset: 100, duration: 2000 });
      return;
    }
    proxy.$message.primary({ message: x.message, offset: 100, duration: 2000 });
  });
};


const convertFormColRow = (formOptions) => {
  // const rowOptions = [];
  let rowWidth = 0;
  let rowIndex = 1;
  let colIndex = -4;
  formOptions.forEach((item) => {
    if (!item.width) {
      item.width = 25;
    }
    rowWidth += item.width;
    if (rowWidth <= 100) {
      //設置查詢、編輯列
      colIndex = colIndex + 5;
    } else {
      //設置查詢、編輯行
      rowIndex = rowIndex + 5;
      //設置查詢、編輯列
      colIndex = 1;
      rowWidth = item.width;
    }
    item.colNo = colIndex;
    item.rowNo = rowIndex;
  })
}

const getParam = () => {
  const param = JSON.parse(JSON.stringify(formFields.value))
  if (param.parentId?.length) {
    param.parentId = param.parentId.pop();
  }
  if (!param.parentId) {
    param.parentId = 0;
  }

  searchFormOptions.value = formRef.value.getFormOptions()
  editFormOptions.value = editRef.value.getFormOptions();
  convertFormColRow(searchFormOptions.value)
  convertFormColRow(editFormOptions.value)
  //   convertFormColRow(formRef.value.getFormOptions())
  // convertFormColRow(editRef.value.getFormOptions())
  param.TableColumns = tableData.value;
  // title: x.columnCnName || x.columnName, //名字
  //     field: x.columnName, //字段
  //     rowNo: x.searchRowNo, //查詢行
  //     colNo: x.searchColNo, //查詢列
  //     formType: x.columnType == "DateTime" && !x.searchType ? 'date' : (x.searchType || ''), //查詢類型
  //     dropNo: x.searchColNo || x.dropNo,//數據源
  //     // colSize: x.searchColSize//查詢宽度
  //     width: x.searchColSize

  param.TableColumns.forEach(x => {
    let searchItem = searchFormOptions.value.find(c => { return c.field == x.columnName }) || { rowNo: null, colNo: null, formType: null, dropNo: null, width: null }
    x.searchRowNo = searchItem.rowNo;
    x.searchColNo = searchItem.colNo;
    x.searchType = searchItem.formType;
    x.searchDropNo = searchItem.dropNo;
    x.searchColSize = searchItem.width;

    // title: x.columnCnName || x.columnName, //名字
    // field: x.columnName, //字段
    // rowNo: x.editRowNo, //查詢行
    // colNo: x.editColNo, //查詢列
    // formType: x.columnType == "DateTime" && !x.editType ? 'date' : (x.editType || ''), //編輯類型
    // dropNo: x.dropNo || x.searchColNo,//數據源
    // //  colSize: x.colSize,//編輯宽度
    // width: x.colSize,
    // isReadDataset: x.isReadDataset, //是否只讀
    // isNull: x.isNull,
    let editItem = editFormOptions.value.find(c => { return c.field == x.columnName }) || { rowNo: null, colNo: null, formType: null, dropNo: null, width: null }
    x.editRowNo = editItem.rowNo || 0;
    x.editColNo = editItem.colNo;
    x.editType = editItem.formType;
    x.colSize = editItem.width;
    if (!x.dropNo&&(editItem.dropNo || searchItem.dropNo)) {
      x.dropNo = editItem.dropNo || searchItem.dropNo;
    }
    x.isReadDataset = editItem.isReadDataset || 0;
  })

  return param;
};

const ceateVuePage = async (isApp) => {
  if (!(await validateTableInfo())) {
    return;
  }

  let vuePath;
  if (!isApp) {
    vuePath = localStorage.getItem("vuePath");
    if (!vuePath) {
      return proxy.$message.error("請先設置Vue項目對應Views的绝對路径,然后再保存!");
    }
  } else {
    vuePath = localStorage.getItem("appPath");
    if (!vuePath) {
      return proxy.$message.error("請先設置app路径,然后再保存!");
    }
  }

  let url = `/api/builder/createVuePage?vuePath=${vuePath}&vite=1&v3=1&app=${isApp || 0}`;
  const param = getParam();
  proxy.http.post(url, param, true).then((x) => {
    if (x == '頁面創建成功!') {
      proxy.$message.success(x);
      return;
    }
    proxy.$message.error(x);
  });
};

const buttons = ref([
  {
    name: "保存",
    icon: "Check",
    color: "#1e6fff",
    click: save,
  },
  {
    name: "新建",
    icon: "Plus",
    //  plain:true,
    type: 'danger',

    click: () => {
      proxy.base.resetForm(addFields, addOptions);
      addModel.value = true;
    },
    // type: "success",
  },
  {
    name: "預覽",
    icon: "Monitor",
    type: "danger",
    color: "#626aef",
    click: () => {
      if (!formFields.value.table_Id) {
        proxy.$message.error('請先加載表格信息')
        return;
      }
      window.open(location.origin + '/#/coderV3Priview?id=' + formFields.value.table_Id)
    },
  },
  {
    name: "生成vue頁面",
    icon: "Document",
    click: () => {
      ceateVuePage(0);
    },
  },
  {
    name: "生成app頁面",
    icon: "Cellphone",
    click: () => {
      ceateVuePage(1);
    },
  },
  {
    name: "生成model",
    icon: "Iphone",
    click: async () => {
      if (await validateTableInfo()) {
        const param = getParam();
        proxy.http.post("/api/builder/CreateModel", param, true).then((x) => {
          proxy.$message.info(x);
        });
      }
    },
  },
  {
    name: "生成業務類",
    icon: "Coin",
    click: async () => {
      if (await validateTableInfo()) {
        const param = getParam();
        let queryParam =
          "tableName=" +
          formFields.value.tableName +
          "&nameSpace=" +
          formFields.value.namespace +
          "&foldername=" +
          formFields.value.folderName;
        proxy.http
          .post("/api/builder/CreateServices?" + queryParam, param, true)
          .then((x) => {
            proxy.$message.info(x);
          });
      }
    },
  },
  {
    name: "删除配置",
    icon: "Delete",
    click: () => {
      let tableId = formFields.value.table_Id;
      if (!tableId) return proxy.$message.error("請選擇節點");
      proxy
        .$confirm("删除警告?", "確認要删除吗", {
          confirmButtonText: "確定",
          cancelButtonText: "取消",
          type: "warning",
          center: true,
        })
        .then(() => {
          proxy.http
            .post("/api/builder/delTree?table_Id=" + tableId, {}, true)
            .then((x) => {
              if (!x.status) return proxy.$message.error(x.message);
              proxy.$message.error("删除成功,請刷新頁面");
            });
        });
    },
  },
]);

const initData = (nameSpace, treeData, orginData, db) => {
  treeOrginData.value = orginData;
  addOptions.value[0][0].data = treeData;
  addOptions.value[0][0].orginData = orginData;
  addOptions.value[1][0].data = nameSpace;
  formOptions.value[0][2].data = nameSpace;
  formOptions.value[0][1].data = treeData;
  formOptions.value[0][1].orginData = orginData;
  // addOptions.value[addOptions.value.length - 1][0].data = nameSpace;
};

defineExpose({
  nodeClick,
  initData,
});
</script>
<style lang="less" scoped>
@import "./style/coderV3Content.less";

.desc {
  color: #0247de;
  font-size: 13px;
}
</style>
