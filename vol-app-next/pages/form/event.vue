<template>
	<view class="demo-form vol-bg">
		<view class="demo-pd-30">
			<vol-alert type="primary">
				<view>輸入框、下拉框、日期、範圍輸入、級聯、掃碼、圖片文件上傳等選擇及輸入事件處理</view>
			</vol-alert>
		</view>
		<view class="form-content">
			<vol-form ref="formRef" :form-options="editFormOptions" :formFields="editFormFields"
				@extraClick="extraClick" @inputConfirm="inputConfirm" @onChange="onChange">
			</vol-form>
		</view>
		<view class="btns">
			<vol-button class="btn" type="default" @click="reset">重置表單</vol-button>
			<vol-button class="btn" type="default" @click="setInputFocus">設置焦點</vol-button>
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
		getCurrentInstance,
		onMounted
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
	//掃一掃、額外按鈕點擊事件
	const extraClick = (item, fields) => {
		if (item.field == 'scanCode') {
			uni.scanCode({
				success: (res) => {
					editFormFields.value[item.field] = res.result;
					proxy.$toast('掃碼成功')
				},
				fail() {
					proxy.$toast('掃碼失败')
				}
			})
			return;
		}
		if (item.field == 'customInput') {
			editFormFields.value[item.field] = ~~(Math.random() * 1000000)
			proxy.$toast('點擊了按鈕')
		}
	}
	//回车事件
	const inputConfirm = (field, $e) => {
		if (field == 'inputText1') {
			proxy.$toast('觸發了回車事件')
		}
	}
	//下拉框、日期、radio選擇事件
	const onChange = (field, value, item, data) => {
		//選擇時可以给其他下拉框重新绑定值,同時清空其他字段的值： editFormFields.value.字段=null
		// editFormOptions.value.find(c=>{
		// 	  if (c.field=='字段') {
		// 	  	  c.data=[{key:1,value:"名稱"}]
		// 	  }
		// })
		
		if (field == 'selectVal') {
			proxy.$toast('選擇了下拉框：' + value+':'+item.value)
			return;
		}
		if (field == 'selectListVal') {
			proxy.$toast('選擇多選下拉框：' + value.join(','))
			return;
		}
		proxy.$toast('選擇了：' + value)
		return;
	}
    
	//表單配置
	const editFormFields = ref({
		inputText1: "",
		InputFocus: "",
		inputText4: "輸入框後固定文本樣式",
		customInput: "自定義輸入框",
		scanCode: null,
		
		selectVal: "2",
		selectListVal: [], //多選這里的值是數组 
		dateValue: proxy.base.getDate(), //設置默認日期為當天
		datetimeValue: "2022-03-27 20:15",
		dateRange: ["2022-03-10", "2022-06-20"], //數组 
	});
	const editFormOptions = ref([ {
			type: "input",
			"title": "監聽回車",
			"required": true,
			placeholder: "按回車或手機按完成觸發事件",
			"field": "inputText1",
		},
		{
			type: "input",
			"title": "設置焦點",
			placeholder: "自動獲取焦點",
			"field": "InputFocus",
		},
		{
			"title": "自定義按鈕",
			"field": "customInput",
			extra: {
				text: "按鈕",
				button: true,
				type: "primary",
				icon: "search",
				color: "#ffff",
				size: 13
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
				style: "margin-left:20rpx;align-items: center;color:red;font-size:26rpx",
			}
		},
		{
			type: "group", //表單分组
			style: "color: #9e9e9e;font-size: 26rpx;",
			title: "下拉框單選、多選、日期、事件"
		},
		{
			"title": "下拉框",
			"field": "selectVal",
			type: "select",
			"required": true,
			data: [],
			key: "訂單類型"
		},
		{
			"title": "下拉框多選",
			"field": "selectListVal",
			type: "selectList",
			"required": true,
			data: [],
			key: "頂單類型"
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
			//設置時間選擇範圍，如果日期是datetim類型，時間后面加上時分秒
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
		}
	])

	const setInputFocus = () => {
		editFormOptions.value.forEach(x => {
			if (x.field == 'InputFocus') {
				x.focus = false; //必須先設置為false
				setTimeout(() => {
					x.focus = true;
					proxy.$toast('設置焦點成功')
				}, 500)
			}
		})
	}
	//可以在頁面啟動的時候設置默認焦點
	//setInputFocus();
	
	
	//頁面加載時可以從后台獲取给表單绑定值
	onLoad((options)=>{
		 //options獲取跳轉時参數
		 
		 // proxy.http.post("url",{参數},true).then(res=>{
		 // 	editFormFields.value.字段1=res.字段1;
		 // 	editFormFields.value.字段2=res.字段2;
		 // 	//注意如果是多選，或者區間，值應該是數组，見上面给的editFormFields的格式
		 
		 // 	//如果是圖片，見上面editFormFields圖片字段格式的說明
		 // 	editFormFields.value.字段3=(res.字段3||'').split(',')
		 // 	   .filter(c=>{return c})
		 // 	   .map(c=>{return {
		 // 		    orginUrl:c.split('/').pop(),
		 // 	         url:proxy.ipAddress+c
		 // 	   }})
		 //})
	})
	
</script>

<style scoped lang="less">
	.demo-form {
		display: flex;
		flex-direction: column;
	}

	.form-content {
		//flex: 1;//按鈕顯示到最底部
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
