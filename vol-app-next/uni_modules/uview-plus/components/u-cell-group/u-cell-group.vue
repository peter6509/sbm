<template>
    <view :style="[addStyle(customStyle)]" :class="[customClass]" class="u-cell-group">
        <view v-if="title" class="u-cell-group__title">
            <slot name="title">
				<text class="u-cell-group__title__text">{{ title }}</text>
			</slot>
        </view>
        <view class="u-cell-group__wrapper">
			<u-line v-if="border"></u-line>
            <slot />
        </view>
    </view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle } from '../../libs/function/index';
	/**
	 * cellGroup  單元格
	 * @description cell單元格一般用于一组列表的情况，比如個人中心頁，設置頁等。
	 * @tutorial https://uview-plus.jiangruyi.com/components/cell.html
	 * 
	 * @property {String}	title		分组標題
	 * @property {Boolean}	border		是否顯示外邊框 (默認 true )
	 * @property {Object}	customStyle	定義需要用到的外部樣式
	 * 
	 * @event {Function} click 	點擊cell列表時触發
	 * @example <u-cell-group title="設置喜好">
	 */
	export default {
		name: 'u-cell-group',
		mixins: [mpMixin, mixin, props],
		methods: {
			addStyle
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	
	$u-cell-group-title-padding: 16px 16px 8px !default;
	$u-cell-group-title-font-size: 15px !default;
	$u-cell-group-title-line-height: 16px !default;
	$u-cell-group-title-color: $u-main-color !default;

    .u-cell-group {
		flex: 1;
		
        &__title {
            padding: $u-cell-group-title-padding;

            &__text {
                font-size: $u-cell-group-title-font-size;
                line-height: $u-cell-group-title-line-height;
                color: $u-cell-group-title-color;
            }
        }
		
		&__wrapper {
			position: relative;
		}
    }
</style>

