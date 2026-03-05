<template>
    <div class="form-content" :style="{ height: height + 'px' }">
        <div class="vol-custom-fields">
            <div class="custom-title">
                <i class="el-icon-edit-outline"></i> {{ $ts("字段列表") }}
            </div>
            <el-scrollbar style="flex:1;">

                <coderV3FormFields ref="fieldsRef" :checkEnd="checkEnd" @end="end"></coderV3FormFields>
            </el-scrollbar>
        </div>
        <div class="form-content-options">
            <div class="custom-title">
                <div style="flex:1"><i class="el-icon-receiving" style="margin-right:3px"></i>{{ currentTabs == 'search'
                    ? '查詢' : '編輯' }}{{
                        $ts("表單布局") }}
                    <el-popover title="配置说明" :width="480">
                        <template #reference>
                            <span style="color: #0247de; font-size: 13px;" class="el-icon-info">一對多明细表</span>
                        </template>
                        <div style="line-height: 2;">
                            <div>1.主表與明细表分開配置生成代碼</div>
                            <div>2.主表【基礎信息】填寫【明细表】,點擊:保存->預覽即可實時看到效果
                            </div>
                            <!-- <p>2.配置表结構信息</p>
            <p>3.配置查詢表單</p>
            <p>4.配置編輯表單</p> -->
                        </div>
                    </el-popover>
                </div>
                <!-- <span class="desc" v-show="currentTabs == 'edit'">
                    <span class="el-icon-info">主表與明细表分開維护,主表表單配置填寫明细表信息,點擊預覽即可查看。。。</span>
                </span> -->

                <div class="attr-item">
                    <div class="attr-item-title"> <span class="el-icon-info"></span>批量設置表單字段數量</div>
                    <el-select style="width: 180px;" v-model="bacthItemWidth" @change="batchWidthChange"
                        :placeholder="$ts('請選擇')">
                        <el-option v-for="item in batchWidth" :key="item.key" :label="item.value" :value="item.key">
                        </el-option>
                    </el-select>
                </div>
                <el-button color="#626aef" icon="Refresh" plain @click="clearClick">清除表單</el-button>
            </div>
            <el-scrollbar style="height: 100%;">
                <div class="ep-text" v-if="!formOptions.length">
                    <div><i class="el-icon-receiving"></i></div>
                    請將左邊字段拖到此處
                </div>
                <el-form label-position="top">
                    <draggable class="draggable-container" v-model="formOptions" @end="formEnd" animation="300"
                        :move="onMove" style="min-height: 300px" group="componentsGroup">
                        <transition-group>
                            <div class="vol-custom-form-item" @click="itemClick(item, index)"
                                :class="{ actived: index === currentIndex }"
                                :style="{ width: (item.width || 25) + '%' }" v-for="(item, index) in formOptions"
                                :key="index">
                                <i class="el-icon-delete" @click.stop="removeItem(index)"> </i>
                                <el-form-item :required="item.required" label-position="top" style="width: 100%"
                                    :label="$ts(item.title)">
                                    <coderV3FormItem :typeField="typeField" :item="item"></coderV3FormItem>
                                </el-form-item>
                            </div>
                        </transition-group>
                    </draggable>
                </el-form>
            </el-scrollbar>
        </div>
        <div class="form-content-attr">
            <div class="custom-title">
                <i class="el-icon-paperclip"></i> {{ $ts("屬性設置") }}
            </div>
            <el-scrollbar style="height: 100%;">
                <coderV3FormAttr :currentTabs="currentAction" ref="formAttrRef" @batchWidthChange="batchWidthChange"
                    :currentItem="currentItem">
                </coderV3FormAttr>
            </el-scrollbar>
        </div>
    </div>

</template>
<script lang="jsx" setup>
import { VueDraggableNext as draggable } from "vue-draggable-next";
import coderV3FormFields from "./coderV3FormFields.vue";

import coderV3FormItem from './coderV3FormItem.vue'
import coderV3FormAttr from './coderV3FormAttr.vue'
import { ref, reactive, getCurrentInstance, nextTick } from "vue";
const { proxy } = getCurrentInstance();
const tableHeight = ref(0);
tableHeight.value = document.body.clientHeight - 443;
if (tableHeight.value <= 260) {
    tableHeight.value = 260;
}
const pros = defineProps({
    height: {
        type: Number,
        default: 100,
    },
    currentAction: {
        type: String,
        default: ''
    }
    // fieldOptions: {
    //     type: Array,
    //     default: () => {
    //         return [];
    //     },
    // }
});
const formOptions = ref([]);
const formAttrRef = ref();
const fieldsRef = ref()
const currentTabs = ref();
const typeField = ref();
// const batchWidthChange = (val) => {
//    // emit('batchWidthChange', val || 0)
// }
const bacthItemWidth = ref(null)
const batchWidth = reactive([
    { key: 0, value: "自動" },
    { key: 100, value: "表單每行顯示1個字段" },
    { key: 50, value: "表單每行顯示2個字段" },
    { key: 33.333, value: "表單每行顯示3個字段" },
    { key: 25, value: "表單每行顯示4個字段" },//每行顯示4個字段
    { key: 20, value: "表單每行顯示5個字段" },//每行顯示5個字段
    { key: 16.666, value: "表單每行顯示6個字段" },//每行顯示6個字段
    { key: 14.285, value: "表單每行顯示7個字段" },//每行顯示7個字段
    { key: 12.5, value: "表單每行顯示8個字段" },//每行顯示7個字段
    { key: 11.111, value: "表單每行顯示9個字段" },//每行顯示7個字段
    { key: 10, value: "表單每行顯示10個字段" },//每行顯示10個字段
]);

const initDraggableFields = (options, searchEditFormOptions, action) => {
    currentTabs.value = action || ''
    if (action == 'search') {
        typeField.value = 'searchType'
    } else if (action == 'edit') {
        typeField.value = 'editType'
    } else {
        typeField.value = ''
    }
    nextTick(() => {
        formOptions.value = searchEditFormOptions;
        // formOptions.value = options.filter(x => {
        //     return (!action || action == 'search' ? x.searchRowNo : x.editRowNo) > 0
        // })
        fieldsRef.value.initDraggableFields(options, action);
        formAttrRef.value.reset(action);

    })
}

const currentIndex = ref(-1);
const currentItem = ref({})
const itemClick = (item, index) => {
    currentItem.value = item;
    if (!currentItem.value.width) {
        currentItem.value.width = 25;
    }
    currentIndex.value = index;
};

const removeItem = (index) => {
    formOptions.value.splice(index, 1);
    currentIndex.value = -1;
};
const end = (ops) => {
    ops = Object.assign({}, ops);
    currentItem.value = ops;
    console.log(ops)
};
const formEnd = (field) => { };
const onMove = () => { };
let timeMsg = false;

const checkEnd = (field) => {
    const b = formOptions.value.some((c) => {
        return c.field == field;
    })
    if (b) {
        if (!timeMsg) {
            timeMsg = true;
            setTimeout(() => {
                timeMsg = false;
            }, 1000);
            proxy.$message.error("字段已存在");
        }
        return false;
    }
    return true;
};

const batchWidthChange = (wd) => {
    formOptions.value.forEach(x => {
        x.width = wd;
    })
}

const getFormOptions = () => {
    return formOptions.value
}

const clearClick = () => {
    formOptions.value = []
    proxy.$message.success('清除成功')
}

defineExpose({ initDraggableFields, getFormOptions });
</script>
<style lang="less" scoped>
@import "./style/coderV3Content.less";

.form-content {
    display: flex;
    padding: 0 !important;

    .vol-custom-fields {
        width: 222px;
        display: flex;
        flex-direction: column;
        // background: #eee;
        border-right: 1px solid #eee;
    }

    .form-content-options {
        position: relative;
        display: flex;
        flex-direction: column;
        width: 0;
        flex: 1;
        border-right: 1px solid #eee;
    }

    .form-content-attr {
        display: flex;
        flex-direction: column;
        width: 220px;
    }

    ::v-deep(.el-scrollbar__bar.is-vertical) {
        width: 3px !important;
    }
}

.custom-title {
    // background: #ebedff;
    padding: 10px;
    color: #000;
    font-weight: bold;
    border-bottom: 1px solid #eee;
}

.draggable-container {
    min-height: 300px;
}

.vol-custom-form-item {
    position: relative;
    cursor: move;
    padding: 18px 10px 10px 10px;
    text-align: left;
    float: left;
    margin-bottom: 0px;

    ::v-deep(.el-form-item__label) {
        line-height: 0 !important;
        color: #000;
    }

    ::v-deep(.el-form-item) {
        margin-bottom: 5px !important;
    }

    .el-icon-delete {
        position: absolute;
        right: 10px;
        top: 5px;
        padding: 5px;
        display: none;
        color: red;
        cursor: pointer;
    }
}

.vol-custom-form-item.actived,
.vol-custom-form-item:hover {
    background: #f3faff;

    .el-icon-delete {
        display: inline-block;
    }
}

.ep-text {
    font-size: 15px;
    color: #737272;
    text-align: center;
    position: absolute;
    width: 100%;
    top: 150px;
    letter-spacing: 1px;

    i {
        margin-top: 10px;
        color: #e2e2e2;
        font-size: 35px;
    }
}

.custom-title {
    display: flex;
    height: 44px;
    align-items: center;

    button {
        margin-left: 15px;
    }

    .attr-item {
        align-items: center;
        display: flex;
        font-weight: 400;
    }

    .attr-item-title {
        color: #0247de;
        margin-right: 10px;
        font-size: 13px;
    }
}
</style>
