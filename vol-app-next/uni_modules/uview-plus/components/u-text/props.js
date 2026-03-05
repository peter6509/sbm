import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 主題颜色
        type: {
            type: String,
            default: () => defProps.text.type
        },
        // 是否显示
        show: {
            type: Boolean,
            default: () => defProps.text.show
        },
        // 显示的值
        text: {
            type: [String, Number],
            default: () => defProps.text.text
        },
        // 前置图標
        prefixIcon: {
            type: String,
            default: () => defProps.text.prefixIcon
        },
        // 后置图標
        suffixIcon: {
            type: String,
            default: () => defProps.text.suffixIcon
        },
        // 文本处理的匹配模式
        // text-普通文本，price-价格，phone-手机号，name-姓名，date-日期，link-超链接
        mode: {
            type: String,
            default: () => defProps.text.mode
        },
        // mode=link下，配置的链接
        href: {
            type: String,
            default: () => defProps.text.href
        },
        // 格式化规则
        format: {
            type: [String, Function],
            default: () => defProps.text.format
        },
        // mode=phone时，点击文本是否拨打電话
        call: {
            type: Boolean,
            default: () => defProps.text.call
        },
        // 小程序的打开方式
        openType: {
            type: String,
            default: () => defProps.text.openType
        },
        // 是否粗体，默认normal
        bold: {
            type: Boolean,
            default: () => defProps.text.bold
        },
        // 是否块状
        block: {
            type: Boolean,
            default: () => defProps.text.block
        },
        // 文本显示的行數，如果設置，超出此行數，將会显示省略号
        lines: {
            type: [String, Number],
            default: () => defProps.text.lines
        },
        // 文本颜色
        color: {
            type: String,
            default: () => defProps.text.color
        },
        // 字体大小
        size: {
            type: [String, Number],
            default: () => defProps.text.size
        },
        // 图標的樣式
        iconStyle: {
            type: [Object, String],
            default: () => defProps.text.iconStyle
        },
        // 文字裝饰，下划线，中划线等，可選值 none|underline|line-through
        decoration: {
            tepe: String,
            default: () => defProps.text.decoration
        },
        // 外边距，对象、字符串，數值形式均可
        margin: {
            type: [Object, String, Number],
            default: () => defProps.text.margin
        },
        // 文本行高
        lineHeight: {
            type: [String, Number],
            default: () => defProps.text.lineHeight
        },
        // 文本对齊方式，可選值left|center|right
        align: {
            type: String,
            default: () => defProps.text.align
        },
        // 文字換行，可選值break-word|normal|anywhere
        wordWrap: {
            type: String,
            default: () => defProps.text.wordWrap
        }
    }
})
