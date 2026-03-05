// *Author：jxx
// *Contact：283591387@qq.com
// *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
export default function(){
    const table = {
        key: 'Order_Id',
        footer: "Foots",
        cnName: '訂單管理',
        name: 'Demo_Order',
        newTabEdit: false,
        url: "/Demo_Order/",
        sortName: "CreateDate"
    };
    const tableName = table.name;
    const tableCNName = table.cnName;
    const newTabEdit = false;
    const key = table.key;
    const editFormFields = {"OrderNo":"","OrderType":"","TotalPrice":"","TotalQty":"","OrderDate":"","Customer":"","PhoneNo":"","OrderStatus":""};
    const editFormOptions = [[{"title":"訂單編號","required":true,"field":"OrderNo","disabled":true},
                               {"dataKey":"訂單類型","data":[],"title":"訂單類型","required":true,"field":"OrderType","type":"select"},
                               {"title":"總價","field":"TotalPrice","type":"decimal"},
                               {"title":"總數量","field":"TotalQty","type":"number"}],
                              [{"title":"訂單日期","required":true,"field":"OrderDate","type":"datetime"},
                               {"title":"客户","field":"Customer","disabled":true,"type":"selectTable"},
                               {"title":"手機","field":"PhoneNo","disabled":true,"type":"text"},
                               {"dataKey":"訂單狀態","data":[],"title":"訂單狀態","required":true,"field":"OrderStatus","type":"select"}]];
    const searchFormFields = {"OrderNo":"","OrderType":"","TotalPrice":[null,null],"OrderDate":"","PhoneNo":"","OrderStatus":"","CreateDate":""};
    const searchFormOptions = [[{"title":"訂單編號","field":"OrderNo"},{"dataKey":"訂單類型","data":[],"title":"訂單類型","field":"OrderType","type":"select"},{"title":"總價","field":"TotalPrice","type":"range"},{"title":"手機","field":"PhoneNo"}],[{"title":"訂單日期","field":"OrderDate","type":"datetime"},{"dataKey":"訂單狀態","data":[],"title":"訂單狀態","field":"OrderStatus","type":"select"},{"title":"創建時間","field":"CreateDate","type":"datetime"}]];
    const columns = [{field:'Order_Id',title:'Order_Id',type:'string',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'OrderNo',title:'訂單編號',type:'string',link:true,width:130,readonly:true,require:true,align:'left'},
                       {field:'OrderType',title:'訂單類型',type:'int',bind:{ key:'訂單類型',data:[]},width:90,require:true,align:'left'},
                       {field:'TotalPrice',title:'總價',type:'decimal',width:70,align:'left'},
                       {field:'TotalQty',title:'總數量',type:'int',width:80,align:'left'},
                       {field:'OrderDate',title:'訂單日期',type:'date',width:95,require:true,align:'left'},
                       {field:'CustomerId',title:'客户',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Customer',title:'客户',type:'string',width:80,readonly:true,align:'left'},
                       {field:'PhoneNo',title:'手機',type:'string',width:110,readonly:true,align:'left'},
                       {field:'OrderStatus',title:'訂單狀態',type:'int',bind:{ key:'訂單狀態',data:[]},width:90,require:true,align:'left'},
                       {field:'AuditStatus',title:'審核狀態',type:'int',bind:{ key:'audit',data:[]},width:80,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:100,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:80,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:145,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:130,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:110,hidden:true,align:'left'},
                       {field:'AuditId',title:'AuditId',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Auditor',title:'審核人',type:'string',width:110,hidden:true,align:'left'},
                       {field:'AuditDate',title:'審核時間',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'AuditReason',title:'AuditReason',type:'string',width:220,hidden:true,align:'left'}];
    const detail =  {
                    cnName: '訂單明细',
                    table: 'Demo_OrderList',
                    columns: [{field:'OrderList_Id',title:'OrderList_Id',type:'string',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Order_Id',title:'Order_Id',type:'string',width:110,hidden:true,align:'left'},
                       {field:'GoodsId',title:'商品id',type:'string',width:110,hidden:true,align:'left'},
                       {field:'GoodsName',title:'商品名稱',type:'string',width:120,edit:{type:'selectTable'},require:true,align:'left'},
                       {field:'GoodsCode',title:'商品編號',type:'string',width:120,edit:{type:'text'},require:true,align:'left'},
                       {field:'Img',title:'商品圖片',type:'img',width:100,align:'left'},
                       {field:'Specs',title:'商品規格',type:'string',bind:{ key:'商品規格',data:[]},width:120,readonly:true,edit:{type:'select'},align:'left'},
                       {field:'Price',title:'單價',type:'decimal',width:110,readonly:true,edit:{type:''},require:true,align:'left'},
                       {field:'Qty',title:'數量',type:'int',width:110,edit:{type:''},require:true,align:'left'},
                       {field:'Remark',title:'備註',type:'string',width:100,edit:{type:''},align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:145,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:130,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:110,hidden:true,align:'left'}],
                    sortName: 'CreateDate',
                    key: 'OrderList_Id'
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