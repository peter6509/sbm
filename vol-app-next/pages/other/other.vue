<template>
	<view class="charts">
		<view style="padding: 0;">
			<vol-alert type="primary">
				<view>框架提供查詢、table、表單组件,簡單配置即可實現功能</view>
			</vol-alert>
			<view class=" search">
				<u-search @search="loadData" placeholder="請輸訂單編號" v-model="orderNo" @custom="loadData" @clear="loadData"
					:showAction="true" clearabled actionText="搜索"></u-search>
			</view>
		</view>
		<view>
			<view class="title">
				<vol-title title="vol-table表格"></vol-title>
			</view>
			<view class="charts-item" style="padding: 0;">
				<vol-table ref="tableRef" :rowStyle="rowStyle" :format="formatData" index :height='-1'
					direction="horizontal" :table-data="tableData" :columns="columns">
				</vol-table>
			</view>
		</view>
		<view>
			<view class="title">
				<vol-title title="vol-form表單"></vol-title>
			</view>
			<view class="charts-item" style="padding: 0;">
				<vol-form ref="formRef" :form-options="editFormOptions" :formFields="editFormFields">
				</vol-form>
			</view>
		</view>

		<view>
			<view class="title">
				<vol-title title="vol-grid宫格"></vol-title>
			</view>
			<view class="charts-item" style="padding: 0;">
				<vol-grid :data="gridData" align="center" :col="4"></vol-grid>
			</view>
		</view>

		<view>
			<view class="title">
				<vol-title title="vol-line進度條"></vol-title>
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

	const orderNo = ref();
	const search = () => {}

    const loadData=()=>{
		
	}

	//設置背景顏色
	const rowStyle = (row, index, rows) => {
		if (row.OrderType == -1) {
			return {
				'background': '#daeef7'
			}
		}
	}


	//表格配置
	const tableData = ref([{
			name: '銷售訂單',
			count: 100,
			totalPrice: 3000,
			qty: 1000
		},
		{
			name: '採購訂單',
			count: 200,
			totalPrice: 3500,
			qty: 1200
		},
		{
			name: '退貨訂單',
			count: 150,
			totalPrice: 4000,
			qty: 3000
		},
		{
			name: '匯總合計',
			orderType: -1,
			count: 3600,
			totalPrice: 9200,
			qty: 4500
		}
	])

	const columns = ref([{
		title: "訂單類型",
		field: "name",
	}, {
		title: "訂單數量",
		field: "count",
	}, {
		title: "訂單單價",
		field: "qty",
	}, {
		title: "訂單金額",
		field: "totalPrice",
		format: true, //設置啟用格式化
		width: 110,
	}])

	const editFormFields = ref({
		processCode: "G202412080000001",
		processLine: "測試第二路線第一工序",
		materialAmount: 3000,
		jobAmount: 1200,
		cj: "(ICR)成品部",
		major: 1
	});
	const editFormOptions = ref([{
			type: "input",
			"title": "工序編號",
			"required": true,
			"readonly": true,
			"field": "processCode",
			align: 'left'
		},
		{
			type: "input",
			"title": "工藝路線",
			"readonly": true,
			"field": "processLine",
			align: 'left'
		},
		[{
				"title": "用料數量",
				type: "decimal",
				"field": "materialAmount",
			},
			{
				"title": "作業次數",
				type: "number",
				"field": "jobAmount",
			}
		],
		[{
			"title": "生產車間",
			"field": "cj",
		}, {
			"title": "關鍵工序",
			"field": "major",
			type: "select",
			data: [],
			dataKey: "enable"
		}]
	])


	const gridData = ref([{
			"title": "原料檢驗",
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
			"title": "熱處理工序",
			"value": 1900
		},
		{
			"title": "車間加工",
			"value": 2400
		},
		{
			"title": "車間裝配",
			"value": 2300
		},
		{
			"title": "成品檢驗",
			"value": 3100
		}
	]);

	const lineData = [{
			title: "在制工單數",
			value: 1800,
			rate: 98,
			unit: '件'
		},
		{
			title: "在制工單計畫生產數",
			value: 1500,
			rate: 80,
			unit: '件'
		},
		{
			title: "在制工單超期生產數",
			value: 300,
			color: '#c0dfff',
			unit: '件'
		},
		{
			title: "在制工序任務數",
			value: 1200,
			rate: 75,
			unit: '件'
		},
		{
			title: "在制工序計畫生產數",
			value: 1000,
			unit: '件'
		},
		{
			title: "在制工序超期生產數",
			value: 500,
			color: '#c0dfff',
			unit: '件'
		}
	]
</script>

<style scope lang="less">
	.charts {
		background-color: #f3f3f3ad;
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

	.search {
		background: #ffff;
		padding: 20rpx;
	}
</style>