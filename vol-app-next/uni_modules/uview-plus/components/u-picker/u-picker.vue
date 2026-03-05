<template>
    <view class="u-picker-warrper">
        <view v-if="hasInput" class="u-picker-input cursor-pointer" @click="showByClickInput = !showByClickInput">
            <slot>
                <view>
					{{ inputLabel && inputLabel.length ? inputLabel.join('/') : placeholder }}
				</view>
            </slot>
        </view>
		<u-popup
			:show="show || (hasInput && showByClickInput)"
			:mode="popupMode"
			@close="closeHandler"
		>
			<view class="u-picker">
				<u-toolbar
					v-if="showToolbar"
					:cancelColor="cancelColor"
					:confirmColor="confirmColor"
					:cancelText="cancelText"
					:confirmText="confirmText"
					:title="title"
					:rightSlot="toolbarRightSlot ? true : false"
					@cancel="cancel"
					@confirm="confirm"
				>
					<template #right>
						<slot name="toolbar-right"></slot>
					</template>
				</u-toolbar>
				<slot name="toolbar-bottom"></slot>
				<picker-view
					class="u-picker__view"
					:indicatorStyle="`height: ${addUnit(itemHeight)}`"
					:value="innerIndex"
					:immediateChange="immediateChange"
					:style="{
						height: `${addUnit(visibleItemCount * itemHeight)}`
					}"
					@change="changeHandler"
				>
					<picker-view-column
						v-for="(item, index) in innerColumns"
						:key="index"
						class="u-picker__view__column"
					>
						<view
							v-if="testArray(item)"
							class="u-picker__view__column__item u-line-1"
							v-for="(item1, index1) in item"
							:key="index1"
							:style="{
								height: addUnit(itemHeight),
								lineHeight: addUnit(itemHeight),
								fontWeight: index1 === innerIndex[index] ? 'bold' : 'normal',
								display: 'block'
							}"
						>{{ getItemText(item1) }}</view>
					</picker-view-column>
				</picker-view>
				<view
					v-if="loading"
					class="u-picker--loading"
				>
					<u-loading-icon mode="circle"></u-loading-icon>
				</view>
			</view>
		</u-popup>
    </view>
</template>

<script>
/**
 * u-picker
 * @description 選擇器
 * @property {Boolean}			show				是否顯示picker彈窗（默認 false ）
 * @property {Boolean}			showToolbar			是否顯示顶部的操作栏（默認 true ）
 * @property {String}			title				顶部標題
 * @property {Array}			columns				對象數组，設置每一列的數據
 * @property {Boolean}			loading				是否顯示加載中狀態（默認 false ）
 * @property {String | Number}	itemHeight			各列中，單個選項的高度（默認 44 ）
 * @property {String}			cancelText			取消按鈕的文字（默認 '取消' ）
 * @property {String}			confirmText			確認按鈕的文字（默認 '確定' ）
 * @property {String}			cancelColor			取消按鈕的顏色（默認 '#909193' ）
 * @property {String}			confirmColor		確認按鈕的顏色（默認 '#3c9cff' ）
 * @property {String | Number}	visibleItemCount	每列中可見選項的數量（默認 5 ）
 * @property {String}			keyName				選項對象中，需要展示的屬性鍵名（默認 'text' ）
 * @property {Boolean}			closeOnClickOverlay	是否允许點擊遮罩關闭選擇器（默認 false ）
 * @property {Array}			defaultIndex		各列的默認索引
 * @property {Boolean}			immediateChange		是否在手指松開時立即触發change事件（默認 true ）
 * @event {Function} close		關闭選擇器時触發
 * @event {Function} cancel		點擊取消按鈕触發
 * @event {Function} change		當選擇值變化時触發
 * @event {Function} confirm	點擊確定按鈕，返回當前選擇的值
 */
import { props } from './props';
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { addUnit, deepClone, sleep } from '../../libs/function/index';
import test from '../../libs/function/test';
export default {
	name: 'u-picker',
	mixins: [mpMixin, mixin, props],
	data() {
		return {
			// 上一次選擇的列索引
			lastIndex: [],
			// 索引值 ，對應picker-view的value
			innerIndex: [],
			// 各列的值
			innerColumns: [],
			// 上一次的變化列索引
			columnIndex: 0,
            showByClickInput: false,
		}
	},
	watch: {
		// 監聽默認索引的變化，重新設置對應的值
		defaultIndex: {
			immediate: true,
			deep:true,
			handler(n) {
				this.setIndexs(n, true)
			}
		},
		// 監聽columns参數的變化
		columns: {
			immediate: true,
			deep:true,
			handler(n) {
				this.setColumns(n)
			}
		},
	},
	emits: ['close', 'cancel', 'confirm', 'change', 'update:modelValue', 'update:show'],
    computed: {
        inputLabel() {
            let items = this.innerColumns.map((item, index) => item[this.innerIndex[index]])
            let res = []
            items.forEach(element => {
                res.push(element[this.keyName])
            });
            return res
        },
        inputValue() {
            let items = this.innerColumns.map((item, index) => item[this.innerIndex[index]])
            let res = []
            items.forEach(element => {
                res.push(element['id'])
            });
            return res
        }
    },
	methods: {
		addUnit,
		testArray: test.array,
		// 獲取item需要顯示的文字，判别為對象還是文本
		getItemText(item) {
			if (test.object(item)) {
				return item[this.keyName]
			} else {
				return item
			}
		},
		// 關闭選擇器
		closeHandler() {
			if (this.closeOnClickOverlay) {
                if (this.hasInput) {
                    this.showByClickInput = false
                }
				this.$emit('update:show', false)
				this.$emit('close')
			}
		},
		// 點擊工具栏的取消按鈕
		cancel() {
            if (this.hasInput) {
                this.showByClickInput = false
            }
			this.$emit('update:show', false)
			this.$emit('cancel')
		},
		// 點擊工具栏的確定按鈕
		confirm() {
            this.$emit('update:modelValue', this.inputValue)
            if (this.hasInput) {
                this.showByClickInput = false
            }
			this.$emit('update:show', false)
			this.$emit('confirm', {
				indexs: this.innerIndex,
				value: this.innerColumns.map((item, index) => item[this.innerIndex[index]]),
				values: this.innerColumns
			})
		},
		// 選擇器某一列的數據發生變化時触發
		changeHandler(e) {
			const {
				value
			} = e.detail
			let index = 0,
				columnIndex = 0
			// 通過對比前后两次的列索引，得出當前變化的是哪一列
			for (let i = 0; i < value.length; i++) {
				let item = value[i]
				if (item !== (this.lastIndex[i] || 0)) { // 把undefined轉為合法假值0
					// 設置columnIndex為當前變化列的索引
					columnIndex = i
					// index则為變化列中的變化項的索引
					index = item
					break // 终止循环，即使少一次循环，也是性能的提升
				}
			}
			this.columnIndex = columnIndex
			const values = this.innerColumns
			// 將當前的各項變化索引，設置為"上一次"的索引變化值
			this.setLastIndex(value)
			this.setIndexs(value)

            this.$emit('update:modelValue', this.inputValue)

			this.$emit('change', {
				// #ifndef MP-WEIXIN || MP-LARK
				// 微信小程序不能傳递this，會因為循环引用而報错
				// picker: this,
				// #endif
				value: this.innerColumns.map((item, index) => item[value[index]]),
				index,
				indexs: value,
				// values為當前變化列的數组内容
				values,
				columnIndex
			})
		},
		// 設置index索引，此方法可被外部調用設置
		setIndexs(index, setLastIndex) {
			this.innerIndex = deepClone(index)
			if (setLastIndex) {
				this.setLastIndex(index)
			}
		},
		// 记錄上一次的各列索引位置
		setLastIndex(index) {
			// 當能进入此方法，意味着當前設置的各列默認索引，即為“上一次”的選中值，需要记錄，是因為changeHandler中
			// 需要拿前后的變化值进行對比，得出當前發生改變的是哪一列
			this.lastIndex = deepClone(index)
		},
		// 設置對應列選項的所有值
		setColumnValues(columnIndex, values) {
			// 替換innerColumns數组中columnIndex索引的值為values，使用的是數组的splice方法
			this.innerColumns.splice(columnIndex, 1, values)
            // 替換完成之后將修改列之后的已選值置空
			this.setLastIndex(this.innerIndex.slice(0, columnIndex))
			// 拷贝一份原有的innerIndex做临時變量，將大于當前變化列的所有的列的默認索引設置為0
			let tmpIndex = deepClone(this.innerIndex)
			for (let i = 0; i < this.innerColumns.length; i++) {
				if (i > this.columnIndex) {
					tmpIndex[i] = 0
				}
			}
			// 一次性赋值，不能單個修改，否则無效
			this.setIndexs(tmpIndex)
		},
		// 獲取對應列的所有選項
		getColumnValues(columnIndex) {
			// 进行同步阻塞，因為外部得到change事件之后，可能需要執行setColumnValues更新列的值
			// 索引如果在外部change的回調中調用getColumnValues的話，可能無法得到變更后的列值，這里进行一定延時，保证值的准確性
			(async () => {
				await sleep()
			})()
			return this.innerColumns[columnIndex]
		},
		// 設置整體各列的columns的值
		setColumns(columns) {
			// console.log(columns)
			this.innerColumns = deepClone(columns)
			// 如果在設置各列數據時，没有被設置默認的各列索引defaultIndex，那么用0去填充它，數组長度為列的數量
			if (this.innerIndex.length === 0) {
				this.innerIndex = new Array(columns.length).fill(0)
			}
		},
		// 獲取各列選中值對應的索引
		getIndexs() {
			return this.innerIndex
		},
		// 獲取各列選中的值
		getValues() {
			// 进行同步阻塞，因為外部得到change事件之后，可能需要執行setColumnValues更新列的值
			// 索引如果在外部change的回調中調用getValues的話，可能無法得到變更后的列值，這里进行一定延時，保证值的准確性
			(async () => {
				await sleep()
			})()
			return this.innerColumns.map((item, index) => item[this.innerIndex[index]])
		}
	},
}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-picker {
		position: relative;

		&__view {

			&__column {
				@include flex;
				flex: 1;
				justify-content: center;

				&__item {
					@include flex;
					justify-content: center;
					align-items: center;
					font-size: 16px;
					text-align: center;
					/* #ifndef APP-NVUE */
					display: block;
					/* #endif */
					color: $u-main-color;

					&--disabled {
						/* #ifndef APP-NVUE */
						cursor: not-allowed;
						/* #endif */
						opacity: 0.35;
					}
				}
			}
		}

		&--loading {
			position: absolute;
			top: 0;
			right: 0;
			left: 0;
			bottom: 0;
			@include flex;
			justify-content: center;
			align-items: center;
			background-color: rgba(255, 255, 255, 0.87);
			z-index: 1000;
		}
	}
</style>
