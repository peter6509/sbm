<template>
    <view
        class="u-subsection"
        ref="u-subsection"
        :class="[`u-subsection--${mode}`]"
        :style="[addStyle(customStyle), wrapperStyle]"
    >
        <view
            class="u-subsection__bar cursor-pointer"
            ref="u-subsection__bar"
            :style="[barStyle]"
            :class="[
                mode === 'button' && 'u-subsection--button__bar',
                innerCurrent === 0 &&
                    mode === 'subsection' &&
                    'u-subsection__bar--first',
                innerCurrent > 0 &&
                innerCurrent < list.length - 1 &&
                    mode === 'subsection' &&
                    'u-subsection__bar--center',
                innerCurrent === list.length - 1 &&
                    mode === 'subsection' &&
                    'u-subsection__bar--last',
            ]"
        ></view>
        <view
            class="u-subsection__item cursor-pointer"
            :class="[
                `u-subsection__item--${index}`,
                index < list.length - 1 &&
                    'u-subsection__item--no-border-right',
                index === 0 && 'u-subsection__item--first',
                index === list.length - 1 && 'u-subsection__item--last',
            ]"
            :ref="`u-subsection__item--${index}`"
            :style="[itemStyle(index)]"
            @tap="clickHandler(index)"
            v-for="(item, index) in list"
            :key="index"
        >
            <text
                class="u-subsection__item__text"
                :style="[textStyle(index)]"
                >{{ getText(item) }}</text
            >
        </view>
    </view>
</template>

<script>
// #ifdef APP-NVUE
const dom = uni.requireNativePlugin("dom");
const animation = uni.requireNativePlugin("animation");
// #endif
import { props } from "./props.js";
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { addStyle, addUnit, sleep } from '../../libs/function/index';
/**
 * Subsection 分段器
 * @description 該分段器一般用于用户從几個選項中選擇某一個的场景
 * @tutorial https://ijry.github.io/uview-plus/components/subsection.html
 * @property {Array}			list			tab的數據
 * @property {String ｜ Number}	current			當前活動的tab的index（默認 0 ）
 * @property {String}			activeColor		激活時的顏色（默認 '#3c9cff' ）
 * @property {String}			inactiveColor	未激活時的顏色（默認 '#303133' ）
 * @property {String}			mode			模式選擇，mode=button為按鈕形式，mode=subsection時為分段模式（默認 'button' ）
 * @property {String ｜ Number}	fontSize		字體大小，單位px（默認 12 ）
 * @property {Boolean}			bold			激活選項的字體是否加粗（默認 true ）
 * @property {String}			bgColor			组件背景顏色，mode為button時有效（默認 '#eeeeef' ）
 * @property {Object}			customStyle		定義需要用到的外部樣式
 * @property {String}	        keyName	        從`list`元素對象中讀取的鍵名（默認 'name' ）
 *
 * @event {Function} change		分段器選項發生改變時触發  回調 index：選項的index索引值，從0開始
 * @example <u-subsection :list="list" :current="curNow" @change="sectionChange"></u-subsection>
 */
export default {
    name: "u-subsection",
    mixins: [mpMixin, mixin, props],
    data() {
        return {
            // 组件尺寸
            itemRect: {
                width: 0,
                height: 0,
            },
            innerCurrent: '',
            windowResizeCallback: {}
        };
    },
    watch: {
        list(newValue, oldValue) {
            this.init();
        },
        current: {
            immediate: true,
            handler(n) {
                if (n !== this.innerCurrent) {
                    this.innerCurrent = n
                }
                // #ifdef APP-NVUE
                // 在安卓nvue上，如果通過translateX进行位移，到最后一個時，會导致右侧無法绘制圆角
                // 故用animation模块进行位移
                const ref = this.$refs?.["u-subsection__bar"]?.ref;
                // 不存在ref的時候(理解為第一次初始化時，需要渲染dom，进行一定延時再獲取ref)，這里的100ms是經過測試得出的结果(某些安卓需要延時久一點)，勿随意修改
                sleep(ref ? 0 : 100).then(() => {
                    animation.transition(this.$refs["u-subsection__bar"].ref, {
                        styles: {
                            transform: `translateX(${
                                n * this.itemRect.width
                            }px)`,
                            transformOrigin: "center center",
                        },
                        duration: 300,
                    });
                });
                // #endif
            },
        },
    },
    computed: {
        wrapperStyle() {
            const style = {};
            // button模式時，設置背景色
            if (this.mode === "button") {
                style.backgroundColor = this.bgColor;
            }
            return style;
        },
        // 滑块的樣式
        barStyle() {
            const style = {};
            style.width = `${this.itemRect.width}px`;
            style.height = `${this.itemRect.height}px`;
            // 通過translateX移動滑块，其移動的距离為索引*item的寬度
            // #ifndef APP-NVUE
            style.transform = `translateX(${
                this.innerCurrent * this.itemRect.width
            }px)`;
            // #endif
            if (this.mode === "subsection") {
                // 在subsection模式下，需要動態設置滑块的圆角，因為移動滑块使用的是translateX，無法通過父元素設置overflow: hidden隐藏滑块的直角
                style.backgroundColor = this.activeColor;
            }
            return style;
        },
        // 分段器item的樣式
        itemStyle(index) {
            return (index) => {
                const style = {};
                if (this.mode === "subsection") {
                    // 設置border的樣式
                    style.borderColor = this.activeColor;
                    style.borderWidth = "1px";
                    style.borderStyle = "solid";
                }
                return style;
            };
        },
        // 分段器文字顏色
        textStyle(index) {
            return (index) => {
                const style = {};
                style.fontWeight =
                    this.bold && this.innerCurrent === index ? "bold" : "normal";
                style.fontSize = addUnit(this.fontSize);
                // subsection模式下，激活時默認為白色的文字
                if (this.mode === "subsection") {
                    style.color =
                        this.innerCurrent === index ? "#fff" : this.inactiveColor;
                } else {
                    // button模式下，激活時文字顏色默認為activeColor
                    style.color =
                        this.innerCurrent === index
                            ? this.activeColor
                            : this.inactiveColor;
                }
                return style;
            };
        },
    },
    mounted() {
        this.init();
        this.windowResizeCallback = (res) => {
            this.init();
        }
        uni.onWindowResize(this.windowResizeCallback)
    },
    beforeUnmount() {
        uni.offWindowResize(this.windowResizeCallback)
    },
	emits: ["change"],
    methods: {
        addStyle,
        init() {
            this.innerCurrent = this.current
            sleep().then(() => this.getRect());
        },
		// 判斷展示文本
		getText(item) {
			return typeof item === 'object' ? item[this.keyName] : item
		},
        // 獲取组件的尺寸
        getRect() {
            // #ifndef APP-NVUE
            this.$uGetRect(".u-subsection__item--0").then((size) => {
                this.itemRect = size;
            });
            // #endif

            // #ifdef APP-NVUE
            const ref = this.$refs["u-subsection__item--0"][0];
            ref &&
                dom.getComponentRect(ref, (res) => {
                    this.itemRect = res.size;
                });
            // #endif
        },
        clickHandler(index) {
            this.innerCurrent = index
            this.$emit("change", index);
        },
    },
};
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";

.u-subsection {
    @include flex;
    position: relative;
    overflow: hidden;
	/* #ifndef APP-NVUE */
	width: 100%;
	box-sizing: border-box;
	/* #endif */

    &--button {
        height: 34px;
        background-color: rgb(238, 238, 239);
        padding: 3px;
        border-radius: 4px;
        align-items: stretch;

        &__bar {
            background-color: #ffffff;
            border-radius: 4px !important;
        }
    }

    &--subsection {
        height: 32px;
    }

    &__bar {
        position: absolute;
        /* #ifndef APP-NVUE */
        transition-property: transform, color;
        transition-duration: 0.3s;
        transition-timing-function: ease-in-out;
        /* #endif */

        &--first {
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px;
            border-top-right-radius: 0px;
            border-bottom-right-radius: 0px;
        }

        &--center {
            border-top-left-radius: 0px;
            border-bottom-left-radius: 0px;
            border-top-right-radius: 0px;
            border-bottom-right-radius: 0px;
        }

        &--last {
            border-top-left-radius: 0px;
            border-bottom-left-radius: 0px;
            border-top-right-radius: 4px;
            border-bottom-right-radius: 4px;
        }
    }

    &__item {
        @include flex;
        flex: 1;
        justify-content: center;
        align-items: center;
        // vue环境下，需要設置相對定位，因為滑块為绝對定位，item需要在滑块的上面
        position: relative;

        &--no-border-right {
            border-right-width: 0 !important;
        }

        &--first {
            border-top-left-radius: 4px;
            border-bottom-left-radius: 4px;
        }

        &--last {
            border-top-right-radius: 4px;
            border-bottom-right-radius: 4px;
        }

        &__text {
            font-size: 12px;
            line-height: 14px;
            @include flex;
            align-items: center;
            transition-property: color;
            transition-duration: 0.3s;
        }
    }
}
</style>
