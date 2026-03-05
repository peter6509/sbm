// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'WorkFlowTable_Id',
        footer: "Foots",
        cnName: '審批流程',
        name: 'Sys_WorkFlowTable',
        newTabEdit: false,
        url: "/Sys_WorkFlowTable/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {};
    const editFormOptions = [];
    const searchFormFields = {"WorkName":"","WorkTableName":"","AuditStatus":"","CreateDate":""};
    const searchFormOptions = [[{"dataKey":"audit","data":[],"title":"審批狀態","field":"AuditStatus","type":"select"},{"title":"流程名稱","field":"WorkName","type":"like"},{"title":"業務名稱","field":"WorkTableName","type":"like"},{"title":"創建時間","field":"CreateDate","type":"datetime"}]];
    const columns = [{field:'WorkFlowTable_Id',title:'WorkFlowTable_Id',type:'string',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'WorkFlow_Id',title:'流程id',type:'string',width:110,hidden:true,align:'left'},
                       {field:'WorkName',title:'流程名稱',type:'string',width:100,align:'left'},
                       {field:'TitleTemplate',title:'標題',type:'string',width:120,align:'left'},
                       {field:'WorkTableKey',title:'表主鍵id',type:'string',width:180,hidden:true,align:'left'},
                       {field:'WorkTable',title:'表名',type:'string',width:90,align:'left'},
                       {field:'WorkTableName',title:'業務名稱',type:'string',width:100,align:'left'},
                       {field:'CurrentStepId',title:'當前審核節點ID',type:'string',width:110,align:'left'},
                       {field:'StepName',title:'當前審核節點名稱',type:'string',width:100,align:'left'},
                       {field:'CurrentOrderId',title:'不用',type:'int',width:90,hidden:true,align:'left'},
                       {field:'AuditStatus',title:'審批狀態',type:'int',bind:{ key:'audit',data:[]},width:110,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:80,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:135,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Enable',title:'Enable',type:'byte',width:110,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:130,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:110,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'DbServiceId',title:'DbServiceId',type:'string',width:120,hidden:true,align:'left'}];
    const detail =  {
                    cnName: '審批節點',
                    table: 'Sys_WorkFlowTableStep',
                    columns: [{field:'Sys_WorkFlowTableStep_Id',title:'Sys_WorkFlowTableStep_Id',type:'string',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'WorkFlowTable_Id',title:'主表id',type:'string',width:110,require:true,align:'left'},
                       {field:'WorkFlow_Id',title:'流程id',type:'string',width:110,align:'left'},
                       {field:'StepId',title:'節點id',type:'string',width:120,align:'left'},
                       {field:'StepName',title:'節名稱',type:'string',width:180,align:'left'},
                       {field:'StepType',title:'審批類型',type:'int',width:110,align:'left'},
                       {field:'StepValue',title:'節點類型(1=按用户審批,2=按角色審批)',type:'string',width:110,align:'left'},
                       {field:'OrderId',title:'審批顺序',type:'int',width:110,align:'left'},
                       {field:'Remark',title:'Remark',type:'string',width:220,align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:110,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:130,align:'left'},
                       {field:'Enable',title:'Enable',type:'byte',width:110,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:130,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:110,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'AuditId',title:'審核人id',type:'int',width:80,align:'left'},
                       {field:'Auditor',title:'審核人',type:'string',width:120,align:'left'},
                       {field:'AuditStatus',title:'審核狀態',type:'int',width:80,align:'left'},
                       {field:'AuditDate',title:'審核時間',type:'datetime',width:150,align:'left'},
                       {field:'StepAttrType',title:'節點屬性(start、node、end))',type:'string',width:110,align:'left'},
                       {field:'ParentId',title:'ParentId',type:'string',width:120,align:'left'},
                       {field:'NextStepId',title:'NextStepId',type:'string',width:120,align:'left'},
                       {field:'Weight',title:'Weight',type:'int',width:80,align:'left'},
                       {field:'AuditMethod',title:'AuditMethod',type:'int',width:120,align:'left'},
                       {field:'FormOptions',title:'FormOptions',type:'string',width:120,align:'left'},
                       {field:'SourceType',title:'SourceType',type:'string',width:120,align:'left'},
                       {field:'AttachFile',title:'附件',type:'string',width:120,align:'left'},
                       {field:'AttachType',title:'附件類型',type:'string',width:120,align:'left'},
                       {field:'StepEditForm',title:'編輯表單',type:'string',width:120,align:'left'},
                       {field:'AttachQty',title:'附件數量',type:'int',width:120,align:'left'}],
                    sortName: 'CreateDate',
                    key: 'Sys_WorkFlowTableStep_Id'
                                            };
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