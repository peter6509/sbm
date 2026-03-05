// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能导致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'id',
        footer: "Foots",
        cnName: 'TEST',
        name: 'sbm_test',
        newTabEdit: false,
        url: "/sbm_test/",
        sortName: "test_no",
        fixedSearch:false,
        width: 900,     // 彈出框寬度 (px)
        height: 600,    // 彈出框高度 (px)
        top: 50,        // 距離頂部距離
        fullscreen: false, // 是否全螢幕
        draggable: true,   // 是否可拖曳
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"test_no":"","test_name":"","test_type":"","test_addr":"","test_memo":""};
    const editFormOptions = [[{"title":"TEST單號","field":"test_no","colSize":33.333},
                               {"title":"TEST名稱","field":"test_name","colSize":33.333},
                               {"dataKey":"夥伴類型","data":[],"title":"TEST屬性","field":"test_type","colSize":33.333,"type":"select"}],
                              [{"title":"TEST地址","field":"test_addr","colSize":100.0,"type":"textarea"}],
                              [{"title":"TEST備註","field":"test_memo","colSize":100.0,"type":"textarea"}]];
    const searchFormFields = {};
    const searchFormOptions = [];
    const columns = [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'test_no',title:'TEST單號',type:'string',link:true,width:110,align:'left'},
                       {field:'test_name',title:'TEST名稱',type:'string',width:110,align:'left'},
                       {field:'test_type',title:'TEST屬性',type:'int',bind:{ key:'夥伴類型',data:[]},width:110,align:'left'},
                       {field:'test_addr',title:'TEST地址',type:'string',width:150,align:'left'},
                       {field:'test_memo',title:'TEST備註',type:'string',width:150,align:'left'}];
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