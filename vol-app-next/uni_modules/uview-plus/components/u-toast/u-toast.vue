<template>
	<view class="u-toast">
		<u-overlay
			:show="isShow"
			:zIndex="tmpConfig.overlay ? 10070 : -1"
			:custom-style="overlayStyle"
		>
			<view
				class="u-toast__content"
				:style="[contentStyle]"
				:class="['u-type-' + tmpConfig.type, (tmpConfig.type === 'loading' || tmpConfig.loading) ?  'u-toast__content--loading' : '']"
			>
				<u-loading-icon
					v-if="tmpConfig.type === 'loading'"
					mode="circle"
					color="rgb(255, 255, 255)"
					inactiveColor="rgb(120, 120, 120)"
					size="25"
				></u-loading-icon>
				<u-icon
					v-else-if="tmpConfig.type !== 'defalut' && iconName"
					:name="iconName"
					size="17"
					:color="tmpConfig.type"
					:customStyle="iconStyle"
				></u-icon>
				<u-gap
					v-if="tmpConfig.type === 'loading' || tmpConfig.loading"
					height="12"
					bgColor="transparent"
				></u-gap>
				<text
					class="u-toast__content__text"
					:class="['u-toast__content__text--' + tmpConfig.type]"
					style="max-width: 400rpx;"
				>{{ tmpConfig.message }}</text>
			</view>
		</u-overlay>
	</view>
</template>

<script>
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { os, sys, deepMerge, type2icon } from '../../libs/function/index';
	import color from '../../libs/config/color';
	import { hexToRgb } from '../../libs/function/colorGradient';
	/**
	 * toast 消息提示
	 * @description 此组件表現形式類似uni的uni.showToastAPI，但也有不同的地方。
	 * @tutorial https://ijry.github.io/uview-plus/components/toast.html
	 * @property {String | Number}	zIndex		toast展示時的zIndex值 (默認 10090 )
	 * @property {Boolean}			loading		是否加載中 （默認 false ）
	 * @property {String | Number}	message		顯示的文字内容
	 * @property {String}			icon		圖標，或者绝對路徑的圖片
	 * @property {String}			type		主題類型 （默認 default）
	 * @property {Boolean}			show		是否顯示該组件 （默認 false）
	 * @property {Boolean}			overlay		是否顯示透明遮罩，防止點擊穿透 （默認 true ）
	 * @property {String}			position	位置 （默認 'center' ）
	 * @property {Object}			params		跳轉的参數 
	 * @property {String | Number}  duration	展示時間，單位ms （默認 2000 ）
	 * @property {Boolean}			isTab		是否返回的為tab頁面 （默認 false ）
	 * @property {String}			url			toast消失后是否跳轉頁面，有则跳轉，优先級高于back参數 
	 * @property {Function}			complete	執行完后的回調函數 
	 * @property {Boolean}			back		结束toast是否自動返回上一頁 （默認 false ）
	 * @property {Object}			customStyle	组件的樣式，對象形式
	 * @event {Function} show 顯示toast，如需一进入頁面就顯示toast，請在onReady生命周期調用
	 * @example <u-toast ref="uToast" />
	 */
	export default {
		name: 'u-toast',
		mixins: [mpMixin, mixin],
		data() {
			return {
				isShow: false,
				timer: null, // 定時器
				config: {
					message: '', // 顯示文本
					type: '', // 主題類型，primary，success，error，warning，black
					duration: 2000, // 顯示的時間，毫秒
					icon: true, // 顯示的圖標
					position: 'center', // toast出現的位置
					complete: null, // 執行完后的回調函數
					overlay: true, // 是否防止触摸穿透
					loading: false, // 是否加載中狀態
				},
				tmpConfig: {}, // 將用户配置和内置配置合並后的临時配置變量
			}
		},
		computed: {
			iconName() {
				// 只有不為none，並且type為error|warning|succes|info時候，才顯示圖標
				if(!this.tmpConfig.icon || this.tmpConfig.icon == 'none') {
					return '';
				}
				if (this.tmpConfig.icon === true) {
					if (['error', 'warning', 'success', 'primary'].includes(this.tmpConfig.type)) {
						return type2icon(this.tmpConfig.type)
					} else {
						return ''
					}
				} else {
					return this.tmpConfig.icon
				}
			},
			overlayStyle() {
				const style = {
					justifyContent: 'center',
					alignItems: 'center',
					display: 'flex'
				}
				// 將遮罩設置為100%透明度，避免出現灰色背景
				style.backgroundColor = 'rgba(0, 0, 0, 0)'
				return style
			},
			iconStyle() {
				const style = {}
				// 圖標需要一個右邊距，以跟右邊的文字有隔開的距离
				style.marginRight = '4px'
				// #ifdef APP-NVUE
				// iOSAPP下，圖標有1px的向下偏移，這里进行修正
				if (os() === 'ios') {
					style.marginTop = '-1px'
				}
				// #endif
				return style
			},
			loadingIconColor() {
				let colorTmp = 'rgb(255, 255, 255)'
				if (['error', 'warning', 'success', 'primary'].includes(this.tmpConfig.type)) {
					// loading-icon组件内部會對color参數进行一個透明度處理，該方法要求傳入的顏色值
					// 必須為rgb格式的，所以這里做一個處理
					colorTmp = hexToRgb(color[this.tmpConfig.type])
				}
				return colorTmp
			},
			// 内容盒子的樣式
			contentStyle() {
				const windowHeight = sys().windowHeight, style = {}
				let value = 0
				// 根據top和bottom，對Y軸进行窗體高度的百分比偏移
				if(this.tmpConfig.position === 'top') {
					value = - windowHeight * 0.25
				} else if(this.tmpConfig.position === 'bottom') {
					value = windowHeight * 0.25
				}
				style.transform = `translateY(${value}px)`
				return style
			}
		},
		created() {
			// 通過主題的形式調用toast，批量生成方法函數
			['primary', 'success', 'error', 'warning', 'default', 'loading'].map(item => {
				this[item] = message => this.show({
					type: item,
					message
				})
			})
		},
		methods: {
			// 顯示toast组件，由父组件通過this.$refs.xxx.show(options)形式調用
			show(options) {
				// 不將结果合並到this.config變量，避免多次調用u-toast，前后的配置造成混乱
				this.tmpConfig = deepMerge(this.config, options)
				// 清除定時器
				this.clearTimer()
				this.isShow = true
				// -1時不自動關闭
				if (this.tmpConfig.duration !== -1) {
					this.timer = setTimeout(() => {
						// 倒計時结束，清除定時器，隐藏toast组件
						this.clearTimer()
						// 判斷是否存在callback方法，如果存在就執行
						typeof(this.tmpConfig.complete) === 'function' && this.tmpConfig.complete()
					}, this.tmpConfig.duration)
				}
			},
			// 隐藏toast组件，由父组件通過this.$refs.xxx.hide()形式調用
			hide() {
				this.clearTimer()
			},
			clearTimer() {
				this.isShow = false
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

	$u-toast-color:#fff !default;
	$u-toast-border-radius:4px !default;
	$u-toast-border-background-color:#585858 !default;
	$u-toast-border-font-size:14px !default;
	$u-toast-border-padding:12px 20px !default;
	$u-toast-loading-border-padding: 20px 20px !default;
	$u-toast-content-text-color:#fff !default;
	$u-toast-content-text-font-size:15px !default;
	$u-toast-u-icon:10rpx !default;
	$u-toast-u-type-primary-color:$u-primary !default;
	$u-toast-u-type-primary-background-color:#ecf5ff !default;
	$u-toast-u-type-primary-border-color:rgb(215, 234, 254) !default;
	$u-toast-u-type-primary-border-width:1px !default;
	$u-toast-u-type-success-color: $u-success !default;
	$u-toast-u-type-success-background-color: #dbf1e1 !default;
	$u-toast-u-type-success-border-color: #BEF5C8 !default;
	$u-toast-u-type-success-border-width: 1px !default;
	$u-toast-u-type-error-color:$u-error !default;
	$u-toast-u-type-error-background-color:#fef0f0 !default;
	$u-toast-u-type-error-border-color:#fde2e2 !default;
	$u-toast-u-type-error-border-width: 1px !default;
	$u-toast-u-type-warning-color:$u-warning !default;
	$u-toast-u-type-warning-background-color:#fdf6ec !default;
	$u-toast-u-type-warning-border-color:#faecd8 !default;
	$u-toast-u-type-warning-border-width: 1px !default;
	$u-toast-u-type-default-color:#fff !default;
	$u-toast-u-type-default-background-color:#585858 !default;

	.u-toast {
		&__content {
			@include flex;
			padding: $u-toast-border-padding;
			border-radius: $u-toast-border-radius;
			background-color: $u-toast-border-background-color;
			color: $u-toast-color;
			align-items: center;
			/* #ifndef APP-NVUE */
			max-width: 600rpx;
			/* #endif */
			position: relative;

			&--loading {
				flex-direction: column;
				padding: $u-toast-loading-border-padding;
			}

			&__text {
				color: $u-toast-content-text-color;
				font-size: $u-toast-content-text-font-size;
				line-height: $u-toast-content-text-font-size;

				&--default {
					color: $u-toast-content-text-color;
				}

				&--error {
					color: $u-error;
				}

				&--primary {
					color: $u-primary;
				}

				&--success {
					color: $u-success;
				}

				&--warning {
					color: $u-warning;
				}
			}
		}
	}

	.u-type-primary {
		color: $u-toast-u-type-primary-color;
		background-color: $u-toast-u-type-primary-background-color;
		border-color: $u-toast-u-type-primary-border-color;
		border-width: $u-toast-u-type-primary-border-width;
	}

	.u-type-success {
		color: $u-toast-u-type-success-color;
		background-color: $u-toast-u-type-success-background-color;
		border-color: $u-toast-u-type-success-border-color;
		border-width: 1px;
	}

	.u-type-error {
		color: $u-toast-u-type-error-color;
		background-color: $u-toast-u-type-error-background-color;
		border-color: $u-toast-u-type-error-border-color;
		border-width: $u-toast-u-type-error-border-width;
	}

	.u-type-warning {
		color: $u-toast-u-type-warning-color;
		background-color: $u-toast-u-type-warning-background-color;
		border-color: $u-toast-u-type-warning-border-color;
		border-width: 1px;
	}

	.u-type-default {
		color: $u-toast-u-type-default-color;
		background-color: $u-toast-u-type-default-background-color;
	}
</style>
