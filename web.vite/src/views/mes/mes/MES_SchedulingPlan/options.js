// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'SchedulePlanID',
        footer: "Foots",
        cnName: '排班計划',
        name: 'MES_SchedulingPlan',
        newTabEdit: false,
        url: "/MES_SchedulingPlan/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"PlanName":"","ProductionLine":"","TeamName":"","StartDate":"","EndDate":""};
    const editFormOptions = [[{"title":"排班名稱","required":true,"field":"PlanName"}],
                              [{"title":"产線名稱","required":true,"field":"ProductionLine"}],
                              [{"title":"班组名稱","required":true,"field":"TeamName"}],
                              [{"title":"開始時間","required":true,"field":"StartDate","type":"datetime"}],
                              [{"title":"结束時間","required":true,"field":"EndDate","type":"datetime"}]];
    const searchFormFields = {"PlanName":"","ProductionLine":"","TeamName":"","StartDate":"","EndDate":""};
    const searchFormOptions = [[{"title":"排班名稱","field":"PlanName"},{"title":"产線名稱","field":"ProductionLine"},{"title":"班组名稱","field":"TeamName"},{"title":"開始時間","field":"StartDate","type":"datetime"},{"title":"结束時間","field":"EndDate","type":"datetime"}]];
    const columns = [{field:'SchedulePlanID',title:'排班計划ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'PlanName',title:'排班名稱',type:'string',link:true,width:120,require:true,align:'left'},
                       {field:'ProductionLine',title:'产線名稱',type:'string',width:90,require:true,align:'left'},
                       {field:'TeamName',title:'班组名稱',type:'string',width:90,require:true,align:'left'},
                       {field:'StartDate',title:'開始時間',type:'datetime',width:120,require:true,align:'left'},
                       {field:'EndDate',title:'结束時間',type:'datetime',width:120,require:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:120,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:120,align:'left'}];
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