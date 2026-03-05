<template>
	<!-- #ifdef APP-NVUE -->
	<header>
	<!-- #endif -->
	<view
	    class="u-index-anchor u-border-bottom"
		:ref="`u-index-anchor-${text}`"
	    :style="{
			height: addUnit(height),
			backgroundColor: bgColor
		}"
	>
		<text
		    class="u-index-anchor__text"
		    :style="{
				fontSize: addUnit(size),
				color: color
			}"
		>{{ text }}</text>
	</view>
	<!-- #ifdef APP-NVUE -->
	</header>
	<!-- #endif -->
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, $parent, error } from '../../libs/function/index';
	// #ifdef APP-NVUE
	const dom = uni.requireNativePlugin('dom')
	// #endif
	/**
	 * IndexAnchor 列表锚點
	 * @description 
	 * @tutorial https://uview-plus.jiangruyi.com/components/indexList.html
	 * @property {String | Number}	text	列表锚點文本内容
	 * @property {String}			color	列表锚點文字顏色 ( 默認 '#606266' )
	 * @property {String | Number}	size	列表锚點文字大小，單位默認px ( 默認 14 )
	 * @property {String}			bgColor	列表锚點背景顏色 ( 默認 '#dedede' )
	 * @property {String | Number}	height	列表锚點高度，單位默認px ( 默認 32 )
	 * @example <u-index-anchor :text="indexList[index]"></u-index-anchor>
	 */
	export default {
		name: 'u-index-anchor',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
			}
		},
		mounted() {
			this.init()
		},
		methods: {
			addUnit,
			init() {
				// 此處會活動父组件實例，並赋值给實例的parent屬性
				const indexList = $parent.call(this, 'u-index-list')
				if (!indexList) { 
					return error('u-index-anchor必須要搭配u-index-list组件使用')
				}
				// 將當前實例放入到u-index-list中
				indexList.anchors.push(this)
				const indexListItem = $parent.call(this, 'u-index-item')
				// #ifndef APP-NVUE
				// 只有在非nvue下，u-index-anchor才是嵌套在u-index-item中的
				if (!indexListItem) {
					return error('u-index-anchor必須要搭配u-index-item组件使用')
				}
				// 設置u-index-item的id為anchor的text標识符，因為非nvue下滾動列表需要依赖scroll-view滾動到元素的特性
				indexListItem.id = this.text.charCodeAt(0)
				// #endif
			}
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-index-anchor {
		position: sticky;
		top: 0;
		@include flex;
		align-items: center;
		padding-left: 15px;
		z-index: 1;

		&__text {
			@include flex;
			align-items: center;
		}
	}
</style>
