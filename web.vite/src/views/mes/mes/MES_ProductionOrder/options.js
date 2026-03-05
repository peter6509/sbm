// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'OrderID',
        footer: "Foots",
        cnName: '生产訂單',
        name: 'MES_ProductionOrder',
        newTabEdit: false,
        url: "/MES_ProductionOrder/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"OrderNumber":"","CustomerName":"","OrderDate":"","DeliveryDate":"","OrderQty":"","LV":"","OrderStatus":"","CreateDate":""};
    const editFormOptions = [[{"title":"訂單編號","required":true,"field":"OrderNumber"},
                               {"title":"客户名稱","required":true,"field":"CustomerName"},
                               {"title":"訂單日期","required":true,"field":"OrderDate","type":"date"},
                               {"title":"交货日期","required":true,"field":"DeliveryDate","type":"date"}],
                              [{"title":"訂單數量","field":"OrderQty","type":"number"},
                               {"dataKey":"优先级","data":[],"title":"优先级","field":"LV","type":"select"},
                               {"dataKey":"排产狀態","data":[],"title":"排产狀態","required":true,"field":"OrderStatus","type":"select"},
                               {"title":"創建時間","field":"CreateDate","disabled":true}]];
    const searchFormFields = {"OrderNumber":"","CustomerName":"","OrderDate":"","DeliveryDate":"","OrderStatus":""};
    const searchFormOptions = [[{"title":"訂單編號","field":"OrderNumber"},{"title":"客户名稱","field":"CustomerName"},{"title":"訂單日期","field":"OrderDate","type":"date"},{"title":"交货日期","field":"DeliveryDate","type":"date"},{"dataKey":"排产狀態","data":[],"title":"排产狀態","field":"OrderStatus","type":"select"}]];
    const columns = [{field:'OrderID',title:'訂單ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'OrderNumber',title:'訂單編號',type:'string',link:true,width:90,require:true,align:'left'},
                       {field:'CustomerName',title:'客户名稱',type:'string',width:110,hidden:true,require:true,align:'left'},
                       {field:'OrderDate',title:'訂單日期',type:'date',sort:true,width:90,require:true,align:'left'},
                       {field:'DeliveryDate',title:'交货日期',type:'date',sort:true,width:90,hidden:true,require:true,align:'left'},
                       {field:'OrderQty',title:'訂單數量',type:'int',sort:true,width:80,align:'left'},
                       {field:'LV',title:'优先级',type:'string',bind:{ key:'优先级',data:[]},sort:true,width:70,align:'left'},
                       {field:'OrderStatus',title:'排产狀態',type:'string',bind:{ key:'排产狀態',data:[]},sort:true,width:80,require:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,hidden:true,readonly:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:140,readonly:true,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,hidden:true,align:'left'}];
    const detail =  {
                    cnName: '訂單明细',
                    table: 'MES_ProductionPlanDetail',
                    columns: [{field:'PlanDetailID',title:'計划明细ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'OrderID',title:'訂單ID',type:'string',width:90,hidden:true,align:'left'},
                       {field:'MaterialCode',title:'物料編碼',type:'string',bind:{ key:'物料列表',data:[]},width:100,edit:{type:'select'},require:true,align:'left'},
                       {field:'MaterialName',title:'物料名稱',type:'string',width:100,readonly:true,edit:{type:''},require:true,align:'left'},
                       {field:'MaterialSpecification',title:'物料規格',type:'string',width:130,readonly:true,edit:{type:''},align:'left'},
                       {field:'Unit',title:'單位',type:'string',bind:{ key:'物料單位',data:[]},width:70,readonly:true,edit:{type:'select'},align:'left'},
                       {field:'PlanQuantity',title:'訂單數量',type:'int',width:80,edit:{type:'number'},align:'left'},
                       {field:'PlannedStartTime',title:'計划開始時間',type:'datetime',width:90,hidden:true,align:'left'},
                       {field:'PlannedEndTime',title:'計划结束時間',type:'datetime',width:90,hidden:true,align:'left'},
                       {field:'PlanStatus',title:'計划狀態',type:'string',width:90,hidden:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,hidden:true,align:'left'}],
                    sortName: 'CreateDate',
                    key: 'PlanDetailID'
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