import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 是否显示input
        hasInput: {
            type: Boolean,
            default: () => false
        },
        placeholder: {
            type: String,
            default: () => '请選择'
        },
        format: {
            type: String,
            default: () => ''
        },
        // 是否打开组件
        show: {
            type: Boolean,
            default: () => defProps.datetimePicker.show
        },
		// 彈出的方向，可選值為 top bottom right left center
        popupMode: {
            type: String,
            default: () => defProps.picker.popupMode
        },
        // 是否展示顶部的操作栏
        showToolbar: {
            type: Boolean,
            default: () => defProps.datetimePicker.showToolbar
        },
        // 工具栏右侧内容
		toolbarRightSlot:{
			type: Boolean,
			default:  false
		},
        // #ifdef VUE2
        // 绑定值
        value: {
            type: [String, Number],
            default: () => defProps.datetimePicker.value
        },
        // #endif
        // #ifdef VUE3
        // 绑定值
        modelValue: {
            type: [String, Number],
            default: () => defProps.datetimePicker.value
        },
        // #endif
        // 顶部標題
        title: {
            type: String,
            default: () => defProps.datetimePicker.title
        },
        // 展示格式，mode=date為日期選择，mode=time為时间選择，mode=year-month為年月選择，mode=datetime為日期时间選择
        mode: {
            type: String,
            default: () => defProps.datetimePicker.mode
        },
        // 可選的最大时间
        maxDate: {
            type: Number,
            // 最大默认值為后10年
            default: () => defProps.datetimePicker.maxDate
        },
        // 可選的最小时间
        minDate: {
            type: Number,
            // 最小默认值為前10年
            default: () => defProps.datetimePicker.minDate
        },
        // 可選的最小小时，仅mode=time有效
        minHour: {
            type: Number,
            default: () => defProps.datetimePicker.minHour
        },
        // 可選的最大小时，仅mode=time有效
        maxHour: {
            type: Number,
            default: () => defProps.datetimePicker.maxHour
        },
        // 可選的最小分钟，仅mode=time有效
        minMinute: {
            type: Number,
            default: () => defProps.datetimePicker.minMinute
        },
        // 可選的最大分钟，仅mode=time有效
        maxMinute: {
            type: Number,
            default: () => defProps.datetimePicker.maxMinute
        },
        // 選项过濾函數
        filter: {
            type: [Function, null],
            default: () => defProps.datetimePicker.filter
        },
        // 選项格式化函數
        formatter: {
            type: [Function, null],
            default: () => defProps.datetimePicker.formatter
        },
        // 是否显示加载中状态
        loading: {
            type: Boolean,
            default: () => defProps.datetimePicker.loading
        },
        // 各列中，單个選项的高度
        itemHeight: {
            type: [String, Number],
            default: () => defProps.datetimePicker.itemHeight
        },
        // 取消按鈕的文字
        cancelText: {
            type: String,
            default: () => defProps.datetimePicker.cancelText
        },
        // 確认按鈕的文字
        confirmText: {
            type: String,
            default: () => defProps.datetimePicker.confirmText
        },
        // 取消按鈕的颜色
        cancelColor: {
            type: String,
            default: () => defProps.datetimePicker.cancelColor
        },
        // 確认按鈕的颜色
        confirmColor: {
            type: String,
            default: () => defProps.datetimePicker.confirmColor
        },
        // 每列中可见選项的數量
        visibleItemCount: {
            type: [String, Number],
            default: () => defProps.datetimePicker.visibleItemCount
        },
        // 是否允许点击遮罩關闭選择器
        closeOnClickOverlay: {
            type: Boolean,
            default: () => defProps.datetimePicker.closeOnClickOverlay
        },
        // 各列的默认索引
        defaultIndex: {
            type: Array,
            default: () => defProps.datetimePicker.defaultIndex
        }
    }
})
