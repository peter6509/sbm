export default function() {
	return {
		url: { //接口地址
			type: String,
			default: ""
		},
		index: { //是否显示行号
			type: Boolean,
			default: false
		},
		height: {
			type: Number, //表格高度,為0时默认為页面剩餘高度(100%高度),為-1时，不固定高度，内容有多長就显示多高
			default: 0 //可選值0、-1、自定义高度值
		},
		minHeight: {  //表格為自动高度时最小高度
			type: Number,
			default: 0
		},
		ck: { //設置显示checkbox，只有水平(table)显示类型时才生效
			type: Boolean,
			default: false
		},
		direction: { //table显示方向
			type: String,
			default: "list" //horizontal:水平table形式显示，list：列表表單形式展示 
		},
		titleField: { //如果direction是以list显示，可以指定第一个標題
			type: String,
			default: ""
		},
		readonly: { //表格是否只讀状态(true不可以編輯)
			type: Boolean,
			default: true
		},
		columns: {
			type: Array,
			default: () => {
				return []
			}
		},
		tableData: {
			type: Array,
			default: () => {
				return []
			}
		},
		padding: { //表格padding，main.js也可以全局設置
			type: Number,
			default: 0
		},
		contentPadding:{
			type: Number,
			default: 0
		},
		textAlign: "", //表格標簽显示位置(左边、右边)：left、center、right
		textInline: { //文本是否显示在一行（超出不換行）
			type: Boolean,
			default: true
		},
		labelWidth: {
			type: Number,
			default: 0
		},
		labelPosition: {
			type: String,
			default: '' //left\right
		},
		loadKey: {
			type: Boolean,
			default: true
		},
		defaultLoadPage: {
			// 配置了url，是否默认加载表格數据
			type: Boolean,
			default: true
		},
		labelWidth: {
			type: Number,
			default: 0
		},
		rowStyle: { //每行自定义樣式
			type: Function,
			default: (row, index, rows) => {
				return;
			}
		},
		columnStyle: { //每个單元格自定义樣式
			type: Function,
			default: (row, column, field, rows) => {
				return;
			}
		},
		getStyle: null, //表單形式，標題與屬性樣式
		selectable: null,
		// {
		// 	type: Function,
		// 	default: (row, column, rowIndex) => { // //複選框是否可以選中
		// 		return true;
		// 	}
		// },
		rowClick: null,
		formatter: null,
		loadBefore: null,
		loadAfter: null,
		getButtons: null,
		format: null
	}
}
