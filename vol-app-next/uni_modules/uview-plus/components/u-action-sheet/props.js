import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'

export const props = defineMixin({
    props: {
        // 操作菜單是否展示 （默认false）
        show: {
            type: Boolean,
            default: () => defProps.actionSheet.show
        },
        // 標題
        title: {
            type: String,
            default: () => defProps.actionSheet.title
        },
        // 選项上方的描述信息
        description: {
            type: String,
            default: () => defProps.actionSheet.description
        },
        // 數据
        actions: {
            type: Array,
            default: () => defProps.actionSheet.actions
        },
        // 取消按鈕的文字，不為空时显示按鈕
        cancelText: {
            type: String,
            default: () => defProps.actionSheet.cancelText
        },
        // 点击某个菜單项时是否關闭彈窗
        closeOnClickAction: {
            type: Boolean,
            default: () => defProps.actionSheet.closeOnClickAction
        },
        // 处理底部安全區（默认true）
        safeAreaInsetBottom: {
            type: Boolean,
            default: () => defProps.actionSheet.safeAreaInsetBottom
        },
        // 小程序的打开方式
        openType: {
            type: String,
            default: () => defProps.actionSheet.openType
        },
        // 点击遮罩是否允许關闭 (默认true)
        closeOnClickOverlay: {
            type: Boolean,
            default: () => defProps.actionSheet.closeOnClickOverlay
        },
        // 圆角值
        round: {
            type: [Boolean, String, Number],
            default: () => defProps.actionSheet.round
        },
        // 選项區域最大高度
        wrapMaxHeight: {
            type: [String],
            default: () => defProps.actionSheet.wrapMaxHeight
        },
    }
})
