<template>
	<view
	    class="u-line"
	    :style="[lineStyle]"
	>

	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, deepMerge } from '../../libs/function/index';
	/**
	 * line 线條
	 * @description 此组件一般用于顯示一根线條，用于分隔内容块，有横向和竖向两种模式，且能設置0.5px线條，使用也很簡單
	 * @tutorial https://ijry.github.io/uview-plus/components/line.html
	 * @property {String}			color		线條的顏色 ( 默認 '#d6d7d9' )
	 * @property {String | Number}	length		長度，竖向時表現為高度，横向時表現為長度，可以為百分比，带px單位的值等 ( 默認 '100%' )
	 * @property {String}			direction	线條的方向，row-横向，col-竖向 (默認 'row' )
	 * @property {Boolean}			hairline	是否顯示细线條 (默認 true )
	 * @property {String | Number}	margin		线條與上下左右元素的間距，字符串形式，如"30px"  (默認 0 )
	 * @property {Boolean}			dashed		是否虚线，true-虚线，false-實线 (默認 false )
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * @example <u-line color="red"></u-line>
	 */
	export default {
		name: 'u-line',
		mixins: [mpMixin, mixin, props],
		computed: {
			lineStyle() {
				const style = {}
				style.margin = this.margin
				// 如果是水平线條，邊框高度為1px，再通過transform缩小一半，就是0.5px了
				if (this.direction === 'row') {
					// 此處采用兼容分開写，兼容nvue的写法
					style.borderBottomWidth = '1px'
					style.borderBottomStyle = this.dashed ? 'dashed' : 'solid'
					style.width = addUnit(this.length)
					if (this.hairline) style.transform = 'scaleY(0.5)'
				} else {
					// 如果是竖向线條，邊框寬度為1px，再通過transform缩小一半，就是0.5px了
					style.borderLeftWidth = '1px'
					style.borderLeftStyle = this.dashed ? 'dashed' : 'solid'
					style.height = addUnit(this.length)
					if (this.hairline) style.transform = 'scaleX(0.5)'
				}

				style.borderColor = this.color
				return deepMerge(style, addStyle(this.customStyle))
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-line {
		/* #ifndef APP-NVUE */
		vertical-align: middle;
		/* #endif */
	}
</style>
