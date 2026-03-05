// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'CatalogID',
        footer: "Foots",
        cnName: '物料分類',
        name: 'MES_MaterialCatalog',
        newTabEdit: false,
        url: "/MES_MaterialCatalog/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"CatalogCode":"","CatalogName":"","ParentId":[],"Enable":"","Remarks":""};
    const editFormOptions = [[{"title":"分類編碼","required":true,"field":"CatalogCode"}],
                              [{"title":"分類名稱","required":true,"field":"CatalogName"}],
                              [{"dataKey":"物料分類","data":[],"title":"上级分類","field":"ParentId","type":"cascader"}],
                              [{"dataKey":"enable","data":[],"title":"啟用狀態","field":"Enable","type":"radio"}],
                              [{"title":"備註信息","field":"Remarks","type":"textarea"}]];
    const searchFormFields = {"CatalogName":"","CatalogCode":"","Enable":"","Creator":"","CreateDate":""};
    const searchFormOptions = [[{"title":"分類名稱","field":"CatalogName","type":"like"},{"title":"分類編碼","field":"CatalogCode","type":"like"},{"dataKey":"enable","data":[],"title":"啟用狀態","field":"Enable","type":"select"},{"title":"創建人","field":"Creator"},{"title":"創建時間","field":"CreateDate","type":"datetime"}]];
    const columns = [{field:'CatalogID',title:'分類ID',type:'string',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'CatalogName',title:'分類名稱',type:'string',width:120,require:true,align:'left'},
                       {field:'CatalogCode',title:'分類編碼',type:'string',link:true,width:120,require:true,align:'left'},
                       {field:'CatalogType',title:'分類類型',type:'string',width:120,hidden:true,align:'left'},
                       {field:'ParentId',title:'上级分類',type:'string',bind:{ key:'物料分類',data:[]},width:110,hidden:true,align:'left'},
                       {field:'Enable',title:'啟用狀態',type:'int',bind:{ key:'enable',data:[]},width:110,align:'left'},
                       {field:'Remarks',title:'備註信息',type:'string',width:120,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:140,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:100,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:140,align:'left'}];
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