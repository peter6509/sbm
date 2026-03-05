<!--
這是生成的文件，事件處理、自定義配置，见移動端文档：表單、表格配置
Author:vol
QQ:283591387
Date:2024
-->
<template>
	<vol-view ref="viewRef" :table="table" :columns="columns" :table-data="tableData"
			  :searchFormFields="searchFormFields" :searchFormOptions="searchFormOptions" :editFormFields="editFormFields"
			  :editFormOptions="editFormOptions" @searchClick="loadData" @addClick="modelOpenBefore" :saveBefore="saveBefore"
			  :saveAfter="saveAfter" :delBefore="delBefore">
		<!--表格配置 -->
		<vol-table ref="tableRef" :ck="false" :index="false" :default-load="true" :url="tableUrl" @rowClick="modelOpenBefore" :loadBefore="searchBefore"
				   :loadAfter="searchAfter" :direction="direction" :titleField="table.titleField" :columns="columns"
				   :table-data="tableData">
		</vol-table>
	</vol-view>
</template>
<script setup>
    import options from "./sbm_sale_orderOptions.js";
	import { onLoad } from '@dcloudio/uni-app'
	import { ref, reactive, getCurrentInstance, defineEmits, defineExpose, defineProps, watch, nextTick } from "vue";
	const { proxy } = getCurrentInstance();
	//發起請求proxy.http.get/post
	//消息提示proxy.$toast()

	//表格顯示方式:list=列表顯示，horizontal=表格顯示
	const direction = ref('list')

	//vol-view组件
	const viewRef = ref(null);
	//table组件
	const tableRef = ref(null);

	//表格數據，可以直接获取使用
	const tableData = ref([]);

	//编輯、查詢、表格配置
	//要對table注册事件、格式化、按钮等，看移動端文档上的table示例配置
	//表單配置看移動端文档上的表單示例配置，searchFormOptions查詢配置，editFormOptions编輯配置
	const { table, searchFormFields, searchFormOptions, editFormFields, editFormOptions, columns } = reactive(options());
	const tableUrl = ref('api/' + table.tableName + '/getPageData');

	//查詢前方法，可以設置查詢条件(與生成页面文档上的searchBefore配置一致)
    const searchBefore = (params) => {
        if (viewRef.value && !params.wheres.length) {
            params.wheres = viewRef.value.getSearchParameters().wheres;
        }
        return true;
    }

	//查詢后方法，res返回的查詢结果
	const searchAfter = (res) => {
		nextTick(() => {
			viewRef.value.searchAfter(res);
		})
		return true;
	}

	//打開新建、编輯弹出框
	const modelOpenBefore = (row, index, obj, callback) => {
        //跳转到新页面编輯
        uni.navigateTo({
			url: "/pages/sbm/sbm_sale_order/sbm_sale_orderEdit?id=" + ((row || {})[table.key] || ''),
            fail(e) {
                console.log(e)
            }
		})

		//與上面二选一，當前页面弹出框编輯或跳转新页面编輯
		////新建操作
		//if (!row) {
		//	//這里可以設置默認值：editFormFields.字段=
		//	callback(true); //返回false，不會弹出框
		//	return;
		//}
		////编輯
		//viewRef.value.showEdit(row, index);
		////這里可以给弹出框字段設置或修改值：editFormFields.字段=
	}

	//新建、编輯保存前
	const saveBefore = (formData, isAdd, callback) => {
		callback(true); //返回false，不會保存
	}

	//新建、编輯保存后
	const saveAfter = (res, isAdd) => {}

	//主表删除前方法
	const delBefore = (ids, rows, result) => {
		return true;//返回false不會執行删除
	}

	//調用表格查詢
	const loadData = (params) => {
		//生成查詢条件
		params = params || viewRef.value.getSearchParameters();
		//params可以設置查詢条件
		tableRef.value.load(params);
	}

	//如果是其他页面跳转過来的，获取页面跳转参數
	onLoad((ops) => {})

	//監聽表單输入，做實時计算
	// watch(
	// 	() => editFormFields.字段,
	// 	(newValue, oldValue) => {
	// 	})
	defineExpose({
		//對外暴露數據
	})
</script>
<style lang="less" scoped>
</style>
