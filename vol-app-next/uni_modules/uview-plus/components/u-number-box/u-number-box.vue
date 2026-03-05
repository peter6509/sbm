<template>
	<view class="u-number-box">
		<view
		    class="u-number-box__slot cursor-pointer"
		    @tap.stop="clickHandler('minus')"
		    @touchstart="onTouchStart('minus')"
		    @touchend.stop="clearTimeout"
		    v-if="showMinus && $slots.minus"
		>
			<slot name="minus" />
		</view>
		<view
		    v-else-if="showMinus"
		    class="u-number-box__minus cursor-pointer"
		    @tap.stop="clickHandler('minus')"
		    @touchstart="onTouchStart('minus')"
		    @touchend.stop="clearTimeout"
		    hover-class="u-number-box__minus--hover"
		    hover-stay-time="150"
		    :class="{ 'u-number-box__minus--disabled': isDisabled('minus') }"
		    :style="[buttonStyle('minus')]"
		>
			<u-icon
			    name="minus"
			    :color="isDisabled('minus') ? '#c8c9cc' : '#323233'"
			    size="15"
			    bold
				:customStyle="iconStyle"
			></u-icon>
		</view>

		<slot name="input">
			<!-- #ifdef MP-WEIXIN -->
			<input
			    :disabled="disabledInput || disabled"
			    :cursor-spacing="getCursorSpacing"
			    :class="{ 'u-number-box__input--disabled': disabled || disabledInput }"
			    :value="currentValue"
			    class="u-number-box__input"
			    @blur="onBlur"
			    @focus="onFocus"
			    @input="onInput"
			    type="number"
			    :style="[inputStyle]"
			/>
			<!-- #endif -->
			<!-- #ifndef MP-WEIXIN -->
			<input
			    :disabled="disabledInput || disabled"
			    :cursor-spacing="getCursorSpacing"
			    :class="{ 'u-number-box__input--disabled': disabled || disabledInput }"
			    v-model="currentValue"
			    class="u-number-box__input"
			    @blur="onBlur"
			    @focus="onFocus"
			    @input="onInput"
			    type="number"
			    :style="[inputStyle]"
			/>
			<!-- #endif -->
		</slot>
		<view
		    class="u-number-box__slot cursor-pointer"
		    @tap.stop="clickHandler('plus')"
		    @touchstart="onTouchStart('plus')"
		    @touchend.stop="clearTimeout"
		    v-if="showPlus && $slots.plus"
		>
			<slot name="plus" />
		</view>
		<view
		    v-else-if="showPlus"
		    class="u-number-box__plus cursor-pointer"
		    @tap.stop="clickHandler('plus')"
		    @touchstart="onTouchStart('plus')"
		    @touchend.stop="clearTimeout"
		    hover-class="u-number-box__plus--hover"
		    hover-stay-time="150"
		    :class="{ 'u-number-box__minus--disabled': isDisabled('plus') }"
		    :style="[buttonStyle('plus')]"
		>
			<u-icon
			    name="plus"
			    :color="isDisabled('plus') ? '#c8c9cc' : '#323233'"
			    size="15"
			    bold
				:customStyle="iconStyle"
			></u-icon>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { getPx, addUnit } from '../../libs/function/index';
	/**
	 * numberBox 步进器
	 * @description 該组件一般用于商城购物選擇物品數量的场景。
	 * @tutorial https://uview-plus.jiangruyi.com/components/numberBox.html
	 * @property {String | Number}	name			步进器標识符，在change回調返回
	 * @property {String | Number}	value			用于双向绑定的值，初始化時設置設為默認min值(最小值)  （默認 0 ）
	 * @property {String | Number}	min				最小值 （默認 1 ）
	 * @property {String | Number}	max				最大值 （默認 Number.MAX_SAFE_INTEGER ）
	 * @property {String | Number}	step			加减的步長，可為小數 （默認 1 ）
	 * @property {Boolean}			integer			是否只允许輸入整數 （默認 false ）
	 * @property {Boolean}			disabled		是否禁用，包括輸入框，加减按鈕 （默認 false ）
	 * @property {Boolean}			disabledInput	是否禁用輸入框 （默認 false ）
	 * @property {Boolean}			asyncChange		是否開啟異步變更，開啟后需要手動控制輸入值 （默認 false ）
	 * @property {String | Number}	inputWidth		輸入框寬度，單位為px （默認 35 ）
	 * @property {Boolean}			showMinus		是否顯示减少按鈕 （默認 true ）
	 * @property {Boolean}			showPlus		是否顯示增加按鈕 （默認 true ）
	 * @property {String | Number}	decimalLength	顯示的小數位數
	 * @property {Boolean}			longPress		是否開啟長按加减手势 （默認 true ）
	 * @property {String}			color			輸入框文字和加减按鈕圖標的顏色 （默認 '#323233' ）
	 * @property {String | Number}	buttonSize		按鈕大小，寬高等于此值，單位px，輸入框高度和此值保持一致 （默認 30 ）
	 * @property {String}			bgColor			輸入框和按鈕的背景顏色 （默認 '#EBECEE' ）
	 * @property {String | Number}	cursorSpacing	指定光標于鍵盘的距离，避免鍵盘遮挡輸入框，單位px （默認 100 ）
	 * @property {Boolean}			disablePlus		是否禁用增加按鈕 （默認 false ）
	 * @property {Boolean}			disableMinus	是否禁用减少按鈕 （默認 false ）
	 * @property {Object ｜ String}	iconStyle		加减按鈕圖標的樣式
	 *
	 * @event {Function}	onFocus	輸入框活動焦點
	 * @event {Function}	onBlur	輸入框失去焦點
	 * @event {Function}	onInput	輸入框值發生變化
	 * @event {Function}	onChange
	 * @example <u-number-box v-model="value" @change="valChange"></u-number-box>
	 */
	export default {
		name: 'u-number-box',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				// 輸入框實际操作的值
				currentValue: '',
				// 定時器
				longPressTimer: null
			}
		},
		watch: {
			// 多個值之間，只要一個值發生變化，都要重新檢查check()函數
			watchChange(n) {
				this.check()
			},
			// #ifdef VUE2
			// 監聽v-mode的變化，重新初始化内部的值
			value(n) {
				if (n !== this.currentValue) {
					this.currentValue = this.format(this.value)
				}
			},
			// #endif
			// #ifdef VUE3
			// 監聽v-mode的變化，重新初始化内部的值
			modelValue: {
				handler: function (newV, oldV) {
					if (newV !== this.currentValue) {
						this.currentValue = this.format(this.modelValue)
					}
				},
				immediate: true
		}
			// #endif
		},
		computed: {
			getCursorSpacing() {
				// 判斷傳入的單位，如果為px單位，需要轉成px
				return getPx(this.cursorSpacing)
			},
			// 按鈕的樣式
			buttonStyle() {
				return (type) => {
					const style = {
						backgroundColor: this.bgColor,
						height: addUnit(this.buttonSize),
						color: this.color
					}
					if (this.isDisabled(type)) {
						style.backgroundColor = '#f7f8fa'
					}
					return style
				}
			},
			// 輸入框的樣式
			inputStyle() {
				const disabled = this.disabled || this.disabledInput
				const style = {
					color: this.color,
					backgroundColor: this.bgColor,
					height: addUnit(this.buttonSize),
					width: addUnit(this.inputWidth)
				}
				return style
			},
			// 用于監聽多個值發生變化
			watchChange() {
				return [this.integer, this.decimalLength, this.min, this.max]
			},
			isDisabled() {
				return (type) => {
					if (type === 'plus') {
						// 在點擊增加按鈕情况下，判斷整體的disabled，是否單独禁用增加按鈕，以及當前值是否大于最大的允许值
						return (
							this.disabled ||
							this.disablePlus ||
							this.currentValue >= this.max
						)
					}
					// 點擊减少按鈕同理
					return (
						this.disabled ||
						this.disableMinus ||
						this.currentValue <= this.min
					)
				}
			},
		},
		mounted() {
			this.init()
		},
		// #ifdef VUE3
		emits: ['update:modelValue', 'focus', 'blur', 'overlimit', 'change', 'plus', 'minus'],
		// #endif
		methods: {
			init() {
				// #ifdef VUE3
				this.currentValue = this.format(this.modelValue)
				// #endif
				// #ifdef VUE2
				this.currentValue = this.format(this.value)
				// #endif
			},
			// 格式化整理數據，限制範圍
			format(value) {
				value = this.filter(value)
				// 如果為空字符串，那么設置為0，同時將值轉為Number類型
				value = value === '' ? 0 : +value
				// 對比最大最小值，取在min和max之間的值
				value = Math.max(Math.min(this.max, value), this.min)
				// 如果設定了最大的小數位數，使用toFixed去进行格式化
				if (this.decimalLength !== null) {
					value = value.toFixed(this.decimalLength)
				}
				return value
			},
			// 過濾非法的字符
			filter(value) {
				// 只允许0-9之間的數字，"."為小數點，"-"為负數時候使用
				value = String(value).replace(/[^0-9.-]/g, '')
				// 如果只允许輸入整數，则過濾掉小數點后的部分
				if (this.integer && value.indexOf('.') !== -1) {
					value = value.split('.')[0]
				}
				return value;
			},
			check() {
				// 格式化了之后，如果前后的值不相等，那么設置為格式化后的值
				const val = this.format(this.currentValue);
				if (val !== this.currentValue) {
					this.currentValue = val
					this.emitChange(val)
				}
			},
			// 判斷是否出于禁止操作狀態
			// isDisabled(type) {
			// 	if (type === 'plus') {
			// 		// 在點擊增加按鈕情况下，判斷整體的disabled，是否單独禁用增加按鈕，以及當前值是否大于最大的允许值
			// 		return (
			// 			this.disabled ||
			// 			this.disablePlus ||
			// 			this.currentValue >= this.max
			// 		)
			// 	}
			// 	// 點擊减少按鈕同理
			// 	return (
			// 		this.disabled ||
			// 		this.disableMinus ||
			// 		this.currentValue <= this.min
			// 	)
			// },
			// 輸入框活動焦點
			onFocus(event) {
				this.$emit('focus', {
					...event.detail,
					name: this.name,
				})
			},
			// 輸入框失去焦點
			onBlur(event) {
				// 對輸入值进行格式化
				const value = this.format(event.detail.value)
				// 發出blur事件
				this.$emit(
					'blur',{
						...event.detail,
						name: this.name,
					}
				)
			},
			// 輸入框值發生變化
			onInput(e) {
				const {
					value = ''
				} = e.detail || {}
				// 為空返回
				if (value === '') return
				let formatted = this.filter(value)
				// 最大允许的小數長度
				if (this.decimalLength !== null && formatted.indexOf('.') !== -1) {
					const pair = formatted.split('.');
					formatted = `${pair[0]}.${pair[1].slice(0, this.decimalLength)}`
				}
				formatted = this.format(formatted)
				this.emitChange(formatted);
				// #ifdef MP-WEIXIN 
				return formatted
				// #endif 
			
			},
			// 發出change事件
			emitChange(value) {
				// 如果開啟了異步變更值，则不修改内部的值，需要用户手動在外部通過v-model變更
				if (!this.asyncChange) {
					this.$nextTick(() => {
						// #ifdef VUE3
						this.$emit('update:modelValue', value)
						// #endif
						// #ifdef VUE2
						this.$emit('input', value)
						// #endif
						this.currentValue = value
						this.$forceUpdate()
					})
				}
				this.$emit('change', {
					value,
					name: this.name,
				});
			},
			onChange() {
				const {
					type
				} = this
				if (this.isDisabled(type)) {
					return this.$emit('overlimit', type)
				}
				const diff = type === 'minus' ? -this.step : +this.step
				const value = this.format(this.add(+this.currentValue, diff))
				this.emitChange(value)
				this.$emit(type)
			},
			// 對值扩大后进行四舍五入，再除以扩大因子，避免出現浮點數操作的精度問題
			add(num1, num2) {
				const cardinal = Math.pow(10, 10);
				return Math.round((num1 + num2) * cardinal) / cardinal
			},
			// 點擊加减按鈕
			clickHandler(type) {
				this.type = type
				this.onChange()
			},
			longPressStep() {
				// 每隔一段時間，重新調用longPressStep方法，實現長按加减
				this.clearTimeout()
				this.longPressTimer = setTimeout(() => {
					this.onChange()
					this.longPressStep()
				}, 250);
			},
			onTouchStart(type) {
				if (!this.longPress) return
				this.clearTimeout()
				this.type = type
				// 一定時間后，默認达到長按狀態
				this.longPressTimer = setTimeout(() => {
					this.onChange()
					this.longPressStep()
				}, 600)
			},
			// 触摸结束，清除定時器，停止長按加减
			onTouchEnd() {
				if (!this.longPress) return
				this.clearTimeout()
			},
			// 清除定時器
			clearTimeout() {
				clearTimeout(this.longPressTimer)
				this.longPressTimer = null
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import '../../libs/css/components.scss';

	$u-numberBox-hover-bgColor: #E6E6E6 !default;
	$u-numberBox-disabled-color: #c8c9cc !default;
	$u-numberBox-disabled-bgColor: #f7f8fa !default;
	$u-numberBox-plus-radius: 4px !default;
	$u-numberBox-minus-radius: 4px !default;
	$u-numberBox-input-text-align: center !default;
	$u-numberBox-input-font-size: 15px !default;
	$u-numberBox-input-padding: 0 !default;
	$u-numberBox-input-margin: 0 2px !default;
	$u-numberBox-input-disabled-color: #c8c9cc !default;
	$u-numberBox-input-disabled-bgColor: #f2f3f5 !default;

	.u-number-box {
		@include flex(row);
		align-items: center;

		&__slot {
			/* #ifndef APP-NVUE */
			touch-action: none;
			/* #endif */
		}

		&__plus,
		&__minus {
			width: 35px;
			@include flex;
			justify-content: center;
			align-items: center;
			/* #ifndef APP-NVUE */
			touch-action: none;
			/* #endif */

			&--hover {
				background-color: $u-numberBox-hover-bgColor !important;
			}

			&--disabled {
				color: $u-numberBox-disabled-color;
				background-color: $u-numberBox-disabled-bgColor;
			}
		}

		&__plus {
			border-top-right-radius: $u-numberBox-plus-radius;
			border-bottom-right-radius: $u-numberBox-plus-radius;
		}

		&__minus {
			border-top-left-radius: $u-numberBox-minus-radius;
			border-bottom-left-radius: $u-numberBox-minus-radius;
		}

		&__input {
			position: relative;
			text-align: $u-numberBox-input-text-align;
			font-size: $u-numberBox-input-font-size;
			padding: $u-numberBox-input-padding;
			margin: $u-numberBox-input-margin;
			@include flex;
			align-items: center;
			justify-content: center;

			&--disabled {
				color: $u-numberBox-input-disabled-color;
				background-color: $u-numberBox-input-disabled-bgColor;
			}
		}
	}
</style>
