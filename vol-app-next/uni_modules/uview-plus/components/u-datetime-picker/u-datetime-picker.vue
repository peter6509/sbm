<template>
    <view class="u-datetime-picker">
        <view v-if="hasInput" class="u-datetime-picker__has-input"
            @click="showByClickInput = !showByClickInput"
        >
            <up-input
                :placeholder="placeholder"
                :readonly="!!showByClickInput"
                border="surround"
                v-model="inputValue"
            ></up-input>
        </view>
        <u-picker
            ref="picker"
            :show="show || (hasInput && showByClickInput)"
            :popupMode="popupMode"
            :closeOnClickOverlay="closeOnClickOverlay"
            :columns="columns"
            :title="title"
            :itemHeight="itemHeight"
            :showToolbar="showToolbar"
            :visibleItemCount="visibleItemCount"
            :defaultIndex="innerDefaultIndex"
            :cancelText="cancelText"
            :confirmText="confirmText"
            :cancelColor="cancelColor"
            :confirmColor="confirmColor"
            :toolbarRightSlot="toolbarRightSlot"
            @close="close"
            @cancel="cancel"
            @confirm="confirm"
            @change="change"
        >
            <template #toolbar-right>
                <slot name="toolbar-right">
                </slot>
            </template>
            <template #toolbar-bottom>
                <slot name="toolbar-bottom">
                </slot>
            </template>
        </u-picker>
    </view>
</template>

<script>
	function times(n, iteratee) {
	    let index = -1
	    const result = Array(n < 0 ? 0 : n)
	    while (++index < n) {
	        result[index] = iteratee(index)
	    }
	    return result
	}
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import dayjs from 'dayjs/esm/index';
	import { range, error, padZero } from '../../libs/function/index';
	import test from '../../libs/function/test';
	/**
	 * DatetimePicker 時間日期選擇器
	 * @description 此選擇器用于時間日期
	 * @tutorial https://ijry.github.io/uview-plus/components/datetimePicker.html
	 * @property {Boolean}			show				用于控制選擇器的彈出與收起 ( 默認 false )
	 * @property {Boolean}			showToolbar			是否顯示顶部的操作栏  ( 默認 true )
	 * @property {String | Number}	modelValue		    绑定值
	 * @property {String}			title				顶部標題
	 * @property {String}			mode				展示格式 mode=date為日期選擇，mode=time為時間選擇，mode=year-month為年月選擇，mode=datetime為日期時間選擇  ( 默認 ‘datetime )
	 * @property {Number}			maxDate				可選的最大時間  默認值為后10年
	 * @property {Number}			minDate				可選的最小時間  默認值為前10年
	 * @property {Number}			minHour				可選的最小小時，仅mode=time有效   ( 默認 0 )
	 * @property {Number}			maxHour				可選的最大小時，仅mode=time有效	  ( 默認 23 )
	 * @property {Number}			minMinute			可選的最小分钟，仅mode=time有效	  ( 默認 0 )
	 * @property {Number}			maxMinute			可選的最大分钟，仅mode=time有效   ( 默認 59 )
	 * @property {Function}			filter				選項過濾函數
	 * @property {Function}			formatter			選項格式化函數
	 * @property {Boolean}			loading				是否顯示加載中狀態   ( 默認 false )
	 * @property {String | Number}	itemHeight			各列中，單個選項的高度   ( 默認 44 )
	 * @property {String}			cancelText			取消按鈕的文字  ( 默認 '取消' )
	 * @property {String}			confirmText			確認按鈕的文字  ( 默認 '確認' )
	 * @property {String}			cancelColor			取消按鈕的顏色  ( 默認 '#909193' )
	 * @property {String}			confirmColor		確認按鈕的顏色  ( 默認 '#3c9cff' )
	 * @property {String | Number}	visibleItemCount	每列中可見選項的數量  ( 默認 5 )
	 * @property {Boolean}			closeOnClickOverlay	是否允许點擊遮罩關闭選擇器  ( 默認 false )
	 * @property {Array}			defaultIndex		各列的默認索引
	 * @event {Function} close 關闭選擇器時触發
	 * @event {Function} confirm 點擊確定按鈕，返回當前選擇的值
	 * @event {Function} change 當選擇值變化時触發
	 * @event {Function} cancel 點擊取消按鈕
	 * @example  <u-datetime-picker :show="show" :value="value1"  mode="datetime" ></u-datetime-picker>
	 */
	export default {
		name: 'up-datetime-picker',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
                // 原来的日期選擇器不方便，這里增加一個hasInput選項支持類似element的自带輸入框的功能。
                inputValue: '', // 表單顯示值
                showByClickInput: false, // 是否在hasInput模式下顯示日期選擇彈唱
				columns: [],
				innerDefaultIndex: [],
				innerFormatter: (type, value) => value
			}
		},
		watch: {
			show(newValue, oldValue) {
				if (newValue) {
					this.updateColumnValue(this.innerValue)
				}
			},
			// #ifdef VUE3
			modelValue(newValue) {
				this.init()
				// this.getInputValue(newValue)
			},
			// #endif
			// #ifdef VUE2
			value(newValue) {
				this.init()
				// this.getInputValue(newValue)
			},
			// #endif
			propsChange() {
				this.init()
			}
		},
		computed: {
			// 如果以下這些變量發生了變化，意味着需要重新初始化各列的值
			propsChange() {
				return [this.mode, this.maxDate, this.minDate, this.minHour, this.maxHour, this.minMinute, this.maxMinute, this.filter, ]
			}
		},
		mounted() {
			this.init()
		},
		// #ifdef VUE3
		emits: ['close', 'cancel', 'confirm', 'change', 'update:modelValue'],
		// #endif
		methods: {
			getInputValue(newValue) {
				if (newValue == '' || !newValue || newValue == undefined) {
					this.inputValue = ''
					return
				}
				if (this.mode == 'time') {
					this.inputValue = newValue
				} else {
					if (this.format) {
						this.inputValue = dayjs(newValue).format(this.format)
					} else {
						let format = ''
						switch (this.mode) {
							case 'date':
								format = 'YYYY-MM-DD'
								break;
							case 'year-month':
								format = 'YYYY-MM'
								break;
							case 'datetime':
								format = 'YYYY-MM-DD HH:mm'
								break;
							case 'time':
								format = 'HH:mm'
								break;
							default:
								break;
						}
						this.inputValue = dayjs(newValue).format(format)
					}
				}
			},
			init() {
				// #ifdef VUE3
				this.innerValue = this.correctValue(this.modelValue)
				// #endif
				// #ifdef VUE2
				this.innerValue = this.correctValue(this.value)
				// #endif
				this.updateColumnValue(this.innerValue)

				// 初始化hasInput展示
				this.getInputValue(this.innerValue)
			},
			// 在微信小程序中，不支持將函數當做props参數，故只能通過ref形式調用
			setFormatter(e) {
				this.innerFormatter = e
			},
			// 關闭選擇器
			close() {
				if (this.closeOnClickOverlay) {
					this.$emit('close')
				}
			},
			// 點擊工具栏的取消按鈕
			cancel() {
                if (this.hasInput) {
                    this.showByClickInput = false
                }
				this.$emit('cancel')
			},
			// 點擊工具栏的確定按鈕
			confirm() {
				// #ifdef VUE3
				this.$emit('update:modelValue', this.innerValue)
				// #endif
				// #ifdef VUE2
				this.$emit('input', this.innerValue)
				// #endif
                if (this.hasInput) {
					this.getInputValue(this.innerValue)
                    this.showByClickInput = false
                }
				this.$emit('confirm', {
					value: this.innerValue,
					mode: this.mode
				})
			},
			//用正则截取輸出值,當出現多组數字時,抛出错误
			intercept(e,type){
				let judge = e.match(/\d+/g)
				//判斷是否掺杂數字
				if(judge.length>1){
					error("請勿在過濾或格式化函數時添加數字")
					return 0
				}else if(type&&judge[0].length==4){//判斷是否是年份
					return judge[0]
				}else if(judge[0].length>2){
					error("請勿在過濾或格式化函數時添加數字")
					return 0
				}else{
					return judge[0]
				}
			},
			// 列發生變化時触發
			change(e) {
				const { indexs, values } = e
				let selectValue = ''
				if(this.mode === 'time') {
					// 根據value各列索引，從各列數组中，取出當前時間的選中值
					selectValue = `${this.intercept(values[0][indexs[0]])}:${this.intercept(values[1][indexs[1]])}`
				} else {
					// 將選擇的值轉為數值，比如'03'轉為數值的3，'2019'轉為數值的2019
					const year = parseInt(this.intercept(values[0][indexs[0]],'year'))
					const month = parseInt(this.intercept(values[1][indexs[1]]))
					let date = parseInt(values[2] ? this.intercept(values[2][indexs[2]]) : 1)
					let hour = 0, minute = 0
					// 此月份的最大天數
					const maxDate = dayjs(`${year}-${month}`).daysInMonth()
					// year-month模式下，date不會出現在列中，設置為1，為了符合后邊需要减1的需求
					if (this.mode === 'year-month') {
					    date = 1
					}
					// 不允许超過maxDate值
					date = Math.min(maxDate, date)
					if (this.mode === 'datetime') {
					    hour = parseInt(this.intercept(values[3][indexs[3]]))
					    minute = parseInt(this.intercept(values[4][indexs[4]]))
					}
					// 轉為時間模式
					selectValue = Number(new Date(year, month - 1, date, hour, minute))
				}
				// 取出准確的合法值，防止超越邊界的情况
				selectValue = this.correctValue(selectValue)
				this.innerValue = selectValue
				this.updateColumnValue(selectValue)
				// 發出change時間，value為當前選中的時間戳
				this.$emit('change', {
					value: selectValue,
					// #ifndef MP-WEIXIN
					// 微信小程序不能傳递this實例，會因為循环引用而報错
					// picker: this.$refs.picker,
					// #endif
					mode: this.mode
				})
			},
			// 更新各列的值，进行补0、格式化等操作
			updateColumnValue(value) {
				this.innerValue = value
				this.updateColumns()
				// 延迟執行,等待u-picker组件列數據更新完后再設置選中值索引
				setTimeout(() => {
				this.updateIndexs(value)
				}, 0);
			},
			// 更新索引
			updateIndexs(value) {
				let values = []
				const formatter = this.formatter || this.innerFormatter
				if (this.mode === 'time') {
					// 將time模式的時間用:分隔成數组
				    const timeArr = value.split(':')
					// 使用formatter格式化方法进行管道處理
				    values = [formatter('hour', timeArr[0]), formatter('minute', timeArr[1])]
				} else {
				    const date = new Date(value)
				    values = [
				        formatter('year', `${dayjs(value).year()}`),
						// 月份补0
				        formatter('month', padZero(dayjs(value).month() + 1))
				    ]
				    if (this.mode === 'date') {
						// date模式，需要添加天列
				        values.push(formatter('day', padZero(dayjs(value).date())))
				    }
				    if (this.mode === 'datetime') {
						// 數组的push方法，可以写入多個参數
				        values.push(formatter('day', padZero(dayjs(value).date())), formatter('hour', padZero(dayjs(value).hour())), formatter('minute', padZero(dayjs(value).minute())))
				    }
				}

				// 根據當前各列的所有值，從各列默認值中找到默認值在各列中的索引
				const indexs = this.columns.map((column, index) => {
					// 通過取大值，可以保证不會出現找不到索引的-1情况
					return Math.max(0, column.findIndex(item => item === values[index]))
				})
				this.innerDefaultIndex = indexs
			},
			// 更新各列的值
			updateColumns() {
			    const formatter = this.formatter || this.innerFormatter
				// 獲取各列的值，並且map后，對各列的具體值进行补0操作
			    const results = this.getOriginColumns().map((column) => column.values.map((value) => formatter(column.type, value)))
				this.columns = results
			},
			getOriginColumns() {
			    // 生成各列的值
			    const results = this.getRanges().map(({ type, range }) => {
			        let values = times(range[1] - range[0] + 1, (index) => {
			            let value = range[0] + index
			            value = type === 'year' ? `${value}` : padZero(value)
			            return value
			        })
					// 进行過濾
			        if (this.filter) {
			            values = this.filter(type, values)
						if (!values || (values && values.length == 0)) {
							// uni.showToast({
							// 	title: '日期filter结果不能為空',
							// 	icon: 'error',
							// 	mask: true
							// })
							console.log('日期filter结果不能為空')
						}
			        }
			        return { type, values }
			    })
			    return results
			},
			// 通過最大值和最小值生成數组
			generateArray(start, end) {
				return Array.from(new Array(end + 1).keys()).slice(start)
			},
			// 得出合法的時間
			correctValue(value) {
				const isDateMode = this.mode !== 'time'
				// if (isDateMode && !test.date(value)) {
				if (isDateMode && !dayjs.unix(value).isValid()) {
					// 如果是日期類型，但是又没有設置合法的當前時間的話，使用最小時間為當前時間
					value = this.minDate
				} else if (!isDateMode && !value) {
					// 如果是時間類型，而又没有默認值的話，就用最小時間
					value = `${padZero(this.minHour)}:${padZero(this.minMinute)}`
				}
				// 時間類型
				if (!isDateMode) {
					if (String(value).indexOf(':') === -1) return error('時間错误，請傳递如12:24的格式')
					let [hour, minute] = value.split(':')
					// 對時間补零，同時控制在最小值和最大值之間
					hour = padZero(range(this.minHour, this.maxHour, Number(hour)))
					minute = padZero(range(this.minMinute, this.maxMinute, Number(minute)))
					return `${ hour }:${ minute }`
				} else {
					// 如果是日期格式，控制在最小日期和最大日期之間
					value = dayjs(value).isBefore(dayjs(this.minDate)) ? this.minDate : value
					value = dayjs(value).isAfter(dayjs(this.maxDate)) ? this.maxDate : value
					return value
				}
			},
			// 獲取每列的最大和最小值
			getRanges() {
			    if (this.mode === 'time') {
			        return [
			            {
			                type: 'hour',
			                range: [this.minHour, this.maxHour],
			            },
			            {
			                type: 'minute',
			                range: [this.minMinute, this.maxMinute],
			            },
			        ];
			    }
			    const { maxYear, maxDate, maxMonth, maxHour, maxMinute, } = this.getBoundary('max', this.innerValue);
			    const { minYear, minDate, minMonth, minHour, minMinute, } = this.getBoundary('min', this.innerValue);
			    const result = [
			        {
			            type: 'year',
			            range: [minYear, maxYear],
			        },
			        {
			            type: 'month',
			            range: [minMonth, maxMonth],
			        },
			        {
			            type: 'day',
			            range: [minDate, maxDate],
			        },
			        {
			            type: 'hour',
			            range: [minHour, maxHour],
			        },
			        {
			            type: 'minute',
			            range: [minMinute, maxMinute],
			        },
			    ];
			    if (this.mode === 'date')
			        result.splice(3, 2);
			    if (this.mode === 'year-month')
			        result.splice(2, 3);
			    return result;
			},
			// 根據minDate、maxDate、minHour、maxHour等邊界值，判斷各列的開始和结束邊界值
			getBoundary(type, innerValue) {
			    const value = new Date(innerValue)
			    const boundary = new Date(this[`${type}Date`])
			    const year = dayjs(boundary).year()
			    let month = 1
			    let date = 1
			    let hour = 0
			    let minute = 0
			    if (type === 'max') {
			        month = 12
					// 月份的天數
			        date = dayjs(value).daysInMonth()
			        hour = 23
			        minute = 59
			    }
				// 獲取邊界值，逻輯是：當年达到了邊界值(最大或最小年)，就檢查月允许的最大和最小值，以此類推
			    if (dayjs(value).year() === year) {
			        month = dayjs(boundary).month() + 1
			        if (dayjs(value).month() + 1 === month) {
			            date = dayjs(boundary).date()
			            if (dayjs(value).date() === date) {
			                hour = dayjs(boundary).hour()
			                if (dayjs(value).hour() === hour) {
			                    minute = dayjs(boundary).minute()
			                }
			            }
			        }
			    }
			    return {
			        [`${type}Year`]: year,
			        [`${type}Month`]: month,
			        [`${type}Date`]: date,
			        [`${type}Hour`]: hour,
			        [`${type}Minute`]: minute
			    }
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import '../../libs/css/components.scss';
	.u-datetime-picker {
        &__has-input {
            /* #ifndef APP-NVUE */
            width: 100%;
            /* #endif */
        }
	}
</style>
