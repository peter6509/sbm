<template>
	<text
		class="u-count-num"
		:style="{
			fontSize: addUnit(fontSize),
			fontWeight: bold ? 'bold' : 'normal',
			color: color
		}"
	>{{ displayValue }}</text>
</template>

<script>
import { props } from './props';
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { addUnit } from '../../libs/function/index';
/**
 * countTo 數字滾動
 * @description 該组件一般用于需要滾動數字到某一個值的场景，目標要求是一個递增的值。
 * @tutorial https://ijry.github.io/uview-plus/components/countTo.html
 * @property {String | Number}	startVal	開始的數值，默認從0增長到某一個數（默認 0 ）
 * @property {String | Number}	endVal		要滾動的目標數值，必須 （默認 0 ）
 * @property {String | Number}	duration	滾動到目標數值的動画持续時間，單位為毫秒（ms） （默認 2000 ）
 * @property {Boolean}			autoplay	設置數值后是否自動開始滾動 （默認 true ）
 * @property {String | Number}	decimals	要顯示的小數位數，見官网說明（默認 0 ）
 * @property {Boolean}			useEasing	滾動结束時，是否缓動结尾，見官网說明（默認 true ）
 * @property {String}			decimal		十进制分割 （ 默認 "." ）
 * @property {String}			color		字體顏色（ 默認 '#606266' )
 * @property {String | Number}	fontSize	字體大小，單位px（ 默認 22 ）
 * @property {Boolean}			bold		字體是否加粗（默認 false ）
 * @property {String}			separator	千位分隔符，見官网說明
 * @event {Function} end 數值滾動到目標值時触發
 * @example <u-count-to ref="uCountTo" :end-val="endVal" :autoplay="autoplay"></u-count-to>
 */
export default {
	name: 'u-count-to',
	data() {
		return {
			localStartVal: this.startVal,
			displayValue: this.formatNumber(this.startVal),
			printVal: null,
			paused: false, // 是否暂停
			localDuration: Number(this.duration),
			startTime: null, // 開始的時間
			timestamp: null, // 時間戳
			remaining: null, // 停留的時間
			rAF: null,
			lastTime: 0 // 上一次的時間
		};
	},
	mixins: [mpMixin, mixin,props],
	computed: {
		countDown() {
			return this.startVal > this.endVal;
		}
	},
	watch: {
		startVal() {
			this.autoplay && this.start();
		},
		endVal() {
			this.autoplay && this.start();
		}
	},
	mounted() {
		this.autoplay && this.start();
	},
	emits: ["end"],
	methods: {
		addUnit,
		easingFn(t, b, c, d) {
			return (c * (-Math.pow(2, (-10 * t) / d) + 1) * 1024) / 1023 + b;
		},
		requestAnimationFrame(callback) {
			const currTime = new Date().getTime();
			// 為了使setTimteout的尽可能的接近每秒60帧的效果
			const timeToCall = Math.max(0, 16 - (currTime - this.lastTime));
			const id = setTimeout(() => {
				callback(currTime + timeToCall);
			}, timeToCall);
			this.lastTime = currTime + timeToCall;
			return id;
		},
		cancelAnimationFrame(id) {
			clearTimeout(id);
		},
		// 開始滾動數字
		start() {
			this.localStartVal = this.startVal;
			this.startTime = null;
			this.localDuration = this.duration;
			this.paused = false;
			this.rAF = this.requestAnimationFrame(this.count);
		},
		// 暂定狀態，重新再開始滾動；或者滾動狀態下，暂停
		reStart() {
			if (this.paused) {
				this.resume();
				this.paused = false;
			} else {
				this.stop();
				this.paused = true;
			}
		},
		// 暂停
		stop() {
			this.cancelAnimationFrame(this.rAF);
		},
		// 重新開始(暂停的情况下)
		resume() {
			if (!this.remaining) return
			this.startTime = 0;
			this.localDuration = this.remaining;
			this.localStartVal = this.printVal;
			this.requestAnimationFrame(this.count);
		},
		// 重置
		reset() {
			this.startTime = null;
			this.cancelAnimationFrame(this.rAF);
			this.displayValue = this.formatNumber(this.startVal);
		},
		count(timestamp) {
			if (!this.startTime) this.startTime = timestamp;
			this.timestamp = timestamp;
			const progress = timestamp - this.startTime;
			this.remaining = this.localDuration - progress;
			if (this.useEasing) {
				if (this.countDown) {
					this.printVal = this.localStartVal - this.easingFn(progress, 0, this.localStartVal - this.endVal, this.localDuration);
				} else {
					this.printVal = this.easingFn(progress, this.localStartVal, this.endVal - this.localStartVal, this.localDuration);
				}
			} else {
				if (this.countDown) {
					this.printVal = this.localStartVal - (this.localStartVal - this.endVal) * (progress / this.localDuration);
				} else {
					this.printVal = this.localStartVal + (this.endVal - this.localStartVal) * (progress / this.localDuration);
				}
			}
			if (this.countDown) {
				this.printVal = this.printVal < this.endVal ? this.endVal : this.printVal;
			} else {
				this.printVal = this.printVal > this.endVal ? this.endVal : this.printVal;
			}
			this.displayValue = this.formatNumber(this.printVal) || 0;
			if (progress < this.localDuration) {
				this.rAF = this.requestAnimationFrame(this.count);
			} else {
				this.$emit('end');
			}
		},
		// 判斷是否數字
		isNumber(val) {
			return !isNaN(parseFloat(val));
		},
		formatNumber(num) {
			// 將num轉為Number類型，因為其值可能為字符串數值，調用toFixed會報错
			num = Number(num);
			num = num.toFixed(Number(this.decimals));
			num += '';
			const x = num.split('.');
			let x1 = x[0];
			const x2 = x.length > 1 ? this.decimal + x[1] : '';
			const rgx = /(\d+)(\d{3})/;
			if (this.separator && !this.isNumber(this.separator)) {
				while (rgx.test(x1)) {
					x1 = x1.replace(rgx, '$1' + this.separator + '$2');
				}
			}
			return x1 + x2;
		},
		destroyed() {
			this.cancelAnimationFrame(this.rAF);
		}
	}
};
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";

.u-count-num {
	/* #ifndef APP-NVUE */
	display: inline-flex;
	/* #endif */
	text-align: center;
}
</style>
