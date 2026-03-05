// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'ProductionLineID',
        footer: "Foots",
        cnName: '产線管理',
        name: 'MES_ProductionLine',
        newTabEdit: false,
        url: "/MES_ProductionLine/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"LineName":"","LineType":"","ResponsiblePerson":"","Location":"","StartDate":"","EndDate":"","Remarks":""};
    const editFormOptions = [[{"title":"产線名稱","required":true,"field":"LineName"},
                               {"title":"产線類型","field":"LineType"},
                               {"title":"负责人","field":"ResponsiblePerson"}],
                              [{"title":"产線位置","field":"Location"},
                               {"title":"啟用日期","field":"StartDate","type":"date"},
                               {"title":"停用日期","field":"EndDate","type":"date"}],
                              [{"title":"備註信息","field":"Remarks","colSize":12,"type":"textarea"}]];
    const searchFormFields = {"LineName":"","LineType":"","Capacity":"","Status":"","ResponsiblePerson":""};
    const searchFormOptions = [[{"title":"产線名稱","field":"LineName","type":"like"},{"title":"产線類型","field":"LineType"},{"title":"产能信息","field":"Capacity"},{"title":"产線狀態","field":"Status"},{"title":"负责人","field":"ResponsiblePerson"}]];
    const columns = [{field:'ProductionLineID',title:'产線ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'LineName',title:'产線名稱',type:'string',link:true,width:90,require:true,align:'left'},
                       {field:'LineType',title:'产線類型',type:'string',width:90,align:'left'},
                       {field:'Capacity',title:'产能信息',type:'string',width:90,hidden:true,align:'left'},
                       {field:'Status',title:'产線狀態',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ResponsiblePerson',title:'负责人',type:'string',width:90,align:'left'},
                       {field:'Location',title:'产線位置',type:'string',width:90,align:'left'},
                       {field:'StartDate',title:'啟用日期',type:'date',width:90,align:'left'},
                       {field:'EndDate',title:'停用日期',type:'date',width:90,align:'left'},
                       {field:'Remarks',title:'備註信息',type:'string',width:90,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,hidden:true,align:'left'}];
    const detail =  {
                    cnName: '产線設備',
                    table: 'MES_ProductionLineDevice',
                    columns: [{field:'DeviceID',title:'設備ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'ProductionLineID',title:'产線ID',type:'string',width:90,hidden:true,align:'left'},
                       {field:'DeviceName',title:'設備名稱',type:'string',width:90,edit:{type:''},require:true,align:'left'},
                       {field:'DeviceModel',title:'設備型號',type:'string',width:90,edit:{type:''},align:'left'},
                       {field:'Manufacturer',title:'制造商',type:'string',width:90,edit:{type:''},align:'left'},
                       {field:'PurchaseDate',title:'购买日期',type:'date',width:90,edit:{type:'date'},align:'left'},
                       {field:'WarrantyDate',title:'保修日期',type:'date',width:90,edit:{type:'date'},align:'left'},
                       {field:'Status',title:'設備狀態',type:'string',width:90,edit:{type:''},align:'left'},
                       {field:'LocationOnLine',title:'線上位置',type:'string',width:90,hidden:true,edit:{type:''},align:'left'},
                       {field:'Remarks',title:'備註信息',type:'string',width:90,edit:{type:''},align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:120,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,hidden:true,align:'left'}],
                    sortName: 'CreateDate',
                    key: 'DeviceID'
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