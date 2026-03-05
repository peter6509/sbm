<template>
	<!-- #ifdef APP-NVUE -->
	<cell ref="u-index-item">
		<!-- #endif -->
		<view
			class="u-index-item"
			:id="`u-index-item-${id}`"
			:class="[`u-index-item-${id}`]"
		>
			<slot />
		</view>
		<!-- #ifdef APP-NVUE -->
	</cell>
	<!-- #endif -->
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { sleep, error } from '../../libs/function/index';
	// #ifdef APP-NVUE
	// 由于weex為阿里的KPI业绩考核的產物，所以不支持百分比單位，這里需要通過dom查詢组件的寬度
	const dom = uni.requireNativePlugin('dom')
	// #endif
	/**
	 * IndexItem 
	 * @description 
	 * @tutorial https://uview-plus.jiangruyi.com/components/indexList.html
	 * @property {String}
	 * @event {Function}
	 * @example
	 */
	export default {
		name: 'u-index-item',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				// 本组件到滾動條顶部的距离
				top: 0,
				height: 0,
				id: ''
			}
		},
		created() {
			// 子组件u-index-anchor的實例
			this.anchor = {}
		},
		mounted() {
			this.init()
		},
		methods: {
			init() {
				// 此處會活動父组件實例，並赋值给實例的parent屬性
				this.getParentData('u-index-list')
				if (!this.parent) {
					return error('u-index-item必須要搭配u-index-list组件使用')
				}
				sleep().then(() =>{
					this.getIndexItemRect().then(size => {
						// 由于對象的引用特性，此處會同時生效到父组件的children數组的本實例的top屬性中，供父组件判斷讀取
						this.top = Math.ceil(size.top)
						this.height = Math.ceil(size.height)
					})
				})
			},
			getIndexItemRect() {
				return new Promise(resolve => {
					// #ifndef APP-NVUE
					this.$uGetRect('.u-index-item').then(size => {
						resolve(size)
					})
					// #endif

					// #ifdef APP-NVUE
					const ref = this.$refs['u-index-item']
					dom.getComponentRect(ref, res => {
						resolve(res.size)
					})
					// #endif
				}) 
			}
		},
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";
	
</style>
