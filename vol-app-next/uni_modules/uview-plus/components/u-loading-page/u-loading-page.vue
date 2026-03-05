<template>
    <u-transition
        :show="loading"
        :custom-style="{
            position: 'fixed',
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            backgroundColor: bgColor,
            display: 'flex',
            zIndex: zIndex,
            ...customStyle
        }"
    >
        <view class="u-loading-page">
            <view class="u-loading-page__warpper">
                <view class="u-loading-page__warpper__loading-icon">
                    <image
                        v-if="image"
                        :src="image"
                        class="u-loading-page__warpper__loading-icon__img"
                        mode="widthFit"
						:style="{
							width: addUnit(iconSize),
						    height: addUnit(iconSize)
						}"
                    ></image>
                    <u-loading-icon
                        v-else
                        :mode="loadingMode"
                        :size="addUnit(iconSize)"
                        :color="loadingColor"
                    ></u-loading-icon>
                </view>
                <slot>
                    <text
                        class="u-loading-page__warpper__text"
                        :style="{
                            fontSize: addUnit(fontSize),
                            color: color,
                        }"
                        >{{ loadingText }}</text
                    >
                </slot>
            </view>
        </view>
    </u-transition>
</template>

<script>
import { props } from "./props";
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { addUnit } from '../../libs/function/index';
/**
 * loadingPage 加載動画
 * @description 警此组件為一個小動画，目前用在uView的loadmore加載更多和switch開關等组件的正在加載狀態场景。
 * @tutorial https://ijry.github.io/uview-plus/components/loading.html
 * @property {String | Number}	loadingText		提示内容  (默認 '正在加載' )
 * @property {String}			image			文字上方用于替換loading動画的圖片
 * @property {String}			loadingMode		加載動画的模式，circle-圆形，spinner-花朵形，semicircle-半圆形 （默認 'circle' ）
 * @property {Boolean}			loading			是否加載中 （默認 false ）
 * @property {String}			bgColor			背景色 （默認 '#ffffff' ）
 * @property {String}			color			文字顏色 （默認 '#C8C8C8' ）
 * @property {String | Number}	fontSize		文字大小 （默認 19 ）
 * @property {String | Number}	iconSize		圖標大小 （默認 28 ）
 * @property {String}			loadingColor	加載中圖標的顏色，只能rgb或者十六进制顏色值 （默認 '#C8C8C8' ）
 * @property {Number}			zIndex	        z-index層級 （默認10 ）
 * @property {Object}			customStyle		自定義樣式
 * @example <u-loading mode="circle"></u-loading>
 */
export default {
    name: "u-loading-page",
    mixins: [mpMixin, mixin, props],
    data() {
        return {};
    },
    methods: {
        addUnit
    }
};
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";

$text-color: rgb(200, 200, 200) !default;
$text-size: 19px !default;
$u-loading-icon-margin-bottom: 10px !default;

.u-loading-page {
    @include flex(column);
    flex: 1;
    align-items: center;
    justify-content: center;

    &__warpper {
        margin-top: -150px;
        justify-content: center;
        align-items: center;
        /* #ifndef APP-NVUE */
        color: $text-color;
        font-size: $text-size;
        /* #endif */
        @include flex(column);

        &__loading-icon {
            margin-bottom: $u-loading-icon-margin-bottom;

            &__img {
                width: 40px;
                height: 40px;
            }
        }

        &__text {
            font-size: $text-size;
            color: $text-color;
        }
    }
}
</style>
