// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'EquipmentID',
        footer: "Foots",
        cnName: '設備管理',
        name: 'MES_EquipmentManagement',
        newTabEdit: false,
        url: "/MES_EquipmentManagement/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"EquipmentCode":"","EquipmentName":"","Manufacturer":"","PurchaseDate":"","EquipmentStatus":"","ResponsiblePerson":""};
    const editFormOptions = [[{"title":"設備編碼","required":true,"field":"EquipmentCode"}],
                              [{"title":"設備名稱","required":true,"field":"EquipmentName"}],
                              [{"title":"制造商","field":"Manufacturer"}],
                              [{"title":"购买日期","field":"PurchaseDate","type":"date"}],
                              [{"title":"設備狀態","field":"EquipmentStatus"}],
                              [{"title":"责任人","field":"ResponsiblePerson"}]];
    const searchFormFields = {"EquipmentCode":"","EquipmentName":"","Manufacturer":"","PurchaseDate":"","EquipmentStatus":""};
    const searchFormOptions = [[{"title":"設備編碼","field":"EquipmentCode","type":"like"},{"title":"設備名稱","field":"EquipmentName","type":"like"},{"title":"制造商","field":"Manufacturer"},{"title":"购买日期","field":"PurchaseDate","type":"date"},{"title":"設備狀態","field":"EquipmentStatus"}]];
    const columns = [{field:'EquipmentID',title:'設備ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'EquipmentCode',title:'設備編碼',type:'string',link:true,width:90,require:true,align:'left'},
                       {field:'EquipmentName',title:'設備名稱',type:'string',width:90,require:true,align:'left'},
                       {field:'EquipmentType',title:'設備類型',type:'string',width:90,hidden:true,align:'left'},
                       {field:'Manufacturer',title:'制造商',type:'string',width:90,align:'left'},
                       {field:'PurchaseDate',title:'购买日期',type:'date',width:90,align:'left'},
                       {field:'WarrantyPeriod',title:'保修期',type:'int',width:90,align:'left'},
                       {field:'InstallationLocation',title:'安装位置',type:'string',width:90,align:'left'},
                       {field:'EquipmentStatus',title:'設備狀態',type:'string',width:90,align:'left'},
                       {field:'ResponsiblePerson',title:'责任人',type:'string',width:90,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:140,align:'left'},
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