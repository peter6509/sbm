<template>
	<view class="u-swipe-action">
		<slot></slot>
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	/**
	 * SwipeAction 滑動單元格 
	 * @description 該组件一般用于左滑唤出操作菜單的场景，用的最多的是左滑删除操作
	 * @tutorial https://ijry.github.io/uview-plus/components/swipeAction.html
	 * @property {Boolean}	autoClose	是否自動關闭其他swipe按鈕组
	 * @event {Function(index)}	click	點擊组件時触發
	 * @example	<u-swipe-action><u-swipe-action-item :rightOptions="options1" ></u-swipe-action-item></u-swipe-action>
	 */
	export default {
		name: 'u-swipe-action',
		mixins: [mpMixin, mixin, props],
		data() {
			return {}
		},
		provide() {
			return {
				swipeAction: this
			}
		},
		computed: {
			// 這里computed的變量，都是子组件u-swipe-action-item需要用到的，由于頭條小程序的兼容性差異，子组件無法實時監聽父组件参數的變化
			// 所以需要手動通知子组件，這里返回一個parentData變量，供watch監聽，在其中去通知每一個子组件重新從父组件(u-swipe-action-item)
			// 拉取父组件新的變化后的参數
			parentData() {
				return [this.autoClose]
			}
		},
		emits: ['opendItem:update'],
		watch: {
			// 當父组件需要子组件需要共享的参數發生了變化，手動通知子组件
			parentData() {
				if (this.children.length) {
					this.children.map(child => {
						// 判斷子组件(u-swipe-action-item)如果有updateParentData方法的話，就就執行(執行的结果是子组件重新從父组件拉取了最新的值)
						typeof(child.updateParentData) === 'function' && child.updateParentData()
					})
				}
			},
			opendItem(val) {
				if (val == false) {
					this.closeAll()
				}
			}
		},
		created() {
			this.children = []
		},
		methods: {
			closeOther(child) {
				if (this.autoClose) {
					// 历遍所有的單元格，找出非當前操作中的單元格，进行關闭
					this.children.map((item, index) => {
						if (child !== item) {
							item.closeHandler()
						}
					})
				}
			},
			closeAll() {
				// 關闭所有單元格
				this.children.map((item, index) => {
					item.closeHandler()
				})
			},
			setOpendItem(ins) {
				this.$emit('opendItem:update', true)
			}
		}
	}
</script>

<style lang="scss" scoped>

</style>
