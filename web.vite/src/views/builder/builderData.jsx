
import { columnType,dataType,searchDataType } from "./coderV3Options.jsx";

let data = {
  form: {
    fields: {
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
    },
    addOptions: [
      [
        {
          title: "父级ID",
          min: 0,
          field: "parentId",
          required: true,
          type: "number",
          labelRender: (h, {}) => {
            return (
              <div>
                <el-tooltip placement="top-start" title="" trigger="hover">
                  {{
                    default: () => {
                      return (
                        <span>
                          父级ID
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
        },
      ],
      [
        {
          title: "實際表名",
          field: "tableName",
          required: true,
          placeholder: "數據表/视圖名(多表批量生成:多表逗號隔開,后台啟動builder_run.bat)",
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
    ],
    options: [
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
          title: "父 级 ID",
          field: "parentId",
          min: 0,
          required: true,
          type: "number",
        },
        {
          title: "項目類庫",
          placeholder: "代碼生成存放的位置",
          field: "namespace",
          type: "select",
          required: true,
          data: [],
          colSize: 6,
        },
      ],
      [
        {
          title: "表中文名",
          field: "columnCNName",
          dataSource: [],
          required: true,
        },
        {
          title: "表 别 名",
          placeholder: "默認與實際表名相同",
          field: "tableName",
          required: true,
        },
        { title: "實際表名", field: "tableTrueName" },
        {
          title: "文件夹名",
          placeholder: "生成文件所在類庫中的文件夹名(文件夹可以不存在)",
          field: "folderName",
          required: true,
        },
      ],
      [
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
          title: "排序字段",
          field: "sortName",
          placeholder: "多個排序字段逗號隔開(默認降序排序),如：Name,Age",
        },
      ],

      [
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
        {
          title: "Vue路径",
          field: "vuePath",
          type: "text",
          placeholder: "路径：E:/app/src/views",
          //  colSize: 6
        },
        {
          title: "app路径",
          field: "appPath",
          type: "text",
          placeholder: "路径：E:/uniapp/pages",
          /// colSize: 6
        },

        {
          title: "數據庫",
          field: "dbServer",
          type: "select",
          required: true,
          dataKey: "dbServer",
          //2020.08.22配置多個數據庫的DBContext,數據源data的key必須與后台項目VOL.Core-》EFDbContext下的文件名相同
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
      ],
    ],
  },
  columns: [
    {
      field: "columnId",
      title: "ColumnId",
      width: 120,
      align: "left",
      edit: { type: "text" },
      hidden: true,
    },
    {
      field: "table_Id",
      title: "Table_Id",
      width: 120,
      align: "left",
      editor: "text",
      hidden: true,
    },
    {
      field: "columnCnName",
      title: "列顯示名稱",
      fixed: true,
      width: 120,
      align: "left",
      edit: { type: "text" },
    },
    {
      field: "columnName",
      title: "列名",
      fixed: true,
      width: 120,
      align: "left",
      edit: { type: "text" },
    },
    {
      field: "isKey",
      title: "主鍵",
      width: 90,
      align: "left",
      edit: { type: "switch" },
    },
    {
      field: "sortable",
      title: "是否排序",
      width: 90,
      align: "left",
      edit: { type: "switch", keep: true },
    },
    {
      field: "enable",
      title: "app列",
      width: 140,
      align: "left",
      edit: { type: "select" },
      bind: {
        data: [
          { key: 1, value: "顯示/查詢/編輯" },
          { key: 2, value: "顯示/編輯" },
          { key: 3, value: "顯示/查詢" },
          { key: 4, value: "顯示" },
          { key: 5, value: "查詢/編輯" },
          { key: 6, value: "查詢" },
          { key: 7, value: "編輯" },
        ],
      },
    },
    {
      field: "searchRowNo",
      title: "查詢行",
      width: 90,
      align: "left",
      edit: { type: "text" },
    },
    {
      field: "searchColNo",
      title: "查詢列",
      width: 90,
      align: "left",
      edit: { type: "text" },
    },
    {
      field: "searchType",
      title: "查詢類型",
      width: 150,
      align: "left",
      edit: { type: "select" },
      bind: { data: searchDataType },
    },
    {
      field: "editRowNo",
      title: "編輯行",
      width: 90,
      align: "numberbox",
      edit: { type: "text" },
    },
    {
      field: "editColNo",
      title: "編輯列",
      width: 90,
      align: "numberbox",
      edit: { type: "text" },
    },
    {
      field: "editType",
      title: "編輯類型",
      width: 150,
      align: "left",
      edit: { type: "select" },
      bind: { data: dataType },
    },
    {
      field: "dropNo",
      title: "數據源",
      width: 120,
      align: "left",
      bind: { data: [] },
      edit: { type: "select", data: [] },
    },
    {
      field: "isImage",
      title: "table列顯示類型",
      hidden: false,
      width: 130,
      align: "left",
      edit: { type: "select" },
      bind: { data: columnType },
    },
    {
      field: "orderNo",
      title: "列顯示顺序",
      width: 120,
      align: "left",
      edit: { type: "text" },
    },
    {
      field: "maxlength",
      title: "字段最大長度",
      width: 130,
      align: "left",
      edit: { type: "text" },
    },
    {
      field: "columnType",
      title: "數據類型",
      width: 120,
      align: "left",
      edit: { type: "text" },
    },
    {
      field: "isNull",
      title: "可為空",
      width: 120,
      align: "left",
      edit: { type: "switch", keep: true },
    },
    {
      field: "isReadDataset",
      title: "是否只讀",
      width: 120,
      align: "left",
      edit: { type: "switch", keep: true },
    },
    // {
    //   field: 'isColumnData',
    //   title: '數據列',
    //   width: 120,
    //   align: 'left',
    //   edit: { type: 'switch', keep: true }
    // },
    {
      field: "isDisplay",
      title: "是否顯示",
      width: 120,
      align: "left",
      edit: { type: "switch", keep: true },
    },
    {
      field: "columnWidth",
      title: "table列宽度",
      width: 120,
      align: "left",
      edit: { type: "text" },
    },
    {
      field: "colSize",
      title: "編輯字段宽度colSize",
      width: 180,
      align: "left",
      edit: { type: "select" },
      bind: {
        data: [
          { key: 0, value: "自動宽度" },
          // { key: 2, value: "20%" },
          // { key: 3, value: "30%" },
          // { key: 4, value: "40%" },
          // { key: 6, value: "50%" },
          // { key: 8, value: "60%" },
          // { key: 10, value: "80%" },
          // { key: 12, value: "100%" },
             { key: 20, value: "20%" },
          { key: 33, value: "30%" },
          { key: 40, value: "40%" },
          { key: 50, value: "50%" },
          { key: 60, value: "60%" },
          { key: 80, value: "80%" },
          { key: 100, value: "100%" },
        ],
      },
    },
    { field: "createDate", title: "創建時間", width: 120, align: "left" },
  ],
};

export default data;
