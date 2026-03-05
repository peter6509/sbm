import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 开始的數值，默认从0增長到某一个數
        startVal: {
            type: [String, Number],
            default: () => defProps.countTo.startVal
        },
        // 要滾动的目標數值，必须
        endVal: {
            type: [String, Number],
            default: () => defProps.countTo.endVal
        },
        // 滾动到目標數值的动画持续时间，單位為毫秒（ms）
        duration: {
            type: [String, Number],
            default: () => defProps.countTo.duration
        },
        // 設置數值后是否自动开始滾动
        autoplay: {
            type: Boolean,
            default: () => defProps.countTo.autoplay
        },
        // 要显示的小數位數
        decimals: {
            type: [String, Number],
            default: () => defProps.countTo.decimals
        },
        // 是否在即將到达目標數值的时候，使用缓慢滾动的效果
        useEasing: {
            type: Boolean,
            default: () => defProps.countTo.useEasing
        },
        // 十进制分割
        decimal: {
            type: [String, Number],
            default: () => defProps.countTo.decimal
        },
        // 字体颜色
        color: {
            type: String,
            default: () => defProps.countTo.color
        },
        // 字体大小
        fontSize: {
            type: [String, Number],
            default: () => defProps.countTo.fontSize
        },
        // 是否加粗字体
        bold: {
            type: Boolean,
            default: () => defProps.countTo.bold
        },
        // 千位分隔符，类似金额的分割(￥23,321.05中的",")
        separator: {
            type: String,
            default: () => defProps.countTo.separator
        }
    }
})
