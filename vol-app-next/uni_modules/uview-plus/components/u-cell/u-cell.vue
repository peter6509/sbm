<template>
	<view class="u-cell" :class="[customClass]" :style="[addStyle(customStyle)]"
		:hover-class="(!disabled && (clickable || isLink)) ? 'u-cell--clickable' : ''" :hover-stay-time="250"
		@tap="clickHandler">
		<view class="u-cell__body" :class="[ center && 'u-cell--center', size === 'large' && 'u-cell__body--large']">
			<view class="u-cell__body__content">
				<view class="u-cell__left-icon-wrap" v-if="$slots.icon || icon">
					<slot name="icon" v-if="$slots.icon">
					</slot>
					<u-icon v-else :name="icon"
						:custom-style="iconStyle"
						:size="size === 'large' ? 22 : 18"></u-icon>
				</view>
				<view class="u-cell__title">
                    <!-- 將slot與默認内容用if/else分開主要是因為微信小程序不支持slot嵌套傳递，這樣才能解决collapse组件的slot不失效問題，label暂時未用到。 -->
					<slot name="title" v-if="$slots.title || !title">
					</slot>
                    <text v-else class="u-cell__title-text" :style="[titleTextStyle]"
                        :class="[required && 'u-cell--required', disabled && 'u-cell--disabled', size === 'large' && 'u-cell__title-text--large']">{{ title }}</text>
					<slot name="label">
						<text class="u-cell__label" v-if="label"
							:class="[disabled && 'u-cell--disabled', size === 'large' && 'u-cell__label--large']">{{ label }}</text>
					</slot>
				</view>
			</view>
			<slot name="value">
				<text class="u-cell__value"
					:class="[disabled && 'u-cell--disabled', size === 'large' && 'u-cell__value--large']"
					v-if="!testEmpty(value)">{{ value }}</text>
			</slot>
			<view class="u-cell__right-icon-wrap" v-if="$slots['right-icon'] || isLink"
				:class="[`u-cell__right-icon-wrap--${arrowDirection}`]">
				<u-icon v-if="rightIcon && !$slots['right-icon']" :name="rightIcon"
					:custom-style="rightIconStyle" :color="disabled ? '#c8c9cc' : 'info'"
					:size="size === 'large' ? 18 : 16"></u-icon>
				<slot v-else name="right-icon">
				</slot>
			</view>
			<view class="u-cell__right-icon-wrap" v-if="$slots['righticon']"
				:class="[`u-cell__right-icon-wrap--${arrowDirection}`]">
				<slot name="righticon">
				</slot>
			</view>
		</view>
		<u-line v-if="border"></u-line>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle } from '../../libs/function/index';
	import test from '../../libs/function/test';
	/**
	 * cell  單元格
	 * @description cell單元格一般用于一组列表的情况，比如個人中心頁，設置頁等。
	 * @tutorial https://uview-plus.jiangruyi.com/components/cell.html
	 * @property {String | Number}	title			標題
	 * @property {String | Number}	label			標題下方的描述信息
	 * @property {String | Number}	value			右侧的内容
	 * @property {String}			icon			左侧圖標名稱，或者圖片链接(本地文件建议使用绝對地址)
	 * @property {Boolean}			disabled		是否禁用cell	
	 * @property {Boolean}			border			是否顯示下邊框 (默認 true )
	 * @property {Boolean}			center			内容是否垂直居中(主要是针對右侧的value部分) (默認 false )
	 * @property {String}			url				點擊后跳轉的URL地址
	 * @property {String}			linkType		链接跳轉的方式，内部使用的是uView封裝的route方法，可能會进行拦截操作 (默認 'navigateTo' )
	 * @property {Boolean}			clickable		是否開啟點擊反馈(表現為點擊時加上灰色背景) （默認 false ） 
	 * @property {Boolean}			isLink			是否展示右侧箭頭並開啟點擊反馈 （默認 false ）
	 * @property {Boolean}			required		是否顯示表單狀態下的必填星號(此组件可能會内嵌入input组件) （默認 false ）
	 * @property {String}			rightIcon		右侧的圖標箭頭 （默認 'arrow-right'）
	 * @property {String}			arrowDirection	右侧箭頭的方向，可選值為：left，up，down
	 * @property {Object | String}			rightIconStyle	右侧箭頭圖標的樣式
	 * @property {Object | String}			titleStyle		標題的樣式
	 * @property {Object | String}			iconStyle		左侧圖標樣式
	 * @property {String}			size			單位元的大小，可選值為 large，normal，mini 
	 * @property {Boolean}			stop			點擊cell是否阻止事件傳播 (默認 true )
	 * @property {Object}			customStyle		定義需要用到的外部樣式
	 * 
	 * @event {Function}			click			點擊cell列表時触發
	 * @example 該组件需要搭配cell-group组件使用，見官方文檔示例
	 */
	export default {
		name: 'u-cell',
		data() {
			return {

			}
		},
		mixins: [mpMixin, mixin, props],
		computed: {
			titleTextStyle() {
				return addStyle(this.titleStyle)
			}
		},
		emits: ['click'],
		methods: {
			addStyle,
			testEmpty: test.empty,
			// 點擊cell
			clickHandler(e) {
				if (this.disabled) return
				this.$emit('click', {
					name: this.name
				})
				// 如果配置了url(此props参數通過mixin引入)参數，跳轉頁面
				this.openPage()
				// 是否阻止事件傳播
				this.stop && this.preventEvent(e)
			},
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	$u-cell-padding: 13px 15px !default;
	$u-cell-font-size: 15px !default;
	$u-cell-line-height: 24px !default;
	$u-cell-color: $u-main-color !default;
	$u-cell-icon-size: 16px !default;
	$u-cell-title-font-size: 15px !default;
	$u-cell-title-line-height: 22px !default;
	$u-cell-title-color: $u-main-color !default;
	$u-cell-label-font-size: 12px !default;
	$u-cell-label-color: $u-tips-color !default;
	$u-cell-label-line-height: 18px !default;
	$u-cell-value-font-size: 14px !default;
	$u-cell-value-color: $u-content-color !default;
	$u-cell-clickable-color: $u-bg-color !default;
	$u-cell-disabled-color: #c8c9cc !default;
	$u-cell-padding-top-large: 13px !default;
	$u-cell-padding-bottom-large: 13px !default;
	$u-cell-value-font-size-large: 15px !default;
	$u-cell-label-font-size-large: 14px !default;
	$u-cell-title-font-size-large: 16px !default;
	$u-cell-left-icon-wrap-margin-right: 4px !default;
	$u-cell-right-icon-wrap-margin-left: 4px !default;
	$u-cell-title-flex:1 !default;
	$u-cell-label-margin-top:5px !default;


	.u-cell {
		&__body {
			@include flex();
			/* #ifndef APP-NVUE */
			box-sizing: border-box;
			/* #endif */
			padding: $u-cell-padding;
			font-size: $u-cell-font-size;
			color: $u-cell-color;
			// line-height: $u-cell-line-height;
			align-items: center;

			&__content {
				@include flex(row);
				align-items: center;
				flex: 1;
			}

			&--large {
				padding-top: $u-cell-padding-top-large;
				padding-bottom: $u-cell-padding-bottom-large;
			}
		}

		&__left-icon-wrap,
		&__right-icon-wrap {
			@include flex();
			align-items: center;
			// height: $u-cell-line-height;
			font-size: $u-cell-icon-size;
		}

		&__left-icon-wrap {
			margin-right: $u-cell-left-icon-wrap-margin-right;
		}

		&__right-icon-wrap {
			margin-left: $u-cell-right-icon-wrap-margin-left;
			transition: transform 0.3s;

			&--up {
				transform: rotate(-90deg);
			}

			&--down {
				transform: rotate(90deg);
			}
		}

		&__title {
			flex: $u-cell-title-flex;
			display: flex;
			flex-direction: column;

			&-text {
				font-size: $u-cell-title-font-size;
				line-height: $u-cell-title-line-height;
				color: $u-cell-title-color;

				&--large {
					font-size: $u-cell-title-font-size-large;
				}
			}

		}

		&__label {
			margin-top: $u-cell-label-margin-top;
			font-size: $u-cell-label-font-size;
			color: $u-cell-label-color;
			line-height: $u-cell-label-line-height;

			&--large {
				font-size: $u-cell-label-font-size-large;
			}
		}

		&__value {	
			text-align: right;
			/* #ifndef APP-NVUE */
			margin-left: auto;
			/* #endif */	
			font-size: $u-cell-value-font-size;
			line-height: $u-cell-line-height;
			color: $u-cell-value-color;
			&--large {
				font-size: $u-cell-value-font-size-large;
			}
		}

		&--required {
			/* #ifndef APP-NVUE */
			overflow: visible;
			/* #endif */
			@include flex;
			align-items: center;
		}

		&--required:before {
			position: absolute;
			/* #ifndef APP-NVUE */
			content: '*';
			/* #endif */
			left: -8px;
			margin-top: 4rpx;
			font-size: 14px;
			color: $u-error;
		}

		&--clickable {
			background-color: $u-cell-clickable-color;
		}

		&--disabled {
			color: $u-cell-disabled-color;
			/* #ifndef APP-NVUE */
			cursor: not-allowed;
			/* #endif */
		}

		&--center {
			align-items: center;
		}
	}
</style>
