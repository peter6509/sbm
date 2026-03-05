<template>
	<u-transition
		mode="fade"
		:show="show"
		style="display: inline-flex;"
	>
		<view class="u-tag-wrapper cursor-pointer">
			<view
				class="u-tag"
				:class="[`u-tag--${shape}`, !plain && `u-tag--${type}`, plain && `u-tag--${type}--plain`, `u-tag--${size}`, plain && plainFill && `u-tag--${type}--plain--fill`]"
				@tap.stop="clickHandler"
				:style="[{
					marginRight: closable ? '10px' : 0,
					marginTop: closable ? '10px' : 0,
				}, style]"
			>
				<slot name="icon">
					<view
						class="u-tag__icon"
						v-if="icon"
					>
						<image
							v-if="testImage(icon)"
							:src="icon"
							:style="[imgStyle]"
						></image>
						<u-icon
							v-else
							:color="elIconColor"
							:name="icon"
							:size="iconSize"
						></u-icon>
					</view>
				</slot>
				<text
					class="u-tag__text"
					:style="[textColor]"
					:class="[`u-tag__text--${type}`, plain && `u-tag__text--${type}--plain`, `u-tag__text--${size}`]"
				>
					<slot>
						{{ text }}
					</slot>
				</text>
			</view>
			<view
				class="u-tag__close"
				:class="[`u-tag__close--${size}`]"
				v-if="closable"
				@tap.stop="closeHandler"
				:style="{backgroundColor: closeColor}"
			>
				<u-icon
					name="close"
					:size="closeSize"
					color="#ffffff"
				></u-icon>
			</view>
		</view>
	</u-transition>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import test from '../../libs/function/test';
	/**
	 * Tag 標簽
	 * @description tag组件一般用于標记和選擇，我们提供了更加丰富的表現形式，能够较全面的涵蓋您的使用场景
	 * @tutorial https://ijry.github.io/uview-plus/components/tag.html
	 * @property {String}			type		標簽類型info、primary、success、warning、error （默認 'primary' ）
	 * @property {Boolean | String}	disabled	不可用（默認 false ）
	 * @property {String}			size		標簽的大小，large，medium，mini （默認 'medium' ）
	 * @property {String}			shape		tag的形狀，circle（两邊半圆形）, square（方形，带圆角）（默認 'square' ）
	 * @property {String | Number}	text		標簽的文字内容 
	 * @property {String}			bgColor		背景顏色，默認為空字符串，即不處理
	 * @property {String}			color		標簽字體顏色，默認為空字符串，即不處理
	 * @property {String}			borderColor	镂空形式標簽的邊框顏色
	 * @property {String}			closeColor	關闭按鈕圖標的顏色（默認 #C6C7CB）
	 * @property {String | Number}	name		點擊時返回的索引值，用于區分例遍的數组哪個元素被點擊了
	 * @property {Boolean}			plainFill	镂空時是否填充背景色（默認 false ）
	 * @property {Boolean}			plain		是否镂空（默認 false ）
	 * @property {Boolean}			closable	是否可關闭，設置為true，文字右邊會出現一個關闭圖標（默認 false ）
	 * @property {Boolean}			show		標簽顯示與否（默認 true ）
	 * @property {String}			icon		内置圖標，或绝對路徑的圖片
	 * @event {Function(index)} click 點擊標簽時触發 index: 傳递的index参數值
	 * @event {Function(index)} close closable為true時，點擊標簽關闭按鈕触發 index: 傳递的index参數值	
	 * @example <u-tag text="標簽" type="error" plain plainFill></u-tag>
	 */
	export default {
		name: 'u-tag',
		mixins: [mpMixin, mixin, props],
		data() {
			return {

			}
		},
		computed: {
			style() {
				const style = {}
				if (this.bgColor) {
					style.backgroundColor = this.bgColor
				}
				if (this.color) {
					style.color = this.color
				}
				if(this.borderColor) {
					style.borderColor = this.borderColor
				}
				return style
			},
			// nvue下，文本顏色無法继承父元素
			textColor() {
				const style = {}
				if (this.color) {
					style.color = this.color
				}
				return style
			},
			imgStyle() {
				const width = this.size === 'large' ? '17px' : this.size === 'medium' ? '15px' : '13px'
				return {
					width,
					height: width
				}
			},
			// 文本的樣式
			closeSize() {
				const size = this.size === 'large' ? 15 : this.size === 'medium' ? 13 : 12
				return size
			},
			// 圖標大小
			iconSize() {
				const size = this.size === 'large' ? 21 : this.size === 'medium' ? 19 : 16
				return size
			},
			// 圖標顏色
			elIconColor() {
				return this.iconColor ? this.iconColor : this.plain ? this.type : '#ffffff'
			}
		},
		emits: ["click", "close"],
		methods: {
			testImage: test.image,
			// 點擊關闭按鈕
			closeHandler() {
				this.$emit('close', this.name)
			},
			// 點擊標簽
			clickHandler() {
				this.$emit('click', this.name)
			}
		}
	}
</script>

<style
	lang="scss"
	scoped
>
	@import "../../libs/css/components.scss";

	.u-tag-wrapper {
		position: relative;
	}

	.u-tag {
		@include flex;
		align-items: center;
		border-style: solid;

		&--circle {
			border-radius: 100px;
		}

		&--square {
			border-radius: 3px;
		}

		&__icon {
			margin-right: 4px;
		}

		&__text {
			&--mini {
				font-size: 12px;
				line-height: 12px;
			}

			&--medium {
				font-size: 13px;
				line-height: 13px;
			}

			&--large {
				font-size: 15px;
				line-height: 15px;
			}
		}

		&--mini {
			height: 22px;
			line-height: 22px;
			padding: 0 5px;
		}

		&--medium {
			height: 26px;
			line-height: 22px;
			padding: 0 10px;
		}

		&--large {
			height: 32px;
			line-height: 32px;
			padding: 0 15px;
		}

		&--primary {
			background-color: $u-primary;
			border-width: 1px;
			border-color: $u-primary;
		}

		&--primary--plain {
			border-width: 1px;
			border-color: $u-primary;
		}

		&--primary--plain--fill {
			background-color: #ecf5ff;
		}

		&__text--primary {
			color: #FFFFFF;
		}

		&__text--primary--plain {
			color: $u-primary;
		}

		&--error {
			background-color: $u-error;
			border-width: 1px;
			border-color: $u-error;
		}

		&--error--plain {
			border-width: 1px;
			border-color: $u-error;
		}

		&--error--plain--fill {
			background-color: #fef0f0;
		}

		&__text--error {
			color: #FFFFFF;
		}

		&__text--error--plain {
			color: $u-error;
		}

		&--warning {
			background-color: $u-warning;
			border-width: 1px;
			border-color: $u-warning;
		}

		&--warning--plain {
			border-width: 1px;
			border-color: $u-warning;
		}

		&--warning--plain--fill {
			background-color: #fdf6ec;
		}

		&__text--warning {
			color: #FFFFFF;
		}

		&__text--warning--plain {
			color: $u-warning;
		}

		&--success {
			background-color: $u-success;
			border-width: 1px;
			border-color: $u-success;
		}

		&--success--plain {
			border-width: 1px;
			border-color: $u-success;
		}

		&--success--plain--fill {
			background-color: #f5fff0;
		}

		&__text--success {
			color: #FFFFFF;
		}

		&__text--success--plain {
			color: $u-success;
		}

		&--info {
			background-color: $u-info;
			border-width: 1px;
			border-color: $u-info;
		}

		&--info--plain {
			border-width: 1px;
			border-color: $u-info;
		}

		&--info--plain--fill {
			background-color: #f4f4f5;
		}

		&__text--info {
			color: #FFFFFF;
		}

		&__text--info--plain {
			color: $u-info;
		}

		&__close {
			position: absolute;
			z-index: 999;
			top: 10px;
			right: 10px;
			border-radius: 100px;
			background-color: #C6C7CB;
			@include flex(row);
			align-items: center;
			justify-content: center;
			/* #ifndef APP-NVUE */
			transform: scale(0.6) translate(80%, -80%);
			/* #endif */
			/* #ifdef APP-NVUE */
			transform: scale(0.6) translate(50%, -50%);
			/* #endif */

			&--mini {
				width: 18px;
				height: 18px;
			}

			&--medium {
				width: 22px;
				height: 22px;
			}

			&--large {
				width: 25px;
				height: 25px;
			}
		}

	}
</style>
