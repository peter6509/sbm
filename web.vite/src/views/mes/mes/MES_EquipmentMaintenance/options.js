// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'MaintenanceID',
        footer: "Foots",
        cnName: '設備保养',
        name: 'MES_EquipmentMaintenance',
        newTabEdit: false,
        url: "/MES_EquipmentMaintenance/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"EquipmentID":"","MaintenanceDate":"","MaintenanceType":"","MaintenanceContent":"","MaintenancePerson":"","MaintenanceStatus":"","NextMaintenanceDate":"","MaintenanceCost":"","MaintenanceRemarks":""};
    const editFormOptions = [[{"dataKey":"設備列表","data":[],"title":"設備信息","field":"EquipmentID","type":"select"},
                               {"title":"保养日期","field":"MaintenanceDate"},
                               {"title":"保养類型","field":"MaintenanceType"}],
                              [{"title":"保养内容","field":"MaintenanceContent"},
                               {"title":"保养人员","field":"MaintenancePerson"},
                               {"title":"保养狀態","field":"MaintenanceStatus"}],
                              [{"title":"下次保养日期","field":"NextMaintenanceDate"},
                               {"title":"保养成本","field":"MaintenanceCost","type":"decimal"},
                               {"title":"保养備註","field":"MaintenanceRemarks"}]];
    const searchFormFields = {"EquipmentID":"","MaintenanceDate":"","MaintenanceType":"","MaintenanceContent":"","MaintenancePerson":""};
    const searchFormOptions = [[{"dataKey":"設備列表","data":[],"title":"設備信息","field":"EquipmentID","type":"select"},{"title":"保养日期","field":"MaintenanceDate","type":"datetime"},{"title":"保养類型","field":"MaintenanceType"},{"title":"保养内容","field":"MaintenanceContent"},{"title":"保养人员","field":"MaintenancePerson"}]];
    const columns = [{field:'MaintenanceID',title:'保养ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'EquipmentID',title:'設備信息',type:'string',bind:{ key:'設備列表',data:[]},width:90,align:'left'},
                       {field:'MaintenanceDate',title:'保养日期',type:'datetime',width:90,align:'left'},
                       {field:'MaintenanceType',title:'保养類型',type:'string',width:90,align:'left'},
                       {field:'MaintenanceContent',title:'保养内容',type:'string',link:true,width:90,align:'left'},
                       {field:'MaintenancePerson',title:'保养人员',type:'string',width:90,align:'left'},
                       {field:'MaintenanceStatus',title:'保养狀態',type:'string',width:90,align:'left'},
                       {field:'NextMaintenanceDate',title:'下次保养日期',type:'datetime',width:90,align:'left'},
                       {field:'MaintenanceCost',title:'保养成本',type:'decimal',width:90,align:'left'},
                       {field:'MaintenanceRemarks',title:'保养備註',type:'string',width:90,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,align:'left'},
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