<template>
	<view class="table-demo">
		<view style="padding:0">
			<vol-alert type="primary">
					<view>(需要在手機上或小程序預覽)表頭固定,且可左右可滾動</view>
			</vol-alert>
		</view>
		<vol-table :height='0' direction="horizontal" :table-data="tableData" :columns="columns">
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
		}
	]);


	const rowStyle = (row, index, rows) => {
		return {}
	}
	const columnStyle = (row, column, field, rows) => {
		return {};
	}

	const rowClick = (row) => {
		console.log('aaa')
		row.checked = !row.checked;
	}
</script>

<style scoped lang="less">
	.table-demo {
		// height: 100%;
		// overflow-y: auto;
		background: #fbfbfb;
	}
</style>
