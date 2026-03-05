export default {
    methods: {
        // 關闭时执行
        closeHandler() {
            this.status = 'close'
        },
        setState(status) {
            this.status = status
        },
        closeOther() {
            // 尝试關闭其他打开的單元格
            this.parent && this.parent.closeOther(this)
        }
    }
}
