import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 图標类名
        name: {
            type: String,
            default: () => defProps.icon.name
        },
        // 图標颜色，可接受主題色
        color: {
            type: String,
            default: () => defProps.icon.color
        },
        // 字体大小，單位px
        size: {
            type: [String, Number],
            default: () => defProps.icon.size
        },
        // 是否显示粗体
        bold: {
            type: Boolean,
            default: () => defProps.icon.bold
        },
        // 点击图標的时候傳递事件出去的index（用于區分点击了哪一个）
        index: {
            type: [String, Number],
            default: () => defProps.icon.index
        },
        // 触摸图標时的类名
        hoverClass: {
            type: String,
            default: () => defProps.icon.hoverClass
        },
        // 自定义扩展前缀，方便用户扩展自己的图標庫
        customPrefix: {
            type: String,
            default: () => defProps.icon.customPrefix
        },
        // 图標右边或者下面的文字
        label: {
            type: [String, Number],
            default: () => defProps.icon.label
        },
        // label的位置，只能右边或者下边
        labelPos: {
            type: String,
            default: () => defProps.icon.labelPos
        },
        // label的大小
        labelSize: {
            type: [String, Number],
            default: () => defProps.icon.labelSize
        },
        // label的颜色
        labelColor: {
            type: String,
            default: () => defProps.icon.labelColor
        },
        // label與图標的距离
        space: {
            type: [String, Number],
            default: () => defProps.icon.space
        },
        // 图片的mode
        imgMode: {
            type: String,
            default: () => defProps.icon.imgMode
        },
        // 用于显示图片小图標时，图片的寬度
        width: {
            type: [String, Number],
            default: () => defProps.icon.width
        },
        // 用于显示图片小图標时，图片的高度
        height: {
            type: [String, Number],
            default: () => defProps.icon.height
        },
        // 用于解决某些情况下，让图標垂直居中的用途
        top: {
            type: [String, Number],
            default: () => defProps.icon.top
        },
        // 是否阻止事件傳播
        stop: {
            type: Boolean,
            default: () => defProps.icon.stop
        }
    }
})
