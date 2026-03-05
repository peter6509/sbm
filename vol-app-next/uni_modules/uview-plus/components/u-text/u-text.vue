<template>
    <view
        class="u-text"
        :class="[customClass]"
        v-if="show"
        :style="{
            margin: margin,
			justifyContent: align === 'left' ? 'flex-start' : align === 'center' ? 'center' : 'flex-end'
        }"
        @tap="clickHandler"
    >
        <text
            :class="['u-text__price', type && `u-text__value--${type}`]"
            v-if="mode === 'price'"
            :style="[valueStyle]"
            >￥</text
        >
        <view class="u-text__prefix-icon" v-if="prefixIcon">
            <u-icon
                :name="prefixIcon"
                :customStyle="addStyle(iconStyle)"
            ></u-icon>
        </view>
        <u-link
            v-if="mode === 'link'" class="u-text__value"
            :style="{fontWeight: valueStyle.fontWeight, wordWrap: valueStyle.wordWrap, fontSize: valueStyle.fontSize}"
            :class="[type && `u-text__value--${type}`,lines && `u-line-${lines}`]" :text="value"
            :href="href"
            underLine
        ></u-link>
        <template v-else-if="openType && isMp">
            <button
                class="u-reset-button u-text__value"
                :style="[valueStyle]"
                :data-index="index"
                :openType="openType"
                @getuserinfo="onGetUserInfo"
                @contact="onContact"
                @getphonenumber="onGetPhoneNumber"
                @error="onError"
                @launchapp="onLaunchApp"
                @opensetting="onOpenSetting"
                :lang="lang"
                :session-from="sessionFrom"
                :send-message-title="sendMessageTitle"
                :send-message-path="sendMessagePath"
                :send-message-img="sendMessageImg"
                :show-message-card="showMessageCard"
                :app-parameter="appParameter"
            >
                {{ value }}
            </button>
        </template>
        <text
            v-else
            class="u-text__value"
            :style="[valueStyle]"
            :class="[
                type && `u-text__value--${type}`,
                lines && `u-line-${lines}`
            ]"
            >{{ value }}</text
        >
        <view class="u-text__suffix-icon" v-if="suffixIcon">
            <u-icon
                :name="suffixIcon"
                :customStyle="addStyle(iconStyle)"
            ></u-icon>
        </view>
    </view>
</template>

<script>
import { props } from './props'
import value from './value.js'
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { buttonMixin } from '../../libs/mixin/button';
import { openType } from '../../libs/mixin/openType';
import { addStyle, addUnit, deepMerge } from '../../libs/function/index';
/**
 * Text 文本
 * @description 此组件集成了文本類在項目中的常用功能，包括狀態，拨打電話，格式化日期，*替換，超链接...等功能。 您大可不必在使用特殊文本時自己定義，text组件几乎涵蓋您能使用的大部分场景。
 * @tutorial https://ijry.github.io/uview-plus/components/loading.html
 * @property {String} 					type		主題顏色
 * @property {Boolean} 					show		是否顯示（默認 true ）
 * @property {String | Number}			text		顯示的值
 * @property {String}					prefixIcon	前置圖標
 * @property {String} 					suffixIcon	后置圖標
 * @property {String} 					mode		文本處理的匹配模式 text-普通文本，price-價格，phone-手機號，name-姓名，date-日期，link-超链接
 * @property {String} 					href		mode=link下，配置的链接
 * @property {String | Function} 		format		格式化规则
 * @property {Boolean} 					call		mode=phone時，點擊文本是否拨打電話（默認 false ）
 * @property {String} 					openType	小程序的打開方式
 * @property {Boolean} 					bold		是否粗體，默認normal（默認 false ）
 * @property {Boolean} 					block		是否块狀（默認 false ）
 * @property {String | Number} 			lines		文本顯示的行數，如果設置，超出此行數，將會顯示省略號
 * @property {String} 					color		文本顏色（默認 '#303133' ）
 * @property {String | Number} 			size		字體大小（默認 15 ）
 * @property {Object | String} 			iconStyle	圖標的樣式 （默認 {fontSize: '15px'} ）
 * @property {String} 					decoration	文字裝饰，下划线，中划线等，可選值 none|underline|line-through（默認 'none' ）
 * @property {Object | String | Number}	margin		外邊距，對象、字符串，數值形式均可（默認 0 ）
 * @property {String | Number} 			lineHeight	文本行高
 * @property {String} 					align		文本對齊方式，可選值left|center|right（默認 'left' ）
 * @property {String} 					wordWrap	文字換行，可選值break-word|normal|anywhere（默認 'normal' ）
 * @event {Function} click  點擊触發事件
 * @example <up-text text="我用十年青春,赴你最后之约"></up-text>
 */
export default {
    name: 'up-text',
    // #ifdef MP
    mixins: [mpMixin, mixin, value, buttonMixin, openType, props],
    // #endif
    // #ifndef MP
    mixins: [mpMixin, mixin, value, props],
    // #endif
	emits: ['click'],
    computed: {
        valueStyle() {
            const style = {
                textDecoration: this.decoration,
                fontWeight: this.bold ? 'bold' : 'normal',
                wordWrap: this.wordWrap,
                fontSize: addUnit(this.size)
            }
            !this.type && (style.color = this.color)
            this.isNvue && this.lines && (style.lines = this.lines)
            this.lineHeight &&
                (style.lineHeight = addUnit(this.lineHeight))
            !this.isNvue && this.block && (style.display = 'block')
            return deepMerge(style, addStyle(this.customStyle))
        },
        isNvue() {
            let nvue = false
            // #ifdef APP-NVUE
            nvue = true
            // #endif
            return nvue
        },
        isMp() {
            let mp = false
            // #ifdef MP
            mp = true
            // #endif
            return mp
        }
    },
    data() {
        return {}
    },
    methods: {
        addStyle,
        clickHandler() {
            // 如果為手機號模式，拨打電話
            if (this.call && this.mode === 'phone') {
                uni.makePhoneCall({
                    phoneNumber: this.text
                })
            }
            this.$emit('click')
        }
    }
}
</script>

<style lang="scss" scoped>
@import '../../libs/css/components.scss';

.u-text {
    @include flex(row);
    align-items: center;
    flex-wrap: nowrap;
    flex: 1;
	/* #ifndef APP-NVUE */
	width: 100%;
	/* #endif */

    &__price {
        font-size: 14px;
        color: $u-content-color;
    }

    &__value {
        font-size: 14px;
        @include flex;
        color: $u-content-color;
        flex-wrap: wrap;
        // flex: 1;
        text-overflow: ellipsis;
        align-items: center;

        &--primary {
            color: $u-primary;
        }

        &--warning {
            color: $u-warning;
        }

        &--success {
            color: $u-success;
        }

        &--info {
            color: $u-info;
        }

        &--error {
            color: $u-error;
        }

        &--main {
            color: $u-main-color;
        }

        &--content {
            color: $u-content-color;
        }

        &--tips {
            color: $u-tips-color;
        }

        &--light {
            color: $u-light-color;
        }
    }
}
</style>
