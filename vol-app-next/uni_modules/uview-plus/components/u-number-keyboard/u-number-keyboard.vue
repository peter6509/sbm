<template>
	<view
		class="u-keyboard"
		@touchmove.stop.prevent="noop"
	>
		<view
			class="u-keyboard__button-wrapper"
			v-for="(item, index) in numList"
			:key="index"
		>
			<view
				class="u-keyboard__button-wrapper__button"
				:style="[itemStyle(index)]"
				@tap="keyboardClick(item)"
				hover-class="u-hover-class"
				:hover-stay-time="200"
			>
				<text class="u-keyboard__button-wrapper__button__text">{{ item }}</text>
			</view>
		</view>
		<view
			class="u-keyboard__button-wrapper"
		>
			<view
				class="u-keyboard__button-wrapper__button u-keyboard__button-wrapper__button--gray"
				hover-class="u-hover-class"
				:hover-stay-time="200"
				@touchstart.stop="backspaceClick"
				@touchend="clearTimer"
			>
				<u-icon
					name="backspace"
					color="#303133"
					size="28"
				></u-icon>
			</view>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { randomArray } from '../../libs/function/index';
	/**
	 * keyboard 鍵盘组件
	 * @description
	 * @tutorial
	 * @property {String}	mode		鍵盘的類型，number-數字鍵盘，card-身份证鍵盘
	 * @property {Boolean}	dotDisabled	是否顯示鍵盘的"."符號
	 * @property {Boolean}	random		是否打乱鍵盘按鍵的顺序
	 * @event {Function} change		點擊鍵盘触發
	 * @event {Function} backspace	點擊退格鍵触發
	 * @example
	 */
	export default {
		name: 'u-number-keyboard',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				backspace: 'backspace', // 退格鍵内容
				dot: '.', // 點
				timer: null, // 長按多次删除的事件監聽
				cardX: 'X' // 身份证的X符號
			};
		},
		computed: {
			// 鍵盘需要顯示的内容
			numList() {
				let tmp = [];
				if (this.dotDisabled && this.mode == 'number') {
					if (!this.random) {
						return [1, 2, 3, 4, 5, 6, 7, 8, 9, 0];
					} else {
						return randomArray([1, 2, 3, 4, 5, 6, 7, 8, 9, 0]);
					}
				} else if (!this.dotDisabled && this.mode == 'number') {
					if (!this.random) {
						return [1, 2, 3, 4, 5, 6, 7, 8, 9, this.dot, 0];
					} else {
						return randomArray([1, 2, 3, 4, 5, 6, 7, 8, 9, this.dot, 0]);
					}
				} else if (this.mode == 'card') {
					if (!this.random) {
						return [1, 2, 3, 4, 5, 6, 7, 8, 9, this.cardX, 0];
					} else {
						return randomArray([1, 2, 3, 4, 5, 6, 7, 8, 9, this.cardX, 0]);
					}
				}
			},
			// 按鍵的樣式，在非乱序&&數字鍵盘&&不顯示點按鈕時，index為9時，按鍵占位两個空間
			itemStyle() {
				return index => {
					let style = {};
					if (this.mode == 'number' && this.dotDisabled && index == 9) style.width = '464rpx';
					return style;
				};
			},
			// 是否让按鍵顯示灰色，只在非乱序&&數字鍵盘&&且允许點按鍵的時候
			btnBgGray() {
				return index => {
					if (!this.random && index == 9 && (this.mode != 'number' || (this.mode == 'number' && !this
							.dotDisabled))) return true;
					else return false;
				};
			},
		},
		created() {
		},
		emits: ["backspace", "change"],
		methods: {
			// 點擊退格鍵
			backspaceClick() {
				this.$emit('backspace');
				clearInterval(this.timer); //再次清空定時器，防止重複注册定時器
				this.timer = null;
				this.timer = setInterval(() => {
					this.$emit('backspace');
				}, 250);
			},
			clearTimer() {
				clearInterval(this.timer);
				this.timer = null;
			},
			// 獲取鍵盘顯示的内容
			keyboardClick(val) {
				// 允许鍵盘顯示點模式和触發非點按鍵時，將内容轉為數字類型
				if (!this.dotDisabled && val != this.dot && val != this.cardX) val = Number(val);
				this.$emit('change', val);
			}
		}
	};
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	$u-number-keyboard-background-color:rgb(224, 228, 230) !default;
	$u-number-keyboard-padding:8px 10rpx 8px 10rpx !default;
	$u-number-keyboard-button-width:222rpx !default;
	$u-number-keyboard-button-margin:4px 6rpx !default;
	$u-number-keyboard-button-border-top-left-radius:4px !default;
	$u-number-keyboard-button-border-top-right-radius:4px !default;
	$u-number-keyboard-button-border-bottom-left-radius:4px !default;
	$u-number-keyboard-button-border-bottom-right-radius:4px !default;
	$u-number-keyboard-button-height: 90rpx!default;
	$u-number-keyboard-button-background-color:#FFFFFF !default;
	$u-number-keyboard-button-box-shadow:0 2px 0px #BBBCBE !default;
	$u-number-keyboard-text-font-size:20px !default;
	$u-number-keyboard-text-font-weight:500 !default;
	$u-number-keyboard-text-color:$u-main-color !default;
	$u-number-keyboard-gray-background-color:rgb(200, 202, 210) !default;
	$u-number-keyboard-u-hover-class-background-color: #BBBCC6 !default;

	.u-keyboard {
		@include flex;
		flex-direction: row;
		justify-content: space-around;
		background-color: $u-number-keyboard-background-color;
		flex-wrap: wrap;
		padding: $u-number-keyboard-padding;

		&__button-wrapper {
			box-shadow: $u-number-keyboard-button-box-shadow;
			margin: $u-number-keyboard-button-margin;
			border-top-left-radius: $u-number-keyboard-button-border-top-left-radius;
			border-top-right-radius: $u-number-keyboard-button-border-top-right-radius;
			border-bottom-left-radius: $u-number-keyboard-button-border-bottom-left-radius;
			border-bottom-right-radius: $u-number-keyboard-button-border-bottom-right-radius;

			&__button {
				width: $u-number-keyboard-button-width;
				height: $u-number-keyboard-button-height;
				background-color: $u-number-keyboard-button-background-color;
				@include flex;
				justify-content: center;
				align-items: center;
				border-top-left-radius: $u-number-keyboard-button-border-top-left-radius;
				border-top-right-radius: $u-number-keyboard-button-border-top-right-radius;
				border-bottom-left-radius: $u-number-keyboard-button-border-bottom-left-radius;
				border-bottom-right-radius: $u-number-keyboard-button-border-bottom-right-radius;

				&__text {
					font-size: $u-number-keyboard-text-font-size;
					font-weight: $u-number-keyboard-text-font-weight;
					color: $u-number-keyboard-text-color;
				}

				&--gray {
					background-color: $u-number-keyboard-gray-background-color;
				}
			}
		}
	}

	.u-hover-class {
		background-color: $u-number-keyboard-u-hover-class-background-color;
	}
</style>
