<template>
	<view class="demo-form vol-bg">
		<view class="demo-pd-30">
			<vol-alert type="primary">
				<view>常用表單组件、事件處理、格式化、數據绑定、數據字典自動轉換等操作框架都内置好了，只需簡單配置即可</view>
			</vol-alert>
		</view>
		<view class="form-content">
			<vol-form ref="formRef" :form-options="editFormOptions" :formFields="editFormFields">
			</vol-form>
		</view>
		<view class="btns">
			<vol-button class="btn" type="default" @click="reset">重置表單</vol-button>
			<vol-button class="btn" type="default" @click="setReadonly">{{readonly?'取消只讀':'表單只讀'}}</vol-button>
			<vol-button class="btn" @click="validate" type="error">表單校驗</vol-button>
		</view>
	</view>
</template>

<script setup>
	import {
		onLoad,
		onShow,
		onHide,
		onPageScroll
	} from '@dcloudio/uni-app'
	import {
		ref,
		defineProps,
		defineEmits,
		defineExpose,
		getCurrentInstance
	} from 'vue'

	const {
		proxy
	} = getCurrentInstance()



	const formRef = ref(null)
	const reset = () => {
		formRef.value.reset();
		proxy.$toast('表單已重置')
	}
	const validate = () => {
		const b = formRef.value.validate();
		if (b) {
			proxy.$toast('校驗成功')
		}
	}
	//只讀
	const readonly = ref(false)
	const setReadonly = () => {
		readonly.value = !readonly.value
		editFormOptions.value.forEach(item => {
			//這里可以哪些字段指定只讀
			// if (item.field=='字段') {

			// }
			if (Array.isArray(item)) {
				item.forEach(x => {
					x.readonly = !x.readonly;
				})
			} else {
				item.readonly = !item.readonly;
			}
			proxy.$toast('設置成功')
		})


		//或者調用方法全部只讀
		//formRef.value.setReadonly(readonly.value)
	}

	const editFormFields = ref({
		inputText1: "這裡是一個輸入框",
		inputReaonly: "只讀不可輸入",
		inputText2: "按回車觸發事件",
		inputText4: "輸入框後固定文本樣式",
		customInput: "自定義輸入框",

		name: "小黄瓜",
		phone: "139100000066",
		height: 183,
		weight: 140,

		scanCode: null,
		textarea: "這裡文字有點長有點長,這裡有點長文字有點長這有點長。",
		pwd: "12345",
		readonlyText: "只讀輸入框",
		cascader1: null,
		cascader2: null,
		cascader3: '004',
		selectVal: "2",
		selectListVal: [], //多選這里的值是數组 
		dateValue: proxy.base.getDate(), //設置默認日期為當天
		datetimeValue: "2022-03-27 20:15",
		dateRange: ["2022-03-10", "2022-06-20"], //數组 
		inputRange: [100000000, 900000000], //區間是數组


		province: "北京市,北京市,海淀區", //省市區縣值必須以逗號隔開

		inputDecimal: null, //小數
		inputNumber: null, //數字
		switchValue: 1,
		radioVal: "1", //單選
		selectClickValue: "",
		dateClickValue: null,
		//注意圖片格式，如果是后台返回的圖片，在前端拼接下路徑，如：
		//  "xx/a.png,xx/b.png".split(',').filter(c=>{return c})
		//    .map(c=>{return {
		//	        orginUrl:c.split('/').pop(),
		//          url:proxy.ipAddress+c
		//    }})
		//如果没有默認值，只需要設置imgs:[]
		imgs: [{
			orginUrl: "Upload/Tables/Sys_User/202411200151443700/wechat.jpg", //文件存儲的路徑
			url: "https://api.volcore.xyz/Upload/Tables/Sys_User/202411200151443700/wechat.jpg" //文件全路徑
		}],
		files: [{
			orginUrl: "Upload/Tables/Sys_User/202411200151443700/wechat.jpg", //文件存儲的路徑
			url: "https://api.volcore.xyz/Upload/Tables/Sys_User/202411200151443700/wechat.jpg" //文件全路徑
		}, {
			orginUrl: "/Upload/Tables/Sys_User/202412300231051917/NET8運行時新增功能.pdf", //文件存儲的路徑
			url: "https://proapi.volcore.xyz/Upload/Tables/Sys_User/202412300231051917/NET8運行時新增功能.pdf" //文件全路徑
		}],
	});
	const editFormOptions = ref([{
			"title": "基礎輸入操作",
			style: "font-weight: 500;color: #9e9e9e;font-size: 26rpx;",
			type: "group"
		}, {
			type: "input",
			"title": "輸入框",
			"required": true,
			"field": "inputText1",
		},
		{
			type: "input",
			"title": "只讀字段",
			"readonly": true,
			"field": "inputReaonly",
		},

		{
			"title": "自定義按鈕+顏色",
			"field": "customInput",
			itemStyle: "background:rgb(232 244 255)", //行樣式
			titleStyle: "color:#007aff", //標題樣式
			valueStyle: "color:red", //值樣式
			extra: {
				text: "按鈕",
				button: true,
				type: "primary",
				icon: "search",
				color: "#ffff",
				size: 12
			}
		},
		{
			"title": "掃碼事件",
			"field": "scanCode",
			placeholder: "請掃碼",
			extra: {
				text: "掃一掃",
				icon: "scan",
				style: "margin-left:20rpx;align-items: center;;color:#007aff;font-size:26rpx",
				color: "#007aff",
				size: 18
			}
		},
		{
			"title": "額外文本",
			"required": false,
			"field": "inputText4",
			valueStyle: "color:red",
			extra: {
				text: "單位(KG)",
				style: "margin-left:20rpx;align-items: center;color:#848383;font-size:26rpx",
			}
		},
		{
			"title": "小數(手機上預覽)",
			"type": "decimal",
			field: "inputDecimal" //只能輸入小數

		},
		{
			"title": "整數(手機上預覽)",
			"type": "number",
			field: "inputNumber" //只能輸入數字 
		},
		{
			"title": "多字段顯示及字段輸入",
			style: "color: #9e9e9e;font-size: 26rpx;",
			type: "group"
		},
		[{
				"title": "姓名",
				"field": "name",
				type: "",
			},
			{
				"title": "電話",
				"field": "phone",
				type: "",
			}
		],
		[{
				"title": "身高(cm)",
				"field": "height",
				type: "decimal",
			},
			{
				"title": "體量(kg)",
				"field": "weight",
				type: "decimal",
			}
		],
		{
			"title": "密碼框",
			"field": "pwd",
			"type": "password"
		},
		{
			"title": "只讀框",
			"field": "readonlyText",
			"type": "text",
			readonly: true
		},
		{
			"title": "省市區縣",
			"field": "province",
			type: "city" //type必須為city
		}, {
			"title": "多文本",
			"field": "textarea",
			type: "textarea"
		},
		{
			type: "group", //表單分组
			style: "color: #9e9e9e;font-size: 26rpx;",
			title: "下拉框單選、多選"
		},
		{
			"title": "下拉框",
			"field": "selectVal",
			type: "select",
			"required": true,
			data: [],
			// data: [{ //也可以手動绑定數據源，参照格式：
			// 	key: "1",
			// 	value: "正常"
			// }, {
			// 	key: "2",
			// 	value: "異常"
			// }],
			key: "訂單類型"
		},
		{
			"title": "多選框",
			"field": "selectListVal",
			type: "selectList",
			"required": true,
			data: [],
			key: "訂單類型"
		},
		{
			"title": "是否值",
			"type": "switch",
			"field": "switchValue"
		},
		{
			"title": "單選",
			"type": "radio",
			data: [{
				key: "1",
				value: "正常"
			}, {
				key: "2",
				value: "異常"
			}, {
				key: "2",
				value: "離線"
			}],
			key: "",
			placement: 'row', //布局方式，row横向，column縱向	,具體見uvivew文檔
			//placement:"column",	
			"field": "radioVal"
		},
		{
			type: "group", //表單分组
			style: "font-weight: 500;font-size: 26rpx;color: #848383;",
			title: "設置checkStrictly=true,只能選擇最后一級節點"
		},
		{
			"title": "樹形級聯",
			"field": "cascader1",
			type: "cascader",
			"required": true,
			checkStrictly: true, //是否只能選擇最后一個節點,默認可以選擇任意節點
			data: [],
			key: "tree_roles"
		},
		{
			"title": "樹形級聯(只能選最後一級)",
			"field": "cascader2",
			type: "cascader",
			"required": true,
			checkStrictly: false, //是否只能選擇最后一個節點,默認可以選擇任意節點
			data: [],
			key: "tree_roles"
		},
		{
			"title": "自定義級聯",
			"field": "cascader3",
			type: "cascader",
			"required": true,
			checkStrictly: false, //是否只能選擇最后一個節點,默認可以選擇任意節點
			data: [{
				id: "001",
				parentId: null,
				name: "一級節點"
			}, {
				id: "002",
				parentId: "001",
				name: "二級節點"
			}, {
				id: "003",
				parentId: null,
				name: "三級節點"
			}, {
				id: "004",
				parentId: "003",
				name: "四級節點"
			}]
		},

		{
			type: "group", //表單分组
			style: "font-weight: 500;font-size: 26rpx;color: #848383;",
			title: "日期設置min與max屬性限制選擇範圍"
		},
		{
			"title": "日期",
			"required": true,
			"type": "date",
			"field": "dateValue",
			//設置時間選擇範圍，如果日期是datetime類型，時間后面加上時分秒
			//2023.04.02更新util->common.js才能使用獲取日期的方法
			// min:'2023-04-01',
			// max:'2023-07-02'

			//設置只能選擇半個月内的數據
			min: proxy.base.addDays(proxy.base.getDate(), -15),
			max: proxy.base.getDate()
		},
		{
			"title": "日期時分秒",
			"type": "datetime",
			"field": "datetimeValue"
		},
		{
			"title": "日期範圍",
			"type": "date",
			range: true, //區間輸入
			"field": "dateRange"
		},
		{
			"title": "區間輸入",
			"type": "decimal", //number數字，text文本輸入
			range: true, //區間輸入
			"field": "inputRange"
		},
		{
			type: "group", //表單分组
			style: "font-weight: 500;font-size: 26rpx;color: #848383;",
			"title": "多圖片、視频、文件上傳",
		},
		{
			"title": "圖片上傳",
			"type": "img",
			readonly: false, //設置圖片只讀
			"url": "api/sys_user/upload", //后台框架自带的上傳方法，如果涉及權限問題，請参照后台開發文檔上重写權限来重写upload方法的權限
			"multiple": true, //從圖上傳
			"maxCount": 3, //最多只能上傳3張圖片
			"field": "imgs"
		}, {
			"title": "文件上傳",
			"type": "file",
			readonly: false, //設置圖片只讀
			"url": "api/sys_user/upload", //后台框架自带的上傳方法，如果涉及權限問題，請参照后台開發文檔上重写權限来重写upload方法的權限
			"multiple": true, //從圖上傳
			"maxCount": 3, //最多只能上傳3張圖片
			"field": "files"
		},
	])

	//頁面加載時可以從后台獲取给表單绑定值
	onLoad((options) => {
		//options獲取跳轉時参數
		//調用接口獲取表單的值
		// proxy.http.post("url",{参數},true).then(res=>{
		//const fields=res;
		////表單設置值，注意fields的類型應該是json數组格式：{字段1:值1,字段2:值2}
		//proxy.base.resetForm(editFormFields.value, editFormOptions.value,fields)
		//})
	})
</script>

<style scoped lang="less">
	.demo-form {
		display: flex;
		flex-direction: column;
	}

	.form-content {
		flex: 1;
		overflow: auto;
	}

	.btns {
		// background: #fff;
		bottom: 40rpx;
		display: flex;
		padding: 20rpx 0;

		.btn {
			flex: 1;
		}
	}
</style>
