<!--
這是生成的文件，事件處理、自定義配置，見移動端文檔：表單、表格配置
Author:vol
QQ:283591387
Date:2024
-->
<template>
	<vol-view ref="viewRef" :table="table" :columns="columns" :table-data="tableData"
		:searchFormFields="searchFormFields" :searchFormOptions="searchFormOptions" :editFormFields="editFormFields"
		:editFormOptions="editFormOptions" @searchClick="loadData" @addClick="modelOpenBefore">
		<!--表格配置 -->
		<orderCharts ref="chartsRef"></orderCharts>
		<vol-table ref="tableRef" :min-height='200' :ck="false" :index="true" :url="tableUrl" @rowClick="modelOpenBefore"
			:loadAfter="searchAfter" :direction="direction" :columns="columns"
			:table-data="tableData">
		</vol-table>
	</vol-view>
</template>
<script setup>
	import orderCharts from '@/pagesCharts/Demo_Order/order-charts.vue'
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
		nextTick,
		onMounted
	} from "vue";
	const {
		proxy
	} = getCurrentInstance();

	//表格顯示方式:list=列表顯示，horizontal=表格顯示
	const direction = ref('horizontal')
	//vol-view组件
	const viewRef = ref(null);
	//table组件
	const tableRef = ref(null);
	//表格數據，可以直接獲取使用
	const tableData = ref([]);
	
	const chartsRef=ref()

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

	//查詢后方法，res返回的查詢结果
	const searchAfter = (res) => {
		nextTick(() => {
			viewRef.value.searchAfter(res);
			//每次查詢可以重新加載圖表數據
			//chartsRef.value.initData();
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

	//調用表格查詢
	const loadData = (params) => {
		//生成查詢條件
		params = params || viewRef.value.getSearchParameters();
		//params可以設置查詢條件
		tableRef.value.load(params);
	}
	
	//調用加載圖表數據
	// onMounted(()=>{
	// 	chartsRef.value.initData();
	// })
	
</script>
<style lang="less" scoped>
</style>