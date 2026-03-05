<template>
	<view
	    :style="[style]"
	    class="u-status-bar"
	>
		<slot />
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, deepMerge, sys } from '../../libs/function/index';
	/**
	 * StatbusBar 狀態栏占位
	 * @description 本组件主要用于狀態填充，比如在自定导航栏的時候，它會自動適配一個恰當的狀態栏高度。
	 * @tutorial https://uview-plus.jiangruyi.com/components/statusBar.html
	 * @property {String}			bgColor			背景色 (默認 'transparent' )
	 * @property {String | Object}	customStyle		自定義樣式 
	 * @example <u-status-bar></u-status-bar>
	 */
	export default {
		name: 'u-status-bar',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
			}
		},
		computed: {
			style() {
				const style = {}
				// 狀態栏高度，由于某些安卓和微信開發工具無法识别css的顶部狀態栏變量，所以使用js獲取的方式
				style.height = addUnit(sys().statusBarHeight, 'px')
				style.backgroundColor = this.bgColor
				return deepMerge(style, addStyle(this.customStyle))
			}
		},
	}
</script>

<style lang="scss" scoped>
	.u-status-bar {
		// nvue會默認100%，如果nvue下，顯式写100%的話，會导致寬度不為100%而異常
		/* #ifndef APP-NVUE */
		width: 100%;
		/* #endif */
	}
</style>
