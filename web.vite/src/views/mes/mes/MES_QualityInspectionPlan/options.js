// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'InspectionPlanID',
        footer: "Foots",
        cnName: '质量檢驗',
        name: 'MES_QualityInspectionPlan',
        newTabEdit: false,
        url: "/MES_QualityInspectionPlan/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"InspectionPlanNumber":"","PlanStartTime":"","PlanEndTime":"","ResponsiblePerson":""};
    const editFormOptions = [[{"title":"檢驗單號","required":true,"field":"InspectionPlanNumber"},
                               {"title":"檢驗開始時間","required":true,"field":"PlanStartTime","type":"datetime"},
                               {"title":"檢驗结束時間","required":true,"field":"PlanEndTime","type":"datetime"},
                               {"dataKey":"用户列表","data":[],"title":"檢驗人","required":true,"field":"ResponsiblePerson","type":"select"}]];
    const searchFormFields = {"InspectionPlanNumber":"","PlanStartTime":"","PlanEndTime":"","ResponsiblePerson":""};
    const searchFormOptions = [[{"title":"檢驗單號","field":"InspectionPlanNumber"},{"title":"檢驗開始時間","field":"PlanStartTime","type":"datetime"},{"title":"檢驗结束時間","field":"PlanEndTime","type":"datetime"},{"dataKey":"用户列表","data":[],"title":"檢驗人","field":"ResponsiblePerson"}]];
    const columns = [{field:'InspectionPlanID',title:'檢驗計划ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'InspectionPlanNumber',title:'檢驗單號',type:'string',link:true,width:130,require:true,align:'left'},
                       {field:'OrderID',title:'訂單ID',type:'string',width:90,hidden:true,align:'left'},
                       {field:'PlanStartTime',title:'檢驗開始時間',type:'datetime',width:140,require:true,align:'left'},
                       {field:'PlanEndTime',title:'檢驗结束時間',type:'datetime',width:140,require:true,align:'left'},
                       {field:'ResponsiblePerson',title:'檢驗人',type:'string',bind:{ key:'用户列表',data:[]},width:90,require:true,align:'left'},
                       {field:'PlanStatus',title:'計划狀態',type:'string',width:90,hidden:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:140,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:140,align:'left'}];
    const detail =  {
                    cnName: '质檢明细',
                    table: 'MES_QualityInspectionPlanDetail',
                    columns: [{field:'InspectionPlanDetailID',title:'明细ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'InspectionPlanID',title:'檢驗id',type:'string',width:90,hidden:true,align:'left'},
                       {field:'MaterialCode',title:'檢驗物料',type:'string',bind:{ key:'物料列表',data:[]},width:90,edit:{type:'select'},require:true,align:'left'},
                       {field:'MaterialName',title:'物料名稱',type:'string',width:90,hidden:true,edit:{type:''},align:'left'},
                       {field:'MaterialSpecification',title:'物料規格',type:'string',width:90,edit:{type:''},require:true,align:'left'},
                       {field:'QuantityToInspect',title:'檢驗數量',type:'int',width:90,edit:{type:''},require:true,align:'left'},
                       {field:'InspectionMethod',title:'檢驗方法',type:'string',width:90,edit:{type:''},require:true,align:'left'},
                       {field:'InspectionStandard',title:'檢驗標準',type:'string',width:90,edit:{type:''},require:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,hidden:true,align:'left'}],
                    sortName: 'CreateDate',
                    key: 'InspectionPlanDetailID'
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