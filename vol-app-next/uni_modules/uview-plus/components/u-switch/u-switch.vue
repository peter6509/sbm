<template>
	<view
	    class="u-switch cursor-pointer"
	    :class="[disabled && 'u-switch--disabled']"
	    :style="[switchStyle, addStyle(customStyle)]"
	    @tap="clickHandler"
	>
		<view
		    class="u-switch__bg"
		    :style="[bgStyle]"
		>
		</view>
		<view
		    class="u-switch__node"
		    <!-- #ifdef VUE3 -->
			:class="[modelValue && 'u-switch__node--on']"
			<!-- #endif -->
			<!-- #ifdef VUE2 -->
			:class="[value && 'u-switch__node--on']"
			<!-- #endif -->
		    :style="[nodeStyle]"
		    ref="u-switch__node"
		>
			<u-loading-icon
			    :show="loading"
			    mode="circle"
			    timingFunction='linear'
			    <!-- #ifdef VUE3 -->
				:color="modelValue ? activeColor : '#AAABAD'"
				<!-- #endif -->
				<!-- #ifdef VUE2 -->
				:color="value ? activeColor : '#AAABAD'"
				<!-- #endif -->
			    :size="size * 0.6"
			/>
		</view>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addStyle, addUnit, error } from '../../libs/function/index';
	/**
	 * switch 開關選擇器
	 * @description 選擇開關一般用于只有两個選擇，且只能選其一的场景。
	 * @tutorial https://ijry.github.io/uview-plus/components/switch.html
	 * @property {Boolean}						loading			是否處于加載中（默認 false ）
	 * @property {Boolean}						disabled		是否禁用（默認 false ）
	 * @property {String | Number}				size			開關尺寸，單位px （默認 25 ）
	 * @property {String}						activeColor		打開時的背景色 （默認 '#2979ff' ）
	 * @property {String} 						inactiveColor	關闭時的背景色 （默認 '#ffffff' ）
	 * @property {Boolean | String | Number}	value			通過v-model双向绑定的值 （默認 false ）
	 * @property {Boolean | String | Number}	activeValue		打開選擇器時通過change事件發出的值 （默認 true ）
	 * @property {Boolean | String | Number}	inactiveValue	關闭選擇器時通過change事件發出的值 （默認 false ）
	 * @property {Boolean}						asyncChange		是否開啟異步變更，開啟后需要手動控制輸入值 （默認 false ）
	 * @property {String | Number}				space			圆點與外邊框的距离 （默認 0 ）
	 * @property {Object}						customStyle		定義需要用到的外部樣式
	 *
	 * @event {Function} change 在switch打開或關闭時触發
	 * @example <u-switch v-model="checked" active-color="red" inactive-color="#eee"></u-switch>
	 */
	export default {
		name: "u-switch",
		mixins: [mpMixin, mixin,props],
		watch: {
			// #ifdef VUE3
			modelValue: {
				immediate: true,
				handler(n) {
					if(n !== this.inactiveValue && n !== this.activeValue) {
						error('v-model绑定的值必須為inactiveValue、activeValue二者之一')
					}
				}
			},
			// #endif
        	// #ifdef VUE2
			value: {
				immediate: true,
				handler(n) {
					if(n !== this.inactiveValue && n !== this.activeValue) {
						error('v-model绑定的值必須為inactiveValue、activeValue二者之一')
					}
				}
			}
			// #endif
		},
		data() {
			return {
				bgColor: '#ffffff'
			}
		},
		computed: {
			isActive(){
				// #ifdef VUE3
				return this.modelValue === this.activeValue;
				// #endif
        		// #ifdef VUE2
				return this.value === this.activeValue;
				// #endif
			},
			switchStyle() {
				let style = {}
				// 這里需要加2，是為了腾出邊框的距离，否则圆點node會和外邊框紧貼在一起
				style.width = addUnit(this.size * 2 + 2)
				style.height = addUnit(Number(this.size) + 2)
				// style.borderColor = this.value ? 'rgba(0, 0, 0, 0)' : 'rgba(0, 0, 0, 0.12)'
				// 如果自定義了“非激活”演示，name邊框顏色設置為透明(跟非激活顏色一致)
				// 這里不能簡單的設置為非激活的顏色，否则打開狀態時，會有邊框，所以需要透明
				if(this.customInactiveColor) {
					style.borderColor = 'rgba(0, 0, 0, 0)'
				}
				style.backgroundColor = this.isActive ? this.activeColor : this.inactiveColor
				return style;
			},
			nodeStyle() {
				let style = {}
				// 如果自定義非激活顏色，將node圆點的尺寸减少两個像素，让其與外邊框距离更大一點
				style.width = addUnit(this.size - this.space)
				style.height = addUnit(this.size - this.space)
				const translateX = this.isActive ? addUnit(this.space) : addUnit(this.size);
				style.transform = `translateX(-${translateX})`
				return style
			},
			bgStyle() {
				let style = {}
				// 這里配置一個多餘的元素在HTML中，是為了让switch切換時，有更良好的背景色扩充體验(見實际效果)
				style.width = addUnit(Number(this.size) * 2 - this.size / 2)
				style.height = addUnit(this.size)
				style.backgroundColor = this.inactiveColor
				// 打開時，让此元素收缩，否则反之
				style.transform = `scale(${this.isActive ? 0 : 1})`
				return style
			},
			customInactiveColor() {
				// 之所以需要判斷是否自定義了“非激活”顏色，是為了让node圆點离外邊框更寬一點的距离
				return this.inactiveColor !== '#fff' && this.inactiveColor !== '#ffffff'
			}
		},
		// #ifdef VUE3
		emits: ['update:modelValue', 'change'],
    	// #endif
		methods: {
			addStyle,
			clickHandler() {
				if (!this.disabled && !this.loading) {
					const oldValue = this.isActive ? this.inactiveValue : this.activeValue
					if(!this.asyncChange) {
						// #ifdef VUE3
						this.$emit("update:modelValue", oldValue);
						// #endif
						// #ifdef VUE2
						this.$emit("input", oldValue);
						// #endif
					}
					// 放到下一個生命周期，因為双向绑定的value修改父组件狀態需要時間，且是異步的
					this.$nextTick(() => {
						this.$emit('change', oldValue)
					})
				}
			}
		}
	};
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-switch {
		@include flex(row);
		/* #ifndef APP-NVUE */
		box-sizing: border-box;
		/* #endif */
		position: relative;
		background-color: #fff;
		border-width: 1px;
		border-radius: 100px;
		transition: background-color 0.4s;
		border-color: rgba(0, 0, 0, 0.12);
		border-style: solid;
		justify-content: flex-end;
		align-items: center;
		// 由于weex為阿里逗着玩的KPI項目，导致bug奇多，這必須要写這一行，
		// 否则在iOS上，點擊頁面任意地方，都會触發switch的點擊事件
		overflow: hidden;

		&__node {
			@include flex(row);
			align-items: center;
			justify-content: center;
			border-radius: 100px;
			background-color: #fff;
			border-radius: 100px;
			box-shadow: 1px 1px 1px 0 rgba(0, 0, 0, 0.25);
			transition-property: transform;
			transition-duration: 0.4s;
			transition-timing-function: cubic-bezier(0.3, 1.05, 0.4, 1.05);
		}

		&__bg {
			position: absolute;
			border-radius: 100px;
			background-color: #FFFFFF;
			transition-property: transform;
			transition-duration: 0.4s;
			border-top-left-radius: 0;
			border-bottom-left-radius: 0;
			transition-timing-function: ease;
		}

		&--disabled {
			opacity: 0.6;
		}
	}
</style>
