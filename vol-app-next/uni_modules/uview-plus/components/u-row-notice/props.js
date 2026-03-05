import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 显示的内容，字符串
        text: {
            type: String,
            default: () => defProps.rowNotice.text
        },
        // 是否显示左侧的音量图標
        icon: {
            type: String,
            default: () => defProps.rowNotice.icon
        },
        // 通告模式，link-显示右箭头，closable-显示右侧關闭图標
        mode: {
            type: String,
            default: () => defProps.rowNotice.mode
        },
        // 文字颜色，各图標也会使用文字颜色
        color: {
            type: String,
            default: () => defProps.rowNotice.color
        },
        // 背景颜色
        bgColor: {
            type: String,
            default: () => defProps.rowNotice.bgColor
        },
        // 字体大小，單位px
        fontSize: {
            type: [String, Number],
            default: () => defProps.rowNotice.fontSize
        },
        // 水平滾动时的滾动速度，即每秒滾动多少px(rpx)，这有利于控制文字無论多少时，都能有一个恒定的速度
        speed: {
            type: [String, Number],
            default: () => defProps.rowNotice.speed
        }
    }
})
