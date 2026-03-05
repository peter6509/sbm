<template>
	<view
	    class="u-checkbox cursor-pointer"
	    :style="[checkboxStyle]"
	    @tap.stop="wrapperClickHandler"
	    :class="[`u-checkbox-label--${parentData.iconPlacement}`, parentData.borderBottom && parentData.placement === 'column' && 'u-border-bottom']"
	>
		<view
		    class="u-checkbox__icon-wrap cursor-pointer"
		    @tap.stop="iconClickHandler"
		    :class="iconClasses"
		    :style="[iconWrapStyle]"
		>
			<slot name="icon">
				<u-icon
				    class="u-checkbox__icon-wrap__icon"
				    name="checkbox-mark"
				    :size="elIconSize"
				    :color="elIconColor"
				/>
			</slot>
		</view>
		<view class="u-checkbox__label-wrap cursor-pointer" @tap.stop="labelClickHandler">
			<slot name="label" :label="label" :elDisabled="elDisabled">
				<text
					:style="{
						color: elDisabled ? elInactiveColor : elLabelColor,
						fontSize: elLabelSize,
						lineHeight: elLabelSize
					}"
				>{{label}}</text>
			</slot>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, addUnit, deepMerge, formValidate, error } from '../../libs/function/index';
	import test from '../../libs/function/test';
	/**
	 * checkbox  複選框
	 * @description 複選框组件一般用于需要多個選擇的场景，該组件功能完整，使用方便
	 * @tutorial https://uview-plus.jiangruyi.com/components/checkbox.html
	 * @property {String | Number | Boolean}	name			checkbox组件的標示符
	 * @property {String}						shape			形狀，square為方形，circle為圆型
	 * @property {String | Number}				size			整體的大小
	 * @property {Boolean}						checked			是否默認選中
	 * @property {String | Boolean}				disabled		是否禁用
	 * @property {String}						activeColor		選中狀態下的顏色，如設置此值，將會覆蓋parent的activeColor值
	 * @property {String}						inactiveColor	未選中的顏色
	 * @property {String | Number}				iconSize		圖標的大小，單位px
	 * @property {String}						iconColor		圖標顏色
	 * @property {String | Number}				label			label提示文字，因為nvue下，直接slot进来的文字，由于特殊的结构，無法修改樣式
	 * @property {String}						labelColor 		label的顏色
	 * @property {String | Number}				labelSize		label的字體大小，px單位
	 * @property {String | Boolean}				labelDisabled	是否禁止點擊提示語選中複選框
	 * @property {Object}						customStyle		定義需要用到的外部樣式
	 * 
	 * @event {Function}	change	任一個checkbox狀態發生變化時触發，回調為一個對象
	 * @example <u-checkbox v-model="checked" :disabled="false">天涯</u-checkbox>
	 */
	export default {
		name: "u-checkbox",
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				isChecked: false,
				// 父组件的默認值，因為頭條小程序不支持在computed中使用this.parent.shape的形式
				// 故只能使用如此方法
				parentData: {
					iconSize: 12,
					labelDisabled: null,
					disabled: null,
					shape: 'square',
					activeColor: null,
					inactiveColor: null,
					size: 18,
					// #ifdef VUE2
					value: null,
					// #endif
					// #ifdef VUE3
					modelValue: null,
					// #endif
					iconColor: null,
					placement: 'row',
					borderBottom: false,
					iconPlacement: 'left'
				}
			}
		},
		computed: {
			// 是否禁用，如果父组件u-radios-group禁用的話，將會忽略子组件的配置
			elDisabled() {
				return this.disabled !== '' ? this.disabled : this.parentData.disabled !== null ? this.parentData.disabled : false;
			},
			// 是否禁用label點擊
			elLabelDisabled() {
				return this.labelDisabled !== '' ? this.labelDisabled : this.parentData.labelDisabled !== null ? this.parentData.labelDisabled :
					false;
			},
			// 组件尺寸，對應size的值，默認值為21px
			elSize() {
				return this.size ? this.size : (this.parentData.size ? this.parentData.size : 21);
			},
			// 组件的勾選圖標的尺寸，默認12px
			elIconSize() {
				return this.iconSize ? this.iconSize : (this.parentData.iconSize ? this.parentData.iconSize : 12);
			},
			// 组件選中激活時的顏色
			elActiveColor() {
				return this.activeColor ? this.activeColor : (this.parentData.activeColor ? this.parentData.activeColor : '#2979ff');
			},
			// 组件選未中激活時的顏色
			elInactiveColor() {
				return this.inactiveColor ? this.inactiveColor : (this.parentData.inactiveColor ? this.parentData.inactiveColor :
					'#c8c9cc');
			},
			// label的顏色
			elLabelColor() {
				return this.labelColor ? this.labelColor : (this.parentData.labelColor ? this.parentData.labelColor : '#606266')
			},
			// 组件的形狀
			elShape() {
				return this.shape ? this.shape : (this.parentData.shape ? this.parentData.shape : 'circle');
			},
			// label大小
			elLabelSize() {
				return addUnit(this.labelSize ? this.labelSize : (this.parentData.labelSize ? this.parentData.labelSize :
					'15'))
			},
			elIconColor() {
				const iconColor = this.iconColor ? this.iconColor : (this.parentData.iconColor ? this.parentData.iconColor :
					'#ffffff');
				// 圖標的顏色
				if (this.elDisabled) {
					// disabled狀態下，已勾選的checkbox圖標改為elInactiveColor
					return this.isChecked ? this.elInactiveColor : 'transparent'
				} else {
					return this.isChecked ? iconColor : 'transparent'
				}
			},
			iconClasses() {
				let classes = []
				// 组件的形狀
				classes.push('u-checkbox__icon-wrap--' + this.elShape)
				if (this.elDisabled) {
					classes.push('u-checkbox__icon-wrap--disabled')
				}
				if (this.isChecked && this.elDisabled) {
					classes.push('u-checkbox__icon-wrap--disabled--checked')
				}
				// 支付宝，頭條小程序無法動態绑定一個數组類名，否则解析出来的结果會带有","，而导致失效
				// #ifdef MP-ALIPAY || MP-TOUTIAO
				classes = classes.join(' ')
				// #endif
				return classes
			},
			iconWrapStyle() {
				// checkbox的整體樣式
				const style = {}
				style.backgroundColor = this.isChecked && !this.elDisabled ? this.elActiveColor : '#ffffff'
				style.borderColor = this.isChecked && !this.elDisabled ? this.elActiveColor : this.elInactiveColor
				style.width = addUnit(this.elSize)
				style.height = addUnit(this.elSize)
				// 如果是圖標在右邊的話，移除它的右邊距
				if (!this.usedAlone) {
					if (this.parentData.iconPlacement === 'right') {
						style.marginRight = 0
					}
				}
				return style
			},
			checkboxStyle() {
				const style = {}
				if (!this.usedAlone) {
					if (this.parentData.borderBottom && this.parentData.placement === 'row') {
						error('檢测到您將borderBottom設置為true，需要同時將u-checkbox-group的placement設置為column才有效')
					}
					// 當父组件設置了顯示下邊框並且排列形式為縱向時，给内容和邊框之間加上一定間隔
					if (this.parentData.borderBottom && this.parentData.placement === 'column') {
						style.paddingBottom = '8px'
					}
				}
				return deepMerge(style, addStyle(this.customStyle))
			}
		},
		mounted() {
			this.init()
		},
		emits: ["change", "update:checked"],
		methods: {
			init() {
				if (!this.usedAlone) {
					// 支付宝小程序不支持provide/inject，所以使用這個方法獲取整個父组件，在created定義，避免循环引用
					this.updateParentData()
					if (!this.parent) {
						error('u-checkbox必須搭配u-checkbox-group组件使用')
					}
					// #ifdef VUE2
					const value = this.parentData.value
					// #endif
					// #ifdef VUE3
					const value = this.parentData.modelValue
					// #endif
					// 設置初始化時，是否默認選中的狀態，父组件u-checkbox-group的value可能是array，所以額外判斷
					if (this.checked) {
						this.isChecked = true
					} else if (!this.usedAlone && test.array(value)) {
						// 查找數组是是否存在this.name元素值
						this.isChecked = value.some(item => {
							return item === this.name
						})
					}
				} else {
					if (this.checked) {
						this.isChecked = true
					}
				}
			},
			updateParentData() {
				this.getParentData('u-checkbox-group')
			},
			// 横向两端排列時，點擊组件即可触發選中事件
			wrapperClickHandler(e) {
				if (!this.usedAlone) {
					this.parentData.iconPlacement === 'right' && this.iconClickHandler(e)
				} else {
					this.iconClickHandler(e)
				}
			},
			// 點擊圖標
			iconClickHandler(e) {
				this.preventEvent(e)
				// 如果整體被禁用，不允许被點擊
				if (!this.elDisabled) {
					this.setRadioCheckedStatus()
				}
			},
			// 點擊label
			labelClickHandler(e) {
				this.preventEvent(e)
				// 如果按鈕整體被禁用或者label被禁用，则不允许點擊文字修改狀態
				if (!this.elLabelDisabled && !this.elDisabled) {
					this.setRadioCheckedStatus()
				}
			},
			emitEvent() {
				this.$emit('change', this.isChecked)
				// 双向绑定
				if (this.usedAlone) {
					this.$emit('update:checked', this.isChecked)
				}
				// 尝试調用u-form的验证方法，进行一定延迟，否则微信小程序更新可能會不及時
				this.$nextTick(() => {
					formValidate(this, 'change')
				})
			},
			// 改變组件選中狀態
			// 這里的改變的依據是，更改本组件的checked值為true，同時通過父组件遍历所有u-checkbox實例
			// 將本组件外的其他u-checkbox的checked都設置為false(都被取消選中狀態)，因而只剩下一個為選中狀態
			setRadioCheckedStatus() {
				// 將本组件標记為與原来相反的狀態
				this.isChecked = !this.isChecked
				this.emitEvent()
				if (!this.usedAlone) {
					typeof this.parent.unCheckedOther === 'function' && this.parent.unCheckedOther(this)
				}
			}
		},
		watch:{
			checked(newValue, oldValue){
				if (newValue !== this.isChecked) {
					this.isChecked = newValue
				}
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	$u-checkbox-icon-wrap-margin-right:6px !default;
	$u-checkbox-icon-wrap-font-size:6px !default;
	$u-checkbox-icon-wrap-border-width:1px !default;
	$u-checkbox-icon-wrap-border-color:#c8c9cc !default;
	$u-checkbox-icon-wrap-icon-line-height:0 !default;
	$u-checkbox-icon-wrap-circle-border-radius:100% !default;
	$u-checkbox-icon-wrap-square-border-radius:3px !default;
	$u-checkbox-icon-wrap-checked-color:#fff !default;
	$u-checkbox-icon-wrap-checked-background-color:red !default;
	$u-checkbox-icon-wrap-checked-border-color:#2979ff !default;
	$u-checkbox-icon-wrap-disabled-background-color:#ebedf0 !default;
	$u-checkbox-icon-wrap-disabled-checked-color:#c8c9cc !default;
	$u-checkbox-label-margin-left:5px !default;
	$u-checkbox-label-margin-right:12px !default;
	$u-checkbox-label-color:$u-content-color !default;
	$u-checkbox-label-font-size:15px !default;
	$u-checkbox-label-disabled-color:#c8c9cc !default;

	.u-checkbox {
		/* #ifndef APP-NVUE */
		@include flex(row);
		/* #endif */
		overflow: hidden;
		flex-direction: row;
		align-items: center;
		margin-bottom: 5px;
		margin-top: 5px;

		&-label--left {
			flex-direction: row
		}

		&-label--right {
			flex-direction: row-reverse;
			justify-content: space-between
		}

		&__icon-wrap {
			/* #ifndef APP-NVUE */
			box-sizing: border-box;
			// nvue下，border-color過渡有問題
			transition-property: border-color, background-color, color;
			transition-duration: 0.2s;
			/* #endif */
			color: $u-content-color;
			@include flex;
			align-items: center;
			justify-content: center;
			color: transparent;
			text-align: center;
			margin-right: $u-checkbox-icon-wrap-margin-right;

			font-size: $u-checkbox-icon-wrap-font-size;
			border-width: $u-checkbox-icon-wrap-border-width;
			border-color: $u-checkbox-icon-wrap-border-color;
			border-style: solid;

			/* #ifdef MP-TOUTIAO */
			// 頭條小程序兼容性問題，需要設置行高為0，否则圖標偏下
			&__icon {
				line-height: $u-checkbox-icon-wrap-icon-line-height;
			}

			/* #endif */

			&--circle {
				border-radius: $u-checkbox-icon-wrap-circle-border-radius;
			}

			&--square {
				border-radius: $u-checkbox-icon-wrap-square-border-radius;
			}

			&--checked {
				color: $u-checkbox-icon-wrap-checked-color;
				background-color: $u-checkbox-icon-wrap-checked-background-color;
				border-color: $u-checkbox-icon-wrap-checked-border-color;
			}

			&--disabled {
				background-color: $u-checkbox-icon-wrap-disabled-background-color !important;
			}

			&--disabled--checked {
				color: $u-checkbox-icon-wrap-disabled-checked-color !important;
			}
		}

		&__label {
			/* #ifndef APP-NVUE */
			word-wrap: break-word;
			/* #endif */
			margin-left: $u-checkbox-label-margin-left;
			margin-right: $u-checkbox-label-margin-right;
			color: $u-checkbox-label-color;
			font-size: $u-checkbox-label-font-size;

			&--disabled {
				color: $u-checkbox-label-disabled-color;
			}
		}
	}
</style>
