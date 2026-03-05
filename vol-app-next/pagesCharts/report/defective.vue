<template>
	<view class="charts">
		<view class="charts-item" style="padding: 0;">
			<vol-alert type="primary">
				<view>框架提供了查詢、table组件,簡單配置即可實現功能</view>
			</vol-alert>
		</view>
		<view class="search-item">
			<view class="search-item-filter">
				<vol-form ref="formRef" :form-options="searchFormOptions" :formFields="searchFormFields">
				</vol-form>
			</view>
			<view style="margin-left: 40rpx;">
				<vol-button type="primary" size="small" @click="initData">查詢</vol-button>
			</view>
		</view>
		<view class="title">
			<vol-title title="生產情况"></vol-title>
		</view>
		<view class="charts-item" style="padding: 0;">
			<vol-table ref="tableRef" :rowStyle="rowStyle" :format="formatData" index :height='-1'
				direction="horizontal" :table-data="tableData" :columns="columns">
			</vol-table>
		</view>
		<view class="title">
			<vol-title title="不良品分析"></vol-title>
		</view>
		<view class="charts-item">
			<view style="width:100%; height:360rpx">
				<l-echart ref="chartRef"></l-echart>
			</view>
		</view>
	</view>
</template>
<!-- 注意：微信小程序開發者工具上會出現層級過高問題，請在手機上預覽或者發布 -->
<script setup>
	import {
		ref,
		onMounted,
		getCurrentInstance,
		nextTick
	} from 'vue'

	const {
		proxy
	} = getCurrentInstance();

	import * as echarts from '../echarts.esm.min.js'
	import {
		option
	} from './defective-optons.js'
	const chartRef = ref(null)
	let chartOption = option;

	//設置背景顏色
	const rowStyle = (row, index, rows) => {
		if (row.orderType == -1) {
			return {
				'background': 'rgb(253 244 230)'
			}
		}
	}
	//格式化單元格内容,這里目前不支持返回自定義標簽，待開發
	const formatData = (row, column, index) => {
		if (column.field == 'totalPrice') {
			return '￥' + ((row.totalPrice + '').replace(/\B(?=(\d{3})+(?!\d))/g, ","))
		}
		return row[column.field]
	}

	//表格配置
	const tableData = ref([{
			"name": "LCD屏幕",
			"code": "SCR1001",
			"planQty": 5000,
			"completedQty": 4800,
			"completionRate": "96%",
			"passRate": "98%",
			"passQty": 4704
		},
		{
			"name": "锂電池",
			"code": "BAT2001",
			"planQty": 3000,
			"completedQty": 2900,
			"completionRate": "97%",
			"passRate": "99%",
			"passQty": 2871
		},
		{
			"name": "主板電路",
			"code": "CBC1001",
			"planQty": 8000,
			"completedQty": 7800,
			"completionRate": "97.5%",
			"passRate": "98.5%",
			"passQty": 7701
		},
		{
			"name": "塑料外殼",
			"code": "CSN3001",
			"planQty": 10000,
			"completedQty": 9800,
			"completionRate": "98%",
			"passRate": "98.7%",
			"passQty": 9706
		},
		{
			"name": "前置摄像頭",
			"code": "CAS4001",
			"planQty": 2000,
			"completedQty": 1950,
			"completionRate": "97.5%",
			"passRate": "98.5%",
			"passQty": 1918
		}
	])

	const columns = ref([{
			title: "產品名稱",
			field: "name",
			width: 100
		}, {
			title: "產品編碼",
			field: "code",
			width: 100
		}, {
			title: "計划數量",
			field: "planQty",
		}, {
			title: "完成數量",
			field: "completedQty",
		},
		{
			title: "完成率",
			field: "completionRate",
		},
		{
			title: "合格率",
			field: "passRate",
		},
		{
			title: "合格數量",
			field: "passQty",
		}
	])

	//配置查詢條件
	const searchFormFields = ref({
		orderDate: [null, null]
	})
	const searchFormOptions = ref([{
		title: "生產日期：",
		align: "left",
		field: 'orderDate',
		range: true,
		type: 'date'
	}])

	//設置查詢默認值
	const dateNow = proxy.base.getDate()
	searchFormFields.value.orderDate[0] = proxy.base.addDays(dateNow, -3600)
	searchFormFields.value.orderDate[1] = dateNow

	//獲取接口统計信息
	const initData = async (showMsg) => {
		//黑喵數據，與order.vue操作一樣
		// const url =
		// 	`api/demo_order/getTotal?
		// 	  beginDate=${searchFormFields.value.orderDate[0]}
		// 	  &endDate=${searchFormFields.value.orderDate[1]}`
		// proxy.http.get(url, {}, true).then((res) => {})
		nextTick(async () => {
			if (!chartRef.value) return
			const myChart = await chartRef.value.init(echarts)
			myChart.setOption(chartOption)
		})
	}

	onMounted(() => {
		initData()
	})
</script>

<style scope lang="less">
	.charts {
		background: #fcfcfc;
		padding-top: 20rpx;
		height: 100%;
		overflow: auto;

		.search-item {
			display: flex;
			margin: 20rpx;
			padding-right: 20rpx;
			margin-top: 0;
			background: #fff;
			align-items: center;
			border-radius: 8rpx;

			.search-item-filter {
				flex: 1;
			}
		}

		.title {
			margin: 0 0 10rpx 26rpx;
		}

		.charts-item {
			margin: 20rpx;
			margin-top: 0;
			padding: 10rpx 18rpx;
			background: #fff;
			border-radius: 8rpx;
		}
	}
</style>
