// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'id',
        footer: "Foots",
        cnName: '出貨單創建',
        name: 'sbm_stockoutopen',
        newTabEdit: false,
        url: "/sbm_stockoutopen/",
        sortName: "so_id",
        fixedSearch:false
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"so_id":"","picking_date":""};
    const editFormOptions = [[{"dataKey":"業務報價單","data":[],"title":"報價單號","required":true,"field":"so_id","colSize":30.0,"type":"select"},
                               {"title":"調撥日期","field":"picking_date","colSize":25.0,"type":"date"}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'so_id',title:'報價單號',type:'int',bind:{ key:'業務報價單',data:[]},link:true,width:110,require:true,align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'picking_date',title:'調撥日期',type:'date',width:150,align:'left'}];
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