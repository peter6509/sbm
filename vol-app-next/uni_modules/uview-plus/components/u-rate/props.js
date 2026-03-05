import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // #ifdef VUE3
        // 用于v-model双向绑定選中的星星數量
        modelValue: {
            type: [String, Number],
            default: () => defProps.rate.value
        },
        // #endif
        // #ifdef VUE2
        // 用于v-model双向绑定選中的星星數量
        value: {
            type: [String, Number],
            default: () => defProps.rate.value
        },
        // #endif
        // 要显示的星星數量
        count: {
            type: [String, Number],
            default: () => defProps.rate.count
        },
        // 是否不可選中
        disabled: {
            type: Boolean,
            default: () => defProps.rate.disabled
        },
        // 是否只讀
        readonly: {
            type: Boolean,
            default: () => defProps.rate.readonly
        },
        // 星星的大小，單位px
        size: {
            type: [String, Number],
            default: () => defProps.rate.size
        },
        // 未選中时的颜色
        inactiveColor: {
            type: String,
            default: () => defProps.rate.inactiveColor
        },
        // 選中的颜色
        activeColor: {
            type: String,
            default: () => defProps.rate.activeColor
        },
        // 星星之间的间距，單位px
        gutter: {
            type: [String, Number],
            default: () => defProps.rate.gutter
        },
        // 最少能選择的星星个數
        minCount: {
            type: [String, Number],
            default: () => defProps.rate.minCount
        },
        // 是否允许半星
        allowHalf: {
            type: Boolean,
            default: () => defProps.rate.allowHalf
        },
        // 選中时的图標(星星)
        activeIcon: {
            type: String,
            default: () => defProps.rate.activeIcon
        },
        // 未選中时的图標(星星)
        inactiveIcon: {
            type: String,
            default: () => defProps.rate.inactiveIcon
        },
        // 是否可以通过滑动手势選择评分
        touchable: {
            type: Boolean,
            default: () => defProps.rate.touchable
        }
    }
})
