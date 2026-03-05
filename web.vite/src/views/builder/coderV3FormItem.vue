<template>
    <el-col class="el-col-item">
        <!-- {{typeField}}/{{item.formType}} -->
        <el-date-picker clearable style="width: 100%" v-if="
            ['date', 'datetime', 'month', 'year', 'time'].indexOf(item.formType) != -1
        " :disabled="!!(item.isReadDataset && typeField == 'editType')"
            :type="item.formType + (typeField == 'searchType' ? 'range' : '')" :placeholder="$ts('請選擇')"
            :value-format="getDateFormat(item)" start-placeholder="開始時間" end-placeholder="结束時間">
        </el-date-picker>

        <el-select v-else-if="
            ['select', 'selectList'].indexOf(item.formType) != -1
        " filterable :multiple="item.type != 'select'" :placeholder="$ts('請選擇')"
            :disabled="!!(item.isReadDataset && typeField == 'editType')" clearable>
            <el-option v-for="item2 in item.data" :key="item2.key" :label="$ts(item2.value)" :value="item2.key">
            </el-option>
        </el-select>

        <el-cascader style="width: 100%;" :props="{
            checkStrictly: true
        }" v-else-if="item.formType == 'cascader'" :options="formValue.cascader" />
        <el-tree-select :show-checkbox="true" check-on-click-node :check-strictly="true" :multiple="true"
            v-else-if="item.formType == 'treeSelect'" :data="formValue.cascader" />


        <el-radio-group v-else-if="item.formType == 'radio'"
            :disabled="!!(item.isReadDataset && typeField == 'editType')">
            <el-radio :key="1" label="選項一" :value="1"></el-radio>
            <!-- <el-radio :key="2" label="選項二" :value="2"></el-radio> -->
        </el-radio-group>
        <el-checkbox-group v-model="formValue.check" v-else-if="item.formType == 'checkbox'"
            :disabled="!!(item.isReadDataset && typeField == 'editType')">
            <el-checkbox :key="1" label="選項一" :value="1"></el-checkbox>
            <!-- <el-checkbox :key="2" label="選項二" :value="2"></el-checkbox> -->
        </el-checkbox-group>
        <el-input-number v-else-if="item.formType == 'number' || item.formType == 'decimal'"
            :disabled="!!(item.isReadDataset && typeField == 'editType')" controls-position="right"></el-input-number>


        <template v-else-if="item.formType == 'color'">
            <el-color-picker show-alpha :predefine="[
                '#ff4500',
                '#ff8c00',
                '#ffd700',
                '#90ee90',
                '#00ced1',
                '#1e90ff',
                '#c71585',
            ]" />
        </template>
        <el-rate v-else-if="item.formType == 'rate'" :value="3" />
        <div v-else-if="item.formType == 'file' || item.formType == 'excel'">
            <el-button><i class="el-icon-upload"></i>選擇文件</el-button>
        </div>

        <div v-else-if="item.formType == 'img'">
            <i class="el-icon-camera-solid" style="font-size:30px"></i>
        </div>
        <div v-else-if="item.formType == 'editor'">
            <vol-wang-editor :height="200"></vol-wang-editor>
        </div>

        <div v-else-if="item.formType == 'range'">
            <div v-if="item.range" class="input-range">
                <el-input-number class="mx-4" controls-position="right" />
                <span>-</span>
                <el-input-number class="mx-4" controls-position="right" />
            </div>
            <div v-else>
                <el-input-number style="width: 100%" placeholder="請輸入" v-model="item.value" class="mx-4"
                    controls-position="right" />
            </div>
        </div>
        <el-input v-else-if="item.formType == 'textarea'" type="textarea" :autosize="{
            minRows: 0,
            maxRows: 1,
        }" :disabled="!!(item.isReadDataset && typeField == 'editType')" placeholder="請輸入内容"></el-input>
        <el-input v-else placeholder="請輸入内容" :disabled="!!(item.isReadDataset && typeField == 'editType')"></el-input>
    </el-col>
</template>
<script lang="jsx" setup>
import { ref, getCurrentInstance, defineAsyncComponent } from "vue";
const VolWangEditor = defineAsyncComponent(() =>
    import("@/components/editor/VolWangEditor.vue")
);
const { proxy } = getCurrentInstance();
const pros = defineProps({
    typeField: {
        type: String,
        default: ""
    },
    item: {
        type: Object,
        default: () => {
            return {
                searchType: "",
                isReadDataset: 0
            }
        }
    }
})
const formValue = ref({
    cascader: [
        {
            value: 'guide',
            label: '原材料',
            children: [
                {
                    value: '助焊剂',
                    label: '助焊剂'
                }, {
                    value: '焊條1',
                    label: '焊條1',
                },
                {
                    value: '焊條2',
                    label: '焊條2',
                }]
        },
        {
            value: '半成品',
            label: '半成品',
            children: [
                {
                    value: '引線框架',
                    label: '引線框架',

                }, {
                    value: '覆铜板',
                    label: '覆铜板',

                }]
        }]
})

const getDateFormat = (item) => {
    if (item.format) {
        return item.format;
    }
    if (item.type == "month") {
        return "YYYY-MM";
    }
    return item.type == "date" ? "YYYY-MM-DD" : "YYYY-MM-DD HH:mm:ss";
};


const isNubmer = (type) => {
    return ["decimal", "number", "int"].indexOf(type) != -1;
};




defineExpose({});
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

.el-col-item ::v-deep(.el-radio-group) {
    margin-top: -4px;

    .el-radio {
        margin-right: 10px;
    }
}

.el-col-item ::v-deep(.el-checkbox-group) {
    margin-top: -4px;

    .el-checkbox {
        margin-right: 10px;
    }
}
</style>
