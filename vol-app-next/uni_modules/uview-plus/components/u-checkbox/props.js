import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // checkbox的名稱
        name: {
            type: [String, Number, Boolean],
            default: () => defProps.checkbox.name
        },
        // 形状，square為方形，circle為圆型
        shape: {
            type: String,
            default: () => defProps.checkbox.shape
        },
        // 整体的大小
        size: {
            type: [String, Number],
            default: () => defProps.checkbox.size
        },
        // 是否默认選中
        checked: {
            type: Boolean,
            default: () => defProps.checkbox.checked
        },
        // 是否禁用
        disabled: {
            type: [String, Boolean],
            default: () => defProps.checkbox.disabled
        },
        // 選中状态下的颜色，如設置此值，將会覆蓋parent的activeColor值
        activeColor: {
            type: String,
            default: () => defProps.checkbox.activeColor
        },
        // 未選中的颜色
        inactiveColor: {
            type: String,
            default: () => defProps.checkbox.inactiveColor
        },
        // 图標的大小，單位px
        iconSize: {
            type: [String, Number],
            default: () => defProps.checkbox.iconSize
        },
        // 图標颜色
        iconColor: {
            type: String,
            default: () => defProps.checkbox.iconColor
        },
        // label提示文字，因為nvue下，直接slot进来的文字，由于特殊的结构，無法修改樣式
        label: {
            type: [String, Number],
            default: () => defProps.checkbox.label
        },
        // label的字体大小，px單位
        labelSize: {
            type: [String, Number],
            default: () => defProps.checkbox.labelSize
        },
        // label的颜色
        labelColor: {
            type: String,
            default: () => defProps.checkbox.labelColor
        },
        // 是否禁止点击提示语選中複選框
        labelDisabled: {
            type: [String, Boolean],
            default: () => defProps.checkbox.labelDisabled
        },
		// 是否独立使用
        usedAlone: {
            type: [Boolean],
            default: () => false
        }
    }
})
