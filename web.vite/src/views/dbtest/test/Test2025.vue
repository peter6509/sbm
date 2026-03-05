<!--
*Author：jxx
 *Contact：283591387@qq.com
 *代碼由框架生成,任何更改都可能導致被代碼生成器覆盖
 *業務請在@/extension/dbtest/test/Test2025.jsx此處編寫
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
               :extend="extend">
    </view-grid>
</template>
<script>
    import extend from "@/extension/dbtest/test/Test2025.jsx";
    import { ref, defineComponent } from "vue";
    export default defineComponent({
        setup() {
            const table = ref({
                key: 'Id',
                footer: "Foots",
                cnName: '測試表2025',
                name: 'test/Test2025',
                newTabEdit: true,
                url: "/Test2025/",
                sortName: "Id"
            });
            const editFormFields = ref({"Name":"","Address":"","OrderCode":"","Age":""});
            const editFormOptions = ref([[{"title":"名字","field":"Name"}],
                              [{"title":"地址","field":"Address"}],
                              [{"dataKey":"訂單狀態","data":[],"title":"編號","field":"OrderCode","type":"select"}],
                              [{"title":"Age","field":"Age","type":"number"}]]);
            const searchFormFields = ref({"Name":"","Address":"","OrderCode":"","CreateDate":""});
            const searchFormOptions = ref([[{"title":"名字","field":"Name","type":"like"},{"title":"地址","field":"Address","type":"like"},{"dataKey":"訂單狀態","data":[],"title":"編號","field":"OrderCode"},{"title":"創建時間","field":"CreateDate","type":"datetime"}]]);
            const columns = ref([{field:'Id',title:'Id',type:'int',width:110,hidden:true,readonly:true,require:true,align:'left'},
                       {field:'Name',title:'名字',type:'string',link:true,width:120,align:'left'},
                       {field:'Address',title:'地址',type:'string',width:120,align:'left'},
                       {field:'OrderCode',title:'編號',type:'string',bind:{ key:'訂單狀態',data:[]},width:120,align:'left'},
                       {field:'Age',title:'Age',type:'int',width:80,align:'left'},
                       {field:'CreateDate',title:'創建時間',type:'datetime',width:110,align:'left'},
                       {field:'Creator',title:'創建人',type:'string',width:100,align:'left'}]);
            const detail = ref({columns:[]});
            const details = ref([]);
            return {
                table,
                extend,
                editFormFields,
                editFormOptions,
                searchFormFields,
                searchFormOptions,
                columns,
                detail,
                details
            };
        },
    });
</script>
