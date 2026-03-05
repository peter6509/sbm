<template>
	<view class="vol-bg">
		<view class="demo-pd-30">
			<vol-alert type="primary">
				<view>可指定標題顯示靠左或靠右顯示及標簽寬度設置</view>
			</vol-alert>
		</view>
		<vol-form ref="editRef" :formFields="editFormFields" :formOptions="editFormOptions"
			:label-position="labelPosition" :label-width="labelWidth">
		</vol-form>
		<view class="btns">
			<view class="btn">
				<vol-button type="default" @click="clickWidth">標題寬度</vol-button>
			</view>
			<view class="btn">
				<vol-button type="success" @click="clickLeft">標題靠左</vol-button>
			</view>
			<view class="btn">
				<vol-button type="primary" @click="clickRight">標題靠右</vol-button>
			</view>
		</view>
	</view>
</template>
<script setup>
	import {
		onLoad
	} from '@dcloudio/uni-app'
	import {
		ref,
		getCurrentInstance,
		defineEmits,
		defineProps,
		watch,
	} from "vue";

	const {
		proxy
	} = getCurrentInstance();

	//發起請求proxy.http.get/post
	//消息提示proxy.$toast()

	const labelWidth = ref(70);

	const clickWidth = () => {
		labelWidth.value = labelWidth.value == 70 ? labelWidth.value = 80 : labelWidth.value = 70;
		proxy.$toast(`標題寬度已設置為：${labelWidth.value}`)
	}

	const labelPosition = ref('left');

	const clickLeft = () => {
		labelPosition.value = 'left'
		proxy.$toast(`標題已靠左顯示`)
	}

	const clickRight = () => {
		labelPosition.value = 'right'
		proxy.$toast(`標題已靠右顯示`)
	}

	//vol-view组件
	const editRef = ref(null);

	const editFormFields = ref({
		"OrderNo": "P202411200000001",
		"OrderType": "1",
		"TotalPrice": 12000,
		"TotalQty": 9000,
		"OrderDate": '2024-12-20',
		"Customer": '不要問',
		"PayType": "微信支付",
		"PhoneNo": "13888888888",
		"OrderStatus": "2",
		"Address": "北京市海淀區西北旺東路10號",
		"Remark": "還没想好。。。。。。。。"
	});

	const editFormOptions = ref([{
			"title": "訂單編號",
			"required": true,
			"field": "OrderNo",
			"disabled": false
		},
		[{
				"key": "訂單類型",
				"data": [],
				"title": "訂單類型",
				"required": true,
				"field": "OrderType",
				"type": "select"
			},
			{
				"key": "訂單狀態",
				"data": [],
				"title": "訂單狀態",
				"required": true,
				"field": "OrderStatus",
				"type": "select"
			}
		],
		[{
				"title": "訂單總價",
				"field": "TotalPrice",
				"type": "decimal"
			},
			{
				"title": "訂單數量",
				"field": "TotalQty",
				"type": "number"
			}
		],
		[{
			"title": "客户姓名",
			"field": "Customer"
		}, {
			"title": "支付方式",
			"field": "PayType"
		}, ],
		{
			"title": "手機號碼",
			"field": "PhoneNo"
		},
		{
			"title": "訂單日期",
			"required": true,
			"field": "OrderDate",
			"type": "datetime"
		},
		{
			"type": "group"
		},
		{
			"title": "詳细地址",
			"field": "Address"
		},
		{
			"title": "備註說明",
			"field": "Remark"
		}
	]);
	
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
<style lang="less" scoped>
	.btns {
		margin-top: 20rpx;
	}
</style>
