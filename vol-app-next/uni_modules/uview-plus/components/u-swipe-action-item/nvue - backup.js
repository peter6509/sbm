// nvue操作dom的庫，用于获取dom的尺寸信息
const dom = uni.requireNativePlugin('dom')
// nvue中用于操作元素动画的庫，类似于uni.animation，只不过uni.animation不能用于nvue
const animation = uni.requireNativePlugin('animation')
import { sleep } from '../../libs/function/index';
export default {
    data() {
        return {
            // 是否滑动中
            moving: false,
            // 状态，open-打开状态，close-關闭状态
            status: 'close',
            // 开始触摸点的X和Y軸坐標
            startX: 0,
            startY: 0,
            // 所有隐藏按鈕的尺寸信息數组
            buttons: [],
            // 所有按鈕的总寬度
            buttonsWidth: 0,
            // 记录上一次移动的位置值
            moveX: 0,
            // 记录上一次滑动的位置，用于前后两次做对比，如果移动的距离小于某一阈值，则认為前后之间没有移动，為了解决可能存在的通信阻塞问題
            lastX: 0
        }
    },
    computed: {
        // 获取过渡时间
        getDuratin() {
            let duration = String(this.duration)
            // 如果ms為單位，返回ms的數值部分
            if (duration.indexOf('ms') >= 0) return parseInt(duration)
            // 如果s為單位，為了得到ms的數值，需要乘以1000
            if (duration.indexOf('s') >= 0) return parseInt(duration) * 1000
            // 如果值傳了數值，且小于30，认為是s單位
            duration = Number(duration)
            return duration < 30 ? duration * 1000 : duration
        }
    },
    watch: {
        show: {
            immediate: true,
            handler(n) {
                // if(n === true) {
                // 	sleep(50).then(() => {
                // 		this.openSwipeAction()
                // 	})
                // } else {
                // 	this.closeSwipeAction()
                // }
            }
        }
    },
    mounted() {
        sleep(20).then(() => {
            this.queryRect()
        })
    },
    methods: {
        close() {
            this.closeSwipeAction()
        },
        // 触摸單元格
        touchstart(event) {
            if (this.disabled) return
            this.closeOther()
            const { touches } = event
            // 记录触摸开始点的坐標值
            this.startX = touches[0].pageX
            this.startY = touches[0].pageY
        },
        // // 触摸滑动
        touchmove(event) {
            if (this.disabled) return
            const { touches } = event
            const { pageX } = touches[0]
            const { pageY } = touches[0]
            let moveX = pageX - this.startX
            const moveY = pageY - this.startY
            const { buttonsWidth } = this
            const len = this.buttons.length

            // 判断前后两次的移动距离，如果小于一定值，则不进行移动处理
            if (Math.abs(pageX - this.lastX) < 0.3) return
            this.lastX = pageX

            // 移动的X軸距离大于Y軸距离，也即终点與起点位置连线，與X軸夹角小于45度时，禁止页面滾动
            if (Math.abs(moveX) > Math.abs(moveY) || Math.abs(moveX) > this.threshold) {
                event.stopPropagation()
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
                if (moveX > buttonsWidth) moveX = buttonsWidth
                // 如果是已經打开了的状态，向左滑动时，移动收起菜單
                this.moveSwipeAction(-buttonsWidth + moveX)
            } else {
                // 關闭状态下，右滑动需忽略
                if (moveX > 0) moveX = 0
                // 滑动的距离不允许超过所有按鈕的总寬度，此时只能是左滑，最终設置按鈕的总寬度，同时為负數
                if (Math.abs(moveX) > buttonsWidth) moveX = -buttonsWidth
                // 只要是在滑过程中，就不断移动菜單的内容部分，从而使隐藏的菜單显示出来
                this.moveSwipeAction(moveX)
            }
        },
        // 單元格结束触摸
        touchend(event) {
            if (this.disabled) return
            const touches = event.changedTouches ? event.changedTouches[0] : {}
            const { pageX } = touches
            const { pageY } = touches
            const { buttonsWidth } = this
            this.moveX = pageX - this.startX
            if (this.status === 'open') {
                // 在展开的状态下，继续左滑，無需操作
                if (this.moveX < 0) this.moveX = 0
                if (this.moveX > buttonsWidth) this.moveX = buttonsWidth
                // 在开啟状态下，点击一下内容區域，moveX為0，也即没有进行移动，这时执行收起菜單逻輯
                if (this.moveX === 0) {
                    return this.closeSwipeAction()
                }
                // 在开啟状态下，滑动距离小于阈值，则默认為不關闭，同时恢複原来的打开状态
                if (Math.abs(this.moveX) < this.threshold) {
                    this.openSwipeAction()
                } else {
                    // 如果滑动距离大于阈值，则执行收起逻輯
                    this.closeSwipeAction()
                }
            } else {
                // 在關闭的状态下，右滑，無需操作
                if (this.moveX >= 0) this.moveX = 0
                if (this.moveX <= -buttonsWidth) this.moveX = -buttonsWidth
                // 理由同上
                if (Math.abs(this.moveX) < this.threshold) {
                    this.closeSwipeAction()
                } else {
                    this.openSwipeAction()
                }
            }
        },
        // 移动滑动選择器内容區域，同时显示出其隐藏的菜單
        moveSwipeAction(moveX) {
            if (this.moving) return
            this.moving = true

            let previewButtonsMoveX = 0
            const len = this.buttons.length
            animation.transition(this.$refs['u-swipe-action-item__content'].ref, {
                styles: {
                    transform: `translateX(${moveX}px)`
                },
                timingFunction: 'linear'
            }, () => {
                this.moving = false
            })
            // 按鈕的组的長度
            for (let i = len - 1; i >= 0; i--) {
                const buttonRef = this.$refs[`u-swipe-action-item__right__button-${i}`][0].ref
                // 通过比例，得出元素自身该移动的距离
                const translateX = this.buttons[i].width / this.buttonsWidth * moveX
                // 最终移动的距离，是通过自身比例算出的距离，再加上在它之前所有按鈕移动的距离之和
                const realTranslateX = translateX + previewButtonsMoveX
                animation.transition(buttonRef, {
                    styles: {
                        transform: `translateX(${realTranslateX}px)`
                    },
                    duration: 0,
                    delay: 0,
                    timingFunction: 'linear'
                }, () => {})
                // 记录本按鈕之前的所有按鈕的移动距离之和
                previewButtonsMoveX += translateX
            }
        },
        // 關闭菜單
        closeSwipeAction() {
            if (this.status === 'close') return
            this.moving = true
            const { buttonsWidth } = this
            animation.transition(this.$refs['u-swipe-action-item__content'].ref, {
                styles: {
                    transform: 'translateX(0px)'
                },
                duration: this.getDuratin,
                timingFunction: 'ease-in-out'
            }, () => {
                this.status = 'close'
                this.moving = false
                this.closeHandler()
            })
            // 按鈕的组的長度
            const len = this.buttons.length
            for (let i = len - 1; i >= 0; i--) {
                const buttonRef = this.$refs[`u-swipe-action-item__right__button-${i}`][0].ref
                // 如果不满足边界条件，返回
                if (this.buttons.length === 0 || !this.buttons[i] || !this.buttons[i].width) return

                animation.transition(buttonRef, {
                    styles: {
                        transform: 'translateX(0px)'
                    },
                    duration: this.getDuratin,
                    timingFunction: 'ease-in-out'
                }, () => {})
            }
        },
        // 打开菜單
        openSwipeAction() {
            if (this.status === 'open') return
            this.moving = true
            const buttonsWidth = -this.buttonsWidth
            let previewButtonsMoveX = 0
            animation.transition(this.$refs['u-swipe-action-item__content'].ref, {
                styles: {
                    transform: `translateX(${buttonsWidth}px)`
                },
                duration: this.getDuratin,
                timingFunction: 'ease-in-out'
            }, () => {
                this.status = 'open'
                this.moving = false
                this.openHandler()
            })
            // 按鈕的组的長度
            const len = this.buttons.length
            for (let i = len - 1; i >= 0; i--) {
                const buttonRef = this.$refs[`u-swipe-action-item__right__button-${i}`][0].ref
                // 如果不满足边界条件，返回
                if (this.buttons.length === 0 || !this.buttons[i] || !this.buttons[i].width) return
                // 通过比例，得出元素自身该移动的距离
                const translateX = this.buttons[i].width / this.buttonsWidth * buttonsWidth
                // 最终移动的距离，是通过自身比例算出的距离，再加上在它之前所有按鈕移动的距离之和
                const realTranslateX = translateX + previewButtonsMoveX
                animation.transition(buttonRef, {
                    styles: {
                        transform: `translateX(${realTranslateX}px)`
                    },
                    duration: this.getDuratin,
                    timingFunction: 'ease-in-out'
                }, () => {})
                previewButtonsMoveX += translateX
            }
        },
        // 查询按鈕节点信息
        queryRect() {
            // 历遍所有按鈕數组，通过getRectByDom返回一个promise
            const promiseAll = this.rightOptions.map((item, index) => this.getRectByDom(this.$refs[`u-swipe-action-item__right__button-${index}`][0]))
            // 通过promise.all方法，让所有按鈕的查询结果返回一个數组的形式
            Promise.all(promiseAll).then((sizes) => {
                this.buttons = sizes
                // 计算所有按鈕总寬度
                this.buttonsWidth = sizes.reduce((sum, cur) => sum + cur.width, 0)
            })
        },
        // 通过nvue的dom模块，查询节点信息
        getRectByDom(ref) {
            return new Promise((resolve) => {
                dom.getComponentRect(ref, (res) => {
                    resolve(res.size)
                })
            })
        }
    }
}
