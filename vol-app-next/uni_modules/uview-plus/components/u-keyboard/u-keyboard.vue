<template>
	<u-popup
	    :overlay="overlay"
	    :closeOnClickOverlay="closeOnClickOverlay"
	    mode="bottom"
	    :popup="false"
	    :show="show"
	    :safeAreaInsetBottom="safeAreaInsetBottom"
	    @close="popupClose"
	    :zIndex="zIndex"
	    :customStyle="{
			backgroundColor: 'rgb(214, 218, 220)'
		}"
	>
		<view class="u-keyboard">
			<slot />
			<view
			    class="u-keyboard__tooltip"
			    v-if="tooltip"
			>
				<view
				    hover-class="u-hover-class"
				    :hover-stay-time="100"
				>
					<text
					    class="u-keyboard__tooltip__item u-keyboard__tooltip__cancel"
					    v-if="showCancel"
					    @tap="onCancel"
					>{{showCancel && cancelText}}</text>
				</view>
				<view>
					<text
					    v-if="showTips"
					    class="u-keyboard__tooltip__item u-keyboard__tooltip__tips"
					>{{tips ? tips : mode == 'number' ? '數字鍵盘' : mode == 'card' ? '身份证鍵盘' : '车牌號鍵盘'}}</text>
				</view>
				<view
				    hover-class="u-hover-class"
				    :hover-stay-time="100"
				>
					<text
					    v-if="showConfirm"
					    @tap="onConfirm"
					    class="u-keyboard__tooltip__item u-keyboard__tooltip__submit"
					    hover-class="u-hover-class"
					>{{showConfirm && confirmText}}</text>
				</view>
			</view>
			<template v-if="mode == 'number' || mode == 'card'">
				<u-number-keyboard
				    :random="random"
				    @backspace="backspace"
				    @change="change"
				    :mode="mode"
				    :dotDisabled="dotDisabled"
				></u-number-keyboard>
			</template>
			<template v-else>
				<u-car-keyboard
				    :random="random"
					:autoChange="autoChange"
				    @backspace="backspace"
				    @change="change"
				></u-car-keyboard>
			</template>
		</view>
	</u-popup>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';

	/**
	 * keyboard 鍵盘
	 * @description 此為uViw自定義的鍵盘面板，内含了數字鍵盘，车牌號鍵，身份证號鍵盘3中模式，都有可以打乱按鍵顺序的選項。
	 * @tutorial https://ijry.github.io/uview-plus/components/keyboard.html
	 * @property {String}			mode				鍵盘類型，見官网基本使用的說明 （默認 'number' ）
	 * @property {Boolean}			dotDisabled			是否顯示"."按鍵，只在mode=number時有效 （默認 false ）
	 * @property {Boolean}			tooltip				是否顯示鍵盘顶部工具條 （默認 true ）
	 * @property {Boolean}			showTips			是否顯示工具條中間的提示 （默認 true ）
	 * @property {String}			tips				工具條中間的提示文字，見上方基本使用的說明，如不需要，請傳""空字符
	 * @property {Boolean}			showCancel			是否顯示工具條左邊的"取消"按鈕 （默認 true ）
	 * @property {Boolean}			showConfirm			是否顯示工具條右邊的"完成"按鈕（ 默認 true ）
	 * @property {Boolean}			random				是否打乱鍵盘按鍵的顺序 （默認 false ）
	 * @property {Boolean}			safeAreaInsetBottom	是否開啟底部安全區適配 （默認 true ）
	 * @property {Boolean}			closeOnClickOverlay	是否允许點擊遮罩收起鍵盘 （默認 true ）
	 * @property {Boolean}			show				控制鍵盘的彈出與收起（默認 false ）
	 * @property {Boolean}			overlay				是否顯示遮罩 （默認 true ）
	 * @property {String | Number}	zIndex				彈出鍵盘的z-index值 （默認 1075 ）
	 * @property {String}			cancelText			取消按鈕的文字 （默認 '取消' ）
	 * @property {String}			confirmText			確認按鈕的文字 （默認 '確認' ）
	 * @property {Object}			customStyle			自定義樣式，對象形式
	 * @event {Function} change 按鍵被點擊(不包含退格鍵被點擊)
	 * @event {Function} cancel 鍵盘顶部工具條左邊的"取消"按鈕被點擊
	 * @event {Function} confirm 鍵盘顶部工具條右邊的"完成"按鈕被點擊
	 * @event {Function} backspace 鍵盘退格鍵被點擊
	 * @example <u-keyboard mode="number" v-model="show"></u-keyboard>
	 */
	export default {
		name: "u-keyboard",
		data() {
			return {

			}
		},
		mixins: [mpMixin, mixin, props],
		emits: ["change", "close", "confirm", "cancel", "backspace"],
		methods: {
			change(e) {
				this.$emit('change', e);
			},
			// 鍵盘關闭
			popupClose() {
				this.$emit('close');
			},
			// 輸入完成
			onConfirm() {
				this.$emit('confirm');
			},
			// 取消輸入
			onCancel() {
				this.$emit('cancel');
			},
			// 退格鍵
			backspace() {
				this.$emit('backspace');
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-keyboard {

		&__tooltip {
			@include flex;
			justify-content: space-between;
			background-color: #FFFFFF;
			padding: 14px 12px;

			&__item {
				color: #333333;
				flex: 1;
				text-align: center;
				font-size: 15px;
			}

			&__submit {
				text-align: right;
				color: $u-primary;
			}

			&__cancel {
				text-align: left;
				color: #888888;
			}

			&__tips {
				color: $u-tips-color;
			}
		}
	}
</style>
