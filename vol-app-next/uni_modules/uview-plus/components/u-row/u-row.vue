<template>
	<view
	    class="u-row"
		ref="u-row"
	    :style="[rowStyle]"
	    @tap="clickHandler"
	>
		<slot />
	</view>
</template>

<script>
	// #ifdef APP-NVUE
	const dom = uni.requireNativePlugin('dom')
	// #endif
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, deepMerge, sleep } from '../../libs/function/index';
	/**
	 * Row 栅格系统中的行
	 * @description 通過基礎的 12 分栏，迅速簡便地創建布局 
	 * @tutorial https://ijry.github.io/uview-plus/components/layout.html
	 * @property {String | Number}	gutter		栅格間隔，左右各為此值的一半，單位px  (默認 0 )
	 * @property {String}			justify		水平排列方式(微信小程序暂不支持) 可選值為`start`(或`flex-start`)、`end`(或`flex-end`)、`center`、`around`(或`space-around`)、`between`(或`space-between`)  (默認 'start' )
	 * @property {String}			align		垂直排列方式 (默認 'center' )
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * 
	 * @event {Function} click row被點擊
	 * @example <u-row justify="space-between" customStyle="margin-bottom: 10px"></u-row>
	 */
	export default {
		name: "u-row",
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				
			}
		},
		computed: {
			uJustify() {
				if (this.justify == 'end' || this.justify == 'start') return 'flex-' + this.justify
				else if (this.justify == 'around' || this.justify == 'between') return 'space-' + this.justify
				else return this.justify
			},
			uAlignItem() {
				if (this.align == 'top') return 'flex-start'
				if (this.align == 'bottom') return 'flex-end'
				else return this.align
			},
			rowStyle() {
				const style = {
					alignItems: this.uAlignItem,
					justifyContent: this.uJustify
				}
				// 通過给u-row左右两邊的负外邊距，消除u-col在有gutter時，第一個和最后一個元素的左内邊距和右内邊距造成的影响
				if(this.gutter) {
					style.marginLeft = addUnit(-Number(this.gutter)/2)
					style.marginRight = addUnit(-Number(this.gutter)/2)
				}
				return deepMerge(style, addStyle(this.customStyle))
			}
		},
		emits: ["click"],
		methods: {
			clickHandler(e) {
				this.$emit('click')
			},
			async getComponentWidth() {
				// 延時一定時間，以確保節點渲染完成
				await sleep()
				return new Promise(resolve => {
					// uView封裝的獲取節點的方法，詳見文檔
					// #ifndef APP-NVUE
					this.$uGetRect('.u-row').then(res => {
						resolve(res.width)
					})
					// #endif
					// #ifdef APP-NVUE
					// nvue的dom模块用于獲取節點
					dom.getComponentRect(this.$refs['u-row'], (res) => {
						resolve(res.size.width)
					})
					// #endif
				})
			},
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	
	.u-row {
		@include flex;
	}
</style>
