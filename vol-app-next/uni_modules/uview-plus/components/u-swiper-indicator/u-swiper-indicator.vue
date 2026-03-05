<template>
	<view class="u-swiper-indicator">
		<view
			class="u-swiper-indicator__wrapper"
			v-if="indicatorMode === 'line'"
			:class="[`u-swiper-indicator__wrapper--${indicatorMode}`]"
			:style="{
				width: addUnit(lineWidth * length),
				backgroundColor: indicatorInactiveColor
			}"
		>
			<view
				class="u-swiper-indicator__wrapper--line__bar"
				:style="[lineStyle]"
			></view>
		</view>
		<view
			class="u-swiper-indicator__wrapper"
			v-if="indicatorMode === 'dot'"
		>
			<view
				class="u-swiper-indicator__wrapper__dot"
				v-for="(item, index) in length"
				:key="index"
				:class="[index === current && 'u-swiper-indicator__wrapper__dot--active']"
				:style="[dotStyle(index)]"
			>
			</view>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit } from '../../libs/function/index';
	/**
	 * SwiperIndicator 轮播圖指示器
	 * @description 該组件一般用于导航轮播，广告展示等场景,可開箱即用，
	 * @tutorial https://ijry.github.io/uview-plus/components/swiper.html
	 * @property {String | Number}	length					轮播的長度（默認 0 ）
	 * @property {String | Number}	current					當前處于活動狀態的轮播的索引（默認 0 ）
	 * @property {String}			indicatorActiveColor	指示器非激活顏色
	 * @property {String}			indicatorInactiveColor	指示器的激活顏色
	 * @property {String}			indicatorMode			指示器模式（默認 'line' ）
	 * @example	<u-swiper :list="list4" indicator keyName="url" :autoplay="false"></u-swiper>
	 */
	export default {
		name: 'u-swiper-indicator',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				lineWidth: 22
			}
		},
		computed: {
			// 指示器為线型的樣式
			lineStyle() {
				let style = {}
				style.width = addUnit(this.lineWidth)
				style.transform = `translateX(${ addUnit(this.current * this.lineWidth) })`
				style.backgroundColor = this.indicatorActiveColor
				return style
			},
			// 指示器為點型的樣式
			dotStyle() {
				return index => {
					let style = {}
					style.backgroundColor = index === this.current ? this.indicatorActiveColor : this.indicatorInactiveColor
					return style
				}
			}
		},
		methods: {
			addUnit
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-swiper-indicator {

		&__wrapper {
			@include flex;

			&--line {
				border-radius: 100px;
				height: 4px;

				&__bar {
					width: 22px;
					height: 4px;
					border-radius: 100px;
					background-color: #FFFFFF;
					transition: transform 0.3s;
				}
			}

			&__dot {
				width: 5px;
				height: 5px;
				border-radius: 100px;
				margin: 0 4px;

				&--active {
					width: 12px;
				}
			}

		}
	}
</style>
