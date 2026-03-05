import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 控制是否出现滾动条，仅nvue有效
        showScrollbar: {
            type: Boolean,
            default: () => defProps.list.showScrollbar
        },
        // 距底部多少时触发scrolltolower事件
        lowerThreshold: {
            type: [String, Number],
            default: () => defProps.list.lowerThreshold
        },
        // 距顶部多少时触发scrolltoupper事件，非nvue有效
        upperThreshold: {
            type: [String, Number],
            default: () => defProps.list.upperThreshold
        },
        // 設置竖向滾动条位置
        scrollTop: {
            type: [String, Number],
            default: () => defProps.list.scrollTop
        },
        // 控制 onscroll 事件触发的频率，仅nvue有效
        offsetAccuracy: {
            type: [String, Number],
            default: () => defProps.list.offsetAccuracy
        },
        // 啟用 flexbox 布局。开啟后，当前节点声明了display: flex就会成為flex container，并作用于其孩子节点，仅微信小程序有效
        enableFlex: {
            type: Boolean,
            default: () => defProps.list.enableFlex
        },
        // 是否按分页模式显示List，默认值false
        pagingEnabled: {
            type: Boolean,
            default: () => defProps.list.pagingEnabled
        },
        // 是否允许List滾动
        scrollable: {
            type: Boolean,
            default: () => defProps.list.scrollable
        },
        // 值应為某子元素id（id不能以數字开头）
        scrollIntoView: {
            type: String,
            default: () => defProps.list.scrollIntoView
        },
        // 在設置滾动条位置时使用动画过渡
        scrollWithAnimation: {
            type: Boolean,
            default: () => defProps.list.scrollWithAnimation
        },
        // iOS点击顶部状态栏、安卓双击標題栏时，滾动条返回顶部，只对微信小程序有效
        enableBackToTop: {
            type: Boolean,
            default: () => defProps.list.enableBackToTop
        },
        // 列表的高度
        height: {
            type: [String, Number],
            default: () => defProps.list.height
        },
        // 列表寬度
        width: {
            type: [String, Number],
            default: () => defProps.list.width
        },
        // 列表前后預渲染的屏數，1代表一个屏幕的高度，1.5代表1个半屏幕高度
        preLoadScreen: {
            type: [String, Number],
            default: () => defProps.list.preLoadScreen
        },
        // 开啟自定义下拉刷新
        refresherEnabled: {
            type: Boolean,
            default: () => false
        },
        // 設置自定义下拉刷新阈值	
        refresherThreshold: {
            type: Number,
            default: () => 45
        },
        // 設置自定义下拉刷新默认樣式，支持設置 black，white，none，none 表示不使用默认樣式
        refresherDefaultStyle: {
            type: String,
            default: () => 'black'
        },
        // 設置自定义下拉刷新區域背景颜色
        refresherBackground: {
            type: String,
            default: () => '#FFF'
        },
        // 設置当前下拉刷新状态，true 表示下拉刷新已經被触发，false 表示下拉刷新未被触发
        refresherTriggered: {
            type: Boolean,
            default: () => false
        }
    }
})
