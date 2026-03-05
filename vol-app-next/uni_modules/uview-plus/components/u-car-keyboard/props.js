import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'

export const props = defineMixin({
    props: {
        // 是否打乱鍵盘按鍵的顺序
        random: {
            type: Boolean,
            default: false
        },
        // 输入一个中文后，是否自动切換到英文
        autoChange: {
            type: Boolean,
            default: false
        }
    }
})
