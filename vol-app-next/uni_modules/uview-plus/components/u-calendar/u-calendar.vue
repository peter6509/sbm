<template>
	<u-popup
		:show="show"
		mode="bottom"
		closeable
		@close="close"
		:round="round"
		:closeOnClickOverlay="closeOnClickOverlay"
	>
		<view class="u-calendar">
			<uHeader
				:title="title"
				:subtitle="subtitle"
				:showSubtitle="showSubtitle"
				:showTitle="showTitle"
			></uHeader>
			<scroll-view
				:style="{
                    height: addUnit(listHeight)
                }"
				scroll-y
				@scroll="onScroll"
				:scroll-top="scrollTop"
				:scrollIntoView="scrollIntoView"
			>
				<uMonth
					:color="color"
					:rowHeight="rowHeight"
					:showMark="showMark"
					:months="months"
					:mode="mode"
					:maxCount="maxCount"
					:startText="startText"
					:endText="endText"
					:defaultDate="defaultDate"
					:minDate="innerMinDate"
					:maxDate="innerMaxDate"
					:maxMonth="monthNum"
					:readonly="readonly"
					:maxRange="maxRange"
					:rangePrompt="rangePrompt"
					:showRangePrompt="showRangePrompt"
					:allowSameDay="allowSameDay"
					ref="month"
					@monthSelected="monthSelected"
					@updateMonthTop="updateMonthTop"
				></uMonth>
			</scroll-view>
			<slot name="footer" v-if="showConfirm">
				<view class="u-calendar__confirm">
					<u-button
						shape="circle"
						:text="
                            buttonDisabled ? confirmDisabledText : confirmText
                        "
						:color="color"
						@click="confirm"
						:disabled="buttonDisabled"
					></u-button>
				</view>
			</slot>
		</view>
	</u-popup>
</template>

<script>
import uHeader from './header.vue'
import uMonth from './month.vue'
import { props } from './props.js'
import util from './util.js'
import dayjs from 'dayjs/esm/index'
import Calendar from '../../libs/util/calendar.js'
import { mpMixin } from '../../libs/mixin/mpMixin.js'
import { mixin } from '../../libs/mixin/mixin.js'
import { addUnit, range, error, padZero } from '../../libs/function/index';
import test from '../../libs/function/test';
/**
 * Calendar 日历
 * @description  此组件用于單個選擇日期，範圍選擇日期等，日历被包裹在底部彈起的容器中.
 * @tutorial https://ijry.github.io/uview-plus/components/calendar.html
 *
 * @property {String}				title				標題内容 (默認 日期選擇 )
 * @property {Boolean}				showTitle			是否顯示標題  (默認 true )
 * @property {Boolean}				showSubtitle		是否顯示副標題	(默認 true )
 * @property {String}				mode				日期類型選擇  single-選擇單個日期，multiple-可以選擇多個日期，range-選擇日期範圍 （ 默認 'single' )
 * @property {String}				startText			mode=range時，第一個日期底部的提示文字  (默認 '開始' )
 * @property {String}				endText				mode=range時，最后一個日期底部的提示文字 (默認 '结束' )
 * @property {Array}				customList			自定義列表
 * @property {String}				color				主題色，對底部按鈕和選中日期有效  (默認 ‘#3c9cff' )
 * @property {String | Number}		minDate				最小的可選日期	 (默認 0 )
 * @property {String | Number}		maxDate				最大可選日期  (默認 0 )
 * @property {Array | String| Date}	defaultDate			默認選中的日期，mode為multiple或range是必須為數组格式
 * @property {String | Number}		maxCount			mode=multiple時，最多可選多少個日期  (默認 	Number.MAX_SAFE_INTEGER  )
 * @property {String | Number}		rowHeight			日期行高 (默認 56 )
 * @property {Function}				formatter			日期格式化函數
 * @property {Boolean}				showLunar			是否顯示农历  (默認 false )
 * @property {Boolean}				showMark			是否顯示月份背景色 (默認 true )
 * @property {String}				confirmText			確定按鈕的文字 (默認 '確定' )
 * @property {String}				confirmDisabledText	確認按鈕處于禁用狀態時的文字 (默認 '確定' )
 * @property {Boolean}				show				是否顯示日历彈窗 (默認 false )
 * @property {Boolean}				closeOnClickOverlay	是否允许點擊遮罩關闭日历 (默認 false )
 * @property {Boolean}				readonly	        是否為只讀狀態，只讀狀態下禁止選擇日期 (默認 false )
 * @property {String | Number}		maxRange	        日期區間最多可選天數，默認無限制，mode = range時有效
 * @property {String}				rangePrompt	        範圍選擇超過最多可選天數時的提示文案，mode = range時有效
 * @property {Boolean}				showRangePrompt	    範圍選擇超過最多可選天數時，是否展示提示文案，mode = range時有效 (默認 true )
 * @property {Boolean}				allowSameDay	    是否允许日期範圍的起止時間為同一天，mode = range時有效 (默認 false )
 * @property {Number|String}	    round				圆角值，默認無圆角  (默認 0 )
 * @property {Number|String}	    monthNum			最多展示的月份數量  (默認 3 )
 *
 * @event {Function()} confirm 		點擊確定按鈕時触發		選擇日期相關的返回参數
 * @event {Function()} close 		日历關闭時触發			可定義頁面關闭時的回調事件
 * @example <u-calendar  :defaultDate="defaultDateMultiple" :show="show" mode="multiple" @confirm="confirm">
	</u-calendar>
 * */
export default {
	name: 'u-calendar',
	mixins: [mpMixin, mixin, props],
	components: {
		uHeader,
		uMonth
	},
	data() {
		return {
			// 需要顯示的月份的數组
			months: [],
			// 在月份滾動區域中，當前视圖中月份的index索引
			monthIndex: 0,
			// 月份滾動區域的高度
			listHeight: 0,
			// month组件中選擇的日期數组
			selected: [],
			scrollIntoView: '',
			scrollIntoViewScroll: '',
			scrollTop:0,
			// 過濾處理方法
			innerFormatter: (value) => value
		}
	},
	watch: {
		scrollIntoView: {
			immediate: true,
			handler(n) {
				// console.log('scrollIntoView', n)
			}
		},
		selectedChange: {
			immediate: true,
			handler(n) {
				this.setMonth()
			}
		},
		// 打開彈窗時，設置月份數據
		show: {
			immediate: true,
			handler(n) {
				if (n) {
					this.setMonth()
				} else {
					// 關闭時重置scrollIntoView，否则會出現二次打開日历，當前月份數據顯示不正確。
					// scrollIntoView需要有一個值變動過程，才會產生作用。
					this.scrollIntoView = ''
				}
			}
		}
	},
	computed: {
		// 由于maxDate和minDate可以為字符串(2021-10-10)，或者數值(時間戳)，但是dayjs如果接受字符串形式的時間戳會有問題，這里进行處理
		innerMaxDate() {
			return test.number(this.maxDate)
				? Number(this.maxDate)
				: this.maxDate
		},
		innerMinDate() {
			return test.number(this.minDate)
				? Number(this.minDate)
				: this.minDate
		},
		// 多個條件的變化，會引起選中日期的變化，這里统一管理監聽
		selectedChange() {
			return [this.innerMinDate, this.innerMaxDate, this.defaultDate]
		},
		subtitle() {
			// 初始化時，this.months為空數组，所以需要特别判斷處理
			if (this.months.length) {
				return `${this.months[this.monthIndex].year}年${
					this.months[this.monthIndex].month
				}月`
			} else {
				return ''
			}
		},
		buttonDisabled() {
			// 如果為range類型，且選擇的日期個數不足1個時，让底部的按鈕出于disabled狀態
			if (this.mode === 'range') {
				if (this.selected.length <= 1) {
					return true
				} else {
					return false
				}
			} else {
				return false
			}
		}
	},
	mounted() {
		this.start = Date.now()
		this.init()
	},
	emits: ["confirm", "close"],
	methods: {
		addUnit,
		// 在微信小程序中，不支持將函數當做props参數，故只能通過ref形式調用
		setFormatter(e) {
			this.innerFormatter = e
		},
		// month组件内部選擇日期后，通過事件通知给父组件
		monthSelected(e,scene ='init') {
			this.selected = e
			if (!this.showConfirm) {
				// 在不需要確認按鈕的情况下，如果為單選，或者範圍多選且已選長度大于2，则直接进行返還
				if (
					this.mode === 'multiple' ||
					this.mode === 'single' ||
					(this.mode === 'range' && this.selected.length >= 2)
				) {
				   if( scene === 'init'){
					 return
				   }
				   if( scene === 'tap') {
					 this.$emit('confirm', this.selected)
				   }
				}
			}
		},
		init() {
			// 校验maxDate，不能小于minDate。
			if (
				this.innerMaxDate &&
                this.innerMinDate &&
				new Date(this.innerMaxDate).getTime() < new Date(this.innerMinDate).getTime()
			) {
				return error('maxDate不能小于minDate時間')
			}
			// 滾動區域的高度
			this.listHeight = this.rowHeight * 5 + 30
			this.setMonth()
		},
		close() {
			this.$emit('close')
		},
		// 點擊確定按鈕
		confirm() {
			if (!this.buttonDisabled) {
				this.$emit('confirm', this.selected)
			}
		},
		// 獲得两個日期之間的月份數
		getMonths(minDate, maxDate) {
			const minYear = dayjs(minDate).year()
			const minMonth = dayjs(minDate).month() + 1
			const maxYear = dayjs(maxDate).year()
			const maxMonth = dayjs(maxDate).month() + 1
			return (maxYear - minYear) * 12 + (maxMonth - minMonth) + 1
		},
		// 設置月份數據
		setMonth() {
			// 最小日期的毫秒數
			const minDate = this.innerMinDate || dayjs().valueOf()
			// 如果没有指定最大日期，则往后推3個月
			const maxDate =
				this.innerMaxDate ||
				dayjs(minDate)
					.add(this.monthNum - 1, 'month')
					.valueOf()
			// 最大最小月份之間的共有多少個月份，
			const months = range(
				1,
				this.monthNum,
				this.getMonths(minDate, maxDate)
			)
			// 先清空數组
			this.months = []
			for (let i = 0; i < months; i++) {
				this.months.push({
					date: new Array(
						dayjs(minDate).add(i, 'month').daysInMonth()
					)
						.fill(1)
						.map((item, index) => {
							// 日期，取值1-31
							let day = index + 1
							// 星期，0-6，0為周日
							const week = dayjs(minDate)
								.add(i, 'month')
								.date(day)
								.day()
							const date = dayjs(minDate)
								.add(i, 'month')
								.date(day)
								.format('YYYY-MM-DD')
							let bottomInfo = ''
							if (this.showLunar) {
								// 將日期轉為农历格式
								const lunar = Calendar.solar2lunar(
									dayjs(date).year(),
									dayjs(date).month() + 1,
									dayjs(date).date()
								)
								bottomInfo = lunar.IDayCn
							}
							let config = {
								day,
								week,
								// 小于最小允许的日期，或者大于最大的日期，则設置為disabled狀態
								disabled:
									dayjs(date).isBefore(
										dayjs(minDate).format('YYYY-MM-DD')
									) ||
									dayjs(date).isAfter(
										dayjs(maxDate).format('YYYY-MM-DD')
									),
								// 返回一個日期對象，供外部的formatter獲取當前日期的年月日等信息，进行加工處理
								date: new Date(date),
								bottomInfo,
								dot: false,
								month:
									dayjs(minDate).add(i, 'month').month() + 1
							}
							const formatter =
								this.formatter || this.innerFormatter
							return formatter(config)
						}),
					// 當前所屬的月份
					month: dayjs(minDate).add(i, 'month').month() + 1,
					// 當前年份
					year: dayjs(minDate).add(i, 'month').year()
				})
			}
		},
		// 滾動到默認設置的月份
		scrollIntoDefaultMonth(selected) {
			// 查詢默認日期在可選列表的下標
			const _index = this.months.findIndex(({
				  year,
				  month
			  }) => {
				month = padZero(month)
				return `${year}-${month}` === selected
			})
			if (_index !== -1) {
				// #ifndef MP-WEIXIN
				this.$nextTick(() => {
					this.scrollIntoView = `month-${_index}`
					this.scrollIntoViewScroll = this.scrollIntoView
				})
				// #endif
				// #ifdef MP-WEIXIN
				this.scrollTop = this.months[_index].top || 0;
				// #endif
			}
		},
		// scroll-view滾動監聽
		onScroll(event) {
			// 不允许小于0的滾動值，如果scroll-view到顶了，继续下拉，會出現负數值
			const scrollTop = Math.max(0, event.detail.scrollTop)
			// 將當前滾動條數值，除以滾動區域的高度，可以得出當前滾動到了哪一個月份的索引
			for (let i = 0; i < this.months.length; i++) {
				if (scrollTop >= (this.months[i].top || this.listHeight)) {
					this.monthIndex = i
					this.scrollIntoViewScroll = `month-${i}`
				}
			}
		},
		// 更新月份的top值
		updateMonthTop(topArr = []) {
			// 設置對應月份的top值，用于onScroll方法更新月份
			topArr.map((item, index) => {
				this.months[index].top = item
			})

			// 獲取默認日期的下標
			if (!this.defaultDate) {
				// 如果没有設置默認日期，则將當天日期設置為默認選中的日期
				const selected = dayjs().format("YYYY-MM")
				this.scrollIntoDefaultMonth(selected)
				return
			}
			let selected = dayjs().format("YYYY-MM");
			// 單選模式，可以是字符串或數组，Date對象等
			if (!test.array(this.defaultDate)) {
				selected = dayjs(this.defaultDate).format("YYYY-MM")
			} else {
				selected = dayjs(this.defaultDate[0]).format("YYYY-MM");
			}
			this.scrollIntoDefaultMonth(selected)
		}
	}
}
</script>

<style lang="scss" scoped>
@import '../../libs/css/components.scss';

.u-calendar {
	&__confirm {
		padding: 7px 18px;
	}
}
</style>
