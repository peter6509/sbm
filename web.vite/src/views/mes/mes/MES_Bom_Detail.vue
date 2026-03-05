<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/mes/mes/MES_Bom_Detail.jsx或MES_Bom_Detail.vue文件編寫
 *新版本支持vue或【表.jsx]文件編寫業務,文檔見:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
 -->
<template>
    <view-grid ref="grid" :columns="columns" :detail="detail" :details="details" :editFormFields="editFormFields"
        :editFormOptions="editFormOptions" :searchFormFields="searchFormFields" :searchFormOptions="searchFormOptions"
        :table="table" :extend="extend" :onInit="onInit" :onInited="onInited" :searchBefore="searchBefore"
        :searchAfter="searchAfter" :addBefore="addBefore" :updateBefore="updateBefore" :rowClick="rowClick"
        :modelOpenBefore="modelOpenBefore" :modelOpenAfter="modelOpenAfter">
        <!-- 自定義组件數據槽擴展，更多數據槽slot見文檔 -->
        <template #gridHeader>
        </template>
    </view-grid>
</template>
<script setup lang="jsx">
import extend from "@/extension/mes/mes/MES_Bom_Detail.jsx";
import viewOptions from './MES_Bom_Detail/options.js'
import { ref, reactive, getCurrentInstance, watch, onMounted } from "vue";
const grid = ref(null);
const { proxy } = getCurrentInstance()
//http請求，proxy.http.post/get
const { table, editFormFields, editFormOptions, searchFormFields, searchFormOptions, columns, detail, details } = reactive(viewOptions())

let gridRef;//對應[表.jsx]文件中this.使用方式一樣
//生成對象屬性初始化
const onInit = async ($vm) => {
    gridRef = $vm;
    //默認不加載數據
    gridRef.load = false
}
//隐藏高级查詢按鈕
const onInited = async () => {
    gridRef.height = 200;
    gridRef.buttons.forEach(x => {
        if (x.name == '高级查詢') {
            x.hidden = true;
        }
    })
}
const searchBefore = async (param) => {
    //界面查詢前,可以给param.wheres添加查詢参數
    //返回false，則不會執行查詢
    const bomRows = getBomSelectRow();
    if (!bomRows.length) {
        proxy.$message.error('請先選擇【制造BOM】數據')
        return false;
    }
    //查詢前獲取bom的id，查詢明细表
    param.wheres.push({ name: "BomId", value: bomRows[0].BomId })
    return true;
}
const searchAfter = async (rows, result) => {
    return true;
}
const addBefore = async (formData) => {
    //明细表新建前獲取【制造bom】選中的主鍵BomId值(若不獲取，寫入的明细表數據無法知道屬于哪個nom)
    const BomId = getBomSelectRow()[0].BomId;
    formData.mainData['BomId'] = BomId
    return true;
}
const updateBefore = async (formData) => {
    //編輯保存前formData為對象，包括明细表、删除行的Id
    return true;
}
const rowClick = ({ row, column, event }) => {
    //查詢界面點擊行事件
}
//獲取MES_Bom_Main.vue中缓存的方法，讀取bom主表選中的行，保存數據時可以知道是哪條bom數據的明细
const getBomSelectRow = () => {
    const fn = proxy.base.getItem('getBomSelectRow');
    return fn();
}
const modelOpenBefore = async (row) => {//彈出框打開后方法
    //獲取MES_Bom_Main.vue中缓存的方法，讀取bom主表選中的行，保存數據時可以知道是哪條bom數據的明细
    const bomRows = getBomSelectRow();
    if (!bomRows.length) {
        proxy.$message.error('請先選擇【制造BOM】數據')
        return false;
    }
    return true;//返回false，不會打開彈出框
}
const modelOpenAfter = (row) => {
    //彈出框打開后方法,設置表單默認值,按鈕操作等
}
//監聽表單輸入，做實時計算
//watch(() => editFormFields.字段,(newValue, oldValue) => {	})
//對外暴露數據
defineExpose({})
</script>
