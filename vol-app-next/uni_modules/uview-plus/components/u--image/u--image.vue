<template>
	<uvImage 
		:src="src"
		:mode="mode"
		:width="width"
		:height="height"
		:shape="shape"
		:radius="radius"
		:lazyLoad="lazyLoad"
		:showMenuByLongpress="showMenuByLongpress"
		:loadingIcon="loadingIcon"
		:errorIcon="errorIcon"
		:showLoading="showLoading"
		:showError="showError"
		:fade="fade"
		:webp="webp"
		:duration="duration"
		:bgColor="bgColor"
		:customStyle="customStyle"
		@click="$emit('click')"
		@error="$emit('error')"
		@load="$emit('load')"
	>
		<template v-slot:loading>
			<slot name="loading"></slot>
		</template>
		<template v-slot:error>
			<slot name="error"></slot>
		</template>
	</uvImage>
</template>

<script>
	/**
	 * 此组件存在的理由是，在nvue下，u-image被uni-app官方占用了，u-image在nvue中相當于image组件
	 * 所以在nvue下，取名為u--image，内部其實還是u-iamge.vue，只不過做一層中轉
	 */
	import uvImage from '../u-image/u-image.vue';
	import { props } from '../u-image/props.js';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	export default {
		name: 'u--image',
		mixins: [mpMixin, props, mixin],
		components: {
			uvImage
		},
		emits: ['click', 'error', 'load']
	}
</script>