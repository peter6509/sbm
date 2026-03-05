<template>
	<view class="u-tabbar">
		<view
		    class="u-tabbar__content"
		    ref="u-tabbar__content"
		    @touchmove.stop.prevent="noop"
		    :class="[border && 'u-border-top', fixed && 'u-tabbar--fixed']"
		    :style="[tabbarStyle]"
		>
			<view class="u-tabbar__content__item-wrapper">
				<slot />
			</view>
			<u-safe-bottom v-if="safeAreaInsetBottom"></u-safe-bottom>
		</view>
		<view
		    class="u-tabbar__placeholder"
			v-if="placeholder"
		    :style="{
				height: placeholderHeight + 'px',
			}"
		></view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, deepMerge, sleep } from '../../libs/function/index';
	// #ifdef APP-NVUE
	const dom = uni.requireNativePlugin('dom')
	// #endif
	/**
	 * Tabbar 底部导航栏
	 * @description 此组件提供了自定義tabbar的能力。
	 * @tutorial https://ijry.github.io/uview-plus/components/tabbar.html
	 * @property {String | Number}	value				當前匹配項的name
	 * @property {Boolean}			safeAreaInsetBottom	是否為iPhoneX留出底部安全距离（默認 true ）
	 * @property {Boolean}			border				是否顯示上方邊框（默認 true ）
	 * @property {String | Number}	zIndex				元素層級z-index（默認 1 ）
	 * @property {String}			activeColor			選中標簽的顏色（默認 '#1989fa' ）
	 * @property {String}			inactiveColor		未選中標簽的顏色（默認 '#7d7e80' ）
	 * @property {Boolean}			fixed				是否固定在底部（默認 true ）
	 * @property {Boolean}			placeholder			fixed定位固定在底部時，是否生成一個等高元素防止塌陷（默認 true ）
	 * @property {Object}			customStyle			定義需要用到的外部樣式
	 * 
	 * @example <u-tabbar :value="value2" :placeholder="false" @change="name => value2 = name" :fixed="false" :safeAreaInsetBottom="false"><u-tabbar-item text="首頁" icon="home" dot ></u-tabbar-item></u-tabbar>
	 */
	export default {
		name: 'u-tabbar',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				placeholderHeight: 0
			}
		},
		computed: {
			tabbarStyle() {
				const style = {
					zIndex: this.zIndex
				}
				// 合並来自父组件的customStyle樣式
				return deepMerge(style, addStyle(this.customStyle))
			},
			// 監聽多個参數的變化，通過在computed執行對應的操作
			updateChild() {
				return [this.value, this.activeColor, this.inactiveColor]
			},
			updatePlaceholder() {
				return [this.fixed, this.placeholder]
			}
		},
		watch: {
			updateChild() {
				// 如果updateChildren中的元素發生了變化，则執行子元素初始化操作
				this.updateChildren()
			},
			updatePlaceholder() {
				// 如果fixed，placeholder等参數發生變化，重新計算占位元素的高度
				this.setPlaceholderHeight()
			}
		},
		created() {
			this.children = []
		},
		mounted() {
			this.setPlaceholderHeight()
		},
		methods: {
			updateChildren() {
				// 如果存在子元素，则執行子元素的updateFromParent进行更新數據
				this.children.length && this.children.map(child => child.updateFromParent())
			},
			// 設置用于防止塌陷元素的高度
			async setPlaceholderHeight() {
				if (!this.fixed || !this.placeholder) return
				// 延時一定時間
				await sleep(20)
				// #ifndef APP-NVUE
				this.$uGetRect('.u-tabbar__content').then(({height = 50}) => {
					// 修複IOS safearea bottom 未填充高度
					this.placeholderHeight = height
				})
				// #endif

				// #ifdef APP-NVUE
				dom.getComponentRect(this.$refs['u-tabbar__content'], (res) => {
					const {
						size
					} = res
					this.placeholderHeight = size.height
				})
				// #endif
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-tabbar {
		@include flex(column);
		flex: 1;
		justify-content: center;
		
		&__content {
			@include flex(column);
			background-color: #fff;
			
			&__item-wrapper {
				height: 50px;
				@include flex(row);
				justify-content: space-around;
			}
		}

		&--fixed {
			position: fixed;
			bottom: 0;
			left: 0;
			right: 0;
		}
	}
</style>
