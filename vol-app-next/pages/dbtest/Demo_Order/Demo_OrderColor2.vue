<!--
這是生成的文件，事件處理、自定義配置，見移動端文檔：表單、表格配置
Author:vol
QQ:283591387
Date:2024
-->
<template>
	<vol-view ref="viewRef" :table="table" :columns="columns" :table-data="tableData"
		:searchFormFields="searchFormFields" :searchFormOptions="searchFormOptions" :editFormFields="editFormFields"
		:editFormOptions="editFormOptions" @searchClick="loadData" @addClick="modelOpenBefore" :saveBefore="saveBefore"
		:saveAfter="saveAfter" :delBefore="delBefore">
		<!--表格配置 -->
		<vol-alert>
			<view>1. 生成代碼后自定義列表顏色,更多配置見table示例</view>
			<view>2. 新建點添加或者行數據进入編輯頁面也有配置</view>
		</vol-alert>
		<vol-table ref="tableRef" :ck="false" :index="true" :url="tableUrl" @rowClick="modelOpenBefore"
			:loadBefore="searchBefore" :loadAfter="searchAfter" :direction="direction" :titleField="table.titleField"
			:columns="columns" :table-data="tableData" :getStyle="getStyle">
		</vol-table>
	</vol-view>
</template>
<script setup>
	import options from "./Demo_OrderOptions.js";
	import {
		onLoad
	} from '@dcloudio/uni-app'
	import {
		ref,
		reactive,
		getCurrentInstance,
		defineEmits,
		defineExpose,
		defineProps,
		watch,
		nextTick
	} from "vue";
	const {
		proxy
	} = getCurrentInstance();
	//發起請求proxy.http.get/post
	//消息提示proxy.$toast()

	//表格顯示方式:list=列表顯示，horizontal=表格顯示
	const direction = ref('list')

	//vol-view组件
	const viewRef = ref(null);
	//table组件
	const tableRef = ref(null);

	//表格數據，可以直接獲取使用
	const tableData = ref([]);


	//編輯、查詢、表格配置
	//要對table注册事件、格式化、按鈕等，看移動端文檔上的table示例配置
	//表單配置看移動端文檔上的表單示例配置，searchFormOptions查詢配置，editFormOptions編輯配置
	const {
		table,
		searchFormFields,
		searchFormOptions,
		editFormFields,
		editFormOptions,
		columns
	} = reactive(options());
	const tableUrl = ref('api/' + table.tableName + '/getPageData');

	//tabs點擊事件
	const tabClick = (item) => {
		orderType.value = item.value;
		loadData();
	}

	//查詢前方法，可以設置查詢條件(與生成頁面文檔上的searchBefore配置一致)
	const searchBefore = (params) => {
		return true;
	}

	//查詢后方法，res返回的查詢结果
	const searchAfter = (res) => {
		nextTick(() => {
			viewRef.value.searchAfter(res);
		})
		return true;
	}

	//打開新建、編輯彈出框
	const modelOpenBefore = (row, index, obj, callback) => {
		//跳轉到新頁面編輯
		uni.navigateTo({
			url: "/pages/dbtest/Demo_Order/Demo_OrderEdit?id=" + ((row || {})[table.key] || ''),
			fail(e) {
				console.log(e)
			}
		})
	}

	//新建、編輯保存前
	const saveBefore = (formData, isAdd, callback) => {
		callback(true); //返回false，不會保存
	}

	//新建、編輯保存后
	const saveAfter = (res, isAdd) => {}

	//主表删除前方法
	const delBefore = (ids, rows, result) => {
		return true; //返回false不會執行删除
	}

	//調用表格查詢
	const loadData = (params) => {
		//生成查詢條件
		params = params || viewRef.value.getSearchParameters();
		//params可以設置查詢條件
		tableRef.value.load(params);
	}

	//如果是其他頁面跳轉過来的，獲取頁面跳轉参數
	onLoad((ops) => {})

	defineExpose({
		//對外暴露數據
	})

	/**************格式化配置部分***************/

	//格式化内容
	columns.forEach(x => {
		//設置字段的固定樣式
		if (x.field == 'TotalPrice') {
			x.itemStyle = "background:rgb(232 244 255);"; //行樣式
			x.titleStyle = "color:#007aff;"; //標題樣式
			x.valueStyle = "color:red;"; //值樣式
			x.extra = {
				text: "元",
				style: "font-size:26rpx;margin-left:6rpx;color: #6f6b6b;"
			}
		} else if (x.field == 'TotalQty') { //數量字段后面添加單位文本 
			x.extra = {
				text: "箱",
				style: "font-size:26rpx;margin-left:6rpx;color: #6f6b6b;"
			}
		}
	})

	//動態設置右邊值的樣式
	const getStyle = (fields, item, isValue) => {
		//isValue=true右邊值的樣式，false：左邊標題樣式，自己判斷
		if (item.field == 'TotalQty') {
			//設置右邊值的樣式
			if (isValue) {
				if (fields[item.field] >=300) {
					return 'color:red;font-weight:700;font-size:36rpx;'
				} else if (fields[item.field] >=200) {
					return 'color:#40c400;font-weight:700;font-size:36rpx;'
				} else {
					return 'color:#007aff;font-weight:700;font-size:36rpx;'
				}
			}
			//設置左邊標簽樣式,可以判斷不同的值設置不同的標簽顏色
			return 'color: #007aff;'
		}
	}
</script>
<style lang="less" scoped>
	.summary {
		padding: 20rpx 0;

		.txt {
			margin-left: 20rpx;
			font-size: 26rpx;
		}
	}
</style>
