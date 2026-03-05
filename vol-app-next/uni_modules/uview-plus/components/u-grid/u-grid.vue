<template>
	<view
	    class="u-grid"
		ref='u-grid'
	    :style="[gridStyle]"
	>
		<slot />
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, deepMerge } from '../../libs/function/index';
	/**
	 * grid 宫格布局
	 * @description 宫格组件一般用于同時展示多個同類項目的场景，可以给宫格的項目設置徽標组件(badge)，或者圖標等，也可以扩展為左右滑動的轮播形式。
	 * @tutorial https://ijry.github.io/uview-plus/components/grid.html
	 * @property {String | Number}	col			宫格的列數（默認 3 ）
	 * @property {Boolean}			border		是否顯示宫格的邊框（默認 false ）
	 * @property {String}			align		宫格對齊方式，表現為數量少的時候，靠左，居中，還是靠右 （默認 'left' ）
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * @event {Function} click 點擊宫格触發
	 * @example <u-grid :col="3" @click="click"></u-grid>
	 */
	export default {
		name: 'u-grid',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				index: 0,
				width: 0
			}
		},
		watch: {
			// 當父组件需要子组件需要共享的参數發生了變化，手動通知子组件
			parentData() {
				if (this.children.length) {
					this.children.map(child => {
						// 判斷子组件(u-radio)如果有updateParentData方法的話，就就執行(執行的结果是子组件重新從父组件拉取了最新的值)
						typeof(child.updateParentData) == 'function' && child.updateParentData();
					})
				}
			},
		},
		created() {
			// 如果將children定義在data中，在微信小程序會造成循环引用而報错
			this.children = []
		},
		computed: {
			// 計算父组件的值是否發生變化
			parentData() {
				return [this.hoverClass, this.col, this.size, this.border];
			},
			// 宫格對齊方式
			gridStyle() {
				let style = {};
				switch (this.align) {
					case 'left':
						style.justifyContent = 'flex-start';
						break;
					case 'center':
						style.justifyContent = 'center';
						break;
					case 'right':
						style.justifyContent = 'flex-end';
						break;
					default:
						style.justifyContent = 'flex-start';
				};
				return deepMerge(style, addStyle(this.customStyle));
			}
		},
		emits: ['click'], // 防止事件執行两次
		// 20240409發現抖音小程序如果開啟virtualHost會出現严重問題，几乎所有事件包括created等生命周期事件全部失效。
		// #ifdef MP-WEIXIN
		options: {
		    // virtualHost: true ,//將自定義節點設置成虚拟的，更加接近Vue组件的表現。我们不希望自定義组件的這個節點本身可以設置樣式、响應 flex 布局等
		},
		// #endif
		methods: {
			// 此方法由u-grid-item触發，用于在u-grid發出事件
			childClick(name) {
				this.$emit('click', name)
			}
		}
	};
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
     $u-grid-width:100% !default;
	.u-grid {
		/* #ifdef APP-NVUE */
		width: $u-grid-width;
		position: relative;
		box-sizing: border-box;
		overflow: hidden;
		display: block;
		/* #endif */
		justify-content: center;
		@include flex;
		flex-wrap: wrap;
		align-items: center;
		// 在uni-app中應尽量避免使用flex布局以外的方式,因為nvue/uvue等方案都支持flex布局
		// 這里使用grid布局使用為目前20240409uni-app在抖音小程序開啟virtualHost時有bug，存在事件失效問題。
		/* #ifndef APP-NVUE */
		display: grid;
		grid-gap: v-bind(gap);
		grid-template-columns: repeat(v-bind(col), 1fr);
		/* #endif */
	}
</style>
