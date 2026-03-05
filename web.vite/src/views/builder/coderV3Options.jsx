export const columnType = [
  { key: 1, value: "img" },
  { key: 2, value: "excel" },
  { key: 3, value: "file" },
  { key: 6, value: "year(年)" },
  { key: 4, value: "date(年月日)" },
  { key: 5, value: "month(年月)" },
];

export const dataType = [
  { key: "text", value: "input" },
  { key: "textarea", value: "textarea" },
  { key: "switch", value: "switch" },
  { key: "select", value: "select" },
  { key: "selectList", value: "select多選" },
  { key: "year", value: "年" },
  { key: "date", value: "date(年月日)" },
  { key: "datetime", value: "datetime" },
  { key: "month", value: "年月" },
  { key: "time", value: "time" },
  { key: "password", value: "密碼輸入框" },
  { key: "checkbox", value: "checkbox多選" },
  { key: "radio", value: "radio單選" },
  { key: "cascader", value: "级聯" },
  { key: "treeSelect", value: "樹形级聯(多選)" },
  { key: "selectTable", value: "下拉框Table搜索" },
  { key: "editor", value: "富文本編輯器" },
  { key: "mail", value: "mail" },
  { key: "number", value: "number" },
  { key: "decimal", value: "decimal" },
  { key: "phone", value: "phone" },
  { key: "color", value: "颜色選擇器" },
  { key: "img", value: "img" },
  { key: "excel", value: "excel" },
  { key: "file", value: "file" },
  { key: "rate", value: "評分" },
];

export const searchDataType = [
  { key: "=", value: "等于" },
  { key: "!=", value: "不等于" },
  { key: "like", value: "%模糊查詢%" },
  { key: "likeStart", value: "模糊查詢%" },
  { key: "likeEnd", value: "%模糊查詢" },
  { key: "textarea", value: "textarea" },
  { key: "switch", value: "switch" },
  { key: "select", value: "select" },
  { key: "selectList", value: "select多選" },
  { key: "year", value: "年" },
  { key: "date", value: "date(年月日)" },
  { key: "datetime", value: "datetime" },
  { key: "month", value: "年月" },
  { key: "time", value: "time" },
  { key: "cascader", value: "级聯" },
  { key: "treeSelect", value: "樹形级聯(多選)" },
  { key: "selectTable", value: "下拉框Table搜索" },
  { key: "checkbox", value: "checkbox" },
  { key: "radio", value: "radio" },
  { key: "range", value: "區間查詢" },
  { key: "mail", value: "mail" },
  { key: "number", value: "number" },
  { key: "decimal", value: "decimal" },
  // { key: 'phone', value: 'phone' }
];
import { ref, reactive } from "vue";
export const tableOptions = () => {
  return {
    formFields: ref({
      table_Id: "",
      parentId: null,
      namespace: "",
      columnCNName: "",
      tableName: "",
      tableTrueName: "",
      folderName: "",
      detailCnName: "",
      detailName: "",
      expressField: "",
      sortName: "",
      richtitle: "",
      uploadField: "",
      uploadMaxCount: "",
      enable: 0,
      vuePath: "",
      appPath: "",
      dbServer: "",
      editType: null, //編輯模式
      userPermissionDesc: "",
      foreignKey: null,
      dbSql: null,
      dyPage:0,
      fixedSearch:0,
      dyScript:null
    }),
    formOptions: ref([
      [
        {
          title: "主 鍵 ID",
          field: "table_Id",
          dataSource: [],
          readonly: true,
          disabled: true,
          columnType: "int",
        },
        {
          title: "父级ID",
          field: "parentId",
         // min: 0,
                    changeOnSelect:true,
          required: true,
          // type: "number",
          dataKey: "",
          data: [],
          type: "cascader",
        },
        {
          title: "項目類庫",
          placeholder: "代碼生成存放的位置",
          field: "namespace",
          type: "select",
          required: true,
          data: [],
          //  colSize: 6,
        },

        {
          title: "表中文名",
          field: "columnCNName",
          dataSource: [],
          required: true,
        },
        {
          title: "表别名",
          placeholder: "默認與實際表名相同",
          field: "tableName",
          required: true,
        },
        { title: "實際表名", field: "tableTrueName" },
      ],
      [
        {
          title: "文件夹名",
          //placeholder: "生成文件所在類庫中的文件夹名(文件夹可以不存在)",
          field: "folderName",
          required: true,
        },

        {
          title: "明细表中文名",
          field: "detailCnName",
          placeholder: "多個名字,隔開",
        },
        {
          title: "明细表(多張表逗號隔開)",
          field: "detailName",
          placeholder: "如：tabl1,table2",
        },
        {
          title: "快捷編輯",
          field: "expressField",
          placeholder: "快捷編輯字段",
        },
        {
          title: "默認排序字段",
          field: "sortName",
          placeholder: "多個排序字段逗號隔開",
        },

        {
          title: "編輯模式",
          field: "editType",
          type: "select",
          data: [
            { key: 0, value: "彈出框編輯" },
            { key: 1, value: "新頁面編輯" },
            { key: 2, value: "表格行内編輯" },
          ],
          extra: {
            render: (h) => {
              return (
                <el-popover
                  placement="top-start"
                  title="提示信息"
                  width={350}
                  trigger="hover"
                  content={
                    "1、彈出框編輯：以彈出框形式新建或修改 ；  2、新頁面編輯：打開一個新的tab頁面編輯或新建；   3、表格行内編輯：在當前查詢頁面的表格直接編輯"
                  }
                >
                  {{
                    reference: () => {
                      return (
                        <span
                          style="color:#9E9E9E"
                          class="el-icon-warning-outline"
                        ></span>
                      );
                    },
                  }}
                </el-popover>
              );
            },
          },

          //  colSize: 6
        },
      ],
      [
        {
          title: "與主表關聯字段",
          field: "foreignKey",
          type: "text",
          readonly:true
        },
        {
          title: "所在數據庫",
          field: "dbServer",
          type: "select",
          required: true,
          dataKey: "dbServer",
          data: [], // dbOptions
          extra: {
            render: (h) => {
              return (
                <el-popover
                  placement="top-start"
                  title="提示信息"
                  width={350}
                  trigger="hover"
                  content={"如果不分庫，選擇【系统庫SysDbContext】"}
                >
                  {{
                    reference: () => {
                      return (
                        <span
                          style="color:#9E9E9E"
                          class="el-icon-warning-outline"
                        ></span>
                      );
                    },
                  }}
                </el-popover>
              );
            },
          },
        },
        {
          title: "Vue路径",
          field: "vuePath",
          type: "text",
          placeholder: "路径：E:/app/src/views",
          colSize: 4,
        },
        {
          title: "app路径",
          field: "appPath",
          type: "text",
          placeholder: "路径：E:/uniapp/pages",
          colSize: 4,
        },
      ],
      [
        {
          title: "關聯sql",
          field: "dbSql",
          type: "textarea",
          placeholder: "多表關聯時在此處理直接寫sql或者數據庫使用视圖",
          colSize: 12,
          readonly:true
        },
      ],
    ]),
    addFields: ref({
      //table_Id: "",
      parentId: [],
      namespace: "",
      columnCNName: "",
      tableName: "",
      tableTrueName: "",
      folderName: "",

      dbServer: "",
      // editType: null, //編輯模式
      // userPermissionDesc: "",
    }),
    addOptions: ref([
      [
        {
          title: "父级ID",
          min: 0,
          field: "parentId",
          required: true,
          type: "number",
          dataKey: "",
          data: [],
          type: "cascader",
          changeOnSelect:true,
          labelRender: (h, {}) => {
            return (
              <div>
                <el-tooltip placement="top-start" title="" trigger="hover">
                  {{
                    default: () => {
                      return (
                        <span>
                          父级目錄ID
                          <i
                            style="font-size:12px;margin-left:3px;color:#0076d4"
                            class="el-icon-warning-outline"
                          ></i>
                        </span>
                      );
                    },
                    content: () => {
                      return (
                        <div>
                          放在左邊樹形结構的文件夹ID下,如果填入【0】就是一级目錄
                        </div>
                      );
                    },
                  }}
                </el-tooltip>
              </div>
            );
          },
        },
      ],
      [
        {
          title: "項目類庫",
          field: "namespace",
          type: "select",
          required: true,
          data: [],
          labelRender: (h, {}) => {
            return (
              <div>
                <el-tooltip placement="top-start" title="" trigger="hover">
                  {{
                    default: () => {
                      return (
                        <span>
                          項目類庫
                          <i
                            style="font-size:12px;margin-left:3px;color:#0076d4"
                            class="el-icon-warning-outline"
                          ></i>
                        </span>
                      );
                    },
                    content: () => {
                      return (
                        <div>
                          代碼生成后的所在類庫(可以自己提前在后台項目中創建一個.netcore類庫)
                        </div>
                      );
                    },
                  }}
                </el-tooltip>
              </div>
            );
          },
        },
      ],
      [
        {
          title: "表中文名",
          field: "columnCNName",
          required: true,
          placeholder: "表對應的中文名字",
          labelRender: (h, {}) => {
            return (
              <div>
                <el-tooltip placement="top-start" title="" trigger="hover">
                  {{
                    default: () => {
                      return (
                        <span>
                          表中文名
                          <i
                            style="font-size:12px;margin-left:3px;color:#0076d4"
                            class="el-icon-warning-outline"
                          ></i>
                        </span>
                      );
                    },
                    content: () => {
                      return <div>表對應的中文名字,自己填寫</div>;
                    },
                  }}
                </el-tooltip>
              </div>
            );
          },
        },
      ],
      [
        {
          title: "實際表名",
          field: "tableName",
          required: true,
          placeholder:
            "數據表/视圖名(多表批量生成:多表逗號隔開,后台啟動builder_run.bat)",
          labelRender: (h, {}) => {
            return (
              <div>
                <el-tooltip placement="top-start" title="" trigger="hover">
                  {{
                    default: () => {
                      return (
                        <span>
                          實際表名
                          <i
                            style="font-size:12px;margin-left:3px;color:#0076d4"
                            class="el-icon-warning-outline"
                          ></i>
                        </span>
                      );
                    },
                    content: () => {
                      return (
                        <div>
                          數據庫實際表名或者视圖名(多表關聯請創建视圖再生成代碼);如果只是創建目錄，表名寫一個不存在的名字並且没有填過這個名字
                        </div>
                      );
                    },
                  }}
                </el-tooltip>
              </div>
            );
          },
        },
      ],
      [
        {
          title: "文件夹名",
          field: "folderName",
          required: true,
          labelRender: (h, {}) => {
            return (
              <div>
                <el-tooltip placement="top-start" title="" trigger="hover">
                  {{
                    default: () => {
                      return (
                        <span>
                          文件夹名
                          <i
                            style="font-size:12px;margin-left:3px;color:#0076d4"
                            class="el-icon-warning-outline"
                          ></i>
                        </span>
                      );
                    },
                    content: () => {
                      return (
                        <div>
                          生成文件所在類庫中的文件夹名(文件夹可以不存在);注意只需要填寫文件夹名，不是路径
                        </div>
                      );
                    },
                  }}
                </el-tooltip>
              </div>
            );
          },
        },
      ],
      [
        {
          title: "數據庫",
          field: "dbServer",
          type: "select",
          dataKey: "dbServer",
          required: true,
          data: [],
          labelRender: (h, {}) => {
            return (
              <div>
                <el-tooltip placement="top-start" title="" trigger="hover">
                  {{
                    default: () => {
                      return (
                        <span>
                          所在數據庫
                          <i
                            style="font-size:12px;margin-left:3px;color:#0076d4"
                            class="el-icon-warning-outline"
                          ></i>
                        </span>
                      );
                    },
                    content: () => {
                      return <div>如果不分庫，選擇【系统庫SysDbContext】</div>;
                    },
                  }}
                </el-tooltip>
              </div>
            );
          },
        },
      ],
    ]),
    columns: ref([
      // {
      //   field: "columnId",
      //   title: "ColumnId",
      //   width: 120,
      //   align: "left",
      //   edit: { type: "text"},
      //   hidden: true,
      // },
      // {
      //   field: "table_Id",
      //   title: "Table_Id",
      //   width: 10,
      //   align: "left",
      //   editor: "text",
      //   hidden: true,
      // },
      {
        field: "columnCnName",
        title: "名稱",
        fixed: true,
        width: 90,
        placeholder:"請輸入",
        align: "left",
        edit: { type: "text", keep: false },
      },
      {
        field: "columnName",
        title: "字段",
        fixed: true,
        width: 90,
        align: "left",
        edit: { type: "text" },
      },
      {
        field: "referenceField",
        title: "關聯字段",
        fixed: true,
        width: 80,
        align: "center",
        edit: { type: "switch", keep: false },
      },
      {
        field: "isKey",
        title: "主鍵",
        width: 50,
        align: "center",
        edit: { type: "switch" },
      },
      // {
      //   field: "enable",
      //   title: "app列",
      //   width: 110,
      //   align: "left",
      //   edit: { type: "select" },
      //   bind: {
      //     data: [
      //       { key: 1, value: "顯示/查詢/編輯" },
      //       { key: 2, value: "顯示/編輯" },
      //       { key: 3, value: "顯示/查詢" },
      //       { key: 4, value: "顯示" },
      //       { key: 5, value: "查詢/編輯" },
      //       { key: 6, value: "查詢" },
      //       { key: 7, value: "編輯" },
      //     ],
      //   },
      // },
      // {
      //   field: "searchRowNo",
      //   title: "查詢行",
      //   width: 90,
      //   align: "left",
      //   edit: { type: "text" },
      // },
      // {
      //   field: "searchColNo",
      //   title: "查詢列",
      //   width: 90,
      //   align: "left",
      //   edit: { type: "text" },
      // },
      // {
      //   field: "searchType",
      //   title: "查詢類型",
      //   width: 150,
      //   align: "left",
      //   edit: { type: "select" },
      //   bind: { data: searchDataType },
      // },
      // {
      //   field: "editRowNo",
      //   title: "編輯行",
      //   width: 90,
      //   align: "numberbox",
      //   edit: { type: "text" },
      // },
      // {
      //   field: "editColNo",
      //   title: "編輯列",
      //   width: 90,
      //   align: "numberbox",
      //   edit: { type: "text" },
      // },
      // {
      //   field: "editType",
      //   title: "編輯類型",
      //   width: 150,
      //   align: "left",
      //   edit: { type: "select" },
      //   bind: { data: dataType },
      // },
      {
        field: "dropNo",
        title: "數據源",
        width: 80,
              placeholder:" ",
        align: "center",
        bind: { data: [] },
        edit: { type: "select", data: [], keep: false },
      },
      {
        field: "orderNo",
        title: "顯示顺序",
        width: 70,
        align: "center",
        edit: { type: "text", keep: false },
      },

      // {
      //   field: "isReadDataset",
      //   title: "是否只讀",
      //   width: 120,
      //   align: "left",
      //   edit: { type: "switch", keep: false },
      // },
      {
        field: "isDisplay",
        title: "是否顯示",
        width: 80,
        align: "center",
        edit: { type: "switch", keep: false, keep: false },
      },
      {
        field: "columnWidth",
        title: "表格宽度",
        width: 80,
        align: "center",
        edit: { type: "text", keep: false },
      },
      // {
      //   field: "maxlength",
      //   title: "字段最大長度",
      //   width: 80,
      //   align: "left",
      //   edit: { type: "text" },
      // },
              {
        field: "isNull",
        title: "可為空",
        width: 70,
        align: "center",
        edit: { type: "switch", keep: false, keep: false },
      },
            {
        field: "isImage",
        title: "顯示類型",
        hidden: false,
        width: 80,
        align: "center",
          placeholder:" ",
        edit: { type: "select", keep: false },
        bind: { data: columnType },
      },
            {
        field: "sortable",
        title: "表格排序",
        width: 80,
        align: "center",
        edit: { type: "switch", keep: false },
      },
      {
        field: "columnType",
        title: "數據類型",
        width: 70,
        align: "center",
        // edit: { type: "text", keep: false  },
      }
      // {
      //   field: "colSize",
      //   title: "編輯字段宽度colSize",
      //   width: 180,
      //   align: "left",
      //   edit: { type: "select" },
      //   bind: {
      //     data: [
      //       { key: 0, value: "自動宽度" },
      //       { key: 2, value: "20%" },
      //       { key: 3, value: "30%" },
      //       { key: 4, value: "40%" },
      //       { key: 6, value: "50%" },
      //       { key: 8, value: "60%" },
      //       { key: 10, value: "80%" },
      //       { key: 12, value: "100%" },
      //     ],
      //   },
      // },
      //{ field: "createDate", title: "創建時間", width: 120, align: "left" },
    ]),
  };
};
