// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'id',
        footer: "Foots",
        cnName: '詢價開案',
        name: 'sbm_reqopen',
        newTabEdit: false,
        url: "/sbm_reqopen/",
        sortName: "so_id",
        fixedSearch:false
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"so_id":"","partner_name":"","partner_contact":""};
    const editFormOptions = [[{"dataKey":"業務報價單","data":[],"title":"報價單號","field":"so_id","colSize":30.0,"type":"select"},
                               {"dataKey":"夥伴公司(供應商)","data":[],"title":"報價廠商","field":"partner_name","colSize":30.0,"type":"select"},
                               {"dataKey":"夥伴個人名稱","data":[],"title":"廠商代表","field":"partner_contact","colSize":30.0,"type":"select"}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'id',title:'ID',type:'int',width:110,require:true,align:'left'},
                       {field:'so_id',title:'報價單號',type:'int',bind:{ key:'業務報價單',data:[]},link:true,width:110,align:'left'},
                       {field:'partner_name',title:'報價廠商',type:'int',bind:{ key:'夥伴公司(供應商)',data:[]},width:110,align:'left'},
                       {field:'partner_contact',title:'廠商代表',type:'int',bind:{ key:'夥伴個人名稱',data:[]},width:110,align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,align:'left'}];
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