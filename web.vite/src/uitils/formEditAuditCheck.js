
//編輯彈出框全局設置頁面已經審批的數據是否可以編輯
//row:當前編輯的行數據
const checkFormReadonly = (row, tableInfo) => {
    //1.返回true時，整個編輯設置為只讀狀態,不可修改數據
    //2.可以判断tableInfo.name對應是哪張單獨處理邏輯
    //3.默認AuditStatus=1審批通過的字段不能再編輯，更多狀態判断自己接着加
    if (row.AuditStatus == 1&&!(tableInfo.name||'').startsWith('sys')) {
        return true;
    }
    return false;
}
//全局判断已經審批的數據是否可以删除
const checkDelRows = (rows, table) => {
    //這里自己判断數據，不能删除就提示
    // if (rows.some(x=>{return AuditStatus==1})) {
    //     return '已審批的數據不能删除';
    // }
    return;
}
export {
    checkFormReadonly,
    checkDelRows
}