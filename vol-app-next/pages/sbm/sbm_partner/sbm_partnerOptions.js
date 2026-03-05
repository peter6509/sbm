/*這是生成的配置信息,任何修改都會被生成覆蓋,如果需要修改,請在生成sbm_partner.vue中修改,searchFormOptions、editFormOptions、columns屬性
Author:vol
 QQ:283591387
 Date:2024
*/
export default function(){
		const table = {
			tableName: "sbm_partner",
			tableCNName: "合作夥伴",
			titleField:'name',
			key: 'id',
			sortName: "name"
		}

	    const searchFormFields = {"name":"","partner_type":"","parent_id":"","address":"","tel":"","mobile":"","email":""};
	    const searchFormOptions = [{"title":"電話","field":"tel"},{"title":"行動電話","field":"mobile"},{"title":"EMail","field":"email"},{"type":"group"},{"title":"夥伴名稱","field":"name","type":"like"},{"key":"夥伴類型","data":[],"title":"公司/個人","field":"partner_type","type":"select"},{"key":"夥伴公司名稱","data":[],"title":"所屬公司","field":"parent_id","type":"select"},{"type":"group"},{"title":"公司地址","field":"address","type":"like"}]
        const editFormFields = {};
        const editFormOptions = [];

		const columns = [{field:'name',title:'夥伴名稱',type:'string',link:true,width:150},
                       {field:'partner_type',title:'公司/個人',type:'int',bind:{ key:'夥伴類型',data:[]},width:80},
                       {field:'parent_id',title:'所屬公司',type:'int',bind:{ key:'夥伴公司名稱',data:[]},width:130},
                       {field:'address',title:'公司地址',type:'string',width:150},
                       {field:'tel',title:'電話',type:'string',width:100},
                       {field:'mobile',title:'行動電話',type:'string',width:90},
                       {field:'email',title:'EMail',type:'string',width:100}];

        const detail = {columns:[]};
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