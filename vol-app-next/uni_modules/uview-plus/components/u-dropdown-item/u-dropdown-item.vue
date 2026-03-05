<template>
	<view class="u-dropdown-item" v-if="active" @touchmove.stop.prevent="() => {}" @tap.stop.prevent="() => {}">
		<block v-if="!$slots.default && !$slots.$default">
			<scroll-view class="u-dropdown-item__scroll" scroll-y="true" :style="{
				height: addUnit(height)
			}">
				<view class="u-dropdown-item__options">
					<up-cell-group>
						<up-cell @click="cellClick(item.value)" :arrow="false" :title="item.label" v-for="(item, index) in options"
						 :key="index" :title-style="{
							color: modelValue == item.value ? activeColor : inactiveColor
						}">
							<up-icon v-if="modelValue == item.value" name="checkbox-mark" :color="activeColor" size="32"></up-icon>
						</up-cell>
					</up-cell-group>
				</view>
			</scroll-view>
		</block>
		<slot v-else />
	</view>
</template>

<script>
    import { props } from './props';
    import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, $parent } from '../../libs/function/index';
	/**
	 * dropdown-item 下拉菜單
	 * @description 該组件一般用于向下展開菜單，同時可切換多個選項卡的场景
	 * @tutorial https://ijry.github.io/uview-plus/components/dropdown.html
	 * @property {String | Number} v-model 双向绑定選項卡選擇值
	 * @property {String} title 菜單項標題
	 * @property {Array[Object]} options 選項數據，如果傳入了默認slot，此参數無效
	 * @property {Boolean} disabled 是否禁用此選項卡（默認false）
	 * @property {String | Number} duration 選項卡展開和收起的過渡時間，單位ms（默認300）
	 * @property {String | Number} height 彈窗下拉内容的高度(内容超出將會滾動)（默認auto）
	 * @example <u-dropdown-item title="標題"></u-dropdown-item>
	 */
	export default {
		name: 'u-dropdown-item',
		mixins: [mpMixin, mixin, props],
        options: {
            styleIsolation: 'shared',
        },
		data() {
			return {
				active: false, // 當前項是否處于展開狀態
				activeColor: '#2979ff', // 激活時左邊文字和右邊對勾圖標的顏色
				inactiveColor: '#606266', // 未激活時左邊文字和右邊對勾圖標的顏色
			}
		},
		computed: {
			// 監聽props是否發生了變化，有些值需要傳递给父组件u-dropdown，無法双向绑定
			propsChange() {
				return `${this.title}-${this.disabled}`;
			}
		},
		watch: {
			propsChange(n) {
				// 當值變化時，通知父组件重新初始化，让父组件執行每個子组件的init()方法
				// 將所有子组件數據重新整理一遍
				if (this.parent) this.parent.init();
			}
		},
		created() {
			// 父组件的實例
			this.parent = false;
		},
        emits: ['update:modelValue', 'change'],
		methods: {
			addUnit,
			init() {
				// 獲取父组件u-dropdown
				let parent = $parent.call(this, 'u-dropdown');
				if (parent) {
					this.parent = parent;
					// 將子组件的激活顏色配置為父组件設置的激活和未激活時的顏色
					this.activeColor = parent.activeColor;
					this.inactiveColor = parent.inactiveColor;
					// 將本组件的this，放入到父组件的children數组中，让父组件可以操作本(子)组件的方法和屬性
					// push进去前，顯判斷是否已經存在了本實例，因為在子组件内部數據變化時，會通過父组件重新初始化子组件
					let exist = parent.children.find(val => {
						return this === val;
					})
					if (!exist) parent.children.push(this);
					if (parent.children.length == 1) this.active = true;
					// 父组件無法監聽children的變化，故將子组件的title，傳入父组件的menuList數组中
					parent.menuList.push({
						title: this.title,
						disabled: this.disabled
					});
				}
			},
			// cell被點擊
			cellClick(value) {
				// 修改通過v-model绑定的值
                // #ifdef VUE2
				this.$emit('input', value);
                // #endif
		        // #ifdef VUE3
                this.$emit('update:modelValue', value);
                // #endif
				// 通知父组件(u-dropdown)收起菜單
				this.parent.close();
				// 發出事件，抛出當前勾選項的value
				this.$emit('change', value);
			}
		},
		mounted() {
			this.init();
		}
	}
</script>

<style scoped lang="scss">
	@import "../../libs/css/components.scss";
    .u-dropdown-item__scroll {
        background: #ffffff;
    }
</style>
