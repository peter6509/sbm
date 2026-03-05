// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'OutboundID',
        footer: "Foots",
        cnName: '产品出庫',
        name: 'MES_ProductOutbound',
        newTabEdit: false,
        url: "/MES_ProductOutbound/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"DocumentNo":"","MaterialName":"","MaterialCode":"","SpecificationModel":"","LocationID":"","OutboundQuantity":"","OutboundUnit":"","OutboundDate":""};
    const editFormOptions = [[{"title":"單據號","field":"DocumentNo"},
                               {"title":"物料名稱","required":true,"field":"MaterialName"}],
                              [{"title":"物料編號","required":true,"field":"MaterialCode"},
                               {"title":"規格型號","field":"SpecificationModel"}],
                              [{"dataKey":"货位","data":[],"title":"货位","required":true,"field":"LocationID","type":"select"},
                               {"title":"出庫數量","required":true,"field":"OutboundQuantity","type":"number"}],
                              [{"dataKey":"物料單位","data":[],"title":"出庫單位","field":"OutboundUnit","type":"select"},
                               {"title":"出庫日期","field":"OutboundDate","type":"date"}]];
    const searchFormFields = {"DocumentNo":"","MaterialName":"","MaterialCode":"","SpecificationModel":"","LocationID":"","OutboundQuantity":[null,null],"OutboundUnit":"","OutboundDate":"","Creator":"","CreateDate":""};
    const searchFormOptions = [[{"title":"單據號","field":"DocumentNo"},{"title":"物料名稱","field":"MaterialName"},{"title":"物料編號","field":"MaterialCode"},{"title":"規格型號","field":"SpecificationModel"},{"dataKey":"货位","data":[],"title":"货位","field":"LocationID","type":"select"}],[{"title":"出庫數量","field":"OutboundQuantity","type":"range"},{"dataKey":"物料單位","data":[],"title":"出庫單位","field":"OutboundUnit"},{"title":"出庫日期","field":"OutboundDate","type":"date"},{"title":"創建人","field":"Creator"},{"title":"創建時間","field":"CreateDate","type":"datetime"}]];
    const columns = [{field:'OutboundID',title:'出庫ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'DocumentNo',title:'單據號',type:'string',link:true,width:130,align:'left'},
                       {field:'MaterialName',title:'物料名稱',type:'string',width:90,require:true,align:'left'},
                       {field:'MaterialCode',title:'物料編號',type:'string',width:90,require:true,align:'left'},
                       {field:'SpecificationModel',title:'規格型號',type:'string',width:90,align:'left'},
                       {field:'WarehouseID',title:'仓庫',type:'string',width:90,hidden:true,align:'left'},
                       {field:'LocationID',title:'货位',type:'string',bind:{ key:'货位',data:[]},width:90,require:true,align:'left'},
                       {field:'OutboundQuantity',title:'出庫數量',type:'int',width:70,require:true,align:'left'},
                       {field:'OutboundUnit',title:'出庫單位',type:'string',bind:{ key:'物料單位',data:[]},width:70,align:'left'},
                       {field:'OutboundDate',title:'出庫日期',type:'date',width:90,align:'left'},
                       {field:'OutboundOperator',title:'出庫操作员',type:'string',width:90,hidden:true,align:'left'},
                       {field:'OutboundReason',title:'出庫原因',type:'string',width:90,hidden:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:140,align:'left'},
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