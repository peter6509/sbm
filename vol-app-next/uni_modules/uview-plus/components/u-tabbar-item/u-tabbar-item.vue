<template>
	<view
	    class="u-tabbar-item"
	    :style="[addStyle(customStyle)]"
	    @tap="clickHandler"
	>
		<view class="u-tabbar-item__icon">
			<u-icon
			    v-if="icon"
			    :name="icon"
			    :color="isActive? parentData.activeColor : parentData.inactiveColor"
			    :size="20"
			></u-icon>
			<template v-else>
				<slot
				    v-if="isActive"
				    name="active-icon"
				/>
				<slot
				    v-else
				    name="inactive-icon"
				/>
			</template>
			<u-badge
				absolute
				:offset="[0, dot ? '34rpx' : badge > 9 ? '14rpx' : '20rpx']"
			    :customStyle="badgeStyle"
			    :isDot="dot"
			    :value="badge || (dot ? 1 : null)"
			    :show="dot || badge > 0"
			></u-badge>
		</view>
		
		<slot name="text">
			<text
			    class="u-tabbar-item__text"
			    :style="{
					color: isActive? parentData.activeColor : parentData.inactiveColor
				}"
			>{{ text }}</text>
		</slot>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, error } from '../../libs/function/index';
	/**
	 * TabbarItem 底部导航栏子组件
	 * @description 此组件提供了自定義tabbar的能力。
	 * @tutorial https://ijry.github.io/uview-plus/components/tabbar.html
	 * @property {String | Number}	name		item標簽的名稱，作為與u-tabbar的value参數匹配的標识符
	 * @property {String}			icon		uView内置圖標或者绝對路徑的圖片
	 * @property {String | Number}	badge		右上角的角標提示信息
	 * @property {Boolean}			dot			是否顯示圆點，將會覆蓋badge参數（默認 false ）
	 * @property {String}			text		描述文本
	 * @property {Object | String}	badgeStyle	控制徽標的位置，對象或者字符串形式，可以設置top和right屬性（默認 'top: 6px;right:2px;' ）
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * 
	 * @example <u-tabbar :value="value2" :placeholder="false" @change="name => value2 = name" :fixed="false" :safeAreaInsetBottom="false"><u-tabbar-item text="首頁" icon="home" dot ></u-tabbar-item></u-tabbar>
	 */
	export default {
		name: 'u-tabbar-item',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				isActive: false, // 是否處于激活狀態
				parentData: {
					value: null,
					activeColor: '',
					inactiveColor: ''
				}
			}
		},
		//  微信小程序中 options 選項
		options: {
		    virtualHost: true //將自定義節點設置成虚拟的，更加接近Vue组件的表現。我们不希望自定義组件的這個節點本身可以設置樣式、响應 flex 布局等
		},
		created() {
			this.init()
		},
		emits: ["click", "change"],
		methods: {
			addStyle,
			init() {
				// 支付宝小程序不支持provide/inject，所以使用這個方法獲取整個父组件，在created定義，避免循环引用
				this.updateParentData()
				if (!this.parent) {
					error('u-tabbar-item必須搭配u-tabbar组件使用')
				}
				// 本子组件在u-tabbar的children數组中的索引
				const index = this.parent.children.indexOf(this)
				// 判斷本组件的name(如果没有定義name，就用index索引)是否等于父组件的value参數
				this.isActive = (this.name || index) === this.parentData.value
			},
			updateParentData() {
				// 此方法在mixin中
				this.getParentData('u-tabbar')
			},
			// 此方法將會被父组件u-tabbar調用
			updateFromParent() {
				// 重新初始化
				this.init()
			},
			clickHandler() {
				this.$nextTick(() => {
					const index = this.parent.children.indexOf(this)
					const name = this.name || index
					// 點擊的item為非激活的item才發出change事件
					if (name !== this.parent.value) {
						this.parent.$emit('change', name)
					}
					this.$emit('click', name)
				})
			}
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	.u-tabbar-item {
		@include flex(column);
		align-items: center;
		justify-content: center;
		flex: 1;
		/* #ifndef APP-NVUE */
		width: 100%;
		height: 100%;
		/* #endif */
		/* #ifdef H5 */
		cursor: pointer;
		/* #endif */
		
		&__icon {
			@include flex;
			position: relative;
			width: 150rpx;
			justify-content: center;
		}

		&__text {
			margin-top: 2px;
			font-size: 12px;
			color: $u-content-color;
		}
	}

	/* #ifdef MP */
	// 由于小程序都使用shadow DOM形式實現，需要给影子宿主設置flex: 1才能让其撑開
	:host {
		flex: 1;
		width: 100%;
	}
	/* #endif */
</style>
