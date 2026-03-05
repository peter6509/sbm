export default function() {
	return {
		table: {
			type: Object,
			default: () => {
				return {
					key: '',
					sortName: "",
					tableName: "",
					tableCNName: "",
				}
			}
		},
		keyField: {
			//主鍵字段
			type: String,
			default: ''
		},
		tableName: {
			type: String,
			default: ''
		},
		tableCNName: {
			type: String,
			default: ''
		},
		defaultLoad: { //默认加载表格數据
			type: Boolean,
			default: true
		},
		direction: { //页面显示表格或列表：horizontal、list
			type: String,
			default: "list"
		},
		titleField: { //list表列標題字段
			type: String,
			default: ""
		},
		columns: { //表格配置
			type: Array,
			default: () => {
				return []
			}
		},
		tableData: { //表格數据
			type: Array,
			default: () => {
				return []
			}
		},
		textInline: { //文本是否显示在一行，超出不換行(table生效)
			type: Boolean,
			default: true
		},
		tableAction: { //指定表的權限
			type: String,
			default: ""
		},
		labelPosition: {
			type: String,
			default: '' //left\right
		},
		labelWidth: {
			type: Number, //表單標簽寬度
			default: 0
		},
		showTotal:{//显示分页总數
			type: Boolean,
			default: true
		},
		searchFormFields: {
			type: Object,
			default: {}
		},
		searchFormOptions: {
			type: Object,
			default: () => {
				return []
			}
		},
		editFormFields: {
			type: Object,
			default: {}
		},
		editFormOptions: {
			type: Object,
			default: () => {
				return []
			}
		},
		index: { //显示行号
			type: Boolean,
			default: false
		},
		ck: {
			//明细表複選框
			type: Boolean,
			default: false
		},
		textInline: {
			type: Boolean,
			default: true
		},
		searchInput: { //显示快捷输入搜索
			type: Boolean,
			default: true
		},
		showSearchInput:{//显示搜索框
			type: Boolean,
			default: true
		},
		searchBefore: null, //表單加载前
		searchAfter: null,
		addBefore: null, //新建前
		addBeforeAsync: null, //新建前
		addAfter: null, //新建后
		updateBefore: null, //更新前
		updateBeforeAsync: null, //更新前
		updateAfter: null, //更新后
		delBefore: null,
		delAfter: null,
		modelOpenBefore: null,
		modelOpenBeforeAsync: null,
		modelOpenAfter: null,
		rowClick: null,
		getBottomButton: null,
		saveBefore: null,
		saveAfter: null
	}
}
