<template>
    <view class="u-input" :class="inputClass" :style="[wrapperStyle]">
        <view class="u-input__content">
            <view
                class="u-input__content__prefix-icon"
                v-if="prefixIcon || $slots.prefix"
            >
                <slot name="prefix">
                    <u-icon
                        :name="prefixIcon"
                        size="18"
                        :customStyle="prefixIconStyle"
                    ></u-icon>
                </slot>
            </view>
            <view class="u-input__content__field-wrapper" @tap="clickHandler">
				<!-- 根據uni-app的input组件文檔，H5和APP中只要声明了password参數(無论true還是false)，type均失效，此時
					為了防止type=number時，又存在password屬性，type無效，此時需要設置password為undefined
				 -->
            	<input
                    ref="input-native"
            	    class="u-input__content__field-wrapper__field"
            	    :style="[inputStyle]"
            	    :type="type"
            	    :focus="focus"
            	    :cursor="cursor"
            	    :value="innerValue"
            	    :auto-blur="autoBlur"
            	    :disabled="disabled || readonly"
            	    :maxlength="maxlength"
            	    :placeholder="placeholder"
            	    :placeholder-style="placeholderStyle"
            	    :placeholder-class="placeholderClass"
            	    :confirm-type="confirmType"
            	    :confirm-hold="confirmHold"
            	    :hold-keyboard="holdKeyboard"
            	    :cursor-spacing="cursorSpacing"
            	    :adjust-position="adjustPosition"
            	    :selection-end="selectionEnd"
            	    :selection-start="selectionStart"
            	    :password="password || type === 'password' || false"
                    :ignoreCompositionEvent="ignoreCompositionEvent"
            	    @input="onInput"
            	    @blur="onBlur"
            	    @focus="onFocus"
            	    @confirm="onConfirm"
            	    @keyboardheightchange="onkeyboardheightchange"
                    @nicknamereview="onnicknamereview"
            	/>
            </view>
            <view
                class="u-input__content__clear"
                v-if="isShowClear"
                @click="onClear"
            >
                <u-icon
                    name="close"
                    size="11"
                    color="#ffffff"
                    customStyle="line-height: 12px"
                ></u-icon>
            </view>
            <view
                class="u-input__content__subfix-icon"
                v-if="suffixIcon || $slots.suffix"
            >
                <slot name="suffix">
                    <u-icon
                        :name="suffixIcon"
                        size="18"
                        :customStyle="suffixIconStyle"
                    ></u-icon>
                </slot>
            </view>
        </view>
    </view>
</template>

<script>
import { props } from "./props.js";
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { debounce } from '../../libs/function/debounce';
import { addStyle, addUnit, deepMerge, formValidate, $parent, sleep, os } from '../../libs/function/index';
/**
 * Input 輸入框
 * @description  此组件為一個輸入框，默認没有邊框和樣式，是专门為配合表單组件u-form而設計的，利用它可以快速實現表單验证，輸入内容，下拉選擇等功能。
 * @tutorial https://uview-plus.jiangruyi.com/components/input.html
 * @property {String | Number}	value					輸入的值
 * @property {String}			type					輸入框類型，見上方說明 （ 默認 'text' ）
 * @property {Boolean}			fixed					如果 textarea 是在一個 position:fixed 的區域，需要顯示指定屬性 fixed 為 true，兼容性：微信小程序、百度小程序、字节跳動小程序、QQ小程序 （ 默認 false ）
 * @property {Boolean}			disabled				是否禁用輸入框 （ 默認 false ）
 * @property {String}			disabledColor			禁用狀態時的背景色（ 默認 '#f5f7fa' ）
 * @property {Boolean}			clearable				是否顯示清除控件 （ 默認 false ）
 * @property {Boolean}			password				是否密碼類型 （ 默認 false ）
 * @property {Number}       	maxlength				最大輸入長度，設置為 -1 的時候不限制最大長度 （ 默認 -1 ）
 * @property {String}			placeholder				輸入框為空時的占位符
 * @property {String}			placeholderClass		指定placeholder的樣式類，注意頁面或组件的style中写了scoped時，需要在類名前写/deep/ （ 默認 'input-placeholder' ）
 * @property {String | Object}	placeholderStyle		指定placeholder的樣式，字符串/對象形式，如"color: red;"
 * @property {Boolean}			showWordLimit			是否顯示輸入字數统計，只在 type ="text"或type ="textarea"時有效 （ 默認 false ）
 * @property {String}			confirmType				設置右下角按鈕的文字，兼容性詳見uni-app文檔 （ 默認 'done' ）
 * @property {Boolean}			confirmHold				點擊鍵盘右下角按鈕時是否保持鍵盘不收起，H5無效 （ 默認 false ）
 * @property {Boolean}			holdKeyboard			focus時，點擊頁面的時候不收起鍵盘，微信小程序有效 （ 默認 false ）
 * @property {Boolean}			focus					自動獲取焦點，在 H5 平台能否聚焦以及软鍵盘是否跟随彈出，取决于當前浏覽器本身的實現。nvue 頁面不支持，需使用组件的 focus()、blur() 方法控制焦點 （ 默認 false ）
 * @property {Boolean}			autoBlur				鍵盘收起時，是否自動失去焦點，目前仅App3.0.0+有效 （ 默認 false ）
 * @property {Boolean}			disableDefaultPadding	是否去掉 iOS 下的默認内邊距，仅微信小程序，且type=textarea時有效 （ 默認 false ）
 * @property {String ｜ Number}	cursor					指定focus時光標的位置（ 默認 140 ）
 * @property {String ｜ Number}	cursorSpacing			輸入框聚焦時底部與鍵盘的距离 （ 默認 30 ）
 * @property {String ｜ Number}	selectionStart			光標起始位置，自動聚集時有效，需與selection-end搭配使用 （ 默認 -1 ）
 * @property {String ｜ Number}	selectionEnd			光標结束位置，自動聚集時有效，需與selection-start搭配使用 （ 默認 -1 ）
 * @property {Boolean}			adjustPosition			鍵盘彈起時，是否自動上推頁面 （ 默認 true ）
 * @property {String}			inputAlign				輸入框内容對齊方式（ 默認 'left' ）
 * @property {String | Number}	fontSize				輸入框字體的大小 （ 默認 '15px' ）
 * @property {String}			color					輸入框字體顏色	（ 默認 '#303133' ）
 * @property {Function}			formatter			    内容式化函數
 * @property {String}			prefixIcon				輸入框前置圖標
 * @property {String | Object}	prefixIconStyle			前置圖標樣式，對象或字符串
 * @property {String}			suffixIcon				輸入框后置圖標
 * @property {String | Object}	suffixIconStyle			后置圖標樣式，對象或字符串
 * @property {String}			border					邊框類型，surround-四周邊框，bottom-底部邊框，none-無邊框 （ 默認 'surround' ）
 * @property {Boolean}			readonly				是否只讀，與disabled不同之處在于disabled會置灰组件，而readonly则不會 （ 默認 false ）
 * @property {String}			shape					輸入框形狀，circle-圆形，square-方形 （ 默認 'square' ）
 * @property {Object}			customStyle				定義需要用到的外部樣式
 * @property {Boolean}			ignoreCompositionEvent	是否忽略组件内對文本合成系统事件的處理。
 * @example <u-input v-model="value" :password="true" suffix-icon="lock-fill" />
 */
export default {
    name: "u-input",
    mixins: [mpMixin, mixin, props],
    data() {
        return {
            // 清除操作
            clearInput: false,
            // 輸入框的值
            innerValue: "",
            // 是否處于獲得焦點狀態
            focused: false,
            // value是否第一次變化，在watch中，由于加入immediate屬性，會在第一次触發，此時不應該認為value發生了變化
            firstChange: true,
            // value绑定值的變化是由内部還是外部引起的
            changeFromInner: false,
			// 過濾處理方法
			innerFormatter: value => value
        };
    },
    created() {
        // 格式化過濾方法
        if (this.formatter) {
            this.innerFormatter = this.formatter;
        }
    },
    watch: {
        modelValue: {
            immediate: true,
            handler(newVal, oldVal) {
                // console.log(newVal, oldVal)
                if (this.changeFromInner || this.innerValue === newVal) {
                    this.changeFromInner = false; // 重要否则會出現双向绑定失效問題https://github.com/ijry/uview-plus/issues/419
                    return;
                }
                this.innerValue = newVal;
                // 在H5中，外部value變化后，修改input中的值，不會触發@input事件，此時手動調用值變化方法
                if (
                    this.firstChange === false &&
					this.changeFromInner === false
                ) {
                    this.valueChange(this.innerValue, true);
                } else {
					// 尝试調用up-form的验证方法
                    if(!this.firstChange) formValidate(this, "change");
				}
                this.firstChange = false;
                // 重置changeFromInner的值為false，標识下一次引起默認為外部引起的
                this.changeFromInner = false;
            }
        }
    },
    computed: {
        // 是否顯示清除控件
        isShowClear() {
            const { clearable, readonly, focused, innerValue } = this;
            return !!clearable && !readonly && !!focused && innerValue !== "";
        },
        // 组件的類名
        inputClass() {
            let classes = [],
                { border, disabled, shape } = this;
            border === "surround" &&
                (classes = classes.concat(["u-border", "u-input--radius"]));
            classes.push(`u-input--${shape}`);
            border === "bottom" &&
                (classes = classes.concat([
                    "u-border-bottom",
                    "u-input--no-radius",
                ]));
            return classes.join(" ");
        },
        // 组件的樣式
        wrapperStyle() {
            const style = {};
            // 禁用狀態下，被背景色加上對應的樣式
            if (this.disabled) {
                style.backgroundColor = this.disabledColor;
            }
            // 無邊框時，去除内邊距
            if (this.border === "none") {
                style.padding = "0";
            } else {
                // 由于uni-app的iOS開發者能力有限，导致需要分開写才有效
                style.paddingTop = "6px";
                style.paddingBottom = "6px";
                style.paddingLeft = "9px";
                style.paddingRight = "9px";
            }
            return deepMerge(style, addStyle(this.customStyle));
        },
        // 輸入框的樣式
        inputStyle() {
            const style = {
                color: this.color,
                fontSize: addUnit(this.fontSize),
				textAlign: this.inputAlign
            };
            return style;
        },
    },
    // #ifdef VUE3
    emits: ['update:modelValue', 'focus', 'blur', 'change', 'confirm', 'clear', 'keyboardheightchange', 'nicknamereview'],
    // #endif
    methods: {
		// 在微信小程序中，不支持將函數當做props参數，故只能通過ref形式調用
		setFormatter(e) {
			this.innerFormatter = e
		},
        // 當鍵盘輸入時，触發input事件
        onInput(e) {
            let { value = "" } = e.detail || {};
            // 為了避免props的單向數據流特性，需要先將innerValue值設置為當前值，再在$nextTick中重新赋予設置后的值才有效
            // console.log('onInput', value, this.innerValue)
            this.innerValue = value;
            this.$nextTick(() => {
                let formatValue = this.innerFormatter(value);
            	this.innerValue = formatValue;
                this.valueChange(formatValue);
            })
        },
        // 輸入框失去焦點時触發
        onBlur(event) {
            this.$emit("blur", event.detail.value);
            // H5端的blur會先于點擊清除控件的點擊click事件触發，导致focused
            // 瞬間為false，從而隐藏了清除控件而無法被點擊到
            sleep(150).then(() => {
                this.focused = false;
            });
            // 尝试調用u-form的验证方法
            formValidate(this, "blur");
        },
        // 輸入框聚焦時触發
        onFocus(event) {
            this.focused = true;
            this.$emit("focus");
        },
        doFocus() {
            this.$refs['input-native'].focus();
        },
        doBlur() {
            this.$refs['input-native'].blur();
        },
        // 點擊完成按鈕時触發
        onConfirm(event) {
            this.$emit("confirm", this.innerValue);
        },
        // 鍵盘高度發生變化的時候触發此事件
        // 兼容性：微信小程序2.7.0+、App 3.1.0+
		onkeyboardheightchange(event) {
            this.$emit("keyboardheightchange", event);
        },
        onnicknamereview(event) {
            this.$emit("nicknamereview", event);
        },
        // 内容發生變化，进行處理
        valueChange(value, isOut = false) {
            if(this.clearInput) {
                this.innerValue = '';
                this.clearInput = false;
            }
            this.$nextTick(() => {
                if (!isOut || this.clearInput) {
                    // 標识value值的變化是由内部引起的
                    this.changeFromInner = true;
                    this.$emit("change", value);

                    // #ifdef VUE3
                    this.$emit("update:modelValue", value);
                    // #endif
                    // #ifdef VUE2
                    this.$emit("input", value);
                    // #endif
                }

                // 尝试調用u-form的验证方法
                formValidate(this, "change");
            });
        },
        // 點擊清除控件
        onClear() {
            this.clearInput = true;
            this.innerValue = "";
            this.$nextTick(() => {
                this.valueChange("");
                this.$emit("clear");
            });
        },
        /**
         * 在安卓nvue上，事件無法冒泡
         * 在某些時間，我们希望監聽u-from-item的點擊事件，此時會导致點擊u-form-item内的u-input后
         * 無法触發u-form-item的點擊事件，這里通過手動調用u-form-item的方法进行触發
         */
        clickHandler() {
            // #ifdef APP-NVUE
            if (os() === "android") {
                const formItem = $parent.call(this, "u-form-item");
                if (formItem) {
                    formItem.clickHandler();
                }
            }
            // #endif
        },
    },
};
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";

.u-input {
    @include flex(row);
    align-items: center;
    justify-content: space-between;
    flex: 1;

    &--radius,
    &--square {
        border-radius: 4px;
    }

    &--no-radius {
        border-radius: 0;
    }

    &--circle {
        border-radius: 100px;
    }

    &__content {
        flex: 1;
        @include flex(row);
        align-items: center;
        justify-content: space-between;

        &__field-wrapper {
            position: relative;
            @include flex(row);
            margin: 0;
            flex: 1;
			
			&__field {
				line-height: 26px;
				text-align: left;
				color: $u-main-color;
				height: 24px;
				font-size: 15px;
				flex: 1;
			}
        }

        &__clear {
            width: 20px;
            height: 20px;
            border-radius: 100px;
            background-color: #c6c7cb;
            @include flex(row);
            align-items: center;
            justify-content: center;
            transform: scale(0.82);
            margin-left: 4px;
        }

        &__subfix-icon {
            margin-left: 4px;
        }

        &__prefix-icon {
            margin-right: 4px;
        }
    }
}
</style>
