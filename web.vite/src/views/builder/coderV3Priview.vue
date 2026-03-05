<template>
    <div class="priview-grid">
        <div class="priview-grid-content">
            <view-grid ref="grid" :onInit="onInit" :onInited="onInited" :searchBefore="searchBefore"
                :searchAfter="searchAfter" :addBefore="addBefore" :updateBefore="updateBefore" :rowClick="rowClick"
                :modelOpenBefore="modelOpenBefore" :modelOpenAfter="modelOpenAfter" :detailSortEnd="detailSortEnd"
                :priview="true" :coderTableId="coderTableId">
                <!-- 自定義组件數據槽擴展，更多數據槽slot見文檔 -->
                <template #gridBody>
                    <div style="margin-bottom: 5px;">
                        <el-alert style="margin-bottom: -1px"
                            title="1、預覽看不到最新效果：請點保存再預覽； 2、打開頁面提示未找到請求地址：代碼生成器點擊[生成model]、[生成業務類]，然后重開后台"
                            class="alert-primary" :closable="false"></el-alert>
                    </div>
                </template>
            </view-grid>
        </div>
    </div>
</template>
<script setup lang="jsx">
import { ref, reactive, getCurrentInstance, watch, onMounted } from 'vue'
const grid = ref(null)
const { proxy } = getCurrentInstance()
//http請求，proxy.http.post/get

const coderTableId = ref((proxy.$route.query.id * 1) || 0)


let gridRef //對應[表.jsx]文件中this.使用方式一樣
//生成對象屬性初始化
const onInit = async ($vm) => {
    gridRef = $vm
    //這里會影响表格滚動條顯示
    //gridRef.setFixedSearchForm(true);

    //與jsx中的this.xx使用一樣，只需將this.xx改為gridRef.xx
    //更多屬性見：https://doc.volcore.xyz/docs/view-grid
}
//生成對象屬性初始化后,操作明细表配置用到
const onInited = async () => {
  // gridRef.tableMaxHeight = gridRef.height- 125

    // gridRef.height = 0;
}
let hasWheres = false;
const searchBefore = async (param) => {
    //界面查詢前,可以给param.wheres添加查詢参數
    //返回false，則不會執行查詢
    hasWheres = !!param.wheres.length
    return true
}
const searchAfter = async (rows, result) => {
    if (!rows.length && !hasWheres) {
        rows.push(...[{}, {}, {}])
    }
    return true
}
const addBefore = async (formData) => {
    //新建保存前formData為對象，包括明细表，可以给给表單設置值，自己輸出看formData的值
    return true
}
const updateBefore = async (formData) => {
    //編輯保存前formData為對象，包括明细表、删除行的Id
    return true
}
const footerRef = ref()
//查詢界面點擊行事件,加載明细表數據
const rowClick = ({ row, column, event }) => {
    // grid.value.toggleRowSelection(row); //單擊行時選中當前行;
    footerRef.value.gridRowClick(row)
}
const modelOpenBefore = async (row) => {
    //彈出框打開后方法
    return true //返回false，不會打開彈出框
}
const modelOpenAfter = (row) => {
    //彈出框打開后方法,設置表單默認值,按鈕操作等
}
//明细表拖動排序
const detailSortEnd = (rows, newIndex, oldIndex, table) => {
    rows.forEach((x, index) => {
        x.RouteSequence = index + 1
    })
}
//監聽表單輸入，做實時計算
//watch(() => editFormFields.字段,(newValue, oldValue) => {	})
//對外暴露數據
defineExpose({})
</script>
<style scoped lang="less">
.priview-grid {
    position: absolute;
    height: 100%;
    width: 100%;
    padding: 15px;
    background: #f8f7f7;

}

.priview-grid-content {
    padding: 5px 5px 20px 5px;
    background: #fff;
    border-radius: 5px;
    border: 1px solid #e7e7e7;
    max-width: 1400px;
    align-items: center;
    margin: 0 auto;
}
</style>
