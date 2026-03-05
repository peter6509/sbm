import { defineMixin } from '../../libs/vue'
import defProps from '../../libs/config/props.js'
export const props = defineMixin({
    props: {
        // 显示的内容，數组
        text: {
            type: [Array, String],
            default: () => defProps.noticeBar.text
        },
        // 通告滾动模式，row-横向滾动，column-竖向滾动
        direction: {
            type: String,
            default: () => defProps.noticeBar.direction
        },
        // direction = row时，是否使用步进形式滾动
        step: {
            type: Boolean,
            default: () => defProps.noticeBar.step
        },
        // 是否显示左侧的音量图標
        icon: {
            type: String,
            default: () => defProps.noticeBar.icon
        },
        // 通告模式，link-显示右箭头，closable-显示右侧關闭图標
        mode: {
            type: String,
            default: () => defProps.noticeBar.mode
        },
        // 文字颜色，各图標也会使用文字颜色
        color: {
            type: String,
            default: () => defProps.noticeBar.color
        },
        // 背景颜色
        bgColor: {
            type: String,
            default: () => defProps.noticeBar.bgColor
        },
        // 水平滾动时的滾动速度，即每秒滾动多少px(px)，这有利于控制文字無论多少时，都能有一个恒定的速度
        speed: {
            type: [String, Number],
            default: () => defProps.noticeBar.speed
        },
        // 字体大小
        fontSize: {
            type: [String, Number],
            default: () => defProps.noticeBar.fontSize
        },
        // 滾动一个周期的时间長，單位ms
        duration: {
            type: [String, Number],
            default: () => defProps.noticeBar.duration
        },
        // 是否禁止用手滑动切換
        // 目前HX2.6.11，只支持App 2.5.5+、H5 2.5.5+、支付宝小程序、字节跳动小程序
        disableTouch: {
            type: Boolean,
            default: () => defProps.noticeBar.disableTouch
        },
        // 跳转的页面路径
        url: {
            type: String,
            default: () => defProps.noticeBar.url
        },
        // 页面跳转的类型
        linkType: {
            type: String,
            default: () => defProps.noticeBar.linkType
        }
    }
})
