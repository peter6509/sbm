<template>
	<view class="u-navbar-mini" :class="[customClass]">
		<view class="u-navbar-mini__inner" :class="[fixed && 'u-navbar-mini--fixed']">
			<u-status-bar
				v-if="safeAreaInsetTop"
			></u-status-bar>
			<view
				class="u-navbar-mini__content"
				:class="[border && 'u-border-bottom']"
				:style="{
					height: addUnit(height),
					backgroundColor: bgColor,
				}"
			>
				<view
					class="u-navbar-mini__content__left"
					hover-class="u-navbar-mini__content__left--hover"
					hover-start-time="150"
					@tap="leftClick"
				>
					<slot name="left">
						<up-icon
							:name="leftIcon"
							:size="iconSize"
							:color="iconColor"
						></up-icon>
					</slot>
				</view>
				<view style="padding: 10px 10px;">
					<up-line direction="col" color="#fff" length="16px"></up-line>
				</view>
                <view
					class="u-navbar-mini__content__center" @tap="homeClick">
                    <slot name="center">
                        <up-icon name="home" :size="iconSize" :color="iconColor"></up-icon>
                    </slot>
                </view>
			</view>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, getPx, sys } from '../../libs/function/index';
	/**
	 * NavbarMini 迷你导航栏
	 * @description 此组件一般用于在全屏頁面中，典型的如微信小程序左上角。
	 * @tutorial https://ijry.github.io/uview-plus/components/navbar-mini.html
	 * @property {Boolean}			safeAreaInsetTop	是否開啟顶部安全區適配  （默認 true ）
	 * @property {Boolean}			placeholder			固定在顶部時，是否生成一個等高元素，以防止塌陷 （默認 false ）
	 * @property {Boolean}			fixed				导航栏是否固定在顶部 （默認 false ）
	 * @property {String}			leftIcon			左邊返回圖標的名稱，只能為uView自带的圖標 （默認 'arrow-left' ）
	 * @property {String}			title				导航栏標題，如設置為空字符，將會隐藏標題占位區域
	 * @property {String}			bgColor				导航栏背景設置 （默認 '#ffffff' ）
	 * @property {String | Number}	height				导航栏高度(不包括狀態栏高度在内，内部自動加上)（默認 '44px' ）
	 * @property {String | Number}	iconSize			左侧返回圖標的大小（默認 20px ）
	 * @property {String | Number}	leftIconColor		左侧返回圖標的顏色（默認 #303133 ）
	 * @property {Boolean}	        autoBack			點擊左侧區域(返回圖標)，是否自動返回上一頁（默認 false ）
	 * @property {Object | String}	titleStyle			標題的樣式，對象或字符串
	 * @event {Function} leftClick		點擊左侧區域
	 * @event {Function} rightClick		點擊右侧區域
	 * @example <u-navbar-mini @click-left="onClickBack"></u-navbar-mini>
	 */
	export default {
		name: 'u-navbar-mini',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
			}
		},
		emits: ["leftClick", "homeClick"],
        created() {
        },
		methods: {
			addStyle,
			addUnit,
			sys,
			getPx,
			// 點擊左侧區域
			leftClick() {
				// 如果配置了autoBack，自動返回上一頁
				this.$emit('leftClick')
				if(this.autoBack) {
					uni.navigateBack()
				}
			},
			homeClick() {
				if (this.homeUrl) {
					uni.reLaunch({ url: this.homeUrl })
				}
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-navbar-mini {

        &__inner {
            width: 180rpx;
            overflow: hidden;
        }

		&--fixed {
			position: fixed;
			left: 20px;
			right: 0;
			top: 10px;
			z-index: 11;
		}

		&__content {
			@include flex(row);
            padding: 0 15px;
			border-radius: 20px;
			align-items: center;
			height: 36px;
			background-color: #9acafc;
			position: relative;
			justify-content: space-between;

			&__left {
				@include flex(row);
				align-items: center;
			}

			&__left {
				&--hover {
					opacity: 0.7;
				}
			}
		}
	}
</style>
