// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'BomId',
        footer: "Foots",
        cnName: '制造BOM',
        name: 'MES_Bom_Main',
        newTabEdit: false,
        url: "/MES_Bom_Main/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"Code":"","MaterialCode":"","MaterialName":"","Spec":"","Purpose":"","Edition":"","EffectiveDate":"","InvalidDate":""};
    const editFormOptions = [[{"title":"BOM編號","required":true,"field":"Code"},
                               {"title":"母件物料編碼","field":"MaterialCode"},
                               {"title":"母件物料名稱","field":"MaterialName"},
                               {"title":"規格型號","field":"Spec"}],
                              [{"title":"用途","field":"Purpose"},
                               {"title":"版本","field":"Edition"},
                               {"title":"有效日期","required":true,"field":"EffectiveDate","type":"date"},
                               {"title":"失效日期","required":true,"field":"InvalidDate","type":"date"}]];
    const searchFormFields = {"Code":"","MaterialCode":"","MaterialName":"","Purpose":"","EffectiveDate":""};
    const searchFormOptions = [[{"title":"BOM編號","field":"Code","type":"like"},{"title":"母件物料編碼","field":"MaterialCode","type":"like"},{"title":"母件物料名稱","field":"MaterialName","type":"like"},{"title":"用途","field":"Purpose"},{"title":"有效日期","field":"EffectiveDate","type":"date"}]];
    const columns = [{field:'BomId',title:'ID',type:'string',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Code',title:'BOM編號',type:'string',link:true,sort:true,width:110,require:true,align:'left'},
                       {field:'MaterialCode',title:'母件物料編碼',type:'string',width:120,align:'left'},
                       {field:'MaterialName',title:'母件物料名稱',type:'string',width:130,align:'left'},
                       {field:'Spec',title:'規格型號',type:'string',width:70,align:'left'},
                       {field:'Purpose',title:'用途',type:'string',width:70,align:'left'},
                       {field:'Edition',title:'版本',type:'string',width:70,align:'left'},
                       {field:'EffectiveDate',title:'有效日期',type:'date',sort:true,width:90,require:true,align:'left'},
                       {field:'InvalidDate',title:'失效日期',type:'date',sort:true,width:90,require:true,align:'left'},
                       {field:'Enable',title:'啟用狀態',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'CreateID',title:'創建人',type:'int',width:70,hidden:true,require:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,require:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:110,hidden:true,require:true,align:'left'},
                       {field:'ModifyID',title:'更新人',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'更新人名稱',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'更新時間',type:'datetime',width:110,hidden:true,align:'left'}];
    const detail =  {
                    cnName: 'BOM明细',
                    table: 'MES_Bom_Detail',
                    columns: [{field:'DomDetailId',title:'ID',type:'string',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'BomId',title:'BomId',type:'string',width:110,hidden:true,align:'left'},
                       {field:'Sequence',title:'序號',type:'int',width:70,require:true,align:'left'},
                       {field:'MaterialCode',title:'子件物料編碼',type:'string',width:130,edit:{type:''},align:'left'},
                       {field:'MaterialName',title:'子件物料名稱',type:'string',width:130,edit:{type:''},align:'left'},
                       {field:'Spec',title:'規格型號',type:'string',width:80,edit:{type:''},align:'left'},
                       {field:'UsageQty',title:'單台用量',type:'decimal',width:90,edit:{type:'number'},require:true,align:'left'},
                       {field:'ConsumeModel',title:'消耗方式',type:'string',width:90,edit:{type:''},require:true,align:'left'},
                       {field:'Warehouse',title:'投料仓庫',type:'string',width:90,edit:{type:''},align:'left'},
                       {field:'SupplierCode',title:'供應商',type:'string',bind:{ key:'供應商',data:[]},width:120,edit:{type:'select'},align:'left'},
                       {field:'KitScale',title:'齐套比例',type:'decimal',width:90,edit:{type:''},align:'left'},
                       {field:'Enable',title:'啟用狀態',type:'int',width:110,hidden:true,align:'left'},
                       {field:'CreateID',title:'創建人',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:120,hidden:true,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:110,hidden:true,align:'left'},
                       {field:'ModifyID',title:'更新人',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'更新人名稱',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'更新時間',type:'datetime',width:110,hidden:true,align:'left'}],
                    sortName: 'CreateDate',
                    key: 'DomDetailId'
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