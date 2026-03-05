<!--
這是生成的文件，事件處理、自定義配置，見移動端文檔：表單、表格配置
Author:vol
QQ:283591387
Date:2024
-->
<template>
	<vol-view ref="viewRef" :table="table" :columns="columns" :table-data="tableData"
		:searchFormFields="searchFormFields" :searchFormOptions="searchFormOptions" :editFormFields="editFormFields"
		:editFormOptions="editFormOptions" @searchClick="loadData" @addClick="modelOpenBefore">
		<!--表格配置 -->
		<vol-alert>生成頁面配置字段分组顯示,更多配置参照表格示例</vol-alert>
		<vol-table ref="tableRef" :ck="false" :index="true" :url="tableUrl" @rowClick="modelOpenBefore"
			:loadBefore="searchBefore" :label-width="60" :loadAfter="searchAfter" :direction="direction" :titleField="table.titleField"
			:columns="columns" :table-data="tableData">
				<template #bottom="scope">
					<view class="btns" style="margin-top: 20rpx;">
						<view class="fx-3"></view>
						<view class="btn">
							<vol-button size="small" type="default"
								@click="delBtnClick(scope.data.row,scope.data.rowIndex)">删除
							</vol-button>
						</view>
						<view class="btn">
							<vol-button size="small" type="primary"
								@click="viewBtnClick(scope.data.row,scope.data.rowIndex)">查看
							</vol-button>
						</view>
					</view>
				</template>
		</vol-table>
	</vol-view>
</template>
<script setup>
	import options from "./Demo_OrderOptions.js";
	import {
		onLoad
	} from '@dcloudio/uni-app'
	import {
		ref,
		reactive,
		getCurrentInstance,
		defineEmits,
		defineExpose,
		defineProps,
		watch,
		nextTick
	} from "vue";
	const {
		proxy
	} = getCurrentInstance();
	//發起請求proxy.http.get/post
	//消息提示proxy.$toast()

	//表格顯示方式:list=列表顯示，horizontal=表格顯示
	const direction = ref('list')

	//vol-view组件
	const viewRef = ref(null);
	//table组件
	const tableRef = ref(null);

	//表格數據，可以直接獲取使用
	const tableData = ref([]);

	//編輯、查詢、表格配置
	//要對table注册事件、格式化、按鈕等，看移動端文檔上的table示例配置
	//表單配置看移動端文檔上的表單示例配置，searchFormOptions查詢配置，editFormOptions編輯配置
	const {
		table,
		searchFormFields,
		searchFormOptions,
		editFormFields,
		editFormOptions,
		columns
	} = reactive(options());
	const tableUrl = ref('api/' + table.tableName + '/getPageData');
    
	//注意上面的direction屬性設置為list,上面vol-table標簽加上label-width指定文字寬度
	
	//清空原有數據，重新手動配置格式
    columns.splice(0)
	columns.push(...[{field:'OrderNo',title:'訂單編號',type:'string',link:true,width:130,readonly:true},
                       [{field:'OrderType',title:'訂單類型',type:'int',bind:{ key:'訂單類型',data:[]},width:90},
                       {field:'TotalPrice',title:'訂單價格',type:'decimal',width:70}],
                       [{field:'TotalQty',title:'訂單數量',type:'int',width:80},
                       {field:'OrderDate',title:'訂單日期',type:'date',width:95}],
                       [{field:'Customer',title:'客户姓名',type:'string',width:80,readonly:true},
                       {field:'PhoneNo',title:'聯系方式',type:'string',width:110,readonly:true}],
                       [{field:'OrderStatus',title:'訂單狀態',type:'int',bind:{ key:'訂單狀態',data:[]},width:90},
                       {field:'Creator',title:'創建人',type:'string',width:80}],
                       {field:'CreateDate',title:'創建時間',type:'datetime',align:'left'},
					   {field:'Remark',title:'備註',type:'textarea',align:'left'}])

	//查詢前方法，可以設置查詢條件(與生成頁面文檔上的searchBefore配置一致)
	const searchBefore = (params) => {
		return true;
	}

	//查詢后方法，res返回的查詢结果
	const searchAfter = (res) => {
		nextTick(() => {
			viewRef.value.searchAfter(res);
		})
		return true;
	}

	//打開新建、編輯彈出框
	const modelOpenBefore = (row, index, obj, callback) => {
		//跳轉到新頁面編輯
		uni.navigateTo({
			url: "/pages/dbtest/Demo_Order/Demo_OrderEdit?id=" + ((row || {})[table.key] || ''),
			fail(e) {
				console.log(e)
			}
		})
	}
	//查看按鈕
	const viewBtnClick=(row,rowIndex)=>{
		modelOpenBefore(row);
	}
	//删除按鈕
	const delBtnClick=(row,rowIndex)=>{
		viewRef.value.del([row]);
	}

	//調用表格查詢
	const loadData = (params) => {
		//生成查詢條件
		params = params || viewRef.value.getSearchParameters();
		//params可以設置查詢條件
		tableRef.value.load(params);
	}

	//如果是其他頁面跳轉過来的，獲取頁面跳轉参數
	onLoad((ops) => {})

	defineExpose({
		//對外暴露數據
	})
</script>
<style lang="less" scoped>
	.summary{
		padding: 20rpx 0;
		.txt{
			margin-left: 20rpx;
			font-size: 26rpx;
		}
	}
</style>