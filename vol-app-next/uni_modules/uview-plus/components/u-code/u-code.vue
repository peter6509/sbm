<template>
	<view class="u-code">
		<!-- 此组件功能由js完成，無需写html逻輯 -->
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	/**
	 * Code 验证碼輸入框
	 * @description 考虑到用户實际發送验证碼的场景，可能是一個按鈕，也可能是一段文字，提示語各有不同，所以本组件 不提供界面顯示，只提供提示語，由用户將提示語嵌入到具體的场景
	 * @tutorial https://ijry.github.io/uview-plus/components/code.html
	 * @property {String | Number}	seconds			倒計時所需的秒數（默認 60 ）
	 * @property {String}			startText		開始前的提示語，見官网說明（默認 '獲取验证碼' ）
	 * @property {String}			changeText		倒計時期間的提示語，必須带有字母"x"，見官网說明（默認 'X秒重新獲取' ）
	 * @property {String}			endText			倒計结束的提示語，見官网說明（默認 '重新獲取' ）
	 * @property {Boolean}			keepRunning		是否在H5刷新或各端返回再进入時继续倒計時（ 默認false ）
	 * @property {String}			uniqueKey		為了區分多個頁面，或者一個頁面多個倒計時组件本地存儲的继续倒計時變了
	 *
	 * @event {Function}	change	倒計時期間，每秒触發一次
	 * @event {Function}	start	開始倒計時触發
	 * @event {Function}	end		结束倒計時触發
	 * @example <u-code ref="uCode" @change="codeChange" seconds="20"></u-code>
	 */
	export default {
		name: "u-code",
		mixins: [mpMixin, mixin,props],
		data() {
			return {
				secNum: this.seconds,
				timer: null,
				canGetCode: true, // 是否可以執行验证碼操作
			}
		},
		mounted() {
			this.checkKeepRunning()
		},
		watch: {
			seconds: {
				immediate: true,
				handler(n) {
					this.secNum = n
				}
			}
		},
		emits: ["start", "end", "change"],
		methods: {
			checkKeepRunning() {
				// 獲取上一次退出頁面(H5還包括刷新)時的時間戳，如果没有上次的保存，此值可能為空
				let lastTimestamp = Number(uni.getStorageSync(this.uniqueKey + '_$uCountDownTimestamp'))
				if(!lastTimestamp) return this.changeEvent(this.startText)
				// 當前秒的時間戳
				let nowTimestamp = Math.floor((+ new Date()) / 1000)
				// 判斷當前的時間戳，是否小于上一次的本該按設定结束，却提前结束的時間戳
				if(this.keepRunning && lastTimestamp && lastTimestamp > nowTimestamp) {
					// 剩餘尚未執行完的倒計秒數
					this.secNum = lastTimestamp - nowTimestamp
					// 清除本地保存的變量
					uni.removeStorageSync(this.uniqueKey + '_$uCountDownTimestamp')
					// 開始倒計時
					this.start()
				} else {
					// 如果不存在需要继续上一次的倒計時，執行正常的逻輯
					this.changeEvent(this.startText)
				}
			},
			// 開始倒計時
			start() {
				// 防止快速點擊獲取验证碼的按鈕而导致内部產生多個定時器导致混乱
				if(this.timer) {
					clearInterval(this.timer)
					this.timer = null
				}
				this.$emit('start')
				this.canGetCode = false
				// 這里放這句，是為了一開始時就提示，否则要等setInterval的1秒后才會有提示
				this.changeEvent(this.changeText.replace(/x|X/, this.secNum))
				this.timer = setInterval(() => {
					if (--this.secNum) {
						// 用當前倒計時的秒數替換提示字符串中的"x"字母
						this.changeEvent(this.changeText.replace(/x|X/, this.secNum))
					} else {
						clearInterval(this.timer)
						this.timer = null
						this.changeEvent(this.endText)
						this.secNum = this.seconds
						this.$emit('end')
						this.canGetCode = true
					}
				}, 1000)
				this.setTimeToStorage()
			},
			// 重置，可以让用户再次獲取验证碼
			reset() {
				this.canGetCode = true
				clearInterval(this.timer)
				this.secNum = this.seconds
				this.changeEvent(this.endText)
			},
			changeEvent(text) {
				this.$emit('change', text)
			},
			// 保存時間戳，為了防止倒計時尚未结束，H5刷新或者各端的右上角返回上一頁再进来
			setTimeToStorage() {
				if(!this.keepRunning || !this.timer) return
				// 记錄當前的時間戳，為了下次进入頁面，如果還在倒計時内的話，继续倒計時
				// 倒計時尚未结束，结果大于0；倒計時已經開始，就會小于初始值，如果等于初始值，說明没有開始倒計時，無需處理
				if(this.secNum > 0 && this.secNum < this.seconds) {
					// 獲取當前時間戳(+ new Date()為特殊写法)，除以1000變成秒，再去除小數部分
					let nowTimestamp = Math.floor((+ new Date()) / 1000)
					// 將本該结束時候的時間戳保存起来 => 當前時間戳 + 剩餘的秒數
					uni.setStorage({
						key: this.uniqueKey + '_$uCountDownTimestamp',
						data: nowTimestamp + Number(this.secNum)
					})
				}
			}
		},
		// 组件銷毁的時候，清除定時器，否则定時器會继续存在，系统不會自動清除
		beforeUnmount() {
			this.setTimeToStorage()
			clearTimeout(this.timer)
			this.timer = null
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
</style>
