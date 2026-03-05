<template>
	<view class="charts">
		<view class="charts-item" style="padding: 0;">
			<vol-alert type="primary">
				<view>框架提供了查詢、grid、line组件,簡單配置即可實現功能</view>
			</vol-alert>
		</view>
		<view class="search-item">
			<view class="search-item-filter">
				<vol-form ref="formRef" :form-options="searchFormOptions" :formFields="searchFormFields">
				</vol-form>
			</view>
			<view style="margin-left: 20rpx;">
				<vol-button type="primary" size="small" @click="initData">查詢</vol-button>
			</view>
		</view>
		<view>
			<view class="title">
				<vol-title title="生產工單"></vol-title>
			</view>
			<view class="charts-item" style="padding: 0;">
				<vol-grid background="#f0faffa3" :data="lineData" :col="3"></vol-grid>
			</view>
		</view>
		<view>
			<view class="title">
				<vol-title title="不良品分析"></vol-title>
			</view>
			<view class="charts-item">
				<view style="width:100%; height:360rpx">
					<l-echart ref="chartRef"></l-echart>
				</view>
			</view>
		</view>
		<view>
			<view class="title">
				<vol-title title="工序统計"></vol-title>
			</view>
			<view class="charts-item" style="padding: 10rpx 0;">
				<vol-grid :data="gridData" align="center" :col="4"></vol-grid>
			</view>
		</view>
		<view>
			<view class="title">
				<vol-title title="生產情况"></vol-title>
			</view>
			<view class="charts-item" style="padding: 0;">
				<vol-line :data="lineData"></vol-line>
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
	} from './production-options.js'
	const chartRef = ref(null)
	let chartOption = option;

	//配置查詢條件
	const searchFormFields = ref({
		orderDate: null,
	})
	const searchFormOptions = ref([
		[{
			title: "日期",
			align: "left",
			field: 'orderDate',
			range: false,
			type: 'month'
		}, {
			title: "狀態",
			align: "left",
			field: 'orderStatus',
			placeholder: "工單狀態",
			type: "select",
			data: [{
				key: "1",
				value: "已派工"
			}, {
				key: "0",
				value: "待派工"
			}],
			key: "" //這里可以维护字典編號，由框架自動绑定數據源
		}]
	])

	const lineData = [{
			title: "在制工單數",
			value: 1800,
			rate: 98,
			unit: '件'
		},
		{
			title: "在制工單計划生產數",
			value: 1500,
			rate: 80,
			unit: '件'
		},
		{
			title: "工制工單超期生產數",
			value: 300,
			color: '#c0dfff',
			unit: '件'
		},
		{
			title: "在制工序任务數",
			value: 1200,
			rate: 75,
			unit: '件'
		},
		{
			title: "在制工序計划生產數",
			value: 1000,
			unit: '件'
		},
		{
			title: "工制工序超期生產數",
			value: 500,
			color: '#c0dfff',
			unit: '件'
		}
	]

	const gridData = ref([{
			"title": "原材料檢验",
			"value": 2000
		},
		{
			"title": "外觀檢查",
			"value": 1500
		},
		{
			"title": "功能測試",
			"value": 2600
		},
		{
			"title": "耐久性測試",
			"value": 1800
		},
		{
			"title": "热處理工序",
			"value": 1900
		},
		{
			"title": "车間加工",
			"value": 2400
		},
		{
			"title": "车間裝配",
			"value": 2300
		},
		{
			"title": "成品檢验",
			"value": 3100
		}
	]);

	//設置查詢默認值
	const dateNow = proxy.base.getDate()
	searchFormFields.value.orderDate = dateNow;
	// searchFormFields.value.orderDate[1] = dateNow

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

	// .vol-form-item .f-form-content-select{
	// 	flex
	// }
</style>
