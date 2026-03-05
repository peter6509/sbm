<template>
  <vol-edit
    ref="edit"
    :keyField="key"
    :tableName="tableName"
    :tableCNName="tableCNName"
    :labelWidth="labelWidth"
    :formFields="editFormFields"
    :formOptions="editFormOptions"
    :detail-height="detailHeight"
    :detail="detail"
    :details="details"
  >
    <template #header>
      <!-- 自定義數據槽顯示 -->
    </template>
    <template #content>
      <!-- 自定義數據槽顯示 -->
    </template>
    <template #foooter>
      <!-- 自定義數據槽顯示 -->
    </template>
  </vol-edit>
</template>
<script lang="jsx">
export default { name: "#Demo_Catalog_edit" };
</script>
<script setup lang="jsx">
//新窗口編輯默認使用的纯vue3語法,文檔参考：http://doc.volcore.xyz/edit/guid.html
import editOptions from "./options.js";
import { ref, reactive, getCurrentInstance, nextTick } from "vue";
import { useRouter, useRoute } from "vue-router";
const emit = defineEmits([]);
alert(emit);
const props = defineProps({});

//發起請求proxy.http.get/post
//消息提示proxy.$message.success()
const { proxy } = getCurrentInstance();

//這里表單與明细表参數，具體信息看options.js里面
const {
  key,
  tableName,
  tableCNName,
  editFormFields,
  editFormOptions,
  detail,
  details,
} = reactive(editOptions());

//獲取路由参數
const route = useRoute();
//是否新建操作
let isAdd = !route.query.id;

//vol-edit组件對象
const edit = ref(null);

//表單標簽文字顯示宽度
const labelWidth = 90;

//明细表高度
const detailHeight = 240;

defineExpose({});
</script>
