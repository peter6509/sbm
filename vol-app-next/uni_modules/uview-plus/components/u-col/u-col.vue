<template>
	<view
	    class="u-col"
		ref="u-col"
	    :class="[
			'u-col-' + span
		]"
	    :style="[colStyle]"
	    @tap="clickHandler"
	>
		<slot></slot>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, addUnit, deepMerge, getPx } from '../../libs/function/index';
	/**
	 * CodeInput 栅格系统的列 
	 * @description 該组件一般用于Layout 布局 通過基礎的 12 分栏，迅速簡便地創建布局
	 * @tutorial https://ijry.github.io/uview-plus/components/Layout.html
	 * @property {String | Number}	span		栅格占據的列數，總12等份 (默認 12 ) 
	 * @property {String | Number}	offset		分栏左邊偏移，計算方式與span相同 (默認 0 ) 
	 * @property {String}			justify		水平排列方式，可選值為`start`(或`flex-start`)、`end`(或`flex-end`)、`center`、`around`(或`space-around`)、`between`(或`space-between`)  (默認 'start' ) 
	 * @property {String}			align		垂直對齊方式，可選值為top、center、bottom、stretch (默認 'stretch' ) 
	 * @property {String}			textAlign	文字水平對齊方式 (默認 'left' ) 
	 * @property {Object}			customStyle	定義需要用到的外部樣式
	 * @event {Function}	click	col被點擊，會阻止事件冒泡到row
	 * @example	 <u-col  span="3" offset="3" > <view class="demo-layout bg-purple"></view> </u-col>
	 */
	export default {
		name: 'u-col',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				width: 0,
				parentData: {
					gutter: 0
				},
				gridNum: 12
			}
		},
		//  微信小程序中 options 選項
		options: {
			virtualHost: true // 將自定義節點設置成虚拟的，更加接近Vue组件的表現。我们不希望自定義组件的這個節點本身可以設置樣式、响應 flex 布局等
		},
		computed: {
			uJustify() {
				if (this.justify == 'end' || this.justify == 'start') return 'flex-' + this.justify
				else if (this.justify == 'around' || this.justify == 'between') return 'space-' + this.justify
				else return this.justify
			},
			uAlignItem() {
				if (this.align == 'top') return 'flex-start'
				if (this.align == 'bottom') return 'flex-end'
				else return this.align
			},
			colStyle() {
				const style = {
					// 這里写成"padding: 0 10px"的形式是因為nvue的需要
					paddingLeft: addUnit(getPx(this.parentData.gutter)/2),
					paddingRight: addUnit(getPx(this.parentData.gutter)/2),
					alignItems: this.uAlignItem,
					justifyContent: this.uJustify,
					textAlign: this.textAlign,
					// #ifndef APP-NVUE
					// 在非nvue上，使用百分比形式
					flex: `0 0 ${100 / this.gridNum * this.span}%`,
					marginLeft: 100 / 12 * this.offset + '%',
					// #endif
					// #ifdef APP-NVUE
					// 在nvue上，由于無法使用百分比單位，這里需要獲取父组件的寬度，再計算得出該有對應的百分比尺寸
					width: addUnit(Math.floor(this.width / this.gridNum * Number(this.span))),
					marginLeft: addUnit(Math.floor(this.width / this.gridNum * Number(this.offset))),
					// #endif
				}
				return deepMerge(style, addStyle(this.customStyle))
			}
		},
		mounted() {
			this.init()
		},
		emits: ["click"],
		methods: {
			async init() {
				// 支付宝小程序不支持provide/inject，所以使用這個方法獲取整個父组件，在created定義，避免循环引用
				this.updateParentData()
				this.width = await this.parent.getComponentWidth()
			},
			updateParentData() {
				this.getParentData('u-row')
			},
			clickHandler(e) {
				this.$emit('click');
			}
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-col {
		padding: 0;
		/* #ifndef APP-NVUE */
		box-sizing:border-box;
		/* #endif */
		/* #ifdef MP */
		display: block;
		/* #endif */
	}

	// nvue下百分比無效
	/* #ifndef APP-NVUE */
	.u-col-0 {
		width: 0;
	}

	.u-col-1 {
		width: calc(100%/12);
	}

	.u-col-2 {
		width: calc(100%/12 * 2);
	}

	.u-col-3 {
		width: calc(100%/12 * 3);
	}

	.u-col-4 {
		width: calc(100%/12 * 4);
	}

	.u-col-5 {
		width: calc(100%/12 * 5);
	}

	.u-col-6 {
		width: calc(100%/12 * 6);
	}

	.u-col-7 {
		width: calc(100%/12 * 7);
	}

	.u-col-8 {
		width: calc(100%/12 * 8);
	}

	.u-col-9 {
		width: calc(100%/12 * 9);
	}

	.u-col-10 {
		width: calc(100%/12 * 10);
	}

	.u-col-11 {
		width: calc(100%/12 * 11);
	}

	.u-col-12 {
		width: calc(100%/12 * 12);
	}

	/* #endif */
</style>
