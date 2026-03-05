import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'

export const props = defineMixin({
    props: {
        // 標识符
        name: {
            type: String,
            default: () => defProps.checkboxGroup.name
        },
		// #ifdef VUE3
		// 绑定的值
		modelValue: {
		    type: Array,
		    default: () => defProps.checkboxGroup.value
		},
		// #endif
		// #ifdef VUE2
		// 绑定的值
		value: {
		    type: Array,
		    default: () => defProps.checkboxGroup.value
		},
		// #endif
        // 形状，circle-圆形，square-方形
        shape: {
            type: String,
            default: () => defProps.checkboxGroup.shape
        },
        // 是否禁用全部checkbox
        disabled: {
            type: Boolean,
            default: () => defProps.checkboxGroup.disabled
        },

        // 選中状态下的颜色，如設置此值，將会覆蓋parent的activeColor值
        activeColor: {
            type: String,
            default: () => defProps.checkboxGroup.activeColor
        },
        // 未選中的颜色
        inactiveColor: {
            type: String,
            default: () => defProps.checkboxGroup.inactiveColor
        },

        // 整个组件的尺寸，默认px
        size: {
            type: [String, Number],
            default: () => defProps.checkboxGroup.size
        },
        // 布局方式，row-横向，column-纵向
        placement: {
            type: String,
            default: () => defProps.checkboxGroup.placement
        },
        // label的字体大小，px單位
        labelSize: {
            type: [String, Number],
            default: () => defProps.checkboxGroup.labelSize
        },
        // label的字体颜色
        labelColor: {
            type: [String],
            default: () => defProps.checkboxGroup.labelColor
        },
        // 是否禁止点击文本操作
        labelDisabled: {
            type: Boolean,
            default: () => defProps.checkboxGroup.labelDisabled
        },
        // 图標颜色
        iconColor: {
            type: String,
            default: () => defProps.checkboxGroup.iconColor
        },
        // 图標的大小，單位px
        iconSize: {
            type: [String, Number],
            default: () => defProps.checkboxGroup.iconSize
        },
        // 勾選图標的对齊方式，left-左边，right-右边
        iconPlacement: {
            type: String,
            default: () => defProps.checkboxGroup.iconPlacement
        },
        // 竖向配列时，是否显示下划线
        borderBottom: {
            type: Boolean,
            default: () => defProps.checkboxGroup.borderBottom
        }
    }
})
