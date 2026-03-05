// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'ChangeRecordID',
        footer: "Foots",
        cnName: '變更記錄',
        name: 'MES_ProductionPlanChangeRecord',
        newTabEdit: false,
        url: "/MES_ProductionPlanChangeRecord/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"OrderNumber":"","CustomerName":"","OrderDate":"","ChangeDate":"","OriginalPlanQuantity":"","NewPlanQuantity":"","ChangeReason":"","ChangedBy":""};
    const editFormOptions = [[{"title":"訂單編號","required":true,"field":"OrderNumber"},
                               {"title":"客户名稱","required":true,"field":"CustomerName"}],
                              [{"title":"訂單日期","field":"OrderDate","type":"date"},
                               {"title":"變更日期","required":true,"field":"ChangeDate","type":"date"}],
                              [{"title":"原計划數量","required":true,"field":"OriginalPlanQuantity","type":"number"},
                               {"title":"新計划數量","required":true,"field":"NewPlanQuantity","type":"number"}],
                              [{"title":"變更原因","required":true,"field":"ChangeReason"},
                               {"title":"變更人","required":true,"field":"ChangedBy"}]];
    const searchFormFields = {"OrderNumber":"","CustomerName":"","OrderDate":"","ChangeDate":"","OriginalPlanQuantity":[null,null]};
    const searchFormOptions = [[{"title":"訂單編號","field":"OrderNumber"},{"title":"客户名稱","field":"CustomerName"},{"title":"訂單日期","field":"OrderDate","type":"date"},{"title":"變更日期","field":"ChangeDate","type":"date"},{"title":"原計划數量","field":"OriginalPlanQuantity","type":"range"}]];
    const columns = [{field:'ChangeRecordID',title:'變更記錄ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'PlanDetailID',title:'計划明细ID',type:'string',width:90,hidden:true,align:'left'},
                       {field:'OrderNumber',title:'訂單編號',type:'string',link:true,width:90,require:true,align:'left'},
                       {field:'CustomerName',title:'客户名稱',type:'string',width:90,require:true,align:'left'},
                       {field:'OrderDate',title:'訂單日期',type:'date',width:90,align:'left'},
                       {field:'ChangeDate',title:'變更日期',type:'date',width:90,require:true,align:'left'},
                       {field:'OriginalPlanQuantity',title:'原計划數量',type:'int',width:90,require:true,align:'left'},
                       {field:'NewPlanQuantity',title:'新計划數量',type:'int',width:90,require:true,align:'left'},
                       {field:'OriginalPlannedStartTime',title:'原計划開始時間',type:'date',width:90,hidden:true,align:'left'},
                       {field:'NewPlannedStartTime',title:'新計划開始時間',type:'date',width:90,hidden:true,align:'left'},
                       {field:'OriginalPlannedEndTime',title:'原計划结束時間',type:'date',width:90,hidden:true,align:'left'},
                       {field:'NewPlannedEndTime',title:'新計划结束時間',type:'date',width:90,hidden:true,align:'left'},
                       {field:'ChangeReason',title:'變更原因',type:'string',width:90,require:true,align:'left'},
                       {field:'ChangedBy',title:'變更人',type:'string',width:90,require:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,hidden:true,align:'left'},
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