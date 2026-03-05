<template>
	<view class="demo-form vol-bg">
		<view class="demo-pd-30">
			<vol-alert type="primary">
				<view>多個字段按分組顯示在一行，减少頁面幅度，並支持編輯功能，同時適用於自動生成的頁面</view>
			</vol-alert>
		</view>
		<view class="form-content">
			<vol-form ref="formRef" :form-options="editFormOptions" :formFields="editFormFields">
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

	const editFormFields = ref({
		processCode: "G202412080000001",
		processLine: "測試第二路線第一工序",
		materialAmount: 100,
		jobAmount: 5,
		cj: "(ICR)成品部",
		major: 1,
		remark: "測試第二路線第一工序測試第二路線"
	});
	const editFormOptions = ref([{
			type: "input",
			"title": "工序編號",
			"required": true,
			"field": "processCode",

		},
		{
			type: "input",
			"title": "工藝路線",
			"readonly": true,
			"field": "processLine",
		},
		{
			type: "group"
		},
		[{
				"title": "用料數量",
				type: "decimal",
				"field": "materialAmount",
			},
			{
				"title": "作業次數",
				type: "number",
				"field": "jobAmount",
			}
		],
		[{
			"title": "生產車間",
			"field": "cj",
		}, {
			"title": "關鍵工序",
			"field": "major",
			type: "select",
			data: [{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"},{key:"a",value:"aaaa"}],
			//dataKey: "enable"
		}],
		{
			type: "group"
		},
		{
			"title": "工序備註",
			"field": "remark",
			"type": "input"
		}
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
