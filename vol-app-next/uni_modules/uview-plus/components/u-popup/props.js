import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 是否展示彈窗
        show: {
            type: Boolean,
            default: () => defProps.popup.show
        },
        // 是否显示遮罩
        overlay: {
            type: Boolean,
            default: () => defProps.popup.overlay
        },
        // 彈出的方向，可選值為 top bottom right left center
        mode: {
            type: String,
            default: () => defProps.popup.mode
        },
        // 动画时長，單位ms
        duration: {
            type: [String, Number],
            default: () => defProps.popup.duration
        },
        // 是否显示關闭图標
        closeable: {
            type: Boolean,
            default: () => defProps.popup.closeable
        },
        // 自定义遮罩的樣式
        overlayStyle: {
            type: [Object, String],
            default: () => defProps.popup.overlayStyle
        },
        // 点击遮罩是否關闭彈窗
        closeOnClickOverlay: {
            type: Boolean,
            default: () => defProps.popup.closeOnClickOverlay
        },
        // 层級
        zIndex: {
            type: [String, Number],
            default: () => defProps.popup.zIndex
        },
        // 是否為iPhoneX留出底部安全距离
        safeAreaInsetBottom: {
            type: Boolean,
            default: () => defProps.popup.safeAreaInsetBottom
        },
        // 是否留出顶部安全距离（状态栏高度）
        safeAreaInsetTop: {
            type: Boolean,
            default: () => defProps.popup.safeAreaInsetTop
        },
        // 自定义關闭图標位置，top-left為左上角，top-right為右上角，bottom-left為左下角，bottom-right為右下角
        closeIconPos: {
            type: String,
            default: () => defProps.popup.closeIconPos
        },
        // 是否显示圆角
        round: {
            type: [Boolean, String, Number],
            default: () => defProps.popup.round
        },
        // mode=center，也即中部彈出时，是否使用缩放模式
        zoom: {
            type: Boolean,
            default: () => defProps.popup.zoom
        },
        // 彈窗背景色，設置為transparent可去除白色背景
        bgColor: {
            type: String,
            default: () => defProps.popup.bgColor
        },
        // 遮罩的透明度，0-1之间
        overlayOpacity: {
            type: [Number, String],
            default: () => defProps.popup.overlayOpacity
        }
    }
})
