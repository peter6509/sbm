// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'PlanDetailID',
        footer: "Foots",
        cnName: '訂單明细',
        name: 'MES_ProductionPlanDetail',
        newTabEdit: false,
        url: "/MES_ProductionPlanDetail/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"MaterialCode":"","MaterialName":"","MaterialSpecification":"","Unit":"","PlanQuantity":""};
    const editFormOptions = [[{"dataKey":"物料列表","data":[],"title":"物料編碼","required":true,"field":"MaterialCode","type":"select"},
                               {"title":"物料名稱","required":true,"field":"MaterialName","disabled":true},
                               {"title":"物料規格","field":"MaterialSpecification","disabled":true},
                               {"dataKey":"物料單位","data":[],"title":"單位","field":"Unit","disabled":true,"type":"select"},
                               {"title":"訂單數量","field":"PlanQuantity","type":"number"}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'PlanDetailID',title:'計划明细ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'OrderID',title:'訂單ID',type:'string',width:90,hidden:true,align:'left'},
                       {field:'MaterialCode',title:'物料編碼',type:'string',bind:{ key:'物料列表',data:[]},width:100,require:true,align:'left'},
                       {field:'MaterialName',title:'物料名稱',type:'string',width:100,readonly:true,require:true,align:'left'},
                       {field:'MaterialSpecification',title:'物料規格',type:'string',width:130,readonly:true,align:'left'},
                       {field:'Unit',title:'單位',type:'string',bind:{ key:'物料單位',data:[]},width:70,readonly:true,align:'left'},
                       {field:'PlanQuantity',title:'訂單數量',type:'int',width:80,align:'left'},
                       {field:'PlannedStartTime',title:'計划開始時間',type:'datetime',width:90,hidden:true,align:'left'},
                       {field:'PlannedEndTime',title:'計划结束時間',type:'datetime',width:90,hidden:true,align:'left'},
                       {field:'PlanStatus',title:'計划狀態',type:'string',width:90,hidden:true,align:'left'},
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