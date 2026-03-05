// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'req_id',
        footer: "Foots",
        cnName: '詢價單主檔',
        name: 'sbm_require_purchase',
        newTabEdit: false,
        url: "/sbm_require_purchase/",
        sortName: "name",
        fixedSearch:false
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"sale_id":"","end_user":"","partner_company":"","partner_contact":"","untax_amount":"","discount_amount":"","tax":"","tot_amount":"","description":""};
    const editFormOptions = [[{"dataKey":"SO訂單","data":[],"title":"SO單號","field":"sale_id","readonly":true,"colSize":33.333,"type":"select"},
                               {"dataKey":"夥伴公司名稱","data":[],"title":"終端客戶","field":"end_user","readonly":true,"colSize":30.0,"type":"select"},
                               {"dataKey":"夥伴公司名稱","data":[],"title":"廠商名稱","field":"partner_company","colSize":33.333,"type":"select"}],
                              [{"dataKey":"夥伴個人名稱","data":[],"title":"廠商聯絡人","field":"partner_contact","colSize":33.333,"type":"select"},
                               {"title":"未稅小計","field":"untax_amount","readonly":true,"colSize":33.333,"type":"decimal"},
                               {"title":"優惠小計","field":"discount_amount","colSize":33.333,"type":"decimal"}],
                              [{"title":"稅金","field":"tax","readonly":true,"colSize":33.333,"type":"decimal"},
                               {"title":"含稅合計","field":"tot_amount","readonly":true,"colSize":33.333,"type":"decimal"}],
                              [{"title":"備註","field":"description","colSize":60.0}]];
    const searchFormFields = {"name":"","partner_company":"","description":""};
    const searchFormOptions = [[{"title":"詢價單號","field":"name","colSize":11.111,"type":"like"},{"dataKey":"夥伴公司名稱","data":[],"title":"廠商名稱","field":"partner_company","colSize":12.5,"type":"select"},{"title":"備註","field":"description","colSize":25.0,"type":"like"}]];
    const columns = [{field:'req_id',title:'req_id',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'name',title:'詢價單號',type:'string',link:true,sort:true,width:110,align:'left'},
                       {field:'sale_id',title:'SO單號',type:'int',bind:{ key:'SO訂單',data:[]},sort:true,width:110,readonly:true,align:'left'},
                       {field:'end_user',title:'終端客戶',type:'int',bind:{ key:'夥伴公司名稱',data:[]},width:80,readonly:true,align:'left'},
                       {field:'partner_company',title:'廠商名稱',type:'int',bind:{ key:'夥伴公司名稱',data:[]},width:110,align:'left'},
                       {field:'partner_contact',title:'廠商聯絡人',type:'int',bind:{ key:'夥伴個人名稱',data:[]},width:110,align:'left'},
                       {field:'untax_amount',title:'未稅小計',type:'float',width:110,readonly:true,align:'left'},
                       {field:'discount_amount',title:'優惠小計',type:'float',width:110,align:'left'},
                       {field:'tax',title:'稅金',type:'float',width:110,readonly:true,align:'left'},
                       {field:'tot_amount',title:'含稅合計',type:'float',width:110,readonly:true,align:'left'},
                       {field:'description',title:'備註',type:'string',width:150,align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'}];
    const detail ={columns:[]};
    const details = [  { 
                    cnName: '詢價單明細檔',
                    table: 'sbm_require_purchase_line',
                    columns: [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'req_id',title:'主檔關聯',type:'int',width:110,hidden:true,align:'left'},
                       {field:'req_item',title:'項次',type:'string',link:true,sort:true,width:110,edit:{type:''},align:'left'},
                       {field:'prod_name',title:'產品',type:'string',width:150,edit:{type:''},align:'left'},
                       {field:'prod_num',title:'數量',type:'float',width:110,edit:{type:''},align:'left'},
                       {field:'prod_uom',title:'單位',type:'string',bind:{ key:'單位',data:[]},width:110,edit:{type:'select'},align:'left'},
                       {field:'price_unit',title:'單價',type:'float',width:110,edit:{type:''},align:'left'},
                       {field:'price_sub',title:'小計',type:'float',width:110,readonly:true,edit:{type:''},align:'left'},
                       {field:'prod_desc',title:'備註',type:'string',width:150,edit:{type:''},align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'}],
                    sortName: 'req_item',
                    key: 'id',
                    buttons:[],
                    delKeys:[],
                    detail:null
                                            },                    { 
                    cnName: '詢價單上傳檔案',
                    table: 'sbm_require_purchase_doc',
                    columns: [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'req_id',title:'主檔關聯',type:'int',width:110,hidden:true,align:'left'},
                       {field:'partner_company',title:'廠商名稱',type:'int',bind:{ key:'夥伴公司名稱',data:[]},link:true,width:110,edit:{type:'select'},align:'left'},
                       {field:'partner_contact',title:'廠商聯絡人',type:'int',bind:{ key:'夥伴個人名稱',data:[]},width:110,edit:{type:'select'},align:'left'},
                       {field:'name',title:'詢價案名',type:'string',width:150,edit:{type:''},align:'left'},
                       {field:'req_doc',title:'詢價文件檔',type:'file',width:150,edit:{type:'file'},align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'}],
                    sortName: 'partner_company',
                    key: 'id',
                    buttons:[],
                    delKeys:[],
                    detail:null
                                            }];

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