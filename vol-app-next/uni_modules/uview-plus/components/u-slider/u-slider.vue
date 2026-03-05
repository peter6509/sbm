<template>
	<view
		class="u-slider"
		:style="[addStyle(customStyle)]"
	>
		<template v-if="!useNative || isRange">
			<view ref="u-slider-inner" class="u-slider-inner" @click="onClick"
				@onTouchStart="onTouchStart2($event, 1)" @touchmove="onTouchMove2($event, 1)"
				@touchend="onTouchEnd2($event, 1)" @touchcancel="onTouchEnd2($event, 1)"
				:class="[disabled ? 'u-slider--disabled' : '']" :style="{
					height: (isRange && showValue) ? (getPx(blockSize) + 24) + 'px' : (getPx(blockSize)) + 'px',
				}"
			>
				<view ref="u-slider__base"
					class="u-slider__base"
					:style="[
						{
							height: height,
							backgroundColor: inactiveColor
						}
					]"
				>
				</view>
				<view
					@click="onClick"
					class="u-slider__gap"
					:style="[
						barStyle,
						{
							height: height,
							marginTop: '-' + height,
							backgroundColor: activeColor
						}
					]"
				>
				</view>
				<view v-if="isRange"
					class="u-slider__gap u-slider__gap-0"
					:style="[
						barStyle0,
						{
							height: height,
							marginTop: '-' + height,
							backgroundColor: inactiveColor
						}
					]"
				>
				</view>
				<text v-if="isRange && showValue"
					class="u-slider__show-range-value" :style="{left: (getPx(barStyle0.width) + getPx(blockSize)/2) + 'px'}">
					{{ this.rangeValue[0] }}
				</text>
				<text v-if="isRange && showValue"
					class="u-slider__show-range-value" :style="{left: (getPx(barStyle.width) + getPx(blockSize)/2) + 'px'}">
					{{ this.rangeValue[1] }}
				</text>
				<template v-if="isRange">
					<view class="u-slider__button-wrap u-slider__button-wrap-0" @touchstart="onTouchStart($event, 0)"
						@touchmove="onTouchMove($event, 0)" @touchend="onTouchEnd($event, 0)"
						@touchcancel="onTouchEnd($event, 0)" :style="{left: (getPx(barStyle0.width) + getPx(blockSize)/2) + 'px'}">
						<slot v-if="$slots.default  || $slots.$default"/>
						<view v-else class="u-slider__button" :style="[blockStyle, {
							height: getPx(blockSize, true),
							width: getPx(blockSize, true),
							backgroundColor: blockColor
						}]"></view>
					</view>
				</template>
				<view class="u-slider__button-wrap" @touchstart="onTouchStart"
					@touchmove="onTouchMove" @touchend="onTouchEnd"
					@touchcancel="onTouchEnd" :style="{left: (getPx(barStyle.width) + getPx(blockSize)/2) + 'px'}">
					<slot v-if="$slots.default  || $slots.$default"/>
					<view v-else class="u-slider__button" :style="[blockStyle, {
						height: getPx(blockSize, true),
						width: getPx(blockSize, true),
						backgroundColor: blockColor
					}]"></view>
				</view>
			</view>
			<view class="u-slider__show-value" v-if="showValue && !isRange">{{ modelValue }}</view>
		</template>
		<slider
			class="u-slider__native"
			v-else
			:min="min"
			:max="max"
			:step="step"
			:value="modelValue"
			:activeColor="activeColor"
			:backgroundColor="inactiveColor"
			:blockSize="getPx(blockSize)"
			:blockColor="blockColor"
			:showValue="showValue"
			:disabled="disabled"
			@changing="changingHandler"
			@change="changeHandler"
		></slider>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, getPx, sleep } from '../../libs/function/index.js';
	// #ifdef APP-NVUE
	const dom = uni.requireNativePlugin('dom')
	// #endif
	/**
	 * slider 滑块選擇器
	 * @tutorial https://uview-plus.jiangruyi.com/components/slider.html
	 * @property {Number | String} value 滑块默認值（默認0）
	 * @property {Number | String} min 最小值（默認0）
	 * @property {Number | String} max 最大值（默認100）
	 * @property {Number | String} step 步長（默認1）
	 * @property {Number | String} blockWidth 滑块寬度，高等于寬（30）
	 * @property {Number | String} height 滑块條高度，單位rpx（默認6）
	 * @property {String} inactiveColor 底部條背景顏色（默認#c0c4cc）
	 * @property {String} activeColor 底部選擇部分的背景顏色（默認#2979ff）
	 * @property {String} blockColor 滑块顏色（默認#ffffff）
	 * @property {Object} blockStyle 给滑块自定義樣式，對象形式
	 * @property {Boolean} disabled 是否禁用滑块(默認為false)
	 * @event {Function} changing 正在滑動中
	 * @event {Function} change 滑動结束
	 * @example <up-slider v-model="value" />
	 */
	export default {
		name: 'u-slider',
		mixins: [mpMixin, mixin, props],
		emits: ["start", "changing", "change", "update:modelValue"],
		data() {
			return {
				startX: 0,
				status: 'end',
				newValue: 0,
				distanceX: 0,
				startValue0: 0,
				startValue: 0,
				barStyle0: {},
				barStyle: {},
				sliderRect: {
					left: 0,
					width: 0
				}
			};
		},
		watch: {
			// #ifdef VUE3
			modelValue(n) {
				// 只有在非滑動狀態時，才可以通過value更新滑块值，這里監聽，是為了让用户触發
				if(this.status == 'end') this.updateValue(this.modelValue, false);
			},
			// #endif
			// #ifdef VUE2
			value(n) {
				// 只有在非滑動狀態時，才可以通過value更新滑块值，這里監聽，是為了让用户触發
				if(this.status == 'end') this.updateValue(this.value, false);
			}
			// #endif
		},
		created() {
		},
		async mounted() {
			// 獲取滑块條的尺寸信息
			if (!this.useNative) {
				// #ifndef APP-NVUE
				this.$uGetRect('.u-slider__base').then(rect => {
					this.sliderRect = rect;
					this.init()
				});
				// #endif
				// #ifdef APP-NVUE
				await sleep(30) // 不延迟會出現size獲取都為0的問題
				const ref = this.$refs['u-slider__base']
				ref &&
					dom.getComponentRect(ref, (res) => {
						// console.log(res)
						this.sliderRect = {
							left: res.size.left,
							width: res.size.width
						};
						this.init()
					})
				// #endif
			}
		},
		methods: {
			addStyle,
			getPx,
			init() {
				if (this.isRange) {
					this.updateValue(this.rangeValue[0], false, 0);
					this.updateValue(this.rangeValue[1], false, 1);
				} else {
					// #ifdef VUE3
					this.updateValue(this.modelValue, false);
					// #endif
					// #ifdef VUE2
					this.updateValue(this.value, false);
					// #endif
				}
			},
			// native拖動過程中触發
			changingHandler(e) {
				const {
					value
				} = e.detail
				// 更新v-model的值
				// #ifdef VUE3
                this.$emit("update:modelValue", value);
                // #endif
                // #ifdef VUE2
                this.$emit("input", value);
                // #endif
				// 触發事件
				this.$emit('changing', value)
			},
			// native滑動结束時触發
			changeHandler(e) {
				const {
					value
				} = e.detail
				// 更新v-model的值
				// #ifdef VUE3
                this.$emit("update:modelValue", value);
                // #endif
                // #ifdef VUE2
                this.$emit("input", value);
                // #endif
				// 触發事件
				this.$emit('change', value);
			},
			onTouchStart(event, index = 1) {
				if (this.disabled) return;
				this.startX = 0;
				// 触摸點集
				let touches = event.touches[0];
				// 触摸點到屏幕左邊的距离
				this.startX = touches.clientX;
				// 此處的this.modelValue虽為props值，但是通過$emit('update:modelValue')进行了修改
				if (this.isRange) {
					this.startValue0 = this.format(this.rangeValue[0], 0);
					this.startValue = this.format(this.rangeValue[1], 1);
				} else {
					// #ifdef VUE3
					this.startValue = this.format(this.modelValue);
					// #endif
					// #ifdef VUE2
					this.startValue = this.format(this.value);
					// #endif
				}
				// 標示當前的狀態為開始触摸滑動
				this.status = 'start';

				let clientX = 0;
				// #ifndef APP-NVUE
				clientX = touches.clientX;
				// #endif
				// #ifdef APP-NVUE
				clientX = touches.screenX;
				// #endif
				this.distanceX = clientX - this.sliderRect.left;
				// 獲得移動距离對整個滑块的值，此為带有多位小數的值，不能用此更新视圖
				// 否则造成通信阻塞，需要每改變一個step值時修改一次视圖
				this.newValue = ((this.distanceX / this.sliderRect.width) * (this.max - this.min)) + parseFloat(this.min);
				this.status = 'moving';
				// 發出moving事件
				this.$emit('changing');
				this.updateValue(this.newValue, true, index);
			},
			onTouchMove(event, index = 1) {
				if (this.disabled) return;
				// 连续触摸的過程會一直触發本方法，但只有手指触發且移動了才被認為是拖動了，才發出事件
				// 触摸后第一次移動已經將status設置為moving狀態，故触摸第二次移動不會触發本事件
				if (this.status == 'start') this.$emit('start');
				let touches = event.touches[0];
				console.log('touchs', touches)
				// 滑块的左邊不一定跟屏幕左邊接壤，所以需要减去最外層父元素的左邊值
				let clientX = 0;
				// #ifndef APP-NVUE
				clientX = touches.clientX;
				// #endif
				// #ifdef APP-NVUE
				clientX = touches.screenX;
				// #endif
				this.distanceX = clientX - this.sliderRect.left;
				// 獲得移動距离對整個滑块的值，此為带有多位小數的值，不能用此更新视圖
				// 否则造成通信阻塞，需要每改變一個step值時修改一次视圖
				this.newValue = ((this.distanceX / this.sliderRect.width) * (this.max - this.min)) + parseFloat(this.min);
				this.status = 'moving';
				// 發出moving事件
				this.$emit('changing');
				this.updateValue(this.newValue, true, index);
			},
			onTouchEnd(event, index = 1) {
				if (this.disabled) return;
				if (this.status === 'moving') {
					this.updateValue(this.newValue, false, index);
					this.$emit('change');
				}
				this.status = 'end';
			},
			onTouchStart2(event, index = 1) {
				if (!this.isRange) {
					// this.onChangeStart(event, index);
				}
			},
			onTouchMove2(event, index = 1) {
				if (!this.isRange) {
					// this.onTouchMove(event, index);
				}
			},
			onTouchEnd2(event, index = 1) {
				if (!this.isRange) {
					// this.onTouchEnd(event, index);
				}
			},
			onClick(event) {
				// if (this.isRange) return;
				if (this.disabled) return;
				// 直接點擊滑块的情况，計算方式與onTouchMove方法相同
				// console.log('click', event)
				// #ifndef APP-NVUE
				// nvue下暂時無法獲取坐標
				let clientX = event.detail.x - this.sliderRect.left
				this.newValue = ((clientX / this.sliderRect.width) * (this.max - this.min)) + parseFloat(this.min);
				this.updateValue(this.newValue, false, 1);
				// #endif
			},
			updateValue(value, drag, index = 1) {
				// 去掉小數部分，同時也是對step步进的處理
				let valueFormat = this.format(value, index);
				// 不允许滑動的值超過max最大值
				if(valueFormat > this.max ) {
					valueFormat = this.max
				}
				// 設置移動的距离，不能用百分比，因為NVUE不支持。
				let width = Math.min((valueFormat - this.min) / (this.max - this.min) * this.sliderRect.width, this.sliderRect.width)
				let barStyle = {
					width: width + 'px'
				};
				// 移動期間無需過渡動画
				if (drag == true) {
					barStyle.transition = 'none';
				} else {
					// 非移動期間，删掉對過渡為空的声明，让css中的声明起效
					delete barStyle.transition;
				}
				// 修改value值
				if (this.isRange) {
					this.rangeValue[index] = valueFormat;
					this.$emit("update:modelValue", this.rangeValue);
				} else {
					// #ifdef VUE3
					this.$emit("update:modelValue", valueFormat);
					// #endif
					// #ifdef VUE2
					this.$emit("input", valueFormat);
					// #endif
				}

				switch (index) {
					case 0:
						this.barStyle0 = {...barStyle};
						break;
					case 1:
						this.barStyle = {...barStyle};
						break;
					default:
						break;
				}
				
			},
			format(value, index = 1) {
				// 將小數變成整數，為了减少對视圖的更新，造成视圖層與逻輯層的阻塞
				if (this.isRange) {
					switch (index) {
						case 0:
							return Math.round(
								Math.max(this.min, Math.min(value, this.rangeValue[1] - parseInt(this.step),this.max))
								/ parseInt(this.step)
							) * parseInt(this.step);
							break;
						case 1:
							return Math.round(
								Math.max(this.min, this.rangeValue[0] + parseInt(this.step), Math.min(value, this.max))
								/ parseInt(this.step)
							) * parseInt(this.step);
							break;
						default:
							break;
					}
				} else {
					return Math.round(
						Math.max(this.min, Math.min(value, this.max))
						/ parseInt(this.step)
					) * parseInt(this.step);
				}
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	.u-slider {
		position: relative;
		display: flex;
		flex-direction: row;
		align-items: center;

		&__native {
			flex: 1;
		}

		&-inner {
			flex: 1;
			display: flex;
			flex-direction: column;
			position: relative;
			border-radius: 999px;
			padding: 10px 18px;
			justify-content: center;
		}

		&__show-value {
			margin: 10px 18px 10px 0px;
		}

		&__show-range-value {
			padding-top: 2px;
			font-size: 12px;
			line-height: 12px;
			position: absolute;
    		bottom: 0;
		}

		&__base {
			background-color: #ebedf0;
		}

		/* #ifndef APP-NVUE */
		&-inner:before {
			position: absolute;
			right: 0;
			left: 0;
			content: '';
			top: -8px;
			bottom: -8px;
			z-index: -1;
		}
		/* #endif */

		&__gap {
			position: relative;
			border-radius: 999px;
			transition: width 0.2s;
			background-color: #1989fa;
		}

		&__button {
			width: 24px;
			height: 24px;
			border-radius: 50%;
			box-shadow: 0 1px 2px rgba(0, 0, 0, 0.5);
			background-color: #fff;
			transform: scale(0.9);
			/* #ifdef H5 */
			cursor: pointer;
			/* #endif */
		}

		&__button-wrap {
			position: absolute;
			// transform: translate3d(50%, -50%, 0);
		}

		&--disabled {
			opacity: 0.5;
		}
	}
</style>
