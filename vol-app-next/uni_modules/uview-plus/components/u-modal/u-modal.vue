<template>
	<u-popup
		mode="center"
		:zoom="zoom"
		:show="show"
		:class="[customClass]"
		:customStyle="{
			borderRadius: '6px', 
			overflow: 'hidden',
			marginTop: `-${addUnit(negativeTop)}`
		}"
		:closeOnClickOverlay="closeOnClickOverlay"
		:safeAreaInsetBottom="false"
		:duration="400"
		@click="clickHandler"
	>
		<view
			class="u-modal"
			:style="{
				width: addUnit(width),
			}"
		>
			<view
				class="u-modal__title"
				v-if="title"
			>{{ title }}</view>
			<view
				class="u-modal__content"
				:style="{
					paddingTop: `${title ? 12 : 25}px`
				}"
			>
				<slot>
					<text class="u-modal__content__text" :style="{textAlign: contentTextAlign}">
						{{ content }}
					</text>
				</slot>
			</view>
			<view
				class="u-modal__button-group--confirm-button"
				v-if="$slots.confirmButton"
			>
				<slot name="confirmButton"></slot>
			</view>
			<template v-else>
				<u-line></u-line>
				<view
					class="u-modal__button-group"
					:style="{
						flexDirection: buttonReverse ? 'row-reverse' : 'row'
					}"
				>
					<view
						class="u-modal__button-group__wrapper u-modal__button-group__wrapper--cancel"
						:hover-stay-time="150"
						hover-class="u-modal__button-group__wrapper--hover"
						:class="[showCancelButton && !showConfirmButton && 'u-modal__button-group__wrapper--only-cancel']"
						v-if="showCancelButton"
						@tap="cancelHandler"
					>
						<text
							class="u-modal__button-group__wrapper__text"
							:style="{
								color: cancelColor
							}"
						>{{ cancelText }}</text>
					</view>
					<u-line
						direction="column"
						v-if="showConfirmButton && showCancelButton"
					></u-line>
					<view
						class="u-modal__button-group__wrapper u-modal__button-group__wrapper--confirm"
						:hover-stay-time="150"
						hover-class="u-modal__button-group__wrapper--hover"
						:class="[!showCancelButton && showConfirmButton && 'u-modal__button-group__wrapper--only-confirm']"
						v-if="showConfirmButton"
						@tap="confirmHandler"
					>
						<u-loading-icon v-if="loading"></u-loading-icon>
						<text
							v-else
							class="u-modal__button-group__wrapper__text"
							:style="{
								color: confirmColor
							}"
						>{{ confirmText }}</text>
					</view>
				</view>
			</template>
		</view>
	</u-popup>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit } from '../../libs/function/index';
	/**
	 * Modal 模態框
	 * @description 彈出模態框，常用于消息提示、消息確認、在當前頁面内完成特定的交互操作。
	 * @tutorial https://ijry.github.io/uview-plus/components/modul.html
	 * @property {Boolean}			show				是否顯示模態框，請赋值给show （默認 false ）
	 * @property {String}			title				標題内容
	 * @property {String}			content				模態框内容，如傳入slot内容，则此参數無效
	 * @property {String}			confirmText			確認按鈕的文字 （默認 '確認' ）
	 * @property {String}			cancelText			取消按鈕的文字 （默認 '取消' ）
	 * @property {Boolean}			showConfirmButton	是否顯示確認按鈕 （默認 true ）
	 * @property {Boolean}			showCancelButton	是否顯示取消按鈕 （默認 false ）
	 * @property {String}			confirmColor		確認按鈕的顏色 （默認 '#2979ff' ）
	 * @property {String}			cancelColor			取消按鈕的顏色 （默認 '#606266' ）
	 * @property {Boolean}			buttonReverse		對調確認和取消的位置 （默認 false ）
	 * @property {Boolean}			zoom				是否開啟缩放模式 （默認 true ）
	 * @property {Boolean}			asyncClose			是否異步關闭，只對確定按鈕有效，見上方說明 （默認 false ）
	 * @property {Boolean}			closeOnClickOverlay	是否允许點擊遮罩關闭Modal （默認 false ）
	 * @property {String | Number}	negativeTop			往上偏移的值，给一個负的margin-top，往上偏移，避免和鍵盘重合的情况，單位任意，數值则默認為px單位 （默認 0 ）
	 * @property {String | Number}	width				modal寬度，不支持百分比，可以數值，px，rpx單位 （默認 '650rpx' ）
	 * @property {String}			confirmButtonShape	確認按鈕的樣式,如設置，將不會顯示取消按鈕
	 * @event {Function} confirm	點擊確認按鈕時触發
	 * @event {Function} cancel		點擊取消按鈕時触發
	 * @event {Function} close		點擊遮罩關闭出發，closeOnClickOverlay為true有效
	 * @example <u-modal :show="show" />
	 */
	export default {
		name: 'u-modal',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				loading: false
			}
		},
		watch: {
			show(n) {
				// 為了避免第一次打開modal，又使用了異步關闭的loading
				// 第二次打開modal時，loading依然存在的情况
				if (n && this.loading) this.loading = false
			}
		},
		emits: ["confirm", "cancel", "close", "update:show"],
		methods: {
			addUnit,
			// 點擊確定按鈕
			confirmHandler() {
				// 如果配置了異步關闭，將按鈕值為loading狀態
				if (this.asyncClose) {
					this.loading = true;
				} else {
					this.$emit('update:show', false)
				}
				this.$emit('confirm')
			},
			// 點擊取消按鈕
			cancelHandler() {
				this.$emit('update:show', false)
				this.$emit('cancel')
			},
			// 點擊遮罩
			// 從原理上来說，modal的遮罩點擊，並不是真的點擊到了遮罩
			// 因為modal依赖于popup的中部彈窗類型，中部彈窗比较特殊，虽有然遮罩，但是為了让彈窗内容能flex居中
			// 多了一個透明的遮罩，此透明的遮罩會覆蓋在灰色的遮罩上，所以實际上是點擊不到灰色遮罩的，popup内部在
			// 透明遮罩的子元素做了.stop處理，所以點擊内容區，也不會导致误触發
			clickHandler() {
				if (this.closeOnClickOverlay) {
					this.$emit('update:show', false)
					this.$emit('close')
				}
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	$u-modal-border-radius: 6px;

	.u-modal {
		width: 650rpx;
		border-radius: $u-modal-border-radius;
		overflow: hidden;

		&__title {
			display: flex;
			flex-direction: column;
			justify-content: center;
			align-items: center;
			font-size: 16px;
			font-weight: bold;
			color: $u-content-color;
			text-align: center;
			padding-top: 25px;
		}

		&__content {
			padding: 12px 25px 25px 25px;
			@include flex;
			justify-content: center;

			&__text {
				font-size: 15px;
				color: $u-content-color;
				flex: 1;
			}
		}

		&__button-group {
			@include flex;

			&--confirm-button {
				flex-direction: column;
				padding: 0px 25px 15px 25px;
			}

			&__wrapper {
				flex: 1;
				@include flex;
				justify-content: center;
				align-items: center;
				height: 48px;
				
				&--confirm,
				&--only-cancel {
					border-bottom-right-radius: $u-modal-border-radius;
				}
				
				&--cancel,
				&--only-confirm {
					border-bottom-left-radius: $u-modal-border-radius;
				}

				&--hover {
					background-color: $u-bg-color;
				}

				&__text {
					color: $u-content-color;
					font-size: 16px;
					text-align: center;
				}
			}
		}
	}
</style>
