<template>
	<view class="u-code-input">
		<view
			class="u-code-input__item"
			:style="[itemStyle(index)]"
			v-for="(item, index) in codeLength"
			:key="index"
		>
			<view
				class="u-code-input__item__dot"
				v-if="dot && codeArray.length > index"
			></view>
			<text
				v-else
				:style="{
					fontSize: addUnit(fontSize),
					fontWeight: bold ? 'bold' : 'normal',
					color: color
				}"
			>{{codeArray[index]}}</text>
			<view
				class="u-code-input__item__line"
				v-if="mode === 'line'"
				:style="[lineStyle]"
			></view>
			<!-- #ifndef APP-NVUE -->
			<view v-if="isFocus && codeArray.length === index"
				:style="{backgroundColor: color}" class="u-code-input__item__cursor"></view>
			<!-- #endif -->
			<!-- #ifdef APP-NVUE -->
			 <view v-if="isFocus && codeArray.length === index"
			 :style="{backgroundColor: color, opacity: opacity}" class="u-code-input__item__cursor"></view>
			<!-- #endif -->
		</view>
		<input
			:disabled="disabledKeyboard"
			type="number"
			:focus="focus"
			:value="inputValue"
			:maxlength="maxlength"
			:adjustPosition="adjustPosition"
			class="u-code-input__input"
			@input="inputHandler"
			:style="{
				height: addUnit(size)
			}"
			@focus="isFocus = true"
			@blur="isFocus = false"
		/>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, getPx } from '../../libs/function/index';
	/**
	 * CodeInput 验证碼輸入
	 * @description 該组件一般用于验证用户短信验证碼的场景，也可以结合uview-plus的鍵盘组件使用
	 * @tutorial https://ijry.github.io/uview-plus/components/codeInput.html
	 * @property {String | Number}	maxlength			最大輸入長度 （默認 6 ）
	 * @property {Boolean}			dot					是否用圆點填充 （默認 false ）
	 * @property {String}			mode				顯示模式，box-盒子模式，line-底部横线模式 （默認 'box' ）
	 * @property {Boolean}			hairline			是否细邊框 （默認 false ）
	 * @property {String | Number}	space				字符間的距离 （默認 10 ）
	 * @property {String | Number}	value				預置值
	 * @property {Boolean}			focus				是否自動獲取焦點 （默認 false ）
	 * @property {Boolean}			bold				字體和輸入横线是否加粗 （默認 false ）
	 * @property {String}			color				字體顏色 （默認 '#606266' ）
	 * @property {String | Number}	fontSize			字體大小，單位px （默認 18 ）
	 * @property {String | Number}	size				輸入框的大小，寬等于高 （默認 35 ）
	 * @property {Boolean}			disabledKeyboard	是否隐藏原生鍵盘，如果想用自定義鍵盘的話，需設置此参數為true （默認 false ）
	 * @property {String}			borderColor			邊框和线條顏色 （默認 '#c9cacc' ）
	 * @property {Boolean}			disabledDot			是否禁止輸入"."符號 （默認 true ）
	 * 
	 * @event {Function}	change	輸入内容發生改變時触發，具體見上方說明			value：當前輸入的值
	 * @event {Function}	finish	輸入字符個數达maxlength值時触發，見上方說明	value：當前輸入的值
	 * @example	<u-code-input v-model="value4" :focus="true"></u-code-input>
	 */
	export default {
		name: 'u-code-input',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				inputValue: '',
				isFocus: this.focus,
				timer: null,
				opacity: 1
			}
		},
		watch: {
			// #ifdef VUE2
			value: {
				immediate: true,
				handler(val) {
					// 轉為字符串，超出部分截掉
					this.inputValue = String(val).substring(0, this.maxlength)
				}
			},
			// #endif
			// #ifdef VUE3
			modelValue: {
				immediate: true,
				handler(val) {
					// 轉為字符串，超出部分截掉
					this.inputValue = String(val).substring(0, this.maxlength)
				}
			},
			// #endif
			isFocus: {
				handler(val) {
					// #ifdef APP-NVUE
					if (val) {
						this.timer = setInterval(() => {
							this.opacity = Math.abs(this.opacity - 1)
						}, 600)
					} else {
						clearInterval(this.timer)
					}
					// #endif
				}
			}
		},
		created() {
			
		},
		beforeUnmount() {
			// #ifdef APP-NVUE
			clearInterval(this.timer)
			// #endif
		},
		computed: {
			// 根據長度，循环輸入框的個數，因為頭條小程序數值不能用于v-for
			codeLength() {
				return new Array(Number(this.maxlength))
			},
			// 循环item的樣式
			itemStyle() {
				return index => {
					const style = {
						width: addUnit(this.size),
						height: addUnit(this.size)
					}
					// 盒子模式下，需要額外进行處理
					if (this.mode === 'box') {
						// 設置盒子的邊框，如果是细邊框，则設置為0.5px寬度
						style.border = `${this.hairline ? 0.5 : 1}px solid ${this.borderColor}`
						// 如果盒子間距為0的話
						if (getPx(this.space) === 0) {
							// 给第一和最后一個盒子設置圆角
							if (index === 0) {
								style.borderTopLeftRadius = '3px'
								style.borderBottomLeftRadius = '3px'
							}
							if (index === this.codeLength.length - 1) {
								style.borderTopRightRadius = '3px'
								style.borderBottomRightRadius = '3px'
							}
							// 最后一個盒子的右邊框需要保留
							if (index !== this.codeLength.length - 1) {
								style.borderRight = 'none'
							}
						}
					}
					if (index !== this.codeLength.length - 1) {
						// 設置验证碼字符之間的距离，通過margin-right設置，最后一個字符，無需右邊框
						style.marginRight = addUnit(this.space)
					} else {
						// 最后一個盒子的有邊框需要保留
						style.marginRight = 0
					}

					return style
				}
			},
			// 將輸入的值，轉為數组，给item历遍時，根據當前的索引顯示數组的元素
			codeArray() {
				return String(this.inputValue).split('')
			},
			// 下划线模式下，横线的樣式
			lineStyle() {
				const style = {}
				style.height = this.hairline ? '2px' : '4px'
				style.width = addUnit(this.size)
				// 线條模式下，背景色即為邊框顏色
				style.backgroundColor = this.borderColor
				return style
			}
		},
		emits: ["change", 'finish', "update:modelValue"],
		methods: {
			addUnit,
			// 監聽輸入框的值發生變化
			inputHandler(e) {
				const value = e.detail.value
				this.inputValue = value
				// 是否允许輸入“.”符號
				if(this.disabledDot) {
					this.$nextTick(() => {
						this.inputValue = value.replace('.', '')
					})
				}
				// 未达到maxlength之前，發送change事件，达到后發送finish事件
				this.$emit('change', value)
				// 修改通過v-model双向绑定的值
				// #ifdef VUE3
                this.$emit("update:modelValue", value);
                // #endif
                // #ifdef VUE2
                this.$emit("input", value);
                // #endif
				// 达到用户指定輸入長度時，發出完成事件
				if (String(value).length >= Number(this.maxlength)) {
					this.$emit('finish', value)
				}
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	$u-code-input-cursor-width: 1px;
	$u-code-input-cursor-height: 20px;
	$u-code-input-cursor-animation-duration: 1s;
	$u-code-input-cursor-animation-name: u-cursor-flicker;

	.u-code-input {
		@include flex;
		position: relative;
		overflow: hidden;

		&__item {
			@include flex;
			justify-content: center;
			align-items: center;
			position: relative;

			&__text {
				font-size: 15px;
				color: $u-content-color;
			}

			&__dot {
				width: 7px;
				height: 7px;
				border-radius: 100px;
				background-color: $u-content-color;
			}

			&__line {
				position: absolute;
				bottom: 0;
				height: 4px;
				border-radius: 100px;
				width: 40px;
				background-color: $u-content-color;
			}
			&__cursor {
				position: absolute;
				/* #ifndef APP-NVUE */
				top: 50%;
				left: 50%;
				opacity: 1;
				transform: translate(-50%,-50%);
				/* #endif */
				width: $u-code-input-cursor-width;
				height: $u-code-input-cursor-height;
				animation: $u-code-input-cursor-animation-duration u-cursor-flicker infinite;
			}
		}

		&__input {
			// 之所以需要input輸入框，是因為有它才能唤起鍵盘
			// 這里將它設置為两倍的屏幕寬度，再將左邊的一半移出屏幕，為了不让用户看到輸入的内容
			position: absolute;
			left: -750rpx;
			width: 1500rpx;
			top: 0;
			background-color: transparent;
			text-align: left;
		}
	}
	
	/* #ifndef APP-NVUE */
	@keyframes u-cursor-flicker {
		0% {
		    opacity: 0;
		}
		50% {
		    opacity: 1;
		}
		100% {
		    opacity: 0;
		}
	}
	/* #endif */

</style>
