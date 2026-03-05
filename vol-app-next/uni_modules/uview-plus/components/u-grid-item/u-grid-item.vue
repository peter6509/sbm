<template>
	<!-- #ifndef APP-NVUE -->
	<view
		v-if="parentData.col > 0"
	    class="u-grid-item"
	    hover-class="u-grid-item--hover-class"
	    :hover-stay-time="200"
	    @tap="clickHandler"
	    :class="classes"
	    :style="[itemStyle]"
	>
		<slot />
	</view>
	<!-- #endif -->
	<!-- #ifdef APP-NVUE -->
	<view
	    class="u-grid-item"
	    :hover-stay-time="200"
	    @tap="clickHandler"
	    :class="classes"
	    :style="[itemStyle]"
	>
		<slot />
	</view>
	<!-- #endif -->
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, deepMerge } from '../../libs/function/index';
	/**
	 * gridItem 提示
	 * @description 宫格组件一般用于同時展示多個同類項目的场景，可以给宫格的項目設置徽標组件(badge)，或者圖標等，也可以扩展為左右滑動的轮播形式。搭配u-grid使用
	 * @tutorial https://ijry.github.io/uview-plus/components/grid.html
	 * @property {String | Number}	name		宫格的name ( 默認 null )
	 * @property {String}			bgColor		宫格的背景顏色 （默認 'transparent' ）
	 * @property {Object}			customStyle	自定義樣式，對象形式
	 * @event {Function} click 點擊宫格触發
	 * @example <u-grid-item></u-grid-item>
	 */
	export default {
		name: "u-grid-item",
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				parentData: {
					col: 0, // 父组件划分的宫格數
					border: true, // 是否顯示邊框，根據父组件决定
				},
				// #ifdef APP-NVUE
				width: 0, // nvue下才這么計算，vue下放到computed中，否则會因為延時造成闪烁
				// #endif
				classes: [], // 類名集合，用于判斷是否顯示右邊和下邊框
			};
		},
		mounted() {
			this.init()
		},
		emits: ['click'],
		//  微信小程序中 options 選項
		// #ifdef MP-WEIXIN
		options: {
		    virtualHost: true ,//將自定義節點設置成虚拟的，更加接近Vue组件的表現。我们不希望自定義组件的這個節點本身可以設置樣式、响應 flex 布局等
		},
		// #endif
		computed: {
			itemStyle() {
				const style = {
					background: this.bgColor
				}
				// #ifdef APP-NVUE
				style['width'] = this.width
				// #endif
				// #ifndef APP-NVUE
				style['width'] = '100%'
				// #endif
				return deepMerge(style, addStyle(this.customStyle))
			}
		},
		methods: {
			init() {
				// 用于在父组件u-grid的children中被添加入子组件時，
				// 重新計算item的邊框
				uni.$on('$uGridItem', () => {
					this.gridItemClasses()
				})
				// 父组件的實例
				this.updateParentData()
				// #ifdef APP-NVUE
				// 獲取元素該有的長度，nvue下要延時才准確
				this.$nextTick(function(){
					this.getItemWidth()
				})
				// #endif
				// 發出事件，通知所有的grid-item都重新計算自己的邊框
				uni.$emit('$uGridItem')
				this.gridItemClasses()
			},
			// 獲取父组件的参數
			updateParentData() {
				// 此方法写在mixin中
				this.getParentData('u-grid');
			},
			clickHandler() {
				let name = this.name
				// 如果没有設置name屬性，历遍父组件的children數组，判斷當前的元素是否和本實例this相等，找出當前组件的索引
				const children = this.parent?.children
				if(children && this.name === null) {
					name = children.findIndex(child => child === this)
				}
				// 調用父组件方法，發出事件
				this.parent && this.parent.childClick(name)
				this.$emit('click', name)
			},
			async getItemWidth() {
				// 如果是nvue，不能使用百分比，只能使用固定寬度
				let width = 0
				if(this.parent) {
					// 獲取父组件寬度后，除以栅格數，得出每個item的寬度
					const parentWidth = await this.getParentWidth()
					width = parentWidth / Number(this.parentData.col) + 'px'
				}
				this.width = width
			},
			// 獲取父元素的尺寸
			getParentWidth() {
				// #ifdef APP-NVUE
				// 返回一個promise，让調用者可以用await同步獲取
				const dom = uni.requireNativePlugin('dom')
				return new Promise(resolve => {
					// 調用父组件的ref
					dom.getComponentRect(this.parent.$refs['u-grid'], res => {
						resolve(res.size.width)
					})
				})
				// #endif
			},
			gridItemClasses() {
				if(this.parentData.border) {
					let classes = []
					this.parent.children.map((child, index) =>{
						if(this === child) {
							const len = this.parent.children.length
							// 貼近右邊屏幕邊沿的child，並且最后一個（比如只有横向2個的時候），無需右邊框
							if((index + 1) % this.parentData.col !== 0 && index + 1 !== len) {
								classes.push('u-border-right')
							}
							// 總的宫格數量對列數取餘的值
							// 如果取餘后，值為0，则意味着要將最后一排的宫格，都不需要下邊框
							const lessNum = len % this.parentData.col === 0 ? this.parentData.col : len % this.parentData.col
							// 最下面的一排child，無需下邊框
							if(index < len - lessNum) {
								classes.push('u-border-bottom')
							}
						}
					})
					// 支付宝，頭條小程序無法動態绑定一個數组類名，否则解析出来的结果會带有","，而导致失效
					// #ifdef MP-ALIPAY || MP-TOUTIAO
					classes = classes.join(' ')
					// #endif
					this.classes = classes
				}
			}
		},
		beforeUnmount() {
			// 移除事件監聽，释放性能
			uni.$off('$uGridItem')
		}
	};
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
      $u-grid-item-hover-class-opcatiy:.5 !default;
      $u-grid-item-margin-top:1rpx !default;
      $u-grid-item-border-right-width:0.5px !default;
      $u-grid-item-border-bottom-width:0.5px !default;
      $u-grid-item-border-right-color:$u-border-color !default;
      $u-grid-item-border-bottom-color:$u-border-color !default;
	.u-grid-item {
		align-items: center;
		justify-content: center;
		position: relative;
		flex-direction: column;
		/* #ifndef APP-NVUE */
		box-sizing: border-box;
		display: flex;
		/* #endif */

		/* #ifdef MP */
		position: relative;
		float: left;
		/* #endif */

		/* #ifdef MP-WEIXIN */
		margin-top:$u-grid-item-margin-top;
		/* #endif */

		&--hover-class {
			opacity:$u-grid-item-hover-class-opcatiy;
		}
	}

	/* #ifdef APP-NVUE */
	// 由于nvue不支持组件内引入app.vue中再引入的樣式，所以需要写在這里
	.u-border-right {
		border-right-width:$u-grid-item-border-right-width;
		border-color: $u-grid-item-border-right-color;
	}

	.u-border-bottom {
		border-bottom-width:$u-grid-item-border-bottom-width;
		border-color:$u-grid-item-border-bottom-color;
	}

	/* #endif */
</style>
