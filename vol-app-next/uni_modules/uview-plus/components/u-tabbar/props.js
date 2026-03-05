import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'

export const props = defineMixin({
    props: {
        // 当前匹配项的name
        value: {
            type: [String, Number, null],
            default: () => defProps.tabbar.value
        },
        // 是否為iPhoneX留出底部安全距离
        safeAreaInsetBottom: {
            type: Boolean,
            default: () => defProps.tabbar.safeAreaInsetBottom
        },
        // 是否显示上方边框
        border: {
            type: Boolean,
            default: () => defProps.tabbar.border
        },
        // 元素层級z-index
        zIndex: {
            type: [String, Number],
            default: () => defProps.tabbar.zIndex
        },
        // 選中標簽的颜色
        activeColor: {
            type: String,
            default: () => defProps.tabbar.activeColor
        },
        // 未選中標簽的颜色
        inactiveColor: {
            type: String,
            default: () => defProps.tabbar.inactiveColor
        },
        // 是否固定在底部
        fixed: {
            type: Boolean,
            default: () => defProps.tabbar.fixed
        },
        // fixed定位固定在底部时，是否生成一个等高元素防止塌陷
        placeholder: {
            type: Boolean,
            default: () => defProps.tabbar.placeholder
        }
    }
})
