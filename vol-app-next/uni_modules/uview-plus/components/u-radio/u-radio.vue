<template>
	<view
	    class="u-radio cursor-pointer"
		@tap.stop="wrapperClickHandler"
	    :style="[radioStyle]"
	    :class="[`u-radio-label--${parentData.iconPlacement}`, parentData.borderBottom && parentData.placement === 'column' && 'u-border-bottom']"
	>
		<view
		    class="u-radio__icon-wrap cursor-pointer"
		    @tap.stop="iconClickHandler"
		    :class="iconClasses"
		    :style="[iconWrapStyle]"
		>
			<slot name="icon">
				<u-icon
				    class="u-radio__icon-wrap__icon"
				    name="checkbox-mark"
				    :size="elIconSize"
				    :color="elIconColor"
				/>
			</slot>
		</view>
		<text
			class="u-radio__text"
		    @tap.stop="labelClickHandler"
		    :style="{
				color: elDisabled ? elInactiveColor : elLabelColor,
				fontSize: elLabelSize,
				lineHeight: elLabelSize
			}"
		>{{label}}</text>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, os, deepMerge, formValidate, error } from '../../libs/function/index';
	/**
	 * radio 單選框
	 * @description 單選框用于有一個選擇，用户只能選擇其中一個的场景。搭配u-radio-group使用
	 * @tutorial https://ijry.github.io/uview-plus/components/radio.html
	 * @property {String | Number}	name			radio的名稱
	 * @property {String}			shape			形狀，square為方形，circle為圆型
	 * @property {Boolean}			disabled		是否禁用
	 * @property {String | Boolean}	labelDisabled	是否禁止點擊提示語選中單選框
	 * @property {String}			activeColor		選中時的顏色，如設置parent的active-color將失效
	 * @property {String}			inactiveColor	未選中的顏色
	 * @property {String | Number}	iconSize		圖標大小，單位px
	 * @property {String | Number}	labelSize		label字體大小，單位px
	 * @property {String | Number}	label			label提示文字，因為nvue下，直接slot进来的文字，由于特殊的结构，無法修改樣式
	 * @property {String | Number}	size			整體的大小
	 * @property {String}			iconColor		圖標顏色
	 * @property {String}			labelColor		label的顏色
	 * @property {Object}			customStyle		组件的樣式，對象形式
	 * 
	 * @event {Function} change 某個radio狀態發生變化時触發(選中狀態)
	 * @example <u-radio :labelDisabled="false">门掩黄昏，無計留春住</u-radio>
	 */
	export default {
		name: "u-radio",
		mixins: [mpMixin, mixin,props],
		data() {
			return {
				checked: false,
				// 當你看到這段代碼的時候，
				// 父组件的默認值，因為頭條小程序不支持在computed中使用this.parent.shape的形式
				// 故只能使用如此方法
				parentData: {
					iconSize: 12,
					labelDisabled: null,
					disabled: null,
					shape: null,
					activeColor: null,
					inactiveColor: null,
					size: 18,
					value: null,
					modelValue: null,
					iconColor: null,
					placement: 'row',
					borderBottom: false,
					iconPlacement: 'left'
				}
			}
		},
		computed: {
			// 是否禁用，如果父组件u-raios-group禁用的話，將會忽略子组件的配置
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
					// disabled狀態下，已勾選的radio圖標改為elInactiveColor
					return this.checked ? this.elInactiveColor : 'transparent'
				} else {
					return this.checked ? iconColor : 'transparent'
				}
			},
			iconClasses() {
				let classes = []
				// 组件的形狀
				classes.push('u-radio__icon-wrap--' + this.elShape)
				if (this.elDisabled) {
					classes.push('u-radio__icon-wrap--disabled')
				}
				if (this.checked && this.elDisabled) {
					classes.push('u-radio__icon-wrap--disabled--checked')
				}
				// 支付宝，頭條小程序無法動態绑定一個數组類名，否则解析出来的结果會带有","，而导致失效
				// #ifdef MP-ALIPAY || MP-TOUTIAO
				classes = classes.join(' ')
				// #endif
				return classes
			},
			iconWrapStyle() {
				// radio的整體樣式
				const style = {}
				style.backgroundColor = this.checked && !this.elDisabled ? this.elActiveColor : '#ffffff'
				style.borderColor = this.checked && !this.elDisabled ? this.elActiveColor : this.elInactiveColor
				style.width = addUnit(this.elSize)
				style.height = addUnit(this.elSize)
				// 如果是圖標在右邊的話，移除它的右邊距
				if (this.parentData.iconPlacement === 'right') {
					style.marginRight = 0
				}
				return style
			},
			radioStyle() {
				const style = {}
				if(this.parentData.borderBottom && this.parentData.placement === 'row') {
					error('檢测到您將borderBottom設置為true，需要同時將u-radio-group的placement設置為column才有效')
				}
				// 當父组件設置了顯示下邊框並且排列形式為縱向時，给内容和邊框之間加上一定間隔
				if(this.parentData.borderBottom && this.parentData.placement === 'column') {
					// ios像素密度高，需要多一點的距离
					style.paddingBottom = os() === 'ios' ? '12px' : '8px'
				}
				return deepMerge(style, addStyle(this.customStyle))
			}
		},
		mounted() {
			this.init()
		},
		emits: ["change"],
		methods: {
			init() {
				// 支付宝小程序不支持provide/inject，所以使用這個方法獲取整個父组件，在created定義，避免循环引用
				this.updateParentData()
				if (!this.parent) {
					error('u-radio必須搭配u-radio-group组件使用')
				}
				// 設置初始化時，是否默認選中的狀態
				// #ifdef VUE3
				this.checked = this.name === this.parentData.modelValue
				// #endif
        		// #ifdef VUE2
				this.checked = this.name === this.parentData.value
				// #endif
			},
			updateParentData() {
				this.getParentData('u-radio-group')
			},
			// 點擊圖標
			iconClickHandler(e) {
				this.preventEvent(e)
				// 如果整體被禁用，不允许被點擊
				if (!this.elDisabled) {
					this.setRadioCheckedStatus()
				}
			},
			// 横向两端排列時，點擊组件即可触發選中事件
			wrapperClickHandler(e) {
				this.parentData.iconPlacement === 'right' && this.iconClickHandler(e)
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
				// u-radio的checked不為true時(意味着未選中)，才發出事件，避免多次點擊触發事件
				if (!this.checked) {
					this.$emit('change', this.name)
					// 尝试調用u-form的验证方法，进行一定延迟，否则微信小程序更新可能會不及時
					this.$nextTick(() => {
						formValidate(this, 'change')
					})
				}
			},
			// 改變组件選中狀態
			// 這里的改變的依據是，更改本组件的checked值為true，同時通過父组件遍历所有u-radio實例
			// 將本组件外的其他u-radio的checked都設置為false(都被取消選中狀態)，因而只剩下一個為選中狀態
			setRadioCheckedStatus() {
				this.emitEvent()
				// 將本组件標记為選中狀態
				this.checked = true
				typeof this.parent.unCheckedOther === 'function' && this.parent.unCheckedOther(this)
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	$u-radio-wrap-margin-right:6px !default;
	$u-radio-wrap-font-size:20px !default;
	$u-radio-wrap-border-width:1px !default;
	$u-radio-wrap-border-color: #c8c9cc !default;
	$u-radio-line-height:0 !default;
	$u-radio-circle-border-radius:100% !default;
	$u-radio-square-border-radius:3px !default;
	$u-radio-checked-color:#fff !default;
	$u-radio-checked-background-color:red !default;
	$u-radio-checked-border-color: #2979ff !default;
	$u-radio-disabled-background-color:#ebedf0 !default;
	$u-radio-disabled--checked-color:#c8c9cc !default;
	$u-radio-label-margin-left: 5px !default;
	$u-radio-label-margin-right:12px !default;
	$u-radio-label-color:$u-content-color !default;
	$u-radio-label-font-size:15px !default;
	$u-radio-label-disabled-color:#c8c9cc !default;
	
	.u-radio {
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
			margin-right: $u-radio-wrap-margin-right;
			font-size: $u-radio-wrap-font-size;
			border-width: $u-radio-wrap-border-width;
			border-color: $u-radio-wrap-border-color;
			border-style: solid;

			/* #ifdef MP-TOUTIAO */
			// 頭條小程序兼容性問題，需要設置行高為0，否则圖標偏下
			&__icon {
				line-height: $u-radio-line-height;
			}

			/* #endif */

			&--circle {
				border-radius: $u-radio-circle-border-radius;
			}

			&--square {
				border-radius: $u-radio-square-border-radius;
			}

			&--checked {
				color: $u-radio-checked-color;
				background-color: $u-radio-checked-background-color;
				border-color: $u-radio-checked-border-color;
			}

			&--disabled {
				background-color: $u-radio-disabled-background-color !important;
			}

			&--disabled--checked {
				color: $u-radio-disabled--checked-color !important;
			}
		}

		&__label {
			/* #ifndef APP-NVUE */
			word-wrap: break-word;
			/* #endif */
			margin-left: $u-radio-label-margin-left;
			margin-right: $u-radio-label-margin-right;
			color: $u-radio-label-color;
			font-size: $u-radio-label-font-size;

			&--disabled {
				color: $u-radio-label-disabled-color;
			}
		}
	}
</style>
