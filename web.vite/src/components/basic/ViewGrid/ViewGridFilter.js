let serviceFilter = {
  onInit() {
    //對應created
  },
  onInited() {
    //對應created，在onInit與onInited中間會初始化界面數據對象
  },
  mounted() { },
  searchBefore(param) {
    //查詢ViewGird表數據前,param查詢参數
    // console.log('表' + this.table.cnName + '觸發loadTableBefore');
    return true
  },
  //2020.10.30增加查詢后返回所有的查詢信息
  searchAfter(param, result) {
    //查詢ViewGird表數據后param查詢参數,result回返查詢的结果
    // console.log('表' + this.table.cnName + '觸發loadTableAfter');
    return true
  },
  searchDetailBefore(param, callBack, table, item) {
    //查詢從表表數據前,param查詢参數
    //console.log(this.detailOptions.cnName + '觸發loadDetailTableBefore');
    return true
  },
  searchDetailAfter(param, data) {
    //查詢從表后param查詢参數,result回返查詢的结果
    // console.log(this.detailOptions.cnName + '觸發loadDetailTableAfter');
    return true
  },
  delBefore(ids, rows) {
    //查詢界面的表删除前 ids為删除的id數组,,rows删除的行
    return true
  },
  delBeforeAsync(ids, rows) {
    //查詢界面的表删除前 ids為删除的id數组,,rows删除的行
    return true
  },
  getDelMessage(rows) { },
  delAfter(result) {
    //查詢界面的表删除后
    return true
  },
  delDetailRow(rows,table) {
    //彈出框删除明细表的行數據(只是對table操作，並没有操作后台)
    return true
  },
  delRowBefore(rows, table, item, index){  //彈出框删除明细表的行數據(只是對table操作，並没有操作后台)
    return true
  },
  delRowAfter(rows, table, item, index){//彈出框明细表删除后方法(只是對table操作，並没有操作后台)
    return true
  },
  saveConfirm(callback,formData,isAdd) {//保存前確認操作，formData表數據，isAdd是否新建操作
    //保存前自定義確認操作
    callback && callback()
  },
  addBefore(formData) {
    //新建保存前formData為對象，包括明细表
    return true
  },
  async addBeforeAsync(formData) {
    //異步處理,功能同上(2020.12.06)
    return true
  },
  addAfter(result) {
    //新建保存后result返回的狀態及表單對象
    return true
  },
  updateBefore(formData) {
    //編輯保存前formData為對象，包括明细表、删除行的Id
    return true
  },
  async updateBeforeAsync(formData) {
    //異步處理,功能同上(2020.12.06)
    return true
  },
  updateAfter(result) {
    //編輯保存后result返回的狀態及表單對象
    return true
  },
  auditBefore(ids, rows) {
    //審核前
    return true
  },
  auditAfter(result, rows) {
    // 審核后
    return true
  },
  resetAddFormBefore() {
    //重置新建表單前的内容
    return true
  },
  resetAddFormAfter() {
    //重置新建表單后的内容
    return true
  },
  resetUpdateFormBefore() {
    //重置編輯表單前的内容
    return true
  },
  resetUpdateFormAfter() {
    //重置編輯表單后的内容
    return true
  },
  modelOpenBefore(row) {
    //點擊編輯/新建按鈕彈出框前，可以在此處寫邏輯，如，從后台獲取數據
    return true
  },
  modelOpenBeforeAsync(row) {
    //點擊編輯/新建按鈕彈出框前，可以在此處寫邏輯，如，從后台獲取數據
    return
  },
  modelOpenAfter(row, currentAction, isCopyClick) {
    //點擊編輯/新建按鈕彈出框后，可以在此處寫邏輯，如，從后台獲取數據
  },
  importAfter(data) {
    return true; 
    //導入excel后刷新table表格數據
  },
  importDetailAfter(rows) { return true;  },//明细表導入后方法
  //2020.10.31添加導入前的方法
  importBefore(formData,callback) {
    //導入excel導入前
    //往formData寫一些其他参數提交到后台，
    // formData.append("val2", "xxx");
    //后台按下面方法獲取請求的参數
    // Core.Utilities.HttpContext.Current.Request("val2");

    //如果需要異步執行，請執行callback();//下面的return true去掉

    return true
  },
  reloadDicSource() {
    //重新加載字典绑定的數據源
  },
  exportBefore(param) {
    //2020.06.25增加導出前處理
    return true
  },
  exportAfter(res, param) { },//導出后方法
  getFileName(isDetail) {//導出時自定義文件名
    return ''
  },
  onModelClose(iconClick) {//彈出框關閉事件
    //iconClick=true為點擊左中上角X觸發的關閉事件
    //如果返回 false不會關閉彈出框
    //return false;
    return true
  },
  selectable(row, index) {
    //表CheckBox 是否可以勾選
    return true
  },
  sortEnd(newIndex, oldIndex, rows) { },//主表拖動排序
  detailSortEnd(rows, newIndex, oldIndex) { },//明细表主表拖動排序
  boxAuditOptionOpenBefore(row) { },//審批彈出框打開前方法
  addDetailRow(table, item, index){},//二、三级表添加行返回默認行數據
  tableAddRowBefore(row, index) {
    //行内編輯獲取默認編輯行前方法
    return true
  },
  getDefaultRow(row, index) {
    //行内編輯獲取默認編輯行
    return {}
  },
  dicInited(dic) {
    //字典初始后方法
  },
  rowChange(rows) { },
  selectionChange(rows) { },
  rowClick({ row, column, event }) { },
  rowDbClick({ row, column, event }) { },
  beginEdit(row, column, index) {
    //主表表格編輯
    return true
  },
  endEditBefore(row, column, index) {
    return true
  },
  //表格並合
  spanMethod({ row, column, rowIndex, columnIndex }, rows) { },
  detailSpanMethod({ row, column, rowIndex, columnIndex }, rows) { },
  resetSearchFormAfter() { },//查詢表單重置方法
  //表單分组切换事件
  tabClick(name) { },
  detailAddRowBefore(table,item) {//明细表添加行(包括一對多明细表添加行)
    //這里可以返回: return [{}]或者{}
  },
  //明细表複選框選擇事件
  detailRowChange(row, table) { },//明细表checkbox複選框選中事件
  detailSelectable(row, index, item) { return true; },//明细表checkbox複選框是否可以選中
  continueAddAfter(formFields, formData, res) { }, //連续添加操作保存后事件
  detailRowClick({ row, column, event, item }) { }, //明细表行點擊事件
  detailTabsClick(table){}, //一對明细表tabs點擊事件
  copyDataBefore(rows) {
    return true
  }, //點擊複制按鈕
  auditModelOpenBefore(rows, isAnti, view) {
    //審批彈出框打開前方法
    return true
  },
  getAuditTable(rows, isAnti, view) {//獲取實際審批的表
    return ''
  },
  flowLoadAfter(form, result) { },//審批彈出框加載流程信息
  printBefore(rows) { return true },//打印按鈕點擊前方法
  searchSubDetailBefore(rows, table, item){return true},//三级明细表加載前
  searchSubDetailAfter(rows, table, item){return true},//三级明细表加載后
  printModelClose(){}//打印彈出框關閉
}
export default serviceFilter
