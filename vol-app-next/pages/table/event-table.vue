<template>
	<view class="table-demo">
		<view style="padding:0">
			<vol-alert type="primary">
				<view>1、行點擊事件、單元格點擊事件、自動分頁無限滾動</view>
				<view>2、數據加載事件、單元格顏色、背景顏色、搜索</view>
			</vol-alert>
		</view>
		<view class="search">
			<u-search @search="loadData" placeholder="請輸名稱" v-model="name" @custom="loadData" @clear="loadData"
				:showAction="true" clearabled actionText="搜索"></u-search>
		</view>
		<vol-table ref="tableRef" index url="api/Sys_Region/getPageData" :height='0' direction="horizontal"
			:table-data="tableData" :loadBefore="loadBefore" :loadAfter="loadAfter" :columns="columns"
			@cellClick="cellClick" @rowClick="rowClick" :columnStyle="columnStyle">
		</vol-table>
	</view>
</template>

<script setup>
	import {
		ref,
		defineProps,
		defineEmits,
		defineExpose,
		computed,
		getCurrentInstance,
		nextTick
	} from 'vue'

	const {
		proxy
	} = getCurrentInstance();

	const name = ref('');
	const tableRef = ref(null);
	//表格加載前設置條件
	const loadBefore = (params) => {
			console.log('數據加載后:loadBefore')
		//這里可以設置查詢條件，查詢参數設置見：http://v3.volcore.xyz/viewGrid/code.html#searchbefore-%E6%9F%A5%E8%AF%A2%E5%89%8D
		params.wheres.push({
			name: "name",
			value: name.value,
			displayType: 'like'
		})
		return true;//返回false不會加載數據
	}
	//表格加載后方法
	const loadAfter = (res) => {
		console.log('數據加載后:loadAfter')
		return true;
	}
	const loadData = () => {
		//刷新表格
		tableRef.value.load()
	}
	//單元格點擊事件
	const cellClick = (row, column, rowIndex, rows) => {
		//判斷點擊的哪個字段
		//注意下面columns字段配置要加上click:true
		if (column.field == 'code') {
			proxy.$toast('點擊了[' + row[column.field] + ']');
		}
	}

	//行點擊事件
	const rowClick = (row, rowIndex, rows) => {
		proxy.$toast('點擊了第[' + (rowIndex + 1) + ']行');
	}

	const columnStyle = (row, column, rowIndex) => {
		//可根據row.字段判斷值設置顏色
		if (column.field == 'code' && rowIndex % 2 == 1) {
			return {
				'color': '#007aff',
				'background': '#c0dfff'
			}
		}
	}

	const columns = ref([{
			field: 'id',
			title: 'id',
			type: 'int',
			width: 110,
			hidden: true
		},
		{
			field: 'code',
			title: '編碼',
			type: 'string',
			width: 80,
			click: true
		},
		// {
		// 	field: '操作',
		// 	title: '操作',
		// 	width: 80,
		//     btn:true
		// },
		{
			field: 'name',
			title: '名稱',
			type: 'string',
			width: 120,
		},
		{
			field: 'parentId',
			title: '上級編碼',
			type: 'int',
			width: 80
		},
		{
			field: 'level',
			title: '級别',
			type: 'int',
			width: 50
		},
		{
			field: 'mername',
			title: '完整地址',
			type: 'string',
			width: 200
		},
		{
			field: 'Lng',
			title: '經度',
			type: 'float',
			width: 100
		},
		{
			field: 'Lat',
			title: '緯度',
			type: 'float',
			width: 100
		},
		{
			field: 'pinyin',
			title: '拼音',
			type: 'string',
			width: 150
		}
	]);
	const tableData = ref([]);
</script>

<style scoped lang="less">
	.table-demo {
		height: 100%;
		background: #fbfbfb;
		
	}
   .search {
		padding: 10px;
		background: #fff;
	}
</style>
