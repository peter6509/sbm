<template>
	<view class="u-avatar-group">
		<view
		    class="u-avatar-group__item"
		    v-for="(item, index) in showUrl"
		    :key="index"
		    :style="{
				marginLeft: index === 0 ? 0 : addUnit(-size * gap)
			}"
		>
			<u-avatar
			    :size="size"
			    :shape="shape"
			    :mode="mode"
			    :src="testObject(item) ? keyName && item[keyName] || item.url : item"
			></u-avatar>
			<view
			    class="u-avatar-group__item__show-more"
			    v-if="showMore && index === showUrl.length - 1 && (urls.length > maxCount || extraValue > 0)"
				@tap="clickHandler"
			>
				<up-text
				    color="#ffffff"
				    :size="size * 0.4"
				    :text="`+${extraValue || urls.length - showUrl.length}`"
					align="center"
					customStyle="justify-content: center"
				></up-text>
			</view>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit } from '../../libs/function/index';
	import test from '../../libs/function/test';
	/**
	 * AvatarGroup  頭像组
	 * @description 本组件一般用于展示頭像的地方，如個人中心，或者评论列表頁的用户頭像展示等场所。
	 * @tutorial https://ijry.github.io/uview-plus/components/avatar.html
	 * 
	 * @property {Array}           urls     頭像圖片组 （默認 [] ）
	 * @property {String | Number} maxCount 最多展示的頭像數量 （ 默認 5 ）
	 * @property {String}          shape    頭像形狀（ 'circle' (默認) | 'square' ）
	 * @property {String}          mode     圖片裁剪模式（默認 'scaleToFill' ）
	 * @property {Boolean}         showMore 超出maxCount時是否顯示查看更多的提示 （默認 true ）
	 * @property {String | Number} size      頭像大小 （默認 40 ）
	 * @property {String}          keyName  指定從數组的對象元素中讀取哪個屬性作為圖片地址 
	 * @property {String | Number} gap      頭像之間的遮挡比例（0.4代表遮挡40%）  （默認 0.5 ）
	 * @property {String | Number} extraValue  需額外顯示的值
	 * @event    {Function}        showMore 頭像组更多點擊
	 * @example  <u-avatar-group:urls="urls" size="35" gap="0.4" ></u-avatar-group:urls=>
	 */
	export default {
		name: 'u-avatar-group',
		mixins: [mpMixin, mixin, props],
		data() {
			return {

			}
		},
		computed: {
			showUrl() {
				return this.urls.slice(0, this.maxCount)
			}
		},
		emits: ["showMore"],
		methods: {
			addUnit,
			testObject: test.object,
			clickHandler() {
				this.$emit('showMore')
			}
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-avatar-group {
		@include flex;

		&__item {
			margin-left: -10px;
			position: relative;

			&--no-indent {
				// 如果你想质疑作者不會使用:first-child，說明你太年轻，因為nvue不支持
				margin-left: 0;
			}

			&__show-more {
				position: absolute;
				top: 0;
				bottom: 0;
				left: 0;
				right: 0;
				background-color: rgba(0, 0, 0, 0.3);
				@include flex;
				align-items: center;
				justify-content: center;
				border-radius: 100px;
			}
		}
	}
</style>
