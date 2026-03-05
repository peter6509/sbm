<template>
	<view class="u-char-box">
		<view class="u-char-flex">
			<input :disabled="disabledKeyboard" :value="valueModel" type="number" :focus="focus" :maxlength="maxlength" class="u-input" @input="getVal"/>
			<view v-for="(item, index) in loopCharArr" :key="index">
				<view :class="[breathe && charArrLength == index ? 'u-breathe' : '', 'u-char-item',
				charArrLength === index && mode == 'box' ? 'u-box-active' : '',
				mode === 'box' ? 'u-box' : '']" :style="{
					fontWeight: bold ? 'bold' : 'normal',
					fontSize: fontSize + 'rpx',
					width: width + 'rpx',
					height: width + 'rpx',
					color: inactiveColor,
					borderColor: charArrLength === index && mode == 'box' ? activeColor : inactiveColor
				}">
					<view class="u-placeholder-line" :style="{
							display: charArrLength === index ? 'block' : 'none',
							height: width * 0.5 +'rpx'
						}"
						v-if="mode !== 'middleLine'"
					></view>
					<view v-if="mode === 'middleLine' && charArrLength <= index" :class="[breathe && charArrLength == index ? 'u-breathe' : '', charArrLength === index ? 'u-middle-line-active' : '']"
					 class="u-middle-line" :style="{height: bold ? '4px' : '2px', background: charArrLength === index ? activeColor : inactiveColor}"></view>
					<view v-if="mode === 'bottomLine'" :class="[breathe && charArrLength == index ? 'u-breathe' : '', charArrLength === index ? 'u-bottom-line-active' : '']"
					 class="u-bottom-line" :style="{height: bold ? '4px' : '2px', background: charArrLength === index ? activeColor : inactiveColor}"></view>
					<block v-if="!dotFill"> {{ charArr[index] ? charArr[index] : ''}}</block>
					<block v-else>
						<text class="u-dot">{{ charArr[index] ? '●' : ''}}</text>
					</block>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	/**
	 * messageInput 验证碼輸入框
	 * @description 該组件一般用于验证用户短信验证碼的场景，也可以结合uView的鍵盘组件使用
	 * @tutorial https://www.uviewui.com/components/messageInput.html
	 * @property {String Number} maxlength 輸入字符個數（默認4）
	 * @property {Boolean} dot-fill 是否用圆點填充（默認false）
	 * @property {String} mode 模式選擇，見上方"基本使用"說明（默認box）
	 * @property {String Number} value 預置值
	 * @property {Boolean} breathe 是否開啟呼吸效果，見上方說明（默認true）
	 * @property {Boolean} focus 是否自動獲取焦點（默認false）
	 * @property {Boolean} bold 字體和輸入横线是否加粗（默認true）
	 * @property {String Number} font-size 字體大小，單位rpx（默認60）
	 * @property {String} active-color 當前激活輸入框的樣式（默認#2979ff）
	 * @property {String} inactive-color 非激活輸入框的樣式，文字顏色同此值（默認#606266）
	 * @property {String | Number} width 輸入框寬度，單位rpx，高等于寬（默認80）
	 * @property {Boolean} disabled-keyboard 禁止點擊輸入框唤起系统鍵盘（默認false）
	 * @event {Function} change 輸入内容發生改變時触發，具體見官网說明
	 * @event {Function} finish 輸入字符個數达maxlength值時触發，見官网說明
	 * @example <u-message-input mode="bottomLine"></u-message-input>
	 */
	export default {
		name: "u-message-input",
		props: {
			// 最大輸入長度
			maxlength: {
				type: [Number, String],
				default: 4
			},
			// 是否用圆點填充
			dotFill: {
				type: Boolean,
				default: false
			},
			// 顯示模式，box-盒子模式，bottomLine-横线在底部模式，middleLine-横线在中部模式
			mode: {
				type: String,
				default: "box"
			},
			// 預置值
			modelValue: {
				type: [String, Number],
				default: ''
			},
			// 當前激活輸入item，是否带有呼吸效果
			breathe: {
				type: Boolean,
				default: true
			},
			// 是否自動獲取焦點
			focus: {
				type: Boolean,
				default: false
			},
			// 字體是否加粗
			bold: {
				type: Boolean,
				default: false
			},
			// 字體大小
			fontSize: {
				type: [String, Number],
				default: 60
			},
			// 激活樣式
			activeColor: {
				type: String,
				default: '#2979ff'
			},
			// 未激活的樣式
			inactiveColor: {
				type: String,
				default: '#606266'
			},
			// 輸入框的大小，單位rpx，寬等于高
			width: {
				type: [Number, String],
				default: '80'
			},
			// 是否隐藏原生鍵盘，如果想用自定義鍵盘的話，需設置此参數為true
			disabledKeyboard: {
				type: Boolean,
				default: false
			}
		},
		watch: {
			// maxlength: {
			// 	// 此值設置為true，會在组件加載后無需maxlength變化就會執行一次本監聽函數，無需再created生命周期中處理
			// 	immediate: true,
			// 	handler(val) {
			// 		this.maxlength = Number(val);
			// 	}
			// }, 
			modelValue: {
				immediate: true,
				handler(val) {
					// 轉為字符串
					val = String(val);
					// 超出部分截掉
					this.valueModel = val.substring(0, this.maxlength);
				}
			},
		},
		data() {
			return {
				valueModel: ""
			}
		},
		emits: ['change', 'finish'],
		computed: {
			// 是否顯示呼吸灯效果
			animationClass() {
				return (index) => {
					if (this.breathe && this.charArr.length == index) return 'u-breathe';
					else return '';
				}
			},
			// 用于顯示字符
			charArr() {
				return this.valueModel.split('');
			},
			charArrLength() {
				return this.charArr.length;
			},
			// 根據長度，循环輸入框的個數，因為頭條小程序數值不能用于v-for
			loopCharArr() {
				return new Array(this.maxlength);
			}
		},
		methods: {
			getVal(e) {
				let {
					value
				} = e.detail
				this.valueModel = value;
				// 判斷長度是否超出了maxlength值，理论上不會發生，因為input组件設置了maxlength屬性值
				if (String(value).length > this.maxlength) return;
				// 未达到maxlength之前，發送change事件，达到后發送finish事件
				this.$emit('change', value);
				if (String(value).length == this.maxlength) {
					this.$emit('finish', value);
				}
			}
		}
	}
</script>

<style scoped lang="scss">
	// 定義混入指令，用于在非nvue环境下的flex定義，因為nvue没有display屬性，會報错
	@mixin vue-flex($direction: row) {
		/* #ifndef APP-NVUE */
		display: flex;
		flex-direction: $direction;
		/* #endif */
	}

	@keyframes breathe {
		0% {
			opacity: 0.3;
		}

		50% {
			opacity: 1;
		}

		100% {
			opacity: 0.3;
		}
	}

	.u-char-box {
		text-align: center;
	}

	.u-char-flex {
		@include vue-flex;
		justify-content: center;
		flex-wrap: wrap;
		position: relative;
	}

	.u-input {
		position: absolute;
		top: 0;
		left: -100%;
		width: 200%;
		height: 100%;
		text-align: left;
		z-index: 9;
		opacity: 0;
		background: none;
	}

	.u-char-item {
		position: relative;
		width: 90rpx;
		height: 90rpx;
		margin: 10rpx 10rpx;
		font-size: 60rpx;
		font-weight: bold;
		color: $u-main-color;
		line-height: 90rpx;
		@include vue-flex;
		justify-content: center;
		align-items: center;
	}

	.u-middle-line {
		border: none;
	}

	.u-box {
		box-sizing: border-box;
		border: 2rpx solid #cccccc;
		border-radius: 6rpx;
	}

	.u-box-active {
		overflow: hidden;
		animation-timing-function: ease-in-out;
		animation-duration: 1500ms;
		animation-iteration-count: infinite;
		animation-direction: alternate;
		border: 2rpx solid $u-primary;
	}

	.u-middle-line-active {
		background: $u-primary;
	}

	.u-breathe {
		animation: breathe 2s infinite ease;
	}

	.u-placeholder-line {
		/* #ifndef APP-NVUE */
		display: none;
		/* #endif */
		position: absolute;
		left: 50%;
		top: 50%;
		transform: translate(-50%, -50%);
		width: 2rpx;
		height: 40rpx;
		background: #333333;
		animation: twinkling 1.5s infinite ease;
	}

	.u-animation-breathe {
		animation-name: breathe;
	}

	.u-dot {
		font-size: 34rpx;
		line-height: 34rpx;
	}

	.u-middle-line {
		height: 4px;
		background: #000000;
		width: 80%;
		position: absolute;
		border-radius: 2px;
		top: 50%;
		left: 50%;
		transform: translate(-50%, -50%);
	}

	.u-bottom-line-active {
		background: $u-primary;
	}

	.u-bottom-line {
		height: 4px;
		background: #000000;
		width: 80%;
		position: absolute;
		border-radius: 2px;
		bottom: 0;
		left: 50%;
		transform: translate(-50%);
	}
</style>
