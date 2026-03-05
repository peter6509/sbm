import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 是否自动關闭其他swipe按鈕组
        autoClose: {
            type: Boolean,
            default: () => defProps.swipeAction.autoClose
        },
        // 是否存在打开的按鈕组
        opendItem: {
            type: Boolean,
            default: false
        }
    }
})
