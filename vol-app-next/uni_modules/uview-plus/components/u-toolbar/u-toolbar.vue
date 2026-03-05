<template>
	<view
		class="u-toolbar"
		@touchmove.stop.prevent="noop"
		v-if="show"
	>
		<view
			class="u-toolbar__left"
		>
			<view
				class="u-toolbar__cancel__wrapper"
				hover-class="u-hover-class"
			>
				<text
					class="u-toolbar__wrapper__cancel"
					@tap="cancel"
					:style="{
						color: cancelColor
					}"
				>{{ cancelText }}</text>
			</view>
		</view>
		<text
			class="u-toolbar__title u-line-1"
			v-if="title"
		>{{ title }}</text>
		<view
			class="u-toolbar__right"
		>
			<view
				v-if="!rightSlot"
				class="u-toolbar__confirm__wrapper"
				hover-class="u-hover-class"
			>
				<text
					class="u-toolbar__wrapper__confirm"
					@tap="confirm"
					:style="{
					color: confirmColor
				}"
				>{{ confirmText }}</text>
			</view>
			<template v-else>
				<slot name="right">
				</slot>
			</template>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	/**
	 * Toolbar 工具條
	 * @description 
	 * @tutorial https://ijry.github.io/uview-plus/components/toolbar.html
	 * @property {Boolean}	show			是否展示工具條（默認 true ）
	 * @property {String}	cancelText		取消按鈕的文字（默認 '取消' ）
	 * @property {String}	confirmText		確認按鈕的文字（默認 '確認' ）
	 * @property {String}	cancelColor		取消按鈕的顏色（默認 '#909193' ）
	 * @property {String}	confirmColor	確認按鈕的顏色（默認 '#3c9cff' ）
	 * @property {String}	title			標題文字
	 * @event {Function} 
	 * @example 
	 */
	export default {
		name: 'u-toolbar',
		mixins: [mpMixin, mixin, props],
		emits: ["confirm", "cancel"],
		created() {
			// console.log(this.$slots)
		},
		methods: {
			// 點擊取消按鈕
			cancel() {
				this.$emit('cancel')
			},
			// 點擊確定按鈕
			confirm() {
				this.$emit('confirm')
			}
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-toolbar {
		height: 42px;
		@include flex;
		justify-content: space-between;
		align-items: center;

		&__wrapper {
			&__cancel {
				color: $u-tips-color;
				font-size: 15px;
				padding: 0 15px;
			}
		}

		&__title {
			color: $u-main-color;
			padding: 0 60rpx;
			font-size: 16px;
			flex: 1;
			text-align: center;
		}

		&__wrapper {
			&__left,
			&__right {
				@include flex;
			}
			&__confirm {
				color: $u-primary;
				font-size: 15px;
				padding: 0 15px;
			}
		}
	}
</style>
