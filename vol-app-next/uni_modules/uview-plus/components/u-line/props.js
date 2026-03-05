import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        color: {
            type: String,
            default: () => defProps.line.color
        },
        // 長度，竖向时表现為高度，横向时表现為長度，可以為百分比，带px單位的值等
        length: {
            type: [String, Number],
            default: () => defProps.line.length
        },
        // 线条方向，col-竖向，row-横向
        direction: {
            type: String,
            default: () => defProps.line.direction
        },
        // 是否显示细边框
        hairline: {
            type: Boolean,
            default: () => defProps.line.hairline
        },
        // 线条與上下左右元素的间距，字符串形式，如"30px"、"20px 30px"
        margin: {
            type: [String, Number],
            default: () => defProps.line.margin
        },
        // 是否虚线，true-虚线，false-实线
        dashed: {
            type: Boolean,
            default: () => defProps.line.dashed
        }
    }
})
