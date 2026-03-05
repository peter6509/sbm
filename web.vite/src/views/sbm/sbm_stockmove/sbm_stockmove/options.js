// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'id',
                editTable:true ,
        footer: "Foots",
        cnName: '調撥單明細',
        name: 'sbm_stockmove',
        newTabEdit: false,
        url: "/sbm_stockmove/",
        sortName: "move_item",
        fixedSearch:false
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"move_item":"","prod_name":"","prod_num":"","prod_uom":"","price_unit":"","price_sub":"","prod_desc":""};
    const editFormOptions = [[{"title":"項次","field":"move_item","colSize":9.999},
                               {"title":"產品","field":"prod_name","colSize":14.285},
                               {"title":"數量","field":"prod_num","colSize":9.999,"type":"decimal"},
                               {"dataKey":"單位","data":[],"title":"單位","field":"prod_uom","colSize":9.999,"type":"select"},
                               {"title":"單價","field":"price_unit","colSize":9.999,"type":"decimal"},
                               {"title":"小計","field":"price_sub","readonly":true,"colSize":11.111,"type":"decimal"},
                               {"title":"備註","field":"prod_desc","colSize":25.0}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'picking_id',title:'主檔關聯',type:'int',width:110,hidden:true,align:'left'},
                       {field:'move_item',title:'項次',type:'string',link:true,width:80,edit:{type:''},align:'left'},
                       {field:'prod_name',title:'產品',type:'string',width:200,edit:{type:''},align:'left'},
                       {field:'prod_num',title:'數量',type:'float',width:90,edit:{type:''},align:'left'},
                       {field:'prod_uom',title:'單位',type:'string',bind:{ key:'單位',data:[]},width:90,edit:{type:'select'},align:'left'},
                       {field:'price_unit',title:'單價',type:'float',width:100,edit:{type:''},align:'left'},
                       {field:'price_sub',title:'小計',type:'float',width:110,readonly:true,edit:{type:''},align:'left'},
                       {field:'prod_desc',title:'備註',type:'string',width:150,edit:{type:''},align:'left'},
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