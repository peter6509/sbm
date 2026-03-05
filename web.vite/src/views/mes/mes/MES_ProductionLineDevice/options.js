// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'DeviceID',
        footer: "Foots",
        cnName: '产線設備',
        name: 'MES_ProductionLineDevice',
        newTabEdit: false,
        url: "/MES_ProductionLineDevice/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"DeviceName":"","DeviceModel":"","Manufacturer":"","PurchaseDate":"","WarrantyDate":"","Status":"","LocationOnLine":"","Remarks":""};
    const editFormOptions = [[{"title":"設備名稱","required":true,"field":"DeviceName"},
                               {"title":"設備型號","field":"DeviceModel"},
                               {"title":"制造商","field":"Manufacturer"},
                               {"title":"购买日期","field":"PurchaseDate","type":"date"},
                               {"title":"保修日期","field":"WarrantyDate","type":"date"},
                               {"title":"設備狀態","field":"Status"},
                               {"title":"線上位置","field":"LocationOnLine"},
                               {"title":"備註信息","field":"Remarks"}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'DeviceID',title:'設備ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'ProductionLineID',title:'产線ID',type:'string',width:90,hidden:true,align:'left'},
                       {field:'DeviceName',title:'設備名稱',type:'string',width:90,require:true,align:'left'},
                       {field:'DeviceModel',title:'設備型號',type:'string',width:90,align:'left'},
                       {field:'Manufacturer',title:'制造商',type:'string',width:90,align:'left'},
                       {field:'PurchaseDate',title:'购买日期',type:'date',width:90,align:'left'},
                       {field:'WarrantyDate',title:'保修日期',type:'date',width:90,align:'left'},
                       {field:'Status',title:'設備狀態',type:'string',width:90,align:'left'},
                       {field:'LocationOnLine',title:'線上位置',type:'string',width:90,hidden:true,align:'left'},
                       {field:'Remarks',title:'備註信息',type:'string',width:90,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:120,align:'left'},
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