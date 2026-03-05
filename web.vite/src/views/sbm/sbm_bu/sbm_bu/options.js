// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'id',
        footer: "Foots",
        cnName: '事業單位',
        name: 'sbm_bu',
        newTabEdit: false,
        url: "/sbm_bu/",
        sortName: "bu_name",
        fixedSearch:false
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"bu_name":"","bu_logo":"","bu_invoice_stamp":"","bu_bankno":"","bu_bankname":"","bu_bankbno":"","bu_bankbname":"","bu_bankaccno":"","bu_bankaccname":""};
    const editFormOptions = [[{"title":"單位名稱","field":"bu_name","colSize":25.0},
                               {"title":"單位LOGO","field":"bu_logo","colSize":25.0,"type":"img"},
                               {"title":"單位發票章","field":"bu_invoice_stamp","colSize":30.0,"type":"img"}],
                              [{"title":"銀行代碼","field":"bu_bankno","colSize":25.0},
                               {"title":"銀行名稱","field":"bu_bankname","colSize":25.0},
                               {"title":"分行代碼","field":"bu_bankbno","colSize":25.0},
                               {"title":"分行名稱","field":"bu_bankbname","colSize":25.0}],
                              [{"title":"帳號","field":"bu_bankaccno","colSize":25.0},
                               {"title":"戶名","field":"bu_bankaccname","colSize":25.0}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'id',title:'ID',type:'int',width:110,require:true,align:'left'},
                       {field:'bu_name',title:'單位名稱',type:'string',link:true,width:110,align:'left'},
                       {field:'bu_logo',title:'單位LOGO',type:'img',width:180,align:'left'},
                       {field:'bu_invoice_stamp',title:'單位發票章',type:'img',width:180,align:'left'},
                       {field:'bu_bankno',title:'銀行代碼',type:'string',width:110,hidden:true,align:'left'},
                       {field:'bu_bankname',title:'銀行名稱',type:'string',width:120,hidden:true,align:'left'},
                       {field:'bu_bankbno',title:'分行代碼',type:'string',width:110,hidden:true,align:'left'},
                       {field:'bu_bankbname',title:'分行名稱',type:'string',width:110,hidden:true,align:'left'},
                       {field:'bu_bankaccno',title:'帳號',type:'string',width:110,hidden:true,align:'left'},
                       {field:'bu_bankaccname',title:'戶名',type:'string',width:120,hidden:true,align:'left'}];
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