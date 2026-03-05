import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 鍵盘的类型，number-數字鍵盘，card-身份证鍵盘
        mode: {
            type: String,
            default: () => defProps.numberKeyboard.value
        },
        // 是否显示鍵盘的"."符号
        dotDisabled: {
            type: Boolean,
            default: () => defProps.numberKeyboard.dotDisabled
        },
        // 是否打乱鍵盘按鍵的顺序
        random: {
            type: Boolean,
            default: () => defProps.numberKeyboard.random
        }
    }
})
