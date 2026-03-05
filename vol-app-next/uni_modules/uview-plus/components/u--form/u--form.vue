<template>
	<uvForm
		ref="uForm"
		:model="model"
		:rules="rules"
		:errorType="errorType"
		:borderBottom="borderBottom"
		:labelPosition="labelPosition"
		:labelWidth="labelWidth"
		:labelAlign="labelAlign"
		:labelStyle="labelStyle"
		:customStyle="customStyle"
	>
		<slot />
	</uvForm>
</template>

<script>
	/**
	 * 此组件存在的理由是，在nvue下，u-form被uni-app官方占用了，u-form在nvue中相當于form组件
	 * 所以在nvue下，取名為u--form，内部其實還是u-form.vue，只不過做一層中轉
	 */
	import uvForm from '../u-form/u-form.vue';
	import { props } from '../u-form/props.js';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	export default {
		// #ifdef MP-WEIXIN
		name: 'u-form',
		// #endif
		// #ifndef MP-WEIXIN
		name: 'u--form',
		// #endif
		mixins: [mpMixin, props, mixin],
		components: {
			uvForm
		},
		created() {
			this.children = []
		},
		methods: {
			// 手動設置校验的规则，如果规则中有函數的話，微信小程序中會過濾掉，所以只能手動調用設置规则
			setRules(rules) {
				this.$refs.uForm.setRules(rules)
			},
			validate() {
				/**
				 * 在微信小程序中，通過this.$parent拿到的父组件是u--form，而不是其内嵌的u-form
				 * 导致在u-form组件中，拿不到對應的children數组，從而校验無效，所以這里每次調用u-form组件中的
				 * 對應方法的時候，在小程序中都先將u--form的children赋值给u-form中的children
				 */
				// #ifdef MP-WEIXIN
				this.setMpData()
				// #endif
				return this.$refs.uForm.validate()
			},
			validateField(value, callback) {
				// #ifdef MP-WEIXIN
				this.setMpData()
				// #endif
				return this.$refs.uForm.validateField(value, callback)
			},
			resetFields() {
				// #ifdef MP-WEIXIN
				this.setMpData()
				// #endif
				return this.$refs.uForm.resetFields()
			},
			clearValidate(props) {
				// #ifdef MP-WEIXIN
				this.setMpData()
				// #endif
				return this.$refs.uForm.clearValidate(props)
			},
			setMpData() {
				this.$refs.uForm.children = this.children
			}
		},
	}
</script>
