<!--
編輯頁面
Author:vol
QQ:283591387
Date:2024
-->
<template>
	<vol-edit ref="editRef" :id="id" :table="table" :detail="detail" :details="details" :editFormFields="editFormFields"
			  :editFormOptions="editFormOptions" :saveBefore="saveBefore" :labelWidth="70" labelPosition="right"
			  :saveAfter="saveAfter" :delBefore="delBefore" @onChange="onChange" :delRowBefore="delRowBefore"
			  :delRowAfter="delRowAfter" :loadFormAfter="loadFormAfter">
		<!--<template #header>
			頁面slot内容
		</template>
		<template #content>
			頁面slot内容
		</template> -->
	</vol-edit>
</template>
<script setup>
	import options from "./sbm_partnerOptions.js";
	import { onLoad } from '@dcloudio/uni-app'
	import { ref, reactive, getCurrentInstance, defineEmits, defineExpose, defineProps, watch, nextTick } from "vue";

	//發起請求proxy.http.get/post
	//消息提示proxy.$toast()

	const props = defineProps({ id: '' })
	const id = ref(props.id); //編輯的主鍵值
	const isAdd = !id.value;//當前是新建還是編輯

	const { proxy } = getCurrentInstance();
	//發起請求proxy.http.get/post
	//消息提示proxy.$toast()

	//vol-edit组件對象
	const editRef = ref(null);

	//編輯、查詢、表格配置
	//要對table注册事件、格式化、按鈕等，看移動端文檔上的table示例配置
	//表單配置看移動端文檔上的表單示例配置，searchFormOptions查詢配置，editFormOptions編輯配置
	const { table, editFormFields, editFormOptions, detail, details } = reactive(options());

	//下拉框、日期、radio選擇事件
	const onChange = (field, value, item, data) => { }

	//表單數據加載后方法
	const loadFormAfter = (result) => {
		//isAdd通過判斷是新還是編輯狀態，可以頁面加載后設置一些其他默認值(新建/編輯都可使用)
		//editFormFields.字段=值;
	}

	//新建、編輯保存前,isAdd判斷當前是編輯還是新建操作
	const saveBefore = (formData, isAdd, callback) => {
		callback(true); //返回false，不會保存
	}

	//新建、編輯保存后
	const saveAfter = (res, isAdd) => { }

	//主表删除前方法
	const delBefore = async (id, fields) => {
		return true; //返回false不會執行删除
	}
	//明细表删除前
	const delRowBefore = async (rows, table, ops) => {
		// await proxy.http.post(url,{}).then(x => {
		// })
		return true
	}

	//明细表删除后前
	const delRowAfter = (rows, table, ops) => {
	}

	//如果是其他頁面跳轉過来的，獲取頁面跳轉参數
	onLoad((ops) => { })

	//監聽表單輸入，做實時計算
	// watch(
	// 	() => editFormFields.字段,
	// 	(newValue, oldValue) => {
	// 	})
</script>
<style lang="less" scoped>
</style>
