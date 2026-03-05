import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // #ifdef VUE3
        // 绑定的值
        modelValue: {
            type: [String, Number, Boolean],
            default: () => defProps.radioGroup.value
        },
        // #endif
        // #ifdef VUE2
        // 绑定的值
        value: {
            type: [String, Number, Boolean],
            default: () => defProps.radioGroup.value
        },
        // #endif
        // 是否禁用全部radio
        disabled: {
            type: Boolean,
            default: () => defProps.radioGroup.disabled
        },
        // 形状，circle-圆形，square-方形
        shape: {
            type: String,
            default: () => defProps.radioGroup.shape
        },
        // 選中状态下的颜色，如設置此值，將会覆蓋parent的activeColor值
        activeColor: {
            type: String,
            default: () => defProps.radioGroup.activeColor
        },
        // 未選中的颜色
        inactiveColor: {
            type: String,
            default: () => defProps.radioGroup.inactiveColor
        },
        // 標识符
        name: {
            type: String,
            default: () => defProps.radioGroup.name
        },
        // 整个组件的尺寸，默认px
        size: {
            type: [String, Number],
            default: () => defProps.radioGroup.size
        },
        // 布局方式，row-横向，column-纵向
        placement: {
            type: String,
            default: () => defProps.radioGroup.placement
        },
        // label的文本
        label: {
            type: [String],
            default: () => defProps.radioGroup.label
        },
        // label的颜色 （默认 '#303133' ）
        labelColor: {
            type: [String],
            default: () => defProps.radioGroup.labelColor
        },
        // label的字体大小，px單位
        labelSize: {
            type: [String, Number],
            default: () => defProps.radioGroup.labelSize
        },
        // 是否禁止点击文本操作checkbox(默认 false )
        labelDisabled: {
            type: Boolean,
            default: () => defProps.radioGroup.labelDisabled
        },
        // 图標颜色
        iconColor: {
            type: String,
            default: () => defProps.radioGroup.iconColor
        },
        // 图標的大小，單位px
        iconSize: {
            type: [String, Number],
            default: () => defProps.radioGroup.iconSize
        },
        // 竖向配列时，是否显示下划线
        borderBottom: {
            type: Boolean,
            default: () => defProps.radioGroup.borderBottom
        },
        // 图標與文字的对齊方式
        iconPlacement: {
            type: String,
            default: () => defProps.radio.iconPlacement
        }
    }
})
