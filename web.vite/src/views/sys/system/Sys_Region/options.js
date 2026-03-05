// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'id',
                editTable:true ,
        footer: "Foots",
        cnName: '省市區縣',
        name: 'Sys_Region',
        newTabEdit: false,
        url: "/Sys_Region/",
        sortName: "id"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"code":"","name":"","parentId":"","level":"","mername":"","Lng":"","Lat":"","pinyin":""};
    const editFormOptions = [[{"title":"編碼","field":"code"},
                               {"title":"名稱","field":"name"},
                               {"title":"上级編碼","field":"parentId","type":"number"},
                               {"title":"级别","field":"level","type":"number"},
                               {"title":"完整地址","field":"mername"},
                               {"title":"經度","field":"Lng","type":"decimal"},
                               {"title":"緯度","field":"Lat","type":"decimal"},
                               {"title":"拼音","field":"pinyin"}]];
    const searchFormFields = {"name":"","level":"","mername":"","pinyin":""};
    const searchFormOptions = [[{"title":"名稱","field":"name"},{"title":"级别","field":"level","type":"number"},{"title":"完整地址","field":"mername","type":"like"},{"title":"拼音","field":"pinyin","type":"like"}]];
    const columns = [{field:'id',title:'id',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'code',title:'編碼',type:'string',width:70,edit:{type:''},align:'left'},
                       {field:'name',title:'名稱',type:'string',width:100,edit:{type:''},align:'left'},
                       {field:'parentId',title:'上级編碼',type:'int',width:70,edit:{type:''},align:'left'},
                       {field:'level',title:'级别',type:'int',width:60,edit:{type:''},align:'left'},
                       {field:'mername',title:'完整地址',type:'string',width:170,edit:{type:''},align:'left'},
                       {field:'Lng',title:'經度',type:'float',width:70,edit:{type:''},align:'left'},
                       {field:'Lat',title:'緯度',type:'float',width:70,edit:{type:''},align:'left'},
                       {field:'pinyin',title:'拼音',type:'string',width:120,edit:{type:''},align:'left'}];
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