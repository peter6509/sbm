import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 步进器標识符，在change回调返回
        name: {
            type: [String, Number],
            default: () => defProps.numberBox.name
        },
        // #ifdef VUE2
        // 用于双向绑定的值，初始化时設置設為默认min值(最小值)
        value: {
            type: [String, Number],
            default: () => defProps.numberBox.value
        },
        // #endif
        // #ifdef VUE3
        // 用于双向绑定的值，初始化时設置設為默认min值(最小值)
        modelValue: {
            type: [String, Number],
            default: () => defProps.numberBox.value
        },
        // #endif
        // 最小值
        min: {
            type: [String, Number],
            default: () => defProps.numberBox.min
        },
        // 最大值
        max: {
            type: [String, Number],
            default: () => defProps.numberBox.max
        },
        // 加减的步長，可為小數
        step: {
            type: [String, Number],
            default: () => defProps.numberBox.step
        },
        // 是否只允许输入整數
        integer: {
            type: Boolean,
            default: () => defProps.numberBox.integer
        },
        // 是否禁用，包括输入框，加减按鈕
        disabled: {
            type: Boolean,
            default: () => defProps.numberBox.disabled
        },
        // 是否禁用输入框
        disabledInput: {
            type: Boolean,
            default: () => defProps.numberBox.disabledInput
        },
        // 是否开啟异步变更，开啟后需要手动控制输入值
        asyncChange: {
            type: Boolean,
            default: () => defProps.numberBox.asyncChange
        },
        // 输入框寬度，單位為px
        inputWidth: {
            type: [String, Number],
            default: () => defProps.numberBox.inputWidth
        },
        // 是否显示减少按鈕
        showMinus: {
            type: Boolean,
            default: () => defProps.numberBox.showMinus
        },
        // 是否显示增加按鈕
        showPlus: {
            type: Boolean,
            default: () => defProps.numberBox.showPlus
        },
        // 显示的小數位數
        decimalLength: {
            type: [String, Number, null],
            default: () => defProps.numberBox.decimalLength
        },
        // 是否开啟長按加减手势
        longPress: {
            type: Boolean,
            default: () => defProps.numberBox.longPress
        },
        // 输入框文字和加减按鈕图標的颜色
        color: {
            type: String,
            default: () => defProps.numberBox.color
        },
        // 按鈕大小，寬高等于此值，單位px，输入框高度和此值保持一致
        buttonSize: {
            type: [String, Number],
            default: () => defProps.numberBox.buttonSize
        },
        // 输入框和按鈕的背景颜色
        bgColor: {
            type: String,
            default: () => defProps.numberBox.bgColor
        },
        // 指定光標于鍵盘的距离，避免鍵盘遮挡输入框，單位px
        cursorSpacing: {
            type: [String, Number],
            default: () => defProps.numberBox.cursorSpacing
        },
        // 是否禁用增加按鈕
        disablePlus: {
            type: Boolean,
            default: () => defProps.numberBox.disablePlus
        },
        // 是否禁用减少按鈕
        disableMinus: {
            type: Boolean,
            default: () => defProps.numberBox.disableMinus
        },
        // 加减按鈕图標的樣式
        iconStyle: {
            type: [Object, String],
            default: () => defProps.numberBox.iconStyle
        }
    }
})
