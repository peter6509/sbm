<template>
	<view
		class="u-tooltip"
		:style="[addStyle(customStyle)]"
	>
		<u-overlay
			:show="showTooltip && tooltipTop !== -10000 && overlay"
			customStyle="backgroundColor: rgba(0, 0, 0, 0)"
			@click="overlayClickHandler"
		></u-overlay>
		<view class="u-tooltip__wrapper">
			<text
				class="u-tooltip__wrapper__text"
				:id="textId"
				:ref="textId"
				:userSelect="false"
				:selectable="false"
				@longpress.stop="longpressHandler"
				:style="{
					color: color,
					backgroundColor: bgColor && showTooltip && tooltipTop !== -10000 ? bgColor : 'transparent'
				}"
			>{{ text }}</text>
			<u-transition
				mode="fade"
				:show="showTooltip"
				duration="300"
				:customStyle="{
					position: 'absolute', 
					top: addUnit(tooltipTop),
					zIndex: zIndex,
					...tooltipStyle
				}"
			>
				<view
					class="u-tooltip__wrapper__popup"
					:id="tooltipId"
					:ref="tooltipId"
				>
					<view
						class="u-tooltip__wrapper__popup__indicator"
						hover-class="u-tooltip__wrapper__popup__indicator--hover"
						v-if="showCopy || buttons.length"
						:style="[indicatorStyle, {
							width: addUnit(indicatorWidth),
							height: addUnit(indicatorWidth),
						}]"
					>
						<!-- 由于nvue不支持三角形绘制，這里就做一個四方形，再旋轉45deg，得到露出的一個三角 -->
					</view>
					<view class="u-tooltip__wrapper__popup__list">
						<view
							v-if="showCopy"
							class="u-tooltip__wrapper__popup__list__btn"
							hover-class="u-tooltip__wrapper__popup__list__btn--hover"
							@tap="setClipboardData"
						>
							<text
								class="u-tooltip__wrapper__popup__list__btn__text"
							>複制</text>
						</view>
						<u-line
							direction="column"
							color="#8d8e90"
							v-if="showCopy && buttons.length > 0"
							length="18"
						></u-line>
						<block v-for="(item , index) in buttons" :key="index">
							<view
								class="u-tooltip__wrapper__popup__list__btn"
								hover-class="u-tooltip__wrapper__popup__list__btn--hover"
							>
								<text
									class="u-tooltip__wrapper__popup__list__btn__text"
									@tap="btnClickHandler(index)"
								>{{ item }}</text>
							</view>
							<u-line
								direction="column"
								color="#8d8e90"
								v-if="index < buttons.length - 1"
								length="18"
							></u-line>
						</block>
					</view>
				</view>
			</u-transition>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, addUnit, getPx, guid, toast, sleep, sys } from '../../libs/function/index';
	// #ifdef APP-NVUE 
	const dom = uni.requireNativePlugin('dom')
	// #endif
	/**
	 * Tooltip 
	 * @description 
	 * @tutorial https://ijry.github.io/uview-plus/components/tooltip.html
	 * @property {String | Number}	text		需要顯示的提示文字
	 * @property {String | Number}	copyText	點擊複制按鈕時，複制的文本，為空则使用text值
	 * @property {String | Number}	size		文本大小（默認 14 ）
	 * @property {String}			color		字體顏色（默認 '#606266' ）
	 * @property {String}			bgColor		彈出提示框時，文本的背景色（默認 'transparent' ）
	 * @property {String}			direction	彈出提示的方向，top-上方，bottom-下方（默認 'top' ）
	 * @property {String | Number}	zIndex		彈出提示的z-index，nvue無效（默認 10071 ）
	 * @property {Boolean}			showCopy	是否顯示複制按鈕（默認 true ）
	 * @property {Array}			buttons		扩展的按鈕组
	 * @property {Boolean}			overlay		是否顯示透明遮罩以防止触摸穿透（默認 true ）
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * 
	 * @event {Function} 
	 * @example 
	 */
	export default {
		name: 'u-tooltip',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				// 是否展示气泡
				showTooltip: true,
				// 生成唯一id，防止一個頁面多個组件，造成干扰
				textId: guid(),
				tooltipId: guid(),
				// 初始時甚至為很大的值，让其移到屏幕外面，為了計算元素的尺寸
				tooltipTop: -10000,
				// 气泡的位置信息
				tooltipInfo: {
					width: 0,
					left: 0
				},
				// 文本的位置信息
				textInfo: {
					width: 0,
					left: 0
				},
				// 三角形指示器的樣式
				indicatorStyle: {},
				// 气泡在可能超出屏幕邊沿範圍時，重新定位后，距离屏幕邊沿的距离
				screenGap: 12,
				// 三角形指示器的寬高，由于對元素进行了角度旋轉，精確計算指示器位置時，需要用到其尺寸信息
				indicatorWidth: 14,
			}
		},
		watch: {
			propsChange() {
				this.getElRect()
			}
		},
		computed: {
			// 特别處理H5的複制，因為H5浏覽器是自带系统複制功能的，在H5环境
			// 當一些依赖参數變化時，需要重新計算气泡和指示器的位置信息
			propsChange() {
				return [this.text, this.buttons]
			},
			// 計算气泡和指示器的位置信息
			tooltipStyle() {
				const style = {
						transform: `translateY(${this.direction === 'top' ? '-100%' : '100%'})`,
					},
					sysInfo = sys()
				if (this.tooltipInfo.width / 2 > this.textInfo.left + this.textInfo.width / 2 - this.screenGap) {
					this.indicatorStyle = {}
					style.left = `-${addUnit(this.textInfo.left - this.screenGap)}`
					this.indicatorStyle.left = addUnit(this.textInfo.width / 2 - getPx(style.left) - this.indicatorWidth /
						2)
				} else if (this.tooltipInfo.width / 2 > sysInfo.windowWidth - this.textInfo.right + this.textInfo.width / 2 -
					this.screenGap) {
					this.indicatorStyle = {}
					style.right = `-${addUnit(sysInfo.windowWidth - this.textInfo.right - this.screenGap)}`
					this.indicatorStyle.right = addUnit(this.textInfo.width / 2 - getPx(style.right) - this
						.indicatorWidth / 2)
				} else {
					const left = Math.abs(this.textInfo.width / 2 - this.tooltipInfo.width / 2)
					style.left = this.textInfo.width > this.tooltipInfo.width ? addUnit(left) : -addUnit(left)
					this.indicatorStyle = {}
				}
				if (this.direction === 'top') {
					style.marginTop = '-10px'
					this.indicatorStyle.bottom = '-4px'
				} else {
					style.marginBottom = '-10px'
					this.indicatorStyle.top = '-4px'
				}
				return style
			}
		},
		mounted() {
			this.init()
		},
		emits: ["click"],
		methods: {
			addStyle,
			addUnit,
			init() {
				this.getElRect()
			},
			// 長按触發事件
			async longpressHandler() {
				this.tooltipTop = 0
				this.showTooltip = true
			},
			// 點擊透明遮罩
			overlayClickHandler() {
				this.showTooltip = false
			},
			// 點擊彈出按鈕
			btnClickHandler(index) {
				this.showTooltip = false
				// 如果需要展示複制按鈕，此處index需要加1，因為複制按鈕在第一個位置
				this.$emit('click', this.showCopy ? index + 1 : index)
			},
			// 查詢内容高度
			queryRect(ref) {
				// #ifndef APP-NVUE
				// $uGetRect為uView自带的節點查詢簡化方法，詳見文檔介绍：https://ijry.github.io/uview-plus/js/getRect.html
				// 组件内部一般用this.$uGetRect，對外的為uni.$u.getRect，二者功能一致，名稱不同
				return new Promise(resolve => {
					this.$uGetRect(`#${ref}`).then(size => {
						resolve(size)
					})
				})
				// #endif

				// #ifdef APP-NVUE
				// nvue下，使用dom模块查詢元素高度
				// 返回一個promise，让調用此方法的主體能使用then回調
				return new Promise(resolve => {
					dom.getComponentRect(this.$refs[ref], res => {
						resolve(res.size)
					})
				})
				// #endif
			},
			// 元素尺寸
			getElRect() {
				// 調用之前，先將指示器調整到屏幕外，方便獲取尺寸
				this.showTooltip = true
				this.tooltipTop = -10000
				sleep(500).then(() => {
					this.queryRect(this.tooltipId).then(size => {
						this.tooltipInfo = size
						// 獲取气泡尺寸之后，將其隐藏，為了让下次切換气泡顯示與隐藏時，有淡入淡出的效果
						this.showTooltip = false
					})
					this.queryRect(this.textId).then(size => {
						this.textInfo = size
					})
				})
			},
			// 複制文本到粘貼板
			setClipboardData() {
				// 關闭组件
				this.showTooltip = false
				this.$emit('click', 0)
				uni.setClipboardData({
					// 优先使用copyText字段，如果没有，则默認使用text字段當做複制的内容
					data: this.copyText || this.text,
					success: () => {
						this.showToast && toast('複制成功')
					},
					fail: () => {
						this.showToast && toast('複制失败')
					},
					complete: () => {
						this.showTooltip = false
					}
				})
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-tooltip {
		position: relative;
		@include flex;

		&__wrapper {
			@include flex;
			justify-content: center;
			/* #ifndef APP-NVUE */
			white-space: nowrap;
			/* #endif */

			&__text {
				font-size: 14px;
			}

			&__popup {
				@include flex;
				justify-content: center;

				&__list {
					background-color: #060607;
					position: relative;
					flex: 1;
					border-radius: 5px;
					padding: 0px 0;
					@include flex(row);
					align-items: center;
					overflow: hidden;

					&__btn {
						padding: 11px 13px;

						&--hover {
							background-color: #58595B;
						}

						&__text {
							line-height: 12px;
							font-size: 13px;
							color: #FFFFFF;
						}
					}
				}

				&__indicator {
					position: absolute;
					background-color: #060607;
					width: 14px;
					height: 14px;
					bottom: -4px;
					transform: rotate(45deg);
					border-radius: 2px;
					z-index: -1;

					&--hover {
						background-color: #58595B;
					}
				}
			}
		}
	}
</style>
