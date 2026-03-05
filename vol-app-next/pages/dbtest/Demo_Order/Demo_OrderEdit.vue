<!--
這是生成的文件，事件處理、自定義配置，見移動端文檔：表單、表格配置
Author:vol
QQ:283591387
Date:2024
-->
<template>
	<vol-edit ref="editRef" :id="id" :table="table" :detail="detail" :details="details" :editFormFields="editFormFields"
		:editFormOptions="editFormOptions" :saveBefore="saveBefore" :labelWidth="70" labelPosition="left"
		:saveAfter="saveAfter" :delBefore="delBefore" @onChange="onChange" :delRowBefore="delRowBefore"
		:delRowAfter="delRowAfter" :loadFormAfter="loadFormAfter">
		<template #header>
			<view style="margin-bottom: 22rpx;">
				<vol-alert>1.移動端支持主從表零代碼自動生成,並支持扩展
					<view> 2.框架5分钟配置即可解决繁琐配置,大幅提高開發效率</view>
				</vol-alert>
			</view>
		</template>
		<!--
		<template #content>
			頁面slot内容
		</template> -->
	</vol-edit>
</template>
<script setup>
	import options from "./Demo_OrderOptions.js";
	import {
		onLoad
	} from '@dcloudio/uni-app'
	import {
		defineComponent,
		ref,
		reactive,
		getCurrentInstance,
		defineEmits,
		defineExpose,
		defineProps,
		watch,
		nextTick
	} from "vue";

	const props = defineProps({
		id: ''
	})
	const id = ref(props.id); //編輯的主鍵值
    const isAdd=!id.value;//當前是新建還是編輯
	const {
		proxy
	} = getCurrentInstance();

	//發起請求proxy.http.get/post
	//消息提示proxy.$toast()

	//vol-edit组件對象
	const editRef = ref(null);

	//編輯、查詢、表格配置
	//要對table注册事件、格式化、按鈕等，看移動端文檔上的table示例配置
	//表單配置看移動端文檔上的表單示例配置，searchFormOptions查詢配置，editFormOptions編輯配置
	const {
		table,
		editFormFields,
		editFormOptions,
		detail,
		details
	} = reactive(options());
	
	editFormOptions.splice(0)
	editFormOptions.push(... [{
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
				"title": "總價",
				"field": "TotalPrice",
				"type": "decimal"
			},
			{
				"title": "總數量",
				"field": "TotalQty",
				"type": "number"
			}
		],
		{
			"title": "客户名稱",
			"align": "left",
			"field": "Customer"
		},
		{
			"type": "group"
		},
		{
			"title": "訂單日期",
			"required": true,
			"field": "OrderDate",
			"type": "datetime"
		},
		{
			"title": "備註",
			"field": "Remark"
		}
	])

	//下拉框、日期、radio選擇事件
	const onChange = (field, value, item, data) => {}
    
	//表單數據加載后方法
	const loadFormAfter=(result)=>{
		//isAdd通過判斷是新還是編輯狀態，可以頁面加載后設置一些其他默認值(新建/編輯都可使用)
		//editFormFields.字段=值;
	}

	//新建、編輯保存前
	const saveBefore = (formData, isAdd, callback) => {
		callback(true); //返回false，不會保存
	}

	//新建、編輯保存后
	const saveAfter = (res, isAdd) => {}

	//主表删除前方法
	const delBefore =async (id,fields) => {
		return true; //返回false不會執行删除
	}
	//明细表删除前
	const delRowBefore = async (rows, table, ops) => {
		// await proxy.http.post(url,{}).then(x => {		
		// })
		return true
	}

	//明细表删除后前
	const delRowAfter = (rows, table, ops) => {
	}

	//如果是其他頁面跳轉過来的，獲取頁面跳轉参數
	onLoad((ops) => {})

	//監聽表單輸入，做實時計算
	// watch(
	// 	() => editFormFields.字段,
	// 	(newValue, oldValue) => {
	// 	})
</script>
<style lang="less" scoped>

</style>
