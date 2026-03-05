<template>
    <vol-box :lazy="true" v-model="model" title="選擇客户" :width="1200" :padding="0">
        <div><customer ref="customerRef"></customer></div>
        <template #footer>
            <div> <el-button type="primary" @click="onSelect">確認</el-button></div>
        </template>
    </vol-box>
</template>
<script setup>
import { ref, getCurrentInstance, nextTick } from 'vue'
const { proxy } = getCurrentInstance();
/******注意：如果出現提示没有權限的問題，見后台開發文檔上的【重寫后台權限】*****/
//直接引用生成的頁面(也可以單獨用table配置，單獨配置下見下面【編輯彈出框明细表選擇數據】)
import customer from '@/views/mes/mes/MES_Customer.vue';
const customerRef = ref();
const model = ref(false)
//回調
const emit = defineEmits('onSelectCustom')
const open = () => {
    //打開主表選擇數據
    model.value = true;
    //打開時刷新界面數據
    nextTick(() => { customerRef.value?.$refs.grid?.search()});
}
//選擇數據
const onSelect = () => {
    //主表的選擇數據回寫到主表的表單上
    let rows = customerRef.value.$refs.grid.getSelectRows();
    if (!rows.length) {
        return proxy.$message.error('請選擇數據');
    }
    model.value=false;
    emit('onSelectCustom', rows)
}
//暴露選擇彈出框方法
defineExpose({open})
</script>
<style lang="less" scoped>
.search-form {
    display: flex;
    padding: 10px;
    line-height: 34px;
    button {
        margin-left: 10px;
    }
}
</style>