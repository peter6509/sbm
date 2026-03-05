import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 是否為加载中状态
        loading: {
            type: Boolean,
            default: () => defProps.switch.loading
        },
        // 是否為禁用裝填
        disabled: {
            type: Boolean,
            default: () => defProps.switch.disabled
        },
        // 开關尺寸，單位px
        size: {
            type: [String, Number],
            default: () => defProps.switch.size
        },
        // 打开时的背景颜色
        activeColor: {
            type: String,
            default: () => defProps.switch.activeColor
        },
        // 關闭时的背景颜色
        inactiveColor: {
            type: String,
            default: () => defProps.switch.inactiveColor
        },
        // 通过v-model双向绑定的值
        // #ifdef VUE3
        modelValue: {
            type: [Boolean, String, Number],
            default: () => defProps.switch.value
        },
        // #endif
        // #ifdef VUE2
        value: {
            type: [Boolean, String, Number],
            default: () => defProps.switch.value
        },
        // #endif
        // switch打开时的值
        activeValue: {
            type: [String, Number, Boolean],
            default: () => defProps.switch.activeValue
        },
        // switch關闭时的值
        inactiveValue: {
            type: [String, Number, Boolean],
            default: () => defProps.switch.inactiveValue
        },
        // 是否开啟异步变更，开啟后需要手动控制输入值
        asyncChange: {
            type: Boolean,
            default: () => defProps.switch.asyncChange
        },
        // 圆点與外边框的距离
        space: {
            type: [String, Number],
            default: () => defProps.switch.space
        }
    }
})
