import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 接受的文件类型, 可選值為all media image file video
        accept: {
            type: String,
            default: () => defProps.upload.accept
        },
        extension: {
            type: Array,
            default: () => defProps.upload.extension
        },
        // 	图片或视频拾取模式，当accept為image类型时設置capture可選额外camera可以直接调起摄像头
        capture: {
            type: [String, Array],
            default: () => defProps.upload.capture
        },
        // 当accept為video时生效，是否压缩视频，默认為true
        compressed: {
            type: Boolean,
            default: () => defProps.upload.compressed
        },
        // 当accept為video时生效，可選值為back或front
        camera: {
            type: String,
            default: () => defProps.upload.camera
        },
        // 当accept為video时生效，拍摄视频最長拍摄时间，單位秒
        maxDuration: {
            type: Number,
            default: () => defProps.upload.maxDuration
        },
        // 上傳區域的图標，只能内置图標
        uploadIcon: {
            type: String,
            default: () => defProps.upload.uploadIcon
        },
        // 上傳區域的图標的颜色，默认
        uploadIconColor: {
            type: String,
            default: () => defProps.upload.uploadIconColor
        },
        // 是否开啟文件讀取前事件
        useBeforeRead: {
            type: Boolean,
            default: () => defProps.upload.useBeforeRead
        },
        // 讀取后的处理函數
        afterRead: {
            type: Function,
            default: null
        },
        // 讀取前的处理函數
        beforeRead: {
            type: Function,
            default: null
        },
        // 是否显示组件自带的图片預览功能
        previewFullImage: {
            type: Boolean,
            default: () => defProps.upload.previewFullImage
        },
        // 最大上傳數量
        maxCount: {
            type: [String, Number],
            default: () => defProps.upload.maxCount
        },
        // 是否啟用
        disabled: {
            type: Boolean,
            default: () => defProps.upload.disabled
        },
        // 預览上傳的图片时的裁剪模式，和image组件mode屬性一致
        imageMode: {
            type: String,
            default: () => defProps.upload.imageMode
        },
        // 標识符，可以在回调函數的第二项参數中获取
        name: {
            type: String,
            default: () => defProps.upload.name
        },
        // 所選的图片的尺寸, 可選值為original compressed
        sizeType: {
            type: Array,
            default: () => defProps.upload.sizeType
        },
        // 是否开啟图片多選，部分安卓机型不支持
        multiple: {
            type: Boolean,
            default: () => defProps.upload.multiple
        },
        // 是否展示删除按鈕
        deletable: {
            type: Boolean,
            default: () => defProps.upload.deletable
        },
        // 文件大小限制，單位為byte
        maxSize: {
            type: [String, Number],
            default: () => defProps.upload.maxSize
        },
        // 显示已上傳的文件列表
        fileList: {
            type: Array,
            default: () => defProps.upload.fileList
        },
        // 上傳區域的提示文字
        uploadText: {
            type: String,
            default: () => defProps.upload.uploadText
        },
        // 内部預览图片區域和選择图片按鈕的區域寬度
        width: {
            type: [String, Number],
            default: () => defProps.upload.width
        },
        // 内部預览图片區域和選择图片按鈕的區域高度
        height: {
            type: [String, Number],
            default: () => defProps.upload.height
        },
        // 是否在上傳完成后展示預览图
        previewImage: {
            type: Boolean,
            default: () => defProps.upload.previewImage
        }
    }
})
