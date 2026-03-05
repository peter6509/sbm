<template>
	<view
	    class="u-loadmore"
	    :style="[
			addStyle(customStyle),
			{
				backgroundColor: bgColor,
				marginBottom: addUnit(marginBottom),
				marginTop: addUnit(marginTop),
				height: addUnit(height),
			},
		]"
	>
		<u-line
		    length="140rpx"
		    :color="lineColor"
		    :hairline="false"
			:dashed="dashed"
			v-if="line"
		></u-line>
		<!-- 加載中和没有更多的狀態才顯示两邊的横线 -->
		<view
		    :class="status == 'loadmore' || status == 'nomore' ? 'u-more' : ''"
		    class="u-loadmore__content"
		>
			<view
			    class="u-loadmore__content__icon-wrap"
			    v-if="status === 'loading' && icon"
			>
				<u-loading-icon
				    :color="iconColor"
				    :size="iconSize"
				    :mode="loadingIcon"
				></u-loading-icon>
			</view>
			<!-- 如果没有更多的狀態下，顯示内容為dot（粗點），加載特定樣式 -->
			<text
			    class="u-line-1"
			    :style="[loadTextStyle]"
			    :class="[(status == 'nomore' && isDot == true) ? 'u-loadmore__content__dot-text' : 'u-loadmore__content__text']"
			    @tap="loadMore"
			>{{ showText }}</text>
		</view>
		<u-line
		    length="140rpx"
		    :color="lineColor"
			:hairline="false"
			:dashed="dashed"
			v-if="line"
		></u-line>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle } from '../../libs/function/index';
	/**
	 * loadmore 加載更多
	 * @description 此组件一般用于標识頁面底部加載數據時的狀態。
	 * @tutorial https://ijry.github.io/uview-plus/components/loadMore.html
	 * @property {String}			status			组件狀態（默認 'loadmore' ）
	 * @property {String}			bgColor			组件背景顏色，在頁面是非白色時會用到（默認 'transparent' ）
	 * @property {Boolean}			icon			加載中時是否顯示圖標（默認 true ）
	 * @property {String | Number}	fontSize		字體大小（默認 14 ）
	 * @property {String | Number}	iconSize		圖標大小（默認 17 ）
	 * @property {String}			color			字體顏色（默認 '#606266' ）
	 * @property {String}			loadingIcon		加載圖標（默認 'circle' ）
	 * @property {String}			loadmoreText	加載前的提示語（默認 '加載更多' ）
	 * @property {String}			loadingText		加載中提示語（默認 '正在加載...' ）
	 * @property {String}			nomoreText		没有更多的提示語（默認 '没有更多了' ）
	 * @property {Boolean}			isDot			到上一個相邻元素的距离 （默認 false ）
	 * @property {String}			iconColor		加載中圖標的顏色 （默認 '#b7b7b7' ）
	 * @property {String}			lineColor		线條顏色（默認 #E6E8EB ）
	 * @property {String | Number}	marginTop		上邊距 （默認 10 ）
	 * @property {String | Number}	marginBottom	下邊距 （默認 10 ）
	 * @property {String | Number}	height			高度，單位px （默認 'auto' ）
	 * @property {Boolean}			line			是否顯示左邊分割线  （默認 false ）
	 * @property {Boolean}			dashed		// 是否虚线，true-虚线，false-實线  （默認 false ）
	 * @event {Function} loadmore status為loadmore時，點擊组件會發出此事件
	 * @example <u-loadmore :status="status" icon-type="iconType" load-text="loadText" />
	 */
	export default {
		name: "u-loadmore",
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				// 粗點
				dotText: "●"
			}
		},
		computed: {
			// 加載的文字顯示的樣式
			loadTextStyle() {
				return {
					color: this.color,
					fontSize: addUnit(this.fontSize),
					lineHeight: addUnit(this.fontSize),
					backgroundColor: this.bgColor,
				}
			},
			// 顯示的提示文字
			showText() {
				let text = '';
				if (this.status == 'loadmore') text = this.loadmoreText
				else if (this.status == 'loading') text = this.loadingText
				else if (this.status == 'nomore' && this.isDot) text = this.dotText;
				else text = this.nomoreText;
				return text;
			}
		},
		emits: ["loadmore"],
		methods: {
			addStyle,
			addUnit,
			loadMore() {
				// 只有在“加載更多”的狀態下才發送點擊事件，内容不满一屏時無法触發底部上拉事件，所以需要點擊来触發
				if (this.status == 'loadmore') this.$emit('loadmore');
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-loadmore {
		@include flex(row);
		align-items: center;
		justify-content: center;
		flex: 1;

		&__content {
			margin: 0 15px;
			@include flex(row);
			align-items: center;
			justify-content: center;

			&__icon-wrap {
				margin-right: 8px;
			}

			&__text {
				font-size: 14px;
				color: $u-content-color;
			}

			&__dot-text {
				font-size: 15px;
				color: $u-tips-color;
			}
		}
	}
</style>
