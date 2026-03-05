<template>
	<u-transition
		mode="slide-down"
		:customStyle="containerStyle"
		:show="open"
	>
		<view
			class="u-notify"
			:class="[`u-notify--${tmpConfig.type}`]"
			:style="[backgroundColor, addStyle(customStyle)]"
		>
			<u-status-bar v-if="tmpConfig.safeAreaInsetTop"></u-status-bar>
			<view class="u-notify__warpper">
				<slot name="icon">
					<u-icon
						v-if="['success', 'warning', 'error'].includes(tmpConfig.type)"
						:name="tmpConfig.icon"
						:color="tmpConfig.color"
						:size="1.3 * tmpConfig.fontSize"
						:customStyle="{marginRight: '4px'}"
					></u-icon>
				</slot>
				<text
					class="u-notify__warpper__text"
					:style="{
						fontSize: addUnit(tmpConfig.fontSize),
						color: tmpConfig.color
					}"
				>{{ tmpConfig.message }}</text>
			</view>
		</view>
	</u-transition>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import defProps from '../../libs/config/props.js'
	import { addUnit, addStyle, deepMerge } from '../../libs/function/index';
	/**
	 * notify 顶部提示
	 * @description 該组件一般用于頁面顶部向下滑出一個提示，尔后自動收起的场景
	 * @tutorial
	 * @property {String | Number}	top					到顶部的距离 ( 默認 0 )
	 * @property {String}			type				主題，primary，success，warning，error ( 默認 'primary' )
	 * @property {String}			color				字體顏色 ( 默認 '#ffffff' )
	 * @property {String}			bgColor				背景顏色
	 * @property {String}			message				展示的文字内容
	 * @property {String | Number}	duration			展示時長，為0時不消失，單位ms ( 默認 3000 )
	 * @property {String | Number}	fontSize			字體大小 ( 默認 15 )
	 * @property {Boolean}			safeAreaInsetTop	是否留出顶部安全距离（狀態栏高度） ( 默認 false )
	 * @property {Object}			customStyle			组件的樣式，對象形式
	 * @event {Function}	open	開啟组件時調用的函數
	 * @event {Function}	close	關闭组件式調用的函數
	 * @example <u-notify message="Hi uView"></u-notify>
	 */
	export default {
		name: 'u-notify',
		mixins: [mpMixin, mixin,props],
		data() {
			return {
				// 是否展示组件
				open: false,
				timer: null,
				config: {
					// 到顶部的距离
					top: defProps.notify.top,
					// type主題，primary，success，warning，error
					type: defProps.notify.type,
					// 字體顏色
					color: defProps.notify.color,
					// 背景顏色
					bgColor: defProps.notify.bgColor,
					// 展示的文字内容
					message: defProps.notify.message,
					// 展示時長，為0時不消失，單位ms
					duration: defProps.notify.duration,
					// 字體大小
					fontSize: defProps.notify.fontSize,
					// 是否留出顶部安全距离（狀態栏高度）
					safeAreaInsetTop: defProps.notify.safeAreaInsetTop
				},
				// 合並后的配置，避免多次調用组件后，可能會複用之前使用的配置参數
				tmpConfig: {}
			}
		},
		computed: {
			containerStyle() {
				let top = 0
				if (this.tmpConfig.top === 0) {
					// #ifdef H5
					// H5端，导航栏為普通元素，需要將组件移動到导航栏的下邊沿
					// H5的导航栏高度為44px
					top = 44
					// #endif
				}
				const style = {
					top: addUnit(this.tmpConfig.top === 0 ? top : this.tmpConfig.top),
					// 因為组件底層為u-transition组件，必須將其設置為fixed定位
					// 让其出現在导航栏底部
					position: 'fixed',
					left: 0,
					right: 0,
					zIndex: 10076
				}
				return style
			},
			// 组件背景顏色
			backgroundColor() {
				const style = {}
				if (this.tmpConfig.bgColor) {
					style.backgroundColor = this.tmpConfig.bgColor
				}
				return style
			},
			// 默認主題下的圖標
			icon() {
				let icon
				if (this.tmpConfig.type === 'success') {
					icon = 'checkmark-circle'
				} else if (this.tmpConfig.type === 'error') {
					icon = 'close-circle'
				} else if (this.tmpConfig.type === 'warning') {
					icon = 'error-circle'
				}
				return icon
			}
		},
		created() {
			// 通過主題的形式調用toast，批量生成方法函數
			['primary', 'success', 'error', 'warning'].map(item => {
				this[item] = message => this.show({
					type: item,
					message
				})
			})
		},
		methods: {
			addStyle,
			addUnit,
			show(options) {
				// 不將结果合並到this.config變量，避免多次調用u-toast，前后的配置造成混乱
				this.tmpConfig = deepMerge(this.config, options)
				// 任何定時器初始化之前，都要執行清除操作，否则可能會造成混乱
				this.clearTimer()
				this.open = true
				if (this.tmpConfig.duration > 0) {
					this.timer = setTimeout(() => {
						this.open = false
						// 倒計時结束，清除定時器，隐藏toast组件
						this.clearTimer()
						// 判斷是否存在callback方法，如果存在就執行
						typeof(this.tmpConfig.complete) === 'function' && this.tmpConfig.complete()
					}, this.tmpConfig.duration)
				}
			},
			// 關闭notify
			close() {
				this.clearTimer()
			},
			clearTimer() {
				this.open = false
				// 清除定時器
				clearTimeout(this.timer)
				this.timer = null
			}
		},
		beforeUnmount() {
			this.clearTimer()
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	$u-notify-padding: 8px 10px !default;
	$u-notify-text-font-size: 15px !default;
	$u-notify-primary-bgColor: $u-primary !default;
	$u-notify-success-bgColor: $u-success !default;
	$u-notify-error-bgColor: $u-error !default;
	$u-notify-warning-bgColor: $u-warning !default;


	.u-notify {
		padding: $u-notify-padding;

		&__warpper {
			@include flex;
			align-items: center;
			text-align: center;
			justify-content: center;

			&__text {
				font-size: $u-notify-text-font-size;
				text-align: center;
			}
		}

		&--primary {
			background-color: $u-notify-primary-bgColor;
		}

		&--success {
			background-color: $u-notify-success-bgColor;
		}

		&--error {
			background-color: $u-notify-error-bgColor;
		}

		&--warning {
			background-color: $u-notify-warning-bgColor;
		}
	}
</style>
