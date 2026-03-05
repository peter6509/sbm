import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 图片地址，Array<String>|Array<Object>形式
        urls: {
            type: Array,
            default: () => defProps.album.urls
        },
        // 指定从數组的对象元素中讀取哪个屬性作為图片地址
        keyName: {
            type: String,
            default: () => defProps.album.keyName
        },
        // 單图时，图片長边的長度
        singleSize: {
            type: [String, Number],
            default: () => defProps.album.singleSize
        },
        // 多图时，图片边長
        multipleSize: {
            type: [String, Number],
            default: () => defProps.album.multipleSize
        },
        // 多图时，图片水平和垂直之间的间隔
        space: {
            type: [String, Number],
            default: () => defProps.album.space
        },
        // 單图时，图片缩放裁剪的模式
        singleMode: {
            type: String,
            default: () => defProps.album.singleMode
        },
        // 多图时，图片缩放裁剪的模式
        multipleMode: {
            type: String,
            default: () => defProps.album.multipleMode
        },
        // 最多展示的图片數量，超出时最后一个位置將会显示剩餘图片數量
        maxCount: {
            type: [String, Number],
            default: () => defProps.album.maxCount
        },
        // 是否可以預览图片
        previewFullImage: {
            type: Boolean,
            default: () => defProps.album.previewFullImage
        },
        // 每行展示图片數量，如設置，singleSize和multipleSize將会無效
        rowCount: {
            type: [String, Number],
            default: () => defProps.album.rowCount
        },
        // 超出maxCount时是否显示查看更多的提示
        showMore: {
            type: Boolean,
            default: () => defProps.album.showMore
        },
        // 图片形状，circle-圆形，square-方形
        shape: {
            type: String,
            default: () => defProps.image.shape
        },
        // 圆角，單位任意
        radius: {
            type: [String, Number],
            default: () => defProps.image.radius
        },
        // 自適应換行
        autoWrap: {
            type: Boolean,
            default: () => defProps.album.autoWrap
        },
        // 單位
        unit: {
            type: [String],
            default: () => defProps.album.unit
        },
        // 阻止点击冒泡
        stop: {
            type: Boolean,
            default: () => defProps.album.stop
        }
    }
})
