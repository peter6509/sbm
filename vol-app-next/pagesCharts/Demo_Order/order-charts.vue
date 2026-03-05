<template>
	<view class="charts-item" style="padding: 0;">
		<vol-alert type="primary">
			<view>1.生成頁面增加自定義圖表统計信息</view>
			<view>2.受小程序包大小限制,代碼需放在pagesCharts目錄下並且pages.json配置頁面地址,分包處理</view>
		</vol-alert>
	</view>
	<view class="charts">
		<view class="charts-item">
			<view style="width:100%; height:400rpx">
				<l-echart ref="chartRef"></l-echart>
			</view>
		</view>
	</view>
</template>
<!-- 注意：微信小程序開發者工具上會出現層級過高問題，請在手機上預覽或者發布 -->
<script setup>
	import {ref,onMounted,getCurrentInstance,nextTick} from 'vue'
	const {proxy} = getCurrentInstance();

	import * as echarts from '../echarts.esm.min.js'
	import {option} from './order-options.js'
	const chartRef = ref(null)
	let chartOption = option;

	//配置查詢條件
	const searchFormFields = ref({
		orderDate: [null, null]
	})
	const searchFormOptions = ref([{
		title: "訂單日期：",
		align: "left",
		field: 'orderDate',
		range: true,
		type: 'date'
	}])

	//設置查詢默認值
	const dateNow = proxy.base.getDate()
	const beginDate = proxy.base.addDays(dateNow, -3600)
	
	//圖表史稱 
	const source = {1: '銷售訂單',2: '採購訂單',3: '退货訂單',4: '促銷訂單',5: '預约訂單'}
	//獲取接口统計信息
	const initData = async () => {
		const url =
			`api/demo_order/getTotal?
			  beginDate=${beginDate}
			  &endDate=${dateNow}`
		proxy.http.get(url, {}, true).then((res) => {
			//生成圖表數據
			const data = res.filter(x => {
				return x.orderType > 0
			}).map((x) => {
				return {
					name: source[x.orderType],
					value: x.totalPrice
				}
			})
			let total = (data.reduce((sum, itme) => sum + itme.value, 0) || 1).toFixed(2) * 1.0
			chartOption.title.text = '合計金額'
			chartOption.title.subtext = total
			chartOption.series[0].data = data
			nextTick(async () => {
				const myChart = await chartRef.value.init(echarts)
				myChart.setOption(chartOption)
			})
		})
	}
	onMounted(() => {
		initData()
	})
	
	defineExpose({
		initData
	})
	
</script>

<style scope lang="less">
	.charts {
		background: #fcfcfc;
		margin: 16rpx 0;

		.charts-item {
			margin: 20rpx;
			margin-top: 0;
			padding: 10rpx 18rpx;
			background: #fff;
			border-radius: 8rpx;
		}
	}
</style>