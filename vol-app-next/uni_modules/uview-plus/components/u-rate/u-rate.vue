<template>
    <view
        class="u-rate"
        :id="elId"
        ref="u-rate"
        :style="[addStyle(customStyle)]"
    >
        <view
            class="u-rate__content"
            @touchmove.stop="touchMove"
            @touchend.stop="touchEnd"
        >
            <view
                class="u-rate__content__item cursor-pointer"
                v-for="(item, index) in Number(count)"
                :key="index"
                :class="[elClass]"
            >
                <view
                    class="u-rate__content__item__icon-wrap"
                    ref="u-rate__content__item__icon-wrap"
                    @tap.stop="clickHandler($event, index + 1)"
                >
                    <u-icon
                        :name="
                            Math.floor(activeIndex) > index
                                ? activeIcon
                                : inactiveIcon
                        "
                        :color="
                            disabled
                                ? '#c8c9cc'
                                : Math.floor(activeIndex) > index
                                ? activeColor
                                : inactiveColor
                        "
                        :custom-style="{
                            padding: `0 ${addUnit(gutter / 2)}`,
                        }"
                        :size="size"
                    ></u-icon>
                </view>
                <view
                    v-if="allowHalf"
                    @tap.stop="clickHandler($event, index + 1)"
                    class="u-rate__content__item__icon-wrap u-rate__content__item__icon-wrap--half"
                    :style="[{
                        width: addUnit(rateWidth / 2),
                    }]"
                    ref="u-rate__content__item__icon-wrap"
                >
                    <u-icon
                        :name="
                            Math.ceil(activeIndex) > index
                                ? activeIcon
                                : inactiveIcon
                        "
                        :color="
                            disabled
                                ? '#c8c9cc'
                                : Math.ceil(activeIndex) > index
                                ? activeColor
                                : inactiveColor
                        "
                        :custom-style="{
                            padding: `0 ${addUnit(gutter / 2)}`
                        }"
                        :size="size"
                    ></u-icon>
                </view>
            </view>
        </view>
    </view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle, guid, sleep, range, os } from '../../libs/function/index';
	// #ifdef APP-NVUE
	const dom = weex.requireModule("dom");
	// #endif
	/**
	 * rate 评分
	 * @description 該组件一般用于满意度調查，星型评分的场景
	 * @tutorial https://ijry.github.io/uview-plus/components/rate.html
	 * @property {String | Number}	value			用于v-model双向绑定選中的星星數量 (默認 1 )
	 * @property {String | Number}	count			最多可選的星星數量 （默認 5 ）
	 * @property {Boolean}			disabled		是否禁止用户操作 （默認 false ）
	 * @property {Boolean}			readonly		是否只讀 （默認 false ）
	 * @property {String | Number}	size			星星的大小，單位px （默認 18 ）
	 * @property {String}			inactiveColor	未選中星星的顏色 （默認 '#b2b2b2' ）
	 * @property {String}			activeColor		選中的星星顏色 （默認 '#FA3534' ）
	 * @property {String | Number}	gutter			星星之間的距离 （默認 4 ）
	 * @property {String | Number}	minCount		最少選中星星的個數 （默認 1 ）
	 * @property {Boolean}			allowHalf		是否允许半星選擇 （默認 false ）
	 * @property {String}			activeIcon		選中時的圖標名，只能為uView的内置圖標 （默認 'star-fill' ）
	 * @property {String}			inactiveIcon	未選中時的圖標名，只能為uView的内置圖標 （默認 'star' ）
	 * @property {Boolean}			touchable		是否可以通過滑動手势選擇评分 （默認 'true' ）
	 * @property {Object}			customStyle		组件的樣式，對象形式
	 * @event {Function} change 選中的星星發生變化時触發
	 * @example <u-rate :count="count" :value="2"></u-rate>
	 */
	export default {
		name: "u-rate",
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				// 生成一個唯一id，否则一個頁面多個评分组件，會造成冲突
				elId: guid(),
				elClass: guid(),
				rateBoxLeft: 0, // 评分盒子左邊到屏幕左邊的距离，用于滑動選擇時計算距离
				// #ifdef VUE3
				activeIndex: this.modelValue,
				// #endif
				// #ifdef VUE2
				activeIndex: this.value,
				// #endif
				rateWidth: 0, // 每個星星的寬度
				// 標识是否正在滑動，由于iOS事件上touch比click先触發，导致快速滑動结束后，接着触發click，导致事件混乱而出错
				moving: false,
			};
		},
		watch: {
			// #ifdef VUE3
			modelValue(val) {
				this.activeIndex = val;
			},
			// #endif
        	// #ifdef VUE2
			value(val) {
				this.activeIndex = val;
			},
			// #endif
			activeIndex: 'emitEvent'
		},
		// #ifdef VUE3
		emits: ['update:modelValue', 'change'],
    	// #endif
		methods: {
			addStyle,
			addUnit,
			init() {
				sleep().then(() => {
					this.getRateItemRect();
					this.getRateIconWrapRect();
				})
			},
			// 獲取评分组件盒子的布局信息
			async getRateItemRect() {
				await sleep();
				// uView封裝的獲取節點的方法，詳見文檔
				// #ifndef APP-NVUE
				this.$uGetRect("#" + this.elId).then((res) => {
					this.rateBoxLeft = res.left;
				});
				// #endif
				// #ifdef APP-NVUE
				dom.getComponentRect(this.$refs["u-rate"], (res) => {
					this.rateBoxLeft = res.size.left;
				});
				// #endif
			},
			// 獲取單個星星的尺寸
			getRateIconWrapRect() {
				// uView封裝的獲取節點的方法，詳見文檔
				// #ifndef APP-NVUE
				this.$uGetRect("." + this.elClass).then((res) => {
					this.rateWidth = res.width;
				});
				// #endif
				// #ifdef APP-NVUE
				dom.getComponentRect(
					this.$refs["u-rate__content__item__icon-wrap"][0],
					(res) => {
						this.rateWidth = res.size.width;
					}
				);
				// #endif
			},
			// 手指滑動
			touchMove(e) {
				// 如果禁止通過手動滑動選擇，返回
				if (!this.touchable) {
					return;
				}
				this.preventEvent(e);
				const x = e.changedTouches[0].pageX;
				this.getActiveIndex(x);
			},
			// 停止滑動
			touchEnd(e) {
				// 如果禁止通過手動滑動選擇，返回
				if (!this.touchable) {
					return;
				}
				this.preventEvent(e);
				const x = e.changedTouches[0].pageX;
				this.getActiveIndex(x);
			},
			// 通過點擊，直接選中
			clickHandler(e, index) {
				// ios上，moving狀態取消事件触發
				if (os() === "ios" && this.moving) {
					return;
				}
				this.preventEvent(e);
				let x = 0;
				// 點擊時，在nvue上，無法獲得點擊的坐標，所以無法實現點擊半星選擇
				// #ifndef APP-NVUE
				x = e.changedTouches[0].pageX;
				// #endif
				// #ifdef APP-NVUE
				// nvue下，無法通過點擊獲得坐標信息，這里通過元素的位置尺寸值模拟坐標
				x = index * this.rateWidth + this.rateBoxLeft;
				// #endif
				this.getActiveIndex(x,true);
			},
			// 發出事件
			emitEvent() {
				// 發出change事件
				this.$emit("change", this.activeIndex);
				// 同時修改双向绑定的值
				// #ifdef VUE3
                this.$emit("update:modelValue", this.activeIndex);
                // #endif
                // #ifdef VUE2
				this.$emit("input", this.activeIndex);
				// #endif
			},
			// 獲取當前激活的评分圖標
			getActiveIndex(x,isClick = false) {
				if (this.disabled || this.readonly) {
					return;
				}
				// 判斷當前操作的點的x坐標值，是否在允许的邊界範圍内
				const allRateWidth = this.rateWidth * this.count + this.rateBoxLeft;
				// 如果小于第一個圖標的左邊界，設置為最小值，如果大于所有圖標的寬度，则設置為最大值
				x = range(this.rateBoxLeft, allRateWidth, x) - this.rateBoxLeft
				// 滑動點相對于评分盒子左邊的距离
				const distance = x;
				// 滑動的距离，相當于多少颗星星
				let index;
				// 判斷是否允许半星
				if (this.allowHalf) {
					index = Math.floor(distance / this.rateWidth);
					// 取餘，判斷小數的區間範圍
					const decimal = distance % this.rateWidth;
					if (decimal <= this.rateWidth / 2 && decimal > 0) {
						index += 0.5;
					} else if (decimal > this.rateWidth / 2) {
						index++;
					}
				} else {
					index = Math.floor(distance / this.rateWidth);
					// 取餘，判斷小數的區間範圍
					const decimal = distance % this.rateWidth;
					// 非半星時，只有超過了圖標的一半距离，才認為是選擇了這颗星
					if (isClick){
						if (decimal > 0) index++;
					} else {
						if (decimal > this.rateWidth / 2) index++;
					}

				}
				this.activeIndex = Math.min(index, this.count);
				// 對最少颗星星的限制
				if (this.activeIndex < this.minCount) {
					this.activeIndex = this.minCount;
				}

				// 設置延時為了让click事件在touchmove之前触發
				setTimeout(() => {
					this.moving = true;
				}, 10);
				// 一定時間后，取消標识為移動中狀態，是為了让click事件無效
				setTimeout(() => {
					this.moving = false;
				}, 10);
			},
		},
		mounted() {
			this.init();
		},
	};
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";
$u-rate-margin: 0 !default;
$u-rate-padding: 0 !default;
$u-rate-item-icon-wrap-half-top: 0 !default;
$u-rate-item-icon-wrap-half-left: 0 !default;

.u-rate {
    @include flex;
    align-items: center;
    margin: $u-rate-margin;
    padding: $u-rate-padding;
    /* #ifndef APP-NVUE */
    touch-action: none;
    /* #endif */

    &__content {
        @include flex;

		&__item {
		    position: relative;

		    &__icon-wrap {
		        &--half {
		            position: absolute;
		            overflow: hidden;
		            top: $u-rate-item-icon-wrap-half-top;
		            left: $u-rate-item-icon-wrap-half-left;
		        }
		    }
		}
    }
}

.u-icon {
    /* #ifndef APP-NVUE */
    box-sizing: border-box;
    /* #endif */
}
</style>
