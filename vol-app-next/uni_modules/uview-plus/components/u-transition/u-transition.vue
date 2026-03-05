<template>
	<view
		v-if="inited"
		class="u-transition"
		ref="u-transition"
		@tap="clickHandler"
		:class="classes"
		:style="[mergeStyle]"
		@touchmove="noop"
	>
		<slot />
	</view>
</template>

<script>
import { props } from './props';
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { addStyle } from '../../libs/function/index';
// 组件的methods方法，由于内容较長，写在外部文件中通過mixin引入
import transitionMixin from "./transitionMixin.js";
/**
 * transition  動画组件
 * @description
 * @tutorial
 * @property {String}			show			是否展示组件 （默認 false ）
 * @property {String}			mode			使用的動画模式 （默認 'fade' ）
 * @property {String | Number}	duration		動画的執行時間，單位ms （默認 '300' ）
 * @property {String}			timingFunction	使用的動画過渡函數 （默認 'ease-out' ）
 * @property {Object}			customStyle		自定義樣式
 * @event {Function} before-enter	进入前触發
 * @event {Function} enter			进入中触發
 * @event {Function} after-enter	进入后触發
 * @event {Function} before-leave	离開前触發
 * @event {Function} leave			离開中触發
 * @event {Function} after-leave	离開后触發
 * @example
 */
export default {
	name: 'u-transition',
	data() {
		return {
			inited: false, // 是否顯示/隐藏组件
			viewStyle: {}, // 组件内部的樣式
			status: '', // 记錄组件動画的狀態
			transitionEnded: false, // 组件是否结束的標记
			display: false, // 组件是否展示
			classes: '', // 應用的類名
		}
	},
	emits: ['click', 'beforeEnter', 'enter', 'afterEnter', 'beforeLeave', 'leave', 'afterLeave'],
	computed: {
	    mergeStyle() {
	        const { viewStyle, customStyle } = this
	        return {
	            // #ifndef APP-NVUE
	            transitionDuration: `${this.duration}ms`,
	            // display: `${this.display ? '' : 'none'}`,
				transitionTimingFunction: this.timingFunction,
	            // #endif
				// 避免自定義樣式影响到動画屬性，所以写在viewStyle前面
	            ...addStyle(customStyle),
	            ...viewStyle
	        }
	    }
	},
	// 將mixin挂在到组件中，實际上為一個vue格式對象。
	mixins: [mpMixin, mixin, transitionMixin, props],
	watch: {
		show: {
			handler(newVal) {
				// vue和nvue分别執行不同的方法
				// #ifdef APP-NVUE
				newVal ? this.nvueEnter() : this.nvueLeave()
				// #endif
				// #ifndef APP-NVUE
				newVal ? this.vueEnter() : this.vueLeave()
				// #endif
			},
			// 表示同時監聽初始化時的props的show的意思
			immediate: true
		}
	}
}
</script>

<style lang="scss" scoped>
@import '../../libs/css/components.scss';

/* #ifndef APP-NVUE */
// vue版本動画相關的樣式抽离在外部文件
@import './vue.ani-style.scss';
/* #endif */

.u-transition {}
</style>
