<template>
	<u-transition
	    :show="show"
	    custom-class="u-overlay"
	    :duration="duration"
	    :custom-style="overlayStyle"
	    @click="clickHandler"
	>
		<slot />
	</u-transition>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, deepMerge } from '../../libs/function/index';
	/**
	 * overlay 遮罩
	 * @description 創建一個遮罩層，用于强調特定的頁面元素，並阻止用户對遮罩下層的内容进行操作，一般用于彈窗场景
	 * @tutorial https://ijry.github.io/uview-plus/components/overlay.html
	 * @property {Boolean}			show		是否顯示遮罩（默認 false ）
	 * @property {String | Number}	zIndex		zIndex 層級（默認 10070 ）
	 * @property {String | Number}	duration	動画時長，單位毫秒（默認 300 ）
	 * @property {String | Number}	opacity		不透明度值，當做rgba的第四個参數 （默認 0.5 ）
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * @event {Function} click 點擊遮罩發送事件
	 * @example <u-overlay :show="show" @click="show = false"></u-overlay>
	 */
	export default {
		name: "u-overlay",
		mixins: [mpMixin, mixin,props],
		computed: {
			overlayStyle() {
				const style = {
					position: 'fixed',
					top: 0,
					left: 0,
					right: 0,
					zIndex: this.zIndex,
					bottom: 0,
					'background-color': `rgba(0, 0, 0, ${this.opacity})`
				}
				return deepMerge(style, addStyle(this.customStyle))
			}
		},
		emits: ["click"],
		methods: {
			clickHandler() {
				this.$emit('click')
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
     $u-overlay-top:0 !default;
     $u-overlay-left:0 !default;
     $u-overlay-width:100% !default;
     $u-overlay-height:100% !default;
     $u-overlay-background-color:rgba(0, 0, 0, .7) !default;
	.u-overlay {
		position: fixed;
		top:$u-overlay-top;
		left:$u-overlay-left;
		width: $u-overlay-width;
		height:$u-overlay-height;
		background-color:$u-overlay-background-color;
	}
</style>
