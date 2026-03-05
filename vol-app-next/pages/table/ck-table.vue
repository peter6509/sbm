<template>
	<view class="table-demo">
		<view style="padding:0">
			<vol-alert type="primary">
				<view>1、顯示複選框、行號，獲取選中的行、删除行</view>
			</vol-alert>
		</view>
		<view class="btns">
			<view class="btn"><vol-button size="small" type="default" @click="getRows">獲取選中行</vol-button></view>
			<view class="btn"><vol-button size="small" type="default" @click="setRows">設置選中行</vol-button></view>
			<view class="btn"><vol-button size="small" type="default" @click="delRows">删除選中行</vol-button></view>
			<view class="btn"><vol-button size="small" type="default" @click="clearRows">清除選中行</vol-button></view>
		</view>
		<view class="search">
			<u-search @search="loadData" placeholder="請輸訂單編號" v-model="orderNo" @custom="loadData" @clear="loadData"
				:showAction="true" clearabled actionText="搜索"></u-search>
		</view>
		<vol-table ref="tableRef" ck index url="api/demo_order/getPageData" :height='0' direction="horizontal"
			:table-data="tableData" :loadBefore="loadBefore" :loadAfter="loadAfter" :columns="columns"
			@ckChangeAll="ckChangeAll" @ckChange="ckChange">
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

	//獲取選中行
	const getRows = () => {
		const rows = tableData.value.filter(x => {
			return x.ck
		});
		proxy.$toast(`當前選中[${rows.length}]條數據`)
	}
	//設置選中行
	const setRows = () => {
		tableData.value.forEach((row, index) => {
			if (index < 3) {
				row.ck = true;
			} else {
				row.ck = false;
			}
		})
		proxy.$toast(`設置了前3行數據選中`)
	}
	//删除選中的行
	const delRows = () => {
		let rows = tableRef.value.getSelectedRows();
		if (!rows.length) {
			proxy.$toast(`請選中要删除的行`)
			return;
		}
		uni.showModal({
			title: "確定要删除選中的行嗎",
			success: (res) => {
				if (res.confirm) {
					rows = tableRef.value.delRows();
					//rows删除的行數據
					if (rows.length) {
						proxy.$toast(`删除了[${rows.length}]條數據`)
					}
				}
			}
		})
	}
	//清除選中的行
	const clearRows = () => {
		tableRef.value.clearSelectedRows();
		proxy.$toast(`清除成功`)
	}

	//全選操作
	const ckChangeAll = (rows, isChecked) => {
		proxy.$toast(isChecked ? '全選' : '取消全選')
		//獲取選中的行
		//const rows = tableRef.value.getSelectedRows();
	}
	//單選選中操作
	const ckChange = (row, isChecked, index) => {
		if (isChecked) {
			proxy.$toast(`選中了第${index+1}行`)
			return
		}
		proxy.$toast(`取消選中第${index+1}行`)
		return
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
			width: 60
		},
		{
			field: 'TotalPrice',
			title: '總價',
			type: 'decimal',
			width: 30
		},
		{
			field: 'TotalQty',
			title: '總數量',
			type: 'int',
			width: 45
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

	.btns {
		padding: 20rpx;
		padding-bottom: 0;
	}
</style>