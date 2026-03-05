import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // radio的名稱
        name: {
            type: [String, Number, Boolean],
            default: () => defProps.radio.name
        },
        // 形状，square為方形，circle為圆型
        shape: {
            type: String,
            default: () => defProps.radio.shape
        },
        // 是否禁用
        disabled: {
            type: [String, Boolean],
            default: () => defProps.radio.disabled
        },
        // 是否禁止点击提示语選中單選框
        labelDisabled: {
            type: [String, Boolean],
            default: () => defProps.radio.labelDisabled
        },
        // 選中状态下的颜色，如設置此值，將会覆蓋parent的activeColor值
        activeColor: {
            type: String,
            default: () => defProps.radio.activeColor
        },
        // 未選中的颜色
        inactiveColor: {
            type: String,
            default: () => defProps.radio.inactiveColor
        },
        // 图標的大小，單位px
        iconSize: {
            type: [String, Number],
            default: () => defProps.radio.iconSize
        },
        // label的字体大小，px單位
        labelSize: {
            type: [String, Number],
            default: () => defProps.radio.labelSize
        },
        // label提示文字，因為nvue下，直接slot进来的文字，由于特殊的结构，無法修改樣式
        label: {
            type: [String, Number],
            default: () => defProps.radio.label
        },
        // 整体的大小
        size: {
            type: [String, Number],
            default: () => defProps.radio.size
        },
        // 图標颜色
        color: {
            type: String,
            default: () => defProps.radio.color
        },
        // label的颜色
        labelColor: {
            type: String,
            default: () => defProps.radio.labelColor
        },
        // 图標颜色
        iconColor: {
            type: String,
            default: () => defProps.radio.iconColor
        }
    }
})
