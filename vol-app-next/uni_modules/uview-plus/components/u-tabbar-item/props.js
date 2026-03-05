import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // item標簽的名稱，作為與u-tabbar的value参數匹配的標识符
        name: {
            type: [String, Number, null],
            default: () => defProps.tabbarItem.name
        },
        // uView内置图標或者绝对路径的图片
        icon: {
            icon: String,
            default: () => defProps.tabbarItem.icon
        },
        // 右上角的角標提示信息
        badge: {
            type: [String, Number, null],
            default: () => defProps.tabbarItem.badge
        },
        // 是否显示圆点，將会覆蓋badge参數
        dot: {
            type: Boolean,
            default: () => defProps.tabbarItem.dot
        },
        // 描述文本
        text: {
            type: String,
            default: () => defProps.tabbarItem.text
        },
        // 控制徽標的位置，对象或者字符串形式，可以設置top和right屬性
        badgeStyle: {
            type: [Object, String],
            default: () => defProps.tabbarItem.badgeStyle
        }

    }
})
