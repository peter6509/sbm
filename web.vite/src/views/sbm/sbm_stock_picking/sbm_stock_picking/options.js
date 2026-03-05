// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'picking_id',
        footer: "Foots",
        cnName: '調撥單主檔',
        name: 'sbm_stock_picking',
        newTabEdit: false,
        url: "/sbm_stock_picking/",
        sortName: "name",
        fixedSearch:false
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"name":"","origin":"","company_id":"","picking_date":"","picking_type":"","partner_company":"","partner_contact":""};
    const editFormOptions = [[{"title":"調撥單單號","field":"name","readonly":true,"colSize":33.333},
                               {"title":"來源單號","field":"origin","readonly":true,"colSize":33.333},
                               {"dataKey":"成本中心","data":[],"title":"事業單位","required":true,"field":"company_id","colSize":33.333,"type":"select"}],
                              [{"dataKey":"調撥類型","data":[],"title":"調撥類型","required":true,"field":"picking_type","colSize":33.333,"type":"select"},
                               {"title":"調撥日期","required":true,"field":"picking_date","colSize":33.333,"type":"date"},
                               {"dataKey":"夥伴公司(客戶)","data":[],"title":"合作夥伴公司","required":true,"field":"partner_company","colSize":33.333,"type":"select"}],
                              [{"dataKey":"夥伴個人名稱","data":[],"title":"夥伴聯絡人","field":"partner_contact","colSize":33.333,"type":"select"}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'picking_id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'name',title:'調撥單單號',type:'string',link:true,width:120,readonly:true,align:'left'},
                       {field:'picking_date',title:'調撥日期',type:'date',width:90,require:true,align:'left'},
                       {field:'picking_type',title:'調撥類型',type:'int',bind:{ key:'調撥類型',data:[]},width:60,require:true,align:'left'},
                       {field:'origin',title:'來源單號',type:'string',width:90,readonly:true,align:'left'},
                       {field:'location_source',title:'來源位置',type:'string',width:180,hidden:true,align:'left'},
                       {field:'location_destination',title:'目的位置',type:'string',width:180,hidden:true,align:'left'},
                       {field:'picking_owner',title:'作業人員',type:'int',bind:{ key:'使用者',data:[]},width:90,align:'left'},
                       {field:'company_id',title:'事業單位',type:'int',bind:{ key:'成本中心',data:[]},width:80,require:true,align:'left'},
                       {field:'partner_company',title:'合作夥伴公司',type:'int',bind:{ key:'夥伴公司(客戶)',data:[]},width:120,require:true,align:'left'},
                       {field:'partner_contact',title:'夥伴聯絡人',type:'int',bind:{ key:'夥伴個人名稱',data:[]},width:100,align:'left'},
                       {field:'bu_logo',title:'LOGO',type:'img',width:180,hidden:true,align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'}];
    const detail =  {
                    cnName: '調撥單明細',
                    table: 'sbm_stockmove',
                    columns: [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'picking_id',title:'主檔關聯',type:'int',width:110,hidden:true,align:'left'},
                       {field:'move_item',title:'項次',type:'string',link:true,width:80,align:'left'},
                       {field:'prod_name',title:'產品',type:'string',width:200,align:'left'},
                       {field:'prod_num',title:'數量',type:'float',width:90,align:'left'},
                       {field:'prod_uom',title:'單位',type:'string',bind:{ key:'單位',data:[]},width:90,align:'left'},
                       {field:'price_unit',title:'單價',type:'float',width:100,align:'left'},
                       {field:'price_sub',title:'小計',type:'float',width:110,align:'left'},
                       {field:'prod_desc',title:'備註',type:'string',width:150,align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'}],
                    sortName: 'move_item',
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