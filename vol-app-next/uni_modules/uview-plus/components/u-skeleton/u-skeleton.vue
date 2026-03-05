<template>
	<view class="u-skeleton">
		<view
		    class="u-skeleton__wrapper"
		    ref="u-skeleton__wrapper"
		    v-if="loading"
			style="display: flex; flex-direction: row;"
		>
			<view
			    class="u-skeleton__wrapper__avatar"
			    v-if="avatar"
			    :class="[`u-skeleton__wrapper__avatar--${avatarShape}`, animate && 'animate']"
			    :style="{
						height: addUnit(avatarSize),
						width: addUnit(avatarSize)
					}"
			></view>
			<view
			    class="u-skeleton__wrapper__content"
			    ref="u-skeleton__wrapper__content"
				style="flex: 1;"
			>
				<view
				    class="u-skeleton__wrapper__content__title"
				    v-if="title"
				    :style="{
							width: uTitleWidth,
							height: addUnit(titleHeight),
						}"
				    :class="[animate && 'animate']"
				></view>
				<view
				    class="u-skeleton__wrapper__content__rows"
				    :class="[animate && 'animate']"
				    v-for="(item, index) in rowsArray"
				    :key="index"
				    :style="{
							 width: item.width,
							 height: item.height,
							 marginTop: item.marginTop
						}"
				>
		
				</view>
			</view>
		</view>
		<slot v-else />
	</view>
</template>

<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, sleep, error } from '../../libs/function/index';
	import test from '../../libs/function/test';
	// #ifdef APP-NVUE
	// 由于weex為阿里的KPI业绩考核的產物，所以不支持百分比單位，這里需要通過dom查詢组件的寬度
	const dom = uni.requireNativePlugin('dom')
	const animation = uni.requireNativePlugin('animation')
	// #endif
	/**
	 * Skeleton 骨架屏
	 * @description 骨架屏一般用于頁面在請求远程數據尚未完成時，頁面用灰色块預顯示本来的頁面结构，给用户更好的體验。
	 * @tutorial https://ijry.github.io/uview-plus/components/skeleton.html
	 * @property {Boolean}					loading		是否顯示骨架占位圖，設置為false將會展示子组件内容 (默認 true )
	 * @property {Boolean}					animate		是否開啟動画效果 (默認 true )
	 * @property {String | Number}			rows		段落占位圖行數 (默認 0 )
	 * @property {String | Number | Array}	rowsWidth	段落占位圖的寬度，可以為百分比，數值，带單位字符串等，可通過數组傳入指定每個段落行的寬度 (默認 '100%' )
	 * @property {String | Number | Array}	rowsHeight	段落的高度 (默認 18 )
	 * @property {Boolean}					title		是否展示標題占位圖 (默認 true )
	 * @property {String | Number}			titleWidth	標題的寬度 (默認 '50%' )
	 * @property {String | Number}			titleHeight	標題的高度 (默認 18 )
	 * @property {Boolean}					avatar		是否展示頭像占位圖 (默認 false )
	 * @property {String | Number}			avatarSize	頭像占位圖大小 (默認 32 )
	 * @property {String}					avatarShape	頭像占位圖的形狀，circle-圆形，square-方形 (默認 'circle' )
	 * @example <u-search placeholder="日照香炉生紫烟" v-model="keyword"></u-search>
	 */
	export default {
		name: 'u-skeleton',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				width: 0,
			}
		},
		watch: {
			loading() {
				this.getComponentWidth()
			}
		},
		computed: {
			rowsArray() {
				if (/%$/.test(this.rowsHeight)) {
					error('rowsHeight参數不支持百分比單位')
				}
				const rows = []
				for (let i = 0; i < this.rows; i++) {
					let item = {},
						// 需要預防超出數组邊界的情况
						rowWidth = test.array(this.rowsWidth) ? (this.rowsWidth[i] || (i === this.rows - 1 ? '70%' : '100%')) : i ===
						this.rows - 1 ? '70%' : this.rowsWidth,
						rowHeight = test.array(this.rowsHeight) ? (this.rowsHeight[i] || '18px') : this.rowsHeight
					// 如果有title占位圖，第一個段落占位圖的外邊距需要大一些，如果没有title占位圖，第一個段落占位圖则無需外邊距
					// 之所以需要這么做，是因為weex的無能，以提升性能為借口不支持css的一些伪類
					item.marginTop = !this.title && i === 0 ? 0 : this.title && i === 0 ? '20px' : '12px'
					// 如果設置的為百分比的寬度，轉換為px值，因為nvue不支持百分比單位
					if (/%$/.test(rowWidth)) {
						// 通過parseInt提取出百分比單位中的數值部分，除以100得到百分比的小數值
						item.width = addUnit(this.width * parseInt(rowWidth) / 100)
					} else {
						item.width = addUnit(rowWidth)
					}
					item.height = addUnit(rowHeight)
					rows.push(item)
				}
				// console.log(rows);
				return rows
			},
			uTitleWidth() {
				let tWidth = 0
				if (/%$/.test(this.titleWidth)) {
					// 通過parseInt提取出百分比單位中的數值部分，除以100得到百分比的小數值
					tWidth = addUnit(this.width * parseInt(this.titleWidth) / 100)
				} else {
					tWidth = addUnit(this.titleWidth)
				}
				return addUnit(tWidth)
			},
			
		},
		mounted() {
			this.init()
		},
		methods: {
			addUnit,
			init() {
				this.getComponentWidth()
				// #ifdef APP-NVUE
				this.loading && this.animate && this.setNvueAnimation()
				// #endif
			},
			async setNvueAnimation() {
				// #ifdef APP-NVUE
				// 為了让opacity:1的狀態保持一定時間，這里做一個延時
				await sleep(500)
				const skeleton = this.$refs['u-skeleton__wrapper'];
				skeleton && this.loading && this.animate && animation.transition(skeleton, {
					styles: {
						opacity: 0.5
					},
					duration: 600,
				}, () => {
					// 這里無需判斷是否loading和開啟動画狀態，因為最终的狀態必須达到opacity: 1，否则可能
					// 會停留在opacity: 0.5的狀態中
					animation.transition(skeleton, {
						styles: {
							opacity: 1
						},
						duration: 600,
					}, () => {
						// 只有在loading中時，才執行動画
						this.loading && this.animate && this.setNvueAnimation()
					})
				})
				// #endif
			},
			// 獲取组件的寬度
			async getComponentWidth() {
				// 延時一定時間，以獲取dom尺寸
				await sleep(20)
				// #ifndef APP-NVUE
				this.$uGetRect('.u-skeleton__wrapper__content').then(size => {
					this.width = size.width
				})
				// #endif

				// #ifdef APP-NVUE
				const ref = this.$refs['u-skeleton__wrapper__content']
				ref && dom.getComponentRect(ref, (res) => {
					this.width = res.size.width
				})
				// #endif
			}
		}
	}
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	@mixin background {
		/* #ifdef APP-NVUE */
		background-color: #F1F2F4;
		/* #endif */
		/* #ifndef APP-NVUE */
		background: linear-gradient(90deg, #F1F2F4 25%, #e6e6e6 37%, #F1F2F4 50%);
		background-size: 400% 100%;
		/* #endif */
	}

	.u-skeleton {
		flex: 1;
		
		&__wrapper {
			@include flex(row);
			
			&__avatar {
				@include background;
				margin-right: 15px;
			
				&--circle {
					border-radius: 100px;
				}
			
				&--square {
					border-radius: 4px;
				}
			}
			
			&__content {
				flex: 1;
			
				&__rows,
				&__title {
					@include background;
					border-radius: 3px;
				}
			}
		}
	}

	/* #ifndef APP-NVUE */
	.animate {
		animation: skeleton 1.8s ease infinite
	}

	@keyframes skeleton {
		0% {
			background-position: 100% 50%
		}

		100% {
			background-position: 0 50%
		}
	}

	/* #endif */
</style>
