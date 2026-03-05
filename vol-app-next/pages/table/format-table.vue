<template>
	<view class="table-demo">
		<view style="padding:0">
			<vol-alert type="primary">
				<view>1、動態内容格式化顯示、單元格顏色處理</view>
			</vol-alert>
		</view>
		<view class="search">
			<u-search @search="loadData" placeholder="請輸訂單編號" v-model="orderNo" @custom="loadData" @clear="loadData"
				:showAction="true" clearabled actionText="搜索"></u-search>
		</view>
		<vol-table ref="tableRef" index url="api/demo_order/getPageData" :height='0' direction="horizontal"
			:table-data="tableData" :loadBefore="loadBefore" :loadAfter="loadAfter" :columns="columns"
			:rowStyle="rowStyle" :columnStyle="columnStyle" :format="formatData">
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

	const orderNo = ref('')
	const tableRef = ref(null);

	//表格加載前設置條件
	const loadBefore = (params) => {
		//訂單編號搜索
		params.wheres.push({
			name: "OrderNo",
			value: orderNo.value,
			displayType: 'like'
		})
		return true;
	}
	//表格加載后方法
	const loadAfter = (res) => {
		return true;
	}

	const loadData = () => {
		//刷新表格
		tableRef.value.load()
	}

	//設置背景顏色
	const rowStyle = (row, index, rows) => {
		if (row.OrderType == 2) {
			return {
				'background': '#daeef7'
			}
		}
		if (row.OrderType == 3) {
			return {
				'background': 'rgb(197 229 255)'
			}
		}
		if (row.OrderType == 5) {
			return {
				'background': 'rgb(253 244 230)'
			}
		}
	}

	const columnStyle = (row, column, index) => {
		if (column.field == 'OrderType') {
			if (row.OrderType == 2) {

				return {
					'color': 'rgb(47 121 154)'
				}
			}
			if (row.OrderType == 3) {
				return {
					'color': '#fe2616'
				}
			}
			if (row.OrderType == 5) {
				return {
					'color': '#eb8d04'
				}
			}
		}
	}

	//格式化單元格内容,這里目前不支持返回自定義標簽，待開發
	const formatData = (row, column, index) => {
		if (column.field == 'TotalPrice') {
			return '￥' + ((row.TotalPrice+'').replace(/\B(?=(\d{3})+(?!\d))/g, ","))
		}
		return row[column.field]
	}



	const columns = ref([{
			field: 'Order_Id',
			title: 'Order_Id',
			type: 'guid',
			hidden: true,
			readonly: true
		},
		{
			field: 'OrderType',
			title: '訂單類型',
			type: 'select',
			bind: {
				key: '訂單類型',
				data: []
			},
			width: 80
		},
		{
			field: 'TotalPrice',
			title: '總價',
			type: 'decimal',
			width: 70,
			format: true //啟用格式化
		},
		{
			field: 'TotalQty',
			title: '總數量',
			type: 'int',
			width: 70
		},
		{
			field: 'OrderDate',
			title: '訂單日期',
			type: 'date',
			width: 100
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
	}
</style>