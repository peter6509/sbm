<template>
    <view class="u-album">
        <view
            class="u-album__row"
            ref="u-album__row"
            v-for="(arr, index) in showUrls"
            :forComputedUse="albumWidth"
            :key="index"
            :style="{flexWrap: autoWrap ? 'wrap' : 'nowrap'}"
        >
            <view
                class="u-album__row__wrapper"
                v-for="(item, index1) in arr"
                :key="index1"
                :style="[imageStyle(index + 1, index1 + 1)]"
                @tap="previewFullImage ? onPreviewTap($event, getSrc(item)) : ''"
            >
                <image
                    :src="getSrc(item)"
                    :mode="
                        urls.length === 1
                            ? imageHeight > 0
                                ? singleMode
                                : 'widthFix'
                            : multipleMode
                    "
                    :style="[
                        {
                            width: imageWidth,
                            height: imageHeight,
                            borderRadius: shape == 'circle' ? '10000px' : addUnit(radius)
                        }
                    ]"
                ></image>
                <view
                    v-if="
                        showMore &&
                        urls.length > rowCount * showUrls.length &&
                        index === showUrls.length - 1 &&
                        index1 === showUrls[showUrls.length - 1].length - 1
                    "
                    class="u-album__row__wrapper__text"
                    :style="{
					    borderRadius: shape == 'circle' ? '50%' : addUnit(radius),
				    }"
                >
                    <up-text
                        :text="`+${urls.length - maxCount}`"
                        color="#fff"
                        :size="multipleSize * 0.3"
                        align="center"
                        customStyle="justify-content: center"
                    ></up-text>
                </view>
            </view>
        </view>
    </view>
</template>

<script>
import { props } from './props';
import { mpMixin } from '../../libs/mixin/mpMixin';
import { mixin } from '../../libs/mixin/mixin';
import { addUnit, sleep } from '../../libs/function/index';
import test from '../../libs/function/test';
// #ifdef APP-NVUE
// 由于weex為阿里的KPI业绩考核的產物，所以不支持百分比單位，這里需要通過dom查詢组件的寬度
const dom = uni.requireNativePlugin('dom')
// #endif

/**
 * Album 相册
 * @description 本组件提供一個類似相册的功能，让開發者開發起来更加得心應手。减少重複的模板代碼
 * @tutorial https://ijry.github.io/uview-plus/components/album.html
 *
 * @property {Array}           urls             圖片地址列表 Array<String>|Array<Object>形式
 * @property {String}          keyName          指定從數组的對象元素中讀取哪個屬性作為圖片地址
 * @property {String | Number} singleSize       單圖時，圖片長邊的長度  （默認 180 ）
 * @property {String | Number} multipleSize     多圖時，圖片邊長 （默認 70 ）
 * @property {String | Number} space            多圖時，圖片水平和垂直之間的間隔 （默認 6 ）
 * @property {String}          singleMode       單圖時，圖片缩放裁剪的模式 （默認 'scaleToFill' ）
 * @property {String}          multipleMode     多圖時，圖片缩放裁剪的模式 （默認 'aspectFill' ）
 * @property {String | Number} maxCount         取消按鈕的提示文字 （默認 9 ）
 * @property {Boolean}         previewFullImage 是否可以預覽圖片 （默認 true ）
 * @property {String | Number} rowCount         每行展示圖片數量，如設置，singleSize和multipleSize將會無效	（默認 3 ）
 * @property {Boolean}         showMore         超出maxCount時是否顯示查看更多的提示 （默認 true ）
 * @property {String}          shape            圖片形狀，circle-圆形，square-方形 （默認 'square' ）
 * @property {String | Number} radius           圆角值，單位任意，如果為數值，则為px單位 （默認 0 ）
 * @property {Boolean}         autoWrap         自適應換行模式，不受rowCount限制，圖片會自動換行 （默認 false ）
 * @property {String}          unit             圖片單位 （默認 px ）
 * @event    {Function}        albumWidth       某些特殊的情况下，需要让文字與相册的寬度相等，這里事件的形式對外發送  （回調参數 width ）
 * @example <u-album :urls="urls2" @albumWidth="width => albumWidth = width" multipleSize="68" ></u-album>
 */
export default {
    name: 'u-album',
    mixins: [mpMixin, mixin, props],
    data() {
        return {
            // 單圖的寬度
            singleWidth: 0,
            // 單圖的高度
            singleHeight: 0,
            // 單圖時，如果無法獲取圖片的尺寸信息，让圖片寬度默認為容器的一定百分比
            singlePercent: 0.6
        }
    },
    watch: {
        urls: {
            immediate: true,
            handler(newVal) {
                if (newVal.length === 1) {
                    this.getImageRect()
                }
            }
        }
    },
	emits: ["albumWidth"],
    computed: {
        imageStyle() {
            return (index1, index2) => {
                const { space, rowCount, multipleSize, urls } = this,
                    { addUnit, addStyle } = uni.$u,
                    rowLen = this.showUrls.length,
                    allLen = this.urls.length
                const style = {
                    marginRight: addUnit(space),
                    marginBottom: addUnit(space)
                }
                // 如果為最后一行，则每個圖片都無需下邊框
                if (index1 === rowLen && !this.autoWrap) style.marginBottom = 0
                // 每行的最右邊一張和總長度的最后一張無需右邊框
                if (!this.autoWrap) {
                    if (
                        index2 === rowCount ||
                        (index1 === rowLen &&
                            index2 === this.showUrls[index1 - 1].length)
                    )
                        style.marginRight = 0
                }
                return style
            }
        },
        // 將數组划分為二维數组
        showUrls() {
            if (this.autoWrap) {
                return [ this.urls.slice(0, this.maxCount) ];
            } else {
                const arr = []
                this.urls.map((item, index) => {
                    // 限制最大展示數量
                    if (index + 1 <= this.maxCount) {
                        // 計算該元素為第几個素组内
                        const itemIndex = Math.floor(index / this.rowCount)
                        // 判斷對應的索引是否存在
                        if (!arr[itemIndex]) {
                            arr[itemIndex] = []
                        }
                        arr[itemIndex].push(item)
                    }
                })
                return arr
            }
        },
        imageWidth() {
            return addUnit(
                this.urls.length === 1 ? this.singleWidth : this.multipleSize, this.unit
            )
        },
        imageHeight() {
            return addUnit(
                this.urls.length === 1 ? this.singleHeight : this.multipleSize, this.unit
            )
        },
        // 此變量無實际用途，仅仅是為了利用computed特性，让其在urls長度等變化時，重新計算圖片的寬度
        // 因為用户在某些特殊的情况下，需要让文字與相册的寬度相等，所以這里事件的形式對外發送
        albumWidth() {
            let width = 0
            if (this.urls.length === 1) {
                width = this.singleWidth
            } else {
                width =
                    this.showUrls[0].length * this.multipleSize +
                    this.space * (this.showUrls[0].length - 1)
            }
            this.$emit('albumWidth', width)
            return width
        }
    },
    methods: {
        addUnit,
        // 預覽圖片
        onPreviewTap(e, url) {
            const urls = this.urls.map((item) => {
                return this.getSrc(item)
            })
            uni.previewImage({
                current: url,
                urls
            })
            // 是否阻止事件傳播
			this.stop && this.preventEvent(e)
        },
        // 獲取圖片的路徑
        getSrc(item) {
            return test.object(item)
                ? (this.keyName && item[this.keyName]) || item.src
                : item
        },
        // 單圖時，獲取圖片的尺寸
        // 在小程序中，需要將网络圖片的的域名添加到小程序的download域名才可能獲取尺寸
        // 在没有添加的情况下，让單圖寬度默認為盒子的一定寬度(singlePercent)
        getImageRect() {
            const src = this.getSrc(this.urls[0])
            uni.getImageInfo({
                src,
                success: (res) => {
                    // 判斷圖片横向還是竖向展示方式
                    const isHorizotal = res.width >= res.height
                    this.singleWidth = isHorizotal
                        ? this.singleSize
                        : (res.width / res.height) * this.singleSize
                    this.singleHeight = !isHorizotal
                        ? this.singleSize
                        : (res.height / res.width) * this.singleWidth
                },
                fail: () => {
                    this.getComponentWidth()
                }
            })
        },
        // 獲取组件的寬度
        async getComponentWidth() {
            // 延時一定時間，以獲取dom尺寸
            await sleep(30)
            // #ifndef APP-NVUE
            this.$uGetRect('.u-album__row').then((size) => {
                this.singleWidth = size.width * this.singlePercent
            })
            // #endif

            // #ifdef APP-NVUE
            // 這里ref="u-album__row"所在的標簽為通過for循环出来，导致this.$refs['u-album__row']是一個數组
            const ref = this.$refs['u-album__row'][0]
            ref &&
                dom.getComponentRect(ref, (res) => {
                    this.singleWidth = res.size.width * this.singlePercent
                })
            // #endif
        }
    }
}
</script>

<style lang="scss" scoped>
@import '../../libs/css/components.scss';

.u-album {
    @include flex(column);

    &__row {
        @include flex(row);

        &__wrapper {
            position: relative;

            &__text {
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background-color: rgba(0, 0, 0, 0.3);
                @include flex(row);
                justify-content: center;
                align-items: center;
            }
        }
    }
}
</style>
