<template>
	<view class="u-collapse">
		<u-line v-if="border"></u-line>
		<slot />
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	/**
	 * collapse 折叠面板 
	 * @description 通過折叠面板收纳内容區域
	 * @tutorial https://ijry.github.io/uview-plus/components/collapse.html
	 * @property {String | Number | Array}	value		當前展開面板的name，非手风琴模式：[<string | number>]，手风琴模式：string | number
	 * @property {Boolean}					accordion	是否手风琴模式（ 默認 false ）
	 * @property {Boolean}					border		是否顯示外邊框 ( 默認 true ）
	 * @event {Function}	change 		當前激活面板展開時触發(如果是手风琴模式，参數activeNames類型為String，否则為Array)
	 * @example <u-collapse></u-collapse>
	 */
	export default {
		name: "u-collapse",
		mixins: [mpMixin, mixin,props],
		watch: {
			needInit() {
				this.init()
			},
			// 當父组件需要子组件需要共享的参數發生了變化，手動通知子组件
			parentData() {
				if (this.children.length) {
					this.children.map(child => {
						// 判斷子组件(u-checkbox)如果有updateParentData方法的話，就就執行(執行的结果是子组件重新從父组件拉取了最新的值)
						typeof(child.updateParentData) === 'function' && child.updateParentData()
					})
				}
			}
		},
		created() {
			this.children = []
		},
		computed: {
			needInit() {
				// 通過computed，同時監聽accordion和value值的變化
				// 再通過watch去執行init()方法，进行再一次的初始化
				return [this.accordion, this.value]
			}
		},
		emits: ["open", "close", "change"],
		methods: {
			// 重新初始化一次内部的所有子元素
			init() {
				this.children.map(child => {
					child.init()
				})
			},
			/**
			 * collapse-item被點擊時触發，由collapse统一處理各子组件的狀態
			 * @param {Object} target 被操作的面板的實例
			 */
			onChange(target) {
				let changeArr = []
				this.children.map((child, index) => {
					// 如果是手风琴模式，將其他的折叠面板收起来
					if (this.accordion) {
						child.expanded = child === target ? !target.expanded : false
						child.setContentAnimate()
					} else {
						if(child === target) {
							child.expanded = !child.expanded
							child.setContentAnimate()
						}
					}
					// 拼接change事件中，數组元素的狀態
					changeArr.push({
						// 如果没有定義name屬性，则默認返回组件的index索引
						name: child.name || index,
						status: child.expanded ? 'open' : 'close'
					})
				})

				this.$emit('change', changeArr)
				this.$emit(target.expanded ? 'open' : 'close', target.name)
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
</style>
