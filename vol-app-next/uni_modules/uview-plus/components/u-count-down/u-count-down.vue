<template>
	<view class="u-count-down">
		<slot>
			<text class="u-count-down__text">{{ formattedTime }}</text>
		</slot>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import {
		isSameSecond,
		parseFormat,
		parseTimeData
	} from './utils';
	/**
	 * u-count-down 倒計時
	 * @description 該组件一般使用于某個活動的截止時間上，通過數字的變化，给用户明確的時間感受，提示用户进行某一個行為操作。
	 * @tutorial https://uview-plus.jiangruyi.com/components/countDown.html
	 * @property {String | Number}	time		倒計時時長，單位ms （默認 0 ）
	 * @property {String}			format		時間格式，DD-日，HH-時，mm-分，ss-秒，SSS-毫秒  （默認 'HH:mm:ss' ）
	 * @property {Boolean}			autoStart	是否自動開始倒計時 （默認 true ）
	 * @property {Boolean}			millisecond	是否展示毫秒倒計時 （默認 false ）
	 * @event {Function} finish 倒計時结束時触發 
	 * @event {Function} change 倒計時變化時触發 
	 * @event {Function} start	開始倒計時
	 * @event {Function} pause	暂停倒計時 
	 * @event {Function} reset	重設倒計時，若 auto-start 為 true，重設后會自動開始倒計時 
	 * @example <u-count-down :time="time"></u-count-down>
	 */
	export default {
		name: 'u-count-down',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				timer: null,
				// 各單位(天，時，分等)剩餘時間
				timeData: parseTimeData(0),
				// 格式化后的時間，如"03:23:21"
				formattedTime: '0',
				// 倒計時是否正在进行中
				runing: false,
				endTime: 0, // 结束的毫秒時間戳
				remainTime: 0, // 剩餘的毫秒時間
			}
		},
		watch: {
			time(n) {
				this.reset()
			}
		},
		mounted() {
			this.init()
		},
		emits: ["change", "finish"],
		methods: {
			init() {
				this.reset()
			},
			// 開始倒計時
			start() {
				if (this.runing) return
				// 標识為进行中
				this.runing = true
				// 结束時間戳 = 此刻時間戳 + 剩餘的時間
				this.endTime = Date.now() + this.remainTime
				this.toTick()
			},
			// 根據是否展示毫秒，執行不同操作函數
			toTick() {
				if (this.millisecond) {
					this.microTick()
				} else {
					this.macroTick()
				}
			},
			macroTick() {
				this.clearTimeout()
				// 每隔一定時間，更新一遍定時器的值
				// 同時此定時器的作用也能带来毫秒級的更新
				this.timer = setTimeout(() => {
					// 獲取剩餘時間
					const remain = this.getRemainTime()
					// 重設剩餘時間
					if (!isSameSecond(remain, this.remainTime) || remain === 0) {
						this.setRemainTime(remain)
					}
					// 如果剩餘時間不為0，则继续檢查更新倒計時
					if (this.remainTime !== 0) {
						this.macroTick()
					}
				}, 30)
			},
			microTick() {
				this.clearTimeout()
				this.timer = setTimeout(() => {
					this.setRemainTime(this.getRemainTime())
					if (this.remainTime !== 0) {
						this.microTick()
					}
				}, 50)
			},
			// 獲取剩餘的時間
			getRemainTime() {
				// 取最大值，防止出現小于0的剩餘時間值
				return Math.max(this.endTime - Date.now(), 0)
			},
			// 設置剩餘的時間
			setRemainTime(remain) {
				this.remainTime = remain
				// 根據剩餘的毫秒時間，得出該有天，小時，分钟等的值，返回一個對象
				const timeData = parseTimeData(remain)
				this.$emit('change', timeData)
				// 得出格式化后的時間
				this.formattedTime = parseFormat(this.format, timeData)
				// 如果時間已到，停止倒計時
				if (remain <= 0) {
					this.pause()
					this.$emit('finish')
				}
			},
			// 重置倒計時
			reset() {
				this.pause()
				this.remainTime = this.time
				this.setRemainTime(this.remainTime)
				if (this.autoStart) {
					this.start()
				}
			},
			// 暂停倒計時
			pause() {
				this.runing = false;
				this.clearTimeout()
			},
			// 清空定時器
			clearTimeout() {
				clearTimeout(this.timer)
				this.timer = null
			}
		},
		beforeUnmount() {
			this.clearTimeout()
		}
	}
</script>

<style
	lang="scss"
	scoped
>
	@import "../../libs/css/components.scss";
	$u-count-down-text-color:$u-content-color !default;
	$u-count-down-text-font-size:15px !default;
	$u-count-down-text-line-height:22px !default;

	.u-count-down {
		&__text {
			color: $u-count-down-text-color;
			font-size: $u-count-down-text-font-size;
			line-height: $u-count-down-text-line-height;
		}
	}
</style>
