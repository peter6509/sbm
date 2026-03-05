import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 是否展示工具条
        show: {
            type: Boolean,
            default: () => defProps.toolbar.show
        },
        // 取消按鈕的文字
        cancelText: {
            type: String,
            default: () => defProps.toolbar.cancelText
        },
        // 確认按鈕的文字
        confirmText: {
            type: String,
            default: () => defProps.toolbar.confirmText
        },
        // 取消按鈕的颜色
        cancelColor: {
            type: String,
            default: () => defProps.toolbar.cancelColor
        },
        // 確认按鈕的颜色
        confirmColor: {
            type: String,
            default: () => defProps.toolbar.confirmColor
        },
        // 標題文字
        title: {
            type: String,
            default: () => defProps.toolbar.title
        },
        // 开啟右侧插槽
        rightSlot: {
            type: Boolean,
            default: false
        }
    }
})
