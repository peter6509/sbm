<template>
	<u-transition
	    mode="fade"
	    :customStyle="backTopStyle"
	    :show="show"
	>
		<view
		    class="u-back-top"
			:style="[contentStyle]"
		    v-if="!$slots.default && !$slots.$default"
			@click="backToTop"
		>
			<u-icon
			    :name="icon"
			    :custom-style="iconStyle"
			></u-icon>
			<text
			    v-if="text"
			    class="u-back-top__text"
			>{{text}}</text>
		</view>
		<slot v-else />
	</u-transition>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, getPx, deepMerge, error } from '../../libs/function/index';
	// #ifdef APP-NVUE
	const dom = weex.requireModule('dom')
	// #endif
	/**
	 * backTop 返回顶部
	 * @description 本组件一個用于長頁面，滑動一定距离后，出現返回顶部按鈕，方便快速返回顶部的场景。
	 * @tutorial https://uview-plus.jiangruyi.com/components/backTop.html
	 * 
	 * @property {String}			mode  		返回顶部的形狀，circle-圆形，square-方形 （默認 'circle' ）
	 * @property {String} 			icon 		自定義圖標 （默認 'arrow-upward' ） 見官方文檔示例
	 * @property {String} 			text 		提示文字 
	 * @property {String | Number}  duration	返回顶部滾動時間 （默認 100）
	 * @property {String | Number}  scrollTop	滾動距离 （默認 0 ）
	 * @property {String | Number}  top  		距离顶部多少距离顯示，單位px （默認 400 ）
	 * @property {String | Number}  bottom  	返回顶部按鈕到底部的距离，單位px （默認 100 ）
	 * @property {String | Number}  right  		返回顶部按鈕到右邊的距离，單位px （默認 20 ）
	 * @property {String | Number}  zIndex 		層級   （默認 9 ）
	 * @property {Object<Object>}  	iconStyle 	圖標的樣式，對象形式   （默認 {color: '#909399',fontSize: '19px'}）
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * 
	 * @example <u-back-top :scrollTop="scrollTop"></u-back-top>
	 */
	export default {
		name: 'u-back-top',
		mixins: [mpMixin, mixin, props],
		computed: {
			backTopStyle() {
				// 動画组件樣式
				const style = {
					bottom: addUnit(this.bottom),
					right: addUnit(this.right),
					width: '40px',
					height: '40px',
					position: 'fixed',
					zIndex: 10,
				}
				return style
			},
			show() {
				return getPx(this.scrollTop) > getPx(this.top)
			},
			contentStyle() {
				const style = {}
				let radius = 0
				// 是否圆形
				if(this.mode === 'circle') {
					radius = '100px'
				} else {
					radius = '4px'
				}
				// 為了兼容安卓nvue，只能這么分開写
				style.borderTopLeftRadius = radius
				style.borderTopRightRadius = radius
				style.borderBottomLeftRadius = radius
				style.borderBottomRightRadius = radius
				return deepMerge(style, addStyle(this.customStyle))
			}
		},
		emits: ["click"],
		methods: {
			backToTop() {
				// #ifdef APP-NVUE
				if (!this.$parent.$refs['u-back-top']) {
					error(`nvue頁面需要给頁面最外層元素設置"ref='u-back-top'`)
				}
				dom.scrollToElement(this.$parent.$refs['u-back-top'], {
					offset: 0
				})
				// #endif
				
				// #ifndef APP-NVUE
				uni.pageScrollTo({
					scrollTop: 0,
					duration: this.duration
				});
				// #endif
				this.$emit('click')
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import '../../libs/css/components.scss';
     $u-back-top-flex:1 !default;
     $u-back-top-height:100% !default;
     $u-back-top-background-color:#E1E1E1 !default;
     $u-back-top-tips-font-size:12px !default;
	.u-back-top {
		@include flex;
		flex-direction: column;
		align-items: center;
		flex:$u-back-top-flex;
		height: $u-back-top-height;
		justify-content: center;
		background-color: $u-back-top-background-color;

		&__tips {
			font-size:$u-back-top-tips-font-size;
			transform: scale(0.8);
		}
	}
</style>
