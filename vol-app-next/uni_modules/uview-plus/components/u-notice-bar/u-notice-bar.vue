<template>
	<view
		class="u-notice-bar"
		v-if="show"
		:style="[{
			backgroundColor: bgColor
		}, addStyle(customStyle)]"
	>
		<template v-if="direction === 'column' || (direction === 'row' && step)">
			<u-column-notice
				:color="color"
				:bgColor="bgColor"
				:text="text"
				:mode="mode"
				:step="step"
				:icon="icon"
				:disable-touch="disableTouch"
				:fontSize="fontSize"
				:duration="duration"
				@close="close"
				@click="click"
			></u-column-notice>
		</template>
		<template v-else>
			<u-row-notice
				:color="color"
				:bgColor="bgColor"
				:text="text"
				:mode="mode"
				:fontSize="fontSize"
				:speed="speed"
				:url="url"
				:linkType="linkType"
				:icon="icon"
				@close="close"
				@click="click"
			></u-row-notice>
		</template>
	</view>
</template>
<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle } from '../../libs/function/index';
	/**
	 * noticeBar 滾動通知
	 * @description 該组件用于滾動通告场景，有多种模式可供選擇
	 * @tutorial https://ijry.github.io/uview-plus/components/noticeBar.html
	 * @property {Array | String}	text			顯示的内容，數组
	 * @property {String}			direction		通告滾動模式，row-横向滾動，column-竖向滾動 ( 默認 'row' )
	 * @property {Boolean}			step			direction = row時，是否使用步进形式滾動  ( 默認 false )
	 * @property {String}			icon			是否顯示左侧的音量圖標 ( 默認 'volume' )
	 * @property {String}			mode			通告模式，link-顯示右箭頭，closable-顯示右侧關闭圖標
	 * @property {String}			color			文字顏色，各圖標也會使用文字顏色 ( 默認 '#f9ae3d' )
	 * @property {String}			bgColor			背景顏色 ( 默認 '#fdf6ec' )
	 * @property {String | Number}	speed			水平滾動時的滾動速度，即每秒滾動多少px(px)，這有利于控制文字無论多少時，都能有一個恒定的速度 ( 默認 80 )
	 * @property {String | Number}	fontSize		字體大小 ( 默認 14 )
	 * @property {String | Number}	duration		滾動一個周期的時間長，單位ms ( 默認 2000 )
	 * @property {Boolean}			disableTouch	是否禁止用手滑動切換 目前HX2.6.11，只支持App 2.5.5+、H5 2.5.5+、支付宝小程序、字节跳動小程序（默認34） ( 默認 true )
	 * @property {String}			url				跳轉的頁面路徑
	 * @property {String}			linkType		頁面跳轉的類型 ( 默認 navigateTo )
	 * @property {Object}			customStyle		定義需要用到的外部樣式
	 * 
	 * @event {Function}			click			點擊通告文字触發
	 * @event {Function}			close			點擊右侧關闭圖標触發
	 * @example <u-notice-bar :more-icon="true" :list="list"></u-notice-bar>
	 */
	export default {
		name: "u-notice-bar",
		mixins: [mpMixin, mixin,props],
		data() {
			return {
				show: true
			}
		},
		emits: ["click", "close"],
		methods: {
			addStyle,
			// 點擊通告栏
			click(index) {
				this.$emit('click', index)
				if (this.url && this.linkType) {
					// 此方法写在mixin中，另外跳轉的url和linkType参數也在mixin的props中
					this.openPage()
				}
			},
			// 點擊關闭按鈕
			close() {
				this.show = false
				this.$emit('close')
			}
		}
	};
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-notice-bar {
		overflow: hidden;
		padding: 9px 12px;
		flex: 1;
	}
</style>
