<!--
這是生成的文件，事件處理、自定義配置，見移動端文檔：表單、表格配置
Author:vol
QQ:283591387
Date:2024
-->
<template>
	
	<vol-view ref="viewRef" :table="table" :columns="columns" :table-data="tableData"
		:searchFormFields="searchFormFields" :searchFormOptions="searchFormOptions" :editFormFields="editFormFields"
		:editFormOptions="editFormOptions" @searchClick="loadData" @addClick="modelOpenBefore" :saveBefore="saveBefore"
		:saveAfter="saveAfter" :delBefore="delBefore">
		<template #content>
			<vol-alert>頁面默認自動生成内部實現自動無限滾動分頁、查詢，並可全定自定義配置頁面樣式</vol-alert>
		</template>
		<!--表格配置 -->
		<vol-table custom ref="tableRef" :url="tableUrl" @rowClick="modelOpenBefore" :loadBefore="searchBefore"
			:loadAfter="searchAfter" :direction="direction" :titleField="table.titleField" :columns="columns"
			:table-data="tableData">
			<template #data>
				<view class="fx-item fx" v-for="(row,index) in tableData" :key="index">
					<view>
						<image style="width: 140rpx;height: 140rpx;" :src='http.ipAddress+row.Img'></image>
					</view>
					<view class="fx-right fx-col">
						<view class="title"><text class="vol-tag vol-tag-error">自营</text> {{row.GoodsName}}</view>
						<view class="small-text">銷售10W+ 95%好评</view>
						<view class="price fx">
							<text class="rmb">¥</text> {{row.Price}}
							<view class="cart" @click="addCart">
								<u-icon name="shopping-cart" color="#007aff" size="25"></u-icon>
							</view>
						</view>
					</view>
				</view>
			</template>
<!-- 			<view style="height: 90rpx;"></view> -->
		</vol-table>
	</vol-view>
<!-- 	<view class="goods-carts">
		<uni-goods-nav :options="optionLeft" :fill="true" :button-group="buttonGroup" @click="onClick"
			@buttonClick="buttonClick" />
	</view> -->
</template>
<script setup>
	import options from "./Demo_GoodsOptions.js";
	import {
		onLoad
	} from '@dcloudio/uni-app'
	import {
		defineComponent,
		ref,
		reactive,
		getCurrentInstance,
		defineEmits,
		defineExpose,
		defineProps,
		watch,
		nextTick
	} from "vue";

	const {
		proxy
	} = getCurrentInstance();

	//發起請求proxy.http.get/post
	//消息提示proxy.$toast()

	//表格顯示方式:list=列表顯示，horizontal=表格顯示
	const direction = ref('list')

	//vol-view组件
	const viewRef = ref(null);
	//table组件
	const tableRef = ref(null);

	//表格數據，可以直接獲取
	const tableData = ref([]);

	//編輯、查詢、表格配置
	//要對table注册事件、格式化、按鈕等，看移動端文檔上的table示例配置
	//表單配置看移動端文檔上的表單示例配置，searchFormOptions查詢配置，editFormOptions編輯配置
	const {
		table,
		searchFormFields,
		searchFormOptions,
		editFormFields,
		editFormOptions,
		columns
	} = reactive(options());
	const tableUrl = ref('api/' + table.tableName + '/getPageData');

	//查詢前方法，可以設置默認值
	const searchBefore = (params) => {
		return true;
	}

	//查詢后方法，res返回的查詢结果
	const searchAfter = (res) => {
		nextTick(() => {
			viewRef.value.searchAfter(res);
		})
		return true;
	}

	//打開新建、編輯彈出框
	const modelOpenBefore = (row, index, obj, callback) => {
		//新建操作
		if (!row) {
			//這里可以設置默認值：editFormFields.字段=
			callback(true); //返回false，不會彈出框
			return;
		}
		//編輯
		viewRef.value.showEdit(row, index);
		//這里可以给彈出框字段設置或修改值：editFormFields.字段=
	}

	//新建、編輯保存前
	const saveBefore = (formData, isAdd, callback) => {
		callback(true); //返回false，不會保存
	}

	//新建、編輯保存后
	const saveAfter = (res, isAdd) => {

	}

	//删除前方法
	const delBefore = (ids, rows, result) => {
		return true; //返回false不會執行删除
	}

	//調用查詢
	const loadData = (params) => {
		//生成查詢條件
		params = params || viewRef.value.getSearchParameters();
		//params可以設置查詢條件
		tableRef.value.load(params);
	}

	//如果是其他頁面跳轉過来的，獲取頁面跳轉参數
	onLoad((ops) => {

	})

	const optionLeft = ref([{
		icon: 'chat',
		text: '客服'
	}, {
		icon: 'shop',
		text: '店铺',
		info: 1,
		infoBackgroundColor: '#007aff',
		infoColor: "#f5f5f5"
	}, {
		icon: 'cart',
		text: '购物车',
		info: 2
	}])
	const buttonGroup = ref([{
			text: '加入购物车',
			backgroundColor: 'linear-gradient(90deg, #FFCD1E, #FF8A18)',
			color: '#fff'
		},
		{
			text: '立即购买',
			backgroundColor: 'linear-gradient(90deg, #FE6035, #EF1224)',
			color: '#fff'
		}
	])
	
	const addCart=()=>{
		optionLeft.value[2].info=optionLeft.value[2].info+1;
	}

	const buttonClick = () => {

	}
	const onClick = () => {}

	//監聽表單輸入，做實時計算
	// watch(
	// 	() => editFormFields.字段,
	// 	(newValue, oldValue) => {

	// 	})
</script>
<style lang="less" scoped>
	.fx-item {
		margin: 20rpx;
		border-radius: 8rpx;

		.vol-tag-error {
			font-size: 22rpx !important;
			background: #ff0000;
			padding: 2rpx 6rpx;
			color: #fff;
			border-radius: 4rpx;
			margin-right: 6rpx;
			border: none;
		}

		.fx-right {
			margin-left: 20rpx;
		}

		.title {
			font-size: 28rpx;
			flex: 1;
		}

		.small-text {
			font-size: 22rpx;
			color: #828282;
		}

		.price {
			font-size: 32rpx;
			color: #ea0000;
			align-items: flex-end;

			.rmb {
				font-size: 26rpx;
				margin-right: 4rpx;
			}

			.cart {
				flex: 1;
				display: flex;
				justify-content: flex-end;
			}
		}
	}

	.goods-carts {
		/* #ifndef APP-NVUE */
		display: flex;
		/* #endif */
		flex-direction: column;
		position: fixed;
		left: 0;
		right: 0;
		/* #ifdef H5 */
		left: var(--window-left);
		right: var(--window-right);
		/* #endif */
		bottom: 0;
	}
</style>
