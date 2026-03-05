<template>
    <view class="u-textarea" :class="textareaClass" :style="[textareaStyle]">
        <textarea
            class="u-textarea__field"
            :value="innerValue"
            :style="{ height: addUnit(height) }"
            :placeholder="placeholder"
            :placeholder-style="addStyle(placeholderStyle, 'string')"
            :placeholder-class="placeholderClass"
            :disabled="disabled"
            :focus="focus"
            :autoHeight="autoHeight"
            :fixed="fixed"
            :cursorSpacing="cursorSpacing"
            :cursor="cursor"
            :showConfirmBar="showConfirmBar"
            :selectionStart="selectionStart"
            :selectionEnd="selectionEnd"
            :adjustPosition="adjustPosition"
            :disableDefaultPadding="disableDefaultPadding"
            :holdKeyboard="holdKeyboard"
            :maxlength="maxlength"
            :confirm-type="confirmType"
            :ignoreCompositionEvent="ignoreCompositionEvent"
            @focus="onFocus"
            @blur="onBlur"
            @linechange="onLinechange"
            @input="onInput"
            @confirm="onConfirm"
            @keyboardheightchange="onKeyboardheightchange"
        ></textarea>
		<!-- #ifndef MP-ALIPAY -->
        <text
            class="u-textarea__count"
            :style="{
                'background-color': disabled ? 'transparent' : '#fff',
            }"
            v-if="count"
            >{{ innerValue.length }}/{{ maxlength }}</text
        >
		<!-- #endif -->
    </view>
</template>

<script>
import { props } from "./props.js";
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { addStyle, addUnit, deepMerge, formValidate, os } from '../../libs/function/index';
/**
 * Textarea 文本域
 * @description 文本域此组件满足了可能出現的表單信息补充，編輯等實际逻輯的功能，内置了字數校验等
 * @tutorial https://ijry.github.io/uview-plus/components/textarea.html
 *
 * @property {String | Number} 		value					輸入框的内容
 * @property {String | Number}		placeholder				輸入框為空時占位符
 * @property {String}			    placeholderClass		指定placeholder的樣式類，注意頁面或组件的style中写了scoped時，需要在類名前写/deep/ （ 默認 'input-placeholder' ）
 * @property {String | Object}	    placeholderStyle		指定placeholder的樣式，字符串/對象形式，如"color: red;"
 * @property {String | Number}		height					輸入框高度（默認 70 ）
 * @property {String}				confirmType				設置鍵盘右下角按鈕的文字，仅微信小程序，App-vue和H5有效（默認 'done' ）
 * @property {Boolean}				disabled				是否禁用（默認 false ）
 * @property {Boolean}				count					是否顯示统計字數（默認 false ）
 * @property {Boolean}				focus					是否自動獲取焦點，nvue不支持，H5取决于浏覽器的實現（默認 false ）
 * @property {Boolean | Function}	autoHeight				是否自動增加高度（默認 false ）
 * @property {Boolean}				fixed					如果textarea是在一個position:fixed的區域，需要顯示指定屬性fixed為true（默認 false ）
 * @property {Number}				cursorSpacing			指定光標與鍵盘的距离（默認 0 ）
 * @property {String | Number}		cursor					指定focus時的光標位置
 * @property {Function}			    formatter			    内容式化函數
 * @property {Boolean}				showConfirmBar			是否顯示鍵盘上方带有”完成“按鈕那一栏，（默認 true ）
 * @property {Number}				selectionStart			光標起始位置，自動聚焦時有效，需與selection-end搭配使用，（默認 -1 ）
 * @property {Number | Number}		selectionEnd			光標结束位置，自動聚焦時有效，需與selection-start搭配使用（默認 -1 ）
 * @property {Boolean}				adjustPosition			鍵盘彈起時，是否自動上推頁面（默認 true ）
 * @property {Boolean | Number}		disableDefaultPadding	是否去掉 iOS 下的默認内邊距，只微信小程序有效（默認 false ）
 * @property {Boolean}				holdKeyboard			focus時，點擊頁面的時候不收起鍵盘，只微信小程序有效（默認 false ）
 * @property {String | Number}		maxlength				最大輸入長度，設置為 -1 的時候不限制最大長度（默認 140 ）
 * @property {String}				border					邊框類型，surround-四周邊框，none-無邊框，bottom-底部邊框（默認 'surround' ）
 * @property {Boolean}				ignoreCompositionEvent	是否忽略组件内對文本合成系统事件的處理
 *
 * @event {Function(e)} focus					輸入框聚焦時触發，event.detail = { value, height }，height 為鍵盘高度
 * @event {Function(e)} blur					輸入框失去焦點時触發，event.detail = {value, cursor}
 * @event {Function(e)} linechange				輸入框行數變化時調用，event.detail = {height: 0, heightRpx: 0, lineCount: 0}
 * @event {Function(e)} input					當鍵盘輸入時，触發 input 事件
 * @event {Function(e)} confirm					點擊完成時， 触發 confirm 事件
 * @event {Function(e)} keyboardheightchange	鍵盘高度發生變化的時候触發此事件
 * @example <up-textarea v-model="value1" placeholder="請輸入内容" ></up-textarea>
 */
export default {
    name: "u-textarea",
    mixins: [mpMixin, mixin, props],
	data() {
		return {
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
		}
	},
	created() {
	},
	watch: {
        // #ifdef VUE2
	    value: {
	        immediate: true,
	        handler(newVal, oldVal) {
	            this.innerValue = newVal;
	            /* #ifdef H5 */
	            // 在H5中，外部value變化后，修改input中的值，不會触發@input事件，此時手動調用值變化方法
	            if (
	                this.firstChange === false &&
	                this.changeFromInner === false
	            ) {
	                this.valueChange();
	            }
	            /* #endif */
	            this.firstChange = false;
	            // 重置changeFromInner的值為false，標识下一次引起默認為外部引起的
	            this.changeFromInner = false;
	        },
	    },
        // #endif
        // #ifdef VUE3
        modelValue: {
	        immediate: true,
	        handler(newVal, oldVal) {
	            this.innerValue = newVal;
	            /* #ifdef H5 */
	            // 在H5中，外部value變化后，修改input中的值，不會触發@input事件，此時手動調用值變化方法
	            if (
	                this.firstChange === false &&
	                this.changeFromInner === false
	            ) {
	                this.valueChange();
	            }
	            /* #endif */
	            this.firstChange = false;
	            // 重置changeFromInner的值為false，標识下一次引起默認為外部引起的
	            this.changeFromInner = false;
	        },
	    }
        // #endif
	},
    computed: {
        // 组件的類名
        textareaClass() {
            let classes = [],
                { border, disabled } = this;
            border === "surround" &&
                (classes = classes.concat(["u-border", "u-textarea--radius"]));
            border === "bottom" &&
                (classes = classes.concat([
                    "u-border-bottom",
                    "u-textarea--no-radius",
                ]));
            disabled && classes.push("u-textarea--disabled");
            return classes.join(" ");
        },
        // 组件的樣式
        textareaStyle() {
            const style = {};
            // #ifdef APP-NVUE
            // 由于textarea在安卓nvue上的差異性，需要額外再調整其内邊距
            if (os() === "android") {
                style.paddingTop = "6px";
                style.paddingLeft = "9px";
                style.paddingBottom = "3px";
                style.paddingRight = "6px";
            }
            // #endif
            return deepMerge(style, addStyle(this.customStyle));
        },
    },
    // #ifdef VUE3
    emits: ['update:modelValue', 'linechange', 'focus', 'blur', 'change', 'confirm', 'keyboardheightchange'],
    // #endif
    methods: {
        addStyle,
        addUnit,
		// 在微信小程序中，不支持將函數當做props参數，故只能通過ref形式調用
		setFormatter(e) {
			this.innerFormatter = e
		},
        onFocus(e) {
            this.$emit("focus", e);
        },
        onBlur(e) {
            this.$emit("blur", e);
            // 尝试調用u-form的验证方法
            formValidate(this, "blur");
        },
        onLinechange(e) {
            this.$emit("linechange", e);
        },
        onInput(e) {
			let { value = "" } = e.detail || {};
			// 格式化過濾方法
			const formatter = this.formatter || this.innerFormatter
			const formatValue = formatter(value)
			// 為了避免props的單向數據流特性，需要先將innerValue值設置為當前值，再在$nextTick中重新赋予設置后的值才有效
			this.innerValue = value
			this.$nextTick(() => {
				this.innerValue = formatValue;
				this.valueChange();
			})
        },
		// 内容發生變化，进行處理
		valueChange() {
		    const value = this.innerValue;
		    this.$nextTick(() => {
                // #ifdef VUE3
                this.$emit("update:modelValue", value);
                // #endif
                // #ifdef VUE2
                this.$emit("input", value);
                // #endif
		        // 標识value值的變化是由内部引起的
		        this.changeFromInner = true;
		        this.$emit("change", value);
		        // 尝试調用u-form的验证方法
		        formValidate(this, "change");
		    });
		},
        onConfirm(e) {
            this.$emit("confirm", e);
        },
        onKeyboardheightchange(e) {
            this.$emit("keyboardheightchange", e);
        },
    },
};
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";

.u-textarea {
    border-radius: 4px;
    background-color: #fff;
    position: relative;
    @include flex;
    flex: 1;
	padding: 9px;

    &--radius {
        border-radius: 4px;
    }

    &--no-radius {
        border-radius: 0;
    }

    &--disabled {
        background-color: #f5f7fa;
    }

    &__field {
        flex: 1;
        font-size: 15px;
        color: $u-content-color;
		width: 100%;
    }

    &__count {
        position: absolute;
        right: 5px;
        bottom: 2px;
        font-size: 12px;
        color: $u-tips-color;
        background-color: #ffffff;
        padding: 1px 4px;
    }
}
</style>
