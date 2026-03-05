<template>
	<view class="u-popup"  :class="[customClass]">
		<u-overlay
			:show="show"
			@click="overlayClick"
			v-if="overlay"
			:zIndex="zIndex"
			:duration="overlayDuration"
			:customStyle="overlayStyle"
			:opacity="overlayOpacity"
		></u-overlay>
		<u-transition
			:show="show"
			:customStyle="transitionStyle"
			:mode="position"
			:duration="duration"
			@afterEnter="afterEnter"
			@click="clickHandler"
		>
			<view
				class="u-popup__content"
				:style="[contentStyle]"
			
			>
				<u-status-bar v-if="safeAreaInsetTop"></u-status-bar>
				<slot></slot>
				<view
					v-if="closeable"
					@tap.stop="close"
					class="u-popup__content__close"
					:class="['u-popup__content__close--' + closeIconPos]"
					hover-class="u-popup__content__close--hover"
					hover-stay-time="150"
				>
					<u-icon
						name="close"
						color="#909399"
						size="18"
						bold
					></u-icon>
				</view>
				<u-safe-bottom v-if="safeAreaInsetBottom"></u-safe-bottom>
			</view>
		</u-transition>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, deepMerge, sleep, sys } from '../../libs/function/index';
	/**
	 * popup 彈窗
	 * @description 彈出層容器，用于展示彈窗、信息提示等内容，支持上、下、左、右和中部彈出。组件只提供容器，内部内容由用户自定義
	 * @tutorial https://ijry.github.io/uview-plus/components/popup.html
	 * @property {Boolean}			show				是否展示彈窗 (默認 false )
	 * @property {Boolean}			overlay				是否顯示遮罩 （默認 true ）
	 * @property {String}			mode				彈出方向（默認 'bottom' ）
	 * @property {String | Number}	duration			動画時長，單位ms （默認 300 ）
	 * @property {String | Number}	overlayDuration		遮罩層動画時長，單位ms （默認 350 ）
	 * @property {Boolean}			closeable			是否顯示關闭圖標（默認 false ）
	 * @property {Object | String}	overlayStyle		自定義遮罩的樣式
	 * @property {String | Number}	overlayOpacity		遮罩透明度，0-1之間（默認 0.5）
	 * @property {Boolean}			closeOnClickOverlay	點擊遮罩是否關闭彈窗 （默認  true ）
	 * @property {String | Number}	zIndex				層級 （默認 10075 ）
	 * @property {Boolean}			safeAreaInsetBottom	是否為iPhoneX留出底部安全距离 （默認 true ）
	 * @property {Boolean}			safeAreaInsetTop	是否留出顶部安全距离（狀態栏高度） （默認 false ）
	 * @property {String}			closeIconPos		自定義關闭圖標位置（默認 'top-right' ）
	 * @property {String | Number}	round				圆角值（默認 0）
	 * @property {Boolean}			zoom				當mode=center時 是否開啟缩放（默認 true ）
	 * @property {Object}			customStyle			组件的樣式，對象形式
	 * @event {Function} open 彈出層打開
	 * @event {Function} close 彈出層收起
	 * @example <u-popup v-model:show="show"><text>出淤泥而不染，濯清涟而不妖</text></u-popup>
	 */
	export default {
		name: 'u-popup',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				overlayDuration: this.duration + 50
			}
		},
		watch: {
			show(newValue, oldValue) {
				if (newValue === true) {
					// #ifdef MP-WEIXIN
					const children = this.$children
					this.retryComputedComponentRect(children)
					// #endif
				}
			}
		},
		computed: {
			transitionStyle() {
				const style = {
					zIndex: this.zIndex,
					position: 'fixed',
					display: 'flex',
				}
				style[this.mode] = 0
				if (this.mode === 'left') {
					return deepMerge(style, {
						bottom: 0,
						top: 0,
					})
				} else if (this.mode === 'right') {
					return deepMerge(style, {
						bottom: 0,
						top: 0,
					})
				} else if (this.mode === 'top') {
					return deepMerge(style, {
						left: 0,
						right: 0
					})
				} else if (this.mode === 'bottom') {
					return deepMerge(style, {
						left: 0,
						right: 0,
					})
				} else if (this.mode === 'center') {
					return deepMerge(style, {
						alignItems: 'center',
						'justify-content': 'center',
						top: 0,
						left: 0,
						right: 0,
						bottom: 0
					})
				}
			},
			contentStyle() {
				const style = {}
				// 通過設備信息的safeAreaInsets值来判斷是否需要預留顶部狀態栏和底部安全局的位置
				// 不使用css方案，是因為nvue不支持css的iPhoneX安全區查詢屬性
				const {
					safeAreaInsets
				} = sys()
				if (this.mode !== 'center') {
					style.flex = 1
				}
				// 背景色，一般用于設置為transparent，去除默認的白色背景
				if (this.bgColor) {
					style.backgroundColor = this.bgColor
				}
				if(this.round) {
					const value = addUnit(this.round)
					if(this.mode === 'top') {
						style.borderBottomLeftRadius = value
						style.borderBottomRightRadius = value
					} else if(this.mode === 'bottom') {
						style.borderTopLeftRadius = value
						style.borderTopRightRadius = value
					} else if(this.mode === 'center') {
						style.borderRadius = value
					} 
				}
				return deepMerge(style, addStyle(this.customStyle))
			},
			position() {
				if (this.mode === 'center') {
					return this.zoom ? 'fade-zoom' : 'fade'
				}
				if (this.mode === 'left') {
					return 'slide-left'
				}
				if (this.mode === 'right') {
					return 'slide-right'
				}
				if (this.mode === 'bottom') {
					return 'slide-up'
				}
				if (this.mode === 'top') {
					return 'slide-down'
				}
			},
		},
		emits: ["open", "close", "click", "update:show"],
		methods: {
			// 點擊遮罩
			overlayClick() {
				if (this.closeOnClickOverlay) {
					this.$emit('update:show', false)
					this.$emit('close')
				}
			},
			close(e) {
				this.$emit('update:show', false)
				this.$emit('close')
			},
			afterEnter() {
				this.$emit('open')
			},
			clickHandler() {
				// 由于中部彈出時，其u-transition占據了整個頁面相當于遮罩，此時需要發出遮罩點擊事件，是否無法通過點擊遮罩關闭彈窗
				if(this.mode === 'center') {
					this.overlayClick()
				}
				this.$emit('click')
			},
			// #ifdef MP-WEIXIN
			retryComputedComponentRect(children) {
				// 组件内部需要計算節點的组件
				const names = ['u-calendar-month', 'u-album', 'u-collapse-item', 'u-dropdown', 'u-index-item', 'u-index-list',
					'u-line-progress', 'u-list-item', 'u-rate', 'u-read-more', 'u-row', 'u-row-notice', 'u-scroll-list',
					'u-skeleton', 'u-slider', 'u-steps-item', 'u-sticky', 'u-subsection', 'u-swipe-action-item', 'u-tabbar',
					'u-tabs', 'u-tooltip'
				]
				// 历遍所有的子组件節點
				for (let i = 0; i < children.length; i++) {
					const child = children[i]
					// 拿到子组件的子组件
					const grandChild = child.$children
					// 判斷如果在需要重新初始化的组件數组中名中，並且存在init方法的話，则執行
					if (names.includes(child.$options.name) && typeof child?.init === 'function') {
						// 需要进行一定的延時，因為初始化頁面需要時間
						sleep(50).then(() => {
							child.init()
						})
					}
					// 如果子组件還有孫组件，进行递归历遍
					if (grandChild.length) {
						this.retryComputedComponentRect(grandChild)
					}
				}
			}
			// #endif
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	$u-popup-flex:1 !default;
	$u-popup-content-background-color: #fff !default;

	.u-popup {
		flex: $u-popup-flex;

		&__content {
			background-color: $u-popup-content-background-color;
			position: relative;

			&--round-top {
				border-top-left-radius: 0;
				border-top-right-radius: 0;
				border-bottom-left-radius: 10px;
				border-bottom-right-radius: 10px;
			}

			&--round-left {
				border-top-left-radius: 0;
				border-top-right-radius: 10px;
				border-bottom-left-radius: 0;
				border-bottom-right-radius: 10px;
			}

			&--round-right {
				border-top-left-radius: 10px;
				border-top-right-radius: 0;
				border-bottom-left-radius: 10px;
				border-bottom-right-radius: 0;
			}

			&--round-bottom {
				border-top-left-radius: 10px;
				border-top-right-radius: 10px;
				border-bottom-left-radius: 0;
				border-bottom-right-radius: 0;
			}

			&--round-center {
				border-top-left-radius: 10px;
				border-top-right-radius: 10px;
				border-bottom-left-radius: 10px;
				border-bottom-right-radius: 10px;
			}

			&__close {
				position: absolute;

				&--hover {
					opacity: 0.4;
				}
			}

			&__close--top-left {
				top: 15px;
				left: 15px;
			}

			&__close--top-right {
				top: 15px;
				right: 15px;
			}

			&__close--bottom-left {
				bottom: 15px;
				left: 15px;
			}

			&__close--bottom-right {
				right: 15px;
				bottom: 15px;
			}
		}
	}
</style>
