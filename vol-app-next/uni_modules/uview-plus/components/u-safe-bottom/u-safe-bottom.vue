<template>
	<view
		class="u-safe-bottom"
		:style="[style]"
		:class="[!isNvue && 'u-safe-area-inset-bottom']"
	>
	</view>
</template>

<script>
	import { props } from "./props.js";
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, deepMerge, addUnit, sys } from '../../libs/function/index';
	/**
	 * SafeBottom 底部安全區
	 * @description 這個適配，主要是针對IPhone X等一些底部带指示條的機型，指示條的操作區域與頁面底部存在重合，容易导致用户误操作，因此我们需要针對這些機型进行底部安全區適配。
	 * @tutorial https://ijry.github.io/uview-plus/components/safeAreaInset.html
	 * @property {type}		prop_name
	 * @property {Object}	customStyle	定義需要用到的外部樣式
	 *
	 * @event {Function()}
	 * @example <u-status-bar></u-status-bar>
	 */
	export default {
		name: "u-safe-bottom",
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				safeAreaBottomHeight: 0,
				isNvue: false,
			};
		},
		computed: {
			style() {
				const style = {};
				// #ifdef APP-NVUE || MP-TOUTIAO
				// nvue下，高度使用js計算填充
				style.height = addUnit(sys().safeAreaInsets.bottom, 'px');
				// #endif
				return deepMerge(style, addStyle(this.customStyle));
			},
		},
		mounted() {
			// #ifdef APP-NVUE
			// 標识為是否nvue
			this.isNvue = true;
			// #endif
		},
	};
</script>

<style lang="scss" scoped>
	.u-safe-bottom {
		/* #ifndef APP-NVUE */
		width: 100%;
		/* #endif */
	}
</style>
