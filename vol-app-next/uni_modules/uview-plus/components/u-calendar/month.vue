<template>
	<view class="u-calendar-month-wrapper" ref="u-calendar-month-wrapper">
		<view v-for="(item, index) in months" :key="index" :class="[`u-calendar-month-${index}`]"
			:ref="`u-calendar-month-${index}`" :id="`month-${index}`">
			<text v-if="index !== 0" class="u-calendar-month__title">{{ item.year }}年{{ item.month }}月</text>
			<view class="u-calendar-month__days">
				<view v-if="showMark" class="u-calendar-month__days__month-mark-wrapper">
					<text class="u-calendar-month__days__month-mark-wrapper__text">{{ item.month }}</text>
				</view>
				<view class="u-calendar-month__days__day" v-for="(item1, index1) in item.date" :key="index1"
					:style="[dayStyle(index, index1, item1)]" @tap="clickHandler(index, index1, item1)"
					:class="[item1.selected && 'u-calendar-month__days__day__select--selected']">
					<view class="u-calendar-month__days__day__select" :style="[daySelectStyle(index, index1, item1)]">
						<text class="u-calendar-month__days__day__select__info"
							:class="[item1.disabled && 'u-calendar-month__days__day__select__info--disabled']"
							:style="[textStyle(item1)]">{{ item1.day }}</text>
						<text v-if="getBottomInfo(index, index1, item1)"
							class="u-calendar-month__days__day__select__buttom-info"
							:class="[item1.disabled && 'u-calendar-month__days__day__select__buttom-info--disabled']"
							:style="[textStyle(item1)]">{{ getBottomInfo(index, index1, item1) }}</text>
						<text v-if="item1.dot" class="u-calendar-month__days__day__select__dot"></text>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	// #ifdef APP-NVUE
	// 由于nvue不支持百分比單位，需要查詢寬度来計算每個日期的寬度
	const dom = uni.requireNativePlugin('dom')
	// #endif
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, deepClone, toast, sleep } from '../../libs/function/index';
	import { colorGradient } from '../../libs/function/colorGradient';
	import test from '../../libs/function/test';
	import defProps from '../../libs/config/props';
	import dayjs from 'dayjs/esm/index'
	export default {
		name: 'u-calendar-month',
		mixins: [mpMixin, mixin],
		props: {
			// 是否顯示月份背景色
			showMark: {
				type: Boolean,
				default: true
			},
			// 主題色，對底部按鈕和選中日期有效
			color: {
				type: String,
				default: '#3c9cff'
			},
			// 月份數據
			months: {
				type: Array,
				default: () => []
			},
			// 日期選擇類型
			mode: {
				type: String,
				default: 'single'
			},
			// 日期行高
			rowHeight: {
				type: [String, Number],
				default: 58
			},
			// mode=multiple時，最多可選多少個日期
			maxCount: {
				type: [String, Number],
				default: Infinity
			},
			// mode=range時，第一個日期底部的提示文字
			startText: {
				type: String,
				default: '開始'
			},
			// mode=range時，最后一個日期底部的提示文字
			endText: {
				type: String,
				default: '结束'
			},
			// 默認選中的日期，mode為multiple或range是必須為數组格式
			defaultDate: {
				type: [Array, String, Date],
				default: null
			},
			// 最小的可選日期
			minDate: {
				type: [String, Number],
				default: 0
			},
			// 最大可選日期
			maxDate: {
				type: [String, Number],
				default: 0
			},
			// 如果没有設置maxDate，则往后推多少個月
			maxMonth: {
				type: [String, Number],
				default: 2
			},
			// 是否為只讀狀態，只讀狀態下禁止選擇日期
			readonly: {
				type: Boolean,
				default: () => defProps.calendar.readonly
			},
			// 日期區間最多可選天數，默認無限制，mode = range時有效
			maxRange: {
				type: [Number, String],
				default: Infinity
			},
			// 範圍選擇超過最多可選天數時的提示文案，mode = range時有效
			rangePrompt: {
				type: String,
				default: ''
			},
			// 範圍選擇超過最多可選天數時，是否展示提示文案，mode = range時有效
			showRangePrompt: {
				type: Boolean,
				default: true
			},
			// 是否允许日期範圍的起止時間為同一天，mode = range時有效
			allowSameDay: {
				type: Boolean,
				default: false
			}
		},
		data() {
			return {
				// 每個日期的寬度
				width: 0,
				// 當前選中的日期item
				item: {},
				selected: []
			}
		},
		watch: {
			selectedChange: {
				immediate: true,
				handler(n) {
					this.setDefaultDate()
				}
			}
		},
		computed: {
			// 多個條件的變化，會引起選中日期的變化，這里统一管理監聽
			selectedChange() {
				return [this.minDate, this.maxDate, this.defaultDate]
			},
			dayStyle(index1, index2, item) {
				return (index1, index2, item) => {
					const style = {}
					let week = item.week
					// 不进行四舍五入的形式保留2位小數
					const dayWidth = Number(parseFloat(this.width / 7).toFixed(3).slice(0, -1))
					// 得出每個日期的寬度
					// #ifdef APP-NVUE
					style.width = addUnit(dayWidth, 'px')
					// #endif
					style.height = addUnit(this.rowHeight)
					if (index2 === 0) {
						// 獲取當前為星期几，如果為0，则為星期天，减一為每月第一天時，需要向左偏移的item個數
						week = (week === 0 ? 7 : week) - 1
						style.marginLeft = addUnit(week * dayWidth, 'px')
					}
					if (this.mode === 'range') {
						// 之所以需要這么写，是因為DCloud公司的iOS客户端的開發者能力有限导致的bug
						style.paddingLeft = 0
						style.paddingRight = 0
						style.paddingBottom = 0
						style.paddingTop = 0
					}
					return style
				}
			},
			daySelectStyle() {
				return (index1, index2, item) => {
					let date = dayjs(item.date).format("YYYY-MM-DD"),
						style = {}
					// 判斷date是否在selected數组中，因為月份可能會需要补0，所以使用dateSame判斷，而不用數组的includes判斷
					if (this.selected.some(item => this.dateSame(item, date))) {
						style.backgroundColor = this.color
					}
					if (this.mode === 'single') {
						if (date === this.selected[0]) {
							// 因為需要對nvue的兼容，只能這么写，無法缩写，也無法通過類名控制等等
							style.borderTopLeftRadius = '3px'
							style.borderBottomLeftRadius = '3px'
							style.borderTopRightRadius = '3px'
							style.borderBottomRightRadius = '3px'
						}
					} else if (this.mode === 'range') {
						if (this.selected.length >= 2) {
							const len = this.selected.length - 1
							// 第一個日期設置左上角和左下角的圆角
							if (this.dateSame(date, this.selected[0])) {
								style.borderTopLeftRadius = '3px'
								style.borderBottomLeftRadius = '3px'
							}
							// 最后一個日期設置右上角和右下角的圆角
							if (this.dateSame(date, this.selected[len])) {
								style.borderTopRightRadius = '3px'
								style.borderBottomRightRadius = '3px'
							}
							// 處于第一和最后一個之間的日期，背景色設置為浅色，通過將對應顏色进行等分，再取其尾部的顏色值
							if (dayjs(date).isAfter(dayjs(this.selected[0])) && dayjs(date).isBefore(dayjs(this
									.selected[len]))) {
								style.backgroundColor = colorGradient(this.color, '#ffffff', 100)[90]
								// 增加一個透明度，让範圍區間的背景色也能看到底部的mark水印字符
								style.opacity = 0.7
							}
						} else if (this.selected.length === 1) {
							// 之所以需要這么写，是因為DCloud公司的iOS客户端的開發者能力有限导致的bug
							// 进行還原操作，否则在nvue的iOS，uni-app有bug，會导致诡異的表現
							style.borderTopLeftRadius = '3px'
							style.borderBottomLeftRadius = '3px'
						}
					} else {
						if (this.selected.some(item => this.dateSame(item, date))) {
							style.borderTopLeftRadius = '3px'
							style.borderBottomLeftRadius = '3px'
							style.borderTopRightRadius = '3px'
							style.borderBottomRightRadius = '3px'
						}
					}
					return style
				}
			},
			// 某個日期是否被選中
			textStyle() {
				return (item) => {
					const date = dayjs(item.date).format("YYYY-MM-DD"),
						style = {}
					// 選中的日期，提示文字設置白色
					if (this.selected.some(item => this.dateSame(item, date))) {
						style.color = '#ffffff'
					}
					if (this.mode === 'range') {
						const len = this.selected.length - 1
						// 如果是範圍選擇模式，第一個和最后一個之間的日期，文字顏色設置為高亮的主題色
						if (dayjs(date).isAfter(dayjs(this.selected[0])) && dayjs(date).isBefore(dayjs(this
								.selected[len]))) {
							style.color = this.color
						}
					}
					return style
				}
			},
			// 獲取底部的提示文字
			getBottomInfo() {
				return (index1, index2, item) => {
					const date = dayjs(item.date).format("YYYY-MM-DD")
					const bottomInfo = item.bottomInfo
					// 當為日期範圍模式時，且選擇的日期個數大于0時
					if (this.mode === 'range' && this.selected.length > 0) {
						if (this.selected.length === 1) {
							// 選擇了一個日期時，如果當前日期為數组中的第一個日期，则顯示底部文字為“開始”
							if (this.dateSame(date, this.selected[0])) return this.startText
							else return bottomInfo
						} else {
							const len = this.selected.length - 1
							// 如果數组中的日期大于2個時，第一個和最后一個顯示為開始和结束日期
							if (this.dateSame(date, this.selected[0]) && this.dateSame(date, this.selected[1]) &&
								len === 1) {
								// 如果長度為2，且第一個等于第二個日期，则提示語放在同一個item中
								return `${this.startText}/${this.endText}`
							} else if (this.dateSame(date, this.selected[0])) {
								return this.startText
							} else if (this.dateSame(date, this.selected[len])) {
								return this.endText
							} else {
								return bottomInfo
							}
						}
					} else {
						return bottomInfo
					}
				}
			}
		},
		mounted() {
			this.init()
		},
		methods: {
			init() {
				// 初始化默認選中
				this.$emit('monthSelected', this.selected)
				this.$nextTick(() => {
					// 這里需要另一個延時，因為獲取寬度后，會进行月份數據渲染，只有渲染完成之后，才有真正的高度
					// 因為nvue下，$nextTick並不是100%可靠的
					sleep(10).then(() => {
						this.getWrapperWidth()
						this.getMonthRect()
					})
				})
			},
			// 判斷两個日期是否相等
			dateSame(date1, date2) {
				return dayjs(date1).isSame(dayjs(date2))
			},
			// 獲取月份數據區域的寬度，因為nvue不支持百分比，所以無法通過css設置每個日期item的寬度
			getWrapperWidth() {
				// #ifdef APP-NVUE
				dom.getComponentRect(this.$refs['u-calendar-month-wrapper'], res => {
					this.width = res.size.width
				})
				// #endif
				// #ifndef APP-NVUE
				this.$uGetRect('.u-calendar-month-wrapper').then(size => {
					this.width = size.width
				})
				// #endif
			},
			getMonthRect() {
				// 獲取每個月份數據的尺寸，用于父组件在scroll-view滾動事件中，監聽當前滾動到了第几個月份
				const promiseAllArr = this.months.map((item, index) => this.getMonthRectByPromise(
					`u-calendar-month-${index}`))
				// 一次性返回
				Promise.all(promiseAllArr).then(
					sizes => {
						let height = 1
						const topArr = []
						for (let i = 0; i < this.months.length; i++) {
							// 添加到months數组中，供scroll-view滾動事件中，判斷當前滾動到哪個月份
							topArr[i] = height
							height += sizes[i].height
						}
						// 由于微信下，無法通過this.months[i].top的形式(引用類型)去修改父组件的month的top值，所以使用事件形式對外發出
						this.$emit('updateMonthTop', topArr)
					})
			},
			// 獲取每個月份區域的尺寸
			getMonthRectByPromise(el) {
				// #ifndef APP-NVUE
				// $uGetRect為uView自带的節點查詢簡化方法，詳見文檔介绍：https://ijry.github.io/uview-plus/js/getRect.html
				// 组件内部一般用this.$uGetRect，對外的為uni.$u.getRect，二者功能一致，名稱不同
				return new Promise(resolve => {
					this.$uGetRect(`.${el}`).then(size => {
						resolve(size)
					})
				})
				// #endif

				// #ifdef APP-NVUE
				// nvue下，使用dom模块查詢元素高度
				// 返回一個promise，让調用此方法的主體能使用then回調
				return new Promise(resolve => {
					dom.getComponentRect(this.$refs[el][0], res => {
						resolve(res.size)
					})
				})
				// #endif
			},
			// 點擊某一個日期
			clickHandler(index1, index2, item) {
				if (this.readonly) {
					return;
				}
				this.item = item
				const date = dayjs(item.date).format("YYYY-MM-DD")
				if (item.disabled) return
				// 對上一次選擇的日期數组进行深度克隆
				let selected = deepClone(this.selected)
				if (this.mode === 'single') {
					// 單選情况下，让數组中的元素為當前點擊的日期
					selected = [date]
				} else if (this.mode === 'multiple') {
					if (selected.some(item => this.dateSame(item, date))) {
						// 如果點擊的日期已在數组中，则进行移除操作，也就是达到反選的效果
						const itemIndex = selected.findIndex(item => item === date)
						selected.splice(itemIndex, 1)
					} else {
						// 如果點擊的日期不在數组中，且已有的長度小于總可選長度時，则添加到數组中去
						if (selected.length < this.maxCount) selected.push(date)
					}
				} else {
					// 選擇區間形式
					if (selected.length === 0 || selected.length >= 2) {
						// 如果原来就為0或者大于2的長度，则當前點擊的日期，就是開始日期
						selected = [date]
					} else if (selected.length === 1) {
						// 如果已經選擇了開始日期
						const existsDate = selected[0]
						// 如果當前選擇的日期小于上一次選擇的日期，则當前的日期定為開始日期
						if (dayjs(date).isBefore(existsDate)) {
							selected = [date]
						} else if (dayjs(date).isAfter(existsDate)) {
							// 當前日期减去最大可選的日期天數，如果大于起始時間，则进行提示
							if(dayjs(dayjs(date).subtract(this.maxRange, 'day')).isAfter(dayjs(selected[0])) && this.showRangePrompt) {
								if(this.rangePrompt) {
									toast(this.rangePrompt)
								} else {
									toast(`選擇天數不能超過 ${this.maxRange} 天`)
								}
								return
							}
							// 如果當前日期大于已有日期，將當前的添加到數组尾部
							selected.push(date)
							const startDate = selected[0]
							const endDate = selected[1]
							const arr = []
							let i = 0
							do {
								// 將開始和结束日期之間的日期添加到數组中
								arr.push(dayjs(startDate).add(i, 'day').format("YYYY-MM-DD"))
								i++
								// 累加的日期小于结束日期時，继续下一次的循环
							} while (dayjs(startDate).add(i, 'day').isBefore(dayjs(endDate)))
							// 為了一次性修改數组，避免computed中多次触發，這里才用arr變量一次性赋值的方式，同時將最后一個日期添加近来
							arr.push(endDate)
							selected = arr
						} else {
							// 選擇區間時，只有一個日期的情况下，且不允许選擇起止為同一天的話，不允许選擇自己
							if (selected[0] === date && !this.allowSameDay) return
							selected.push(date)
						}
					}
				}
				this.setSelected(selected)
			},
			// 設置默認日期
			setDefaultDate() {
				if (!this.defaultDate) {
					// 如果没有設置默認日期，则將當天日期設置為默認選中的日期
					const selected = [dayjs().format("YYYY-MM-DD")]
					return this.setSelected(selected, false)
				}
				let defaultDate = []
				const minDate = this.minDate || dayjs().format("YYYY-MM-DD")
				const maxDate = this.maxDate || dayjs(minDate).add(this.maxMonth - 1, 'month').format("YYYY-MM-DD")
				if (this.mode === 'single') {
					// 單選模式，可以是字符串或數组，Date對象等
					if (!test.array(this.defaultDate)) {
						defaultDate = [dayjs(this.defaultDate).format("YYYY-MM-DD")]
					} else {
						defaultDate = [this.defaultDate[0]]
					}
				} else {
					// 如果為非數组，则不執行
					if (!test.array(this.defaultDate)) return
					defaultDate = this.defaultDate
				}
				// 過濾用户傳递的默認數组，取出只在可允许最大值與最小值之間的元素
				defaultDate = defaultDate.filter(item => {
					return dayjs(item).isAfter(dayjs(minDate).subtract(1, 'day')) && dayjs(item).isBefore(dayjs(
						maxDate).add(1, 'day'))
				})
				this.setSelected(defaultDate, false)
			},
			setSelected(selected, event = true) {
				this.selected = selected
				event && this.$emit('monthSelected', this.selected,'tap')
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-calendar-month-wrapper {
		margin-top: 4px;
	}

	.u-calendar-month {

		&__title {
			display: flex;
			flex-direction: column;
			font-size: 14px;
			line-height: 42px;
			height: 42px;
			color: $u-main-color;
			text-align: center;
			font-weight: bold;
		}

		&__days {
			position: relative;
			@include flex;
			flex-wrap: wrap;

			&__month-mark-wrapper {
				position: absolute;
				top: 0;
				bottom: 0;
				left: 0;
				right: 0;
				@include flex;
				justify-content: center;
				align-items: center;

				&__text {
					font-size: 155px;
					color: rgba(231, 232, 234, 0.83);
				}
			}

			&__day {
				@include flex;
				padding: 2px;
				/* #ifndef APP-NVUE */
				// vue下使用css进行寬度計算，因為某些安卓機會無法进行js獲取父元素寬度进行計算得出，會有偏移
				width: calc(100% / 7);
				box-sizing: border-box;
				/* #endif */

				&__select {
					flex: 1;
					@include flex;
					align-items: center;
					justify-content: center;
					position: relative;

					&__dot {
						width: 7px;
						height: 7px;
						border-radius: 100px;
						background-color: $u-error;
						position: absolute;
						top: 12px;
						right: 7px;
					}

					&__buttom-info {
						color: $u-content-color;
						text-align: center;
						position: absolute;
						bottom: 5px;
						font-size: 10px;
						text-align: center;
						left: 0;
						right: 0;

						&--selected {
							color: #ffffff;
						}

						&--disabled {
							color: #cacbcd;
						}
					}

					&__info {
						text-align: center;
						font-size: 16px;

						&--selected {
							color: #ffffff;
						}

						&--disabled {
							color: #cacbcd;
						}
					}

					&--selected {
						background-color: $u-primary;
						@include flex;
						justify-content: center;
						align-items: center;
						flex: 1;
						border-radius: 3px;
					}

					&--range-selected {
						opacity: 0.3;
						border-radius: 0;
					}

					&--range-start-selected {
						border-top-right-radius: 0;
						border-bottom-right-radius: 0;
					}

					&--range-end-selected {
						border-top-left-radius: 0;
						border-bottom-left-radius: 0;
					}
				}
			}
		}
	}
</style>
