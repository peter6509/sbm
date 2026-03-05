import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 需要显示的提示文字
        text: {
            type: [String, Number],
            default: () => defProps.tooltip.text
        },
        // 点击複制按鈕时，複制的文本，為空则使用text值
        copyText: {
            type: [String, Number],
            default: () => defProps.tooltip.copyText
        },
        // 文本大小
        size: {
            type: [String, Number],
            default: () => defProps.tooltip.size
        },
        // 字体颜色
        color: {
            type: String,
            default: () => defProps.tooltip.color
        },
        // 彈出提示框时，文本的背景色
        bgColor: {
            type: String,
            default: () => defProps.tooltip.bgColor
        },
        // 彈出提示的方向，top-上方，bottom-下方
        direction: {
            type: String,
            default: () => defProps.tooltip.direction
        },
        // 彈出提示的z-index，nvue無效
        zIndex: {
            type: [String, Number],
            default: () => defProps.tooltip.zIndex
        },
        // 是否显示複制按鈕
        showCopy: {
            type: Boolean,
            default: () => defProps.tooltip.showCopy
        },
        // 扩展的按鈕组
        buttons: {
            type: Array,
            default: () => defProps.tooltip.buttons
        },
        // 是否显示透明遮罩以防止触摸穿透
        overlay: {
            type: Boolean,
            default: () => defProps.tooltip.overlay
        },
        // 是否显示複制成功或者失败的toast
        showToast: {
            type: Boolean,
            default: () => defProps.tooltip.showToast
        }
    }
})
