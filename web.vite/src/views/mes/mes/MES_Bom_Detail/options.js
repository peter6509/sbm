// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'DomDetailId',
        footer: "Foots",
        cnName: 'BOM明细',
        name: 'MES_Bom_Detail',
        newTabEdit: false,
        url: "/MES_Bom_Detail/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"MaterialCode":"","MaterialName":"","Spec":"","UsageQty":"","ConsumeModel":"","Warehouse":"","SupplierCode":"","KitScale":""};
    const editFormOptions = [[{"title":"子件物料編碼","field":"MaterialCode"}],
                              [{"title":"子件物料名稱","field":"MaterialName"}],
                              [{"title":"規格型號","field":"Spec"}],
                              [{"title":"單台用量","required":true,"field":"UsageQty","type":"number"}],
                              [{"title":"消耗方式","required":true,"field":"ConsumeModel"}],
                              [{"title":"投料仓庫","field":"Warehouse"}],
                              [{"dataKey":"供應商","data":[],"title":"供應商","field":"SupplierCode","type":"select"}],
                              [{"title":"齐套比例","field":"KitScale","type":"decimal"}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'DomDetailId',title:'ID',type:'string',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'BomId',title:'BomId',type:'string',width:110,hidden:true,align:'left'},
                       {field:'Sequence',title:'序號',type:'int',width:50,require:true,align:'left'},
                       {field:'MaterialCode',title:'子件物料編碼',type:'string',link:true,width:130,align:'left'},
                       {field:'MaterialName',title:'子件物料名稱',type:'string',width:130,align:'left'},
                       {field:'Spec',title:'規格型號',type:'string',width:80,align:'left'},
                       {field:'UsageQty',title:'單台用量',type:'decimal',width:90,require:true,align:'left'},
                       {field:'ConsumeModel',title:'消耗方式',type:'string',width:90,require:true,align:'left'},
                       {field:'Warehouse',title:'投料仓庫',type:'string',width:90,align:'left'},
                       {field:'SupplierCode',title:'供應商',type:'string',bind:{ key:'供應商',data:[]},width:120,align:'left'},
                       {field:'KitScale',title:'齐套比例',type:'decimal',width:90,align:'left'},
                       {field:'Enable',title:'啟用狀態',type:'int',width:110,hidden:true,align:'left'},
                       {field:'CreateID',title:'創建人',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:120,hidden:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:110,hidden:true,align:'left'},
                       {field:'ModifyID',title:'更新人',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'更新人名稱',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'更新時間',type:'datetime',width:110,hidden:true,align:'left'}];
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