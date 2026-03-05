<template>
	<view
	    class="u-steps"
	    :class="[`u-steps--${direction}`]"
	>
		<slot></slot>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import test from '../../libs/function/test';
	/**
	 * Steps 步骤條
	 * @description 該组件一般用于完成一個任务要分几個步骤，標识目前處于第几步的场景。
	 * @tutorial https://uview-plus.jiangruyi.com/components/steps.html
	 * @property {String}			direction		row-横向，column-竖向 (默認 'row' )
	 * @property {String | Number}	current			設置當前處于第几步 (默認 0 )
	 * @property {String}			activeColor		激活狀態顏色 (默認 '#3c9cff' )
	 * @property {String}			inactiveColor	未激活狀態顏色 (默認 '#969799' )
	 * @property {String}			activeIcon		激活狀態的圖標
	 * @property {String}			inactiveIcon	未激活狀態圖標 
	 * @property {Boolean}			dot				是否顯示點類型 (默認 false )
	 * @example <u-steps current="0"><u-steps-item title="已出庫" desc="10:35" ></u-steps-item></u-steps>
	 */
	export default {
		name: 'u-steps',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
			}
		},
		watch: {
			children() {
				this.updateChildData()
			},
			parentData() {
				this.updateChildData()
			}
		},
		computed: {
			// 監聽参數的變化，通過watch中，手動去更新子组件的數據，否则子组件不會自動變化
			parentData() {
				return [this.current, this.direction, this.activeColor, this.inactiveColor, this.activeIcon, this.inactiveIcon, this.dot]
			}
		},
		methods: {
			// 更新子组件的數據
			updateChildData() {
				this.children.map(child => {
					// 先判斷子组件是否存在對應的方法
					test.func((child || {}).updateFromParent()) && child.updateFromParent()
				})
			},
			// 接受子组件的通知，去修改其他子组件的數據
			updateFromChild() {
				this.updateChildData()
			}
		},
		created() {
			this.children = []
		},
		options: {
			virtualHost: false
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-steps {
		@include flex;

		&--column {
			flex-direction: column
		}

		&--row {
			flex-direction: row;
			flex: 1;
			/* #ifdef MP */
			display: grid;
			grid-template-columns: repeat(auto-fit, minmax(100px, 1fr));
			/* #endif */
		}
	}
</style>
