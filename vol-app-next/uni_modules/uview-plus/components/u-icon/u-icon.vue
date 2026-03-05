<template>
	<view
	    class="u-icon"
	    @tap="clickHandler"
	    :class="['u-icon--' + labelPos]"
	>
		<image
		    class="u-icon__img"
		    v-if="isImg"
		    :src="name"
		    :mode="imgMode"
		    :style="[imgStyle, addStyle(customStyle)]"
		></image>
		<text
		    v-else
		    class="u-icon__icon"
		    :class="uClasses"
		    :style="[iconStyle, addStyle(customStyle)]"
		    :hover-class="hoverClass"
		>{{icon}}</text>
		<!-- 這里进行空字符串判斷，如果仅仅是v-if="label"，可能會出現傳递0的時候，结果也無法顯示 -->
		<text
		    v-if="label !== ''" 
		    class="u-icon__label"
		    :style="{
			color: labelColor,
			fontSize: addUnit(labelSize),
			marginLeft: labelPos == 'right' ? addUnit(space) : 0,
			marginTop: labelPos == 'bottom' ? addUnit(space) : 0,
			marginRight: labelPos == 'left' ? addUnit(space) : 0,
			marginBottom: labelPos == 'top' ? addUnit(space) : 0,
		}"
		>{{ label }}</text>
	</view>
</template>

<script>
	// #ifdef APP-NVUE
	// nvue通過weex的dom模块引入字體，相關文檔地址如下：
	// https://weex.apache.org/zh/docs/modules/dom.html#addrule
	const fontUrl = 'https://at.alicdn.com/t/font_2225171_8kdcwk4po24.ttf'
	const domModule = weex.requireModule('dom')
	domModule.addRule('fontFace', {
		'fontFamily': "uicon-iconfont",
		'src': `url('${fontUrl}')`
	})
	// #endif

	// 引入圖標名稱，已經對應的unicode
	import icons from './icons'
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle } from '../../libs/function/index';
	import config from '../../libs/config/config';
	/**
	 * icon 圖標
	 * @description 基于字體的圖標集，包含了大多數常見场景的圖標。
	 * @tutorial https://ijry.github.io/uview-plus/components/icon.html
	 * @property {String}			name			圖標名稱，見示例圖標集
	 * @property {String}			color			圖標顏色,可接受主題色 （默認 color['u-content-color'] ）
	 * @property {String | Number}	size			圖標字體大小，單位px （默認 '16px' ）
	 * @property {Boolean}			bold			是否顯示粗體 （默認 false ）
	 * @property {String | Number}	index			點擊圖標的時候傳递事件出去的index（用于區分點擊了哪一個）
	 * @property {String}			hoverClass		圖標按下去的樣式類，用法同uni的view组件的hoverClass参數，詳情見官网
	 * @property {String}			customPrefix	自定義扩展前缀，方便用户扩展自己的圖標庫 （默認 'uicon' ）
	 * @property {String | Number}	label			圖標右侧的label文字
	 * @property {String}			labelPos		label相對于圖標的位置，只能right或bottom （默認 'right' ）
	 * @property {String | Number}	labelSize		label字體大小，單位px （默認 '15px' ）
	 * @property {String}			labelColor		圖標右侧的label文字顏色 （ 默認 color['u-content-color'] ）
	 * @property {String | Number}	space			label與圖標的距离，單位px （默認 '3px' ）
	 * @property {String}			imgMode			圖片的mode
	 * @property {String | Number}	width			顯示圖片小圖標時的寬度
	 * @property {String | Number}	height			顯示圖片小圖標時的高度
	 * @property {String | Number}	top				圖標在垂直方向上的定位 用于解决某些情况下，让圖標垂直居中的用途  （默認 0 ）
	 * @property {Boolean}			stop			是否阻止事件傳播 （默認 false ）
	 * @property {Object}			customStyle		icon的樣式，對象形式
	 * @event {Function} click 點擊圖標時触發
	 * @event {Function} touchstart 事件触摸時触發
	 * @example <u-icon name="photo" color="#2979ff" size="28"></u-icon>
	 */
	export default {
		name: 'u-icon',
		data() {
			return {

			}
		},
		emits: ['click'],
		mixins: [mpMixin, mixin, props],
		computed: {
			uClasses() {
				let classes = []
				classes.push(this.customPrefix + '-' + this.name)
				// uView的自定義圖標類名為u-iconfont
				if (this.customPrefix == 'uicon') {
					classes.push('u-iconfont')
				} else {
					// 不能缺少這一步，否则自定義圖標會無效
					classes.push(this.customPrefix)
				}
				// 主題色，通過類配置
				if (this.color && config.type.includes(this.color)) classes.push('u-icon__icon--' + this.color)
				// 阿里，頭條，百度小程序通過數组绑定類名時，無法直接使用[a, b, c]的形式，否则無法识别
				// 故需將其拆成一個字符串的形式，通過空格隔開各個類名
				//#ifdef MP-ALIPAY || MP-TOUTIAO || MP-BAIDU
				classes = classes.join(' ')
				//#endif
				return classes
			},
			iconStyle() {
				let style = {}
				style = {
					fontSize: addUnit(this.size),
					lineHeight: addUnit(this.size),
					fontWeight: this.bold ? 'bold' : 'normal',
					// 某些特殊情况需要設置一個到顶部的距离，才能更好的垂直居中
					top: addUnit(this.top)
				}
				// 非主題色值時，才當作顏色值
				if (this.color && !config.type.includes(this.color)) style.color = this.color

				return style
			},
			// 判斷傳入的name屬性，是否圖片路徑，只要带有"/"均認為是圖片形式
			isImg() {
				return this.name.indexOf('/') !== -1
			},
			imgStyle() {
				let style = {}
				// 如果設置width和height屬性，则优先使用，否则使用size屬性
				style.width = this.width ? addUnit(this.width) : addUnit(this.size)
				style.height = this.height ? addUnit(this.height) : addUnit(this.size)
				return style
			},
			// 通過圖標名，查找對應的圖標
			icon() {
				// 使用自定義圖標的時候頁面上會把name屬性也展示出来，所以在這里處理一下
				if (this.customPrefix !== "uicon") return "";
				// 如果内置的圖標中找不到對應的圖標，就直接返回name值，因為用户可能傳入的是unicode代碼
				return icons['uicon-' + this.name] || this.name
			}
		},
		methods: {
			addStyle,
			addUnit,
			clickHandler(e) {
				this.$emit('click', this.index)
				// 是否阻止事件冒泡
				this.stop && this.preventEvent(e)
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	// 變量定義
	$u-icon-primary: $u-primary !default;
	$u-icon-success: $u-success !default;
	$u-icon-info: $u-info !default;
	$u-icon-warning: $u-warning !default;
	$u-icon-error: $u-error !default;
	$u-icon-label-line-height:1 !default;

	/* #ifndef APP-NVUE */
	// 非nvue下加載字體
	@font-face {
		font-family: 'uicon-iconfont';
		src: url('https://at.alicdn.com/t/font_2225171_8kdcwk4po24.ttf') format('truetype');
	}

	/* #endif */

	.u-icon {
		/* #ifndef APP-NVUE */
		display: flex;
		/* #endif */
		align-items: center;

		&--left {
			flex-direction: row-reverse;
			align-items: center;
		}

		&--right {
			flex-direction: row;
			align-items: center;
		}

		&--top {
			flex-direction: column-reverse;
			justify-content: center;
		}

		&--bottom {
			flex-direction: column;
			justify-content: center;
		}

		&__icon {
			font-family: uicon-iconfont;
			position: relative;
			@include flex;
			align-items: center;

			&--primary {
				color: $u-icon-primary;
			}

			&--success {
				color: $u-icon-success;
			}

			&--error {
				color: $u-icon-error;
			}

			&--warning {
				color: $u-icon-warning;
			}

			&--info {
				color: $u-icon-info;
			}
		}

		&__img {
			/* #ifndef APP-NVUE */
			height: auto;
			will-change: transform;
			/* #endif */
		}

		&__label {
			/* #ifndef APP-NVUE */
			line-height: $u-icon-label-line-height;
			/* #endif */
		}
	}
</style>
