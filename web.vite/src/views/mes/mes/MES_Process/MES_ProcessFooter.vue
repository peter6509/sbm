<template>
    <div style="padding: 0 4px;border-top: 10px solid #eee;">
        <vol-title style="padding: 10px;" title="工艺路線"></vol-title>
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
    tableRef.value?.load({ value: row.ProcessID, sort: "RouteSequence" })
}

const url = ref('api/MES_Process/getDetailPage')//指定從某個接口獲取table數據
//更多table配置見文檔：http://doc.volcore.xyz/table
//明细表格配置，從生成的vue文件中可以複制過来
const columns = reactive( [{field:'RouteID',title:'路線ID',type:'string',width:90,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'ProcessID',title:'工序',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ProductID',title:'产品ID',type:'string',width:90,hidden:true,align:'left'},
                       {field:'ProductName',title:'路線名稱',type:'string',width:90,edit:{type:''},require:true,align:'left'},
                       {field:'RouteSequence',title:'路線顺序',type:'int',width:90,edit:{type:''},require:true,align:'left'},
                       {field:'RouteDescription',title:'路線描述',type:'string',width:90,edit:{type:''},align:'left'},
                       {field:'PreProcessID',title:'前工序',type:'string',bind:{ key:'工序',data:[]},width:90,edit:{type:'select'},align:'left'},
                       {field:'NextProcessID',title:'后工序',type:'string',bind:{ key:'工序',data:[]},width:90,edit:{type:'select'},align:'left'},
                       {field:'RouteStatus',title:'啟用狀態',type:'string',bind:{ key:'enable',data:[]},width:90,edit:{type:'select'},align:'left'},
                       {field:'RouteResponsible',title:'路線责任人',type:'string',width:90,hidden:true,align:'left'},
                       {field:'CreateID',title:'創建人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:90,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:90,align:'left'},
                       {field:'ModifyID',title:'修改人ID',type:'int',width:90,hidden:true,align:'left'},
                       {field:'Modifier',title:'修改人',type:'string',width:90,align:'left'},
                       {field:'ModifyDate',title:'修改時間',type:'datetime',width:90,align:'left'}])

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
