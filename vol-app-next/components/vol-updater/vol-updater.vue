<template>
	<view>
	</view>
</template>

<script>
	export default {
		props: {
			home: {
				type: Boolean,
				default: false
			}
		},
		name: "vol-updater",
		data() {
			return {

			};
		},
		methods: {
			download(url) {
				var dtask = plus.downloader.createDownload(url, {}, function(d, status) { //新建下載任务
					if (status == 200) { //當下載完成
						let path = plus.io.convertLocalFileSystemURL(d.filename);
						plus.runtime.install(path, {
							force: false
						}, () => {
							plus.runtime.restart();
						}, (error) => {
							uni.showToast({
								title: '安裝失敗',
								duration: 1500,
								icon: 'none'
							});
							console.log(error.message)
						});
						return;
					}
					uni.showToast({
						title: '更新失敗',
						duration: 1500,
						icon: 'none'
					});

				})
				dtask.start();
				var prg = 0;
				var showLoading = plus.nativeUI.showWaiting("正在下載....");
				dtask.addEventListener('statechanged', function(task, status) { //添加下載任务事件監聽器
					// 给下載任务設置一個監聽 並根據狀態 做操作
					switch (task.state) {
						case 1:
							showLoading.setTitle("正在下載");
							console.log('1111')
							break;
						case 2:
							showLoading.setTitle("已連接到服務器");
							console.log('222')
							break;
						case 3:
							prg = parseInt( //下載的进度
								(parseFloat(task.downloadedSize) / parseFloat(task.totalSize)) * 100
							);
							showLoading.setTitle("版本更新,正在下載" + prg + "% ");
							// console.log('3333')
							console.log("版本更新,正在下載" + prg + "% ")
							break;
						case 4:
							plus.nativeUI.closeWaiting(); //關闭系统提示框
							//下載完成
							break;
					}
				});
			},
			getVersion(version, isService) {
				//服务器當前版本被忽略過，不再提示
				if (this.home && isService && uni.getStorageSync('ingVersion') == version) {
					return -999;
				}
				for (var i = 0; i < 5; i++) {
					version = version.replace('.', '');
				}
				return version * 1;
			},
			updateAndroid(wgtinfo) {
				console.log(wgtinfo)
				this.http.get("api/app/getAndroidVersion?home=" + this.home, {}, false).then(result => {
					console.log(result)
					if (!result.status) {
						this.$emit('updaterLoad')
						return;
					}
					//version為manifest.json小版本號(應用版本名稱)
					if (this.getVersion(result.version, true) <= this.getVersion(wgtinfo.version)) {
						if (!this.home) {
							this.$toast('當前已是最新版本')
						} else {
							this.$emit('updaterLoad')
						}
						return;
					}
					uni.showModal({
						title: "發現版本更新",
						content: result.desc || '發現版本,是否立即更新', //更新描述
						confirmText: '立即更新',
						cancelText: '稍後進行',
						success: sucRes => {
							if (!sucRes.confirm) {
								//首頁點擊忽略當前版本
								if (this.home) {
									uni.setStorageSync('ingVersion', result.version)
								}
								return;
							};
							this.download(result.url);
						}
					})
				})
			},
			updateIOS(wgtinfo) {
				this.http.get("api/app/getIOSVersion?home=" + this.home, {}, false).then(result => {
					if (!result.status) {
						this.$emit('updaterLoad')
						return;
					}
					console.log(wgtinfo.version)
					if (this.getVersion(result.version, true) <= this.getVersion(wgtinfo.version)) {
						if (!this.home) {
							this.$toast('當前已是最新版本')
						} else {
							this.$emit('updaterLoad')
						}

						return;
					}
					console.log('ios')

					uni.showModal({
						title: "發現版本更新",
						content: result.desc || '發現版本,是否立即更新', //更新描述
						confirmText: '立即更新',
						cancelText: '稍後進行',
						success: sucRes => {
							if (!sucRes.confirm) {
								//首頁點擊忽略當前版本
								if (this.home) {
									uni.setStorageSync('ingVersion', result.version)
								}
								return;
							};
							plus.runtime.launchApplication({
								action: result.url
								//`itms-apps://itunes.apple.com/cn/app/id${appleId}?mt=8`
							}, (e) => {
								this.$toast('Open system default browser failed: ' + e
									.message);
							});
						}
					})
					//let appleId = 111111111

				})
			},
			updateWechat() {
				const updateManager = uni.getUpdateManager();

				updateManager.onCheckForUpdate(function(res) {
					// 請求完新版本信息的回調
					console.log(res.hasUpdate);
				});

				updateManager.onUpdateReady(function(res) {
					uni.showModal({
						title: '更新提示',
						content: '新版本已經準備好，是否重啟應用？',
						success(res) {
							if (res.confirm) {
								// 新的版本已經下載好，調用 applyUpdate 應用新版本並重啟
								updateManager.applyUpdate();
							} else {

							}
						}
					});
				});
				updateManager.onUpdateFailed(function(res) {
					// 新的版本下載失败
				});
			},
			checkVersion(isUserGetVersion) {

				// // #ifdef MP-WEIXIN
				// !isUserGetVersion && this.updateWechat();
				// this.$emit('updaterLoad')
				// return;
				// // #endif

				//apk文件名不能带中文
				//#ifdef APP-PLUS
				let isAndroid;
				uni.getSystemInfo({
					success: (e) => {
						isAndroid = e.platform === 'android';
						plus.runtime.getProperty(plus.runtime.appid, (wgtinfo) => {
							if (isUserGetVersion) {
								this.$emit('getVersion', wgtinfo.version)
								return;
							}
							if (isAndroid) {
								this.updateAndroid(wgtinfo);
								return;
							}
							this.updateIOS(wgtinfo);
						})
					},
					fail(ex) {
						this.$toast(ex.messsage)
					}
				})
				return;
				//#endif
				//this.$emit('updaterLoad')
				if (!isUserGetVersion) {
					this.$toast('當前已經最新版本')
				}

			}
		},
		created() {
			//https://www.cnblogs.com/menxiaojin/p/13755555.html
			//https://developers.weixin.qq.com/miniprogram/dev/api/base/update/UpdateManager.html
			if (this.home) {
				this.checkVersion();
			} else {
				this.checkVersion(true);
			}
		}
	}
</script>

<style>

</style>