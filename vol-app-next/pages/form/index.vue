<template>
	<view class="demo-container ">
		<view class="demo-pd-30">
			<up-notice-bar bgColor="#3a9bff1a" :speed="60" color="#007aff" text="框架大量组件都屬於自研,可控制性高、易定制、功能更强、提供低配置高效率開發、组件文檔及示例齊全。。。"></up-notice-bar>
		</view>
		<view class="demo-pd-30">
			<vol-alert type="primary">
				<view>表單支持編輯、數據自動绑定、數據字典自動绑定、點擊事件、圖片文件上傳預覽顯示、表單分组顯示,只需要簡單配置即可實現90%以上常見功能</view>
			</vol-alert>
		</view>
		<view class="body">
			<view>
				<view v-for="item in list" :key="item.name" class="back-fff" @click="skipPage(item)">
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

<script>
	export default {
		data() {
			return {
				list: [{
						name: '基礎表單、只讀、數據字典绑定',
						icon: '/static/icon/42.png',
						text: "輸入框、下拉框單多選、日期、範圍輸入、級聯、radio、圖片文件上傳預覽等",
						url: '/pages/form/form1',
					},
					{
						name: '標簽顯示位置、標題寬度',
						text: "指定標簽顯示位置靠左或者右、指定標題寬度值",
						icon: '/static/icon/42.png',
						url: '/pages/form/position',
					},
					{
						name: '事件處理',
						text: "輸入框、下拉框、日期、範圍輸入、級聯、圖片文件上傳等選擇及輸入事件處理",
						icon: '/static/icon/42.png',
						url: '/pages/form/event',
					},
					{
						name: '表單字段實時計算',
						text: "監聽表單輸入或值變化時给其他字段設置值或計算值",
						icon: '/static/icon/42.png',
						url: '/pages/form/calc',
					},
					{
						name: '表單分组',
						text: "多個字段按分组顯示在一行，减少頁面幅度，並支持編輯功能",
						icon: '/static/icon/42.png',
						url: '/pages/form/group',
					},
					{
						name: '格式化處理、自定義按鈕、掃碼',
						text: "字段顯示格式化、自定義顏色、自定義按鈕、掃碼等功能",
						icon: '/static/icon/42.png',
						url: '/pages/form/format',
					}
				],
			}
		},
		methods: {
			skipPage(item) {
				uni.navigateTo({
					url: item.url
				})
			},

		},
		onLoad(options) {}
	}
</script>

<style lang="less">
	.demo-container {
		height: 100%;
		//background: #f8f8f8;
		background: linear-gradient(to bottom, rgba(255, 255, 255, 0), rgb(255, 255, 255), rgb(255, 255, 255)), linear-gradient(to right, #FCE3E2, #F2E5E9, #DAE2EF, #DBE3F0);
		width: 100%;
		height: 100%;
		position: relative;

		.body {
			padding: 30rpx;
		}
	}

	.back-fff {
		border-radius: 16rpx;
		background-color: #fff;
		padding: 30rpx;
		box-shadow: 4px 5px 7px 4px #fafafa;
		margin-bottom: 40rpx;
	}

	.box-center {
		display: flex;
		align-items: center;
		justify-content: center;
	}

	.item-text {
		font-size: 24rpx;
		color: #6c6c6c;
		margin-top: 4rpx;
	}

	.demo-pd-30 {
		padding: 30rpx 30rpx 0rpx 30rpx
	}
</style>
