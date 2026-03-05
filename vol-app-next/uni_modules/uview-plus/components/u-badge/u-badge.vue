<template>
	<text
		v-if="show && ((Number(value) === 0 ? showZero : true) || isDot)"
		:class="[isDot ? 'u-badge--dot' : 'u-badge--not-dot', inverted && 'u-badge--inverted', shape === 'horn' && 'u-badge--horn', `u-badge--${type}${inverted ? '--inverted' : ''}`]"
		:style="[addStyle(customStyle), badgeStyle]"
		class="u-badge"
	>{{ isDot ? '' :showValue }}</text>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, addUnit } from '../../libs/function/index';
	/**
	 * badge 徽標數
	 * @description 該组件一般用于圖標右上角顯示未讀的消息數量，提示用户點擊，有圆點和圆包含文字两种形式。
	 * @tutorial https://uview-plus.jiangruyi.com/components/badge.html
	 * 
	 * @property {Boolean} 			isDot 		是否顯示圆點 （默認 false ）
	 * @property {String | Number} 	value 		顯示的内容
	 * @property {Boolean} 			show 		是否顯示 （默認 true ）
	 * @property {String | Number} 	max 		最大值，超過最大值會顯示 '{max}+'  （默認999）
	 * @property {String} 			type 		主題類型，error|warning|success|primary （默認 'error' ）
	 * @property {Boolean} 			showZero	當數值為 0 時，是否展示 Badge （默認 false ）
	 * @property {String} 			bgColor 	背景顏色，优先級比type高，如設置，type参數會失效
	 * @property {String} 			color 		字體顏色 （默認 '#ffffff' ）
	 * @property {String} 			shape 		徽標形狀，circle-四角均為圆角，horn-左下角為直角 （默認 'circle' ）
	 * @property {String} 			numberType	設置數字的顯示方式，overflow|ellipsis|limit  （默認 'overflow' ）
	 * @property {Array}} 			offset		設置badge的位置偏移，格式為 [x, y]，也即設置的為top和right的值，absolute為true時有效
	 * @property {Boolean} 			inverted	是否反轉背景和字體顏色（默認 false ）
	 * @property {Boolean} 			absolute	是否绝對定位（默認 false ）
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * @example <u-badge :type="type" :count="count"></u-badge>
	 */
	export default {
		name: 'u-badge',
		mixins: [mpMixin, props, mixin],
		computed: {
			// 是否將badge中心與父组件右上角重合
			boxStyle() {
				let style = {};
				return style;
			},
			// 整個组件的樣式
			badgeStyle() {
				const style = {}
				if(this.color) {
					style.color = this.color
				}
				if (this.bgColor && !this.inverted) {
					style.backgroundColor = this.bgColor
				}
				if (this.absolute) {
					style.position = 'absolute'
					// 如果有設置offset参數
					if(this.offset.length) {
						// top和right分為為offset的第一個和第二個值，如果没有第二個值，则right等于top
						const top = this.offset[0]
						const right = this.offset[1] || top
						style.top = addUnit(top)
						style.right = addUnit(right)
					}
				}
				return style
			},
			showValue() {
				switch (this.numberType) {
					case "overflow":
						return Number(this.value) > Number(this.max) ? this.max + "+" : this.value
						break;
					case "ellipsis":
						return Number(this.value) > Number(this.max) ? "..." : this.value
						break;
					case "limit":
						return Number(this.value) > 999 ? Number(this.value) >= 9999 ?
							Math.floor(this.value / 1e4 * 100) / 100 + "w" : Math.floor(this.value /
								1e3 * 100) / 100 + "k" : this.value
						break;
					default:
						return Number(this.value)
				}
			},
		},
		methods: {
			addStyle
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	$u-badge-primary: $u-primary !default;
	$u-badge-error: $u-error !default;
	$u-badge-success: $u-success !default;
	$u-badge-info: $u-info !default;
	$u-badge-warning: $u-warning !default;
	$u-badge-dot-radius: 100px !default;
	$u-badge-dot-size: 8px !default;
	$u-badge-dot-right: 4px !default;
	$u-badge-dot-top: 0 !default;
	$u-badge-text-font-size: 11px !default;
	$u-badge-text-right: 10px !default;
	$u-badge-text-padding: 2px 5px !default;
	$u-badge-text-align: center !default;
	$u-badge-text-color: #FFFFFF !default;

	.u-badge {
		border-top-right-radius: $u-badge-dot-radius;
		border-top-left-radius: $u-badge-dot-radius;
		border-bottom-left-radius: $u-badge-dot-radius;
		border-bottom-right-radius: $u-badge-dot-radius;
		@include flex;
		line-height: $u-badge-text-font-size;
		text-align: $u-badge-text-align;
		font-size: $u-badge-text-font-size;
		color: $u-badge-text-color;

		&--dot {
			height: $u-badge-dot-size;
			width: $u-badge-dot-size;
		}
		
		&--inverted {
			font-size: 13px;
		}
		
		&--not-dot {
			padding: $u-badge-text-padding;
		}

		&--horn {
			border-bottom-left-radius: 0;
		}

		&--primary {
			background-color: $u-badge-primary;
		}
		
		&--primary--inverted {
			color: $u-badge-primary;
		}

		&--error {
			background-color: $u-badge-error;
		}
		
		&--error--inverted {
			color: $u-badge-error;
		}

		&--success {
			background-color: $u-badge-success;
		}
		
		&--success--inverted {
			color: $u-badge-success;
		}

		&--info {
			background-color: $u-badge-info;
		}
		
		&--info--inverted {
			color: $u-badge-info;
		}

		&--warning {
			background-color: $u-badge-warning;
		}
		
		&--warning--inverted {
			color: $u-badge-warning;
		}
	}
</style>
