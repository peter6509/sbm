<template>
	<view class="demo-form vol-bg">
		<view class="demo-pd-30">
			<vol-alert type="primary">
				<view>監聽表單輸入或值變化時给其他字段設置值或計算值，同時適用於自動生成的頁面</view>
			</vol-alert>
		</view>
		<view class="form-content">
			<vol-form ref="formRef" :getStyle="getStyle" :form-options="editFormOptions" :formFields="editFormFields">
			</vol-form>
		</view>
		<view class="btns">
			<vol-button class="btn" type="default" @click="reset">重置表單</vol-button>
			<vol-button class="btn" @click="validate" type="primary">表單校驗</vol-button>
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
		watch,
		onMounted,
		reactive
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

	const getStyle = (fields, option, isValue) => {
		if (option.field === 'input4' && isValue) {
			return {
				color: 'red',
				'font-size': '36rpx',
				'font-weight': 700
			}
		}
	}

	//表單配置
	const editFormFields = ref({
		input1: 100,
		input2: 200,
		input3: 120,
		input4: 320,
	});
	const editFormOptions = ref([{
			type: "number",
			"title": "物料數量",
			"required": true,
			"field": "input1",
			extra: {
				text: "箱",
				style: "margin-left:10rpx;font-size:28rpx;color:rgb(181 185 190)",
			}
		},
		{
			type: "decimal",
			"title": "進貨單價",
			"required": true,
			"field": "input2",
			extra: {
				text: "元",
				style: "margin-left:10rpx;font-size:28rpx;color:rgb(181 185 190)",
			}
		},
		{
			type: "decimal",
			"title": "運費",
			"required": true,
			"field": "input3",
			extra: {
				text: "元",
				style: "margin-left:10rpx;font-size:28rpx;color:#007aff",
			}
		}, {
			type: "group"
		}, {
			type: "input",
			"title": "合計成本",
			readonly: true,
			"field": "input4",
			itemStyle: "background:rgb(232 244 255);color:#007aff;", //行樣式
			titleStyle: "color:#007aff", //標題樣式
			valueStyle: "color:red", //值樣式
			extra: {
				text: "元",
				style: "margin-left:10rpx;font-size:28rpx;color:rgb(181 185 190)",
			}
		},
	])


	const setTotalValue = () => {
		console.log(222)
		editFormFields.value.input4 = (((editFormFields.value.input1 * 1.0 || 0) +
			(editFormFields.value.input2 * 1.0 || 0) +
			(editFormFields.value.input3 * 1.0 || 0)) * 1.0).toFixed(2) * 1.0 //保留兩位小數
	}

	//監聽三個輸入框的變化
	watch(
		() => editFormFields.value.input1,
		(newValue, oldValue) => {
			setTotalValue();
		})

	watch(
		() => editFormFields.value.input2,
		(newValue, oldValue) => {
			setTotalValue();
		})

	watch(
		() => editFormFields.value.input3,
		(newValue, oldValue) => {
			setTotalValue();
		})

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
