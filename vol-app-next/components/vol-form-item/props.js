/* Author:vol
   QQ:283591387
   Date:2024
*/   
export default function() {
	return {
		formOptions: {
			type: Array,
			default: () => {
				return []
			}
		},
		item: {
			type: Object,
			default: () => {
				return {}
			} 
		},
		readonly:{ //默認是否只讀
			type:Boolean,
			default:false
		},
		formFields: {
			type: Object,
			default: () => {
				return {}
			}
		},
		padding: {
			type: Number,
			default: 30
		},
		labelWidth: {//標題標簽寬度
			type: Number,
			default: 0
		},
		labelPosition: {
			type: String,
			default: ''//left\right
		},
		loadKey: {
			type: Boolean,
			default: true
		},
		border:{
			type: Boolean,
			default: false
		},
		fontSize:{
			type: String,
			default: '28rpx'
		},
		getStyle:null,//標題與屬性樣式
	}
}
