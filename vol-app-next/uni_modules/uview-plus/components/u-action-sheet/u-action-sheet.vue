
<template>
	<u-popup
	    :show="show"
	    mode="bottom"
	    @close="closeHandler"
	    :safeAreaInsetBottom="safeAreaInsetBottom"
	    :round="round"
	>
		<view class="u-action-sheet">
			<view
			    class="u-action-sheet__header"
			    v-if="title"
			>
				<text class="u-action-sheet__header__title u-line-1">{{title}}</text>
				<view
				    class="u-action-sheet__header__icon-wrap"
				    @tap.stop="cancel"
				>
					<u-icon
					    name="close"
					    size="17"
					    color="#c8c9cc"
					    bold
					></u-icon>
				</view>
			</view>
			<text
			    class="u-action-sheet__description"
				:style="[{
					marginTop: `${title && description ? 0 : '18px'}`
				}]"
			    v-if="description"
			>{{description}}</text>
			<slot>
				<u-line v-if="description"></u-line>
				<scroll-view scroll-y class="u-action-sheet__item-wrap" :style="{maxHeight: wrapMaxHeight}">
					<view :key="index" v-for="(item, index) in actions">
						<!-- #ifdef MP -->
						<button
						    class="u-reset-button"
						    :openType="item.openType"
						    @getuserinfo="onGetUserInfo"
						    @contact="onContact"
						    @getphonenumber="onGetPhoneNumber"
						    @error="onError"
						    @launchapp="onLaunchApp"
						    @opensetting="onOpenSetting"
						    :lang="lang"
						    :session-from="sessionFrom"
						    :send-message-title="sendMessageTitle"
						    :send-message-path="sendMessagePath"
						    :send-message-img="sendMessageImg"
						    :show-message-card="showMessageCard"
						    :app-parameter="appParameter"
						    @tap="selectHandler(index)"
						    :hover-class="!item.disabled && !item.loading ? 'u-action-sheet--hover' : ''"
						>
							<!-- #endif -->
							<view
							    class="u-action-sheet__item-wrap__item"
							    @tap.stop="selectHandler(index)"
							    :hover-class="!item.disabled && !item.loading ? 'u-action-sheet--hover' : ''"
							    :hover-stay-time="150"
							>
								<template v-if="!item.loading">
									<text
									    class="u-action-sheet__item-wrap__item__name"
									    :style="[itemStyle(index)]"
									>{{ item.name }}</text>
									<text
									    v-if="item.subname"
									    class="u-action-sheet__item-wrap__item__subname"
									>{{ item.subname }}</text>
								</template>
								<u-loading-icon
								    v-else
								    custom-class="van-action-sheet__loading"
								    size="18"
								    mode="circle"
								/>
							</view>
							<!-- #ifdef MP -->
						</button>
						<!-- #endif -->
						<u-line v-if="index !== actions.length - 1"></u-line>
					</view>
				</scroll-view>
			</slot>
			<u-gap
			    bgColor="#eaeaec"
			    height="6"
			    v-if="cancelText"
			></u-gap>
			<view class="u-action-sheet__item-wrap__item u-action-sheet__cancel"
				hover-class="u-action-sheet--hover" @tap="cancel" v-if="cancelText">
				<text
				    @touchmove.stop.prevent
				    :hover-stay-time="150"
				    class="u-action-sheet__cancel-text"
				>{{cancelText}}</text>
			</view>
		</view>
	</u-popup>
</template>

<script>
	import { openType } from '../../libs/mixin/openType'
	import { buttonMixin } from '../../libs/mixin/button'
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit } from '../../libs/function/index';
	/**
	 * ActionSheet 操作菜單
	 * @description 本组件用于從底部彈出一個操作菜單，供用户選擇並返回结果。本组件功能類似于uni的uni.showActionSheetAPI，配置更加灵活，所有平台都表現一致。
	 * @tutorial https://ijry.github.io/uview-plus/components/actionSheet.html
	 * 
	 * @property {Boolean}			show				操作菜單是否展示 （默認 false ）
	 * @property {String}			title				操作菜單標題
	 * @property {String}			description			選項上方的描述信息
	 * @property {Array<Object>}	actions				按鈕的文字數组，見官方文檔示例
	 * @property {String}			cancelText			取消按鈕的提示文字,不為空時顯示按鈕
	 * @property {Boolean}			closeOnClickAction	點擊某個菜單項時是否關闭彈窗 （默認 true ）
	 * @property {Boolean}			safeAreaInsetBottom	處理底部安全區 （默認 true ）
	 * @property {String}			openType			小程序的打開方式 (contact | launchApp | getUserInfo | openSetting ｜getPhoneNumber ｜error )
	 * @property {Boolean}			closeOnClickOverlay	點擊遮罩是否允许關闭  (默認 true )
	 * @property {Number|String}	round				圆角值，默認無圆角  (默認 0 )
	 * @property {String}			lang				指定返回用户信息的語言，zh_CN 簡體中文，zh_TW 繁體中文，en 英文
	 * @property {String}			sessionFrom			會話来源，openType="contact"時有效
	 * @property {String}			sendMessageTitle	會話内消息卡片標題，openType="contact"時有效
	 * @property {String}			sendMessagePath		會話内消息卡片點擊跳轉小程序路徑，openType="contact"時有效
	 * @property {String}			sendMessageImg		會話内消息卡片圖片，openType="contact"時有效
	 * @property {Boolean}			showMessageCard		是否顯示會話内消息卡片，設置此参數為 true，用户进入客服會話會在右下角顯示"可能要發送的小程序"提示，用户點擊后可以快速發送小程序消息，openType="contact"時有效 （默認 false ）
	 * @property {String}			appParameter		打開 APP 時，向 APP 傳递的参數，openType=launchApp 時有效
	 * 
	 * @event {Function} select			點擊ActionSheet列表項時触發 
	 * @event {Function} close			點擊取消按鈕時触發
	 * @event {Function} getuserinfo	用户點擊該按鈕時，會返回獲取到的用户信息，回調的 detail 數據與 wx.getUserInfo 返回的一致，openType="getUserInfo"時有效
	 * @event {Function} contact		客服消息回調，openType="contact"時有效
	 * @event {Function} getphonenumber	獲取用户手機號回調，openType="getPhoneNumber"時有效
	 * @event {Function} error			當使用開放能力時，發生错误的回調，openType="error"時有效
	 * @event {Function} launchapp		打開 APP 成功的回調，openType="launchApp"時有效
	 * @event {Function} opensetting	在打開授權設置頁后回調，openType="openSetting"時有效
	 * @example <u-action-sheet :actions="list" :title="title" :show="show"></u-action-sheet>
	 */
	export default {
		name: "u-action-sheet",
		// 一些props参數和methods方法，通過mixin混入，因為其他文件也會用到
		mixins: [openType, buttonMixin, mixin, props],
		data() {
			return {

			}
		},
		computed: {
			// 操作項目的樣式
			itemStyle() {
				return (index) => {
					let style = {};
					if (this.actions[index].color) style.color = this.actions[index].color
					if (this.actions[index].fontSize) style.fontSize = addUnit(this.actions[index].fontSize)
					// 選項被禁用的樣式
					if (this.actions[index].disabled) style.color = '#c0c4cc'
					return style;
				}
			},
		},
		emits: ["close", "select", "update:show"],
		methods: {
			closeHandler() {
				// 允许點擊遮罩關闭時，才發出close事件
				if(this.closeOnClickOverlay) {
					this.$emit('update:show', false)
					this.$emit('close')
				}
			},
			// 點擊取消按鈕
			cancel() {
				this.$emit('update:show', false)
				this.$emit('close')
			},
			selectHandler(index) {
				const item = this.actions[index]
				if (item && !item.disabled && !item.loading) {
					this.$emit('select', item)
					if (this.closeOnClickAction) {
						this.$emit('update:show', false)
						this.$emit('close')
					}
				}
			},
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	$u-action-sheet-reset-button-width:100% !default;
	$u-action-sheet-title-font-size: 16px !default;
	$u-action-sheet-title-padding: 12px 30px !default;
	$u-action-sheet-title-color: $u-main-color !default;
	$u-action-sheet-header-icon-wrap-right:15px !default;
	$u-action-sheet-header-icon-wrap-top:15px !default;
	$u-action-sheet-description-font-size:13px !default;
	$u-action-sheet-description-color:14px !default;
	$u-action-sheet-description-margin: 18px 15px !default;
	$u-action-sheet-item-wrap-item-padding:17px !default;
	$u-action-sheet-item-wrap-name-font-size:16px !default;
	$u-action-sheet-item-wrap-subname-font-size:13px !default;
	$u-action-sheet-item-wrap-subname-color: #c0c4cc !default;
	$u-action-sheet-item-wrap-subname-margin-top:10px !default;
	$u-action-sheet-cancel-text-font-size:16px !default;
	$u-action-sheet-cancel-text-color:$u-content-color !default;
	$u-action-sheet-cancel-text-font-size:15px !default;
	$u-action-sheet-cancel-text-hover-background-color:rgb(242, 243, 245) !default;

	.u-reset-button {
		width: $u-action-sheet-reset-button-width;
	}

	.u-action-sheet {
		text-align: center;
		&__header {
			position: relative;
			padding: $u-action-sheet-title-padding;
			&__title {
				font-size: $u-action-sheet-title-font-size;
				color: $u-action-sheet-title-color;
				font-weight: bold;
				text-align: center;
			}

			&__icon-wrap {
				position: absolute;
				right: $u-action-sheet-header-icon-wrap-right;
				top: $u-action-sheet-header-icon-wrap-top;
			}
		}

		&__description {
			font-size: $u-action-sheet-description-font-size;
			color: $u-tips-color;
			margin: $u-action-sheet-description-margin;
			text-align: center;
		}

		&__item-wrap {

			&__item {
				padding: $u-action-sheet-item-wrap-item-padding;
				@include flex;
				align-items: center;
				justify-content: center;
				flex-direction: column;

				&__name {
					font-size: $u-action-sheet-item-wrap-name-font-size;
					color: $u-main-color;
					text-align: center;
				}

				&__subname {
					font-size: $u-action-sheet-item-wrap-subname-font-size;
					color: $u-action-sheet-item-wrap-subname-color;
					margin-top: $u-action-sheet-item-wrap-subname-margin-top;
					text-align: center;
				}
			}
		}

		&__cancel-text {
			font-size: $u-action-sheet-cancel-text-font-size;
			color: $u-action-sheet-cancel-text-color;
			text-align: center;
			// padding: $u-action-sheet-cancel-text-font-size;
		}

		&--hover {
			background-color: $u-action-sheet-cancel-text-hover-background-color;
		}
	}
</style>
