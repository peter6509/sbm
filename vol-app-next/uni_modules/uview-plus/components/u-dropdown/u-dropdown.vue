<template>
	<view class="u-dropdown">
		<view class="u-dropdown__menu" :style="{
			height: addUnit(height)
		}" :class="{
			'u-border-bottom': borderBottom
		}">
			<view class="u-dropdown__menu__item" v-for="(item, index) in menuList" :key="index" @tap.stop="menuClick(index)">
				<view class="u-flex u-flex-row">
					<text class="u-dropdown__menu__item__text" :style="{
						color: item.disabled ? '#c0c4cc' : (index === current || highlightIndex == index) ? activeColor : inactiveColor,
						fontSize: addUnit(titleSize)
					}">{{item.title}}</text>
					<view class="u-dropdown__menu__item__arrow" :class="{
						'u-dropdown__menu__item__arrow--rotate': index === current
					}">
						<u-icon :custom-style="{display: 'flex'}" :name="menuIcon" :size="addUnit(menuIconSize)" :color="index === current || highlightIndex == index ? activeColor : '#c0c4cc'"></u-icon>
					</view>
				</view>
			</view>
		</view>
		<view class="u-dropdown__content" :style="[contentStyle, {
			transition: `opacity ${duration / 1000}s linear`,
			top: addUnit(height),
			height: contentHeight + 'px'
		}]"
		 @tap="maskClick" @touchmove.stop.prevent>
			<view @tap.stop.prevent class="u-dropdown__content__popup" :style="[popupStyle]">
				<slot></slot>
			</view>
			<view class="u-dropdown__content__mask"></view>
		</view>
	</view>
</template>

<script>
    import { props } from './props';
    import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, sys} from '../../libs/function/index';
	/**
	 * dropdown 下拉菜單
	 * @description 該组件一般用于向下展開菜單，同時可切換多個選項卡的场景
	 * @tutorial https://ijry.github.io/uview-plus/components/dropdown.html
	 * @property {String} active-color 標題和選項卡選中的顏色（默認#2979ff）
	 * @property {String} inactive-color 標題和選項卡未選中的顏色（默認#606266）
	 * @property {Boolean} close-on-click-mask 點擊遮罩是否關闭菜單（默認true）
	 * @property {Boolean} close-on-click-self 點擊當前激活項標題是否關闭菜單（默認true）
	 * @property {String | Number} duration 選項卡展開和收起的過渡時間，單位ms（默認300）
	 * @property {String | Number} height 標題菜單的高度，單位任意（默認80）
	 * @property {String | Number} border-radius 菜單展開内容下方的圆角值，單位任意（默認0）
	 * @property {Boolean} border-bottom 標題菜單是否顯示下邊框（默認false）
	 * @property {String | Number} title-size 標題的字體大小，單位任意，數值默認為rpx單位（默認28）
	 * @event {Function} open 下拉菜單被打開時触發
	 * @event {Function} close 下拉菜單被關闭時触發
	 * @example <u-dropdown></u-dropdown>
	 */
	export default {
		name: 'u-dropdown',
        mixins: [mpMixin, mixin, props],
		data() {
			return {
				showDropdown: true, // 是否打開下来菜單,
				menuList: [], // 顯示的菜單
				active: false, // 下拉菜單的狀態
				// 當前是第几個菜單處于激活狀態，小程序中此處不能写成false或者""，否则后续將current赋值為0，
				// 無能的TX没有使用===而是使用==判斷，导致程序認為前后二者没有變化，從而不會触發视圖更新
				current: 99999,
				// 外層内容的樣式，初始時處于底層，且透明
				contentStyle: {
					zIndex: -1,
					opacity: 0
				},
				// 让某個菜單保持高亮的狀態
				highlightIndex: 99999,
				contentHeight: 0
			}
		},
		computed: {
			// 下拉出来部分的樣式
			popupStyle() {
				let style = {};
				// 进行Y軸位移，展開狀態時，恢複原位。收齊狀態時，往上位移100%，进行隐藏
				style.transform = `translateY(${this.active ? 0 : '-100%'})`
				style['transition-duration'] = this.duration / 1000 + 's';
				style.borderRadius = `0 0 ${addUnit(this.borderRadius)} ${addUnit(this.borderRadius)}`;
				return style;
			}
		},
		created() {
			// 引用所有子组件(u-dropdown-item)的this，不能在data中声明變量，否则在微信小程序會造成循环引用而報错
			this.children = [];
		},
		mounted() {
			this.getContentHeight();
		},
        emits: ['open', 'close'],
		methods: {
			addUnit,
			init() {
				// 當某個子组件内容變化時，触發父组件的init，父组件再让每一個子组件重新初始化一遍
				// 以保证數據的正確性
				this.menuList = [];
				this.children.map(child => {
					child.init();
				})
			},
			// 點擊菜單
			menuClick(index) {
				// 判斷是否被禁用
				if (this.menuList[index].disabled) return;
				// 如果點擊時的索引和當前激活項索引相同，意味着點擊了激活項，需要收起下拉菜單
				if (index === this.current && this.closeOnClickSelf) {
					this.close();
					// 等動画结束后，再移除下拉菜單中的内容，否则直接移除，也就没有下拉菜單收起的效果了
					setTimeout(() => {
						this.children[index].active = false;
					}, this.duration)
					return;
				}
				this.open(index);
			},
			// 打開下拉菜單
			open(index) {
				// 嵌套popup使用時可能獲取不到正確的高度，重新計算
				if (this.contentHeight < 1) this.getContentHeight()
				// 重置高亮索引，否则會造成多個菜單同時高亮
				// this.highlightIndex = 9999;
				// 展開時，設置下拉内容的樣式
				this.contentStyle = {
					zIndex: 11,
				}
				// 標记展開狀態以及當前展開項的索引
				this.active = true;
				this.current = index;
				// 历遍所有的子元素，將索引匹配的項標记為激活狀態，因為子元素是通過v-if控制切換的
				// 之所以不是因display: none，是因為nvue没有display這個屬性
				this.children.map((val, idx) => {
					val.active = index == idx ? true : false;
				})
				this.$emit('open', this.current);
			},
			// 設置下拉菜單處于收起狀態
			close() {
				this.$emit('close', this.current);
				// 設置為收起狀態，同時current归位，設置為空字符串
				this.active = false;
				this.current = 99999;
				// 下拉内容的樣式进行調整，不透明度設置為0
				this.contentStyle = {
					zIndex: -1,
					opacity: 0
				}
			},
			// 點擊遮罩
			maskClick() {
				// 如果不允许點擊遮罩，直接返回
				if (!this.closeOnClickMask) return;
				this.close();
			},
			// 外部手動設置某個菜單高亮
			highlight(index = undefined) {
				this.highlightIndex = index !== undefined ? index : 99999;
			},
			// 獲取下拉菜單内容的高度
			getContentHeight() {
				// 這里的原理為，因為dropdown组件是相對定位的，它的下拉出来的内容，必須给定一個高度
				// 才能让遮罩占满菜單一下，直到屏幕底部的高度
				// sys()為uview-plus封裝的獲取設備信息的方法
				let windowHeight = sys().windowHeight;
				this.$uGetRect('.u-dropdown__menu').then(res => {
					// 這里獲取的是dropdown的尺寸，在H5上，uniapp獲取尺寸是有bug的(以前提出修複過，后来又出現了此bug，目前hx2.8.11版本)
					// H5端bug表現為元素尺寸的top值為导航栏底部到到元素的上邊沿的距离，但是元素的bottom值確是导航栏顶部到元素底部的距离
					// 二者是互相矛盾的，本质原因是H5端导航栏非原生，uni的開發者大意造成
					// 這里取菜單栏的botton值合理的，不能用res.top，否则頁面會造成滾動
					this.contentHeight = windowHeight - res.bottom;
				})
			}
		}
	}
</script>

<style scoped lang="scss">
	@import "../../libs/css/components.scss";

	.u-dropdown {
		flex: 1;
		width: 100%;
		position: relative;

		&__menu {
			@include flex;
			position: relative;
			z-index: 11;
			height: 80rpx;

			&__item {
				flex: 1;
				@include flex;
				justify-content: center;
				align-items: center;

                .u-flex-row {
                    flex-direction: row;
                }

				&__text {
					font-size: 28rpx;
					color: $u-content-color;
				}

				&__arrow {
					margin-left: 6rpx;
					transition: transform .3s;
					align-items: center;
					@include flex;

					&--rotate {
						transform: rotate(180deg);
					}
				}
			}
		}

		&__content {
			position: absolute;
			z-index: 8;
			width: 100%;
			left: 0px;
			bottom: 0;
			overflow: hidden;
			

			&__mask {
				position: absolute;
				z-index: 9;
				background: rgba(0, 0, 0, .3);
				width: 100%;
				left: 0;
				top: 0;
				bottom: 0;
			}

			&__popup {
				position: relative;
				z-index: 10;
				transition: transform 0.3s;
				transform: translate3D(0, -100%, 0);
				overflow: hidden;
			}
		}

	}
</style>
