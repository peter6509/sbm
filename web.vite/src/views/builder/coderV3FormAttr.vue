<template>
    <div style="padding:10px;">
        <div class="attr-item-group">
            <div class="attr-item">
                <div class="attr-item-title">字段</div>
                <!-- {{currentItem.title||currentItem.field}}/{{currentItem.field}} -->
                <el-input v-model="currentItem.title" :placeholder="$ts('字段名稱')" disabled />
            </div>
            <div class="attr-item">
                <div class="attr-item-title">名字</div>
                <!-- {{currentItem.title||currentItem.field}}/{{currentItem.field}} -->
                <el-input v-model="currentItem.field" :placeholder="$ts('字段名稱')" disabled />
            </div>
        </div>
        <div class="attr-item" v-show="currentTabs == 'search'">
            <div class="attr-item-title">查詢類型</div>
            <el-select style="width: 100%" size="default" v-model="currentItem.formType" :multiple="false" filterable
                @clear="() => { currentItem.formType = '' }" :placeholder="$ts('請選擇')" clearable>
                <el-option v-for="item in searchDataType" :key="item.key" :label="$ts(item.value)" :value="item.key">
                </el-option>
            </el-select>
        </div>
        <!-- {{currentTabs||'無'}} -->
        <div class="attr-item" v-show="currentTabs == 'edit'">
            <div class="attr-item-title">編輯類型</div>
            <el-select style="width: 100%" size="default" v-model="currentItem.formType" :multiple="false" filterable
                @clear="() => { currentItem.formType = '' }" :placeholder="$ts('請選擇')" clearable>
                <el-option v-for="item in editDataType" :key="item.key" :label="$ts(item.value)" :value="item.key">
                </el-option>
            </el-select>
        </div>

        <div class="attr-item">
            <div class="attr-item-title">數據源<el-button style="margin-left:15px" link type="primary"
                    @click="showDic">+維护數據源</el-button>
            </div>
            <el-select style="width: 100%" size="default" v-model="currentItem.dropNo" :multiple="false" filterable
                @clear="() => { currentItem.dropNo = '' }" :placeholder="$ts('請選擇')" clearable>
                <el-option v-for="item in dicData" :key="item.key" :label="$ts(item.value)" :value="item.key">
                </el-option>
            </el-select>
        </div>
        <div class="attr-item" v-show="currentTabs == 'edit'">
            <div class="attr-item-title">字段只讀</div>
            <el-radio-group v-model="currentItem.isReadDataset">
                <el-radio :key="1" label="是" :value="1"></el-radio>
                <el-radio :key="0" label="否" :value="0"></el-radio>
            </el-radio-group>
        </div>
        <!-- <div class="attr-item">
            <div class="attr-item-title">批量設置表單每行顯示字段數量</div>
            <el-select v-model="bacthItemWidth" @change="batchWidthChange" :placeholder="$ts('請選擇')">
                <el-option v-for="item in batchWidth" :key="item.key" :label="item.value" :value="item.key">
                </el-option>
            </el-select>
        </div> -->
        <div class="attr-item">
            <div class="attr-item-title">字段在表單上所占宽度</div>
            <el-radio-group v-model="currentItem.width" :placeholder="$ts('請選擇')">
                <el-radio v-for="item in formWidth" :key="item.key" :label="item.value" :value="item.key">
                </el-radio>
            </el-radio-group>
        </div>
        <div class="dic-ref">
            <dicPage ref="dicRef"></dicPage>
        </div>

    </div>
</template>
<script lang="jsx" setup>

import { columnType, dataType as editDataType, searchDataType } from "./coderV3Options.jsx";
import { ref, reactive, getCurrentInstance, watch, defineAsyncComponent } from "vue";

const dicPage = defineAsyncComponent(() => import('@/views/sys/system/Sys_Dictionary.vue'))

const { proxy } = getCurrentInstance();
const pros = defineProps({
    currentItem: {
        type: Object,
        default: () => {
            return {
                isReadDataset: 0
            };
        }
    },
    currentTabs: {
        type: String,
        default: ""
    }
})
const emit = defineEmits(['batchWidthChange'])
const dicData = ref([])
const dicRef = ref();
let dicGrid
const showDic = () => {
    dicRef.value.$refs.grid.add()
    if (dicGrid) {
        return;
    }
    setTimeout(() => {
        dicGrid = dicRef.value.$refs.grid
        dicGrid.addAfter = () => {
            // console.log('initDic')
            proxy.base.getItem('coder:initDic')();
            return true;
        }
        dicGrid.editFormOptions.forEach(x => {
            x.forEach(ops => {
                ops.readonly = false;
            })
        })
        dicGrid.detailOptions.columns.forEach(x => {
            x.readonly = false;
        })
        dicGrid.detailOptions.buttons.splice(0)
        dicGrid.detailOptions.buttons.push(
            ...[
                {
                    name: '添加行',
                    icon: 'el-icon-plus',
                    type: 'primary',
                    hidden: false,
                    plain: true,
                    onClick() {
                        dicGrid.addRow()
                    }
                },
                {
                    type: 'danger',
                    plain: true,
                    name: '删除行',
                    hidden: false,
                    icon: 'el-icon-delete',
                    onClick() {
                        dicGrid.delRow()
                    }
                }])
        dicGrid.boxButtons.unshift({
            name: "保存",
            icon: "el-icon-check",
            type: "primary",
            onClick: () => {
                dicGrid.save()
            }
        })
    }, 300)

}


watch(
    () => proxy.base.getItem('coderDicData'),
    (data) => {
        dicData.value = data;
    }
)

const formWidth = reactive([
    { key: 0, value: "自動" },
    { key: 9.999, value: "10%" },//每行顯示10個字段
    { key: 11.111, value: "11%" },//每行顯示10個字段
    { key: 12.5, value: "12%" },//每行顯示8個字段
    { key: 14.285, value: "14%" },//每行顯示7個字段
    { key: 16.666, value: "16%" },//每行顯示6個字段
    { key: 20, value: "20%" },//每行顯示5個字段
    { key: 25, value: "25%" },//每行顯示4個字段
    { key: 30, value: "30%" },
    { key: 33.333, value: "33%" },
    { key: 40, value: '40%' },
    { key: 50, value: "50%" },
    { key: 60, value: "60%" },
    { key: 80, value: "80%" },
    { key: 100, value: "100%" },
]);

const batchWidthChange = (val) => {
    emit('batchWidthChange', val || 0)
}

// const bacthItemWidth = ref(null)
// const batchWidth = reactive([
//     { key: 0, value: "自動" },
//     { key: 100, value: "表單每行顯示1個字段" },
//     { key: 50, value: "表單每行顯示2個字段" },
//     { key: 33.333, value: "表單每行顯示3個字段" },
//     { key: 25, value: "表單每行顯示4個字段" },//每行顯示4個字段
//     { key: 20, value: "表單每行顯示5個字段" },//每行顯示5個字段
//     { key: 16.666, value: "表單每行顯示6個字段" },//每行顯示6個字段
//     { key: 14.285, value: "表單每行顯示7個字段" },//每行顯示7個字段
//     { key: 12.5, value: "表單每行顯示8個字段" },//每行顯示7個字段
//     { key: 11.111, value: "表單每行顯示9個字段" },//每行顯示7個字段
//     { key: 10, value: "表單每行顯示10個字段" },//每行顯示10個字段
// ]);

const dataTypeList = ref([])
const rangeChange = () => {
    if (currentItem.value.range) {
        currentItem.value.value = [null, null];
    } else {
        currentItem.value.value = null;
    }
};
const selectChange = () => {
    if (currentItem.value.type == "selectList") {
        currentItem.value.value = [];
    } else {
        currentItem.value.value = null;
    }
};
const isDate = (type) => {
    return ["date", "datetime", "month"].indexOf(type) != -1;
};
const isNubmer = (type) => {
    return ["decimal", "number", "int"].indexOf(type) != -1;
};
const reset = (action) => {
    dataTypeList.value = action == 'search' ? searchDataType : editDataType;
}
defineExpose({ reset });
</script>
<style lang="less" scoped>
.custom-filter-name {
    height: 40px;
    background: #eee;
}

.custom-fields {
    display: flex;
    flex-direction: column;
    border-right: 1px solid #eee;
    box-sizing: content-box;
}

.custom-title {
    padding: 13px 10px;
    font-weight: bold;
    height: 45px;
    color: #000;
    border-bottom: 1px solid #eee;
}

.custom-form-title {
    padding: 0 10px;
    display: flex;
    align-items: center;

    .custom-form-title-input {
        margin-left: auto;
        font-weight: 500;

        button {
            margin-left: 10px;
        }
    }
}

.custom-content {
    flex: 1;
    height: 0;
    display: flex;

    .custom-fields {
        width: 220px;
    }

    .custom-form {
        position: relative;
        flex: 1;
        width: 0;
        display: flex;
        flex-direction: column;

        .custom-form-content {
            flex: 1;
            height: 0;
            position: relative;
        }

        .custom-empty {
            color: #a39f9f;
            position: absolute;
            text-align: center;
            letter-spacing: 1px;
            z-index: 0;
            left: 0;
            right: 0;
            top: 0;
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;

            i {
                font-size: 30px;
                color: #bebebe;
            }
        }

        // background: #bfbfbf;
    }
}

.custom-attr {
    width: 220px;
    display: flex;
    flex-direction: column;
    border-left: 1px solid #eee;
    box-sizing: content-box;
}

.draggable-container {
    min-height: 300px;
    display: inline-block;
    width: 100%;
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

.attr-item {
    margin-bottom: 10px;

    .attr-item-title {
        font-size: 13px;
        color: #000;
        padding-bottom: 2px;
    }

    ::v-deep(.el-radio) {
        margin-right: 13px;

        .el-radio__label {
            padding-left: 5px;
        }
    }
}

.filter-bottom {
    display: flex;
    text-align: left;
    align-items: center;
}

.filter-left {
    // flex: 1;
    overflow: hidden;
}

.filter-add-btn {
    flex: 1;
}

.add-filter {
    font-weight: bolder;
    color: #000;
    margin-right: 30px;
}

.input-range {
    display: flex;
    display: flex;
    align-items: center;

    span {
        margin: 0 4px;
        color: #474646;
    }
}

.dic-ref ::v-deep(.layout-container) {
    display: none;
}
</style>
