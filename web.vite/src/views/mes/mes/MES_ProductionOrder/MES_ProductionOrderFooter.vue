<template>
    <div style="padding: 0 4px;border-top: 10px solid #eee;">
        <vol-title style="padding: 10px;" title="訂單明细"></vol-title>
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
    params.order='asc'
    return callback(true);
}
const tableRef = ref();
const gridRowClick = (row) => { //主表點擊行加載明细表
    //load方法可参照voltable组件api文檔
    //根據主表id查詢明细表
    tableRef.value?.load({ value: row.OrderID })
}

const url = ref('api/MES_ProductionOrder/getDetailPage')//指定從某個接口獲取table數據
//更多table配置見文檔：http://doc.volcore.xyz/table
//明细表格配置，從生成的vue文件中可以複制過来
const columns = reactive([{field:'PlanDetailID',title:'計划明细ID',type:'string',width:90,hidden:true},
                       {field:'OrderID',title:'訂單ID',type:'string',width:90,hidden:true},
                       {field:'MaterialCode',title:'物料編碼',width:90},
                       {field:'MaterialName',title:'物料名稱',type:'string',width:90},
                       {field:'MaterialSpecification',title:'物料規格',type:'string',width:110},
                       {field:'Unit',title:'單位',type:'string',bind:{ key:'物料單位',data:[]},width:80},
                       {field:'PlanQuantity',title:'訂單數量',type:'int',summary:true},
                       {field:'Creator',title:'創建人',type:'string',width:90},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:150}])

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
