/**
 * 路由跳转方法，该方法相对于直接使用uni.xxx的好处是使用更加簡單快捷
 * 并且带有路由拦截功能
 */
import { queryParams, deepMerge, page } from '../function/index';
class Router {
    constructor() {
        // 原始屬性定义
        this.config = {
            type: 'navigateTo',
            url: '',
            delta: 1, // navigateBack页面后退时,回退的层數
            params: {}, // 傳递的参數
            animationType: 'pop-in', // 窗口动画,只在APP有效
            animationDuration: 300, // 窗口动画持续时间,單位毫秒,只在APP有效
            intercept: false // 是否需要拦截
        }
        // 因為route方法是需要对外赋值给另外的对象使用，同时route内部有使用this，会导致route失去上下文
        // 这里在构造函數中进行this绑定
        this.route = this.route.bind(this)
    }

    // 判断url前面是否有"/"，如果没有则加上，否则無法跳转
    addRootPath(url) {
        return url[0] === '/' ? url : `/${url}`
    }

    // 整合路由参數
    mixinParam(url, params) {
        url = url && this.addRootPath(url)

        // 使用正则匹配，主要依据是判断是否有"/","?","="等，如“/page/index/index?name=mary"
        // 如果有url中有get参數，转換后無需带上"?"
        let query = ''
        if (/.*\/.*\?.*=.*/.test(url)) {
            // object对象转為get类型的参數
            query = queryParams(params, false)
            // 因為已有get参數,所以后面拼接的参數需要带上"&"隔开
            return url += `&${query}`
        }
        // 直接拼接参數，因為此处url中没有后面的query参數，也就没有"?/&"之类的符号
        query = queryParams(params)
        return url += query
    }

    // 对外的方法名稱
    async route(options = {}, params = {}) {
        // 合并用户的配置和内部的默认配置
        let mergeConfig = {}

        if (typeof options === 'string') {
            // 如果options為字符串，则為route(url, params)的形式
            mergeConfig.url = this.mixinParam(options, params)
            mergeConfig.type = 'navigateTo'
        } else {
            mergeConfig = deepMerge(this.config, options)
            // 否则正常使用mergeConfig中的url和params进行拼接
            mergeConfig.url = this.mixinParam(options.url, options.params)
        }

        // 如果本次跳转的路径和本页面路径一致，不执行跳转，防止用户快速点击跳转按鈕，造成多次跳转同一个页面的问題
        if (mergeConfig.url === page()) return

        if (params.intercept) {
            this.config.intercept = params.intercept
        }
        // params参數也带给拦截器
        mergeConfig.params = params
        // 合并内外部参數
        mergeConfig = deepMerge(this.config, mergeConfig)
        // 判断用户是否定义了拦截器
        if (typeof uni.$u.routeIntercept === 'function') {
            // 定一个promise，根据用户执行resolve(true)或者resolve(false)来决定是否进行路由跳转
            const isNext = await new Promise((resolve, reject) => {
                uni.$u.routeIntercept(mergeConfig, resolve)
            })
            // 如果isNext為true，则执行路由跳转
            isNext && this.openPage(mergeConfig)
        } else {
            this.openPage(mergeConfig)
        }
    }

    // 执行路由跳转
    openPage(config) {
        // 解构参數
        const {
            url,
            type,
            delta,
            animationType,
            animationDuration
        } = config
        if (config.type == 'navigateTo' || config.type == 'to') {
            uni.navigateTo({
                url,
                animationType,
                animationDuration
            })
        }
        if (config.type == 'redirectTo' || config.type == 'redirect') {
            uni.redirectTo({
                url
            })
        }
        if (config.type == 'switchTab' || config.type == 'tab') {
            uni.switchTab({
                url
            })
        }
        if (config.type == 'reLaunch' || config.type == 'launch') {
            uni.reLaunch({
                url
            })
        }
        if (config.type == 'navigateBack' || config.type == 'back') {
            uni.navigateBack({
                delta
            })
        }
    }
}

export default (new Router()).route
