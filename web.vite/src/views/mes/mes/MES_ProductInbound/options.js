// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'InboundID',
        footer: "Foots",
        cnName: '产品入庫',
        name: 'MES_ProductInbound',
        newTabEdit: false,
        url: "/MES_ProductInbound/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"DocumentNo":"","InboundQuantity":"","MaterialName":"","MaterialCode":"","SpecificationModel":"","LocationID":"","InboundUnit":"","InboundDate":""};
    const editFormOptions = [[{"title":"單據號","field":"DocumentNo"},
                               {"title":"入庫數量","required":true,"field":"InboundQuantity","type":"number"}],
                              [{"title":"物料名稱","field":"MaterialName"},
                               {"title":"物料編號","field":"MaterialCode"}],
                              [{"title":"規格型號","field":"SpecificationModel"},
                               {"dataKey":"仓庫","data":[],"title":"货位","field":"LocationID","type":"select"}],
                              [{"dataKey":"物料單位","data":[],"title":"入庫單位","field":"InboundUnit","type":"select"},
                               {"title":"入庫日期","field":"InboundDate"}]];
    const searchFormFields = {"DocumentNo":"","MaterialName":"","MaterialCode":"","WarehouseID":"","LocationID":""};
    const searchFormOptions = [[{"title":"單據號","field":"DocumentNo"},{"title":"物料名稱","field":"MaterialName"},{"title":"物料編號","field":"MaterialCode"},{"dataKey":"货位","data":[],"title":"仓庫","field":"WarehouseID","type":"select"},{"dataKey":"仓庫","data":[],"title":"货位","field":"LocationID","type":"select"}]];
    const columns = [{field:'InboundID',title:'入庫ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'DocumentNo',title:'單據號',type:'string',link:true,width:120,align:'left'},
                       {field:'MaterialName',title:'物料名稱',type:'string',width:90,align:'left'},
                       {field:'MaterialCode',title:'物料編號',type:'string',width:90,align:'left'},
                       {field:'SpecificationModel',title:'規格型號',type:'string',width:90,align:'left'},
                       {field:'WarehouseID',title:'仓庫',type:'string',bind:{ key:'货位',data:[]},width:90,hidden:true,align:'left'},
                       {field:'LocationID',title:'货位',type:'string',bind:{ key:'仓庫',data:[]},width:90,align:'left'},
                       {field:'InboundQuantity',title:'入庫數量',type:'int',width:90,require:true,align:'left'},
                       {field:'InboundUnit',title:'入庫單位',type:'string',bind:{ key:'物料單位',data:[]},width:90,align:'left'},
                       {field:'InboundDate',title:'入庫日期',type:'datetime',width:90,align:'left'},
                       {field:'InboundOperator',title:'入庫操作员',type:'string',width:90,hidden:true,align:'left'},
                       {field:'InboundReason',title:'入庫原因',type:'string',width:90,hidden:true,align:'left'},
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