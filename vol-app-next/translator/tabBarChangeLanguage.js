//底部导航文字切換
export default function() {
	if (!this.useLang || !this.$ts) {
		return;
	}
	uni.setTabBarItem({
		index: 0,
		text: this.$ts("首頁")
	})
	uni.setTabBarItem({
		index: 1,
		text: this.$ts("菜單")
	})
	uni.setTabBarItem({
		index: 2,
		text: this.$ts("審批流程")
	})
	uni.setTabBarItem({
		index: 3,
		text: this.$ts("消息")
	})
	uni.setTabBarItem({
		index: 4,
		text: this.$ts("我的")
	})
}
