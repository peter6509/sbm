import { defineMixin } from '../vue'
import { queryParams } from '../function/index'
export const mpShare = defineMixin({
    data() {
        return {
            mpShare: {
                title: '', // 默认為小程序名稱
                path: '', // 默认為当前页面路径
                imageUrl: '' // 默认為当前页面的截图 
            }
        }
    },
    async onLoad(options) {
        var pages = getCurrentPages();
        var page = pages[pages.length - 1];
        this.mpShare.path = page.route + queryParams(options);
    },
    onShareAppMessage(res) {
        if (res.from === 'button') {// 来自页面内分享按鈕
            console.log(res.target)
        }
        return this.mpShare;
    }
})

export default mpShare
