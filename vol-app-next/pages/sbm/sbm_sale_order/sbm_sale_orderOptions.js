/*這是生成的配置信息,任何修改都會被生成覆盖,如果需要修改,請在生成sbm_sale_order.vue中修改,searchFormOptions、editFormOptions、columns屬性
Author:vol
 QQ:283591387
 Date:2024
*/
export default function(){
		const table = {
			tableName: "sbm_sale_order",
			tableCNName: "報價單主檔",
			titleField:'name',
			key: 'sale_id',
			sortName: "name"
		}

	    const searchFormFields = {"name":"","sale_date":"","sales":"","partner_company":"","partner_contact":"","untax_amount":"","discount_amount":"","tax":"","tot_amount":"","description":"","partner_mobile":"","partner_email":""};
	    const searchFormOptions = [{"title":"未稅小計","field":"untax_amount","type":"decimal"},{"title":"優惠小計","field":"discount_amount","type":"decimal"},{"title":"稅金","field":"tax","type":"decimal"},{"title":"含稅合計","field":"tot_amount","type":"decimal"},{"title":"聯絡人電話","field":"partner_mobile"},{"title":"客戶EMAIL","field":"partner_email"},{"type":"group"},{"title":"報價單號","field":"name","type":"like"},{"title":"報價日期","field":"sale_date","type":"date"},{"key":"使用者","data":[],"title":"業務員","field":"sales","type":"select"},{"key":"夥伴公司名稱","data":[],"title":"客戶名稱","field":"partner_company","type":"select"},{"key":"夥伴個人名稱","data":[],"title":"客戶聯絡人","field":"partner_contact","type":"select"},{"type":"group"},{"title":"備註","field":"description","type":"like"}]
        const editFormFields = {};
        const editFormOptions = [];

		const columns = [{field:'name',title:'報價單號',type:'string',link:true,width:150,readonly:true},
                       {field:'sale_date',title:'報價日期',type:'date',width:120},
                       {field:'sales',title:'業務員',type:'int',bind:{ key:'使用者',data:[]},width:80},
                       {field:'partner_company',title:'客戶名稱',type:'int',bind:{ key:'夥伴公司名稱',data:[]},width:200},
                       {field:'partner_contact',title:'客戶聯絡人',type:'int',bind:{ key:'夥伴個人名稱',data:[]},width:90},
                       {field:'untax_amount',title:'未稅小計',type:'float',width:100,readonly:true},
                       {field:'discount_amount',title:'優惠小計',type:'float',width:100},
                       {field:'tax',title:'稅金',type:'float',width:90,readonly:true},
                       {field:'tot_amount',title:'含稅合計',type:'float',width:110,readonly:true},
                       {field:'description',title:'備註',type:'string',width:110}];

        const detail =   {
                    cnName: '報價單明細檔',
                    table: 'sbm_sale_order_line',
                    columns: [{field:'id',title:'ID',type:'int',width:110,hidden:true,require:true,align:'left'},
                       {field:'sale_id',title:'主檔關聯',type:'int',width:110,hidden:true,align:'left'},
                       {field:'parent_id',title:'上階ID',type:'int',width:110,hidden:true,align:'left'},
                       {field:'sale_item',title:'項次',type:'string',link:true,sort:true,width:60,edit:{type:''},align:'left'},
                       {field:'prod_name',title:'產品',type:'string',width:350,edit:{type:''},align:'left'},
                       {field:'prod_num',title:'數量',type:'float',width:60,edit:{type:''},align:'left'},
                       {field:'prod_uom',title:'單位',type:'string',bind:{ key:'單位',data:[]},width:60,edit:{type:'select'},align:'left'},
                       {field:'price_unit',title:'單價',type:'float',width:80,edit:{type:''},align:'left'},
                       {field:'price_sub',title:'小計',type:'float',width:90,edit:{type:''},align:'left'},
                       {field:'prod_desc',title:'備註',type:'string',width:150,edit:{type:''},align:'left'},
                       {field:'CreateDate',title:'CreateDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'CreateID',title:'CreateID',type:'int',width:80,hidden:true,align:'left'},
                       {field:'Creator',title:'Creator',type:'string',width:100,hidden:true,align:'left'},
                       {field:'Modifier',title:'Modifier',type:'string',width:100,hidden:true,align:'left'},
                       {field:'ModifyDate',title:'ModifyDate',type:'datetime',width:150,hidden:true,align:'left'},
                       {field:'ModifyID',title:'ModifyID',type:'int',width:80,hidden:true,align:'left'}],
                    sortName: 'sale_item',
                    key: 'id'
                                            };
        const details = [];

    return {
        table,
		searchFormFields,
		searchFormOptions,
        editFormFields,
        editFormOptions,
		columns,
		detail,
		details
    }
}