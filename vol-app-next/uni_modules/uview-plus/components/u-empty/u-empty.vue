<template>
	<view
	    class="u-empty"
	    :style="[emptyStyle]"
	    v-if="show"
	>
		<u-icon
		    v-if="!isSrc"
		    :name="mode === 'message' ? 'chat' : `empty-${mode}`"
		    :size="iconSize"
		    :color="iconColor"
		    margin-top="14"
		></u-icon>
		<image
		    v-else
		    :style="{
				width: addUnit(width),
				height: addUnit(height),
			}"
		    :src="icon"
		    mode="widthFix"
		></image>
		<text
		    class="u-empty__text"
		    :style="[textStyle]"
		>{{text ? text : icons[mode]}}</text>
		<view class="u-empty__wrap" v-if="$slots.default || $slots.$default">
			<slot />
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, deepMerge } from '../../libs/function/index';
	/**
	 * empty 内容為空
	 * @description 該组件用于需要加載内容，但是加載的第一頁數據就為空，提示一個"没有内容"的场景， 我们精心挑選了十几個场景的圖標，方便您使用。
	 * @tutorial https://ijry.github.io/uview-plus/components/empty.html
	 * @property {String}			icon		内置圖標名稱，或圖片路徑，建议绝對路徑
	 * @property {String}			text		提示文字
	 * @property {String}			textColor	文字顏色 (默認 '#c0c4cc' )
	 * @property {String | Number}	textSize	文字大小 （默認 14 ）
	 * @property {String}			iconColor	圖標的顏色 （默認 '#c0c4cc' ）
	 * @property {String | Number}	iconSize	圖標的大小 （默認 90 ）
	 * @property {String}			mode		選擇預置的圖標類型 （默認 'data' ）
	 * @property {String | Number}	width		圖標寬度，單位px （默認 160 ）
	 * @property {String | Number}	height		圖標高度，單位px （默認 160 ）
	 * @property {Boolean}			show		是否顯示组件 （默認 true ）
	 * @property {String | Number}	marginTop	组件距离上一個元素之間的距离，默認px單位 （默認 0 ）
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * 
	 * @event {Function} click 點擊组件時触發
	 * @event {Function} close 點擊關闭按鈕時触發
	 * @example <u-empty text="所谓伊人，在水一方" mode="list"></u-empty>
	 */
	export default {
		name: "u-empty",
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				icons: {
					car: '购物车為空',
					page: '頁面不存在',
					search: '没有搜索结果',
					address: '没有收货地址',
					wifi: '没有WiFi',
					order: '訂單為空',
					coupon: '没有优惠券',
					favor: '暂無收藏',
					permission: '無權限',
					history: '無历史记錄',
					news: '無新闻列表',
					message: '消息列表為空',
					list: '列表為空',
					data: '數據為空',
					comment: '暂無评论',
				}
			}
		},
		computed: {
			// 组件樣式
			emptyStyle() {
				const style = {}
				style.marginTop = addUnit(this.marginTop)
				// 合並customStyle樣式，此参數通過mixin中的props傳递
				return deepMerge(addStyle(this.customStyle), style)
			},
			// 文本樣式
			textStyle() {
				const style = {}
				style.color = this.textColor
				style.fontSize = addUnit(this.textSize)
				return style
			},
			// 判斷icon是否圖片路徑
			isSrc() {
				return this.icon.indexOf('/') >= 0
			}
		},
		methods: {
			addUnit
		}
	}
</script>

<style lang="scss" scoped>
	@import '../../libs/css/components.scss';
	$u-empty-text-margin-top:20rpx !default;
	$u-empty-slot-margin-top:20rpx !default;

	.u-empty {
		@include flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;

		&__text {
			@include flex;
			justify-content: center;
			align-items: center;
			margin-top: $u-empty-text-margin-top;
		}
	}
		.u-slot-wrap {
			@include flex;
			justify-content: center;
			align-items: center;
			margin-top:$u-empty-slot-margin-top;
		}
</style>
