// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'NotificationLogId',
        footer: "Foots",
        cnName: '消息記錄',
        name: 'Sys_NotificationLog',
        newTabEdit: false,
        url: "/Sys_NotificationLog/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {};
    const editFormOptions = [];
    const searchFormFields = {"NotificationTitle":"","NotificationType":"","ReceiveUserTrueName":"","IsRead":"","CreateDate":""};
    const searchFormOptions = [[{"title":"通知標題","field":"NotificationTitle","type":"like"},{"dataKey":"通知類型","data":[],"title":"通知類型","field":"NotificationType","type":"select"},{"title":"接收人","field":"ReceiveUserTrueName","type":"like"},{"dataKey":"enable","data":[],"title":"讀取狀態","field":"IsRead","type":"select"},{"title":"發送時間","field":"CreateDate","type":"datetime"}]];
    const columns = [{field:'NotificationLogId',title:'主鍵ID',type:'string',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'NotificationId',title:'消息ID',type:'string',width:110,hidden:true,align:'left'},
                       {field:'NotificationTemplateId',title:'模板ID',type:'string',width:110,hidden:true,align:'left'},
                       {field:'NotificationTitle',title:'通知標題',type:'string',width:130,align:'left'},
                       {field:'BusinessFunction',title:'業務功能',type:'string',width:100,align:'left'},
                       {field:'TableName',title:'TableName',type:'string',width:180,hidden:true,align:'left'},
                       {field:'TableKey',title:'TableKey',type:'string',width:120,hidden:true,align:'left'},
                       {field:'NotificationContent',title:'通知内容',type:'string',width:140,align:'left'},
                       {field:'NotificationType',title:'通知類型',type:'string',bind:{ key:'通知類型',data:[]},width:90,align:'left'},
                       {field:'NotificationLevel',title:'通知级别',type:'string',width:80,align:'left'},
                       {field:'ReceiveUserId',title:'接收用户id',type:'int',width:110,hidden:true,align:'left'},
                       {field:'ReceiveUserName',title:'接收用户帐號',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ReceiveUserTrueName',title:'接收人',type:'string',width:100,align:'left'},
                       {field:'LinkUrl',title:'跳轉地址',type:'string',width:150,hidden:true,align:'left'},
                       {field:'LinkType',title:'跳轉類型',type:'string',width:120,hidden:true,align:'left'},
                       {field:'IsRead',title:'讀取狀態',type:'int',bind:{ key:'enable',data:[]},width:110,require:true,align:'left'},
                       {field:'ReadDate',title:'讀取時間',type:'datetime',width:130,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:150,hidden:true,align:'left'},
                       {field:'Enable',title:'Enable',type:'int',width:110,hidden:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'發送人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'發送時間',type:'datetime',width:150,align:'left'}];
    const detail ={columns:[]};
    const details = [];

    return {
        table,
        key,
        tableName,
        tableCNName,
        newTabEdit,
        editFormFields,
        editFormOptions,
        searchFormFields,
        searchFormOptions,
        columns,
        detail,
        details
    };
}