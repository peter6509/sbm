<template>
	<view class="table-demo">
		<view style="padding:0">
			<vol-alert type="primary">
				<view>1、表格自動分頁、數據字典自動轉換、api查詢數據</view>
				<view>2、表頭固定、滾動、自適應高度(可手動設置高度)</view>
				<view>3、需要在手機上或小程序預覽</view>
			</vol-alert>
		</view>
		<view class="search">
			<u-search @search="loadData" placeholder="請輸訂單編號" v-model="orderNo" @custom="loadData" @clear="loadData" :showAction="true"
			clearabled	actionText="搜索"></u-search>
		</view>
		<view style="background: #fff;">
			<u-tabs :current="orderType" @click="tabClick" :list="tabs"></u-tabs>
		</view>
		<vol-table ref="tableRef" index url="api/demo_order/getPageData" :height='0' direction="horizontal"
			:table-data="tableData" :loadBefore="loadBefore" :loadAfter="loadAfter" :columns="columns">
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

	const orderType = ref('0')
	const tabs = ref([{
			value: "0",
			name: "全部訂單"
		},
		{
			value: "1",
			name: "銷售訂單"
		},
		{
			value: "5",
			name: "預约訂單"
		},
		{
			value: "2",
			name: "採購訂單"
		},
		{
			value: "4",
			name: "促銷訂單"
		},
		{
			value: "3",
			name: "退货訂單"
		}
	])

	const orderNo = ref('')
	const tableRef = ref(null);

	//表格加載前設置條件
	const loadBefore = (params) => {
		//為全部時不過濾條件
		if (orderType.value !== '0') {
			//設置表格的查詢條件
			params.wheres = [{
				name: "OrderType",
				value: orderType.value
			}]
		}
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
	//tabs點擊事件
	const tabClick = (item) => {
		orderType.value = item.value;
		loadData();
	}
	const loadData = () => {
		//刷新表格
		tableRef.value.load()
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
			width: 90
		},
		{
			field: 'TotalPrice',
			title: '總價',
			type: 'decimal',
			width: 70
		},
		{
			field: 'TotalQty',
			title: '總數量',
			type: 'int',
			width: 80
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

	.item-title {
		font-weight: bold;
		padding: 3px 0 6px 0;
		border-bottom: 1px solid #eee;
		display: flex;
		line-height: 1;
		margin-bottom: 8rpx;

		.border-name {
			font-weight: bold;
			background: #007bff;
			padding-left: 8rpx;
			font-size: 16px;
			border-radius: 8rpx;
			margin-right: 8rpx;
		}
	}

	.search {
		padding: 10px;

	}
</style>