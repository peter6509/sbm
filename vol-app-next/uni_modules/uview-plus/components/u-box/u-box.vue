<template>
	<view class="u-box" :style="[{height: height}, addStyle(customStyle)]">
        <view class="u-box__left" :style="{borderRadius: borderRadius, backgroundColor: bgColors[0]}">
            <slot name="left">左</slot>
        </view>
        <view class="u-box__gap" :style="{width: gap, height: height}"></view>
        <view class="u-box__right">
            <view class="u-box__right-top" :style="{borderRadius: borderRadius, backgroundColor: bgColors[1]}">
                <slot name="rightTop">右上</slot>
            </view>
            <view class="u-box__right-gap" :style="{height: gap}"></view>
            <view class="u-box__right-bottom" :style="{borderRadius: borderRadius, backgroundColor: bgColors[2]}">
                <slot name="rightBottom">右下</slot>
            </view>
        </view>
	</view>
</template>

<script>
	import { propsBox } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle } from '../../libs/function/index';
	import test from '../../libs/function/test';
	/**
	 * box 盒子
	 * @description box盒子一般為左邊一個盒子，右侧两個等高的半盒组成，常用于App首頁座位重點突出。
	 * @tutorial https://uview-plus.jiangruyi.com/components/box.html
	 * @property {Array}	bgColors			背景色
	 * @property {String}	height			    高度
     * @property {String}	borderRadius		圆角
	 * @property {Object}   customStyle		    定義需要用到的外部樣式
	 * 
	 * @event {Function}			click			點擊cell列表時触發
	 * @example <up-box colors=['blue', 'red', 'yellow'] height="200px"></up-box>
	 */
	export default {
		name: 'up-box',
		data() {
			return {
			}
		},
		mixins: [mpMixin, mixin, propsBox],
		computed: {
		},
		emits: [],
		methods: {
			addStyle,
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-box {
        /* #ifndef APP-NVUE */
        /* #endif */
        @include flex();
        flex: 1;

		&__left {
            @include flex();
            justify-content: center;
            align-items: center;
            flex: 1;
        }
        &__gap {
            @include flex();
            flex-direction: column;
        }
        &__right {
            @include flex();
            flex-direction: column;
            flex: 1;
        }

        &__right-top {
            @include flex();
            flex: 1;
            justify-content: center;
            align-items: center;
        }

        &__right-bottom {
            @include flex();
            flex: 1;
            justify-content: center;
            align-items: center;
        }
	}
</style>
