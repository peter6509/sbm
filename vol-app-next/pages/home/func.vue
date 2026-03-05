<template>
	<view class="demo-container ">
		<view class="body">
			<view>
				<view v-for="item in list" :key="item.name" class="bor-radius-32 back-fff pad-40 mar-bot-30"
					@click="itemClick(item)">
					<view class="box-center">
						<image class="mar-rig-20" style="width: 70rpx; height: 70rpx;margin-right: 20rpx;"
							:src="item.icon"></image>
						<view class="fx-1">
							<view class="item-name">
								{{item.name}}
							</view>
							<view class="item-text">
								{{item.text}}
							</view>
						</view>
						<view>
							<u-icon color="rgb(217 217 217)" name="arrow-right"></u-icon>
						</view>
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
	const list = reactive([
		{
			name: '報表分析',
			path:"/pages/report/index",
			tabbar:true,//跳轉到tabbar頁面
			icon: "/static/icon/31.png",
			text: '數據統計、圖表查詢顯示'
		},
		{
			name: '基礎表單',
			path: "/pages/form/index",
			//path: "/pages/dbtest/Demo_Order/Demo_Order",
			icon: "/static/icon/42.png",
			text: '簡單配置即可快速構建移動端表單'
		},
		{
			name: '表格组件',
			path: "/pages/table/index",
			icon: "/static/icon/36.png",
			text: '數據自動绑定、數據源、編輯、固定列等'
		},
		{
			name: '其他组件',
			icon: "/static/icon/11.png",
			path:"/pages/other/other",
			text: '其他一些常用功能及组件'
		}
	])


	const itemClick = (item) => {
		//跳轉到tabbar頁面
		if (item.tabbar) {
			uni.switchTab({
				url: item.path
			})
			return;
		}
		uni.navigateTo({
			url: item.path
		})
	}

	defineExpose({
		list
	})
</script>

<style lang="less" scoped>
	.demo-container {
		// background: linear-gradient(to bottom, rgba(255, 255, 255, 0), rgb(255, 255, 255), rgb(255, 255, 255)), linear-gradient(to right, #FCE3E2, #F2E5E9, #DAE2EF, #DBE3F0);
	}


	.back-fff {
		background-color: #fff;
	}

	.bor-radius-32 {
		border-radius: 16rpx;
	}

	.pad-40 {
		padding: 30rpx;
		box-shadow: 2px 3px 9px 2px #f2f2f2;
	}

	.mar-bot-30 {
		margin-bottom: 26rpx;
	}

	.box-center {
		display: flex;
		align-items: center;
	}

	.item-name {
		font-weight: bolder;
	}

	.item-text {
		font-size: 24rpx;
		color: #6c6c6c;
		margin-top: 4rpx;
	}
</style>
