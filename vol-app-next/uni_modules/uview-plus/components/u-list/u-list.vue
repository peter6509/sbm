<template>
	<!-- #ifdef APP-NVUE -->
	<list
		class="u-list"
		:enableBackToTop="enableBackToTop"
		:loadmoreoffset="lowerThreshold"
		:showScrollbar="showScrollbar"
		:style="[listStyle]"
		:offset-accuracy="Number(offsetAccuracy)"
		@scroll="onScroll"
		@loadmore="scrolltolower"
	>
		<slot />
	</list>
	<!-- #endif -->
	<!-- #ifndef APP-NVUE -->
	<scroll-view
		class="u-list"
		:scroll-into-view="scrollIntoView"
		:style="[listStyle]"
		:scroll-y="scrollable"
		:scroll-top="Number(scrollTop)"
		:lower-threshold="Number(lowerThreshold)"
		:upper-threshold="Number(upperThreshold)"
		:show-scrollbar="showScrollbar"
		:enable-back-to-top="enableBackToTop"
		:scroll-with-animation="scrollWithAnimation"
		@scroll="onScroll"
		@scrolltolower="scrolltolower"
		@scrolltoupper="scrolltoupper"
		:refresher-enabled="refresherEnabled"
		:refresher-threshold="refresherThreshold"
		:refresher-default-style="refresherDefaultStyle"
		:refresher-background="refresherBackground"
		:refresher-triggered="refresherTriggered"
		@refresherpulling="refresherpulling"
		@refresherrefresh="refresherrefresh"
		@refresherrestore="refresherrestore"
		@refresherabort="refresherabort"
		:scroll-anchoring="true"
	>
		<view>
			<slot />
		</view>
	</scroll-view>
	<!-- #endif -->
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, deepMerge, sleep, sys } from '../../libs/function/index';
	// #ifdef APP-NVUE
	const dom = uni.requireNativePlugin('dom')
	// #endif
	/**
	 * List 列表
	 * @description 該组件為高性能列表组件
	 * @tutorial https://ijry.github.io/uview-plus/components/list.html
	 * @property {Boolean}			showScrollbar		控制是否出現滾動條，仅nvue有效 （默認 false ）
	 * @property {String ｜ Number}	lowerThreshold		距底部多少時触發scrolltolower事件 （默認 50 ）
	 * @property {String ｜ Number}	upperThreshold		距顶部多少時触發scrolltoupper事件，非nvue有效 （默認 0 ）
	 * @property {String ｜ Number}	scrollTop			設置竖向滾動條位置（默認 0 ）
	 * @property {String ｜ Number}	offsetAccuracy		控制 onscroll 事件触發的频率，仅nvue有效（默認 10 ）
	 * @property {Boolean}			enableFlex			啟用 flexbox 布局。開啟后，當前節點声明了display: flex就會成為flex container，並作用于其孩子節點，仅微信小程序有效（默認 false ）
	 * @property {Boolean}			pagingEnabled		是否按分頁模式顯示List，（默認 false ）
	 * @property {Boolean}			scrollable			是否允许List滾動（默認 true ）
	 * @property {String}			scrollIntoView		值應為某子元素id（id不能以數字開頭）
	 * @property {Boolean}			scrollWithAnimation	在設置滾動條位置時使用動画過渡 （默認 false ）
	 * @property {Boolean}			enableBackToTop		iOS點擊顶部狀態栏、安卓双擊標題栏時，滾動條返回顶部，只對微信小程序有效 （默認 false ）
	 * @property {String ｜ Number}	height				列表的高度 （默認 0 ）
	 * @property {String ｜ Number}	width				列表寬度 （默認 0 ）
	 * @property {String ｜ Number}	preLoadScreen		列表前后預渲染的屏數，1代表一個屏幕的高度，1.5代表1個半屏幕高度  （默認 1 ）
	 * @property {Object}			customStyle			定義需要用到的外部樣式
	 *
	 * @example <u-list @scrolltolower="scrolltolower"></u-list>
	 */
	export default {
		name: 'u-list',
		mixins: [mpMixin, mixin, props],
		watch: {
			scrollIntoView(n) {
				this.scrollIntoViewById(n)
			}
		},
		data() {
			return {
				// 记錄内部滾動的距离
				innerScrollTop: 0,
				// vue下，scroll-view在上拉加載時的偏移值
				offset: 0,
				sys: sys()
			}
		},
		computed: {
			listStyle() {
				const style = {};
				if (this.width != 0) style.width = addUnit(this.width)
				if (this.height != 0) style.height = addUnit(this.height)
				// 如果没有定義列表高度，则默認使用屏幕高度
				if (!style.height) style.height = addUnit(this.sys.windowHeight, 'px')
				return deepMerge(style, addStyle(this.customStyle))
			}
		},
		provide() {
			return {
				uList: this
			}
		},
		created() {
			this.refs = []
			this.children = []
			this.anchors = []
		},
		mounted() {},
		emits: ["scroll", "scrolltolower", "scrolltoupper",
			"refresherpulling", "refresherrefresh", "refresherrestore", "refresherabort"],
		methods: {
			updateOffsetFromChild(top) {
				this.offset = top
			},
			onScroll(e) {
				let scrollTop = 0
				// #ifdef APP-NVUE
				scrollTop = e.contentOffset.y
				// #endif
				// #ifndef APP-NVUE
				scrollTop = e.detail.scrollTop
				// #endif
				this.innerScrollTop = scrollTop
				this.$emit('scroll', scrollTop)
			},
			scrollIntoViewById(id) {
				// #ifdef APP-NVUE
				// 根據id参數，找到所有u-list-item中匹配的節點，再通過dom模块滾動到對應的位置
				const item = this.refs.find(item => item.$refs[id] ? true : false)
				dom.scrollToElement(item.$refs[id], {
					// 是否需要滾動動画
					animated: this.scrollWithAnimation
				})
				// #endif
			},
			// 滾動到底部触發事件
			scrolltolower(e) {
				sleep(30).then(() => {
					this.$emit('scrolltolower')
				})
			},
			// #ifndef APP-NVUE
			// 滾動到底部時触發，非nvue有效
			scrolltoupper(e) {
				sleep(30).then(() => {
					this.$emit('scrolltoupper')
					// 這一句很重要，能绝對保证在性功能障碍的webview，滾動條到顶時，取消偏移值，让頁面置顶
					this.offset = 0
				})
			},
			refresherpulling(e) {
				this.$emit('refresherpulling', e)
			},
			refresherrefresh(e) {
				this.$emit('refresherrefresh', e)
			},
			refresherrestore(e) {
				this.$emit('refresherrestore', e)
			},
			refresherabort(e) {
				this.$emit('refresherabort', e)
			}
			// #endif
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-list {
		@include flex(column);

	}
</style>
