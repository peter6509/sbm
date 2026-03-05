// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'RepairID',
        footer: "Foots",
        cnName: '設備維修',
        name: 'MES_EquipmentRepair',
        newTabEdit: false,
        url: "/MES_EquipmentRepair/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"EquipmentID":"","RepairDate":"","RepairReason":"","RepairContent":"","RepairPerson":"","RepairCost":"","RepairStatus":"","RepairStartTime":"","RepairEndTime":""};
    const editFormOptions = [[{"dataKey":"設備列表","data":[],"title":"設備名稱","field":"EquipmentID","type":"select"},
                               {"title":"維修日期","field":"RepairDate","type":"datetime"},
                               {"title":"維修原因","field":"RepairReason"}],
                              [{"title":"維修内容","field":"RepairContent"},
                               {"title":"維修人员","field":"RepairPerson"},
                               {"title":"維修成本","field":"RepairCost","type":"decimal"}],
                              [{"title":"維修狀態","field":"RepairStatus"},
                               {"title":"維修開始時間","field":"RepairStartTime","type":"datetime"},
                               {"title":"維修结束時間","field":"RepairEndTime","type":"datetime"}]];
    const searchFormFields = {"EquipmentID":"","RepairDate":"","RepairReason":"","RepairContent":"","RepairPerson":""};
    const searchFormOptions = [[{"dataKey":"設備列表","data":[],"title":"設備名稱","field":"EquipmentID","type":"select"},{"title":"維修日期","field":"RepairDate","type":"datetime"},{"title":"維修原因","field":"RepairReason"},{"title":"維修内容","field":"RepairContent"},{"title":"維修人员","field":"RepairPerson"}]];
    const columns = [{field:'RepairID',title:'維修ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'EquipmentID',title:'設備名稱',type:'string',bind:{ key:'設備列表',data:[]},width:90,align:'left'},
                       {field:'RepairDate',title:'維修日期',type:'datetime',width:120,align:'left'},
                       {field:'RepairReason',title:'維修原因',type:'string',link:true,width:90,align:'left'},
                       {field:'RepairContent',title:'維修内容',type:'string',width:90,align:'left'},
                       {field:'RepairPerson',title:'維修人员',type:'string',width:70,align:'left'},
                       {field:'RepairCost',title:'維修成本',type:'decimal',width:70,align:'left'},
                       {field:'RepairStatus',title:'維修狀態',type:'string',width:70,align:'left'},
                       {field:'RepairStartTime',title:'維修開始時間',type:'datetime',width:90,hidden:true,align:'left'},
                       {field:'RepairEndTime',title:'維修结束時間',type:'datetime',width:90,align:'left'},
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