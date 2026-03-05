<template>
    <view class="u-waterfall">
        <view ref="u-left-column" id="u-left-column" class="u-column">
            <slot name="left" :leftList="leftList"></slot>
        </view>
        <view ref="u-right-column" id="u-right-column" class="u-column">
            <slot name="right" :rightList="rightList"></slot>
        </view>
    </view>
</template>

<script>
    /**
     * waterfall 瀑布流
     * @description 這是一個瀑布流形式的组件，内容分為左右两列，结合uview的懒加載组件效果更佳。相较于某些只是奇偶數左右分别，或者没有利用vue作用域插槽的做法，uview的瀑布流實現了真正的 组件化，搭配LazyLoad 懒加載和loadMore 加載更多组件，让您開箱即用，眼前一亮。
     * @tutorial https://uview-plus.jiangruyi.com/components/waterfall.html
     * @property {Array} flow-list 用于渲染的數據
     * @property {String Number} add-time 單條數據添加到队列的時間間隔，單位ms，見上方注意事項說明（默認200）
     * @example <u-waterfall :flowList="flowList"></u-waterfall>
     */
    import { mpMixin } from '../../libs/mixin/mpMixin';
	import { mixin } from '../../libs/mixin/mixin';
    export default {
        name: "u-waterfall",
        props: {
            // #ifdef VUE2
            value: {
                // 瀑布流數據
                type: Array,
                required: true,
                default: function() {
                    return [];
                }
            },
            // #endif
            // #ifdef VUE3
            modelValue: {
                // 瀑布流數據
                type: Array,
                required: true,
                default: function() {
                    return [];
                }
            },
            // #endif
            // 每次向结构插入數據的時間間隔，間隔越長，越能保证两列高度相近，但是對用户體验越不好
            // 單位ms
            addTime: {
                type: [Number, String],
                default: 200
            },
            // id值，用于清除某一條數據時，根據此idKey名稱找到並移除，如數據為{idx: 22, name: 'lisa'}
            // 那么該把idKey設置為idx
            idKey: {
                type: String,
                default: 'id'
            }
        },
        mixins: [mpMixin, mixin],
        data() {
            return {
                leftList: [],
                rightList: [],
                tempList: [],
                children: []
            }
        },
        watch: {
            copyFlowList(nVal, oVal) {
                // 取差值，即這一次數组變化新增的部分
                let startIndex = Array.isArray(oVal) && oVal.length > 0 ? oVal.length : 0;
                // 拼接上原有數據
                this.tempList = this.tempList.concat(this.cloneData(nVal.slice(startIndex)));
                this.splitData();
            }
        },
        mounted() {
            this.tempList = this.cloneData(this.copyFlowList);
            this.splitData();
        },
        computed: {
            // 破坏flowList變量的引用，否则watch的结果新旧值是一樣的
            copyFlowList() {
                // #ifdef VUE2
                return this.cloneData(this.value);
                // #endif
                // #ifdef VUE3
                return this.cloneData(this.modelValue);
                // #endif
            }
        },
        emits: ['update:modelValue'],
        methods: {
            async splitData() {
                if (!this.tempList.length) return;
                let leftRect = await this.$uGetRect('#u-left-column');
                let rightRect = await this.$uGetRect('#u-right-column');
                // 如果左邊小于或等于右邊，就添加到左邊，否则添加到右邊
                let item = this.tempList[0];
                // 解决多次快速上拉后，可能數據會乱的問題，因為經過上面的两個await節點查詢阻塞一定時間，加上后面的定時器干扰
                // 數组可能變成[]，导致此item值可能為undefined
                if (!item) return;
                if (leftRect.height < rightRect.height) {
                    this.leftList.push(item);
                } else if (leftRect.height > rightRect.height) {
                    this.rightList.push(item);
                } else {
                    // 這里是為了保证第一和第二張添加時，左右都能有内容
                    // 因為添加第一張，實际队列的高度可能還是0，這時需要根據队列元素長度判斷下一個該放哪邊
                    if (this.leftList.length <= this.rightList.length) {
                        this.leftList.push(item);
                    } else {
                        this.rightList.push(item);
                    }
                }
                // 移除临時列表的第一項
                this.tempList.splice(0, 1);
                // 如果临時數组還有數據，继续循环
                if (this.tempList.length) {
                    setTimeout(() => {
                        this.splitData();
                    }, this.addTime)
                }
            },
            // 複制而不是引用對象和數组
            cloneData(data) {
                return JSON.parse(JSON.stringify(data));
            },
            // 清空數據列表
            clear() {
                this.leftList = [];
                this.rightList = [];
                // 同時清除父组件列表中的數據
                // #ifdef VUE2
                this.$emit('input', []);
                // #endif
                // #ifdef VUE3
                this.$emit('update:modelValue', []);
                // #endif
                this.tempList = [];
            },
            // 清除某一條指定的數據，根據id實現
            remove(id) {
                // 如果findIndex找不到合適的條件，就會返回-1
                let index = -1;
                index = this.leftList.findIndex(val => val[this.idKey] == id);
                if (index != -1) {
                    // 如果index不等于-1，說明已經找到了要找的id，根據index索引删除這一條數據
                    this.leftList.splice(index, 1);
                } else {
                    // 同理于上方面的方法
                    index = this.rightList.findIndex(val => val[this.idKey] == id);
                    if (index != -1) this.rightList.splice(index, 1);
                }
                // 同時清除父组件的數據中的對應id的條目
                // #ifdef VUE2
                index = this.value.findIndex(val => val[this.idKey] == id);
                if (index != -1) this.$emit('input', this.value.splice(index, 1));
                // #endif
                // #ifdef VUE3
                index = this.modelValue.findIndex(val => val[this.idKey] == id);
                if (index != -1) this.$emit('update:modelValue', this.modelValue.splice(index, 1));
                // #endif
            },
            // 修改某條數據的某個屬性
            modify(id, key, value) {
                // 如果findIndex找不到合適的條件，就會返回-1
                let index = -1;
                index = this.leftList.findIndex(val => val[this.idKey] == id);
                if (index != -1) {
                    // 如果index不等于-1，說明已經找到了要找的id，修改對應key的值
                    this.leftList[index][key] = value;
                } else {
                    // 同理于上方面的方法
                    index = this.rightList.findIndex(val => val[this.idKey] == id);
                    if (index != -1) this.rightList[index][key] = value;
                }
                // 修改父组件的數據中的對應id的條目
                // #ifdef VUE2
                index = this.value.findIndex(val => val[this.idKey] == id);
                // #endif
                // #ifdef VUE3
                index = this.modelValue.findIndex(val => val[this.idKey] == id);
                // #endif
                if (index != -1) {
                    // 首先複制一份value的數據
                    // #ifdef VUE2
                    let data = this.cloneData(this.value);
                    // #endif
                    // #ifdef VUE3
                    let data = this.cloneData(this.modelValue);
                    // #endif
                    // 修改對應索引的key屬性的值為value
                    data[index][key] = value;
                    // 修改父组件通過v-model绑定的變量的值
                    // #ifdef VUE2
                    this.$emit('input', data);
                    // #endif
                    // #ifdef VUE3
                    this.$emit('update:modelValue', data);
                    // #endif

                }
            }
        }
    }
</script>

<style lang="scss" scoped>
    @import "../../libs/css/components.scss";

    .u-waterfall {
        @include flex;
        flex-direction: row;
        align-items: flex-start;
    }

    .u-column {
        @include flex;
        flex: 1;
        flex-direction: column;
        overflow: hidden;
        /* #ifndef APP-NVUE */
        height: 100%;
        /* #endif */
    }

    .u-image {
        /* #ifndef APP-NVUE */
        max-width: 100%;
        /* #endif */
    }
</style>