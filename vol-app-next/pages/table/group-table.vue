<template>
	<view class="table-demo">
		<view style="padding:0">
			<vol-alert type="primary">
				<view>1、表格自動分頁、數據字典自動轉換、api查詢數據</view>
				<view>2、多字段分组顯示,只需簡單配置可實現大量功能</view>
			</vol-alert>
		</view>
		<view class="search">
			<u-search @search="loadData" placeholder="請輸訂單編號" v-model="orderNo" @custom="loadData" @clear="loadData"
				:showAction="true" clearabled actionText="搜索"></u-search>
		</view>
		<view style="background: #fff;">
			<u-tabs :current="orderType" @click="tabClick" :list="tabs"></u-tabs>
		</view>
		<view class="table-content">
			<vol-table ref="tableRef" index url="api/demo_order/getPageData" :height='0' direction="list"
				titleField="OrderNo" :table-data="tableData" :loadBefore="loadBefore" :loadAfter="loadAfter"
				:columns="columns" :labelWidth="70">
				<template #header="scope">
					<view class="scope-header">
						<view class="btns">
							<vol-button size="small" type="primary"
								@click="rowBtnClick(scope.data.row,scope.data.rowIndex)">按鈕[{{scope.data.rowIndex+1}}]
							</vol-button>
						</view>
					</view>
				</template>
			</vol-table>
		</view>
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


	const rowBtnClick = (row, index) => {
		proxy.$toast('點擊了第[' + (index + 1) + ']行')
	}



	const columns = ref([{
			field: 'Order_Id',
			title: 'Order_Id',
			type: 'guid',
			hidden: true,
			readonly: true
		},
		{
			field: 'OrderNo',
			title: '訂單編號',
			type: 'string',
			link: true,
			readonly: true,
			sort: true
		},
		[{
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
				title: '訂單總價',
				type: 'decimal',
				width: 70
			}
		],
		[{
				field: 'TotalQty',
				title: '物料數量',
				type: 'int',
				width: 80
			},
			{
				field: 'OrderDate',
				title: '訂單日期',
				type: 'date',
				width: 140
			}
		],
		[{
			field: 'Customer',
			title: '客户名稱',
			type: 'string',
			width: 120
		}, {
			field: 'Creator',
			title: '創建人',
			type: 'string',
			width: 120
		}],
		{
			field: 'CreateDate',
			title: '創建時間',
			type: 'datetime',
			align: 'left', //居左邊顯示
			width: 190
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

	.scope-header {
		flex: 1;
		text-align: right;
		font-weight: 400;
		font-size: 26rpx;
		display: flex;
		align-items: center;
		justify-content: flex-end;

		.btns {
			margin-bottom: 10rpx;
		}
	}
</style>
