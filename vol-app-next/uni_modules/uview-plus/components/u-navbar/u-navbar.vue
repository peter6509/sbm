<template>
	<view class="u-navbar" :class="[customClass]">
		<view
			class="u-navbar__placeholder"
			v-if="fixed && placeholder"
			:style="{
				height: addUnit(getPx(height) + sys().statusBarHeight,'px'),
			}"
		></view>
		<view :class="[fixed && 'u-navbar--fixed']">
			<u-status-bar
				v-if="safeAreaInsetTop"
				:bgColor="bgColor"
			></u-status-bar>
			<view
				class="u-navbar__content"
				:class="[border && 'u-border-bottom']"
				:style="{
					height: addUnit(height),
					backgroundColor: bgColor,
				}"
			>
				<view
					class="u-navbar__content__left"
					hover-class="u-navbar__content__left--hover"
					hover-start-time="150"
					@tap="leftClick"
				>
					<slot name="left">
						<u-icon
							v-if="leftIcon"
							:name="leftIcon"
							:size="leftIconSize"
							:color="leftIconColor"
						></u-icon>
						<text
							v-if="leftText"
							:style="{
								color: leftIconColor
							}"
							class="u-navbar__content__left__text"
						>{{ leftText }}</text>
					</slot>
				</view>
				<slot name="center">
					<text
						class="u-line-1 u-navbar__content__title"
						:style="[{
							width: addUnit(titleWidth),
						}, addStyle(titleStyle)]"
					>{{ title }}</text>
				</slot>
				<view
					class="u-navbar__content__right"
					v-if="$slots.right || rightIcon || rightText"
					@tap="rightClick"
				>
					<slot name="right">
						<u-icon
							v-if="rightIcon"
							:name="rightIcon"
							size="20"
						></u-icon>
						<text
							v-if="rightText"
							class="u-navbar__content__right__text"
						>{{ rightText }}</text>
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
	 * Navbar 自定義导航栏
	 * @description 此组件一般用于在特殊情况下，需要自定義导航栏的時候用到，一般建议使用uni-app带的导航栏。
	 * @tutorial https://ijry.github.io/uview-plus/components/navbar.html
	 * @property {Boolean}			safeAreaInsetTop	是否開啟顶部安全區適配  （默認 true ）
	 * @property {Boolean}			placeholder			固定在顶部時，是否生成一個等高元素，以防止塌陷 （默認 false ）
	 * @property {Boolean}			fixed				导航栏是否固定在顶部 （默認 false ）
	 * @property {Boolean}			border				导航栏底部是否顯示下邊框 （默認 false ）
	 * @property {String}			leftIcon			左邊返回圖標的名稱，只能為uView自带的圖標 （默認 'arrow-left' ）
	 * @property {String}			leftText			左邊的提示文字
	 * @property {String}			rightText			右邊的提示文字
	 * @property {String}			rightIcon			右邊返回圖標的名稱，只能為uView自带的圖標
	 * @property {String}			title				导航栏標題，如設置為空字符，將會隐藏標題占位區域
	 * @property {String}			bgColor				导航栏背景設置 （默認 '#ffffff' ）
	 * @property {String | Number}	titleWidth			导航栏標題的最大寬度，内容超出會以省略號隐藏 （默認 '400rpx' ）
	 * @property {String | Number}	height				导航栏高度(不包括狀態栏高度在内，内部自動加上)（默認 '44px' ）
	 * @property {String | Number}	leftIconSize		左侧返回圖標的大小（默認 20px ）
	 * @property {String | Number}	leftIconColor		左侧返回圖標的顏色（默認 #303133 ）
	 * @property {Boolean}	        autoBack			點擊左侧區域(返回圖標)，是否自動返回上一頁（默認 false ）
	 * @property {Object | String}	titleStyle			標題的樣式，對象或字符串
	 * @event {Function} leftClick		點擊左侧區域
	 * @event {Function} rightClick		點擊右侧區域
	 * @example <u-navbar title="剑未配妥，出门已是江湖" left-text="返回" right-text="帮助" @click-left="onClickBack" @click-right="onClickRight"></u-navbar>
	 */
	export default {
		name: 'u-navbar',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
			}
		},
		emits: ["leftClick", "rightClick"],
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
			// 點擊右侧區域
			rightClick() {
				this.$emit('rightClick')
			},
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-navbar {

		&--fixed {
			position: fixed;
			left: 0;
			right: 0;
			top: 0;
			z-index: 11;
		}

		&__content {
			@include flex(row);
			align-items: center;
			height: 44px;
			background-color: #9acafc;
			position: relative;
			justify-content: center;

			&__left,
			&__right {
				padding: 0 13px;
				position: absolute;
				top: 0;
				bottom: 0;
				@include flex(row);
				align-items: center;
			}

			&__left {
				left: 0;
				
				&--hover {
					opacity: 0.7;
				}

				&__text {
					font-size: 15px;
					margin-left: 3px;
				}
			}

			&__title {
				text-align: center;
				font-size: 16px;
				color: $u-main-color;
			}

			&__right {
				right: 0;

				&__text {
					font-size: 15px;
					margin-left: 3px;
				}
			}
		}
	}
</style>
