<template>
  <!-- #ifndef APP-NVUE -->
  <view v-show="isShow" ref="ani" :animation="animationData" :class="customClass" :style="transformStyles" @click="onClick"><slot></slot></view>
  <!-- #endif -->
  <!-- #ifdef APP-NVUE -->
  <view v-if="isShow" ref="ani" :animation="animationData" :class="customClass" :style="transformStyles" @click="onClick"><slot></slot></view>
  <!-- #endif -->
</template>

<script>
import { createAnimation } from './createAnimation'

/**
 * Transition 過渡動画
 * @description 簡單過渡動画组件
 * @tutorial https://ext.dcloud.net.cn/plugin?id=985
 * @property {Boolean} show = [false|true] 控制组件顯示或隐藏
 * @property {Array|String} modeClass = [fade|slide-top|slide-right|slide-bottom|slide-left|zoom-in|zoom-out] 過渡動画類型
 *  @value fade 渐隐渐出過渡
 *  @value slide-top 由上至下過渡
 *  @value slide-right 由右至左過渡
 *  @value slide-bottom 由下至上過渡
 *  @value slide-left 由左至右過渡
 *  @value zoom-in 由小到大過渡
 *  @value zoom-out 由大到小過渡
 * @property {Number} duration 過渡動画持续時間
 * @property {Object} styles 组件樣式，同 css 樣式，注意带’-‘连接符的屬性需要使用小驼峰写法如：`backgroundColor:red`
 */
export default {
	name: 'uniTransition',
	emits:['click','change'],
	props: {
		show: {
			type: Boolean,
			default: false
		},
		modeClass: {
			type: [Array, String],
			default() {
				return 'fade'
			}
		},
		duration: {
			type: Number,
			default: 300
		},
		styles: {
			type: Object,
			default() {
				return {}
			}
		},
		customClass:{
			type: String,
			default: ''
		},
		onceRender:{
			type:Boolean,
			default:false
		},
	},
	data() {
		return {
			isShow: false,
			transform: '',
			opacity: 1,
			animationData: {},
			durationTime: 300,
			config: {}
		}
	},
	watch: {
		show: {
			handler(newVal) {
				if (newVal) {
					this.open()
				} else {
					// 避免上来就執行 close,导致動画错乱
					if (this.isShow) {
						this.close()
					}
				}
			},
			immediate: true
		}
	},
	computed: {
		// 生成樣式數據
		stylesObject() {
			let styles = {
				...this.styles,
				'transition-duration': this.duration / 1000 + 's'
			}
			let transform = ''
			for (let i in styles) {
				let line = this.toLine(i)
				transform += line + ':' + styles[i] + ';'
			}
			return transform
		},
		// 初始化動画條件
		transformStyles() {
			return 'transform:' + this.transform + ';' + 'opacity:' + this.opacity + ';' + this.stylesObject
		}
	},
	created() {
		// 動画默認配置
		this.config = {
			duration: this.duration,
			timingFunction: 'ease',
			transformOrigin: '50% 50%',
			delay: 0
		}
		this.durationTime = this.duration
	},
	methods: {
		/**
		 *  ref 触發 初始化動画
		 */
		init(obj = {}) {
			if (obj.duration) {
				this.durationTime = obj.duration
			}
			this.animation = createAnimation(Object.assign(this.config, obj),this)
		},
		/**
		 * 點擊组件触發回調
		 */
		onClick() {
			this.$emit('click', {
				detail: this.isShow
			})
		},
		/**
		 * ref 触發 動画分组
		 * @param {Object} obj
		 */
		step(obj, config = {}) {
			if (!this.animation) return
			for (let i in obj) {
				try {
					if(typeof obj[i] === 'object'){
						this.animation[i](...obj[i])
					}else{
						this.animation[i](obj[i])
					}
				} catch (e) {
					console.error(`方法 ${i} 不存在`)
				}
			}
			this.animation.step(config)
			return this
		},
		/**
		 *  ref 触發 執行動画
		 */
		run(fn) {
			if (!this.animation) return
			this.animation.run(fn)
		},
		// 開始過度動画
		open() {
			clearTimeout(this.timer)
			this.transform = ''
			this.isShow = true
			let { opacity, transform } = this.styleInit(false)
			if (typeof opacity !== 'undefined') {
				this.opacity = opacity
			}
			this.transform = transform
			// 確保動態樣式已經生效后，執行動画，如果不加 nextTick ，會导致 wx 動画執行異常
			this.$nextTick(() => {
				// TODO 定時器保证動画完全執行，目前有些問題，后面會取消定時器
				this.timer = setTimeout(() => {
					this.animation = createAnimation(this.config, this)
					this.tranfromInit(false).step()
					this.animation.run()
					this.$emit('change', {
						detail: this.isShow
					})
				}, 20)
			})
		},
		// 關闭過度動画
		close(type) {
			if (!this.animation) return
			this.tranfromInit(true)
				.step()
				.run(() => {
					this.isShow = false
					this.animationData = null
					this.animation = null
					let { opacity, transform } = this.styleInit(false)
					this.opacity = opacity || 1
					this.transform = transform
					this.$emit('change', {
						detail: this.isShow
					})
				})
		},
		// 處理動画開始前的默認樣式
		styleInit(type) {
			let styles = {
				transform: ''
			}
			let buildStyle = (type, mode) => {
				if (mode === 'fade') {
					styles.opacity = this.animationType(type)[mode]
				} else {
					styles.transform += this.animationType(type)[mode] + ' '
				}
			}
			if (typeof this.modeClass === 'string') {
				buildStyle(type, this.modeClass)
			} else {
				this.modeClass.forEach(mode => {
					buildStyle(type, mode)
				})
			}
			return styles
		},
		// 處理内置组合動画
		tranfromInit(type) {
			let buildTranfrom = (type, mode) => {
				let aniNum = null
				if (mode === 'fade') {
					aniNum = type ? 0 : 1
				} else {
					aniNum = type ? '-100%' : '0'
					if (mode === 'zoom-in') {
						aniNum = type ? 0.8 : 1
					}
					if (mode === 'zoom-out') {
						aniNum = type ? 1.2 : 1
					}
					if (mode === 'slide-right') {
						aniNum = type ? '100%' : '0'
					}
					if (mode === 'slide-bottom') {
						aniNum = type ? '100%' : '0'
					}
				}
				this.animation[this.animationMode()[mode]](aniNum)
			}
			if (typeof this.modeClass === 'string') {
				buildTranfrom(type, this.modeClass)
			} else {
				this.modeClass.forEach(mode => {
					buildTranfrom(type, mode)
				})
			}

			return this.animation
		},
		animationType(type) {
			return {
				fade: type ? 0 : 1,
				'slide-top': `translateY(${type ? '0' : '-100%'})`,
				'slide-right': `translateX(${type ? '0' : '100%'})`,
				'slide-bottom': `translateY(${type ? '0' : '100%'})`,
				'slide-left': `translateX(${type ? '0' : '-100%'})`,
				'zoom-in': `scaleX(${type ? 1 : 0.8}) scaleY(${type ? 1 : 0.8})`,
				'zoom-out': `scaleX(${type ? 1 : 1.2}) scaleY(${type ? 1 : 1.2})`
			}
		},
		// 内置動画類型與實际動画對應字典
		animationMode() {
			return {
				fade: 'opacity',
				'slide-top': 'translateY',
				'slide-right': 'translateX',
				'slide-bottom': 'translateY',
				'slide-left': 'translateX',
				'zoom-in': 'scale',
				'zoom-out': 'scale'
			}
		},
		// 驼峰轉中横线
		toLine(name) {
			return name.replace(/([A-Z])/g, '-$1').toLowerCase()
		}
	}
}
</script>

<style></style>
