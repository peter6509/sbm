<template>
	<view class="demo-container">
	<view style="padding:26rpx 26rpx 0rpx 26rpx">
			<up-notice-bar bgColor="#3a9bff1a" :speed="60" color="#007aff" text="框架大量组件都屬于自研,可控制性高、易定制、功能更强、提供低配置高效率開發、组件文檔及示例齊全。。。"></up-notice-bar>
		</view>
		<view style="padding:26rpx 26rpx 0rpx 26rpx">
			<vol-alert type="primary">
				<view>框架已實現固定表頭、左右滾動、分頁、數據自動绑定、數據源自動绑定、格式化、行、單元格點擊事件绑定、圖片文件上傳預覽顯示、及完全自定義等功能處理,只需要簡單配置即可實現90%以上常見功能
				</view>
			</vol-alert>
		</view>
		<view class="demo-list">
			<view class="list-item" :class="['list-item-'+(index+1)]" @click="itemClick(item, index)"
				v-for="(item, index) in tabs" :key="index">
				<view class="content">
					<view class="content-right">

						<view class="item-name">
							<text class="border-name"></text>{{ item.name }}
						</view>
						<view class="text">{{item.text}}</view>
					</view>
					<view class="f-icon">
						<image class="f-icon-img" src="/static/grid-bg.svg" />
					</view>
				</view>
			</view>
		</view>
	</view>
</template>
<script setup>
	import {
		ref,
		reactive,
		defineExpose,
		defineEmits,
		getCurrentInstance
	} from 'vue'

	const emit = defineEmits(['parentCall'])

	const {
		proxy
	} = getCurrentInstance()

	//默認選中項
	const activeName = ref(-1)
	const tabs = reactive([{
			name: '基礎表格',
			path: "/pages/table/base-table",
			text: '自適應寬度,無滾動條,超出隐藏或換行顯示'
		},
		{
			name: '可滾動表格',
			path: "/pages/table/scroll-table",
			text: '表格超出屏幕寬度時可滾動查看'
		},
		{
			name: '列表展示',
			path: "/pages/table/list-table",
			text: '表格以列表形式展示,適用于自定義事件處理'
		},
		{
			name: '自動分頁、接口數據',
			path: "/pages/table/api-table",
			text: 'api自動分頁獲取接口數據加載及字典轉換'
		},
		{
			name: '列表分组',
			path: "/pages/table/group-table",
			text: '每行可配置顯示多個字段,减少頁面顯示幅度'
		},
		{
			name: '完全自定義列表',
			path: "/pages/table/custom-table",
			text: '只需處理數據展示;分頁、查詢等框架完成'
		},
		{
			name: '事件處理',
			path: "/pages/table/event-table",
			text: '行點擊、單元格事件、分頁、字典加載等'
		},
		{
			name: '高度設置',
			path: "/pages/table/height-table",
			text: '可設置固定高度、自適應高度及不固定高度'
		},
		{
			name: '格式化處理',
			path: "/pages/table/format-table",
			text: '數據格式化處理與實現方式,點開查看示例'
		},
		{
			name: '顯示複選框、行號',
			path: "/pages/table/ck-table",
			text: '顯示複選框、行號，獲取選中的行、删除行'
		},
		{
			name: '自定義按鈕',
			path: "/pages/table/btn-table",
			text: '自定義按鈕及事件處理，點擊查看'
		},
		{
			name: '行内編輯、下拉框事件',
			path: "/pages/table/edit-table",
			text: '行内編輯、下拉框、日期等選擇事件'
		},
		// {
		// 	name: '彈出框編輯',
		// 	text: '行内編輯形式編輯,適用于數據量大的场景'
		// },
		// {
		// 	name: '表格搜索、排序',
		// 	text: '開啟設置表格數據搜索與設置排序'
		// }
	])


	const itemClick = (item) => {
		uni.navigateTo({
			url: item.path
		})
	}

	defineExpose({
		tabs
	})
</script>

<style lang="less" scoped>
	.demo-container {
		height: 100%;
		//background: #f8f8f8;
		background: linear-gradient(to bottom, rgba(255, 255, 255, 0), rgb(255, 255, 255), rgb(255, 255, 255)), linear-gradient(to right, #FCE3E2, #F2E5E9, #DAE2EF, #DBE3F0);
	}

	.demo-list {
		padding: 26rpx;
		display: grid;
		-moz-column-gap: 20rpx;
		column-gap: 20rpx;
		grid-template-columns: repeat(2, auto);
	}

	.list-item {
		position: relative;
		cursor: pointer;
		display: flex;
		flex-direction: column;
		justify-content: center;
		background: #ffffff;
		border-radius: 5px;
		height: 150rpx;
		overflow: hidden;
		//box-shadow: 4px 5px 10px 1px #f7f7f7;
		box-shadow: 4px 5px 10px 1px #fafafa;
		margin-bottom: 20rpx;
		padding-top: 6rpx;

		.content {
			position: relative;
			display: flex;
			align-items: center;
			height: 100%;
			padding: 36rpx 26rpx;

			.content-right {
				color: #1d252f;

				.el-icon-warning-outline {
					margin-right: 5px;
				}
			}

			.name {
				font-size: 30rpx;
				color: #000;
				font-weight: bold;
				letter-spacing: 1px;
			}
		}

		.f-icon {
			position: absolute;
			right: 5px;
			top: 8px;
			z-index: 0;
			color: #f5f5f5;

			.f-icon-img {
				width: 68rpx;
				height: 68rpx;
			}
		}
	}

	.text {
		font-size: 26rpx;
		color: #837e7e;
		margin-top: 10rpx;
		line-height: 1.4;
		letter-spacing: 1.2;
	}

	.item-name {
		display: flex;
		line-height: 1;
		font-weight: bolder;
		font-size: 26rpx;

		.border-name {
			font-weight: bold;
			//border: 4px solid #007bff;
			background: #007bff;
			padding-left: 6rpx;
			font-size: 16px;
			border-radius: 8rpx;
			margin-right: 8rpx;
		}
	}
</style>