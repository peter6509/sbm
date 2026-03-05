<template>
	<u-transition
		mode="fade"
		:show="show"
		:duration="fade ? 1000 : 0"
	>
		<view
			class="u-image"
			@tap="onClick"
			:style="[wrapStyle, backgroundStyle]"
		>
			<image
				v-if="!isError"
				:src="src"
				:mode="mode"
				@error="onErrorHandler"
				@load="onLoadHandler"
				:show-menu-by-longpress="showMenuByLongpress"
				:lazy-load="lazyLoad"
				class="u-image__image"
				:style="{
					borderRadius: shape == 'circle' ? '10000px' : addUnit(radius),
					width: addUnit(width),
					height: addUnit(height)
				}"
			></image>
			<view
				v-if="showLoading && loading"
				class="u-image__loading"
				:style="{
					borderRadius: shape == 'circle' ? '50%' : addUnit(radius),
					backgroundColor: this.bgColor,
					width: addUnit(width),
					height: addUnit(height)
				}"
			>
				<slot name="loading">
					<u-icon
						:name="loadingIcon"
						:width="width"
						:height="height"
					></u-icon>
				</slot>
			</view>
			<view
				v-if="showError && isError && !loading"
				class="u-image__error"
				:style="{
					borderRadius: shape == 'circle' ? '50%' : addUnit(radius),
					width: addUnit(width),
					height: addUnit(height)
				}"
			>
				<slot name="error">
					<u-icon
						:name="errorIcon"
						:width="width"
						:height="height"
					></u-icon>
				</slot>
			</view>
		</view>
	</u-transition>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, deepMerge } from '../../libs/function/index';
	/**
	 * Image 圖片
	 * @description 此组件為uni-app的image组件的加强版，在继承了原有功能外，還支持淡入動画、加載中、加載失败提示、圆角值和形狀等。
	 * @tutorial https://uview-plus.jiangruyi.com/components/image.html
	 * @property {String}			src 				圖片地址
	 * @property {String}			mode 				裁剪模式，見官网說明 （默認 'aspectFill' ）
	 * @property {String | Number}	width 				寬度，單位任意，如果為數值，则為px單位 （默認 '300' ）
	 * @property {String | Number}	height 				高度，單位任意，如果為數值，则為px單位 （默認 '225' ）
	 * @property {String}			shape 				圖片形狀，circle-圆形，square-方形 （默認 'square' ）
	 * @property {String | Number}	radius		 		圆角值，單位任意，如果為數值，则為px單位 （默認 0 ）
	 * @property {Boolean}			lazyLoad			是否懒加載，仅微信小程序、App、百度小程序、字节跳動小程序有效 （默認 true ）
	 * @property {Boolean}			showMenuByLongpress	是否開啟長按圖片顯示识别小程序碼菜單，仅微信小程序有效 （默認 true ）
	 * @property {String}			loadingIcon 		加載中的圖標，或者小圖片 （默認 'photo' ）
	 * @property {String}			errorIcon 			加載失败的圖標，或者小圖片 （默認 'error-circle' ）
	 * @property {Boolean}			showLoading 		是否顯示加載中的圖標或者自定義的slot （默認 true ）
	 * @property {Boolean}			showError 			是否顯示加載错误的圖標或者自定義的slot （默認 true ）
	 * @property {Boolean}			fade 				是否需要淡入效果 （默認 true ）
	 * @property {Boolean}			webp 				只支持网络资源，只對微信小程序有效 （默認 false ）
	 * @property {String | Number}	duration 			搭配fade参數的過渡時間，單位ms （默認 500 ）
	 * @property {String}			bgColor 			背景顏色，用于深色頁面加載圖片時，為了和背景色融合  (默認 '#f3f4f6' )
	 * @property {Object}			customStyle  		定義需要用到的外部樣式
	 * @event {Function}	click	點擊圖片時触發
	 * @event {Function}	error	圖片加載失败時触發
	 * @event {Function} load 圖片加載成功時触發
	 * @example <u-image width="100%" height="300px" :src="src"></u-image>
	 */
	export default {
		name: 'u-image',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				// 圖片是否加載错误，如果是，则顯示错误占位圖
				isError: false,
				// 初始化组件時，默認為加載中狀態
				loading: true,
				// 不透明度，為了實現淡入淡出的效果
				opacity: 1,
				// 過渡時間，因為props的值無法修改，故需要一個中間值
				durationTime: this.duration,
				// 圖片加載完成時，去掉背景顏色，因為如果是png圖片，就會顯示灰色的背景
				backgroundStyle: {},
				// 用于fade模式的控制组件顯示與否
				show: false
			};
		},
		watch: {
			src: {
				immediate: true,
				handler(n) {
					if (!n) {
						// 如果傳入null或者''，或者false，或者undefined，標记為错误狀態
						this.isError = true
						
					} else {
						this.isError = false;
						this.loading = true;
					}
				}
			}
		},
		computed: {
			wrapStyle() {
				let style = {};
				// 通過調用addUnit()方法，如果有單位，如百分比，px單位等，直接返回，如果是纯粹的數值，则加上rpx單位
				style.width = addUnit(this.width);
				style.height = addUnit(this.height);
				// 如果是顯示圆形，設置一個很多的半徑值即可
				style.borderRadius = this.shape == 'circle' ? '10000px' : addUnit(this.radius)
				// 如果設置圆角，必須要有hidden，否则可能圆角無效
				style.overflow = this.radius > 0 ? 'hidden' : 'visible'
				// if (this.fade) {
				// 	style.opacity = this.opacity
				// 	// nvue下，這几個屬性必須要分開写
				// 	style.transitionDuration = `${this.durationTime}ms`
				// 	style.transitionTimingFunction = 'ease-in-out'
				// 	style.transitionProperty = 'opacity'
				// }
				return deepMerge(style, addStyle(this.customStyle));

			}
		},
		mounted() {
			this.show = true
		},
		emits: ['click', 'error', 'load'],
		methods: {
			addUnit,
			// 點擊圖片
			onClick() {
				this.$emit('click')
			},
			// 圖片加載失败
			onErrorHandler(err) {
				this.loading = false
				this.isError = true
				this.$emit('error', err)
			},
			// 圖片加載完成，標记loading结束
			onLoadHandler(event) {
				this.loading = false
				this.isError = false
				this.$emit('load', event)
				this.removeBgColor()
				// 如果不需要動画效果，就不執行下方代碼，同時移除加載時的背景顏色
				// 否则無需fade效果時，png圖片依然能看到下方的背景色
				// if (!this.fade) return this.removeBgColor();
				// // 原来opacity為1(不透明，是為了顯示占位圖)，改成0(透明，意味着該元素顯示的是背景顏色，默認的灰色)，再改成1，是為了獲得過渡效果
				// this.opacity = 0;
				// // 這里設置為0，是為了圖片展示到背景全透明這個過程時間為0，延時之后延時之后重新設置為duration，是為了獲得背景透明(灰色)
				// // 到圖片展示的過程中的淡入效果
				// this.durationTime = 0;
				// // 延時50ms，否则在浏覽器H5，過渡效果無效
				// setTimeout(() => {
				// 	this.durationTime = this.duration;
				// 	this.opacity = 1;
				// 	setTimeout(() => {
				// 		this.removeBgColor();
				// 	}, this.durationTime);
				// }, 50);
			},
			// 移除圖片的背景色
			removeBgColor() {
				// 淡入動画過渡完成后，將背景設置為透明色，否则png圖片會看到灰色的背景
				this.backgroundStyle = {
					backgroundColor: 'transparent'
				};
			}
		}
	};
</script>

<style lang="scss" scoped>
	@import '../../libs/css/components.scss';

	$u-image-error-top:0px !default;
	$u-image-error-left:0px !default;
	$u-image-error-width:100% !default;
	$u-image-error-hight:100% !default;
	$u-image-error-background-color:$u-bg-color !default;
	$u-image-error-color:$u-tips-color !default;
	$u-image-error-font-size: 46rpx !default;

	.u-image {
		position: relative;
		transition: opacity 0.5s ease-in-out;

		&__image {
			width: 100%;
			height: 100%;
		}

		&__loading,
		&__error {
			position: absolute;
			top: $u-image-error-top;
			left: $u-image-error-left;
			width: $u-image-error-width;
			height: $u-image-error-hight;
			@include flex;
			align-items: center;
			justify-content: center;
			background-color: $u-image-error-background-color;
			color: $u-image-error-color;
			font-size: $u-image-error-font-size;
		}
	}
</style>
