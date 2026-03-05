<template>
	<u-transition
	    mode="fade"
	    :show="show"
	>
		<view
		    class="u-alert"
		    :class="[`u-alert--${type}--${effect}`]"
		    @tap.stop="clickHandler"
		    :style="[addStyle(customStyle)]"
		>
			<view
			    class="u-alert__icon"
			    v-if="showIcon"
			>
				<u-icon
				    :name="iconName"
				    size="18"
				    :color="iconColor"
				></u-icon>
			</view>
			<view
			    class="u-alert__content"
			    :style="[{
					paddingRight: closable ? '20px' : 0
				}]"
			>
				<text
				    class="u-alert__content__title"
				    v-if="title"
					:style="[{
						fontSize: addUnit(fontSize),
						textAlign: center ? 'center' : 'left'
					}]"
				    :class="[effect === 'dark' ? 'u-alert__text--dark' : `u-alert__text--${type}--light`]"
				>{{ title }}</text>
				<text
				    class="u-alert__content__desc"
					v-if="description"
					:style="[{
						fontSize: addUnit(fontSize),
						textAlign: center ? 'center' : 'left'
					}]"
				    :class="[effect === 'dark' ? 'u-alert__text--dark' : `u-alert__text--${type}--light`]"
				>{{ description }}</text>
			</view>
			<view
			    class="u-alert__close"
			    v-if="closable"
			    @tap.stop="closeHandler"
			>
				<u-icon
				    name="close"
				    :color="iconColor"
				    size="15"
				></u-icon>
			</view>
		</view>
	</u-transition>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle } from '../../libs/function/index';
	/**
	 * Alert  警告提示
	 * @description 警告提示，展現需要關注的信息。
	 * @tutorial https://ijry.github.io/uview-plus/components/alertTips.html
	 * 
	 * @property {String}			title       顯示的文字 
	 * @property {String}			type        使用預設的顏色  （默認 'warning' ）
	 * @property {String}			description 辅助性文字，顏色比title浅一點，字號也小一點，可選  
	 * @property {Boolean}			closable    關闭按鈕(默認為叉號icon圖標)  （默認 false ）
	 * @property {Boolean}			showIcon    是否顯示左邊的辅助圖標   （ 默認 false ）
	 * @property {String}			effect      多圖時，圖片缩放裁剪的模式  （默認 'light' ）
	 * @property {Boolean}			center		文字是否居中  （默認 false ）
	 * @property {String | Number}	fontSize    字體大小  （默認 14 ）
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * @event    {Function}        click       點擊组件時触發
	 * @example  <u-alert :title="title"  type = "warning" :closable="closable" :description = "description"></u-alert>
	 */
	export default {
		name: 'u-alert',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				show: true
			}
		},
		computed: {
			iconColor() {
				return this.effect === 'light' ? this.type : '#fff'
			},
			// 不同主題對應不同的圖標
			iconName() {
				switch (this.type) {
					case 'success':
						return 'checkmark-circle-fill';
						break;
					case 'error':
						return 'close-circle-fill';
						break;
					case 'warning':
						return 'error-circle-fill';
						break;
					case 'info':
						return 'info-circle-fill';
						break;
					case 'primary':
						return 'more-circle-fill';
						break;
					default: 
						return 'error-circle-fill';
				}
			}
		},
		emits: ["click"],
		methods: {
			addUnit,
			addStyle,
			// 點擊内容
			clickHandler() {
				this.$emit('click')
			},
			// 點擊關闭按鈕
			closeHandler() {
				this.show = false
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-alert {
		position: relative;
		background-color: $u-primary;
		padding: 8px 10px;
		@include flex(row);
		align-items: center;
		border-top-left-radius: 4px;
		border-top-right-radius: 4px;
		border-bottom-left-radius: 4px;
		border-bottom-right-radius: 4px;

		&--primary--dark {
			background-color: $u-primary;
		}

		&--primary--light {
			background-color: #ecf5ff;
		}

		&--error--dark {
			background-color: $u-error;
		}

		&--error--light {
			background-color: #FEF0F0;
		}

		&--success--dark {
			background-color: $u-success;
		}

		&--success--light {
			background-color: #f5fff0;
		}

		&--warning--dark {
			background-color: $u-warning;
		}

		&--warning--light {
			background-color: #FDF6EC;
		}

		&--info--dark {
			background-color: $u-info;
		}

		&--info--light {
			background-color: #f4f4f5;
		}

		&__icon {
			margin-right: 5px;
		}

		&__content {
			@include flex(column);
			flex: 1;

			&__title {
				color: $u-main-color;
				font-size: 14px;
				font-weight: bold;
				color: #fff;
				margin-bottom: 2px;
			}

			&__desc {
				color: $u-main-color;
				font-size: 14px;
				flex-wrap: wrap;
				color: #fff;
			}
		}

		&__title--dark,
		&__desc--dark {
			color: #FFFFFF;
		}

		&__text--primary--light,
		&__text--primary--light {
			color: $u-primary;
		}

		&__text--success--light,
		&__text--success--light {
			color: $u-success;
		}

		&__text--warning--light,
		&__text--warning--light {
			color: $u-warning;
		}

		&__text--error--light,
		&__text--error--light {
			color: $u-error;
		}

		&__text--info--light,
		&__text--info--light {
			color: $u-info;
		}

		&__close {
			position: absolute;
			top: 11px;
			right: 10px;
		}
	}
</style>
