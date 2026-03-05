import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 文字颜色
        color: {
            type: String,
            default: () => defProps.link.color
        },
        // 字体大小，單位px
        fontSize: {
            type: [String, Number],
            default: () => defProps.link.fontSize
        },
        // 是否显示下划线
        underLine: {
            type: Boolean,
            default: () => defProps.link.underLine
        },
        // 要跳转的链接
        href: {
            type: String,
            default: () => defProps.link.href
        },
        // 小程序中複制到粘貼板的提示语
        mpTips: {
            type: String,
            default: () => defProps.link.mpTips
        },
        // 下划线颜色
        lineColor: {
            type: String,
            default: () => defProps.link.lineColor
        },
        // 超链接的问題，不使用slot形式傳入，是因為nvue下無法修改颜色
        text: {
            type: String,
            default: () => defProps.link.text
        }
    }
})
