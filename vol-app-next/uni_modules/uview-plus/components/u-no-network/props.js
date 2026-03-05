import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 页面文字提示
        tips: {
            type: String,
            default: () => defProps.noNetwork.tips
        },
        // 一个z-index值，用于設置没有网络这个组件的层次，因為页面可能会有其他定位的元素层級过高，导致此组件被覆蓋
        zIndex: {
            type: [String, Number],
            default: () => defProps.noNetwork.zIndex
        },
        // image 没有网络的图片提示
        image: {
            type: String,
            default: () => defProps.noNetwork.image
        }
    }
})
