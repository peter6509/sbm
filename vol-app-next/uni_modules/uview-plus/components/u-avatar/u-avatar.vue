<template>
	<view
		class="u-avatar"
		:class="[`u-avatar--${shape}`]"
		:style="[{
			backgroundColor: (text || icon) ? (randomBgColor ? colors[colorIndex !== '' ? colorIndex : random(0, 19)] : bgColor) : 'transparent',
			width: addUnit(size),
			height: addUnit(size),
		}, addStyle(customStyle)]"
		@tap="clickHandler"
	>
		<slot>
			<!-- #ifdef MP-WEIXIN || MP-QQ || MP-BAIDU  -->
			<open-data
				v-if="mpAvatar && allowMp"
				type="userAvatarUrl"
				:style="[{
					width: addUnit(size),
					height: addUnit(size)
				}]"
			/>
			<!-- #endif -->
			<!-- #ifndef MP-WEIXIN && MP-QQ && MP-BAIDU  -->
			<template v-if="mpAvatar && allowMp"></template>
			<!-- #endif -->
			<u-icon
				v-else-if="icon"
				:name="icon"
				:size="fontSize"
				:color="color"
			></u-icon>
			<up-text
				v-else-if="text"
				:text="text"
				:size="fontSize"
				:color="color"
				align="center"
				customStyle="justify-content: center"
			></up-text>
			<image
				class="u-avatar__image"
				v-else
				:class="[`u-avatar__image--${shape}`]"
				:src="avatarUrl || defaultUrl"
				:mode="mode"
				@error="errorHandler"
				:style="[{
					width: addUnit(size),
					height: addUnit(size)
				}]"
			></image>
		</slot>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, addUnit, random } from '../../libs/function/index';
	const base64Avatar =
		"data:image/jpg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAA8AAD/4QMraHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjMtYzAxMSA2Ni4xNDU2NjEsIDIwMTIvMDIvMDYtMTQ6NTY6MjcgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjREMEQwRkY0RjgwNDExRUE5OTY2RDgxODY3NkJFODMxIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjREMEQwRkY1RjgwNDExRUE5OTY2RDgxODY3NkJFODMxIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6NEQwRDBGRjJGODA0MTFFQTk5NjZEODE4Njc2QkU4MzEiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6NEQwRDBGRjNGODA0MTFFQTk5NjZEODE4Njc2QkU4MzEiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7/7gAOQWRvYmUAZMAAAAAB/9sAhAAGBAQEBQQGBQUGCQYFBgkLCAYG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";
	/**
	 * Avatar  頭像
	 * @description 本组件一般用于展示頭像的地方，如個人中心，或者评论列表頁的用户頭像展示等场所。
	 * @tutorial https://ijry.github.io/uview-plus/components/avatar.html
	 *
	 * @property {String}			src				頭像路徑，如加載失败，將會顯示默認頭像(不能為相對路徑)
	 * @property {String}			shape			頭像形狀  （ circle (默認) | square）
	 * @property {String | Number}	size			頭像尺寸，可以為指定字符串(large, default, mini)，或者數值 （默認 40 ）
	 * @property {String}			mode			頭像圖片的裁剪類型，與uni的image组件的mode参數一致，如效果达不到需求，可尝试傳widthFix值 （默認 'scaleToFill' ）
	 * @property {String}			text			用文字替代圖片，級别优先于src
	 * @property {String}			bgColor			背景顏色，一般顯示文字時用 （默認 '#c0c4cc' ）
	 * @property {String}			color			文字顏色 （默認 '#ffffff' ）
	 * @property {String | Number}	fontSize		文字大小  （默認 18 ）
	 * @property {String}			icon			顯示的圖標
	 * @property {Boolean}			mpAvatar		顯示小程序頭像，只對百度，微信，QQ小程序有效  （默認 false ）
	 * @property {Boolean}			randomBgColor	是否使用随機背景色  （默認 false ）
	 * @property {String}			defaultUrl		加載失败的默認頭像(组件有内置默認圖片)
	 * @property {String | Number}	colorIndex		如果配置了randomBgColor為true，且配置了此值，则從默認的背景色數组中取出對應索引的顏色值，取值0-19之間
	 * @property {String}			name			组件標识符  （默認 'level' ）
	 * @property {Object}			customStyle		定義需要用到的外部樣式
	 *
	 * @event    {Function}        click       點擊组件時触發   index: 用户傳递的標识符
	 * @example  <u-avatar :src="src" mode="square"></u-avatar>
	 */
	export default {
		name: 'u-avatar',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				// 如果配置randomBgColor参數為true，在圖標或者文字的模式下，會随機從中取出一個顏色值當做背景色
				colors: ['#ffb34b', '#f2bba9', '#f7a196', '#f18080', '#88a867', '#bfbf39', '#89c152', '#94d554', '#f19ec2',
					'#afaae4', '#e1b0df', '#c38cc1', '#72dcdc', '#9acdcb', '#77b1cc', '#448aca', '#86cefa', '#98d1ee',
					'#73d1f1',
					'#80a7dc'
				],
				avatarUrl: this.src,
				allowMp: false
			}
		},
		watch: {
			// 監聽頭像src的變化，赋值给内部的avatarUrl變量，因為圖片加載失败時，需要修改圖片的src為默認值
			// 而组件内部不能直接修改props的值，所以需要一個中間變量
			src: {
				immediate: true,
				handler(newVal) {
					this.avatarUrl = newVal
					// 如果没有傳src，则主動触發error事件，用于顯示默認的頭像，否则src為''空字符等的時候，會無内容展示
					if(!newVal) {
						this.errorHandler()
					}
				}
			}
		},
		computed: {
			imageStyle() {
				const style = {}
				return style
			}
		},
		created() {
			this.init()
		},
		emits: ["click"],
		methods: {
			addStyle,
			addUnit,
			random,
			init() {
				// 目前只有這几個小程序平台具有open-data標簽
				// 其他平台可以通過uni.getUserInfo類似接口獲取信息，但是需要彈窗授權(首次)，不合符组件逻輯
				// 故目前自動獲取小程序頭像只支持這几個平台
				// #ifdef MP-WEIXIN || MP-QQ || MP-BAIDU
				this.allowMp = true
				// #endif
			},
			// 判斷傳入的name屬性，是否圖片路徑，只要带有"/"均認為是圖片形式
			isImg() {
				return this.src.indexOf('/') !== -1
			},
			// 圖片加載時失败時触發
			errorHandler() {
				this.avatarUrl = this.defaultUrl || base64Avatar
			},
			clickHandler() {
				this.$emit('click', this.name)
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-avatar {
		@include flex;
		align-items: center;
		justify-content: center;

		&--circle {
			border-radius: 100px;
		}

		&--square {
			border-radius: 4px;
		}

		&__image {
			&--circle {
				border-radius: 100px;
                overflow: hidden;
			}

			&--square {
				border-radius: 4px;
			}
		}
	}
</style>
