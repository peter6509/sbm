<template>
	<view class="menu-container">
		<view class="demo-pd-30 menu-item">
			<vol-alert type="primary">
				<view class="text-item">當前為演示頁面，如果需要權限控制，請在后台管理頁面配置菜單並给用户角色分配權限</view>
			</vol-alert>
		</view>
		<view class="menu-item menu-item-data" v-for="(item,index) in menu">
			<vol-title :border="true" :title="item.name" :key="index"></vol-title>
			<view class="menu-item-grid">
				<u-grid :border="false" col="4">
					<u-grid-item v-for="(item2,index) in item.children" :key="index">
						<view @click="menuClick(item2)" class="menu-item-grid-content">
							<image style="width:80rpx;height: 80rpx;" :src="item2.icon"></image>
							<view class="grid-text">{{$ts(item2.name)}}</view>
						</view>
					</u-grid-item>
				</u-grid>
			</view>
		</view>
	</view>
</template>
<!-- 注意：因小程序包大小限制，需要分包處理，請將報表、圖表统計頁面全部創建在pagesCharts文件夹下,並在pages.json中subPackages配置頁面地址， -->
<script setup>
	import {
		ref,
		reactive,
		computed,
		defineExpose,
		defineEmits,
		getCurrentInstance,
		defineProps
	} from 'vue'

   const {proxy}=getCurrentInstance()

	const menu = ref([{
		name: "報表统計",
		children: [{
			name: "銷售统計",
			icon:"/static/icon/36.png",
			url: "/pagesCharts/report/order"
		},{
			name: "不良品數量",
			icon:"/static/icon/57.png",
			url: "/pagesCharts/report/defective"
		},{
			name: "生產看板",
			icon:"/static/icon/30.png",
			url: "/pagesCharts/report/production"
		}]
	}]);

	const menuClick = (item) => {
		if (!item.url) {
			proxy.$toast('未配置url');
			return
		}
		if (item.url[0] != '/') {
			item.url = '/' + item.url;
		}
		uni.navigateTo({
			url: item.url,
			fail: (err) => {
				proxy.$toast(`跳轉異常:${JSON.stringify(err.errMsg)}`);
			}
		})
	}
</script>

<style lang="less" scoped>
	.menu-container {
		height: 100%;
		background-color: #f3f3f3ad;
		padding-top: 26rpx;
		padding-bottom: 100rpx;
	}

	.menu-item {
		margin: 26rpx;
		margin-top: 0;

	}

	.menu-item-data {
		box-shadow: 1px 1px 9px #f9f9f97d;
		background-color: #fff;
		padding: 20rpx;
	}

	.menu-item-grid {
		margin-top: 20rpx;
	}

	.menu-item-grid-content {
		display: flex;
		flex-direction: column;
		align-items: center;
	}

	.text-item {
		padding: 4rpx;
	}

	.item-icon-text {
		height: 60rpx;
		width: 60rpx;
		padding: 12rpx;

		.item-icon-text-bg {
			height: 100%;
			background: #eee;
			border-radius: 50%;
			text-align: center;
			justify-content: center;
			display: flex;
			align-items: center;
			background: #c3b3ff;
			font-size: 30rpx;
			color: #fff;
		}
	}
</style>
