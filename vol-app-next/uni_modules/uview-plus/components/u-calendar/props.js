import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'

export const props = defineMixin({
    props: {
        // 日历顶部標題
        title: {
            type: String,
            default: () => defProps.calendar.title
        },
        // 是否显示標題
        showTitle: {
            type: Boolean,
            default: () => defProps.calendar.showTitle
        },
        // 是否显示副標題
        showSubtitle: {
            type: Boolean,
            default: () => defProps.calendar.showSubtitle
        },
        // 日期类型選择，single-選择單个日期，multiple-可以選择多个日期，range-選择日期范围
        mode: {
            type: String,
            default: () => defProps.calendar.mode
        },
        // mode=range时，第一个日期底部的提示文字
        startText: {
            type: String,
            default: () => defProps.calendar.startText
        },
        // mode=range时，最后一个日期底部的提示文字
        endText: {
            type: String,
            default: () => defProps.calendar.endText
        },
        // 自定义列表
        customList: {
            type: Array,
            default: () => defProps.calendar.customList
        },
        // 主題色，对底部按鈕和選中日期有效
        color: {
            type: String,
            default: () => defProps.calendar.color
        },
        // 最小的可選日期
        minDate: {
            type: [String, Number],
            default: () => defProps.calendar.minDate
        },
        // 最大可選日期
        maxDate: {
            type: [String, Number],
            default: () => defProps.calendar.maxDate
        },
        // 默认選中的日期，mode為multiple或range是必须為數组格式
        defaultDate: {
            type: [Array, String, Date, null],
            default: () => defProps.calendar.defaultDate
        },
        // mode=multiple时，最多可選多少个日期
        maxCount: {
            type: [String, Number],
            default: () => defProps.calendar.maxCount
        },
        // 日期行高
        rowHeight: {
            type: [String, Number],
            default: () => defProps.calendar.rowHeight
        },
        // 日期格式化函數
        formatter: {
            type: [Function, null],
            default: () => defProps.calendar.formatter
        },
        // 是否显示农历
        showLunar: {
            type: Boolean,
            default: () => defProps.calendar.showLunar
        },
        // 是否显示月份背景色
        showMark: {
            type: Boolean,
            default: () => defProps.calendar.showMark
        },
        // 確定按鈕的文字
        confirmText: {
            type: String,
            default: () => defProps.calendar.confirmText
        },
        // 確认按鈕处于禁用状态时的文字
        confirmDisabledText: {
            type: String,
            default: () => defProps.calendar.confirmDisabledText
        },
        // 是否显示日历彈窗
        show: {
            type: Boolean,
            default: () => defProps.calendar.show
        },
        // 是否允许点击遮罩關闭日历
        closeOnClickOverlay: {
            type: Boolean,
            default: () => defProps.calendar.closeOnClickOverlay
        },
        // 是否為只讀状态，只讀状态下禁止選择日期
        readonly: {
            type: Boolean,
            default: () => defProps.calendar.readonly
        },
        // 	是否展示確认按鈕
        showConfirm: {
            type: Boolean,
            default: () => defProps.calendar.showConfirm
        },
        // 日期區间最多可選天數，默认無限制，mode = range时有效
        maxRange: {
            type: [Number, String],
            default: () => defProps.calendar.maxRange
        },
        // 范围選择超过最多可選天數时的提示文案，mode = range时有效
        rangePrompt: {
            type: String,
            default: () => defProps.calendar.rangePrompt
        },
        // 范围選择超过最多可選天數时，是否展示提示文案，mode = range时有效
        showRangePrompt: {
            type: Boolean,
            default: () => defProps.calendar.showRangePrompt
        },
        // 是否允许日期范围的起止时间為同一天，mode = range时有效
        allowSameDay: {
            type: Boolean,
            default: () => defProps.calendar.allowSameDay
        },
		// 圆角值
		round: {
		    type: [Boolean, String, Number],
		    default: () => defProps.calendar.round
		},
		// 最多展示月份數量
		monthNum: {
			type: [Number, String],
			default: 3
		}	
    }
})
