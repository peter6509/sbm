// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'MaterialID',
        footer: "Foots",
        cnName: '物料管理',
        name: 'MES_Material',
        newTabEdit: false,
        url: "/MES_Material/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"MaterialCode":"","MaterialName":"","CatalogID":[],"MaterialType":"","Specification":"","Unit":"","Remarks":"","Img":""};
    const editFormOptions = [[{"title":"物料編碼","required":true,"field":"MaterialCode"}],
                              [{"title":"物料名稱","required":true,"field":"MaterialName"}],
                              [{"dataKey":"物料分類","data":[],"title":"物料分類","field":"CatalogID","type":"cascader"}],
                              [{"title":"物料類型","field":"MaterialType"}],
                              [{"title":"規格型號","field":"Specification"}],
                              [{"dataKey":"物料單位","data":[],"title":"單位","field":"Unit","type":"select"}],
                              [{"title":"備註信息","field":"Remarks","type":"textarea"}],
                              [{"title":"物料圖片","field":"Img","type":"img"}]];
    const searchFormFields = {"MaterialCode":"","MaterialName":"","MaterialType":"","Specification":"","Unit":""};
    const searchFormOptions = [[{"title":"物料編碼","field":"MaterialCode"},{"title":"物料名稱","field":"MaterialName"},{"title":"物料類型","field":"MaterialType"},{"title":"規格型號","field":"Specification"},{"dataKey":"物料單位","data":[],"title":"單位","field":"Unit"}]];
    const columns = [{field:'MaterialID',title:'物料ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'MaterialCode',title:'物料編碼',type:'string',link:true,width:120,require:true,align:'left'},
                       {field:'MaterialName',title:'物料名稱',type:'string',width:90,require:true,align:'left'},
                       {field:'Img',title:'物料圖片',type:'img',width:80,align:'left'},
                       {field:'CatalogID',title:'物料分類',type:'string',bind:{ key:'物料分類',data:[]},width:120,align:'left'},
                       {field:'MaterialType',title:'物料類型',type:'string',width:90,hidden:true,align:'left'},
                       {field:'Specification',title:'規格型號',type:'string',width:110,align:'left'},
                       {field:'Unit',title:'單位',type:'string',bind:{ key:'物料單位',data:[]},width:70,align:'left'},
                       {field:'Price',title:'單價',type:'decimal',width:70,hidden:true,align:'left'},
                       {field:'Remarks',title:'備註信息',type:'string',width:90,align:'left'},
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