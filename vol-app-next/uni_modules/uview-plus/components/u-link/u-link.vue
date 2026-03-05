<template>
	<text
	    class="u-link"
	    @tap.stop="openLink"
	    :style="[linkStyle, addStyle(customStyle)]"
	>{{text}}</text>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, addUnit, getPx, toast } from '../../libs/function/index';
	/**
	 * link 超链接
	 * @description 該组件為超链接组件，在不同平台有不同表現形式：在APP平台會通過plus环境打開内置浏覽器，在小程序中把链接複制到粘貼板，同時提示信息，在H5中通過window.open打開链接。
	 * @tutorial https://ijry.github.io/uview-plus/components/link.html
	 * @property {String}			color		文字顏色 （默認 color['u-primary'] ）
	 * @property {String ｜ Number}	fontSize	字體大小，單位px （默認 15 ）
	 * @property {Boolean}			underLine	是否顯示下划线 （默認 false ）
	 * @property {String}			href		跳轉的链接，要带上http(s)
	 * @property {String}			mpTips		各個小程序平台把链接複制到粘貼板后的提示語（默認“链接已複制，請在浏覽器打開”）
	 * @property {String}			lineColor	下划线顏色，默認同color参數顏色 
	 * @property {String}			text		超链接的問題，不使用slot形式傳入，是因為nvue下無法修改顏色 
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * 
	 * @example <u-link href="http://www.uviewui.com">蜀道难，难于上青天</u-link>
	 */
	export default {
		name: "u-link",
		mixins: [mpMixin, mixin, props],
		computed: {
			linkStyle() {
				const style = {
					color: this.color,
					fontSize: addUnit(this.fontSize),
					// line-height設置為比字體大小多2px
					lineHeight: addUnit(getPx(this.fontSize) + 2),
					textDecoration: this.underLine ? 'underline' : 'none'
				}
				// if (this.underLine) {
				// 	style.borderBottomColor = this.lineColor || this.color
				// 	style.borderBottomWidth = '1px'
				// }
				return style
			}
		},
		emits: ["click"],
		methods: {
			addStyle,
			openLink() {
				// #ifdef APP-PLUS
				plus.runtime.openURL(this.href)
				// #endif
				// #ifdef H5
				window.open(this.href)
				// #endif
				// #ifdef MP
				uni.setClipboardData({
					data: this.href,
					success: () => {
						uni.hideToast();
						this.$nextTick(() => {
							toast(this.mpTips);
						})
					}
				});
				// #endif
				this.$emit('click')
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	$u-link-line-height:1 !default;

	.u-link {
		/* #ifndef APP-NVUE */
		line-height: $u-link-line-height;
		/* #endif */
		@include flex;
		flex-wrap: wrap;
		flex: 1;
	}
</style>
