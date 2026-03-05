<template>
	<view
	    class="u-checkbox-group"
	    :class="bemClass"
	>
		<slot></slot>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	/**
	 * checkboxGroup 複選框组
	 * @description 複選框组件一般用于需要多個選擇的场景，該组件功能完整，使用方便
	 * @tutorial https://ijry.github.io/uview-plus/components/checkbox.html
	 * @property {String}			name			標识符 
	 * @property {Array}			value			绑定的值
	 * @property {String}			shape			形狀，circle-圆形，square-方形 （默認 'square' ）
	 * @property {Boolean}			disabled		是否禁用全部checkbox （默認 false ）
	 * @property {String}			activeColor		選中狀態下的顏色，如設置此值，將會覆蓋parent的activeColor值 （默認 '#2979ff' ）
	 * @property {String}			inactiveColor	未選中的顏色 （默認 '#c8c9cc' ）
	 * @property {String | Number}	size			整個组件的尺寸 單位px （默認 18 ）
	 * @property {String}			placement		布局方式，row-横向，column-縱向 （默認 'row' ）
	 * @property {String | Number}	labelSize		label的字體大小，px單位  （默認 14 ）
	 * @property {String}			labelColor		label的字體顏色 （默認 '#303133' ）
	 * @property {Boolean}			labelDisabled	是否禁止點擊文本操作 (默認 false )
	 * @property {String}			iconColor		圖標顏色 （默認 '#ffffff' ）
	 * @property {String | Number}	iconSize		圖標的大小，單位px （默認 12 ）
	 * @property {String}			iconPlacement	勾選圖標的對齊方式，left-左邊，right-右邊  （默認 'left' ）
	 * @property {Boolean}			borderBottom	placement為row時，是否顯示下邊框 （默認 false ）
	 * @event {Function}	change	任一個checkbox狀態發生變化時触發，回調為一個對象
	 * @event {Function}	input	修改通過v-model绑定的值時触發，回調為一個對象
	 * @example <u-checkbox-group></u-checkbox-group>
	 */
	export default {
		name: 'u-checkbox-group',
		mixins: [mpMixin, mixin,props],
		computed: {
			// 這里computed的變量，都是子组件u-checkbox需要用到的，由于頭條小程序的兼容性差異，子组件無法實時監聽父组件参數的變化
			// 所以需要手動通知子组件，這里返回一個parentData變量，供watch監聽，在其中去通知每一個子组件重新從父组件(u-checkbox-group)
			// 拉取父组件新的變化后的参數
			parentData() {
			  return [
				// #ifdef VUE2
				this.value,
				// #endif
				// #ifdef VUE3
				this.modelValue,
				// #endif
				this.disabled,
				this.inactiveColor,
				this.activeColor,
				this.size,
				this.labelDisabled,
				this.shape,
				this.iconSize,
				this.borderBottom,
				this.placement,
			  ];
			},
			bemClass() {
				// this.bem為一個computed變量，在mixin中
				return this.bem('checkbox-group', ['placement'])
			},
		},
		watch: {
			// 當父组件需要子组件需要共享的参數發生了變化，手動通知子组件
			parentData: {
			  handler() {
				if (this.children.length) {
				  this.children.map((child) => {
					// 判斷子组件(u-checkbox)如果有init方法的話，就就執行(執行的结果是子组件重新從父组件拉取了最新的值)
					typeof child.init === "function" && child.init();
				  });
				}
			  },
			  deep: true,
			},
		},
		data() {
			return {

			}
		},
		created() {
			this.children = []
		},
		// #ifdef VUE3
		emits: ['update:modelValue', 'change'],
		// #endif
		methods: {
			// 將其他的checkbox設置為未選中的狀態
			unCheckedOther(childInstance) {
				const values = []
				this.children.map(child => {
					// 將被選中的checkbox，放到數组中返回
					if (child.isChecked) {
						values.push(child.name)
					}
				})
				// 發出事件
				this.$emit('change', values)
				// 修改通過v-model绑定的值
				// #ifdef VUE3
				this.$emit("update:modelValue", values);
				// #endif
				// #ifdef VUE2
				this.$emit("input", values);
				// #endif
			},
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-checkbox-group {

		&--row {
			/* #ifndef APP-NVUE */
			display: flex;
			/* #endif */
			flex-flow: row wrap;
		}

		&--column {
			@include flex(column);
		}
	}
</style>
