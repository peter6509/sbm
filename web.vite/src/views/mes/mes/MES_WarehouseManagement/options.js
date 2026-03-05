// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'WarehouseID',
        footer: "Foots",
        cnName: '仓庫管理',
        name: 'MES_WarehouseManagement',
        newTabEdit: false,
        url: "/MES_WarehouseManagement/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"WarehouseCode":"","WarehouseName":"","WarehouseType":"","WarehouseArea":"","WarehouseAddress":"","WarehousePhone":"","WarehouseStatus":"","WarehouseCapacity":""};
    const editFormOptions = [[{"title":"仓庫編碼","required":true,"field":"WarehouseCode"},
                               {"title":"仓庫名稱","required":true,"field":"WarehouseName"},
                               {"dataKey":"仓庫類型","data":[],"title":"仓庫類型","field":"WarehouseType","type":"select"},
                               {"title":"仓庫面积","required":true,"field":"WarehouseArea","type":"decimal"}],
                              [{"title":"仓庫地址","field":"WarehouseAddress"},
                               {"title":"仓庫电话","field":"WarehousePhone"},
                               {"dataKey":"enable","data":[],"title":"啟用狀態","field":"WarehouseStatus","type":"radio"},
                               {"title":"仓庫容量","required":true,"field":"WarehouseCapacity","type":"number"}]];
    const searchFormFields = {"WarehouseCode":"","WarehouseName":"","WarehouseType":"","WarehouseAddress":"","WarehousePhone":""};
    const searchFormOptions = [[{"title":"仓庫編碼","field":"WarehouseCode","type":"like"},{"title":"仓庫名稱","field":"WarehouseName","type":"like"},{"dataKey":"仓庫類型","data":[],"title":"仓庫類型","field":"WarehouseType","type":"select"},{"title":"仓庫地址","field":"WarehouseAddress"},{"title":"仓庫电话","field":"WarehousePhone"}]];
    const columns = [{field:'WarehouseID',title:'仓庫ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'WarehouseCode',title:'仓庫編碼',type:'string',link:true,width:90,require:true,align:'left'},
                       {field:'WarehouseName',title:'仓庫名稱',type:'string',width:90,require:true,align:'left'},
                       {field:'WarehouseType',title:'仓庫類型',type:'string',bind:{ key:'仓庫類型',data:[]},width:90,align:'left'},
                       {field:'WarehouseArea',title:'仓庫面积',type:'decimal',sort:true,width:90,require:true,align:'left'},
                       {field:'WarehouseAddress',title:'仓庫地址',type:'string',width:90,align:'left'},
                       {field:'WarehousePhone',title:'仓庫电话',type:'string',width:90,align:'left'},
                       {field:'WarehouseManager',title:'仓庫管理员',type:'string',width:90,hidden:true,align:'left'},
                       {field:'WarehouseStatus',title:'啟用狀態',type:'string',bind:{ key:'enable',data:[]},width:60,align:'left'},
                       {field:'WarehouseCapacity',title:'仓庫容量',type:'int',sort:true,width:90,require:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:70,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:130,align:'left'},
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