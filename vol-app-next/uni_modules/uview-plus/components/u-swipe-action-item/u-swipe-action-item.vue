<template>	
	<!-- #ifdef MP-ALIPAY -->
	<!-- <import-sjs from="./alipay.sjs" name="mysjs" /> -->
	<!-- #endif -->
	<view class="u-swipe-action-item" ref="u-swipe-action-item">
		<view class="u-swipe-action-item__right">
			<slot name="button">
				<view v-for="(item,index) in options" :key="index" class="u-swipe-action-item__right__button"
					:ref="`u-swipe-action-item__right__button-${index}`" :style="[{
						alignItems: item.style && item.style.borderRadius ? 'center' : 'stretch'
					}]" @tap="buttonClickHandler(item, index)">
					<view class="u-swipe-action-item__right__button__wrapper" :style="[{
							backgroundColor: item.style && item.style.backgroundColor ? item.style.backgroundColor : '#C7C6CD',
							borderRadius: item.style && item.style.borderRadius ? item.style.borderRadius : '0',
							padding: item.style && item.style.borderRadius ? '0' : '0 15px',
						}, item.style]">
						<u-icon v-if="item.icon" :name="item.icon"
							:color="item.style && item.style.color ? item.style.color : '#ffffff'"
							:size="item.iconSize ? addUnit(item.iconSize) : item.style && item.style.fontSize ? getPx(item.style.fontSize) * 1.2 : 17"
							:customStyle="{
								marginRight: item.text ? '2px' : 0
							}"></u-icon>
						<text v-if="item.text" class="u-swipe-action-item__right__button__wrapper__text u-line-1"
							:style="[{
								color: item.style && item.style.color ? item.style.color : '#ffffff',
								fontSize: item.style && item.style.fontSize ? item.style.fontSize : '16px',
								lineHeight: item.style && item.style.fontSize ? item.style.fontSize : '16px',
							}]">{{ item.text }}</text>
					</view>
				</view>
			</slot>
		</view>
		<!-- #ifdef APP-VUE || MP-WEIXIN || MP-QQ || H5  -->
		<view class="u-swipe-action-item__content" @touchstart="wxs.touchstart" @touchmove="wxs.touchmove"
			@touchend="wxs.touchend" :status="status" :change:status="wxs.statusChange" :size="size"
			:change:size="wxs.sizeChange">
			<slot></slot>
		</view>
		<!-- #endif -->
		<!-- #ifdef MP-ALIPAY || MP-BAIDU || MP-TOUTIAO-->
		<view class="u-swipe-action-item__content" @click="clickHandler" @touchstart="touchstart" @touchmove="touchmove"
			@touchend="touchend" :style="sliderStyle">
			<slot></slot>
		</view>
		<!-- <view class="u-swipe-action-item__content" @touchstart="mysjs.touchstart" @touchmove="mysjs.touchmove"
			@touchend="mysjs.touchend">
			<slot></slot>
		</view> -->
		<!-- #endif -->
		<!-- #ifdef APP-NVUE -->
		<view class="u-swipe-action-item__content" ref="u-swipe-action-item__content" @panstart="onTouchstart"
			@tap="clickHandler">
			<slot></slot>
		</view>
		<!-- #endif -->
	</view>
</template>
<!-- #ifdef APP-VUE || MP-WEIXIN || MP-QQ || H5 -->
<script src="./index.wxs" module="wxs" lang="wxs"></script>
<!-- #endif -->
<script>
	import { touchMixin } from '../../libs/mixin/touch'
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, getPx, sleep } from '../../libs/function/index';
	// #ifdef APP-NVUE
	import nvue from './nvue.js';
	// #endif
	// #ifdef APP-VUE || MP-WEIXIN || MP-QQ || H5
	import wxs from './wxs.js';
	// #endif
	// #ifdef MP-ALIPAY || MP-BAIDU || MP-TOUTIAO
	import other from './other.js';
	// #endif
	/**
	 * SwipeActionItem 滑動單元格子组件
	 * @description 該组件一般用于左滑唤出操作菜單的场景，用的最多的是左滑删除操作
	 * @tutorial https://ijry.github.io/uview-plus/components/swipeAction.html
	 * @property {Boolean}			show			控制打開或者關闭（默認 false ）
	 * @property {String | Number}	index			標识符，如果是v-for，可用index索引
	 * @property {Boolean}			disabled		是否禁用（默認 false ）
	 * @property {Boolean}			autoClose		是否自動關闭其他swipe按鈕组（默認 true ）
	 * @property {Number}			threshold		滑動距离阈值，只有大于此值，才被認為是要打開菜單（默認 30 ）
	 * @property {Array}			options			右侧按鈕内容
	 * @property {String | Number}	duration		動画過渡時間，單位ms（默認 350 ）
	 * @event {Function(index)}	open	组件打開時触發
	 * @event {Function(index)}	close	组件關闭時触發
	 * @example	<u-swipe-action><u-swipe-action-item :options="options1" ></u-swipe-action-item></u-swipe-action>
	 */
	export default {
		name: 'u-swipe-action-item',
		emits: ['click', 'update:show'],
		mixins: [
			mpMixin,
			mixin,
			// #ifndef APP-NVUE
			touchMixin,
			// #endif
			// #ifdef APP-NVUE
			nvue,
			// #endif
			// #ifdef APP-VUE || MP-WEIXIN || MP-QQ || H5
			wxs,
			// #endif
			// #ifdef MP-ALIPAY || MP-BAIDU || MP-TOUTIAO
			other,
			// #endif
			props
		],
		data() {
			return {
				// 按鈕的尺寸信息
				size: {},
				// 父组件u-swipe-action的参數
				parentData: {
					autoClose: true,
				},
				// 當前狀態，open-打開，close-關闭
				status: '',
				sliderStyle: {}
			}
		},
		watch: {
			// 由于wxs無法直接讀取外部的值，需要在外部值變化時，重新執行赋值逻輯
			// #ifndef APP-NVUE
			wxsInit(newValue, oldValue) {
				this.queryRect()
			},
			// #endif
			status(newValue) {
				if (newValue === 'open') {
					this.$emit('update:show', true)
					this.parent && this.parent.setOpendItem(this)
				} else {
					this.$emit('update:show', false)
				}
			},
			show(newValue) {
				if (newValue) {
					this.status = 'open'
				} else {
					this.status = 'close'
				}
			}
		},
		computed: {
			wxsInit() {
				return [this.disabled, this.autoClose, this.threshold, this.options, this.duration]
			}
		},
		mounted() {
			this.init()
		},
		beforeUmount() {
			this.closeHandler()
		},
		methods: {
			addUnit,
			getPx,
			init() {
				// 初始化父组件數據
				this.updateParentData()
				// #ifndef APP-NVUE
				sleep().then(() => {
					this.queryRect()
				})
				// #endif
			},
			updateParentData() {
				// 此方法在mixin中
				this.getParentData('u-swipe-action')
			},
			// #ifndef APP-NVUE
			// 查詢節點
			queryRect() {
				this.$uGetRect('.u-swipe-action-item__right__button', true).then(buttons => {
					this.size = {
						buttons,
						show: this.show,
						disabled: this.disabled,
						threshold: this.threshold,
						duration: this.duration
					}
				})
			},
			// #endif
			// 按鈕被點擊
			buttonClickHandler(item, index) {
				let ret = this.$emit('click', {
					index,
					name: this.name
				}, () => {
				})
				if (this.closeOnClick) {
					this.closeHandler()
				}
			}
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-swipe-action-item {
		position: relative;
		overflow: hidden;
		/* #ifndef APP-NVUE || MP-WEIXIN */
		touch-action: pan-y;
		/* #endif */

		&__content {
            transform: translateX(0px); // 修複某些情况下默認右侧按鈕是展開的問題
			background-color: #FFFFFF;
			z-index: 10;
		}

		&__right {
			position: absolute;
			top: 0;
			bottom: 0;
			right: 0;
			@include flex;

			&__button {
				@include flex;
				justify-content: center;
				overflow: hidden;
				align-items: center;

				&__wrapper {
					@include flex;
					align-items: center;
					justify-content: center;
					padding: 0 15px;

					&__text {
						@include flex;
						align-items: center;
						color: #FFFFFF;
						font-size: 15px;
						text-align: center;
						justify-content: center;
					}
				}
			}
		}
	}
</style>
