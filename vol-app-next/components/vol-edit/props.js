export default function() {
	return {
		id: {
			type: String,
			default: ''
		},
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
		detail: { //主从明细表
			type: Object,
			default: () => {
				return {
					cnName: "",
					table: "",
					columns: []
				}
			}
		},
		details: { //一对多明细表
			type: Array,
			default: () => {
				return []
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
			default: true
		},
		ck: {
			//明细表複選框
			type: Boolean,
			default: true
		},
		textInline: {
			type: Boolean,
			default: true
		},
		getStyle: null, //標題與屬性樣式
		loadFormBefore: null, //表單加载前
		loadFormAfter: null, //表單加载后
		delBefore: null,
		delAfter: null,
		loadDetailBefore: null,
		loadDetailAfter: null,//明细表加载
		delRowBefore: null, //明细表删除方法
		delRowAfter: null,
		addRow: null, //明细表添加行方法
		modelOpenBefore: null,
		modelOpenBeforeAsync: null,
		modelOpenAfter: null,
		rowClick: null,
		getBottomButton: null,
		saveBefore: null,
		saveAfter: null
	}
}