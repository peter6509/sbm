<template>
	<view
		class="u-scroll-list"
		ref="u-scroll-list"
	>
		<!-- #ifdef APP-NVUE -->
		<!-- nvue使用bindingX實現，以得到更好的性能 -->
		<scroller
			class="u-scroll-list__scroll-view"
			ref="u-scroll-list__scroll-view"
			scroll-direction="horizontal"
			:show-scrollbar="false"
			:offset-accuracy="1"
			@scroll="nvueScrollHandler"
		>
			<view class="u-scroll-list__scroll-view__content">
				<slot />
			</view>
		</scroller>
		<!-- #endif -->
		<!-- #ifndef APP-NVUE -->
		<!-- #ifdef MP-WEIXIN || APP-VUE || H5 || MP-QQ -->
		<!-- 以上平台，支持wxs -->
		<scroll-view
			class="u-scroll-list__scroll-view scroll-view-native"
			scroll-x
			enable-flex
			@scroll="wxs.scroll"
			@scrolltoupper="wxs.scrolltoupper"
			@scrolltolower="wxs.scrolltolower"
			:data-scrollWidth="scrollWidth"
			:data-barWidth="getPx(indicatorBarWidth)"
			:data-indicatorWidth="getPx(indicatorWidth)"
			:show-scrollbar="false"
			:upper-threshold="0"
			:lower-threshold="0"
		>
			<!-- #endif -->
			<!-- #ifndef APP-NVUE || MP-WEIXIN || H5 || APP-VUE || MP-QQ -->
			<!-- 非以上平台，只能使用普通js實現 -->
			<scroll-view
				class="u-scroll-list__scroll-view scroll-view-js"
				scroll-x
				@scroll="scrollHandler"
				@scrolltoupper="scrolltoupperHandler"
				@scrolltolower="scrolltolowerHandler"
				:show-scrollbar="false"
				:upper-threshold="0"
				:lower-threshold="0"
			>
				<!-- #endif -->
				<view class="u-scroll-list__scroll-view__content">
					<slot />
				</view>
			</scroll-view>
			<!-- #endif -->
			<view
				class="u-scroll-list__indicator"
				v-if="indicator"
				:style="[addStyle(indicatorStyle)]"
			>
				<view
					class="u-scroll-list__indicator__line"
					:style="[lineStyle]"
				>
					<view
						class="u-scroll-list__indicator__line__bar"
						:style="[barStyle]"
						ref="u-scroll-list__indicator__line__bar"
					></view>
				</view>
			</view>
	</view>
</template>

<script
	src="./scrollWxs.wxs"
	module="wxs"
	lang="wxs"
></script>

<script>
/**
 * scrollList 横向滾動列表
 * @description 該组件一般用于同時展示多個商品、分類的场景，也可以完成左右滑動的列表。
 * @tutorial https://ijry.github.io/uview-plus/components/scrollList.html
 * @property {String | Number}	indicatorWidth			指示器的整體寬度 (默認 50 )
 * @property {String | Number}	indicatorBarWidth		滑块的寬度 (默認 20 )
 * @property {Boolean}			indicator				是否顯示面板指示器 (默認 true )
 * @property {String}			indicatorColor			指示器非激活顏色 (默認 '#f2f2f2' )
 * @property {String}			indicatorActiveColor	指示器的激活顏色 (默認 '#3c9cff' )
 * @property {String | Object}	indicatorStyle			指示器樣式，可通過bottom，left，right进行定位
 * @event {Function} left	滑動到左邊時触發
 * @event {Function} right	滑動到右邊時触發
 * @example
 */
// #ifdef APP-NVUE
const dom = uni.requireNativePlugin('dom')
import nvueMixin from "./nvue.js"
// #endif
import { props } from './props';
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { addStyle, addUnit, getPx, sleep } from '../../libs/function/index';
export default {
	name: 'u-scroll-list',
	// #ifndef APP-NVUE
	mixins: [mpMixin, mixin, props],
	// #endif
	// #ifdef APP-NVUE
	mixins: [mpMixin, mixin, nvueMixin, props],
	// #endif
	data() {
		return {
			scrollInfo: {
				scrollLeft: 0,
				scrollWidth: 0
			},
			scrollWidth: 0
		}
	},
	computed: {
		// 指示器為线型的樣式
		barStyle() {
			const style = {}
			// #ifndef APP-NVUE || MP-WEIXIN || H5 || APP-VUE || MP-QQ
			// 此為普通js方案，只有在非nvue和不支持wxs方案的端才使用、
			// 此處的計算理由為：scroll-view的滾動距离與目標滾動距离(scroll-view的實际寬度减去包裹元素的寬度)之比，等于滑块當前移動距离與總需
			// 滑動距离(指示器的總寬度减去滑块寬度)的比值
			const scrollLeft = this.scrollInfo.scrollLeft,
				scrollWidth = this.scrollInfo.scrollWidth,
				barAllMoveWidth = this.indicatorWidth - this.indicatorBarWidth
			const x = scrollLeft / (scrollWidth - this.scrollWidth) * barAllMoveWidth
			style.transform = `translateX(${ x }px)`
			// #endif
			// 設置滑块的寬度和背景色，是每個平台都需要的
			style.width = addUnit(this.indicatorBarWidth)
			style.backgroundColor = this.indicatorActiveColor
			return style
		},
		lineStyle() {
			const style = {}
			// 指示器整體的樣式，需要設置其寬度和背景色
			style.width = addUnit(this.indicatorWidth)
			style.backgroundColor = this.indicatorColor
			return style
		}
	},
	mounted() {
		this.init()
	},
	emits: ["left", "right"],
	methods: {
		addStyle,
		getPx,
		init() {
			this.getComponentWidth()
		},
		// #ifndef APP-NVUE || MP-WEIXIN || H5 || APP-VUE || MP-QQ
		// scroll-view触發滾動事件
		scrollHandler(e) {
			this.scrollInfo = e.detail
		},
		scrolltoupperHandler() {
			this.scrollEvent('left')
			this.scrollInfo.scrollLeft = 0
		},
		scrolltolowerHandler() {
			this.scrollEvent('right')
			// 在普通js方案中，滾動到右邊時，通過設置this.scrollInfo，模拟出滾動到右邊的情况
			// 因為上方是用過computed計算的，設置后，會自動調整滑块的位置
			this.scrollInfo.scrollLeft = getPx(this.indicatorWidth) - getPx(this.indicatorBarWidth)
		},
		// #endif
		//
		scrollEvent(status) {
			this.$emit(status)
		},
		// 獲取组件的寬度
		async getComponentWidth() {
			// 延時一定時間，以獲取dom尺寸
			await sleep(30)
			// #ifndef APP-NVUE
			this.$uGetRect('.u-scroll-list').then(size => {
				this.scrollWidth = size.width
			})
			// #endif

			// #ifdef APP-NVUE
			const ref = this.$refs['u-scroll-list']
			ref && dom.getComponentRect(ref, (res) => {
				this.scrollWidth = res.size.width
			})
			// #endif
		},
	}
}
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";

.u-scroll-list {
	padding-bottom: 10px;

	&__scroll-view {
		@include flex;
		// 缺少會在enable-flex模式下高度異常
		align-items: flex-start;

		&__content {
			@include flex;
		}
	}

	&__indicator {
		@include flex;
		justify-content: center;
		margin-top: 15px;

		&__line {
			width: 60px;
			height: 4px;
			border-radius: 100px;
			overflow: hidden;

			&__bar {
				width: 20px;
				height: 4px;
				border-radius: 100px;
			}
		}
	}
}
</style>
