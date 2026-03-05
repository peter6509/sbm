export default {
	data() {
		return {
			state: {
				moving: false,
				startX: 0,
				startY: 0,
				buttonsWidth: 0
			}
		}
	},
	watch: {
		status(newValue) {
			if (this.disabled) return
			// 打开或關闭單元格
			if (newValue === 'close' && this.status === 'open') {
				this.closeSwipeAction()
			} else if(newValue === 'open' && this.status === 'close') {
				this.openSwipeAction()
			}
		},
		options(newVal) {
			this.getBtnWidth()
		}
	},
	mounted() {
		this.getBtnWidth()
	},
    methods: {
		clickHandler() {
		},
		closeHandler() {
			this.closeSwipeAction()
        },
		setStatus(status) {
			this.status = status
		},
		getBtnWidth() {
			let view = uni.createSelectorQuery().in(this).select(".u-swipe-action-item__right");
			view.fields({
				size: true,
				scrollOffset: true
				}, data => {
					this.state.buttonsWidth = data.width
					// console.log("得到节点信息" + JSON.stringify(data));
			}).exec();
		},
        // 开始触摸
        touchstart(event) {
			// console.log(event)
			// 標识当前為滑动中状态
			this.state.moving = true
			// 记录触摸开始点的坐標值
			var touches = event.touches
			this.state.startX = touches[0].pageX
			this.state.startY = touches[0].pageY
			
			// 關闭其它
			// console.log(this.parent)
			this.parent && this.parent.closeOther(this)
        },
        touchmove(event) {
            // console.log(event)
			if (this.disabled || !this.state.moving) return
			var touches = event.touches
			var pageX = touches[0].pageX
			var pageY = touches[0].pageY
			var moveX = pageX - this.state.startX
			var moveY = pageY - this.state.startY

			// 移动的X軸距离大于Y軸距离，也即终点與起点位置连线，與X軸夹角小于45度时，禁止页面滾动
			if (Math.abs(moveX) > Math.abs(moveY) || Math.abs(moveX) > this.threshold) {
				event.preventDefault && event.preventDefault()
				event.stopPropagation && event.stopPropagation()
			}
			// 如果移动的X軸距离小于Y軸距离，也即终点位置與起点位置连线，與Y軸夹角小于45度时，认為是页面上下滑动，而不是左右滑动單元格
			if (Math.abs(moveX) < Math.abs(moveY)) return

			// 限制右滑的距离，不允许内容部分往右偏移，右滑会导致X軸偏移值大于0，以此做判断
			// 此处不能直接return，因為滑动过程中会缺失某些關鍵点坐標，会导致错乱，最好的办法就是
			// 在超出后，設置為0
			if (this.status === 'open') {
				// 在开啟状态下，向左滑动，需忽略
				if (moveX < 0) moveX = 0
				// 想要收起菜單，最大能移动的距离為按鈕的总寬度
				if (moveX > this.state.buttonsWidth) moveX = this.state.buttonsWidth
				// 如果是已經打开了的状态，向左滑动时，移动收起菜單
				this.moveSwipeAction(-this.state.buttonsWidth + moveX)
			} else {
				// 關闭状态下，右滑动需忽略
				if (moveX > 0) moveX = 0
				// 滑动的距离不允许超过所有按鈕的总寬度，此时只能是左滑，最终設置按鈕的总寬度，同时為负數
				if (Math.abs(moveX) > this.state.buttonsWidth) moveX = -this.state.buttonsWidth
				// 只要是在滑过程中，就不断移动單元格内容部分，从而使隐藏的菜單显示出来
				this.moveSwipeAction(moveX)
			}
        },
        touchend(event) {
            // console.log(event)
			if (!this.state.moving || this.disabled) return
			this.state.moving = false
			var touches = event.changedTouches ? event.changedTouches[0] : {}
			var pageX = touches.pageX
			var pageY = touches.pageY
			var moveX = pageX - this.state.startX
			if (this.status === 'open') {
				// 在展开的状态下，继续左滑，無需操作
				if (moveX < 0) return
				// 在开啟状态下，点击一下内容區域，moveX為0，也即没有进行移动，这时执行收起菜單逻輯
				if (moveX === 0) {
					return this.closeSwipeAction()
				}
				// 在开啟状态下，滑动距离小于阈值，则默认為不關闭，同时恢複原来的打开状态
				if (Math.abs(moveX) < this.threshold) {
					this.openSwipeAction()
				} else {
					// 如果滑动距离大于阈值，则执行收起逻輯
					this.closeSwipeAction()
				}
			} else {
				// 在關闭的状态下，右滑，無需操作
				if (moveX > 0) return
				// 理由同上
				if (Math.abs(moveX) < this.threshold) {
					this.closeSwipeAction()
				} else {
					this.openSwipeAction()
				}
			}
        },
		// 一次性展开滑动菜單
		openSwipeAction() {
			// 处理duration單位问題
			var duration = this.getDuration(this.duration)
			// 展开过程中，是向左移动，所以X的偏移应该為负值
			var buttonsWidth = -this.state.buttonsWidth
			this.sliderStyle = {
				'transition': 'transform ' + duration,
				'transform': 'translateX(' + buttonsWidth + 'px)',
				'-webkit-transform': 'translateX(' + buttonsWidth + 'px)',
			}
			this.setStatus('open')
		},
		// 一次性收起滑动菜單
		closeSwipeAction() {
			// 处理duration單位问題
			var duration = this.getDuration(this.duration)
			this.sliderStyle = {
				'transition': 'transform ' + duration,
				'transform': 'translateX(0px)',
				'-webkit-transform': 'translateX(0px)'
			}
			// 設置各个隐藏的按鈕為收起的状态
			// for (var i = this.state.buttonsWidth - 1; i >= 0; i--) {
			// 	buttons[i].setStyle({
			// 		'transition': 'transform ' + duration,
			// 		'transform': 'translateX(0px)',
			// 		'-webkit-transform': 'translateX(0px)'
			// 	})
			// }
			this.setStatus('close')
		},
		// 移动滑动選择器内容區域，同时显示出其隐藏的菜單
		moveSwipeAction(moveX) {
			// 設置菜單内容部分的偏移
			this.sliderStyle = {
				'transition': 'none',
				transform: 'translateX(' + moveX + 'px)',
				'-webkit-transform': 'translateX(' + moveX + 'px)'
			}
		},
		// 获取过渡时间
		getDuration(value) {
			if (value.toString().indexOf('s') >= 0) return value
			return value > 30 ? value + 'ms' : value + 's'
		}
    }
}
