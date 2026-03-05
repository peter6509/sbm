// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'id',
        footer: "Foots",
        cnName: '合作夥伴',
        name: 'sbm_partner',
        newTabEdit: false,
        url: "/sbm_partner/",
        sortName: "name",
        fixedSearch:false
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"name":"","partner_type":"","position":"","bus_type":"","parent_id":"","tel":"","mobile":"","vat":"","address":"","email":"","memo":"","bank_code":"","bank_name":"","account_name":"","bank_bcode":"","bank_bname":"","account_no":"","account_window":""};
    const editFormOptions = [[{"dataKey":"夥伴類型","data":[],"title":"公司/個人","field":"partner_type","colSize":25.0,"type":"select"},
                               {"dataKey":"夥伴屬性","data":[],"title":"客戶/供應商","field":"bus_type","colSize":25.0,"type":"select"},
                               {"title":"夥伴名稱","required":true,"field":"name","colSize":25.0,"type":"like"},
                               {"title":"職位","field":"position","colSize":25.0}],
                              [{"dataKey":"夥伴公司名稱","data":[],"title":"所屬公司","field":"parent_id","colSize":25.0,"type":"select"},
                               {"title":"統編","field":"vat","colSize":25.0,"type":"like"},
                               {"title":"行動電話","field":"mobile","colSize":25.0},
                               {"title":"電話","field":"tel","colSize":25.0}],
                              [{"title":"公司地址","field":"address","colSize":25.0,"type":"like"},
                               {"title":"EMail","field":"email","colSize":25.0}],
                              [{"title":"備註","field":"memo","colSize":60.0},
                               {"title":"銀行代號","field":"bank_code","colSize":25.0}],
                              [{"title":"銀行名稱","field":"bank_name","colSize":25.0},
                               {"title":"分行代號","field":"bank_bcode","colSize":25.0},
                               {"title":"分行名稱","field":"bank_bname","colSize":25.0},
                               {"title":"帳戶名稱","field":"account_name","colSize":25.0}],
                              [{"title":"帳號","field":"account_no","colSize":25.0}],
                              [{"title":"帳務聯絡窗口","field":"account_window","colSize":100.0}]];
    const searchFormFields = {"name":"","partner_type":"","parent_id":"","address":"","vat":""};
    const searchFormOptions = [[{"title":"夥伴名稱","field":"name","colSize":30.0,"type":"like"},{"dataKey":"夥伴類型","data":[],"title":"公司/個人","field":"partner_type","colSize":9.999,"type":"select"},{"dataKey":"夥伴公司名稱","data":[],"title":"所屬公司","field":"parent_id","colSize":25.0,"type":"select"},{"title":"統編","field":"vat","colSize":11.111,"type":"like"}],[{"title":"公司地址","field":"address","colSize":50.0,"type":"like"}]];
    const columns = [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'name',title:'夥伴名稱',type:'string',link:true,width:180,require:true,align:'left'},
                       {field:'partner_type',title:'公司/個人',type:'int',bind:{ key:'夥伴類型',data:[]},width:60,align:'left'},
                       {field:'parent_id',title:'所屬公司',type:'int',bind:{ key:'夥伴公司名稱',data:[]},width:100,hidden:true,align:'left'},
                       {field:'position',title:'職位',type:'string',width:80,hidden:true,align:'left'},
                       {field:'address',title:'公司地址',type:'string',width:250,align:'left'},
                       {field:'tel',title:'電話',type:'string',width:100,align:'left'},
                       {field:'mobile',title:'行動電話',type:'string',width:90,hidden:true,align:'left'},
                       {field:'email',title:'EMail',type:'string',width:100,hidden:true,align:'left'},
                       {field:'memo',title:'備註',type:'string',width:150,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'vat',title:'統編',type:'string',width:110,align:'left'},
                       {field:'bus_type',title:'客戶/供應商',type:'int',bind:{ key:'夥伴屬性',data:[]},width:80,align:'left'},
                       {field:'bank_code',title:'銀行代號',type:'string',width:110,hidden:true,align:'left'},
                       {field:'bank_name',title:'銀行名稱',type:'string',width:120,hidden:true,align:'left'},
                       {field:'account_name',title:'帳戶名稱',type:'string',width:180,hidden:true,align:'left'},
                       {field:'account_no',title:'帳號',type:'string',width:110,hidden:true,align:'left'},
                       {field:'account_window',title:'帳務聯絡窗口',type:'string',width:110,hidden:true,align:'left'},
                       {field:'bank_bcode',title:'分行代號',type:'string',width:110,hidden:true,align:'left'},
                       {field:'bank_bname',title:'分行名稱',type:'string',width:120,hidden:true,align:'left'}];
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