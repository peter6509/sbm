import { ref, reactive } from 'vue'

const dataConfig = () => {
  return {
    dyScript:{},//動態脚本
    defaultFormReadonlyFields: ref([]), //默認表單只讀的字段(審批設置只讀與還源的時候需要使用)
    // 是否彈出框審批
    isBoxAudit: ref(false),
    auditInited: ref(false),
    showTableAudit: ref(true), //是否顯示表格審批按鈕
    // 表單字段類型
    formFieldsType: ref([]),
    // 工作流步骤
    workFlowSteps: ref([]),
    // 樹形结構的主鍵字段，如果設置值默認會開啟樹形table；注意rowKey字段的值必須是唯一（2021.05.02）
    rowKey: ref(undefined),
    // 2020.09.011是否固定查詢表單，true查詢表單將固定顯示在表單的最上面
    fixedSearchForm: ref(false),
    // 初始化狀態
    isInited: ref(false),
    // 2021.03.19是否開啟查詢界面表格雙擊編輯
    doubleEdit: ref(false),
    // 當前用户没有編輯或新建權限時，表單只讀(可用于判断用户是否有編輯或新建權限)
    currentReadonly: ref(false),
    // 表是否單選
    single: ref(false),
    // 增删改查導入導出等對應的action
    action: ref('action'),
    // 新建或編輯的彈出框初化狀態，默認不做初始化，點擊新建或編輯才初始化彈出框
    boxInit: ref(false),
    // 高级查詢(界面查詢后的下拉框點擊觸發)
    searchBoxShow: ref(false),
    // 快速查詢字段
    singleSearch: ref({}),
    // 導出鏈接
    exportHref: ref(''),
    // 當新建或編輯時，記錄當前的狀態:如當前操作是新建
    currentAction: ref('Add'),
    // 當前編輯或查看數據的行
    currentRow: ref({}),
    // 是否可關閉
    closable: ref(false),
    // 彈出新建、編輯框
    boxModel: ref(false),
    // 彈出框查看表數據结構
    width: ref(700),
    // 高级查詢的標簽宽度
    labelWidth: ref(85),
    // 查看表结構的彈出框
    viewModel: ref(false),
    // 查看表结構的列數據
    viewColumns: ref([]),
    viewColumnsClone: ref([]),
    // 是否顯示自定義配置列按鈕2022.05.27
    showCustom: ref(true),
    showCustomSearch: ref(true), //是否顯示自定義查詢
    // 界面按鈕最多顯示的個數，超過的數量都顯示在更多中
    maxBtnLength: ref(7),
    // 查詢界面按鈕  如需要其他操作按鈕，可在表對應的.js中添加(如:Sys_User.js中buttons添加其他按鈕)
    buttons: ref([]),
    splitButtons: ref([]),
    // 彈出框按鈕 如需要其他操作按鈕，可在表對應的.js中添加
    boxButtons: ref([]),
    // 當前界面所有的下拉框字典編號及數據源
    dicKeys: ref([]),
    keyValueType: reactive({ _dinit: false }),
    // 界面表查詢的數據源的url
    url: ref(''),
    // 是否有從表(明细)表格數據
    hasDetail: ref(false),
    initActivated: ref(false),
    // 是否默認加載表數據
    load: ref(true),
    // 頁面觸發actived時是否刷新頁面數據
    activatedLoad: ref(false),
    // 查詢界面table是否顯示合計
    summary: ref(false),
    // 需要從远程绑定數據源的字典編號,如果字典數據源的查詢结果较多，請在onInit中將字典編號添加進来
    // 只對自定sql有效
    remoteKeys: ref([]),
    // 2020.11.01是否顯示行號
    columnIndex: ref(false),
    // 2020.11.01是否顯示checkbox
    ck: ref(true),
    // 2021.04.11新建時是否可以連续新建操作
    continueAdd: ref(false),
    // 2021.04.11按鈕名稱
    continueAddName: ref('保存后继续添加'),
    // detailUrl: "",
    // 從表配置
    detailOptions: reactive({
      // 明细表隐藏分頁
      paginationHide: false,
      // 明细表格可拖動位置，頂部拖動top,底部bottom
      dragPosition: '',
      // 彈出框從表表格操作按鈕,目前有删除行，添加行，刷新操作，如需要其他操作按鈕，可在表對應的.js中添加
      buttons: [],
      // 從表名稱
      cnName: '',
      // 從表主鍵名
      key: '',
      // 數據源
      data: [],
      // 從表列信息
      columns: [],
      // 明细是否可以編輯
      edit: true,
      // 明细表是否單選
      single: false,
      load: false,
      // 當編輯時删除當前明细的行主鍵值
      delKeys: [],
      // 從表加載數據的url
      url: '',
      // 從表分頁配置數據
      pagination: {
        // sizes: [20, 30, 40, 60, 90], //默認分頁條大小
        // size: 30, //默認分頁大小(每頁大小)
        sortName: '', //排序字段
        order: 'desc' //排序方式desc或者asc
      },
      // 默認從表高度
      height: 0,
      // 明细表行内容顯示在一行上，如果需要换行顯示，請設置為false
      textInline: true,
      // 使用雙擊編輯
      doubleEdit: true,
      // 是否開啟點擊單元格編輯，點擊其他行時结束編輯
      clickEdit: false,
      // 開啟編輯時
      beginEdit: (row, column, index) => {
        return true
      },
      // 结束編輯前
      endEditBefore: (row, column, index) => {
        return true
      },
      // 结束編輯后
      endEditAfter: (row, column, index) => {
        return true
      },
      // 2020.11.01明细是否顯示行號
      columnIndex: false,
      // 2020.11.01明细是否顯示checkbox
      ck: true,
      // 表格是否可以拖拽排序2024.10.06
      sortable: false
    }),
    // 審核對象
    auditParam: reactive({
      // 當前選中審核的行數
      rows: 0,
      // 審核彈出框
      model: false,
      // 審核结果
      value: -1,
      status: -1,
      // 審核原因
      reason: '',
      height: 500,
      showViewButton: true,
      auditHis: [],
      // 是否顯示審批操作(當前節點為用户審批時顯示)
      showAction: false,
      // 審核選項(可自行再添加)
      data: [
        { text: '通過', value: 1 },
        { text: '拒绝', value: 2 },
        { text: '驳回', value: 3 }
      ]
    }),
    // 導入上傳excel對象
    upload: reactive({
      // 導入的彈出框是否顯示
      excel: false,
      // 導入的路径,如果没有值，則不渲染導入功能
      url: '',
      template: {
        // 下載模板路径
        url: '',
        // 模板下載的中文名
        fileName: ''
      },
      // 是否有導入權限，有才渲染導入组件
      init: false
    }),
    // 表高度
    height: ref(0),
    // 查詢頁面table的高度
    tableHeight: ref(0),
    // 查詢頁面table的最大高度
    tableMaxHeight: ref(0),
    // table内容超出后是否不换行2020.01.16
    textInline: ref(true),
    // 從分頁配置數據
    pagination: reactive({ total: 0, size: 30, sortName: '' }),
    boxOptions: reactive({
      // 彈出框顯示的標題2022.08.01
      title: '',
      // saveClose新建或編輯成功后是否關閉彈出框
      saveClose: true,
      // 彈出框的標簽宽度labelWidth
      labelWidth: 85,
      height: 0,
      width: 0,
      // 彈出框明细table是否顯示合計
      summary: false,
      // 2022.09.12彈出框拖動功能
      draggable: false,
      // 2022.09.12彈出框背景遮罩層
      modal: true
    }),
    editor: reactive({
      // 上傳路径
      uploadImgUrl: '',
      // 上傳方法
      upload: null
    }),
    // 2022.09.26增加自定義導出文件名
    downloadFileName: ref(null),
    // 超出500數量顯示select2组件
    select2Count: ref(1500),
    // 新窗口編輯
    newTabEdit: ref(false),
    // 是否多明细表
    isMultiple: ref(false),
    // 明细表的高度
    detailHeight: ref(300),
    // 隐藏字段
    hiddenFields: ref([]),
    text: ref(''),
    // 三级明细表
    subDetails: ref([]),
    // 樹形表格是否默認延迟加載
    lazy: ref(true),
    // 樹形表格是否展開所有
    defaultExpandAll: ref(false),
    // 默認展開的節點
    expandRowKeys: ref([]),
    // 是否隐藏分頁
    paginationHide: ref(false),
    // 樹形表格父级id
    rowParentField: ref(''),
    // 導入excel彈出框的描述
    importDesc: ref(''),
    multiple: reactive({
      // 一對多水平顯示(二级表與三级表是否左右结構顯示)
      horizontal: false,
      // 左邊二级表宽度(默認不用設置)
      leftWidth: 0,
      // 右邊三级表宽度(默認不用設置)
      rightWidth: 0
    }),
    // 表格可拖動位置，頂部拖動top,底部bottom
    dragPosition: ref(''),
    // 編輯表單標簽文字顯示位置:left / top（默認是top，或者在main.js全局配置）
    labelPosition: ref(''),
    // 快捷查詢字段2024.01.18增加多個快捷查詢字段
    queryFields: ref([]),
    // 當前是否點擊的複制行操作
    isCopyClick: ref(false),
    // 是否使用padding間距
    padding: ref(false),
    gridRender: reactive({
      h: (h, {}) => {
        return ''
      },
      data: {}
    }),
    // 只提交變更的明细表數據2024.08.30
    submitChangeRows: ref(true),
    // 分頁或者刷新表格數據后是否保留複選框選擇狀態，2024.09.10
    reserveSelection: ref(false),
    // 表格是否可以拖拽排序2024.10.06
    sortable: ref(false),
    extraHeight: ref(0),
    showTabs: ref(true),
    orginColumnFields: reactive([]),
    //是否審批點擊
    isAuditClick: ref(false)
  }
}

export default dataConfig
