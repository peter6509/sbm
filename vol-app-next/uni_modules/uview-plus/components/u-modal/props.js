import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 是否展示modal
        show: {
            type: Boolean,
            default: () => defProps.modal.show
        },
        // 標題
        title: {
            type: [String],
            default: () => defProps.modal.title
        },
        // 彈窗内容
        content: {
            type: String,
            default: () => defProps.modal.content
        },
        // 確认文案
        confirmText: {
            type: String,
            default: () => defProps.modal.confirmText
        },
        // 取消文案
        cancelText: {
            type: String,
            default: () => defProps.modal.cancelText
        },
        // 是否显示確认按鈕
        showConfirmButton: {
            type: Boolean,
            default: () => defProps.modal.showConfirmButton
        },
        // 是否显示取消按鈕
        showCancelButton: {
            type: Boolean,
            default: () => defProps.modal.showCancelButton
        },
        // 確认按鈕颜色
        confirmColor: {
            type: String,
            default: () => defProps.modal.confirmColor
        },
        // 取消文字颜色
        cancelColor: {
            type: String,
            default: () => defProps.modal.cancelColor
        },
        // 对调確认和取消的位置
        buttonReverse: {
            type: Boolean,
            default: () => defProps.modal.buttonReverse
        },
        // 是否开啟缩放效果
        zoom: {
            type: Boolean,
            default: () => defProps.modal.zoom
        },
        // 是否异步關闭，只对確定按鈕有效
        asyncClose: {
            type: Boolean,
            default: () => defProps.modal.asyncClose
        },
        // 是否允许点击遮罩關闭modal
        closeOnClickOverlay: {
            type: Boolean,
            default: () => defProps.modal.closeOnClickOverlay
        },
        // 给一个负的margin-top，往上偏移，避免和鍵盘重合的情况
        negativeTop: {
            type: [String, Number],
            default: () => defProps.modal.negativeTop
        },
        // modal寬度，不支持百分比，可以數值，px，rpx單位
        width: {
            type: [String, Number],
            default: () => defProps.modal.width
        },
        // 確认按鈕的樣式，circle-圆形，square-方形，如設置，將不会显示取消按鈕
        confirmButtonShape: {
            type: String,
            default: () => defProps.modal.confirmButtonShape
        },
        // 文案对齊方式
        contentTextAlign: {
            type: String,
            default: () => defProps.modal.contentTextAlign
        },
    }
})
