import { defineMixin } from '../vue'

export const mpMixin = defineMixin({
    // #ifdef MP-WEIXIN
    // 將自定义节点設置成虚拟的，更加接近Vue组件的表现，能更好的使用flex屬性
    options: {
        virtualHost: true
    }
    // #endif
})

export default mpMixin

