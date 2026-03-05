import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 鍵盘的类型，number-數字鍵盘，card-身份证鍵盘，car-车牌号鍵盘
        mode: {
            type: String,
            default: () => defProps.keyboard.mode
        },
        // 是否显示鍵盘的"."符号
        dotDisabled: {
            type: Boolean,
            default: () => defProps.keyboard.dotDisabled
        },
        // 是否显示顶部工具条
        tooltip: {
            type: Boolean,
            default: () => defProps.keyboard.tooltip
        },
        // 是否显示工具条中间的提示
        showTips: {
            type: Boolean,
            default: () => defProps.keyboard.showTips
        },
        // 工具条中间的提示文字
        tips: {
            type: String,
            default: () => defProps.keyboard.tips
        },
        // 是否显示工具条左边的"取消"按鈕
        showCancel: {
            type: Boolean,
            default: () => defProps.keyboard.showCancel
        },
        // 是否显示工具条右边的"完成"按鈕
        showConfirm: {
            type: Boolean,
            default: () => defProps.keyboard.showConfirm
        },
        // 是否打乱鍵盘按鍵的顺序
        random: {
            type: Boolean,
            default: () => defProps.keyboard.random
        },
        // 是否开啟底部安全區適配，开啟的话，会在iPhoneX机型底部添加一定的内边距
        safeAreaInsetBottom: {
            type: Boolean,
            default: () => defProps.keyboard.safeAreaInsetBottom
        },
        // 是否允许通过点击遮罩關闭鍵盘
        closeOnClickOverlay: {
            type: Boolean,
            default: () => defProps.keyboard.closeOnClickOverlay
        },
        // 控制鍵盘的彈出與收起
        show: {
            type: Boolean,
            default: () => defProps.keyboard.show
        },
        // 是否显示遮罩，某些时候數字鍵盘时，用户希望看到自己的數值，所以可能不想要遮罩
        overlay: {
            type: Boolean,
            default: () => defProps.keyboard.overlay
        },
        // z-index值
        zIndex: {
            type: [String, Number],
            default: () => defProps.keyboard.zIndex
        },
        // 取消按鈕的文字
        cancelText: {
            type: String,
            default: () => defProps.keyboard.cancelText
        },
        // 確认按鈕的文字
        confirmText: {
            type: String,
            default: () => defProps.keyboard.confirmText
        },
        // 输入一个中文后，是否自动切換到英文
        autoChange: {
            type: Boolean,
            default: () => defProps.keyboard.autoChange
        }
    }
})
