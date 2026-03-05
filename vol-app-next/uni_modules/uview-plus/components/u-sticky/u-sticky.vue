<template>
	<view
		class="u-sticky"
		:id="elId"
		:style="[style]"
	>
		<view
			:style="[stickyContent]"
			class="u-sticky__content"
		>
			<slot />
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, deepMerge, getPx, guid, sys, os } from '../../libs/function/index';
	import zIndex from '../../libs/config/zIndex';
	/**
	 * sticky 吸顶
	 * @description 該组件與CSS中position: sticky屬性實現的效果一致，當组件达到預設的到顶部距离時， 就會固定在指定位置，组件位置大于預設的顶部距离時，會重新按照正常的布局排列。
	 * @tutorial https://ijry.github.io/uview-plus/components/sticky.html
	 * @property {String ｜ Number}	offsetTop		吸顶時與顶部的距离，單位px（默認 0 ）
	 * @property {String ｜ Number}	customNavHeight	自定義导航栏的高度 （h5 默認44  其他默認 0 ）
	 * @property {Boolean}			disabled		是否開啟吸顶功能 （默認 false ）
	 * @property {String}			bgColor			组件背景顏色（默認 '#ffffff' ）
	 * @property {String ｜ Number}	zIndex			吸顶時的z-index值
	 * @property {String ｜ Number}	index			自定義標识，用于區分是哪一個组件
	 * @property {Object}			customStyle		组件的樣式，對象形式
	 * @event {Function} fixed		组件吸顶時触發
	 * @event {Function} unfixed	组件取消吸顶時触發
	 * @example <u-sticky offsetTop="200"><view>塞下秋来风景異，衡阳雁去無留意</view></u-sticky>
	 */
	export default {
		name: 'u-sticky',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				cssSticky: false, // 是否使用css的sticky實現
				stickyTop: 0, // 吸顶的top值，因為可能受自定義导航栏影响，最终的吸顶值非offsetTop值
				elId: guid(),
				left: 0, // js模式時，吸顶的内容因為處于postition: fixed模式，為了和原来保持一致的樣式，需要记錄並重新設置它的left，height，width屬性
				width: 'auto',
				height: 'auto',
				fixed: false, // js模式時，是否處于吸顶模式
			}
		},
		computed: {
			style() {
				const style = {}
				if(!this.disabled) {
					if (this.cssSticky) {
						style.position = 'sticky'
						style.zIndex = this.uZindex
						style.top = addUnit(this.stickyTop)
					} else {
						style.height = this.fixed ? this.height + 'px' : 'auto'
					}
				} else {
					// 無需吸顶時，設置會默認的relative(nvue)和非nvue的static静態模式即可
					// #ifdef APP-NVUE
					style.position = 'relative'
					// #endif
					// #ifndef APP-NVUE
					style.position = 'static'
					// #endif
				}
				style.backgroundColor = this.bgColor
				return deepMerge(addStyle(this.customStyle), style)
			},
			// 吸顶内容的樣式
			stickyContent() {
				const style = {}
				if (!this.cssSticky) {
					style.position = this.fixed ? 'fixed' : 'static'
					style.top = this.stickyTop + 'px'
					style.left = this.left + 'px'
					style.width = this.width == 'auto' ? 'auto' : this.width + 'px'
					style.zIndex = this.uZindex
				}
				return style
			},
			uZindex() {
				return this.zIndex ? this.zIndex : zIndex.sticky
			}
		},
		mounted() {
			this.init()
		},
		methods: {
			init() {
				this.getStickyTop()
				// 判斷使用的模式
				this.checkSupportCssSticky()
				// 如果不支持css sticky，则使用js方案，此方案性能比不上css方案
				if (!this.cssSticky) {
					!this.disabled && this.initObserveContent()
				}
			},
			initObserveContent() {
				// 獲取吸顶内容的高度，用于在js吸顶模式時，给父元素一個填充高度，防止"塌陷"
				this.$uGetRect('#' + this.elId).then((res) => {
					this.height = res.height
					this.left = res.left
					this.width = res.width
					this.$nextTick(() => {
						this.observeContent()
					})
				})
			},
			observeContent() {
				// 先斷掉之前的觀察
				this.disconnectObserver('contentObserver')
				const contentObserver = uni.createIntersectionObserver({
					// 檢测的區間範圍
					thresholds: [0.95, 0.98, 1]
				})
				// 到屏幕顶部的高度時触發
				contentObserver.relativeToViewport({
					top: -this.stickyTop
				})
				// 绑定觀察的元素
				contentObserver.observe(`#${this.elId}`, res => {
					this.setFixed(res.boundingClientRect.top)
				})
				this.contentObserver = contentObserver
			},
			setFixed(top) {
				// 判斷是否出于吸顶條件範圍
				const fixed = top <= this.stickyTop
				this.fixed = fixed
			},
			disconnectObserver(observerName) {
				// 斷掉觀察，释放资源
				const observer = this[observerName]
				observer && observer.disconnect()
			},
			getStickyTop() {
				this.stickyTop = getPx(this.offsetTop) + getPx(this.customNavHeight)
			},
			async checkSupportCssSticky() {
				// #ifdef H5
				// H5，一般都是現代浏覽器，是支持css sticky的，這里使用創建元素嗅探的形式判斷
				if (this.checkCssStickyForH5()) {
					this.cssSticky = true
				}
				// #endif

				// 如果安卓版本高于8.0，依然認為是支持css sticky的(因為安卓7在某些機型，可能不支持sticky)
				if (os() === 'android' && Number(sys().system) > 8) {
					this.cssSticky = true
				}

				// APP-Vue和微信平台，通過computedStyle判斷是否支持css sticky
				// #ifdef APP-VUE || MP-WEIXIN || MP-TOUTIAO
				this.cssSticky = await this.checkComputedStyle()
				// #endif

				// ios上，從ios6開始，都是支持css sticky的
				if (os() === 'ios') {
					this.cssSticky = true
				}

				// nvue，是支持css sticky的
				// #ifdef APP-NVUE
				this.cssSticky = true
				// #endif
			},
			// 在APP和微信小程序上，通過uni.createSelectorQuery可以判斷是否支持css sticky
			checkComputedStyle() {
				// 方法内进行判斷，避免在其他平台生成無用代碼
				// #ifdef APP-VUE || MP-WEIXIN || MP-TOUTIAO
				return new Promise(resolve => {
					uni.createSelectorQuery().in(this).select('.u-sticky').fields({
						computedStyle: ["position"]
					}).exec(e => {
						resolve('sticky' === e[0].position)
					})
				})
				// #endif
			},
			// H5通過創建元素的形式嗅探是否支持css sticky
			// 判斷浏覽器是否支持sticky屬性
			checkCssStickyForH5() {
				// 方法内进行判斷，避免在其他平台生成無用代碼
				// #ifdef H5
				const vendorList = ['', '-webkit-', '-ms-', '-moz-', '-o-'],
					vendorListLength = vendorList.length,
					stickyElement = document.createElement('div')
				for (let i = 0; i < vendorListLength; i++) {
					stickyElement.style.position = vendorList[i] + 'sticky'
					if (stickyElement.style.position !== '') {
						return true
					}
				}
				return false;
				// #endif
			}
		},
		beforeUnmount() {
			this.disconnectObserver('contentObserver')
		}
	}
</script>

<style lang="scss" scoped>
	.u-sticky {
		/* #ifdef APP-VUE || MP-WEIXIN || MP-TOUTIAO */
		// 此處默認写sticky屬性，是為了给微信和APP通過uni.createSelectorQuery查詢是否支持css sticky使用
		position: sticky;
		/* #endif */
	}
</style>
