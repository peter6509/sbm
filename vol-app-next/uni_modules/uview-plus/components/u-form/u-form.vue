<template>
	<view class="u-form">
		<slot />
	</view>
</template>

<script>
	import { props } from "./props.js";
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import Schema from "../../libs/util/async-validator";
	import { toast, getProperty, setProperty, deepClone, error } from '../../libs/function/index';
	import test from '../../libs/function/test';
	// 去除警告信息
	Schema.warning = function() {};
	/**
	 * Form 表單
	 * @description 此组件一般用于表單场景，可以配置Input輸入框，Select彈出框，进行表單验证等。
	 * @tutorial https://ijry.github.io/uview-plus/components/form.html
	 * @property {Object}						model			當前form的需要验证字段的集合
	 * @property {Object | Function | Array}	rules			验证规则
	 * @property {String}						errorType		错误的提示方式，見上方說明 ( 默認 message )
	 * @property {Boolean}						borderBottom	是否顯示表單域的下划线邊框   ( 默認 true ）
	 * @property {String}						labelPosition	表單域提示文字的位置，left-左侧，top-上方 ( 默認 'left' ）
	 * @property {String | Number}				labelWidth		提示文字的寬度，單位px  ( 默認 45 ）
	 * @property {String}						labelAlign		lable字體的對齊方式   ( 默認 ‘left' ）
	 * @property {Object}						labelStyle		lable的樣式，對象形式
	 * @example <up-formlabelPosition="left" :model="model1" :rules="rules" ref="form1"></up-form>
	 */
	export default {
		name: "u-form",
		mixins: [mpMixin, mixin, props],
		provide() {
			return {
				uForm: this,
			};
		},
		data() {
			return {
				formRules: {},
				// 规则校验器
				validator: {},
				// 原始的model快照，用于resetFields方法重置表單時使用
				originalModel: null,
			};
		},
		watch: {
			// 監聽规则的變化
			rules: {
				immediate: true,
				handler(n) {
					this.setRules(n);
				},
			},
			// 監聽屬性的變化，通知子组件u-form-item重新獲取信息
			propsChange(n) {
				if (this.children?.length) {
					this.children.map((child) => {
						// 判斷子组件(u-form-item)如果有updateParentData方法的話，就就執行(執行的结果是子组件重新從父组件拉取了最新的值)
						typeof child.updateParentData == "function" &&
							child.updateParentData();
					});
				}
			},
			// 監聽model的初始值作為重置表單的快照
			model: {
				immediate: true,
				handler(n) {
					if (!this.originalModel) {
						this.originalModel = deepClone(n);
					}
				},
			},
		},
		computed: {
			propsChange() {
				return [
					this.errorType,
					this.borderBottom,
					this.labelPosition,
					this.labelWidth,
					this.labelAlign,
					this.labelStyle,
				];
			},
		},
		created() {
			// 存儲當前form下的所有u-form-item的實例
			// 不能定義在data中，否则微信小程序會造成循环引用而報错
			this.children = [];
		},
		methods: {
			// 手動設置校验的规则，如果规则中有函數的話，微信小程序中會過濾掉，所以只能手動調用設置规则
			setRules(rules) {
				// 判斷是否有规则
				if (Object.keys(rules).length === 0) return;
				if (process.env.NODE_ENV === 'development' && Object.keys(this.model).length === 0) {
					error('設置rules，model必須設置！如果已經設置，請刷新頁面。');
					return;
				};
				this.formRules = rules;
				// 重新將规则赋予Validator
				this.validator = new Schema(rules);
			},
			// 清空所有u-form-item组件的内容，本质上是調用了u-form-item组件中的resetField()方法
			resetFields() {
				this.resetModel();
			},
			// 重置model為初始值的快照
			resetModel(obj) {
				// 历遍所有u-form-item，根據其prop屬性，還原model的原始快照
				this.children.map((child) => {
					const prop = child?.prop;
					const value = getProperty(this.originalModel, prop);
					setProperty(this.model, prop, value);
				});
			},
			// 清空校验结果
			clearValidate(props) {
				props = [].concat(props);
				this.children.map((child) => {
					// 如果u-form-item的prop在props數组中，则清除對應的校验结果信息
					if (props[0] === undefined || props.includes(child.prop)) {
						child.message = null;
					}
				});
			},
			// 對部分表單字段进行校验
			async validateField(value, callback, event = null) {
				// $nextTick是必須的，否则model的變更，可能會延后于此方法的執行
				this.$nextTick(() => {
					// 校验错误信息，返回给回調方法，用于存放所有form-item的错误信息
					const errorsRes = [];
					// 如果為字符串，轉為數组
					value = [].concat(value);
					// 历遍children所有子form-item
					let promises = this.children.map(child => {
						return new Promise((resolve, reject) => {
							// 用于存放form-item的错误信息
							const childErrors = [];
							if (value.includes(child.prop)) {
								// 獲取對應的屬性，通過類似'a.b.c'的形式
								const propertyVal = getProperty(
									this.model,
									child.prop
								);
								// 屬性链數组
								const propertyChain = child.prop.split(".");
								const propertyName =
									propertyChain[propertyChain.length - 1];

								let rule = []
								if (child.itemRules && child.itemRules.length > 0) {
									rule = child.itemRules
								} else {
									rule = this.formRules[child.prop];
								}
								// 如果不存在對應的规则，直接返回，否则校验器會報错
								if (!rule) {
									resolve()
									return;
								}
								// rule规则可為數组形式，也可為對象形式，此處拼接成為數组
								const rules = [].concat(rule);

								// 對rules數组进行校验
								if (!rules.length) {
									resolve()
								}
								for (let i = 0; i < rules.length; i++) {
									const ruleItem = rules[i];
									// 將u-form-item的触發器轉為數组形式
									const trigger = [].concat(ruleItem?.trigger);
									// 如果是有傳入触發事件，但是此form-item却没有配置此触發器的話，不執行校验操作
									if (event && !trigger.includes(event)) {
										resolve()
										continue;
									}
									// 實例化校验對象，傳入构造规则
									const validator = new Schema({
										[propertyName]: ruleItem,
									});
									validator.validate({
										[propertyName]: propertyVal,
									},
										(errors, fields) => {
											if (test.array(errors)) {
												errors.forEach(element => {
													element.prop = child.prop;
												});
												errorsRes.push(...errors);
												childErrors.push(...errors);
											}
											child.message =
												childErrors[0]?.message ? childErrors[0].message : null;

											if (i == (rules.length - 1)) {
												resolve(errorsRes)
											}
										}
									)
								}
							} else {
								resolve({})
							}
						});
					});

					// 使用Promise.all来等待所有Promise完成  
					Promise.all(promises)
						.then(results => {
							// 執行回調函數
							typeof callback === "function" && callback(errorsRes);
						})
						.catch(error => {
							console.error('An error occurred:', error);
						});
				});
			},
			// 校验全部數據
			validate(callback) {
				// 開發环境才提示，生產环境不會提示
				if (process.env.NODE_ENV === 'development' && Object.keys(this.formRules).length === 0) {
					error('未設置rules，請看文檔說明！如果已經設置，請刷新頁面。');
					return;
				}
				return new Promise((resolve, reject) => {
					// $nextTick是必須的，否则model的變更，可能會延后于validate方法
					this.$nextTick(() => {
						// 獲取所有form-item的prop，交给validateField方法进行校验
						const formItemProps = this.children.map(
							(item) => item.prop
						);
						// console.log(formItemProps)
						this.validateField(formItemProps, (errors) => {
							if(errors.length) {
								// 如果错误提示方式為toast，则进行提示
								this.errorType === 'toast' && toast(errors[0].message)
								reject(errors)
							} else {
								resolve(true)
							}
						});
					});
				});
			},
		},
	};
</script>

<style lang="scss" scoped>
</style>
