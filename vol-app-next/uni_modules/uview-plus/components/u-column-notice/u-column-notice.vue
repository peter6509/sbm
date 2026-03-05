<template>
	<view
		class="u-notice"
		@tap="clickHandler"
	>
		<slot name="icon">
			<view
				class="u-notice__left-icon"
				v-if="icon"
			>
				<u-icon
					:name="icon"
					:color="color"
					size="19"
				></u-icon>
			</view>
		</slot>
		<swiper
			:disable-touch="disableTouch"
			:vertical="step ? false : true"
			circular
			:interval="duration"
			:autoplay="true"
			class="u-notice__swiper"
			@change="noticeChange"
		>
			<swiper-item
				v-for="(item, index) in text"
				:key="index"
				class="u-notice__swiper__item"
			>
				<text
					class="u-notice__swiper__item__text u-line-1"
					:style="[textStyle]"
				>{{ item }}</text>
			</swiper-item>
		</swiper>
		<view
			class="u-notice__right-icon"
			v-if="['link', 'closable'].includes(mode)"
		>
			<u-icon
				v-if="mode === 'link'"
				name="arrow-right"
				:size="17"
				:color="color"
			></u-icon>
			<u-icon
				v-if="mode === 'closable'"
				name="close"
				:size="16"
				:color="color"
				@click="close"
			></u-icon>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, error } from '../../libs/function/index';
	import test from '../../libs/function/test';
	/**
	 * ColumnNotice 滾動通知中的垂直滾動 内部组件
	 * @description 該组件用于滾動通告场景，是其中的垂直滾動方式
	 * @tutorial https://ijry.github.io/uview-plus/components/noticeBar.html
	 * @property {Array}			text 			顯示的内容，字符串
	 * @property {String}			icon 			是否顯示左侧的音量圖標 （ 默認 'volume' ）
	 * @property {String}			mode 			通告模式，link-顯示右箭頭，closable-顯示右侧關闭圖標
	 * @property {String}			color 			文字顏色，各圖標也會使用文字顏色 （ 默認 '#f9ae3d' ）
	 * @property {String}			bgColor 		背景顏色 （ 默認 '#fdf6ec' ）
	 * @property {String | Number}	fontSize		字體大小，單位px  （ 默認 14 ）
	 * @property {String | Number}	speed			水平滾動時的滾動速度，即每秒滾動多少px(rpx)，這有利于控制文字無论多少時，都能有一個恒定的速度 （ 默認 80 ）
	 * @property {Boolean}			step			direction = row時，是否使用步进形式滾動 （ 默認 false ）
	 * @property {String | Number}	duration		滾動一個周期的時間長，單位ms （ 默認 1500 ）
	 * @property {Boolean}			disableTouch	是否禁止用手滑動切換   目前HX2.6.11，只支持App 2.5.5+、H5 2.5.5+、支付宝小程序、字节跳動小程序 （ 默認 true ）
	 * @example 
	 */
	export default {
		mixins: [mpMixin, mixin, props],
		watch: {
			text: {
				immediate: true,
				handler(newValue, oldValue) {
					if(!test.array(newValue)) {
						error('noticebar组件direction為column時，要求text参數為數组形式')
					}
				}
			}
		},
		computed: {
			// 文字内容的樣式
			textStyle() {
				let style = {}
				style.color = this.color
				style.fontSize = addUnit(this.fontSize)
				return style
			},
			// 垂直或者水平滾動
			vertical() {
				if (this.mode == 'horizontal') return false
				else return true
			},
		},
		data() {
			return {
				index:0
			}
		},
		emits: ["click", "close"],
		methods: {
			noticeChange(e){
				this.index = e.detail.current
			},
			// 點擊通告栏
			clickHandler() {
				this.$emit('click', this.index)
			},
			// 點擊關闭按鈕
			close() {
				this.$emit('close')
			}
		}
	};
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-notice {
		@include flex;
		align-items: center;
		justify-content: space-between;

		&__left-icon {
			align-items: center;
			margin-right: 5px;
		}

		&__right-icon {
			margin-left: 5px;
			align-items: center;
		}

		&__swiper {
			height: 16px;
			@include flex;
			align-items: center;
			flex: 1;

			&__item {
				@include flex;
				align-items: center;
				overflow: hidden;

				&__text {
					font-size: 14px;
					color: $u-warning;
				}
			}
		}
	}
</style>
