// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'InventoryID',
        footer: "Foots",
        cnName: '庫存管理',
        name: 'MES_InventoryManagement',
        newTabEdit: false,
        url: "/MES_InventoryManagement/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"MaterialName":"","MaterialCode":"","SpecificationModel":"","WarehouseID":"","LocationID":"","InventoryQuantity":"","InventoryUnit":""};
    const editFormOptions = [[{"title":"物料名稱","field":"MaterialName"},
                               {"title":"物料編號","field":"MaterialCode"},
                               {"title":"規格型號","field":"SpecificationModel"}],
                              [{"dataKey":"货位","data":[],"title":"仓庫","field":"WarehouseID","type":"select"},
                               {"dataKey":"仓庫","data":[],"title":"货位","field":"LocationID","type":"select"},
                               {"title":"庫存數量","required":true,"field":"InventoryQuantity","type":"number"},
                               {"dataKey":"物料單位","data":[],"title":"單位","field":"InventoryUnit","type":"select"}]];
    const searchFormFields = {"MaterialName":"","MaterialCode":"","WarehouseID":"","LocationID":"","InboundDate":""};
    const searchFormOptions = [[{"title":"物料名稱","field":"MaterialName"},{"title":"物料編號","field":"MaterialCode"},{"dataKey":"货位","data":[],"title":"仓庫","field":"WarehouseID","type":"select"},{"dataKey":"仓庫","data":[],"title":"货位","field":"LocationID","type":"select"},{"title":"入庫日期","field":"InboundDate","type":"datetime"}]];
    const columns = [{field:'InventoryID',title:'庫存ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'DocumentNo',title:'單據號',type:'string',width:90,hidden:true,align:'left'},
                       {field:'MaterialName',title:'物料名稱',type:'string',width:90,align:'left'},
                       {field:'MaterialCode',title:'物料編號',type:'string',width:90,align:'left'},
                       {field:'SpecificationModel',title:'規格型號',type:'string',width:90,align:'left'},
                       {field:'WarehouseID',title:'仓庫',type:'string',bind:{ key:'货位',data:[]},width:90,align:'left'},
                       {field:'LocationID',title:'货位',type:'string',bind:{ key:'仓庫',data:[]},width:90,align:'left'},
                       {field:'InventoryQuantity',title:'庫存數量',type:'int',sort:true,width:90,require:true,align:'left'},
                       {field:'InventoryUnit',title:'單位',type:'string',bind:{ key:'物料單位',data:[]},width:90,align:'left'},
                       {field:'InventoryCost',title:'庫存成本',type:'decimal',width:90,hidden:true,require:true,align:'left'},
                       {field:'InventoryStatus',title:'庫存狀態',type:'string',width:90,align:'left'},
                       {field:'InboundDate',title:'入庫日期',type:'datetime',width:90,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
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