<template>
	<view
	    class="u-search"
	    @tap="clickHandler"
	    :style="[{
			margin: margin,
		}, addStyle(customStyle)]"
	>
		<view
		    class="u-search__content"
		    :style="{
				backgroundColor: bgColor,
				borderRadius: shape == 'round' ? '100px' : '4px',
				borderColor: borderColor,
			}"
		>
			<template v-if="$slots.label || label !== null">
				<slot name="label">
					<text class="u-search__content__label">{{ label }}</text>
				</slot>
			</template>
			<view class="u-search__content__icon">
				<u-icon
					@tap="clickIcon"
				    :size="searchIconSize"
				    :name="searchIcon"
				    :color="searchIconColor ? searchIconColor : color"
				></u-icon>
			</view>
			<input
			    confirm-type="search"
			    @blur="blur"
			    :value="keyword"
			    @confirm="search"
			    @input="inputChange"
			    :disabled="disabled"
			    @focus="getFocus"
			    :focus="focus"
			    :maxlength="maxlength"
				:adjust-position="adjustPosition"
				:auto-blur="autoBlur"
			    placeholder-class="u-search__content__input--placeholder"
			    :placeholder="placeholder"
			    :placeholder-style="`color: ${placeholderColor}`"
			    class="u-search__content__input"
			    type="text"
			    :style="[{
					textAlign: inputAlign,
					color: color,
					backgroundColor: bgColor,
					height: addUnit(height)
				}, inputStyle]"
			/>
			<view
			    class="u-search__content__icon u-search__content__close"
			    v-if="keyword && clearabled && focused"
			    @click="clear"
			>
				<u-icon
				    name="close"
				    size="11"
				    color="#ffffff"
					customStyle="line-height: 12px"
				></u-icon>
			</view>
		</view>
		<text
		    :style="[actionStyle]"
		    class="u-search__action"
		    :class="[(showActionBtn || show) && 'u-search__action--active']"
		    @tap.stop.prevent="custom"
		>{{ actionText }}</text>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, addStyle } from '../../libs/function/index';
	/**
	 * search 搜索框
	 * @description 搜索组件，集成了常見搜索框所需功能，用户可以一鍵引入，開箱即用。
	 * @tutorial https://ijry.github.io/uview-plus/components/search.html
	 * @property {String}			shape				搜索框形狀，round-圆形，square-方形（默認 'round' ）
	 * @property {String}			bgColor				搜索框背景顏色（默認 '#f2f2f2' ）
	 * @property {String}			placeholder			占位文字内容（默認 '請輸入關鍵字' ）
	 * @property {Boolean}			clearabled			是否啟用清除控件（默認 true ）
	 * @property {Boolean}			focus				是否自動獲得焦點（默認 false ）
	 * @property {Boolean}			showAction			是否顯示右侧控件（默認 true ）
	 * @property {Object}			actionStyle			右侧控件的樣式，對象形式
	 * @property {String}			actionText			右侧控件文字（默認 '搜索' ）
	 * @property {String}			inputAlign			輸入框内容水平對齊方式 （默認 'left' ）
	 * @property {Object}			inputStyle			自定義輸入框樣式，對象形式
	 * @property {Boolean}			disabled			是否啟用輸入框（默認 false ）
	 * @property {String}			borderColor			邊框顏色，配置了顏色，才會有邊框 (默認 'transparent' )
	 * @property {String}			searchIconColor		搜索圖標的顏色，默認同輸入框字體顏色 (默認 '#909399' )
	 * @property {Number | String}	searchIconSize 搜索圖標的字體，默認22
	 * @property {String}			color				輸入框字體顏色（默認 '#606266' ）
	 * @property {String}			placeholderColor	placeholder的顏色（默認 '#909399' ）
	 * @property {String}			searchIcon			輸入框左邊的圖標，可以為uView圖標名稱或圖片路徑  (默認 'search' )
	 * @property {String}			margin				组件與其他上下左右元素之間的距离，带單位的字符串形式，如"30px"   (默認 '0' )
	 * @property {Boolean} 			animation			是否開啟動画，見上方說明（默認 false ）
	 * @property {String}			value				輸入框初始值
	 * @property {String | Number}	maxlength			輸入框最大能輸入的長度，-1為不限制長度  (默認 '-1' )
	 * @property {String | Number}	height				輸入框高度，單位px（默認 64 ）
	 * @property {String | Number}	label				搜索框左邊顯示内容
	 * @property {Boolean}	        adjustPosition	    鍵盘彈起時，是否自動上推頁面	
	 * @property {Boolean}	        autoBlur	        鍵盘收起時，是否自動失去焦點		
	 * @property {Object}			customStyle			定義需要用到的外部樣式
	 *
	 * @event {Function} change 輸入框内容發生變化時触發
	 * @event {Function} search 用户確定搜索時触發，用户按回车鍵，或者手機鍵盘右下角的"搜索"鍵時触發
	 * @event {Function} custom 用户點擊右侧控件時触發
	 * @event {Function} clear 用户點擊清除按鈕時触發
	 * @example <u-search placeholder="日照香炉生紫烟" v-model="keyword"></u-search>
	 */
	export default {
		name: "u-search",
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				keyword: '',
				showClear: false, // 是否顯示右邊的清除圖標
				show: false,
				// 標记input當前狀態是否處于聚焦中，如果是，才會顯示右侧的清除控件
				focused: this.focus
				// 绑定輸入框的值
				// inputValue: this.value
			};
		},
		watch: {
			keyword(nVal) {
				// 双向绑定值，让v-model绑定的值双向變化
				// #ifdef VUE3
				this.$emit("update:modelValue", nVal);
				// #endif
				// #ifdef VUE2
				this.$emit('input', nVal);
				// #endif
				// 触發change事件，事件效果和v-model双向绑定的效果一樣，让用户多一個選擇
				this.$emit('change', nVal);
			},
			// #ifdef VUE3
			modelValue: {
				immediate: true,
				handler(nVal) {
					this.keyword = nVal;
				}
			},
			// #endif
			// #ifdef VUE2
			value: {
				immediate: true,
				handler(nVal) {
					this.keyword = nVal;
				}
			},
			// #endif
		},
		computed: {
			showActionBtn() {
				return !this.animation && this.showAction
			}
		},
		emits: ['clear', 'search', 'custom', 'focus', 'blur', 'click', 'clickIcon', 'update:modelValue', 'change'],
		methods: {
			addStyle,
			addUnit,
			// 目前HX2.6.9 v-model双向绑定無效，故監聽input事件獲取輸入框内容的變化
			inputChange(e) {
				this.keyword = e.detail.value;
			},
			// 清空輸入
			// 也可以作為用户通過this.$refs形式調用清空輸入框内容
			clear() {
				this.keyword = '';
				// 延后發出事件，避免在父组件監聽clear事件時，value為更新前的值(不為空)
				this.$nextTick(() => {
					this.$emit('clear');
				})
			},
			// 確定搜索
			search(e) {
				this.$emit('search', e.detail.value);
				try {
					// 收起鍵盘
					uni.hideKeyboard();
				} catch (e) {}
			},
			// 點擊右邊自定義按鈕的事件
			custom() {
				this.$emit('custom', this.keyword);
				try {
					// 收起鍵盘
					uni.hideKeyboard();
				} catch (e) {}
			},
			// 獲取焦點
			getFocus() {
				this.focused = true;
				// 開啟右侧搜索按鈕展開的動画效果
				if (this.animation && this.showAction) this.show = true;
				this.$emit('focus', this.keyword);
			},
			// 失去焦點
			blur() {
				// 最開始使用的是監聽圖標@touchstart事件，自從hx2.8.4后，此方法在微信小程序出错
				// 這里改為監聽點擊事件，手點擊清除圖標時，同時也發生了@blur事件，导致圖標消失而無法點擊，這里做一個延時
				setTimeout(() => {
					this.focused = false;
				}, 100)
				this.show = false;
				this.$emit('blur', this.keyword);
			},
			// 點擊搜索框，只有disabled=true時才發出事件，因為禁止了輸入，意味着是想跳轉真正的搜索頁
			clickHandler() {
				if (this.disabled) this.$emit('click');
			},
			// 點擊左邊圖標
			clickIcon(e) {
				this.$emit('clickIcon', this.keyword);
				try {
					// 收起鍵盘
					uni.hideKeyboard();
				} catch (e) {}
			}
		}
	}
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";
$u-search-content-padding: 0 10px !default;
$u-search-label-color: $u-main-color !default;
$u-search-label-font-size: 14px !default;
$u-search-label-margin: 0 4px !default;
$u-search-close-size: 20px !default;
$u-search-close-radius: 100px !default;
$u-search-close-bgColor: #C6C7CB !default;
$u-search-close-transform: scale(0.82) !default;
$u-search-input-font-size: 14px !default;
$u-search-input-margin: 0 5px !default;
$u-search-input-color: $u-main-color !default;
$u-search-input-placeholder-color: $u-tips-color !default;
$u-search-action-font-size: 14px !default;
$u-search-action-color: $u-main-color !default;
$u-search-action-width: 0 !default;
$u-search-action-active-width: 40px !default;
$u-search-action-margin-left: 5px !default;

/* #ifdef H5 */
// iOS15在H5下，hx的某些版本，input type=search時，會多了一個搜索圖標，进行移除
[type="search"]::-webkit-search-decoration {
    display: none;
}
/* #endif */

.u-search {
	@include flex(row);
	align-items: center;
	flex: 1;

	&__content {
		@include flex;
		align-items: center;
		padding: $u-search-content-padding;
		flex: 1;
		justify-content: space-between;
		border-width: 1px;
		border-color: transparent;
		border-style: solid;
		overflow: hidden;

		&__icon {
			@include flex;
			align-items: center;
		}

		&__label {
			color: $u-search-label-color;
			font-size: $u-search-label-font-size;
			margin: $u-search-label-margin;
		}

		&__close {
			width: $u-search-close-size;
			height: $u-search-close-size;
			border-top-left-radius: $u-search-close-radius;
			border-top-right-radius: $u-search-close-radius;
			border-bottom-left-radius: $u-search-close-radius;
			border-bottom-right-radius: $u-search-close-radius;
			background-color: $u-search-close-bgColor;
			@include flex(row);
			align-items: center;
			justify-content: center;
			transform: $u-search-close-transform;
		}

		&__input {
			flex: 1;
			font-size: $u-search-input-font-size;
			line-height: 1;
			margin: $u-search-input-margin;
			color: $u-search-input-color;

			&--placeholder {
				color: $u-search-input-placeholder-color;
			}
		}
	}

	&__action {
		font-size: $u-search-action-font-size;
		color: $u-search-action-color;
		width: $u-search-action-width;
		overflow: hidden;
		transition-property: width;
		transition-duration: 0.3s;
		/* #ifndef APP-NVUE */
		white-space: nowrap;
		/* #endif */
		text-align: center;

		&--active {
			width: $u-search-action-active-width;
			margin-left: $u-search-action-margin-left;
		}
	}
}
</style>
