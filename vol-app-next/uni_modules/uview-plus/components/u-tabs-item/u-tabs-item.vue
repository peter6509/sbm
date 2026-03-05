<template>
	<swiper-item>
		<slot />
	</swiper-item>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	/**
	 * TabsItem  tabs標簽组件的自组件
	 * @description tabs標簽组件，在標簽多的時候，可以配置為左右滑動，標簽少的時候，可以禁止滑動。 該组件的一個特點是配置為滾動模式時，激活的tab會自動移動到组件的中間位置。
	 * @tutorial https://ijry.github.io/uview-plus/components/tabs.html
	 * @property {type}	prop_name
	 * @event {Function()} 
	 * @example 
	 */
	export default {
		name: 'u-tabs-item',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				
			}
		}
	}
</script>

<style>
</style>
