// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'FaultRecordID',
        footer: "Foots",
        cnName: '設備故障',
        name: 'MES_EquipmentFaultRecord',
        newTabEdit: false,
        url: "/MES_EquipmentFaultRecord/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"EquipmentID":"","FaultDate":"","FaultType":"","FaultDescription":"","FaultImpact":"","FaultReportedBy":"","FaultStatus":"","TroubleshootingStartTime":"","TroubleshootingEndTime":""};
    const editFormOptions = [[{"dataKey":"設備列表","data":[],"title":"設備名稱","field":"EquipmentID","type":"select"},
                               {"title":"故障日期","field":"FaultDate","type":"datetime"},
                               {"title":"故障類型","field":"FaultType"}],
                              [{"title":"故障描述","field":"FaultDescription"},
                               {"title":"故障影响","field":"FaultImpact"},
                               {"title":"故障報告人","field":"FaultReportedBy"}],
                              [{"title":"故障狀態","field":"FaultStatus"},
                               {"title":"排查開始時間","field":"TroubleshootingStartTime","type":"datetime"},
                               {"title":"排查结束時間","field":"TroubleshootingEndTime","type":"datetime"}]];
    const searchFormFields = {"EquipmentID":"","FaultDate":"","FaultType":"","FaultDescription":"","FaultReportedBy":""};
    const searchFormOptions = [[{"dataKey":"設備列表","data":[],"title":"設備名稱","field":"EquipmentID","type":"select"},{"title":"故障日期","field":"FaultDate","type":"datetime"},{"title":"故障類型","field":"FaultType"},{"title":"故障描述","field":"FaultDescription"},{"title":"故障報告人","field":"FaultReportedBy"}]];
    const columns = [{field:'FaultRecordID',title:'故障記錄ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'EquipmentID',title:'設備名稱',type:'string',bind:{ key:'設備列表',data:[]},width:90,align:'left'},
                       {field:'FaultDate',title:'故障日期',type:'datetime',width:90,align:'left'},
                       {field:'FaultType',title:'故障類型',type:'string',width:90,align:'left'},
                       {field:'FaultDescription',title:'故障描述',type:'string',link:true,width:90,align:'left'},
                       {field:'FaultImpact',title:'故障影响',type:'string',width:90,align:'left'},
                       {field:'FaultReportedBy',title:'故障報告人',type:'string',width:90,align:'left'},
                       {field:'FaultStatus',title:'故障狀態',type:'string',width:90,align:'left'},
                       {field:'TroubleshootingStartTime',title:'排查開始時間',type:'datetime',width:90,hidden:true,align:'left'},
                       {field:'TroubleshootingEndTime',title:'排查结束時間',type:'datetime',width:90,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,hidden:true,align:'left'}];
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