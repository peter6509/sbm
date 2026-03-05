<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/sbm/sbm_stock_picking/sbm_stock_picking.jsx或sbm_stock_picking.vue文件编写
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
               :updateBefore="updateBefore"
               :rowClick="rowClick"
               :modelOpenBefore="modelOpenBefore"
               :modelOpenAfter="modelOpenAfter">
        <!-- 自定義组件數據槽扩展，更多數據槽slot见文档 -->
        <template #gridHeader>
        </template>
    </view-grid>
     <div class="outopen-ref">
       <OutOpen class="outopen-ref" ref="outopenRef" />
     </div>
</template>
<script setup lang="jsx">
    import extend from "@/extension/sbm/sbm_stock_picking/sbm_stock_picking.jsx";
    import viewOptions from './sbm_stock_picking/options.js'
    import { ref, reactive, getCurrentInstance, watch, onMounted } from "vue";
    import OutOpen from '@/views/sbm/sbm_stockoutopen/sbm_stockoutopen.vue';
    const grid = ref(null);
    const { proxy } = getCurrentInstance();
    const outopenRef = ref();
    //http請求，proxy.http.post/get
    const { table, editFormFields, editFormOptions, searchFormFields, searchFormOptions, columns, detail, details } = reactive(viewOptions())

    let gridRef;//對應[表.jsx]文件中this.使用方式一样
    //生成對象屬性初始化
    const onInit = async ($vm) => {
        gridRef = $vm;
        gridRef.pagination.sortName = 'name';
        gridRef.pagination.order = 'desc';
        gridRef.columnIndex = true ;
        gridRef.detailOptions.pagination = {
            total : 0,
            size: 30,
            sizes:[30,60,90],
            sortName : "move_item",
            order : 'asc'
        }
        let compOption,contactOption ;
        gridRef.editFormOptions.forEach((options) => {
        options.forEach((item) => {
        if (item.field == 'partner_company'){
           compOption = item ;
           }   
        if (item.field == 'partner_contact'){
           contactOption = item ;
            }
        });
        });
       compOption.onChange = (value,option) => {
       let url="api/sbm_partner/Getcontact?code=" + value ;
       gridRef.http.get(url,{},true).then(result =>{
       contactOption.data = result ;          
             })
         };
         gridRef.buttons.splice(1, 0, {
            name: '出貨單創建',
            icon: 'el-icon-document',
            type: 'primary',
            plain: true,
            onClick: () => {
            // 打開新開案表單（不立即刷新，交由自動刷新或保存成功後刷新）
            outopenRef.value?.$refs?.grid?.add?.();
            gridRef.search();
            },
         });
          gridRef.buttons.splice(1, 0, {
            name: '重整畫面',
            icon: 'el-icon-document',
            type: 'primary',
            plain: true,
            onClick: () => {
            // 打開新開案表單（不立即刷新，交由自動刷新或保存成功後刷新）
               refreshTree();
            },
         });
        //與jsx中的this.xx使用一样，只需將this.xx改為gridRef.xx
        //更多屬性见：https://doc.volcore.xyz/docs/view-grid
    }
    //生成對象屬性初始化后,操作明细表配置用到
    const refreshTree = async () => {
        gridRef.search();
     };
    const onInited = async () => {
         gridRef.showCustom = false ;
         gridRef.showCustomSearch = false ;
         
          gridRef.detailOptions.buttons.forEach((x) => {
            if (x.name === '導入') {x.hidden = true};
            if (x.name === '導出') {x.hidden = true};
         });
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
    defineExpose({refreshTree})
</script>
<style scoped lang="less">
/* 默認隐藏引用的頁面 */
.outopen-ref ::v-deep(.layout-container) {
  display: none;
}
</style>
