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
		:saveAfter="saveAfter" :delBefore="delBefore" :showTotal="false">
		<!--表格配置 -->
		<vol-alert>
			<view>1. 生成代碼后顯示行號、複選框,更多配置見table示例</view>
			<view>2. 增加删除按鈕，批量删除</view>
			<view>3. 按條件設置checkbox是可選中</view>
		</vol-alert>

		<!--增加删除按鈕 -->
		<template #button>
			<view class="fx-center" @click="clearSelectedRows">
				清除選中
			</view>
			<view class="fx-center" @click="getSelectedRows">
				獲取選中行
			</view>
			<view class="fx-center" @click="delClick">
				<view style="position: relative;top:2rpx">
					<u-icon size="16" name="trash"></u-icon>
				</view>
				删除
			</view>
		</template>
		<vol-table ref="tableRef" :ck="true" :index="true" :url="tableUrl" @rowClick="modelOpenBefore"
			:loadBefore="searchBefore" :loadAfter="searchAfter" :direction="direction" :titleField="table.titleField"
			:columns="columns" :table-data="tableData" :selectable="selectable" @ckChangeAll="ckChangeAll"
			@ckChange="ckChange">
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


	//將template標簽所有代碼與下面這些配置添加到本地
	/**************自定義删除按鈕位置***************/
	const delClick = () => {
		//獲取選中的行
		const rows = tableRef.value.getSelectedRows();
		viewRef.value.del(rows)
	}

	//動態設置checkbox是否可以選中
	const selectable = (row, column, rowIndex) => {
		//判斷條件
		// if (row.字段=="xx") {
		// }
		return rowIndex % 2 === 1
	}
	//全選操作
	const ckChangeAll = (rows, isChecked) => {
		proxy.$toast(isChecked ? '全選' : '取消全選')
		//獲取選中的行
		//const rows = tableRef.value.getSelectedRows();
	}
	//單選選中操作
	const ckChange = (row, isChecked, index) => {
		if (isChecked) {
			proxy.$toast(`選中了第${index+1}行`)
			return
		}
		proxy.$toast(`取消選中第${index+1}行`)
		return
	}

	//獲取選中行
	const getSelectedRows = () => {
		//獲取選中的行
		const rows = tableRef.value.getSelectedRows();
		proxy.$toast(`當前選中${rows.length}行`)
	}
	//清除選中行
	const clearSelectedRows = () => {
		tableRef.value.clearSelectedRows();
		proxy.$toast(`清除成功`)
	}

	//指定設置某行選中
	// tableData.value.forEach((row,index)=>{
	// 	 if (條件) {
	// 	 	row.ck=true;
	// 	 }
	// })
</script>
<style lang="less" scoped>
	.fx-center {
		margin-right: 16rpx;
		font-size: 26rpx;
	}
</style>