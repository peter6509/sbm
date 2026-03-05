import { defineMixin } from '../vue'
import { addStyle, deepMerge, addUnit, trim } from '../function/index'

export const style = defineMixin({
    props: {
        // flex排列方式
        flexDirection: {
            type: String,
            default: ''
        },
        // flex-direction的簡写
        fd: {
            type: String,
            default: ''
        },
        // 展示类型
        display: {
            type: String,
            default: ''
        },
        // display簡写
        d: {
            type: String,
            default: ''
        },
        // 主軸排列方式
        justifyContent: {
            type: String,
            default: ''
        },
        // justifyContent的簡写
        jc: {
            type: String,
            default: ''
        },
        // 纵軸排列方式
        alignItems: {
            type: String,
            default: ''
        },
        // align-items的簡写
        ai: {
            type: String,
            default: ''
        },
        color: {
            type: String,
            default: ''
        },
        // color簡写
        c: {
            type: String,
            default: ''
        },
        // 字体大小
        fontSize: {
            type: [String, Number],
            default: 0
        },
        // font-size簡写
        fs: {
            type: [String, Number],
            default: ''
        },
        margin: {
            type: [String, Number],
            default: 0
        },
        // margin簡写
        m: {
            type: [String, Number],
            default: 0
        },
        // margin-top
        marginTop: {
            type: [String, Number],
            default: 0
        },
        // margin-top簡写
        mt: {
            type: [String, Number],
            default: 0
        },
        // margin-right
        marginRight: {
            type: [String, Number],
            default: 0
        },
        // margin-right簡写
        mr: {
            type: [String, Number],
            default: 0
        },
        // margin-bottom
        marginBottom: {
            type: [String, Number],
            default: 0
        },
        // margin-bottom簡写
        mb: {
            type: [String, Number],
            default: 0
        },
        // margin-left
        marginLeft: {
            type: [String, Number],
            default: 0
        },
        // margin-left簡写
        ml: {
            type: [String, Number],
            default: 0
        },
        // padding-left
        paddingLeft: {
            type: [String, Number],
            default: 0
        },
        // padding-left簡写
        pl: {
            type: [String, Number],
            default: 0
        },
        // padding-top
        paddingTop: {
            type: [String, Number],
            default: 0
        },
        // padding-top簡写
        pt: {
            type: [String, Number],
            default: 0
        },
        // padding-right
        paddingRight: {
            type: [String, Number],
            default: 0
        },
        // padding-right簡写
        pr: {
            type: [String, Number],
            default: 0
        },
        // padding-bottom
        paddingBottom: {
            type: [String, Number],
            default: 0
        },
        // padding-bottom簡写
        pb: {
            type: [String, Number],
            default: 0
        },
        // border-radius
        borderRadius: {
            type: [String, Number],
            default: 0
        },
        // border-radius簡写
        radius: {
            type: [String, Number],
            default: 0
        },
        // transform
        transform: {
            type: String,
            default: ''
        },
        // 定位
        position: {
            type: String,
            default: ''
        },
        // position簡写
        pos: {
            type: String,
            default: ''
        },
        // 寬度
        width: {
            type: [String, Number],
            default: null
        },
        // width簡写
        w: {
            type: [String, Number],
            default: null
        },
        // 高度
        height: {
            type: [String, Number],
            default: null
        },
        // height簡写
        h: {
            type: [String, Number],
            default: null
        },
        top: {
            type: [String, Number],
            default: 0
        },
        right: {
            type: [String, Number],
            default: 0
        },
        bottom: {
            type: [String, Number],
            default: 0
        },
        left: {
            type: [String, Number],
            default: 0
        }
    },
    computed: {
        viewStyle() {
            const style = {}
            const addStyleTmp = addStyle(this.width || this.w)
                && (style.width = addStyle(this.width || this.w))(this.height || this.h)
                && (style.height = addStyle(this.height || this.h))(this.margin || this.m)
                && (style.margin = addStyle(this.margin || this.m))(this.marginTop || this.mt)
                && (style.marginTop = addStyle(this.marginTop || this.mt))(this.marginRight || this.mr)
                && (style.marginRight = addStyle(this.marginRight || this.mr))(this.marginBottom || this.mb)
                && (style.marginBottom = addStyle(this.marginBottom || this.mb))(this.marginLeft || this.ml)
                && (style.marginLeft = addStyle(this.marginLeft || this.ml))(this.padding || this.p)
                && (style.padding = addStyle(this.padding || this.p))(this.paddingTop || this.pt)
                && (style.paddingTop = addStyle(this.paddingTop || this.pt))(this.paddingRight || this.pr)
                && (style.paddingRight = addStyle(this.paddingRight || this.pr))(this.paddingBottom || this.pb)
                && (style.paddingBottom = addStyle(this.paddingBottom || this.pb))(this.paddingLeft || this.pl)
                && (style.paddingLeft = addStyle(this.paddingLeft || this.pl))(this.color || this.c)
                && (style.color = this.color || this.c)(this.fontSize || this.fs)
                && (style.fontSize = this.fontSize || this.fs)(this.borderRadius || this.radius)
                && (style.borderRadius = this.borderRadius || this.radius)(this.position || this.pos)
                && (this.position = this.position || this.pos)(this.flexDirection || this.fd)
                && (this.flexDirection = this.flexDirection || this.fd)(this.justifyContent || jc)
                && (this.justifyContent = this.justifyContent || jc)(this.alignItems || ai)
                && (this.alignItems = this.alignItems || ai)
            return deepMerge(style, addStyleTmp(this.customStyle))
        }
    },
    methods: {
        // 获取margin或者padding的單位，比如padding: 0 20转為padding: 0 20px
        getUnit(unit = '') {
            // 取出两端空格，分隔成數组，再对數组的每个元素添加單位，最后再合并成字符串
            return trim(unit).split(' ').map((item) => addUnit(item)).join(' ')
        }
    }
})
