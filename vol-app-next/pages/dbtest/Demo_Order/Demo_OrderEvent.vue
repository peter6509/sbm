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
		<!--增加删除按鈕 -->
		<template #button>
			<view class="fx-center" @click="delClick" style="font-size: 26rpx;">
				<view style="position: relative;top:2rpx">
					<u-icon size="16" name="trash"></u-icon>
				</view>
				删除
			</view>
		</template>

		<!--表格配置 -->
		<view>
			<vol-alert>
				<view>點擊單元格、行數據触發事件</view>
				<view>更多事件及按鈕绑定，見自定義按鈕頁面文檔</view>
			</vol-alert>
			<view class="btns">
				<view class="btn">
					<vol-button type="default" @click="()=>{direction='list'}">
						切換到列表顯示
					</vol-button>
				</view>
				<view class="btn">
					<vol-button type="primary" @click="()=>{direction='horizontal'}">
						切換到表格顯示
					</vol-button>
				</view>
			</view>
		</view>
		<vol-table ref="tableRef" :ck="true" :index="true" :url="tableUrl" :loadBefore="searchBefore"
			:loadAfter="searchAfter" :direction="direction" :titleField="table.titleField" :columns="columns"
			:table-data="tableData" :rowStyle="rowStyle" :columnStyle="columnStyle" @cellClick="cellClick"
			@rowClick="modelOpenBefore">
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
	const direction = ref('horizontal')

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


	//將template標簽所有代碼與下面這些配置添加到本地
	/**************自定義删除按鈕位置***************/
	const delClick = () => {
		//獲取選中的行
		const rows = tableRef.value.getSelectedRows();
		viewRef.value.del(rows)
	}

	/**************格式化配置部分***************/
	//設置整行背景顏色
	const rowStyle = (row, index, rows) => {
		if (row.OrderType == 3) {
			return {
				'background': 'rgb(197 229 255)'
			}
		}
	}
	//設置單元格背景顏色
	const columnStyle = (row, column, index) => {
		if (column.field == 'OrderNo') {
			return {
				'color': '#007aff'
			}
		}
	}

	//1.字段绑定click事件
	columns.forEach(col => {
		if (col.field == 'OrderNo') {
			col.click = true;
		}
	})
	//2.單元格點擊事件
	const cellClick = (row, column, rowIndex, rows) => {
		//判斷點擊的哪個字段
		//注意上面columns字段配置要加上click:true
		if (column.field == 'OrderNo') {
			proxy.$toast('點擊了[' + row[column.field] + ']');
		}
	}

	//(行點擊事件)打開新建、編輯彈出框
	const modelOpenBefore = (row, index, obj, callback) => {
		proxy.$toast('點擊了第[' + (index + 1) + ']行');
		//跳轉到新頁面編輯
		// uni.navigateTo({
		// 	url: "/pages/dbtest/Demo_Order/Demo_OrderEdit?id=" + ((row || {})[table.key] || ''),
		// 	fail(e) {
		// 		console.log(e)
		// 	}
		// })
	}
</script>
<style lang="less" scoped>
	.btns {
		padding: 20rpx;
	}
</style>