<template>
	<view class="u-form-item" :class="{'u-form-item--error':(!!message && parentData.errorType === 'message')}">
		<view
			class="u-form-item__body"
			@tap="clickHandler"
			:style="[addStyle(customStyle), {
                flexDirection: (labelPosition || parentData.labelPosition) === 'left' ? 'row' : 'column'
			}]"
		>
			<!-- 微信小程序中，將一個参數設置空字符串，结果會變成字符串"true" -->
			<slot name="label">
				<!-- {{required}} -->
				<view
					class="u-form-item__body__left"
					v-if="required || leftIcon || label"
					:style="{
						width: addUnit(labelWidth || parentData.labelWidth),
						marginBottom: parentData.labelPosition === 'left' ? 0 : '5px',
					}"
				>
					<!-- 為了块對齊 -->
					<view class="u-form-item__body__left__content">
						<!-- nvue不支持伪元素before -->
						<text
							v-if="required"
							class="u-form-item__body__left__content__required"
						>*</text>
						<view
							class="u-form-item__body__left__content__icon"
							v-if="leftIcon"
						>
							<u-icon
								:name="leftIcon"
								:custom-style="leftIconStyle"
							></u-icon>
						</view>
						<text
							class="u-form-item__body__left__content__label"
							:style="[parentData.labelStyle, {
								justifyContent: parentData.labelAlign === 'left' ? 'flex-start' : parentData.labelAlign === 'center' ? 'center' : 'flex-end'
							}]"
						>{{ label }}</text>
					</view>
				</view>
			</slot>
			<view class="u-form-item__body__right">
				<view class="u-form-item__body__right__content">
					<view class="u-form-item__body__right__content__slot">
						<slot />
					</view>
					<view
						class="item__body__right__content__icon"
						v-if="$slots.right"
					>
						<slot name="right" />
					</view>
				</view>
			</view>
		</view>
		<slot name="error">
			<text
				v-if="!!message && parentData.errorType === 'message'"
				class="u-form-item__body__right__message"
				:style="{
					marginLeft:  addUnit(parentData.labelPosition === 'top' ? 0 : (labelWidth || parentData.labelWidth))
				}"
			>{{ message }}</text>
		</slot>
		<u-line
			v-if="borderBottom"
			:color="message && parentData.errorType === 'border-bottom' ? color.error : propsLine.color"
			:customStyle="`margin-top: ${message && parentData.errorType === 'message' ? '5px' : 0}`"
		></u-line>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import defProps from '../../libs/config/props.js'
	import color from '../../libs/config/color';
	import { addStyle, addUnit, getProperty, setProperty, error } from '../../libs/function/index';
	/**
	 * Form 表單
	 * @description 此组件一般用于表單场景，可以配置Input輸入框，Select彈出框，进行表單验证等。
	 * @tutorial https://ijry.github.io/uview-plus/components/form.html
	 * @property {String}			label			input的label提示語
	 * @property {String}			prop			绑定的值
	 * @property {Array}			rules			绑定的规则
	 * @property {String | Boolean}	borderBottom	是否顯示表單域的下划线邊框
	 * @property {String | Number}	labelWidth		label的寬度，單位px
	 * @property {String}			rightIcon		右侧圖標
	 * @property {String}			leftIcon		左侧圖標
	 * @property {String | Object} leftIconStyle    左侧圖標的樣式
	 * @property {Boolean}			required		是否顯示左邊的必填星號，只作顯示用，具體校验必填的逻輯，請在rules中配置 (默認 false )
	 *
	 * @example <u-form-item label="姓名" prop="userInfo.name" borderBottom ref="item1"></u-form-item>
	 */
	export default {
		name: 'u-form-item',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				// 错误提示語
				message: '',
				parentData: {
					// 提示文本的位置
					labelPosition: 'left',
					// 提示文本對齊方式
					labelAlign: 'left',
					// 提示文本的樣式
					labelStyle: {},
					// 提示文本的寬度
					labelWidth: 45,
					// 错误提示方式
					errorType: 'message'
				},
				color: color,
				itemRules: []
			}
		},
		// 组件創建完成時，將當前實例保存到u-form中
		computed: {
			propsLine() {
				return defProps.line
			}
		},
		mounted() {
			this.init()
		},
		emits: ["click"],
		watch: {
			// 監聽规则的變化
			rules: {
				immediate: true,
				handler(n) {
					this.setRules(n);
				},
			},
		},
		methods: {
			addStyle,
			addUnit,
			init() {
				// 父组件的實例
				this.updateParentData()
				if (!this.parent) {
					error('u-form-item需要结合u-form组件使用')
				}
			},
			// 手動設置校验的规则，如果规则中有函數的話，微信小程序中會過濾掉，所以只能手動調用設置规则
			setRules(rules) {
				// 判斷是否有规则
				if (rules.length === 0) {
					this.itemRules = [];
					return
				};
				this.itemRules = rules;
			},
			// 獲取父组件的参數
			updateParentData() {
				// 此方法写在mixin中
				this.getParentData('u-form');
			},
			// 移除u-form-item的校验结果
			clearValidate() {
				this.message = null
			},
			// 清空當前的组件的校验结果，並重置為初始值
			resetField() {
				// 找到原始值
				const value = getProperty(this.parent.originalModel, this.prop)
				// 將u-form的model的prop屬性链還原原始值
				setProperty(this.parent.model, this.prop, value)
				// 移除校验结果
				this.message = null
			},
			// 點擊组件
			clickHandler() {
				this.$emit('click')
			}
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-form-item {
		@include flex(column);
		font-size: 14px;
		color: $u-main-color;

		&__body {
			@include flex;
			padding: 10px 0;

			&__left {
				@include flex;
				align-items: center;

				&__content {
					position: relative;
					@include flex;
					align-items: center;
					padding-right: 10rpx;
					flex: 1;

					&__icon {
						margin-right: 8rpx;
					}

					&__required {
						position: absolute;
						left: -9px;
						color: $u-error;
						line-height: 20px;
						font-size: 20px;
						top: 3px;
					}

					&__label {
						@include flex;
						align-items: center;
						flex: 1;
						color: $u-main-color;
						font-size: 15px;
					}
				}
			}

			&__right {
				flex: 1;

				&__content {
					@include flex;
					align-items: center;
					flex: 1;

					&__slot {
						flex: 1;
						/* #ifndef MP */
						@include flex;
						align-items: center;
						/* #endif */
					}

					&__icon {
						margin-left: 10rpx;
						color: $u-light-color;
						font-size: 30rpx;
					}
				}

				&__message {
					font-size: 12px;
					line-height: 12px;
					color: $u-error;
				}
			}
		}
	}
</style>
