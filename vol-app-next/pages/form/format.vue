<template>
	<view class="demo-form vol-bg">
		<view class="demo-pd-30">
			<vol-alert type="primary">
				<view>可設置設置標題、值及行顏色</view>
			</vol-alert>
		</view>
		<view class="form-content">
			<vol-form ref="formRef" :form-options="editFormOptions" :formFields="editFormFields"
				@extraClick="extraClick" :getStyle="getStyle">
			</vol-form>
		</view>
		<view class="btns">
			<vol-button class="btn" type="default" @click="reset">重置表單</vol-button>
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


	//fields表單全部的值，option表單配置。 isValue=true右邊值的樣式。isValue=false左邊值的樣式
	const getStyle = (fields, option, isValue) => {
		//判斷字段来設置樣式
		if (option.field == 'dyValue') {
			////可以根據不同的值判斷設置不同的類型
			// if (fields.dyValue=='xx') {
			// }
			
			//右邊值的樣式
			if (isValue) {
				return {'font-weight':'700','font-size':'32rpx',color:'#f09102'}
			}
			//左邊標題的樣式
			if (!isValue) {
				return {color:'red'}
			}
		}
		console.log(fields, option, isValue)
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
		if (item.field == 'customInput' || item.field == 'customInput2') {
			editFormFields.value[item.field] = ~~(Math.random() * 1000000)
			proxy.$toast('點擊了按鈕')
		}
	}

	//表單配置
	const editFormFields = ref({
		titleColor: "20000",
		colorValue: 10000,
		inputText1: "",
		InputFocus: "",
		dyValue: 36000,
		inputText4: "輸入框後固定文本樣式",
		customInput: "自定義輸入框",
		customInput2: "",
		scanCode: null,
	});
	const editFormOptions = ref([{
			"title": "標題樣式",
			titleStyle: "color:#007aff", //標題樣式
			"field": "titleColor"
		}, {
			"title": "輸入框值樣式",
			valueStyle: "color:#007aff", //標題樣式
			"field": "colorValue"
		},
		{
			type: "group"
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
		}, {
			"title": "自定義按鈕+顏色",
			"field": "customInput2",
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
			type: "group",
			style:"font-size:26rpx;padding-top:10rpx",
			title:"可根據不同值與字段動態設置[標題、值]的樣式"
		},
		{
			"title": "動態樣式",
			"field": "dyValue"
		},
		{
			type: "group"
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
		}

	])

	//可以在頁面啟動的時候設置默認焦點
	//setInputFocus();

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
