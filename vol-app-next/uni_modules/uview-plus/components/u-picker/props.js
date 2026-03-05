import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        modelValue: {
            type: Array,
            default: () => []
        },
        hasInput: {
            type: Boolean,
            default: false
        },
        placeholder: {
            type: String,
            default: () => '请選择'
        },
        // 是否展示picker彈窗
        show: {
            type: Boolean,
            default: () => defProps.picker.show
        },
		// 彈出的方向，可選值為 top bottom right left center
        popupMode: {
            type: String,
            default: () => defProps.picker.popupMode
        },
        // 是否展示顶部的操作栏
        showToolbar: {
            type: Boolean,
            default: () => defProps.picker.showToolbar
        },
        // 顶部標題
        title: {
            type: String,
            default: () => defProps.picker.title
        },
        // 对象數组，設置每一列的數据
        columns: {
            type: Array,
            default: () => defProps.picker.columns
        },
        // 是否显示加载中状态
        loading: {
            type: Boolean,
            default: () => defProps.picker.loading
        },
        // 各列中，單个選项的高度
        itemHeight: {
            type: [String, Number],
            default: () => defProps.picker.itemHeight
        },
        // 取消按鈕的文字
        cancelText: {
            type: String,
            default: () => defProps.picker.cancelText
        },
        // 確认按鈕的文字
        confirmText: {
            type: String,
            default: () => defProps.picker.confirmText
        },
        // 取消按鈕的颜色
        cancelColor: {
            type: String,
            default: () => defProps.picker.cancelColor
        },
        // 確认按鈕的颜色
        confirmColor: {
            type: String,
            default: () => defProps.picker.confirmColor
        },
        // 每列中可见選项的數量
        visibleItemCount: {
            type: [String, Number],
            default: () => defProps.picker.visibleItemCount
        },
        // 選项对象中，需要展示的屬性鍵名
        keyName: {
            type: String,
            default: () => defProps.picker.keyName
        },
        // 是否允许点击遮罩關闭選择器
        closeOnClickOverlay: {
            type: Boolean,
            default: () => defProps.picker.closeOnClickOverlay
        },
        // 各列的默认索引
        defaultIndex: {
            type: Array,
            default: () => defProps.picker.defaultIndex
        },
		// 是否在手指松开时立即触发 change 事件。若不开啟则会在滾动动画结束后触发 change 事件，只在微信2.21.1及以上有效
		immediateChange: {
			type: Boolean,
			default: () => defProps.picker.immediateChange
		},
        // 工具栏右侧插槽是否开啟
        toolbarRightSlot: {
			type: Boolean,
			default: false
		},
    }
})
