<template>
	<view class="u-gap" :style="[gapStyle]"></view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, addUnit, deepMerge } from '../../libs/function/index.js';
	/**
	 * gap 間隔槽
	 * @description 該组件一般用于内容块之間的用一個灰色块隔開的场景，方便用户风格统一，减少工作量
	 * @tutorial https://ijry.github.io/uview-plus/components/gap.html
	 * @property {String}			bgColor			背景顏色 （默認 'transparent' ）
	 * @property {String | Number}	height			分割槽高度，單位px （默認 20 ）
	 * @property {String | Number}	marginTop		與前一個组件的距离，單位px（ 默認 0 ）
	 * @property {String | Number}	marginBottom	與后一個组件的距离，單位px （默認 0 ）
	 * @property {Object}			customStyle		定義需要用到的外部樣式
	 * 
	 * @example <u-gap height="80" bg-color="#bbb"></u-gap>
	 */
	export default {
		name: "u-gap",
		mixins: [mpMixin, mixin, props],
		computed: {
			gapStyle() {
				const style = {
					backgroundColor: this.bgColor,
					height: addUnit(this.height),
					marginTop: addUnit(this.marginTop),
					marginBottom: addUnit(this.marginBottom),
				}
				return deepMerge(style, addStyle(this.customStyle))
			}
		}
	};
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
</style>
