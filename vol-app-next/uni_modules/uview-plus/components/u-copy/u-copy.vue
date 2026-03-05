<template>
	<view @click="handleClick">
        <slot>複制</slot>
    </view>
</template>
<script>
export default {
    name: "up-copy",
    props: {
        content: {
            type: String,
            default: ''
        },
		alertStyle: {
			type: String,
			default: 'toast'
		},
		notice: {
			type: String,
			default: '複制成功'
		}
    },
	emits: ['success'],
    methods: {
        handleClick() {
            let content = this.content;
			if (!content) {
				uni.showToast({
				    title: '暂無',
				    icon: 'none',
				    duration: 2000,
				});
				return false;
			}
            content = typeof content === 'string' ? content : content.toString() // 複制内容，必須字符串，數字需要轉換為字符串
            /**
			* 小程序端 和 app端的複制逻輯
			*/
			let that = this;
            uni.setClipboardData({
                data: content,
                success: function() {
					if (that.alertStyle == 'modal') {
						uni.showModal({
							title: '提示',
							content: that.notice
						});
					} else {
						uni.showToast({
						    title: that.notice,
						    icon: 'none'
						});
					}
					that.$emit('success');
                },
                fail:function(){
                    uni.showToast({
                        title: '複制失败',
                        icon: 'none',
                        duration:3000,
                    });
                }
            });
        }
    }
}
</script>

<style lang="scss" scoped>
</style>
