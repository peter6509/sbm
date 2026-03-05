<template>
	<view
	    class="u-divider"
	    :style="[addStyle(customStyle)]"
		@tap="click"
	>
		<u-line
		    :color="lineColor"
		    :customStyle="leftLineStyle"
		    :hairline="hairline"
			:dashed="dashed"
		></u-line>
		<text
		    v-if="dot"
		    class="u-divider__dot"
		>●</text>
		<text
		    v-else-if="text"
		    class="u-divider__text"
		    :style="[textStyle]"
		>{{text}}</text>
		<u-line
		    :color="lineColor"
		    :customStyle="rightLineStyle"
		    :hairline="hairline"
			:dashed="dashed"
		></u-line>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, addUnit } from '../../libs/function/index';
	/**
	 * divider 分割线
	 * @description 區隔内容的分割线，一般用于頁面底部"没有更多"的提示。
	 * @tutorial https://ijry.github.io/uview-plus/components/divider.html
	 * @property {Boolean}			dashed			是否虚线 （默認 false ）
	 * @property {Boolean}			hairline		是否细线 （默認  true ）
	 * @property {Boolean}			dot				是否以點替代文字，优先于text字段起作用 （默認 false ）
	 * @property {String}			textPosition	内容文本的位置，left-左邊，center-中間，right-右邊 （默認 'center' ）
	 * @property {String | Number}	text			文本内容
	 * @property {String | Number}	textSize		文本大小 （默認 14）
	 * @property {String}			textColor		文本顏色 （默認 '#909399' ）
	 * @property {String}			lineColor		线條顏色 （默認 '#dcdfe6' ）
	 * @property {Object}			customStyle		定義需要用到的外部樣式
	 *
	 * @event {Function}	click	divider组件被點擊時触發
	 * @example <u-divider :color="color">锦瑟無端五十弦</u-divider>
	 */
	export default {
		name:'u-divider',
		mixins: [mpMixin, mixin, props],
		computed: {
			textStyle() {
				const style = {}
				style.fontSize = addUnit(this.textSize)
				style.color = this.textColor
				return style
			},
			// 左邊线條的的樣式
			leftLineStyle() {
				const style = {}
				// 如果是在左邊，設置左邊的寬度為固定值
				if (this.textPosition === 'left') {
					style.width = '80rpx'
				} else {
					style.flex = 1
				}
				return style
			},
			// 右邊线條的的樣式
			rightLineStyle() {
				const style = {}
				// 如果是在右邊，設置右邊的寬度為固定值
				if (this.textPosition === 'right') {
					style.width = '80rpx'
				} else {
					style.flex = 1
				}
				return style
			}
		},
		emits: ["click"],
		methods: {
			addStyle,
			// divider组件被點擊時触發
			click() {
				this.$emit('click');
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import '../../libs/css/components.scss';
	$u-divider-margin:15px 0 !default;
	$u-divider-text-margin:0 15px !default;
	$u-divider-dot-font-size:12px !default;
	$u-divider-dot-margin:0 12px !default;
	$u-divider-dot-color: #c0c4cc !default;

	.u-divider {
		@include flex;
		flex-direction: row;
		align-items: center;
		margin: $u-divider-margin;

		&__text {
			margin: $u-divider-text-margin;
		}

		&__dot {
			font-size: $u-divider-dot-font-size;
			margin: $u-divider-dot-margin;
			color: $u-divider-dot-color;
		}
	}
</style>
