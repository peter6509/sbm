// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'InspectionRecordID',
        footer: "Foots",
        cnName: '质檢記錄',
        name: 'MES_QualityInspectionRecord',
        newTabEdit: false,
        url: "/MES_QualityInspectionRecord/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"InspectionNumber":"","Inspector":"","InspectionTime":"","InspectedQuantity":"","PassedQuantity":"","FailedQuantity":"","InspectionResult":"","Creator":"","CreateDate":"","DefectDescription":""};
    const editFormOptions = [[{"title":"檢驗單號","required":true,"field":"InspectionNumber"},
                               {"dataKey":"用户列表","data":[],"title":"檢驗员","required":true,"field":"Inspector","type":"select"},
                               {"title":"檢驗時間","required":true,"field":"InspectionTime","type":"datetime"}],
                              [{"title":"實際檢驗數量","required":true,"field":"InspectedQuantity","type":"number"},
                               {"title":"合格數量","required":true,"field":"PassedQuantity","type":"number"},
                               {"title":"不合格數量","required":true,"field":"FailedQuantity","type":"number"}],
                              [{"title":"檢驗结果","required":true,"field":"InspectionResult"},
                               {"title":"創建人","field":"Creator","disabled":true},
                               {"title":"創建時間","field":"CreateDate","disabled":true}],
                              [{"title":"缺陷描述","field":"DefectDescription","colSize":12,"type":"textarea"}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'InspectionRecordID',title:'檢驗記錄ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'InspectionPlanDetailID',title:'檢驗計划明细ID',type:'string',width:90,hidden:true,align:'left'},
                       {field:'InspectionNumber',title:'檢驗單號',type:'string',link:true,width:80,require:true,align:'left'},
                       {field:'Inspector',title:'檢驗员',type:'string',bind:{ key:'用户列表',data:[]},width:70,require:true,align:'left'},
                       {field:'InspectionTime',title:'檢驗時間',type:'datetime',sort:true,width:140,require:true,align:'left'},
                       {field:'InspectedQuantity',title:'實際檢驗數量',type:'int',width:90,require:true,align:'left'},
                       {field:'PassedQuantity',title:'合格數量',type:'int',sort:true,width:90,require:true,align:'left'},
                       {field:'FailedQuantity',title:'不合格數量',type:'int',sort:true,width:90,require:true,align:'left'},
                       {field:'InspectionResult',title:'檢驗结果',type:'string',width:90,require:true,align:'left'},
                       {field:'DefectDescription',title:'缺陷描述',type:'string',width:90,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,readonly:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,readonly:true,align:'left'},
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