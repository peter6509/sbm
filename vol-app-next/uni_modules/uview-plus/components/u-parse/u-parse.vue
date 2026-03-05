<template>
	<view id="_root" :class="(selectable ? '_select ' : '') + '_root'" :style="containerStyle">
		<slot v-if="!nodes[0]" />
		<!-- #ifndef APP-PLUS-NVUE -->
		<node v-else :childs="nodes" :opts="[lazyLoad, loadingImg, errorImg, showImgMenu, selectable]" name="span" />
		<!-- #endif -->
		<!-- #ifdef APP-PLUS-NVUE -->
		<web-view ref="web" src="/static/app-plus/mp-html/local.html" :style="'margin-top:-2px;height:' + height + 'px'" @onPostMessage="_onMessage" />
		<!-- #endif -->
	</view>
</template>

<script>
/**
 * mp-html v2.4.1
 * @description 富文本组件
 * @tutorial https://github.com/jin-yufeng/mp-html
 * @property {String} container-style 容器的樣式
 * @property {String} content 用于渲染的 html 字符串
 * @property {Boolean} copy-link 是否允许外部链接被點擊時自動複制
 * @property {String} domain 主域名，用于拼接链接
 * @property {String} error-img 圖片出错時的占位圖链接
 * @property {Boolean} lazy-load 是否開啟圖片懒加載
 * @property {string} loading-img 圖片加載過程中的占位圖链接
 * @property {Boolean} pause-video 是否在播放一個视频時自動暂停其他视频
 * @property {Boolean} preview-img 是否允许圖片被點擊時自動預覽
 * @property {Boolean} scroll-table 是否给每個表格添加一個滾動層使其能單独横向滾動
 * @property {Boolean | String} selectable 是否開啟長按複制
 * @property {Boolean} set-title 是否將 title 標簽的内容設置到頁面標題
 * @property {Boolean} show-img-menu 是否允许圖片被長按時顯示菜單
 * @property {Object} tag-style 標簽的默認樣式
 * @property {Boolean | Number} use-anchor 是否使用锚點链接
 * @event {Function} load dom 结构加載完毕時触發
 * @event {Function} ready 所有圖片加載完毕時触發
 * @event {Function} imgTap 圖片被點擊時触發
 * @event {Function} linkTap 链接被點擊時触發
 * @event {Function} play 音视频播放時触發
 * @event {Function} error 媒體加載出错時触發
 */
// #ifndef APP-PLUS-NVUE
import node from './node/node'
// #endif
import Parser from './parser'
const plugins = []
// #ifdef APP-PLUS-NVUE
const dom = weex.requireModule('dom')
// #endif
export default {
	name: 'u-parse',
	data() {
		return {
			nodes: [],
			// #ifdef APP-PLUS-NVUE
			height: 3
			// #endif
		}
	},
	props: {
		containerStyle: {
			type: String,
			default: ''
		},
		content: {
			type: String,
			default: ''
		},
		copyLink: {
			type: [Boolean, String],
			default: true
		},
		domain: String,
		errorImg: {
			type: String,
			default: ''
		},
		lazyLoad: {
			type: [Boolean, String],
			default: false
		},
		loadingImg: {
			type: String,
			default: ''
		},
		pauseVideo: {
			type: [Boolean, String],
			default: true
		},
		previewImg: {
			type: [Boolean, String],
			default: true
		},
		scrollTable: [Boolean, String],
		selectable: [Boolean, String],
		setTitle: {
			type: [Boolean, String],
			default: true
		},
		showImgMenu: {
			type: [Boolean, String],
			default: true
		},
		tagStyle: Object,
		useAnchor: [Boolean, Number]
	},
	// #ifdef VUE3
	emits: ['load', 'ready', 'imgTap', 'linkTap', 'play', 'error'],
	// #endif
	// #ifndef APP-PLUS-NVUE
	components: {
		node
	},
	// #endif
	watch: {
		content(content) {
			this.setContent(content)
		}
	},
	created() {
		this.plugins = []
		for (let i = plugins.length; i--;) {
			this.plugins.push(new plugins[i](this))
		}
	},
	mounted() {
		if (this.content && !this.nodes.length) {
			this.setContent(this.content)
		}
	},
	beforeUnmount() {
		this._hook('onDetached')
	},
	methods: {
		/**
		 * @description 將锚點跳轉的範圍限定在一個 scroll-view 内
		 * @param {Object} page scroll-view 所在頁面的示例
		 * @param {String} selector scroll-view 的選擇器
		 * @param {String} scrollTop scroll-view scroll-top 屬性绑定的變量名
		 */
		in(page, selector, scrollTop) {
			// #ifndef APP-PLUS-NVUE
			if (page && selector && scrollTop) {
				this._in = {
					page,
					selector,
					scrollTop
				}
			}
			// #endif
		},

		/**
		 * @description 锚點跳轉
		 * @param {String} id 要跳轉的锚點 id
		 * @param {Number} offset 跳轉位置的偏移量
		 * @returns {Promise}
		 */
		navigateTo(id, offset) {
			return new Promise((resolve, reject) => {
				if (!this.useAnchor) {
					reject(Error('Anchor is disabled'))
					return
				}
				offset = offset || parseInt(this.useAnchor) || 0
				// #ifdef APP-PLUS-NVUE
				if (!id) {
					dom.scrollToElement(this.$refs.web, {
						offset
					})
					resolve()
				} else {
					this._navigateTo = {
						resolve,
						reject,
						offset
					}
					this.$refs.web.evalJs('uni.postMessage({data:{action:"getOffset",offset:(document.getElementById(' + id + ')||{}).offsetTop}})')
				}
				// #endif
				// #ifndef APP-PLUS-NVUE
				let deep = ' '
				// #ifdef MP-WEIXIN || MP-QQ || MP-TOUTIAO
				deep = '>>>'
				// #endif
				const selector = uni.createSelectorQuery()
					// #ifndef MP-ALIPAY
					.in(this._in ? this._in.page : this)
					// #endif
					.select((this._in ? this._in.selector : '._root') + (id ? `${deep}#${id}` : '')).boundingClientRect()
				if (this._in) {
					selector.select(this._in.selector).scrollOffset()
						.select(this._in.selector).boundingClientRect()
				} else {
					// 獲取 scroll-view 的位置和滾動距离
					selector.selectViewport().scrollOffset() // 獲取窗口的滾動距离
				}
				selector.exec(res => {
					if (!res[0]) {
						reject(Error('Label not found'))
						return
					}
					const scrollTop = res[1].scrollTop + res[0].top - (res[2] ? res[2].top : 0) + offset
					if (this._in) {
						// scroll-view 跳轉
						this._in.page[this._in.scrollTop] = scrollTop
					} else {
						// 頁面跳轉
						uni.pageScrollTo({
							scrollTop,
							duration: 300
						})
					}
					resolve()
				})
				// #endif
			})
		},

		/**
		 * @description 獲取文本内容
		 * @return {String}
		 */
		getText(nodes) {
			let text = '';
			(function traversal(nodes) {
				for (let i = 0; i < nodes.length; i++) {
					const node = nodes[i]
					if (node.type === 'text') {
						text += node.text.replace(/&amp;/g, '&')
					} else if (node.name === 'br') {
						text += '\n'
					} else {
						// 块級標簽前后加換行
						const isBlock = node.name === 'p' || node.name === 'div' || node.name === 'tr' || node.name === 'li' || (node.name[0] === 'h' && node.name[1] > '0' && node.name[1] < '7')
						if (isBlock && text && text[text.length - 1] !== '\n') {
							text += '\n'
						}
						// 递归獲取子節點的文本
						if (node.children) {
							traversal(node.children)
						}
						if (isBlock && text[text.length - 1] !== '\n') {
							text += '\n'
						} else if (node.name === 'td' || node.name === 'th') {
							text += '\t'
						}
					}
				}
			})(nodes || this.nodes)
			return text
		},

		/**
		 * @description 獲取内容大小和位置
		 * @return {Promise}
		 */
		getRect() {
			return new Promise((resolve, reject) => {
				uni.createSelectorQuery()
					// #ifndef MP-ALIPAY
					.in(this)
					// #endif
					.select('#_root').boundingClientRect().exec(res => res[0] ? resolve(res[0]) : reject(Error('Root label not found')))
			})
		},

		/**
		 * @description 暂停播放媒體
		 */
		pauseMedia() {
			for (let i = (this._videos || []).length; i--;) {
				this._videos[i].pause()
			}
			// #ifdef APP-PLUS
			const command = 'for(var e=document.getElementsByTagName("video"),i=e.length;i--;)e[i].pause()'
			// #ifndef APP-PLUS-NVUE
			let page = this.$parent
			while (!page.$scope) page = page.$parent
			page.$scope.$getAppWebview().evalJS(command)
			// #endif
			// #ifdef APP-PLUS-NVUE
			this.$refs.web.evalJs(command)
			// #endif
			// #endif
		},

		/**
		 * @description 設置媒體播放速率
		 * @param {Number} rate 播放速率
		 */
		setPlaybackRate(rate) {
			this.playbackRate = rate
			for (let i = (this._videos || []).length; i--;) {
				this._videos[i].playbackRate(rate)
			}
			// #ifdef APP-PLUS
			const command = 'for(var e=document.getElementsByTagName("video"),i=e.length;i--;)e[i].playbackRate=' + rate
			// #ifndef APP-PLUS-NVUE
			let page = this.$parent
			while (!page.$scope) page = page.$parent
			page.$scope.$getAppWebview().evalJS(command)
			// #endif
			// #ifdef APP-PLUS-NVUE
			this.$refs.web.evalJs(command)
			// #endif
			// #endif
		},

		/**
		 * @description 設置内容
		 * @param {String} content html 内容
		 * @param {Boolean} append 是否在尾部追加
		 */
		setContent(content, append) {
			if (!append || !this.imgList) {
				this.imgList = []
			}
			const nodes = new Parser(this).parse(content)
			// #ifdef APP-PLUS-NVUE
			if (this._ready) {
				this._set(nodes, append)
			}
			// #endif
			this.$set(this, 'nodes', append ? (this.nodes || []).concat(nodes) : nodes)

			// #ifndef APP-PLUS-NVUE
			this._videos = []
			this.$nextTick(() => {
				this._hook('onLoad')
				this.$emit('load')
			})

			if (this.lazyLoad || this.imgList._unloadimgs < this.imgList.length / 2) {
				// 設置懒加載，每 350ms 獲取高度，不變则認為加載完毕
				let height = 0
				const callback = rect => {
					if (!rect || !rect.height) rect = {}
					// 350ms 總高度無變化就触發 ready 事件
					if (rect.height === height) {
						this.$emit('ready', rect)
					} else {
						height = rect.height
						setTimeout(() => {
							this.getRect().then(callback).catch(callback)
						}, 350)
					}
				}
				this.getRect().then(callback).catch(callback)
			} else {
				// 未設置懒加載，等待所有圖片加載完毕
				if (!this.imgList._unloadimgs) {
					this.getRect().then(rect => {
						this.$emit('ready', rect)
					}).catch(() => {
						this.$emit('ready', {})
					})
				}
			}
			// #endif
		},

		/**
		 * @description 調用插件钩子函數
		 */
		_hook(name) {
			for (let i = plugins.length; i--;) {
				if (this.plugins[i][name]) {
					this.plugins[i][name]()
				}
			}
		},

		// #ifdef APP-PLUS-NVUE
		/**
		 * @description 設置内容
		 */
		_set(nodes, append) {
			this.$refs.web.evalJs('setContent(' + JSON.stringify(nodes).replace(/%22/g, '') + ',' + JSON.stringify([this.containerStyle.replace(/(?:margin|padding)[^;]+/g, ''), this.errorImg, this.loadingImg, this.pauseVideo, this.scrollTable, this.selectable]) + ',' + append + ')')
		},

		/**
		 * @description 接收到 web-view 消息
		 */
		_onMessage(e) {
			const message = e.detail.data[0]
			switch (message.action) {
				// web-view 初始化完毕
				case 'onJSBridgeReady':
					this._ready = true
					if (this.nodes) {
						this._set(this.nodes)
					}
					break
				// 内容 dom 加載完毕
				case 'onLoad':
					this.height = message.height
					this._hook('onLoad')
					this.$emit('load')
					break
				// 所有圖片加載完毕
				case 'onReady':
					this.getRect().then(res => {
						this.$emit('ready', res)
					}).catch(() => {
						this.$emit('ready', {})
					})
					break
				// 總高度發生變化
				case 'onHeightChange':
					this.height = message.height
					break
				// 圖片點擊
				case 'onImgTap':
					this.$emit('imgTap', message.attrs)
					if (this.previewImg) {
						uni.previewImage({
							current: parseInt(message.attrs.i),
							urls: this.imgList
						})
					}
					break
				// 链接點擊
				case 'onLinkTap': {
					const href = message.attrs.href
					this.$emit('linkTap', message.attrs)
					if (href) {
						// 锚點跳轉
						if (href[0] === '#') {
							if (this.useAnchor) {
								dom.scrollToElement(this.$refs.web, {
									offset: message.offset
								})
							}
						} else if (href.includes('://')) {
							// 打開外链
							if (this.copyLink) {
								plus.runtime.openWeb(href)
							}
						} else {
							uni.navigateTo({
								url: href,
								fail() {
									uni.switchTab({
										url: href
									})
								}
							})
						}
					}
					break
				}
				case 'onPlay':
					this.$emit('play')
					break
				// 獲取到锚點的偏移量
				case 'getOffset':
					if (typeof message.offset === 'number') {
						dom.scrollToElement(this.$refs.web, {
							offset: message.offset + this._navigateTo.offset
						})
						this._navigateTo.resolve()
					} else {
						this._navigateTo.reject(Error('Label not found'))
					}
					break
				// 點擊
				case 'onClick':
					this.$emit('tap')
					this.$emit('click')
					break
				// 出错
				case 'onError':
					this.$emit('error', {
						source: message.source,
						attrs: message.attrs
					})
			}
		}
		// #endif
	}
}
</script>

<style>
/* #ifndef APP-PLUS-NVUE */
/* 根節點樣式 */
._root {
	padding: 1px 0;
	overflow-x: auto;
	overflow-y: hidden;
	-webkit-overflow-scrolling: touch;
}

/* 長按複制 */
._select {
	user-select: text;
}

/* #endif */
</style>
