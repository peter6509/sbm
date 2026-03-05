import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // #ifdef VUE3
        // 当前選中项的value值
        modelValue: {
            type: [Number, String, Array],
            default: ''
        },
		// #endif
		// #ifdef VUE2
		// 当前選中项的value值
        value: {
            type: [Number, String, Array],
            default: ''
        },
		// #endif
        // 菜單项標題
        title: {
            type: [String, Number],
            default: ''
        },
        // 選项數据，如果傳入了默认slot，此参數無效
        options: {
            type: Array,
            default() {
                return []
            }
        },
        // 是否禁用此菜單项
        disabled: {
            type: Boolean,
            default: false
        },
        // 下拉彈窗的高度
        height: {
            type: [Number, String],
            default: 'auto'
        },
        // 点击遮罩是否可以收起彈窗
        closeOnClickOverlay: {
            type: Boolean,
            default: true
        }
    }
})
