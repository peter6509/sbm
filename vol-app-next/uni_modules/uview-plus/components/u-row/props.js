import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 给col添加间距，左右边距各占一半
        gutter: {
            type: [String, Number],
            default: () => defProps.row.gutter
        },
        // 水平排列方式，可選值為`start`(或`flex-start`)、`end`(或`flex-end`)、`center`、`around`(或`space-around`)、`between`(或`space-between`)
        justify: {
            type: String,
            default: () => defProps.row.justify
        },
        // 垂直对齊方式，可選值為top、center、bottom
        align: {
            type: String,
            default: () => defProps.row.align
        }
    }
})
