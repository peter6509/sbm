/*这是生成的配置信息,任何修改都会被生成覆蓋,如果需要修改,请在生成Sys_Region.vue中修改,searchFormOptions、editFormOptions、columns屬性
Author:vol
 QQ:283591387
 Date:2024
*/
export default function(){
		const table = {
			tableName: "Sys_Region",
			tableCNName: "省市區縣",
			titleField:'',
			key: 'id',
			sortName: "id"
		}

	    const searchFormFields = {"code":"","name":"","parentId":"","Lng":"","Lat":"","pinyin":""};
	    const searchFormOptions = [{"title":"編碼","field":"code","type":"like"},{"title":"上級編碼","field":"parentId","type":"number"},{"title":"經度","field":"Lng","type":"decimal"},{"title":"緯度","field":"Lat","type":"decimal"},{"type":"group"},{"title":"名稱","field":"name"},{"title":"拼音","field":"pinyin","type":"like"}]
        const editFormFields = {"code":"","name":"","parentId":"","level":"","mername":"","pinyin":"","Lng":"","Lat":""};
        const editFormOptions = [{"title":"編碼","field":"code"},
                               {"title":"名稱","field":"name"},
                               {"title":"上級編碼","field":"parentId","type":"number"},
                               {"type":"group"},
                               {"title":"級别","field":"level","type":"number"},
                               {"title":"完整地址","field":"mername"},
                               {"title":"拼音","field":"pinyin"},
                               {"type":"group"},
                               {"title":"經度","field":"Lng","type":"decimal"},
                               {"title":"緯度","field":"Lat","type":"decimal"}];

		const columns = [{field:'code',title:'編碼',type:'string',width:70},
                       {field:'name',title:'名稱',type:'string',width:100},
                       {field:'parentId',title:'上級編碼',type:'int',width:70},
                       {field:'level',title:'級别',type:'int',width:60},
                       {field:'mername',title:'完整地址',type:'string',width:170},
                       {field:'Lng',title:'經度',type:'float',width:70},
                       {field:'Lat',title:'緯度',type:'float',width:70},
                       {field:'pinyin',title:'拼音',type:'string',width:120}];

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