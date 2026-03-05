<template>
    <div style="padding: 0 4px;border-top: 10px solid #eee;">
        <vol-title style="padding: 10px;" title="产線設備"></vol-title>
        <!-- <h3>
            <i class="ivu-icon ivu-icon-ios-information-circle-outline"></i>訂單明细
        </h3> -->
        <div style="padding:10px;background: white;padding-top: 0;">
            <!-- 更多table配置見文檔：http://doc.volcore.xyz/table 或使用element plus原生table -->
            <vol-table ref="tableRef" :ck="false" :loadKey="true" :columns="columns" :pagination-hide="true"
                :height="230" :defaultLoadPage="false" @loadBefore="loadBefore" :url="url"
                :row-index="true"></vol-table>
        </div>
    </div>
</template>

<script setup>
import { reactive, ref } from 'vue';
const loadBefore = (params, callback) => {//表格數據加載時觸發
    //更多table配置見文檔：http://doc.volcore.xyz/table
    //   這里也可以設置一些查詢條件
    return callback(true);
}
const tableRef = ref();
const gridRowClick = (row) => { //主表點擊行加載明细表
    //load方法可参照voltable组件api文檔
    tableRef.value?.load({ value: row.ProductionLineID, sort: "CreateDate" })
}

const url = ref('api/MES_ProductionLine/getDetailPage')//指定從某個接口獲取table數據
//更多table配置見文檔：http://doc.volcore.xyz/table
//明细表格配置，從生成的vue文件中可以複制過来
const columns = reactive([{ field: 'DeviceID', title: '設備ID', type: 'string', width: 90, hidden: true, readonly: true,  align: 'left' },
{ field: 'DeviceName', title: '設備名稱', type: 'string', width: 90, edit: { type: '' },  align: 'left' },
{ field: 'DeviceModel', title: '設備型號', type: 'string', width: 90, edit: { type: '' }, align: 'left' },
{ field: 'Manufacturer', title: '制造商', type: 'string', width: 90, edit: { type: '' }, align: 'left' },
{ field: 'PurchaseDate', title: '购买日期', type: 'date', width: 90, edit: { type: 'date' }, align: 'left' },
{ field: 'WarrantyDate', title: '保修日期', type: 'date', width: 90, edit: { type: 'date' }, align: 'left' },
{ field: 'Status', title: '設備狀態', type: 'string', width: 90, edit: { type: '' }, align: 'left' },
{ field: 'LocationOnLine', title: '線上位置', type: 'string', width: 90, hidden: true, edit: { type: '' }, align: 'left' },
{ field: 'Remarks', title: '備註信息', type: 'string', width: 90, edit: { type: '' }, align: 'left' },
{ field: 'CreateID', title: '創建人ID', type: 'int', width: 90, hidden: true, align: 'left' },
{ field: 'Creator', title: '創建人', type: 'string', width: 90, align: 'left' },
{ field: 'CreateDate', title: '創建時間', type: 'datetime', width: 120, align: 'left' }])

defineExpose({ gridRowClick })
</script>
<style scoped>
h3 {
    font-weight: 500;
    padding-left: 10px;
    background: white;
    margin-top: 8px;
    padding-bottom: 5px;
}
</style>
