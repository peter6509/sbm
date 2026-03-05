// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'sale_id',
        footer: "Foots",
        cnName: '報價單主檔',
        name: 'sbm_sale_order',
        newTabEdit: false,
        url: "/sbm_sale_order/",
        sortName: "name",
        fixedSearch:false
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"name":"","sale_date":"","company_id":"","sales":"","sales_mobile":"","sales_email":"","partner_company":"","partner_contact":"","partner_mobile":"","untax_amount":"","discount_amount":"","partner_email":"","tax":"","tot_amount":"","description":""};
    const editFormOptions = [[{"dataKey":"成本中心","data":[],"title":"成本中心","field":"company_id","colSize":9.999,"type":"select"},
                               {"title":"報價單號","field":"name","readonly":true,"colSize":33.333,"type":"like"},
                               {"title":"報價日期","field":"sale_date","colSize":33.333,"type":"date"}],
                              [{"dataKey":"使用者","data":[],"title":"業務員","field":"sales","colSize":33.333,"type":"select"},
                               {"title":"業務員手機","field":"sales_mobile","colSize":25.0},
                               {"title":"業務EMAIL","field":"sales_email","colSize":30.0}],
                              [{"dataKey":"夥伴公司名稱","data":[],"title":"客戶名稱","field":"partner_company","colSize":33.333,"type":"select"},
                               {"dataKey":"夥伴個人名稱","data":[],"title":"客戶聯絡人","field":"partner_contact","colSize":33.333,"type":"select"},
                               {"title":"聯絡人電話","field":"partner_mobile","colSize":25.0}],
                              [{"title":"客戶EMAIL","field":"partner_email","colSize":30.0},
                               {"title":"未稅小計","field":"untax_amount","readonly":true,"colSize":33.333,"type":"decimal"},
                               {"title":"優惠小計","field":"discount_amount","colSize":33.333,"type":"decimal"}],
                              [{"title":"稅金","field":"tax","readonly":true,"colSize":33.333,"type":"decimal"},
                               {"title":"含稅合計","field":"tot_amount","readonly":true,"colSize":33.333,"type":"decimal"},
                               {"title":"備註","field":"description","colSize":33.333,"type":"like"}]];
    const searchFormFields = {"name":"","sale_date":"","sales":"","partner_company":"","partner_contact":"","description":""};
    const searchFormOptions = [[{"title":"報價單號","field":"name","colSize":11.111,"type":"like"},{"title":"報價日期","field":"sale_date","colSize":11.111,"type":"date"},{"dataKey":"使用者","data":[],"title":"業務員","field":"sales","colSize":16.666,"type":"select"},{"dataKey":"夥伴公司名稱","data":[],"title":"客戶名稱","field":"partner_company","colSize":25.0,"type":"select"},{"dataKey":"夥伴個人名稱","data":[],"title":"客戶聯絡人","field":"partner_contact","colSize":20.0,"type":"select"}],[{"title":"備註","field":"description","colSize":60.0,"type":"like"}]];
    const columns = [{field:'sale_id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'name',title:'報價單號',type:'string',link:true,width:150,readonly:true,align:'left'},
                       {field:'sale_date',title:'報價日期',type:'date',width:120,align:'left'},
                       {field:'sales',title:'業務員',type:'int',bind:{ key:'使用者',data:[]},width:80,align:'left'},
                       {field:'partner_company',title:'客戶名稱',type:'int',bind:{ key:'夥伴公司名稱',data:[]},width:200,align:'left'},
                       {field:'partner_contact',title:'客戶聯絡人',type:'int',bind:{ key:'夥伴個人名稱',data:[]},width:90,align:'left'},
                       {field:'untax_amount',title:'未稅小計',type:'float',width:100,readonly:true,align:'left'},
                       {field:'discount_amount',title:'優惠小計',type:'float',width:100,align:'left'},
                       {field:'tax',title:'稅金',type:'float',width:90,readonly:true,align:'left'},
                       {field:'tot_amount',title:'含稅合計',type:'float',width:110,readonly:true,align:'left'},
                       {field:'description',title:'備註',type:'string',width:110,align:'left'},
                       {field:'CreateDate',title:'製表日期',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'partner_mobile',title:'聯絡人電話',type:'string',width:110,hidden:true,align:'left'},
                       {field:'sales_mobile',title:'業務員手機',type:'string',width:110,hidden:true,align:'left'},
                       {field:'partner_email',title:'客戶EMAIL',type:'string',width:120,hidden:true,align:'left'},
                       {field:'sales_email',title:'業務EMAIL',type:'string',width:120,hidden:true,align:'left'},
                       {field:'company_id',title:'成本中心',type:'int',bind:{ key:'成本中心',data:[]},width:80,align:'left'},
                       {field:'bu_logo',title:'LOGO',type:'img',width:110,hidden:true,align:'left'},
                       {field:'bu_invoice_stamp',title:'發票章',type:'img',width:110,hidden:true,align:'left'},
                       {field:'bu_bank_info',title:'匯款資訊',type:'string',width:220,hidden:true,align:'left'}];
    const detail =  {
                    cnName: '報價單明細檔',
                    table: 'sbm_sale_order_line',
                    columns: [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'sale_id',title:'主檔關聯',type:'int',width:110,hidden:true,align:'left'},
                       {field:'parent_id',title:'上階ID',type:'int',width:110,hidden:true,align:'left'},
                       {field:'sale_item',title:'項次',type:'string',link:true,sort:true,width:60,edit:{type:''},align:'left'},
                       {field:'prod_name',title:'產品',type:'string',width:350,edit:{type:''},align:'left'},
                       {field:'prod_num',title:'數量',type:'float',width:60,edit:{type:''},align:'left'},
                       {field:'prod_uom',title:'單位',type:'string',bind:{ key:'單位',data:[]},width:60,edit:{type:'select'},align:'left'},
                       {field:'price_unit',title:'單價',type:'float',width:80,edit:{type:''},align:'left'},
                       {field:'price_sub',title:'小計',type:'float',width:90,edit:{type:''},align:'left'},
                       {field:'prod_desc',title:'備註',type:'string',width:150,edit:{type:''},align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'}],
                    sortName: 'sale_item',
                    key: 'id'
                                            };
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