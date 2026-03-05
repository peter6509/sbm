import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 滑块的移动过渡时间，單位ms
        duration: {
            type: Number,
            default: () => defProps.tabs.duration
        },
        // tabs標簽數组
        list: {
            type: Array,
            default: () => defProps.tabs.list
        },
        // 滑块颜色
        lineColor: {
            type: String,
            default: () => defProps.tabs.lineColor
        },
        // 菜單選择中时的樣式
        activeStyle: {
            type: [String, Object],
            default: () => defProps.tabs.activeStyle
        },
        // 菜單非選中时的樣式
        inactiveStyle: {
            type: [String, Object],
            default: () => defProps.tabs.inactiveStyle
        },
        // 滑块長度
        lineWidth: {
            type: [String, Number],
            default: () => defProps.tabs.lineWidth
        },
        // 滑块高度
        lineHeight: {
            type: [String, Number],
            default: () => defProps.tabs.lineHeight
        },
        // 滑块背景显示大小，当滑块背景設置為图片时使用
        lineBgSize: {
            type: String,
            default: () => defProps.tabs.lineBgSize
        },
        // 菜單item的樣式
        itemStyle: {
            type: [String, Object],
            default: () => defProps.tabs.itemStyle
        },
        // 菜單是否可滾动
        scrollable: {
            type: Boolean,
            default: () => defProps.tabs.scrollable
        },
		// 当前選中標簽的索引
		current: {
			type: [Number, String],
			default: () => defProps.tabs.current
		},
		// 默认讀取的鍵名
		keyName: {
			type: String,
			default: () => defProps.tabs.keyName
		}
    }
})
