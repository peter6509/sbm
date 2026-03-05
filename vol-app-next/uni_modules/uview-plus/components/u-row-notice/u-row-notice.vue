<template>
	<view
		class="u-notice"
		@tap="clickHandler"
	>
		<slot name="icon">
			<view
				class="u-notice__left-icon"
				v-if="icon"
			>
				<u-icon
					:name="icon"
					:color="color"
					size="19"
				></u-icon>
			</view>
		</slot>
		<view
			class="u-notice__content"
			ref="u-notice__content"
		>
			<view
				ref="u-notice__content__text"
				class="u-notice__content__text"
				:style="[animationStyle]"
			>
				<text
					v-for="(item, index) in innerText"
					:key="index"
					:style="[textStyle]"
				>{{item}}</text>
			</view>
		</view>
		<view
			class="u-notice__right-icon"
			v-if="['link', 'closable'].includes(mode)"
		>
			<u-icon
				v-if="mode === 'link'"
				name="arrow-right"
				:size="17"
				:color="color"
			></u-icon>
			<u-icon
				v-if="mode === 'closable'"
				@click="close"
				name="close"
				:size="16"
				:color="color"
			></u-icon>
		</view>
	</view>
</template>
<script>
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, error, sleep, getPx } from '../../libs/function/index';
	import test from '../../libs/function/test';
	// #ifdef APP-NVUE
	const animation = uni.requireNativePlugin('animation')
	const dom = uni.requireNativePlugin('dom')
	// #endif
	/**
	 * RowNotice 滾動通知中的水平滾動模式
	 * @description 水平滾動
	 * @tutorial https://ijry.github.io/uview-plus/components/noticeBar.html
	 * @property {String | Number}	text			顯示的内容，字符串
	 * @property {String}			icon			是否顯示左侧的音量圖標 (默認 'volume' )
	 * @property {String}			mode			通告模式，link-顯示右箭頭，closable-顯示右侧關闭圖標
	 * @property {String}			color			文字顏色，各圖標也會使用文字顏色 (默認 '#f9ae3d' )
	 * @property {String}			bgColor			背景顏色 (默認 ''#fdf6ec' )
	 * @property {String | Number}	fontSize		字體大小，單位px (默認 14 )
	 * @property {String | Number}	speed			水平滾動時的滾動速度，即每秒滾動多少px(rpx)，這有利于控制文字無论多少時，都能有一個恒定的速度  (默認 80 )
	 * 
	 * @event {Function} click 點擊通告文字触發
	 * @event {Function} close 點擊右侧關闭圖標触發
	 * @example 
	 */
	export default {
		name: 'u-row-notice',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				animationDuration: '0', // 動画執行時間
				animationPlayState: 'paused', // 動画的開始和结束執行
				// nvue下，内容發生變化，导致滾動寬度也變化，需要標志為是否需要重新計算寬度
				// 不能在内容變化時直接重新計算，因為nvue的animation模块上一次的滾動不是刚好结束，會有影响
				nvueInit: true,
				show: true
			};
		},
		watch: {
			text: {
				immediate: true,
				handler(newValue, oldValue) {
					// #ifdef APP-NVUE
					this.nvueInit = true
					// #endif
					// #ifndef APP-NVUE
					this.vue()
					// #endif
					
					if(!test.string(newValue)) {
						error('noticebar组件direction為row時，要求text参數為字符串形式')
					}
				}
			},
			fontSize() {
				// #ifdef APP-NVUE
				this.nvueInit = true
				// #endif
				// #ifndef APP-NVUE
				this.vue()
				// #endif
			},
			speed() {
				// #ifdef APP-NVUE
				this.nvueInit = true
				// #endif
				// #ifndef APP-NVUE
				this.vue()
				// #endif
			}
		},
		computed: {
			// 文字内容的樣式
			textStyle() {
				let style = {}
				style.whiteSpace = 'nowrap !important'
				style.color = this.color
				style.fontSize = addUnit(this.fontSize)
				return style
			},
			animationStyle() {
				let style = {}
				style.animationDuration = this.animationDuration
				style.animationPlayState = this.animationPlayState
				return style
			},
			// 内部對用户傳入的數據进一步分割，放到多個text標簽循环，否则如果用户傳入的字符串很長（100個字符以上）
			// 放在一個text標簽中进行滾動，在低端安卓機上，動画可能會出現抖動現象，需要分割到多個text中可解决此問題
			innerText() {
				let result = [],
					// 每组text標簽的字符長度
					len = 20
				const textArr = this.text.split('')
				for (let i = 0; i < textArr.length; i += len) {
					// 對拆分的后的text进行slice分割，得到的為數组再进行join拼接為字符串
					result.push(textArr.slice(i, i + len).join(''))
				}
				return result
			}
		},
		mounted() {
			// #ifdef APP-PLUS
			// 在APP上(含nvue)，監聽當前webview是否處于隐藏狀態(进入下一頁時即為hide狀態)
			// 如果webivew隐藏了，為了节省性能的损耗，應停止動画的執行，同時也是為了保持进入下一頁返回后，滾動位置保持不變
			var pages = getCurrentPages()
			var page = pages[pages.length - 1]
			var currentWebview = page.$getAppWebview()
			currentWebview.addEventListener('hide', () => {
				this.webviewHide = true
			})
			currentWebview.addEventListener('show', () => {
				this.webviewHide = false
			})
			// #endif

			this.init()
		},
		emits: ["click", "close"],
		methods: {
			init() {
				// #ifdef APP-NVUE
				this.nvue()
				// #endif

				// #ifndef APP-NVUE
				this.vue()
				// #endif
				
				if(!test.string(this.text)) {
					error('noticebar组件direction為row時，要求text参數為字符串形式')
				}
			},
			// vue版處理
			async vue() {
				// #ifndef APP-NVUE
				let boxWidth = 0,
					textWidth = 0
				// 进行一定的延時
				await sleep()
				// 查詢盒子和文字的寬度
				textWidth = (await this.$uGetRect('.u-notice__content__text')).width
				boxWidth = (await this.$uGetRect('.u-notice__content')).width
				// 根據t=s/v(時間=路程/速度)，這里為何不需要加上#u-notice-box的寬度，因為中設置了.u-notice-content樣式中設置了padding-left: 100%
				// 恰巧計算出来的结果中已經包含了#u-notice-box的寬度
				this.animationDuration = `${textWidth / getPx(this.speed)}s`
				// 這里必須這樣開始動画，否则在APP上動画速度不會改變
				this.animationPlayState = 'paused'
				setTimeout(() => {
					this.animationPlayState = 'running'
				}, 10)
				// #endif
			},
			// nvue版處理
			async nvue() {
				// #ifdef APP-NVUE
				this.nvueInit = false
				let boxWidth = 0,
					textWidth = 0
				// 进行一定的延時
				await sleep()
				// 查詢盒子和文字的寬度
				textWidth = (await this.getNvueRect('u-notice__content__text')).width
				boxWidth = (await this.getNvueRect('u-notice__content')).width
				// 將文字移動到盒子的右邊沿，之所以需要這么做，是因為nvue不支持100%單位，否则可以通過css設置
				animation.transition(this.$refs['u-notice__content__text'], {
					styles: {
						transform: `translateX(${boxWidth}px)`
					},
				}, () => {
					// 如果非禁止動画，则開始滾動
					!this.stopAnimation && this.loopAnimation(textWidth, boxWidth)
				});
				// #endif
			},
			loopAnimation(textWidth, boxWidth) {
				// #ifdef APP-NVUE
				animation.transition(this.$refs['u-notice__content__text'], {
					styles: {
						// 目標移動终點為-textWidth，也即當文字的最右邊貼到盒子的左邊框的位置
						transform: `translateX(-${textWidth}px)`
					},
					// 滾動時間的計算為，時間 = 路程(boxWidth + textWidth) / 速度，最后轉為毫秒
					duration: (boxWidth + textWidth) / getPx(this.speed) * 1000,
					delay: 10
				}, () => {
					animation.transition(this.$refs['u-notice__content__text'], {
						styles: {
							// 重新將文字移動到盒子的右邊沿
							transform: `translateX(${this.stopAnimation ? 0 : boxWidth}px)`
						},
					}, () => {
						// 如果非禁止動画，则继续下一轮滾動
						if (!this.stopAnimation) {
							// 判斷是否需要初始化計算尺寸
							if (this.nvueInit) {
								this.nvue()
							} else {
								this.loopAnimation(textWidth, boxWidth)
							}
						}
					});
				})
				// #endif
			},
			getNvueRect(el) {
				// #ifdef APP-NVUE
				// 返回一個promise
				return new Promise(resolve => {
					dom.getComponentRect(this.$refs[el], (res) => {
						resolve(res.size)
					})
				})
				// #endif
			},
			// 點擊通告栏
			clickHandler(index) {
				this.$emit('click')
			},
			// 點擊右侧按鈕，需要判斷點擊的是關闭圖標還是箭頭圖標
			close() {
				this.$emit('close')
			}
		},
		beforeUnmount() {
			this.stopAnimation = true
		}
	};
</script>

<style lang="scss" scoped>
	@import "../../libs/css/components.scss";

	.u-notice {
		@include flex;
		align-items: center;
		justify-content: space-between;

		&__left-icon {
			align-items: center;
			margin-right: 5px;
		}

		&__right-icon {
			margin-left: 5px;
			align-items: center;
		}

		&__content {
			text-align: right;
			flex: 1;
			@include flex;
			flex-wrap: nowrap;
			overflow: hidden;

			&__text {
				font-size: 14px;
				color: $u-warning;
				/* #ifndef APP-NVUE */
				// 這一句很重要，為了能让滾動左右连接起来
				padding-left: 100%;
				word-break: keep-all;
				white-space: nowrap;
				animation: u-loop-animation 10s linear infinite both;
				/* #endif */
				@include flex(row);
				line-height: 100%;
			}
		}

	}

	/* #ifndef APP-NVUE */
	@keyframes u-loop-animation {
		0% {
			transform: translate3d(0, 0, 0);
		}
	
		100% {
			transform: translate3d(-100%, 0, 0);
		}
	}
	/* #endif */
</style>
