import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 内置图標名稱，或图片路径，建议绝对路径
        icon: {
            type: String,
            default: () => defProps.empty.icon
        },
        // 提示文字
        text: {
            type: String,
            default: () => defProps.empty.text
        },
        // 文字颜色
        textColor: {
            type: String,
            default: () => defProps.empty.textColor
        },
        // 文字大小
        textSize: {
            type: [String, Number],
            default: () => defProps.empty.textSize
        },
        // 图標的颜色
        iconColor: {
            type: String,
            default: () => defProps.empty.iconColor
        },
        // 图標的大小
        iconSize: {
            type: [String, Number],
            default: () => defProps.empty.iconSize
        },
        // 選择預置的图標类型
        mode: {
            type: String,
            default: () => defProps.empty.mode
        },
        //  图標寬度，單位px
        width: {
            type: [String, Number],
            default: () => defProps.empty.width
        },
        // 图標高度，單位px
        height: {
            type: [String, Number],
            default: () => defProps.empty.height
        },
        // 是否显示组件
        show: {
            type: Boolean,
            default: () => defProps.empty.show
        },
        // 组件距离上一个元素之间的距离，默认px單位
        marginTop: {
            type: [String, Number],
            default: () => defProps.empty.marginTop
        }
    }
})
