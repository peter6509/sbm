<template>
	<view
	    class="u-line-progress"
	    :style="[addStyle(customStyle)]"
	>
		<view
		    class="u-line-progress__background"
		    ref="u-line-progress__background"
		    :style="[{
				backgroundColor: inactiveColor,
				height: addUnit(height),
			}]"
		>
		</view>
		<view
		    class="u-line-progress__line"
		    :style="[progressStyle]"
		> 
			<slot>
				<text v-if="showText && percentage >= 10" class="u-line-progress__text">{{innserPercentage + '%'}}</text>
			</slot> 
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, sleep, range } from '../../libs/function/index';
	// #ifdef APP-NVUE
	const dom = uni.requireNativePlugin('dom')
	// #endif
	/**
	 * lineProgress 线型进度條
	 * @description 展示操作或任务的當前进度，比如上傳文件，是一個线形的进度條。
	 * @tutorial https://ijry.github.io/uview-plus/components/lineProgress.html
	 * @property {String}			activeColor		激活部分的顏色 ( 默認 '#19be6b' )
	 * @property {String}			inactiveColor	背景色 ( 默認 '#ececec' )
	 * @property {String | Number}	percentage		进度百分比，數值 ( 默認 0 )
	 * @property {Boolean}			showText		是否在进度條内部顯示百分比的值 ( 默認 true )
	 * @property {String | Number}	height			进度條的高度，單位px ( 默認 12 )
	 * 
	 * @example <u-line-progress :percent="70" :show-percent="true"></u-line-progress>
	 */
	export default {
		name: "u-line-progress",
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				lineWidth: 0,
			}
		},
		watch: {
			percentage(n) {
				this.resizeProgressWidth()
			}
		},
		computed: {
			progressStyle() { 
				let style = {}
				style.width = this.lineWidth
				style.backgroundColor = this.activeColor
				style.height = addUnit(this.height)
				return style
			},
			innserPercentage() {
				// 控制範圍在0-100之間
				return range(0, 100, this.percentage)
			}
		},
		mounted() {
			this.init()
		},
		methods: {
			addStyle,
			addUnit,
			init() {
				sleep(20).then(() => {
					this.resizeProgressWidth()
				})
			},
			getProgressWidth() {
				// #ifndef APP-NVUE
				return this.$uGetRect('.u-line-progress__background')
				// #endif

				// #ifdef APP-NVUE
				// 返回一個promise
				return new Promise(resolve => {
					dom.getComponentRect(this.$refs['u-line-progress__background'], (res) => {
						resolve(res.size)
					})
				})
				// #endif
			},
			resizeProgressWidth() {
				this.getProgressWidth().then(size => {
					const {
						width
					} = size
					// 通過設置的percentage值，計算其所占總長度的百分比
					this.lineWidth = width * this.innserPercentage / 100 + 'px'
				})
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-line-progress {
		align-items: stretch;
		position: relative;
		@include flex(row);
		flex: 1;
		overflow: hidden;
		border-radius: 100px;

		&__background {
			background-color: #ececec;
			border-radius: 100px;
			flex: 1;
		}

		&__line {
			position: absolute;
			top: 0;
			left: 0;
			bottom: 0;
			align-items: center;
			@include flex(row);
			color: #ffffff;
			border-radius: 100px;
			transition: width 0.5s ease;
			justify-content: flex-end;
		}

		&__text {
			font-size: 10px;
			align-items: center;
			text-align: right;
			color: #FFFFFF;
			margin-right: 5px;
			transform: scale(0.9);
		}
	}
</style>
