<template>
	<view class="u-tabs" :class="[customClass]">
		<view class="u-tabs__wrapper">
			<slot name="left" />
			<view class="u-tabs__wrapper__scroll-view-wrapper">
				<scroll-view
					:scroll-x="scrollable"
					:scroll-left="scrollLeft"
					scroll-with-animation
					class="u-tabs__wrapper__scroll-view"
					:show-scrollbar="false"
					ref="u-tabs__wrapper__scroll-view"
				>
					<view
						class="u-tabs__wrapper__nav"
						ref="u-tabs__wrapper__nav"
					>
						<view
							class="u-tabs__wrapper__nav__item"
							v-for="(item, index) in list"
							:key="index"
							@tap="clickHandler(item, index)"
							@longpress="longPressHandler(item,index)"
							:ref="`u-tabs__wrapper__nav__item-${index}`"
							:style="[addStyle(itemStyle), {flex: scrollable ? '' : 1}]"
							:class="[`u-tabs__wrapper__nav__item-${index}`, item.disabled && 'u-tabs__wrapper__nav__item--disabled']"
						>
							<slot v-if="$slots.content" name="content" :item="item" :keyName="keyName" :index="index" />
							<slot v-else-if="!$slots.content && ($slots.default || $slots.$default)"
								:item="item" :keyName="keyName" :index="index" />
							<text v-else
								:class="[item.disabled && 'u-tabs__wrapper__nav__item__text--disabled']"
								class="u-tabs__wrapper__nav__item__text"
								:style="[textStyle(index)]"
							>{{ item[keyName] }}</text>
							<u-badge
								:show="!!(item.badge && (item.badge.show || item.badge.isDot || item.badge.value))"
								:isDot="item.badge && item.badge.isDot || propsBadge.isDot"
								:value="item.badge && item.badge.value || propsBadge.value"
								:max="item.badge && item.badge.max || propsBadge.max"
								:type="item.badge && item.badge.type || propsBadge.type"
								:showZero="item.badge && item.badge.showZero || propsBadge.showZero"
								:bgColor="item.badge && item.badge.bgColor || propsBadge.bgColor"
								:color="item.badge && item.badge.color || propsBadge.color"
								:shape="item.badge && item.badge.shape || propsBadge.shape"
								:numberType="item.badge && item.badge.numberType || propsBadge.numberType"
								:inverted="item.badge && item.badge.inverted || propsBadge.inverted"
								customStyle="margin-left: 4px;"
							></u-badge>
						</view>
						<!-- #ifdef APP-NVUE -->
						<view
							class="u-tabs__wrapper__nav__line"
							ref="u-tabs__wrapper__nav__line"
							:style="[{
								width: addUnit(lineWidth),
								height: addUnit(lineHeight),
								background: lineColor,
								backgroundSize: lineBgSize,
							}]"
						>
						</view>
						<!-- #endif -->
						<!-- #ifndef APP-NVUE -->
						<view
							class="u-tabs__wrapper__nav__line"
							ref="u-tabs__wrapper__nav__line"
							:style="[{
								width: addUnit(lineWidth),
								transform: `translate(${lineOffsetLeft}px)`,
								transitionDuration: `${firstTime ? 0 : duration}ms`,
								height: addUnit(lineHeight),
								background: lineColor,
								backgroundSize: lineBgSize,
							}]"
						>
						</view>
						<!-- #endif -->
					</view>
				</scroll-view>
			</view>
			<slot name="right" />
		</view>
	</view>
</template>

<script>
	// #ifdef APP-NVUE
	const animation = uni.requireNativePlugin('animation')
	const dom = uni.requireNativePlugin('dom')
	// #endif
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import defProps from '../../libs/config/props.js'
	import { addUnit, addStyle, deepMerge, getPx, sleep, sys } from '../../libs/function/index';
	/**
	 * Tabs 標簽
	 * @description tabs標簽组件，在標簽多的時候，可以配置為左右滑動，標簽少的時候，可以禁止滑動。 該组件的一個特點是配置為滾動模式時，激活的tab會自動移動到组件的中間位置。
	 * @tutorial https://ijry.github.io/uview-plus/components/tabs.html
	 * @property {String | Number}	duration			滑块移動一次所需的時間，單位秒（默認 200 ）
	 * @property {String | Number}	swierWidth			swiper的寬度（默認 '750rpx' ）
	 * @property {String}	keyName	 從`list`元素對象中讀取的鍵名（默認 'name' ）
	 * @event {Function(index)} change 標簽改變時触發 index: 點擊了第几個tab，索引從0開始
	 * @event {Function(index)} click 點擊標簽時触發 index: 點擊了第几個tab，索引從0開始
	 * @event {Function(index)} longPress 長按標簽時触發 index: 點擊了第几個tab，索引從0開始
	 * @example <u-tabs :list="list" :is-scroll="false" :current="current" @change="change" @longPress="longPress"></u-tabs>
	 */
	export default {
		name: 'u-tabs',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				firstTime: true,
				scrollLeft: 0,
				scrollViewWidth: 0,
				lineOffsetLeft: 0,
				tabsRect: {
					left: 0
				},
				innerCurrent: 0,
				moving: false,
			}
		},
		watch: {
			current: {
				immediate: true,
				handler (newValue, oldValue) {
					// 内外部值不相等時，才尝试移動滑块
					if (newValue !== this.innerCurrent) {
						if (typeof newValue == 'string') {
							this.innerCurrent = parseInt(newValue)
						} else {
							this.innerCurrent = newValue
						}
						this.$nextTick(() => {
							this.resize()
						})
					}
				}
			},
			// list變化時，重新渲染list各項信息
			list() {
				this.$nextTick(() => {
					this.resize()
				})
			}
		},
		computed: {
			textStyle() {
				return index => {
					const style = {}
					// 取當期是否激活的樣式
					const customeStyle = (index == this.innerCurrent)
						? addStyle(this.activeStyle) 
						: addStyle(this.inactiveStyle)
					// 如果當前菜單被禁用，则加上對應顏色，需要在此做處理，是因為nvue下，無法在style樣式中通過!import覆蓋標簽的内聯樣式
					if (this.list[index].disabled) {
						style.color = '#c8c9cc'
					}
					return deepMerge(customeStyle, style)
				}
			},
			propsBadge() {
				return defProps.badge
			}
		},
		async mounted() {
			this.init()
		},
		emits: ['click', 'longPress', 'change', 'update:current'],
		methods: {
			addStyle,
			addUnit,
			setLineLeft() {
				const tabItem = this.list[this.innerCurrent];
				if (!tabItem) {
					return;
				}
				// 獲取滑块該移動的位置
				let lineOffsetLeft = this.list
					.slice(0, this.innerCurrent)
					.reduce((total, curr) => total + curr.rect.width, 0);
                // 獲取下划线的數值px表示法
				const lineWidth = getPx(this.lineWidth);
				this.lineOffsetLeft = lineOffsetLeft + (tabItem.rect.width - lineWidth) / 2
				// #ifdef APP-NVUE
				// 第一次移動滑块，無需過渡時間
				this.animation(this.lineOffsetLeft, this.firstTime ? 0 : parseInt(this.duration))
				// #endif

				// 如果是第一次執行此方法，让滑块在初始化時，瞬間滑動到第一個tab item的中間
				// 這里需要一個定時器，因為在非nvue下，是直接通過style绑定過渡時間，需要等其過渡完成后，再設置為false(非第一次移動滑块)
				if (this.firstTime) {
					setTimeout(() => {
						this.firstTime = false
					}, 10);
				}
			},
			// nvue下設置滑块的位置
			animation(x, duration = 0) {
				// #ifdef APP-NVUE
				const ref = this.$refs['u-tabs__wrapper__nav__line']
				animation.transition(ref, {
					styles: {
						transform: `translateX(${x}px)`
					},
					duration
				})
				// #endif
			},
			// 點擊某一個標簽
			clickHandler(item, index) {
				// 因為標簽可能為disabled狀態，所以click是一定會發出的，但是change事件是需要可用的狀態才發出
				this.$emit('click', {
					...item,
					index
				}, index)
				// 如果disabled狀態，返回
				if (item.disabled) return
				this.innerCurrent = index
				this.resize()
				this.$emit('update:current', index)
				this.$emit('change', {
					...item,
					index
				}, index)
			},
			// 長按事件
			longPressHandler(item, index) {
				this.$emit('longPress', {
					...item,
					index
				})
			},
			init() {
				sleep().then(() => {
					this.resize()
				})
			},
			setScrollLeft() {
				// 當前活動tab的布局信息，有tab菜單的width和left(為元素左邊界到父元素左邊界的距离)等信息
				if (this.innerCurrent < 0) {
                    this.innerCurrent = 0;
                }
				const tabRect = this.list[this.innerCurrent]
				// 累加得到當前item到左邊的距离
				const offsetLeft = this.list
					.slice(0, this.innerCurrent)
					.reduce((total, curr) => {
						return total + curr.rect.width
					}, 0)
				// 此處為屏幕寬度
				const windowWidth = sys().windowWidth
				// 將活動的tabs-item移動到屏幕正中間，實际上是對scroll-view的移動
				let scrollLeft = offsetLeft - (this.tabsRect.width - tabRect.rect.width) / 2 - (windowWidth - this.tabsRect
					.right) / 2 + this.tabsRect.left / 2
				// 這里做一個限制，限制scrollLeft的最大值為整個scroll-view寬度减去tabs组件的寬度
				scrollLeft = Math.min(scrollLeft, this.scrollViewWidth - this.tabsRect.width)
				this.scrollLeft = Math.max(0, scrollLeft)
			},
			// 獲取所有標簽的尺寸
			resize() {
				// 如果不存在list，则不處理
				if(this.list.length === 0) {
					return
				}
				Promise.all([this.getTabsRect(), this.getAllItemRect()]).then(([tabsRect, itemRect = []]) => {
					// 兼容在swiper组件中使用
					if (tabsRect.left > tabsRect.width) {
						tabsRect.right = tabsRect.right - Math.floor(tabsRect.left / tabsRect.width) * tabsRect.width
						tabsRect.left = tabsRect.left % tabsRect.width
					}
					// console.log(tabsRect)
					this.tabsRect = tabsRect
					this.scrollViewWidth = 0
					itemRect.map((item, index) => {
						// 計算scroll-view的寬度，這里
						this.scrollViewWidth += item.width
						// 另外計算每一個item的中心點X軸坐標
						this.list[index].rect = item
					})
					// 獲取了tabs的尺寸之后，設置滑块的位置
					this.setLineLeft()
					this.setScrollLeft()
				})
			},
			// 獲取导航菜單的尺寸
			getTabsRect() {
				return new Promise(resolve => {
					this.queryRect('u-tabs__wrapper__scroll-view').then(size => resolve(size))
				})
			},
			// 獲取所有標簽的尺寸
			getAllItemRect() {
				return new Promise(resolve => {
					const promiseAllArr = this.list.map((item, index) => this.queryRect(
						`u-tabs__wrapper__nav__item-${index}`, true))
					Promise.all(promiseAllArr).then(sizes => resolve(sizes))
				})
			},
			// 獲取各個標簽的尺寸
			queryRect(el, item) {
				// #ifndef APP-NVUE
				// $uGetRect為uView自带的節點查詢簡化方法，詳見文檔介绍：https://ijry.github.io/uview-plus/js/getRect.html
				// 组件内部一般用this.$uGetRect，對外的為uni.$u.getRect，二者功能一致，名稱不同
				return new Promise(resolve => {
					this.$uGetRect(`.${el}`).then(size => {
						resolve(size)
					})
				})
				// #endif

				// #ifdef APP-NVUE
				// nvue下，使用dom模块查詢元素高度
				// 返回一個promise，让調用此方法的主體能使用then回調
				return new Promise(resolve => {
					dom.getComponentRect(item ? this.$refs[el][0] : this.$refs[el], res => {
						resolve(res.size)
					})
				})
				// #endif
			},
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-tabs {

		&__wrapper {
			@include flex;
			align-items: center;

			&__scroll-view-wrapper {
				flex: 1;
				/* #ifndef APP-NVUE */
				overflow: auto hidden;
				/* #endif */
			}

			&__scroll-view {
				@include flex;
				flex: 1;
			}

			&__nav {
				@include flex;
				position: relative;

				&__item {
					padding: 0 11px;
					@include flex;
					align-items: center;
					justify-content: center;
					/* #ifdef H5 */
					cursor: pointer;
					/* #endif */

					&--disabled {
						/* #ifdef H5 */
						cursor: not-allowed;
						/* #endif */
					}

					&__text {
						font-size: 15px;
						color: $u-content-color;
                        white-space: nowrap !important;

						&--disabled {
							color: $u-disabled-color !important;
						}
					}
				}

				&__line {
					height: 3px;
					background: $u-primary;
					width: 30px;
					position: absolute;
					bottom: 2px;
					border-radius: 100px;
					transition-property: transform;
					transition-duration: 300ms;
				}
			}
		}
	}
</style>
