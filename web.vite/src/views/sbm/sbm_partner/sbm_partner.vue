<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/sbm/sbm_partner/sbm_partner.jsx或sbm_partner.vue文件編写
 *新版本支持vue或【表.jsx]文件編写業務,文档见:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
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
               :editBefore="editBefore"
               :addAfter="addAfter"
               :updateBefore="updateBefore"
               :rowClick="rowClick"
               :modelOpenBefore="modelOpenBefore"
               :modelOpenAfter="modelOpenAfter">
        <!-- 自定义组件數據槽扩展，更多數據槽slot见文档 -->
        <template #gridHeader>
        </template>
    </view-grid>
</template>
<script setup lang="jsx">
    import extend from "@/extension/sbm/sbm_partner/sbm_partner.jsx";
    import viewOptions from './sbm_partner/options.js'
    import { ref, reactive, getCurrentInstance, watch, onMounted } from "vue";
    import {useStore} from 'vuex' ;
    import { nextTick } from 'vue';
    const grid = ref(null);
    const { proxy } = getCurrentInstance()
    //http請求，proxy.http.post/get
    const { table, editFormFields, editFormOptions, searchFormFields, searchFormOptions, columns, detail, details } = reactive(viewOptions())

    let gridRef;//對應[表.jsx]文件中this.使用方式一样
    //生成對象屬性初始化
    const onInit = async ($vm) => {
        gridRef = $vm;
         gridRef.paginationHide = true;
        //禁用延迟加载(必填)
         gridRef.lazy = false;
        //树形结点的id字段(必填)
        gridRef.rowKey = 'id';//DepartmentId为表的主键id字段
        //父级id字段(必填)
        gridRef.rowParentField = "parent_id";//P
        gridRef.pagination.sortName = 'name';
        gridRef.pagination.order = 'asc';
        gridRef.columnIndex = true ;
        gridRef.detailOptions.pagination = {
            total : 0,
            size: 90,
            sizes:[60,90,120],
            sortName : "name",
            order : 'asc'
        }
        
      
    // 只要有任一筆 A == 1 就隱藏
      
        //與jsx中的this.xx使用一样，只需將this.xx改為gridRef.xx
        //更多屬性见：https://doc.volcore.xyz/docs/view-grid
    }
    //生成對象屬性初始化后,操作明细表配置用到
    let fieldMap = {};

   const onInited = () => {
    // 建立 fieldMap
    gridRef.showCustom = false;
    gridRef.showCustomSearch = false;

    gridRef.editFormOptions.forEach(row => {
        row.forEach(item => {
        fieldMap[item.field] = item;
        });
    });

    // 綁定 onChange（避免被覆蓋）
    if (fieldMap.partner_type) {
        fieldMap.partner_type.onChange = (value) => {
        applyPartnerType(value);
        };
    }
    };

    const applyPartnerType = (type) => {
    if (!fieldMap || !Object.keys(fieldMap).length) return;

    const companyHide = ['position', 'parent_id', 'mobile', 'email'];
    const personHide  = ['vat', 'address','bank_name','bank_code','bank_bname','bank_bcode','account_name','account_no','account_window','bus_type'];
    const controlFields = [...companyHide, ...personHide];

    const hideFields = type == 1 ? companyHide : personHide;

    controlFields.forEach(field => {
        if (fieldMap[field]) {
        fieldMap[field].hidden = hideFields.includes(field);
        }
    });
    };

    const searchBefore = async (param) => {
        //界面查詢前,可以给param.wheres添加查詢参數
        //返回false，則不會執行查詢
        return true;
    }

  

    const searchAfter = async (rows, result) => {
        
        return true;
    }
    const store = useStore()
    const addBefore = async (formData) => {
        // await store.dispatch('getDicData',['夥伴公司名稱']) ;
            
        //新建保存前formData為對象，包括明细表，可以给给表單設置值，自己输出看formData的值
         applyPartnerType(1); // 預設公司
        return true;
    }
     
     const addAfter = async (formData) => {
        // 重整資料字典
        gridRef.initDicKeys(true) ;   
        return true;
            
    }
//     const editAfter = (row) => {
//   // 一定要延遲，等表單 render 完
//   setTimeout(() => {
//     applyPartnerType(Number(row.partner_type));
//     gridRef.refreshEditForm();
//   }, 0);
// };
    const updateBefore = async (formData) => {
        //編輯保存前formData為對象，包括明细表、删除行的Id
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
     setTimeout(() => {
    applyPartnerType(Number(row.partner_type));
    gridRef.refreshEditForm();
  }, 0);
    gridRef.initDicKeys(true) ; 
};

// const currentRow = gridRef.currentRow;
// const istype1 = currentRow?.partner_type === 2;

// gridRef.setColumns(buildColumns(istype1));
//         //弹出框打開后方法,設置表單默認值,按钮操作等
//     }
    //監聽表單输入，做實時计算
    //watch(() => editFormFields.字段,(newValue, oldValue) => {	})
    //對外暴露數據
    defineExpose({})
</script>
