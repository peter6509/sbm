<template>
	<view class="u-read-more">
		<view
		    class="u-read-more__content"
		    :style="{
				height: isLongContent && status === 'close' ? addUnit(showHeight) : addUnit(contentHeight),
				textIndent: textIndent
			}"
		>
			<view
			    class="u-read-more__content__inner"
			    ref="u-read-more__content__inner"
			    :class="[elId]"
			>
				<slot></slot>
			</view>
		</view>
		<view
		    class="u-read-more__toggle"
		    :style="[innerShadowStyle]"
		    v-if="isLongContent"
		>
			<slot name="toggle">
				<view
				    class="u-read-more__toggle__text"
				    @tap="toggleReadMore"
				>
					<up-text
					    :text="status === 'close' ? closeText : openText"
					    :color="color"
					    :size="fontSize"
					    :lineHeight="fontSize"
					    margin="0 5px 0 0"
					></up-text>
					<view class="u-read-more__toggle__icon">
						<u-icon
						    :color="color"
						    :size="fontSize + 2"
						    :name="status === 'close' ? 'arrow-down' : 'arrow-up'"
						></u-icon>
					</view>
				</view>
			</slot>
		</view>
	</view>
</template>

<script>
	// #ifdef APP-NVUE
	const dom = uni.requireNativePlugin('dom')
	// #endif
	import { props } from './props';
	import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
	import { addUnit, guid, getPx, sleep } from '../../libs/function/index';
	/**
	 * readMore 阅讀更多
	 * @description 該组件一般用于内容较長，預先收起一部分，點擊展開全部内容的场景。
	 * @tutorial https://ijry.github.io/uview-plus/components/readMore.html
	 * @property {String | Number}	showHeight	内容超出此高度才會顯示展開全文按鈕，單位px（默認 400 ）
	 * @property {Boolean}			toggle		展開后是否顯示收起按鈕（默認 false ）
	 * @property {String}			closeText	關闭時的提示文字（默認 '展開阅讀全文' ）
	 * @property {String}			openText	展開時的提示文字（默認 '收起' ）
	 * @property {String}			color		提示文字的顏色（默認 '#2979ff' ）
	 * @property {String | Number}	fontSize	提示文字的大小，單位px （默認 14 ）
	 * @property {Object}			shadowStyle	顯示阴影的樣式
	 * @property {String}			textIndent	段落首行缩进的字符個數 （默認 '2em' ）
	 * @property {String | Number}	name		用于在 open 和 close 事件中當作回調参數返回
	 * @event {Function} open 内容被展開時触發
	 * @event {Function} close 内容被收起時触發
	 * @example <u-read-more><rich-text :nodes="content"></rich-text></u-read-more>
	 */
	export default {
		name: 'u-read-more',
		mixins: [mpMixin, mixin, props],
		data() {
			return {
				isLongContent: false, // 是否需要隐藏一部分内容
				status: 'close', // 當前隐藏與顯示的狀態，close-收起狀態，open-展開狀態
				elId: guid(), // 生成唯一class
				contentHeight: 100, // 内容高度
			}
		},
		computed: {
			// 展開后無需阴影，收起時才需要阴影樣式
			innerShadowStyle() {
				if (this.status === 'open') return {}
				else return this.shadowStyle
			}
		},
		mounted() {
			this.init()
		},
		emits: ["open", "close"],
		methods: {
			addUnit,
			async init() {
				this.getContentHeight().then(height => {
					this.contentHeight = height
					// 判斷高度，如果真實内容高度大于占位高度，则顯示收起與展開的控制按鈕
					if (height > getPx(this.showHeight)) {
						this.isLongContent = true
						this.status = 'close'
					} else {
						// https://github.com/ijry/uview-plus/issues/270
						this.isLongContent = false
						this.status = 'close'
					}
				})
			},
			// 獲取内容的高度
			async getContentHeight() {
				// 延時一定時間再獲取節點
				await sleep(30)
				return new Promise(resolve => {
					// #ifndef APP-NVUE
					this.$uGetRect('.' + this.elId).then(res => {
						resolve(res.height)
					})
					// #endif

					// #ifdef APP-NVUE
					const ref = this.$refs['u-read-more__content__inner']
					dom.getComponentRect(ref, (res) => {
						resolve(res.size.height)
					})
					// #endif
				})
			},
			// 展開或者收起
			toggleReadMore() {
				this.status = this.status === 'close' ? 'open' : 'close'
				// 如果toggle為false，隐藏"收起"部分的内容
				if (this.toggle == false) this.isLongContent = false
				// 發出打開或者收齊的事件
				this.$emit(this.status, this.name)
			}
		}
	}
</script>

<style lang="scss" scoped>
@import "../../libs/css/components.scss";

.u-read-more {

	&__content {
		overflow: hidden;
		color: $u-content-color;
		font-size: 15px;
		text-align: left;
	}

	&__toggle {
		@include flex;
		justify-content: center;
		position: relative;

		&__text {
			@include flex;
			align-items: center;
			justify-content: center;
			margin-top: 5px;
		}
	}
}
</style>
