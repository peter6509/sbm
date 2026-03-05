<template>
	<view
		class="u-card"
		@tap.stop="click"
		:class="{ 'u-border': border, 'u-card-full': full, 'u-card--border': getPx(borderRadius) > 0 }"
		:style="{
			borderRadius: addUnit(borderRadius),
			margin: margin,
			boxShadow: boxShadow
		}"
	>
		<view
			v-if="showHead"
			class="u-card__head"
			:style="[{padding: addUnit(paddingHead || padding)}, headStyle]"
			:class="{
				'u-border-bottom': headBorderBottom
			}"
			@tap="headClick"
		>
			<view v-if="!$slots.head" class="u-flex u-row-between">
				<view class="u-card__head--left u-flex u-line-1" v-if="title">
					<image
						:src="thumb"
						class="u-card__head--left__thumb"
						mode="aspectFill"
						v-if="thumb"
						:style="{ 
							height: addUnit(thumbWidth), 
							width: addUnit(thumbWidth), 
							borderRadius: thumbCircle ? '50px' : '4px' 
						}"
					></image>
					<text
						class="u-card__head--left__title u-line-1"
						:style="{
							fontSize: addUnit(titleSize),
							color: titleColor
						}"
					>
						{{ title }}
					</text>
				</view>
				<view class="u-card__head--right u-line-1" v-if="subTitle">
					<text
						class="u-card__head__title__text"
						:style="{
							fontSize: addUnit(subTitleSize),
							color: subTitleColor
						}"
					>
						{{ subTitle }}
					</text>
				</view>
			</view>
			<slot name="head" v-else />
		</view>
		<view @tap="bodyClick" class="u-card__body"
			:style="[{padding: addUnit(paddingBody || padding)}, bodyStyle]"><slot name="body" /></view>
		<view
			v-if="showFoot"
			class="u-card__foot"
			 @tap="footClick"
			:style="[{padding: $slots.foot ? addUnit(paddingFoot || padding) : 0}, footStyle]"
			:class="{
				'u-border-top': footBorderTop
			}"
		>
			<slot name="foot" />
		</view>
	</view>
</template>

<script>
    import { propsCard } from './props';
    import { mpMixin } from '../../libs/mixin/mpMixin';
    import { mixin } from '../../libs/mixin/mixin';
    import { addStyle, addUnit, getPx } from '../../libs/function/index';
    /**
     * card 卡片
     * @description 卡片组件一般用于多個列表條目，且风格统一的场景
     * @tutorial https://uview-plus.jiangruyi.com/components/card.html
     * @property {Boolean} full 卡片與屏幕两侧是否留空隙（默認false）
     * @property {String} title 頭部左邊的標題
     * @property {String} title-color 標題顏色（默認#303133）
     * @property {String | Number} title-size 標題字體大小，單位rpx（默認15px）
     * @property {String} sub-title 頭部右邊的副標題
     * @property {String} sub-title-color 副標題顏色（默認#909399）
     * @property {String | Number} sub-title-size 副標題字體大小（默認13px
     * @property {Boolean} border 是否顯示邊框（默認true）
     * @property {String | Number} index 用于標识點擊了第几個卡片
     * @property {String} box-shadow 卡片外围阴影，字符串形式（默認none）
     * @property {String} margin 卡片與屏幕两邊和上下元素的間距，需带單位，如"30px 20px"（默認15px）
     * @property {String | Number} border-radius 卡片整體的圆角值，單位rpx（默認8px）
     * @property {Object} head-style 頭部自定義樣式，對象形式
     * @property {Object} body-style 中部自定義樣式，對象形式
     * @property {Object} foot-style 底部自定義樣式，對象形式
     * @property {Boolean} head-border-bottom 是否顯示頭部的下邊框（默認true）
     * @property {Boolean} foot-border-top 是否顯示底部的上邊框（默認true）
     * @property {Boolean} show-head 是否顯示頭部（默認true）
     * @property {Boolean} show-foot 是否顯示尾部（默認true）
     * @property {String} thumb 缩略圖路徑，如設置將顯示在標題的左邊，不建议使用相對路徑
     * @property {String | Number} thumb-width 缩略圖的寬度，高等于寬，單位px（默認30px）
     * @property {Boolean} thumb-circle 缩略圖是否為圆形（默認false）
     * @event {Function} click 整個卡片任意位置被點擊時触發
     * @event {Function} head-click 卡片頭部被點擊時触發
     * @event {Function} body-click 卡片主體部分被點擊時触發
     * @event {Function} foot-click 卡片底部部分被點擊時触發
     * @example <u-card paddingFoot="2px 15px" title="card"></u-card>
     */
    export default {
        name: 'up-card',
        data() {
            return {};
        },
        mixins: [mpMixin, mixin, propsCard],
        emits: ['click', 'head-click', 'body-click', 'foot-click'],
        methods: {
			addStyle,
			addUnit,
			getPx,
            click() {
                this.$emit('click', this.index);
            },
            headClick() {
                this.$emit('head-click', this.index);
            },
            bodyClick() {
                this.$emit('body-click', this.index);
            },
            footClick() {
                this.$emit('foot-click', this.index);
            }
        }
    };
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";
	
.u-card {
	position: relative;
	overflow: hidden;
	font-size: 28rpx;
	background-color: #ffffff;
	box-sizing: border-box;
	
	&-full {
		// 如果是與屏幕之間不留空隙，應該設置左右邊距為0
		margin-left: 0 !important;
		margin-right: 0 !important;
		width: 100%;
	}
	
	&--border:after {
		border-radius: 16rpx;
	}

	&__head {
		&--left {
			color: $u-main-color;
			
			&__thumb {
				margin-right: 16rpx;
			}
			
			&__title {
				max-width: 400rpx;
			}
		}

		&--right {
			color: $u-tips-color;
			margin-left: 6rpx;
		}
	}

	&__body {
		color: $u-content-color;
	}

	&__foot {
		color: $u-tips-color;
	}
}
</style>
