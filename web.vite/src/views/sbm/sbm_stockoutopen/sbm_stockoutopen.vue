<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/sbm/sbm_stockoutopen/sbm_stockoutopen.jsx或sbm_stockoutopen.vue文件编写
 *新版本支持vue或【表.jsx]文件编写業務,文档见:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
 -->
<template>
    <view-grid ref="grid"
               :columns="columns"
               :detail="detail"
               :details="details"
               :editFormFields="editFormFields"
               :editFormOptions="editFormOptions"
               :searchFormFields="searchFormFields"
               :searchFormOptions="searchFormOptions"
               :table="table"
               :extend="extend"
               :onInit="onInit"
               :onInited="onInited"
               :searchBefore="searchBefore"
               :searchAfter="searchAfter"
               :addBefore="addBefore"
               :addAfter="addAfter"
               :updateBefore="updateBefore"
               :rowClick="rowClick"
               :modelOpenBefore="modelOpenBefore"
               :modelOpenAfter="modelOpenAfter">
        <!-- 自定義组件數據槽扩展，更多數據槽slot见文档 -->
        <template #gridHeader>
        </template>
    </view-grid>
     <vol-table
      ref="gridA"
      :url="tableUrl"
      :key="gridAKey"
      :columns1="tableColumns"
      :pagination-hide="false"
      :height="400"
    ></vol-table>
</template>
<script setup lang="jsx">
    import extend from "@/extension/sbm/sbm_stockoutopen/sbm_stockoutopen.jsx";
    import viewOptions from './sbm_stockoutopen/options.js'
    import { ref, reactive, getCurrentInstance, watch, onMounted } from "vue";
    const gridA = ref(null) ;
    const grid = ref(null);
    const tableUrl = "api/sbm_stock_picking/getPageData" ;
    const gridAKey = ref(0);
    const columns1 = ref([
                 {"title":"調撥單單號","field":"name","readonly":true,"colSize":33.333},
                 {"title":"來源單號","field":"origin","readonly":true,"colSize":33.333},
                 {"dataKey":"成本中心","data":[],"title":"事業單位","required":true,"field":"company_id","colSize":33.333,"type":"select"}],
                 [{"dataKey":"調撥類型","data":[],"title":"調撥類型","required":true,"field":"picking_type","colSize":33.333,"type":"select"},
                 {"title":"調撥日期","field":"picking_date","colSize":33.333,"type":"date"},
                 {"dataKey":"夥伴公司名稱","data":[],"title":"合作夥伴公司","required":true,"field":"partner_company","colSize":33.333,"type":"select"}],
                [{"dataKey":"夥伴個人名稱","data":[],"title":"夥伴聯絡人","field":"partner_contact","colSize":33.333,"type":"select"}
]);
    
    const { proxy } = getCurrentInstance()
    //http請求，proxy.http.post/get
    const { table, editFormFields, editFormOptions, searchFormFields, searchFormOptions, columns, detail, details } = reactive(viewOptions())

    let gridRef;//對應[表.jsx]文件中this.使用方式一样
    //生成對象屬性初始化
    const onInit = async ($vm) => {
        gridRef = $vm;
        //與jsx中的this.xx使用一样，只需將this.xx改為gridRef.xx
        //更多屬性见：https://doc.volcore.xyz/docs/view-grid
    }
    //生成對象屬性初始化后,操作明细表配置用到
    const onInited = async () => {
    }
    const searchBefore = async (param) => {
        //界面查詢前,可以给param.wheres添加查詢参數
        //返回false，则不會執行查詢
        return true;
    }
    const searchAfter = async (rows, result) => {
        return true;
    }
    const addBefore = async (formData) => {
        //新建保存前formData為對象，包括明细表，可以给给表單設置值，自己输出看formData的值
        return true;
    }
  const addAfter = async (result) => {
        console.log('addAfter called', result);

        // 1. 主表重查
        await gridRef.search();
        await gridA.value.$refs.grid.refreshTree();

        // 2. 調用 gridA 的「重整」方法（等同於你手動按那顆 button）
        if (gridA.value && typeof gridA.value.$refs.refreshTree === 'function') {
            await gridA.value.$refs.grid.refreshTree();
        }

        return true;
        };












    const updateBefore = async (formData) => {
        //编輯保存前formData為對象，包括明细表、删除行的Id
        return true;
    }
    const rowClick = ({ row, column, event }) => {
        //查詢界面點击行事件
        // grid.value.toggleRowSelection(row); //單击行時选中當前行;
    }
    const modelOpenBefore = async (row) => {//弹出框打開后方法
        return true;//返回false，不會打開弹出框
    }
    const modelOpenAfter = (row) => {
        //弹出框打開后方法,設置表單默認值,按钮操作等
    }
    //監聽表單输入，做實時计算
    //watch(() => editFormFields.字段,(newValue, oldValue) => {	})
    //對外暴露數據
    defineExpose({})
</script>
