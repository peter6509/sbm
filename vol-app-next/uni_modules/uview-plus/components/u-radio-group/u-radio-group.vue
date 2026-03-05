<template>
	<view
	    class="u-radio-group"
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
	 * radioRroup 單選框父组件
	 * @description 單選框用于有一個選擇，用户只能選擇其中一個的场景。搭配u-radio使用
	 * @tutorial https://ijry.github.io/uview-plus/components/radio.html
	 * @property {String | Number | Boolean}	value 			绑定的值
	 * @property {Boolean}						disabled		是否禁用所有radio（默認 false ）
	 * @property {String}						shape			外觀形狀，shape-方形，circle-圆形(默認 circle )
	 * @property {String}						activeColor		選中時的顏色，應用到所有子Radio组件（默認 '#2979ff' ）
	 * @property {String}						inactiveColor	未選中的顏色 (默認 '#c8c9cc' )
	 * @property {String}						name			標识符
	 * @property {String | Number}				size			组件整體的大小，單位px（默認 18 ）
	 * @property {String}						placement		布局方式，row-横向，column-縱向 （默認 'row' ）
	 * @property {String}						label			文本
	 * @property {String}						labelColor		label的顏色 （默認 '#303133' ）
	 * @property {String | Number}				labelSize		label的字體大小，px單位 （默認 14 ）
	 * @property {Boolean}						labelDisabled	是否禁止點擊文本操作checkbox(默認 false )
	 * @property {String}						iconColor		圖標顏色 （默認 '#ffffff' ）
	 * @property {String | Number}				iconSize		圖標的大小，單位px （默認 12 ）
	 * @property {Boolean}						borderBottom	placement為row時，是否顯示下邊框 （默認 false ）
	 * @property {String}						iconPlacement	圖標與文字的對齊方式 （默認 'left' ）
     * @property {Object}						customStyle		组件的樣式，對象形式
	 * @event {Function} change 任一個radio狀態發生變化時触發
	 * @example <u-radio-group v-model="value"></u-radio-group>
	 */
	export default {
		name: 'u-radio-group',
		mixins: [mpMixin, mixin, props],
		computed: {
			// 這里computed的變量，都是子组件u-radio需要用到的，由于頭條小程序的兼容性差異，子组件無法實時監聽父组件参數的變化
			// 所以需要手動通知子组件，這里返回一個parentData變量，供watch監聽，在其中去通知每一個子组件重新從父组件(u-radio-group)
			// 拉取父组件新的變化后的参數
			parentData() {
				// #ifdef VUE3
                return [this.modelValue, this.disabled, this.inactiveColor, this.activeColor, this.size, this.labelDisabled, this.shape,
					this.iconSize, this.borderBottom, this.placement
				]
                // #endif
                // #ifdef VUE2
                return [this.value, this.disabled, this.inactiveColor, this.activeColor, this.size, this.labelDisabled, this.shape,
					this.iconSize, this.borderBottom, this.placement
				]
				// #endif
				
			},
			bemClass() {
				// this.bem為一個computed變量，在mixin中
				return this.bem('radio-group', ['placement'])
			},
		},
		watch: {
			// 當父组件需要子组件需要共享的参數發生了變化，手動通知子组件
			parentData() {
				if (this.children.length) {
					this.children.map(child => {
						// 判斷子组件(u-radio)如果有init方法的話，就就執行(執行的结果是子组件重新從父组件拉取了最新的值)
						typeof(child.init) === 'function' && child.init()
					})
				}
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
			// 將其他的radio設置為未選中的狀態
			unCheckedOther(childInstance) {
				this.children.map(child => {
					// 所有子radio中，被操作组件實例的checked的值無需修改
					if (childInstance !== child) {
						child.checked = false
					}
				})
				const {
					name
				} = childInstance
				// 通過emit事件，設置父组件通過v-model双向绑定的值
				// #ifdef VUE3
                this.$emit("update:modelValue", name);
                // #endif
                // #ifdef VUE2
                this.$emit("input", name);
				// #endif
				// 發出事件
				this.$emit('change', name)
			},
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-radio-group {
		flex: 1;

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
