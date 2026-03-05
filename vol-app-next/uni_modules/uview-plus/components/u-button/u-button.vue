<template>
    <!-- #ifndef APP-NVUE -->
    <button
        :hover-start-time="Number(hoverStartTime)"
        :hover-stay-time="Number(hoverStayTime)"
        :form-type="formType"
        :open-type="openType"
        :app-parameter="appParameter"
        :hover-stop-propagation="hoverStopPropagation"
        :send-message-title="sendMessageTitle"
        :send-message-path="sendMessagePath"
        :lang="lang"
        :data-name="dataName"
        :session-from="sessionFrom"
        :send-message-img="sendMessageImg"
        :show-message-card="showMessageCard"
        @getphonenumber="getphonenumber"
        @getuserinfo="getuserinfo"
        @error="error"
        @opensetting="opensetting"
        @launchapp="launchapp"
        @agreeprivacyauthorization="agreeprivacyauthorization"
        :hover-class="!disabled && !loading ? 'u-button--active' : ''"
        class="u-button u-reset-button"
        :style="[baseColor, addStyle(customStyle)]"
        @tap="clickHandler"
        :class="bemClass"
    >
        <template v-if="loading">
            <u-loading-icon
                :mode="loadingMode"
                :size="loadingSize * 1.15"
                :color="loadingColor"
            ></u-loading-icon>
            <text
                class="u-button__loading-text"
                :style="[{ fontSize: textSize + 'px' }]"
                >{{ loadingText || text }}</text
            >
        </template>
        <template v-else>
            <u-icon
                v-if="icon"
                :name="icon"
                :color="iconColorCom"
                :size="textSize * 1.35"
                :customStyle="{ marginRight: '2px' }"
            ></u-icon>
            <slot>
                <text
                    class="u-button__text"
                    :style="[{ fontSize: textSize + 'px' }]"
                    >{{ text }}</text
                >
            </slot>
        </template>
    </button>
    <!-- #endif -->

    <!-- #ifdef APP-NVUE -->
    <view
        :hover-start-time="Number(hoverStartTime)"
        :hover-stay-time="Number(hoverStayTime)"
        class="u-button"
        :hover-class="
            !disabled && !loading && !color && (plain || type === 'info')
                ? 'u-button--active--plain'
                : !disabled && !loading && !plain
                ? 'u-button--active'
                : ''
        "
        @tap="clickHandler"
        :class="bemClass"
        :style="[baseColor, addStyle(customStyle)]"
    >
        <template v-if="loading">
            <u-loading-icon
                :mode="loadingMode"
                :size="loadingSize * 1.15"
                :color="loadingColor"
            ></u-loading-icon>
            <text
                class="u-button__loading-text"
                :style="[nvueTextStyle]"
                :class="[plain && `u-button__text--plain--${type}`]"
                >{{ loadingText || text }}</text
            >
        </template>
        <template v-else>
            <u-icon
                v-if="icon"
                :name="icon"
                :color="iconColorCom"
                :size="textSize * 1.35"
            ></u-icon>
            <text
                class="u-button__text"
                :style="[
                    {
                        marginLeft: icon ? '2px' : 0,
                    },
                    nvueTextStyle,
                ]"
                :class="[plain && `u-button__text--plain--${type}`]"
                >{{ text }}</text
            >
        </template>
    </view>
    <!-- #endif -->
</template>

<script lang="ts">
import { buttonMixin } from "../../libs/mixin/button";
import { openType } from "../../libs/mixin/openType";
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { props } from "./props";
import { addStyle } from '../../libs/function/index';
import { throttle } from '../../libs/function/throttle';
import color from '../../libs/config/color';
/**
 * button 按鈕
 * @description Button 按鈕
 * @tutorial https://ijry.github.io/uview-plus/components/button.html
 *
 * @property {Boolean}			hairline				是否顯示按鈕的细邊框 (默認 true )
 * @property {String}			type					按鈕的預置樣式，info，primary，error，warning，success (默認 'info' )
 * @property {String}			size					按鈕尺寸，large，normal，mini （默認 normal）
 * @property {String}			shape					按鈕形狀，circle（两邊為半圆），square（带圆角） （默認 'square' ）
 * @property {Boolean}			plain					按鈕是否镂空，背景色透明 （默認 false）
 * @property {Boolean}			disabled				是否禁用 （默認 false）
 * @property {Boolean}			loading					按鈕名稱前是否带 loading 圖標(App-nvue 平台，在 ios 上為雪花，Android上為圆圈) （默認 false）
 * @property {String | Number}	loadingText				加載中提示文字
 * @property {String}			loadingMode				加載狀態圖標類型 （默認 'spinner' ）
 * @property {String | Number}	loadingSize				加載圖標大小 （默認 15 ）
 * @property {String}			openType				開放能力，具體請看uniapp稳定關于button组件部分說明
 * @property {String}			formType				用于 <form> 组件，點擊分别會触發 <form> 组件的 submit/reset 事件
 * @property {String}			appParameter			打開 APP 時，向 APP 傳递的参數，open-type=launchApp時有效 （注：只微信小程序、QQ小程序有效）
 * @property {Boolean}			hoverStopPropagation	指定是否阻止本節點的祖先節點出現點擊態，微信小程序有效（默認 true ）
 * @property {String}			lang					指定返回用户信息的語言，zh_CN 簡體中文，zh_TW 繁體中文，en 英文（默認 en ）
 * @property {String}			sessionFrom				會話来源，openType="contact"時有效
 * @property {String}			sendMessageTitle		會話内消息卡片標題，openType="contact"時有效
 * @property {String}			sendMessagePath			會話内消息卡片點擊跳轉小程序路徑，openType="contact"時有效
 * @property {String}			sendMessageImg			會話内消息卡片圖片，openType="contact"時有效
 * @property {Boolean}			showMessageCard			是否顯示會話内消息卡片，設置此参數為 true，用户进入客服會話會在右下角顯示"可能要發送的小程序"提示，用户點擊后可以快速發送小程序消息，openType="contact"時有效（默認false）
 * @property {String}			dataName				額外傳参参數，用于小程序的data-xxx屬性，通過target.dataset.name獲取
 * @property {String | Number}	throttleTime			节流，一定時間内只能触發一次 （默認 0 )
 * @property {String | Number}	hoverStartTime			按住后多久出現點擊態，單位毫秒 （默認 0 )
 * @property {String | Number}	hoverStayTime			手指松開后點擊態保留時間，單位毫秒 （默認 200 )
 * @property {String | Number}	text					按鈕文字，之所以通過props傳入，是因為slot傳入的話（注：nvue中無法控制文字的樣式）
 * @property {String}			icon					按鈕圖標
 * @property {String}			iconColor				按鈕圖標顏色
 * @property {String}			color					按鈕顏色，支持傳入linear-gradient渐變色
 * @property {Object}			customStyle				定義需要用到的外部樣式
 *
 * @event {Function}	click			非禁止並且非加載中，才能點擊
 * @event {Function}	getphonenumber	open-type="getPhoneNumber"時有效
 * @event {Function}	getuserinfo		用户點擊該按鈕時，會返回獲取到的用户信息，從返回参數的detail中獲取到的值同uni.getUserInfo
 * @event {Function}	error			當使用開放能力時，發生错误的回調
 * @event {Function}	opensetting		在打開授權設置頁並關闭后回調
 * @event {Function}	launchapp		打開 APP 成功的回調
 * @event {Function}	agreeprivacyauthorization	用户同意隐私协议事件回調
 * @example <u-button>月落</u-button>
 */
export default {
    name: "u-button",
    // #ifdef MP
    mixins: [mpMixin, mixin, buttonMixin, openType, props],
    // #endif
    // #ifndef MP
    mixins: [mpMixin, mixin, props],
    // #endif
    data() {
        return {};
    },
    computed: {
        // 生成bem风格的類名
        bemClass() {
            // this.bem為一個computed變量，在mixin中
            if (!this.color) {
                return this.bem(
                    "button",
                    ["type", "shape", "size"],
                    ["disabled", "plain", "hairline"]
                );
            } else {
                // 由于nvue的原因，在有color参數時，不需要傳入type，否则會生成type相關的類型，影响最终的樣式
                return this.bem(
                    "button",
                    ["shape", "size"],
                    ["disabled", "plain", "hairline"]
                );
            }
        },
        loadingColor() {
            if (this.plain) {
                // 如果有設置color值，则用color值，否则使用type主題顏色
                return this.color
                    ? this.color
                    : color[`u-${this.type}`];
            }
            if (this.type === "info") {
                return "#c9c9c9";
            }
            return "rgb(200, 200, 200)";
        },
        iconColorCom() {
            // 如果是镂空狀態，設置了color就用color值，否则使用主題顏色，
            // u-icon的color能接受一個主題顏色的值
			if (this.iconColor) return this.iconColor;
			if (this.plain) {
                return this.color ? this.color : this.type;
            } else {
                return this.type === "info" ? "#000000" : "#ffffff";
            }
        },
        baseColor() {
            let style = {};
            if (this.color) {
                // 针對自定義了color顏色的情况，镂空狀態下，就是用自定義的顏色
                style.color = this.plain ? this.color : "white";
                if (!this.plain) {
                    // 非镂空，背景色使用自定義的顏色
                    style["background-color"] = this.color;
                }
                if (this.color.indexOf("gradient") !== -1) {
                    // 如果自定義的顏色為渐變色，不顯示邊框，以及通過backgroundImage設置渐變色
                    // weex文檔說明可以写borderWidth的形式，為什么這里需要分開写？
                    // 因為weex是阿里巴巴為了部门业绩考核而做的你懂的東西，所以需要這么写才有效
                    style.borderTopWidth = 0;
                    style.borderRightWidth = 0;
                    style.borderBottomWidth = 0;
                    style.borderLeftWidth = 0;
                    if (!this.plain) {
                        style.backgroundImage = this.color;
                    }
                } else {
                    // 非渐變色，则設置邊框相關的屬性
                    style.borderColor = this.color;
                    style.borderWidth = "1px";
                    style.borderStyle = "solid";
                }
            }
            return style;
        },
        // nvue版本按鈕的字體不會继承父组件的顏色，需要對每一個text组件进行單独的設置
        nvueTextStyle() {
            let style = {};
            // 针對自定義了color顏色的情况，镂空狀態下，就是用自定義的顏色
            if (this.type === "info") {
                style.color = "#323233";
            }
            if (this.color) {
                style.color = this.plain ? this.color : "white";
            }
            style.fontSize = this.textSize + "px";
            return style;
        },
        // 字體大小
        textSize() {
            let fontSize = 14,
                { size } = this;
            if (size === "large") fontSize = 16;
            if (size === "normal") fontSize = 14;
            if (size === "small") fontSize = 12;
            if (size === "mini") fontSize = 10;
            return fontSize;
        },
    },
	emits: ['click', 'getphonenumber', 'getuserinfo',
		'error', 'opensetting', 'launchapp', 'agreeprivacyauthorization'],
    methods: {
        addStyle,
        clickHandler() {
            // 非禁止並且非加載中，才能點擊
            if (!this.disabled && !this.loading) {
				// 进行节流控制，每this.throttle毫秒内，只在開始處執行
				throttle(() => {
					this.$emit("click");
				}, this.throttleTime);
            }
        },
        // 下面為對接uniapp官方按鈕開放能力事件回調的對接
        getphonenumber(res: any) {
            this.$emit("getphonenumber", res);
        },
        getuserinfo(res: any) {
            this.$emit("getuserinfo", res);
        },
        error(res: any) {
            this.$emit("error", res);
        },
        opensetting(res: any) {
            this.$emit("opensetting", res);
        },
        launchapp(res: any) {
            this.$emit("launchapp", res);
        },
        agreeprivacyauthorization(res) {
            this.$emit("agreeprivacyauthorization", res);
        },
    },
};
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";

/* #ifndef APP-NVUE */
@import "./vue.scss";
/* #endif */

/* #ifdef APP-NVUE */
@import "./nvue.scss";
/* #endif */

$u-button-u-button-height: 40px !default;
$u-button-text-font-size: 15px !default;
$u-button-loading-text-font-size: 15px !default;
$u-button-loading-text-margin-left: 4px !default;
$u-button-large-width: 100% !default;
$u-button-large-height: 50px !default;
$u-button-normal-padding: 0 12px !default;
$u-button-large-padding: 0 15px !default;
$u-button-normal-font-size: 14px !default;
$u-button-small-min-width: 60px !default;
$u-button-small-height: 30px !default;
$u-button-small-padding: 0px 8px !default;
$u-button-mini-padding: 0px 8px !default;
$u-button-small-font-size: 12px !default;
$u-button-mini-height: 22px !default;
$u-button-mini-font-size: 10px !default;
$u-button-mini-min-width: 50px !default;
$u-button-disabled-opacity: 0.5 !default;
$u-button-info-color: #323233 !default;
$u-button-info-background-color: #fff !default;
$u-button-info-border-color: #ebedf0 !default;
$u-button-info-border-width: 1px !default;
$u-button-info-border-style: solid !default;
$u-button-success-color: #fff !default;
$u-button-success-background-color: $u-success !default;
$u-button-success-border-color: $u-button-success-background-color !default;
$u-button-success-border-width: 1px !default;
$u-button-success-border-style: solid !default;
$u-button-primary-color: #fff !default;
$u-button-primary-background-color: $u-primary !default;
$u-button-primary-border-color: $u-button-primary-background-color !default;
$u-button-primary-border-width: 1px !default;
$u-button-primary-border-style: solid !default;
$u-button-error-color: #fff !default;
$u-button-error-background-color: $u-error !default;
$u-button-error-border-color: $u-button-error-background-color !default;
$u-button-error-border-width: 1px !default;
$u-button-error-border-style: solid !default;
$u-button-warning-color: #fff !default;
$u-button-warning-background-color: $u-warning !default;
$u-button-warning-border-color: $u-button-warning-background-color !default;
$u-button-warning-border-width: 1px !default;
$u-button-warning-border-style: solid !default;
$u-button-block-width: 100% !default;
$u-button-circle-border-top-right-radius: 100px !default;
$u-button-circle-border-top-left-radius: 100px !default;
$u-button-circle-border-bottom-left-radius: 100px !default;
$u-button-circle-border-bottom-right-radius: 100px !default;
$u-button-square-border-top-right-radius: 3px !default;
$u-button-square-border-top-left-radius: 3px !default;
$u-button-square-border-bottom-left-radius: 3px !default;
$u-button-square-border-bottom-right-radius: 3px !default;
$u-button-icon-min-width: 1em !default;
$u-button-plain-background-color: #fff !default;
$u-button-hairline-border-width: 0.5px !default;

.u-button {
    height: $u-button-u-button-height;
    position: relative;
    align-items: center;
    justify-content: center;
    @include flex;
    /* #ifndef APP-NVUE */
    box-sizing: border-box;
    /* #endif */
    flex-direction: row;

    &__text {
        font-size: $u-button-text-font-size;
    }

    &__loading-text {
        font-size: $u-button-loading-text-font-size;
        margin-left: $u-button-loading-text-margin-left;
    }

    &--large {
        /* #ifndef APP-NVUE */
        width: $u-button-large-width;
        /* #endif */
        height: $u-button-large-height;
        padding: $u-button-large-padding;
    }

    &--normal {
        padding: $u-button-normal-padding;
        font-size: $u-button-normal-font-size;
    }

    &--small {
        /* #ifndef APP-NVUE */
        min-width: $u-button-small-min-width;
        /* #endif */
        height: $u-button-small-height;
        padding: $u-button-small-padding;
        font-size: $u-button-small-font-size;
    }

    &--mini {
        height: $u-button-mini-height;
        font-size: $u-button-mini-font-size;
        /* #ifndef APP-NVUE */
        min-width: $u-button-mini-min-width;
        /* #endif */
        padding: $u-button-mini-padding;
    }

    &--disabled {
        opacity: $u-button-disabled-opacity;
    }

    &--info {
        color: $u-button-info-color;
        background-color: $u-button-info-background-color;
        border-color: $u-button-info-border-color;
        border-width: $u-button-info-border-width;
        border-style: $u-button-info-border-style;
    }

    &--success {
        color: $u-button-success-color;
        background-color: $u-button-success-background-color;
        border-color: $u-button-success-border-color;
        border-width: $u-button-success-border-width;
        border-style: $u-button-success-border-style;
    }

    &--primary {
        color: $u-button-primary-color;
        background-color: $u-button-primary-background-color;
        border-color: $u-button-primary-border-color;
        border-width: $u-button-primary-border-width;
        border-style: $u-button-primary-border-style;
    }

    &--error {
        color: $u-button-error-color;
        background-color: $u-button-error-background-color;
        border-color: $u-button-error-border-color;
        border-width: $u-button-error-border-width;
        border-style: $u-button-error-border-style;
    }

    &--warning {
        color: $u-button-warning-color;
        background-color: $u-button-warning-background-color;
        border-color: $u-button-warning-border-color;
        border-width: $u-button-warning-border-width;
        border-style: $u-button-warning-border-style;
    }

    &--block {
        @include flex;
        width: $u-button-block-width;
    }

    &--circle {
        border-top-right-radius: $u-button-circle-border-top-right-radius;
        border-top-left-radius: $u-button-circle-border-top-left-radius;
        border-bottom-left-radius: $u-button-circle-border-bottom-left-radius;
        border-bottom-right-radius: $u-button-circle-border-bottom-right-radius;
    }

    &--square {
        border-bottom-left-radius: $u-button-square-border-top-right-radius;
        border-bottom-right-radius: $u-button-square-border-top-left-radius;
        border-top-left-radius: $u-button-square-border-bottom-left-radius;
        border-top-right-radius: $u-button-square-border-bottom-right-radius;
    }

    &__icon {
        /* #ifndef APP-NVUE */
        min-width: $u-button-icon-min-width;
        line-height: inherit !important;
        vertical-align: top;
        /* #endif */
    }

    &--plain {
        background-color: $u-button-plain-background-color;
    }

    &--hairline {
        border-width: $u-button-hairline-border-width !important;
    }
}
</style>
