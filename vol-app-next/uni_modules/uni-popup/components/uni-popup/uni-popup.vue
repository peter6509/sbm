<template>
	<view v-if="showPopup" class="uni-popup" :class="[popupstyle, isDesktop ? 'fixforpc-z-index' : '']">
		<view @touchstart="touchstart">
			<uni-transition key="1" v-if="maskShow" name="mask" mode-class="fade" :styles="maskClass"
				:duration="duration" :show="showTrans" @click="onTap" />
			<uni-transition key="2" :mode-class="ani" name="content" :styles="transClass" :duration="duration"
				:show="showTrans" @click="onTap">
				<view class="uni-popup__wrapper" :style="getStyles" :class="[popupstyle]" @click="clear">
					<slot />
				</view>
			</uni-transition>
		</view>
		<!-- #ifdef H5 -->
		<keypress v-if="maskShow" @esc="onTap" />
		<!-- #endif -->
	</view>
</template>

<script>
	// #ifdef H5
	import keypress from './keypress.js'
	// #endif

	/**
	 * PopUp 彈出層
	 * @description 彈出層组件，為了解决遮罩彈層的問題
	 * @tutorial https://ext.dcloud.net.cn/plugin?id=329
	 * @property {String} type = [top|center|bottom|left|right|message|dialog|share] 彈出方式
	 * 	@value top 顶部彈出
	 * 	@value center 中間彈出
	 * 	@value bottom 底部彈出
	 * 	@value left		左侧彈出
	 * 	@value right  右侧彈出
	 * 	@value message 消息提示
	 * 	@value dialog 對話框
	 * 	@value share 底部分享示例
	 * @property {Boolean} animation = [true|false] 是否開啟動画
	 * @property {Boolean} maskClick = [true|false] 蒙版點擊是否關闭彈窗(废弃)
	 * @property {Boolean} isMaskClick = [true|false] 蒙版點擊是否關闭彈窗
	 * @property {String}  backgroundColor 主窗口背景色
	 * @property {String}  maskBackgroundColor 蒙版顏色
	 * @property {String}  borderRadius 設置圆角(左上、右上、右下和左下) 示例:"10px 10px 10px 10px"
	 * @property {Boolean} safeArea		   是否適配底部安全區
	 * @event {Function} change 打開關闭彈窗触發，e={show: false}
	 * @event {Function} maskClick 點擊遮罩触發
	 */

	export default {
		name: 'uniPopup',
		components: {
			// #ifdef H5
			keypress
			// #endif
		},
		emits: ['change', 'maskClick'],
		props: {
			// 開啟動画
			animation: {
				type: Boolean,
				default: true
			},
			// 彈出層類型，可選值，top: 顶部彈出層；bottom：底部彈出層；center：全屏彈出層
			// message: 消息提示 ; dialog : 對話框
			type: {
				type: String,
				default: 'center'
			},
			// maskClick
			isMaskClick: {
				type: Boolean,
				default: null
			},
			// TODO 2 個版本后废弃屬性 ，使用 isMaskClick
			maskClick: {
				type: Boolean,
				default: null
			},
			backgroundColor: {
				type: String,
				default: 'none'
			},
			safeArea: {
				type: Boolean,
				default: true
			},
			maskBackgroundColor: {
				type: String,
				default: 'rgba(0, 0, 0, 0.4)'
			},
			borderRadius:{
				type: String,
			}
		},

		watch: {
			/**
			 * 監聽type類型
			 */
			type: {
				handler: function(type) {
					if (!this.config[type]) return
					this[this.config[type]](true)
				},
				immediate: true
			},
			isDesktop: {
				handler: function(newVal) {
					if (!this.config[newVal]) return
					this[this.config[this.type]](true)
				},
				immediate: true
			},
			/**
			 * 監聽遮罩是否可點擊
			 * @param {Object} val
			 */
			maskClick: {
				handler: function(val) {
					this.mkclick = val
				},
				immediate: true
			},
			isMaskClick: {
				handler: function(val) {
					this.mkclick = val
				},
				immediate: true
			},
			// H5 下禁止底部滾動
			showPopup(show) {
				// #ifdef H5
				// fix by mehaotian 處理 h5 滾動穿透的問題
				document.getElementsByTagName('body')[0].style.overflow = show ? 'hidden' : 'visible'
				// #endif
			}
		},
		data() {
			return {
				duration: 300,
				ani: [],
				showPopup: false,
				showTrans: false,
				popupWidth: 0,
				popupHeight: 0,
				config: {
					top: 'top',
					bottom: 'bottom',
					center: 'center',
					left: 'left',
					right: 'right',
					message: 'top',
					dialog: 'center',
					share: 'bottom'
				},
				maskClass: {
					position: 'fixed',
					bottom: 0,
					top: 0,
					left: 0,
					right: 0,
					backgroundColor: 'rgba(0, 0, 0, 0.4)'
				},
				transClass: {
					backgroundColor: 'transparent',
					borderRadius: this.borderRadius || "0",
					position: 'fixed',
					left: 0,
					right: 0
				},
				maskShow: true,
				mkclick: true,
				popupstyle: 'top'
			}
		},
		computed: {
			getStyles() {
				let res = { backgroundColor: this.bg };
				if (this.borderRadius || "0") {
					res = Object.assign(res, { borderRadius: this.borderRadius })
				}
				return res;
			},
			isDesktop() {
				return this.popupWidth >= 500 && this.popupHeight >= 500
			},
			bg() {
				if (this.backgroundColor === '' || this.backgroundColor === 'none') {
					return 'transparent'
				}
				return this.backgroundColor
			}
		},
		mounted() {
			const fixSize = () => {
				// #ifdef MP-WEIXIN
				const {
					windowWidth,
					windowHeight,
					windowTop,
					safeArea,
					screenHeight,
					safeAreaInsets
				} = uni.getWindowInfo()
				// #endif
				// #ifndef MP-WEIXIN
				const {
					windowWidth,
					windowHeight,
					windowTop,
					safeArea,
					screenHeight,
					safeAreaInsets
				} = uni.getSystemInfoSync()
				// #endif
				this.popupWidth = windowWidth
				this.popupHeight = windowHeight + (windowTop || 0)
				// TODO fix by mehaotian 是否適配底部安全區 ,目前微信ios 、和 app ios 計算有差異，需要框架修複
				if (safeArea && this.safeArea) {
					// #ifdef MP-WEIXIN
					this.safeAreaInsets = screenHeight - safeArea.bottom
					// #endif
					// #ifndef MP-WEIXIN
					this.safeAreaInsets = safeAreaInsets.bottom
					// #endif
				} else {
					this.safeAreaInsets = 0
				}
			}
			fixSize()
			// #ifdef H5
			// window.addEventListener('resize', fixSize)
			// this.$once('hook:beforeDestroy', () => {
			// 	window.removeEventListener('resize', fixSize)
			// })
			// #endif
		},
		// #ifndef VUE3
		// TODO vue2
		destroyed() {
			this.setH5Visible()
		},
		// #endif
		// #ifdef VUE3
		// TODO vue3
		unmounted() {
			this.setH5Visible()
		},
		// #endif
		activated() {
   	  this.setH5Visible(!this.showPopup);
    },
    deactivated() {
      this.setH5Visible(true);
    },
		created() {
			// this.mkclick =  this.isMaskClick || this.maskClick
			if (this.isMaskClick === null && this.maskClick === null) {
				this.mkclick = true
			} else {
				this.mkclick = this.isMaskClick !== null ? this.isMaskClick : this.maskClick
			}
			if (this.animation) {
				this.duration = 300
			} else {
				this.duration = 0
			}
			// TODO 處理 message 组件生命周期異常的問題
			this.messageChild = null
			// TODO 解决頭條冒泡的問題
			this.clearPropagation = false
			this.maskClass.backgroundColor = this.maskBackgroundColor
		},
		methods: {
			setH5Visible(visible = true) {
				// #ifdef H5
				// fix by mehaotian 處理 h5 滾動穿透的問題
				document.getElementsByTagName('body')[0].style.overflow =  visible ? "visible" : "hidden";
				// #endif
			},
			/**
			 * 公用方法，不顯示遮罩層
			 */
			closeMask() {
				this.maskShow = false
			},
			/**
			 * 公用方法，遮罩層禁止點擊
			 */
			disableMask() {
				this.mkclick = false
			},
			// TODO nvue 取消冒泡
			clear(e) {
				// #ifndef APP-NVUE
				e.stopPropagation()
				// #endif
				this.clearPropagation = true
			},

			open(direction) {
				// fix by mehaotian 處理快速打開關闭的情况
				if (this.showPopup) {
					return
				}
				let innerType = ['top', 'center', 'bottom', 'left', 'right', 'message', 'dialog', 'share']
				if (!(direction && innerType.indexOf(direction) !== -1)) {
					direction = this.type
				}
				if (!this.config[direction]) {
					console.error('缺少類型：', direction)
					return
				}
				this[this.config[direction]]()
				this.$emit('change', {
					show: true,
					type: direction
				})
			},
			close(type) {
				this.showTrans = false
				this.$emit('change', {
					show: false,
					type: this.type
				})
				clearTimeout(this.timer)
				// // 自定義關闭事件
				// this.customOpen && this.customClose()
				this.timer = setTimeout(() => {
					this.showPopup = false
				}, 300)
			},
			// TODO 處理冒泡事件，頭條的冒泡事件有問題 ，先這樣兼容
			touchstart() {
				this.clearPropagation = false
			},

			onTap() {
				if (this.clearPropagation) {
					// fix by mehaotian 兼容 nvue
					this.clearPropagation = false
					return
				}
				this.$emit('maskClick')
				if (!this.mkclick) return
				this.close()
			},
			/**
			 * 顶部彈出樣式處理
			 */
			top(type) {
				this.popupstyle = this.isDesktop ? 'fixforpc-top' : 'top'
				this.ani = ['slide-top']
				this.transClass = {
					position: 'fixed',
					left: 0,
					right: 0,
					backgroundColor: this.bg,
					borderRadius:this.borderRadius || "0"
				}
				// TODO 兼容 type 屬性 ，后续會废弃
				if (type) return
				this.showPopup = true
				this.showTrans = true
				this.$nextTick(() => {
					this.showPoptrans()
					if (this.messageChild && this.type === 'message') {
						this.messageChild.timerClose()
					}
				})
			},
			/**
			 * 底部彈出樣式處理
			 */
			bottom(type) {
				this.popupstyle = 'bottom'
				this.ani = ['slide-bottom']
				this.transClass = {
					position: 'fixed',
					left: 0,
					right: 0,
					bottom: 0,
					paddingBottom: this.safeAreaInsets + 'px',
					backgroundColor: this.bg,
					borderRadius:this.borderRadius || "0",
				}
				// TODO 兼容 type 屬性 ，后续會废弃
				if (type) return
				this.showPoptrans()
			},
			/**
			 * 中間彈出樣式處理
			 */
			center(type) {
				this.popupstyle = 'center'
				//微信小程序下，组合動画會出現文字向上闪動問題，再此做特殊處理
				// #ifdef MP-WEIXIN
					this.ani = ['fade']
				// #endif
				// #ifndef MP-WEIXIN
					this.ani = ['zoom-out', 'fade']
				// #endif
				this.transClass = {
					position: 'fixed',
					/* #ifndef APP-NVUE */
					display: 'flex',
					flexDirection: 'column',
					/* #endif */
					bottom: 0,
					left: 0,
					right: 0,
					top: 0,
					justifyContent: 'center',
					alignItems: 'center',
					borderRadius:this.borderRadius || "0"
				}
				// TODO 兼容 type 屬性 ，后续會废弃
				if (type) return
				this.showPoptrans()
			},
			left(type) {
				this.popupstyle = 'left'
				this.ani = ['slide-left']
				this.transClass = {
					position: 'fixed',
					left: 0,
					bottom: 0,
					top: 0,
					backgroundColor: this.bg,
					borderRadius:this.borderRadius || "0",
					/* #ifndef APP-NVUE */
					display: 'flex',
					flexDirection: 'column'
					/* #endif */
				}
				// TODO 兼容 type 屬性 ，后续會废弃
				if (type) return
				this.showPoptrans()
			},
			right(type) {
				this.popupstyle = 'right'
				this.ani = ['slide-right']
				this.transClass = {
					position: 'fixed',
					bottom: 0,
					right: 0,
					top: 0,
					backgroundColor: this.bg,
					borderRadius:this.borderRadius || "0",
					/* #ifndef APP-NVUE */
					display: 'flex',
					flexDirection: 'column'
					/* #endif */
				}
				// TODO 兼容 type 屬性 ，后续會废弃
				if (type) return
				this.showPoptrans()
			},
			showPoptrans(){
				this.$nextTick(()=>{
					this.showPopup = true
					this.showTrans = true
				})
			}
		}
	}
</script>
<style lang="scss">
	.uni-popup {
		position: fixed;
		/* #ifndef APP-NVUE */
		z-index: 99;

		/* #endif */
		&.top,
		&.left,
		&.right {
			/* #ifdef H5 */
			top: var(--window-top);
			/* #endif */
			/* #ifndef H5 */
			top: 0;
			/* #endif */
		}

		.uni-popup__wrapper {
			/* #ifndef APP-NVUE */
			display: block;
			/* #endif */
			position: relative;

			/* iphonex 等安全區設置，底部安全區適配 */
			/* #ifndef APP-NVUE */
			// padding-bottom: constant(safe-area-inset-bottom);
			// padding-bottom: env(safe-area-inset-bottom);
			/* #endif */
			&.left,
			&.right {
				/* #ifdef H5 */
				padding-top: var(--window-top);
				/* #endif */
				/* #ifndef H5 */
				padding-top: 0;
				/* #endif */
				flex: 1;
			}
		}
	}

	.fixforpc-z-index {
		/* #ifndef APP-NVUE */
		z-index: 999;
		/* #endif */
	}

	.fixforpc-top {
		top: 0;
	}
</style>
