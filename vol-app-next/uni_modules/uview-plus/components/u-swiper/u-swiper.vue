<template>
	<view
		class="u-swiper"
		:style="{
			backgroundColor: bgColor,
			height: addUnit(height),
			borderRadius: addUnit(radius)
		}"
	>
		<view
			class="u-swiper__loading"
			v-if="loading"
		>
			<up-loading-icon mode="circle"></up-loading-icon>
		</view>
		<swiper
			v-else
			class="u-swiper__wrapper"
			:style="{
				flex: '1',
				height: addUnit(height)
			}"
			@change="change"
			:circular="circular"
			:interval="interval"
			:duration="duration"
			:autoplay="autoplay"
			:current="current"
			:currentItemId="currentItemId"
			:previousMargin="addUnit(previousMargin)"
			:nextMargin="addUnit(nextMargin)"
			:acceleration="acceleration"
			:displayMultipleItems="displayMultipleItems"
			:easingFunction="easingFunction"
		>
			<swiper-item
				class="u-swiper__wrapper__item"
				v-for="(item, index) in list"
				:key="index"
			>
				<slot :item="item" :index="index">
					<view
						class="u-swiper__wrapper__item__wrapper"
						:style="[itemStyle(index)]"
					>
						<!-- 在nvue中，image圖片的寬度默認為屏幕寬度，需要通過flex:1撑開，另外必須設置高度才能顯示圖片 -->
						<image
							class="u-swiper__wrapper__item__wrapper__image"
							v-if="getItemType(item) === 'image'"
							:src="getSource(item)"
							:mode="imgMode"
							@tap="clickHandler(index)"
							:style="{
								height: addUnit(height),
								borderRadius: addUnit(radius)
							}"
						></image>
						<video
							class="u-swiper__wrapper__item__wrapper__video"
							v-if="getItemType(item) === 'video'"
							:id="`video-${index}`"
							:enable-progress-gesture="false"
							:src="getSource(item)"
							:poster="getPoster(item)"
							:title="showTitle && testObject(item) && item.title ? item.title : ''"
							:style="{
								height: addUnit(height)
							}"
							controls
							@tap="clickHandler(index)"
						></video>
						<view v-if="showTitle && testObject(item) && item.title && testImage(getSource(item))"
							class="u-swiper__wrapper__item__wrapper__title">
							<text class="u-line-1">{{ item.title }}</text>
						</view>
					</view>
				</slot>
			</swiper-item>
		</swiper>
		<view class="u-swiper__indicator" :style="[addStyle(indicatorStyle)]">
			<slot name="indicator">
				<up-swiper-indicator
					v-if="!loading && indicator && !showTitle"
					:indicatorActiveColor="indicatorActiveColor"
					:indicatorInactiveColor="indicatorInactiveColor"
					:length="list.length"
					:current="currentIndex"
					:indicatorMode="indicatorMode"
				></up-swiper-indicator>
			</slot>
		</view>
	</view>
</template>
<script>
	import { props } from './props.js';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, error } from '../../libs/function/index';
	import test from '../../libs/function/test';
	/**
	 * Swiper 轮播圖
	 * @description 該组件一般用于导航轮播，广告展示等场景,可開箱即用，
	 * @tutorial https://ijry.github.io/uview-plus/components/swiper.html
	 * @property {Array}			list					轮播圖數據
	 * @property {Boolean}			indicator				是否顯示面板指示器（默認 false ）
	 * @property {String}			indicatorActiveColor	指示器非激活顏色（默認 '#FFFFFF' ）
	 * @property {String}			indicatorInactiveColor	指示器的激活顏色（默認 'rgba(255, 255, 255, 0.35)' ）
	 * @property {String | Object}	indicatorStyle			指示器樣式，可通過bottom，left，right进行定位
	 * @property {String}			indicatorMode			指示器模式（默認 'line' ）
	 * @property {Boolean}			autoplay				是否自動切換（默認 true ）
	 * @property {String | Number}	current					當前所在滑块的 index（默認 0 ）
	 * @property {String}			currentItemId			當前所在滑块的 item-id ，不能與 current 被同時指定
	 * @property {String | Number}	interval				滑块自動切換時間間隔（ms）（默認 3000 ）
	 * @property {String | Number}	duration				滑块切換過程所需時間（ms）（默認 300 ）
	 * @property {Boolean}			circular				播放到末尾后是否重新回到開頭（默認 false ）
	 * @property {String | Number}	previousMargin			前邊距，可用于露出前一項的一小部分，nvue和支付宝不支持（默認 0 ）
	 * @property {String | Number}	nextMargin				后邊距，可用于露出后一項的一小部分，nvue和支付宝不支持（默認 0 ）
	 * @property {Boolean}			acceleration			當開啟時，會根據滑動速度，连续滑動多屏，支付宝不支持（默認 false ）
	 * @property {Number}			displayMultipleItems	同時顯示的滑块數量，nvue、支付宝小程序不支持（默認 1 ）
	 * @property {String}			easingFunction			指定swiper切換缓動動画類型， 只對微信小程序有效（默認 'default' ）
	 * @property {String}			keyName					list數组中指定對象的目標屬性名（默認 'url' ）
	 * @property {String}			imgMode					圖片的裁剪模式（默認 'aspectFill' ）
	 * @property {String | Number}	height					组件高度（默認 130 ）
	 * @property {String}			bgColor					背景顏色（默認 	'#f3f4f6' ）
	 * @property {String | Number}	radius					组件圆角，數值或带單位的字符串（默認 4 ）
	 * @property {Boolean}			loading					是否加載中（默認 false ）
	 * @property {Boolean}			showTitle				是否顯示標題，要求數组對象中有title屬性（默認 false ）
	 * @event {Function(index)}	click	點擊轮播圖時触發	index：點擊了第几張圖片，從0開始
	 * @event {Function(index)}	change	轮播圖切換時触發(自動或者手動切換)	index：切換到了第几張圖片，從0開始
	 * @example	<u-swiper :list="list4" keyName="url" :autoplay="false"></u-swiper>
	 */
	export default {
		name: 'u-swiper',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				currentIndex: 0
			}
		},
		watch: {
			current(val, preVal) {
				if(val === preVal) return;
				this.currentIndex = val; // 和上游數據關聯上
			}
		},
		emits: ["click", "change"],
		computed: {
			itemStyle() {
				return index => {
					const style = {}
					// #ifndef APP-NVUE || MP-TOUTIAO
					// 左右流出空間的写法不支持nvue和頭條
					// 只有配置了此二值，才加上對應的圆角，以及缩放
					if (this.nextMargin && this.previousMargin) {
						style.borderRadius = addUnit(this.radius)
						if (index !== this.currentIndex) style.transform = 'scale(0.92)'
					}
					// #endif
					return style
				}
			}
		},
		methods: {
			addStyle,
			addUnit,
			testObject: test.object,
			testImage: test.image,
			getItemType(item) {
				if (typeof item === 'string') return test.video(this.getSource(item)) ? 'video' : 'image'
				if (typeof item === 'object' && this.keyName) {
				if (!item.type) return test.video(this.getSource(item)) ? 'video' : 'image'
				if (item.type === 'image') return 'image'
				if (item.type === 'video') return 'video'
				return 'image'
				}
			},
			// 獲取目標路徑，可能數组中為字符串，對象的形式，額外可指定對象的目標屬性名keyName
			getSource(item) {
				if (typeof item === 'string') return item
				if (typeof item === 'object' && this.keyName) return item[this.keyName]
				else error('請按格式傳递列表参數')
				return ''
			},
			// 轮播切換事件
			change(e) {
				// 當前的激活索引
				const {
					current
				} = e.detail
				this.pauseVideo(this.currentIndex)
				this.currentIndex = current
				this.$emit('change', e.detail)
			},
			// 切換轮播時，暂停视频播放
			pauseVideo(index) {
				const lastItem = this.getSource(this.list[index])
				if (test.video(lastItem)) {
					// 當视频隐藏時，暂停播放
					const video = uni.createVideoContext(`video-${index}`, this)
					video.pause()
				}
			},
			// 當一個轮播item為视频時，獲取它的视频海報
			getPoster(item) {
				return typeof item === 'object' && item.poster ? item.poster : ''
			},
			// 點擊某個item
			clickHandler(index) {
				this.$emit('click', index)
			}
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	
	.u-swiper__wrapper {
		flex: 1;
	}
	.u-swiper {
		@include flex;
		justify-content: center;
		align-items: center;
		position: relative;
		overflow: hidden;

		&__wrapper {
			flex: 1;

			&__item {
				flex: 1;

				&__wrapper {
					@include flex;
					position: relative;
					overflow: hidden;
					transition: transform 0.3s;
					flex: 1;

					&__image {
						flex: 1;
					}

					&__video {
						flex: 1;
					}

					&__title {
						position: absolute;
						background-color: rgba(0, 0, 0, 0.3);
						bottom: 0;
						left: 0;
						right: 0;
						font-size: 28rpx;
						padding: 12rpx 24rpx;
						color: #FFFFFF;
						flex: 1;
					}
				}
			}
		}

		&__indicator {
			position: absolute;
			bottom: 10px;
		}
	}
</style>
