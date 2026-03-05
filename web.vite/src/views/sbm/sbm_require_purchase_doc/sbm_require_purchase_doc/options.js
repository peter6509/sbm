// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'id',
        footer: "Foots",
        cnName: '詢價單上傳檔案',
        name: 'sbm_require_purchase_doc',
        newTabEdit: false,
        url: "/sbm_require_purchase_doc/",
        sortName: "partner_company",
        fixedSearch:false
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"partner_company":"","partner_contact":"","name":"","req_doc":""};
    const editFormOptions = [[{"dataKey":"夥伴公司名稱","data":[],"title":"廠商名稱","field":"partner_company","colSize":12.5,"type":"select"},
                               {"dataKey":"夥伴個人名稱","data":[],"title":"廠商聯絡人","field":"partner_contact","colSize":9.999,"type":"select"},
                               {"title":"詢價案名","field":"name","colSize":14.285},
                               {"title":"詢價文件檔","field":"req_doc","colSize":16.666,"type":"file"}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'req_id',title:'主檔關聯',type:'int',width:110,hidden:true,align:'left'},
                       {field:'partner_company',title:'廠商名稱',type:'int',bind:{ key:'夥伴公司名稱',data:[]},link:true,width:110,align:'left'},
                       {field:'partner_contact',title:'廠商聯絡人',type:'int',bind:{ key:'夥伴個人名稱',data:[]},width:110,align:'left'},
                       {field:'name',title:'詢價案名',type:'string',width:150,align:'left'},
                       {field:'req_doc',title:'詢價文件檔',type:'file',width:150,align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'}];
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