<template>
	<view class="table-demo">
		<view style="padding:20rpx 20rpx 0rpx 20rpx">
			<vol-alert type="primary">
				<view>框架内部已實現分頁自動加載數據、table高度自適應，只需自定義表格内容</view>
			</vol-alert>
		</view>
		<view class="search">
			<u-search @search="loadData" placeholder="請輸工序名稱" v-model="processName" @custom="loadData" @clear="loadData"
				:showAction="true" clearabled actionText="搜索"></u-search>
		</view>
		<vol-table :height='0' 
			:table-data="tableData" :columns="columns">
			<template #data>
				<view class="fx-item" @click="rowClick(row)" :class="{'fx-item-activd':!!row.checked}"
					v-for="(row,index) in tableData" :key="index">
					<!-- 	表頭 -->
					<view class="item-title">
						<view class="border-name"></view>
						<view style="flex: 1;">{{row.processCode}}</view>
						<view>{{row.submitTime}}</view>
						<view>
							<u-icon color="rgb(217 217 217)" name="arrow-right"></u-icon>
						</view>
					</view>
					<!-- 	内容部分 -->
					<view class="fx fx-item-row">
						<view class="fx-1 fx-row">
							<text class="fx-text">工序編號:</text>
							<view class="fx-value">{{row.processCode}}</view>
						</view>
						<view class="fx-1 fx-row">
							<text class="fx-text">工序:</text>
							<view class="fx-value">{{row.processName}}</view>
						</view>

					</view>
					<view class="fx fx-item-row">
						<view class="fx-1 fx-row">
							<text class="fx-text">物料名稱:</text>
							<view class="fx-value">{{row.materialName}}</view>
						</view>
						<view class="fx-1 fx-row">
							<text class="fx-text">用料數量:</text>
							<view class="fx-value">{{row.materialAmount}}</view>
						</view>

					</view>
					<view class="fx fx-item-row">
						<view class="fx-1 fx-row">
							<text class="fx-text">提交人員:</text>
							<view class="fx-value">{{row.submitter}}</view>
						</view>
						<view class="fx-1 fx-row">
							<text class="fx-text">關鍵工序:</text>
							<view class="fx-value">
								<u-tag v-if="index%2" text="是" size="mini" type="error" plainFill plain></u-tag>
								<u-tag v-else text="否" size="mini" type="success" plainFill plain></u-tag>
							</view>
						</view>

					</view>
					<view class="fx fx-item-row">
						<view class="fx-1 fx-row">
							<text class="fx-text">提交時間:</text>
							<view class="fx-value">{{row.submitTime}}</view>
						</view>
					</view>
					<view style="margin-bottom: 20rpx;">
						<vol-alert type="primary">
							<view style="color: #3c9cff;">檢查已派工未完成工作</view>
						</vol-alert>
					</view>
					<view class="fx">
						<view class="fx-1"></view>
						<view class="fx btns">
							<view class="btn">
								<vol-button size="small" type="default">查看</vol-button>
							</view>
							<view class="btn">
								<vol-button size="small" type="primary">完成</vol-button>
							</view>
						</view>
					</view>
				</view>
			</template>
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
	
	
	const processName=ref()
	
	const columns = ref([{
			"title": "工序編號",
			"field": "processCode",
			"width": 80
		},
		{
			"title": "工序名稱",
			"field": "processName",
			"width": 85
		},
		{
			"title": "物料名稱",
			"field": "materialName",
			"width": 80
		},
		{
			"title": "用料數量",
			"field": "materialAmount",
			"width": 80
		},
		{
			"title": "提交人員",
			"field": "submitter",
			"width": 80,
			hidden: false
		},
		{
			"title": "提交時間",
			"field": "submitTime",
			"width": 170,
			type: "date",
			hidden: false
		}
	]);
	const tableData = ref([{
			"processCode": "PR001",
			"processName": "插軸",
			"materialName": "螺絲",
			"materialAmount": 100,
			"submitter": "張三",
			"submitTime": "2022-01-15 08:20"
		},
		{
			"processCode": "PR002",
			"processName": "整平",
			"materialName": "電路板",
			"materialAmount": 50,
			"submitter": "李四",
			"submitTime": "2022-01-16 10:30"
		},
		{
			"processCode": "PR003",
			"processName": "下蓋打碼",
			"materialName": "包裝盒",
			"materialAmount": 200,
			"submitter": "王五",
			"submitTime": "2022-01-17 14:45"
		},
		{
			"processCode": "PR004",
			"processName": "半成品组裝",
			"materialName": "螺絲",
			"materialAmount": 150,
			"submitter": "趙六",
			"submitTime": "2022-01-18 09:20"
		},
		{
			"processCode": "PR005",
			"processName": "半成品清洗",
			"materialName": "外殼",
			"materialAmount": 80,
			"submitter": "錢七",
			"submitTime": "2022-01-19 11:30"
		},
		{
			"processCode": "PR006",
			"processName": "成品组裝",
			"materialName": "主板",
			"materialAmount": 120,
			"submitter": "孫八",
			"submitTime": "2022-01-20 13:20"
		},
		{
			"processCode": "PR007",
			"processName": "動檢測試",
			"materialName": "顯示屏",
			"materialAmount": 90,
			"submitter": "周九",
			"submitTime": "2022-01-21 15:30"
		},
		{
			"processCode": "PR008",
			"processName": "外觀檢查",
			"materialName": "外殼",
			"materialAmount": 100,
			"submitter": "吳十",
			"submitTime": "2022-01-22 17:20"
		},
		{
			"processCode": "PR009",
			"processName": "包裝",
			"materialName": "包裝盒",
			"materialAmount": 200,
			"submitter": "李麗",
			"submitTime": "2022-01-23 08:20"
		},
		{
			"processCode": "PR010",
			"processName": "標簽貼附",
			"materialName": "標簽",
			"materialAmount": 300,
			"submitter": "陳麗",
			"submitTime": "2022-01-24 10:30"
		},
		{
			"processCode": "PR011",
			"processName": "包裝箱裝配",
			"materialName": "包裝箱",
			"materialAmount": 150,
			"submitter": "王明",
			"submitTime": "2022-01-25 14:45"
		},
		{
			"processCode": "PR012",
			"processName": "打包",
			"materialName": "產品",
			"materialAmount": 250,
			"submitter": "李軍",
			"submitTime": "2022-01-26 16:20"
		},
		{
			"processCode": "PR013",
			"processName": "運輸",
			"materialName": "成品",
			"materialAmount": 400,
			"submitter": "趙敏",
			"submitTime": "2022-01-27 18:20"
		},
		{
			"processCode": "PR014",
			"processName": "入庫",
			"materialName": "成品",
			"materialAmount": 400,
			"submitter": "孫濤",
			"submitTime": "2022-01-28 20:20"
		},
		{
			"processCode": "PR015",
			"processName": "出庫",
			"materialName": "成品",
			"materialAmount": 400,
			"submitter": "劉芳",
			"submitTime": "2022-01-29 22:20"
		}
	]);

	const rowClick = (row) => {
		console.log('aaa')
		row.checked = !row.checked;
	}
	const loadData = () => {
		console.log('查詢');	
	}
</script>

<style scoped lang="less">
	.table-demo {
		height: 100%;
		background: #fbfbfb;
		// padding: 20rpx;

		.fx-item {
			padding: 16rpx 20rpx;
			margin: 20rpx;
			background: #fff;
			border-radius: 10rpx;
			box-shadow: 3px 5px 11px #f0f1f2a6;

			.fx-row {
				font-size: 28rpx;
			}
		}

		.fx-item-activd {
			background: #eef6ffc7;
		}

		.fx-row {
			font-size: 28rpx !important;
		}

		.fx-item-row {
			padding: 16rpx;
		}
	}

	.item-title {
		font-weight: bold;
		padding: 3px 0 6px 0;
		border-bottom: 1px solid #fafafa;
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
		background: #fff;
	}
</style>
