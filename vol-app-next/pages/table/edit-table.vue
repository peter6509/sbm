<template>
	<view class="table-demo">
		<view style="padding:0">
			<vol-alert type="primary">
				<view>1、支持表格行内直接編輯，並支持分组編輯</view>
				<view>2、下拉框、日期等選擇事件聯動给其他字段設置值</view>
				<view>3、注意：大批量返回的數據不要使用行内編輯</view>
			</vol-alert>
		</view>
		<view class="search">
			<u-search @search="loadData" placeholder="請輸商品名稱" v-model="goodsName" @custom="loadData" @clear="loadData"
				:showAction="true" clearabled actionText="搜索"></u-search>
		</view>
		<view class="table-content">
			<vol-table :readonly="false" ref="tableRef" index url="api/Demo_Goods/getPageData" :height='0'
				direction="list" :loadBefore="loadBefore" :loadAfter="loadAfter" :columns="columns"
				:table-data="tableData" @onChange="onChange">
				<!-- 	頭部自定義按鈕部分 -->
				<template #header="scope">
					<view class="scope-header">
						<view style="font-size: 26rpx;">
							第[{{scope.data.rowIndex+1}}]行
							<!-- 	這里可顯示按鈕在顶部 -->
							<!-- 	<vol-button size="small" type="primary"
								@click="rowBtnClick(scope.data.row,scope.data.rowIndex)">按鈕[{{scope.data.rowIndex+1}}]
							</vol-button> -->
						</view>
					</view>
				</template>
				<!--底部按鈕位置 -->
				<template #bottom="scope">
					<view class="scope-header">
						<view class="fx-1" style="text-align: left;">￥<text
								class="scope-price">{{scope.data.row.Price}}</text></view>
						<view class="btns">
							<view class="btn">
								<vol-button size="small" type="error"
									@click="delClick(scope.data.row,scope.data.rowIndex)">删除
								</vol-button>
							</view>
							<view class="btn">
								<vol-button size="small" type="default"
									@click="saveClick(scope.data.row,scope.data.rowIndex)">保存
								</vol-button>
							</view>
						</view>
					</view>
				</template>
				<view class="add-btn">
					<view class="btn">
						<vol-button type="primary" @click="addClick">添加數據</vol-button>
					</view>
				</view>

			</vol-table>
		</view>
	</view>
</template>

<script setup>
	import {
		ref,
		defineProps,
		defineEmits,
		defineExpose,
		computed,
		getCurrentInstance,
		nextTick
	} from 'vue'

	const {
		proxy
	} = getCurrentInstance();

	const goodsName = ref('')
	const tableRef = ref(null);

	//删除行
	const delClick = (row, index) => {
		uni.showModal({
			title: '提示',
			content: '確定要删除數據嗎?',
			success: (res) => {
				if (res.confirm) {
					tableData.value.splice(index, 1)
					proxy.$toast('删除成功')
				}
			}
		});
	}
	//保存
	const saveClick = (row, index) => {
		console.log(row)
		proxy.$toast('保存成功')
	}

	//表格加載前設置條件,需要在vol-tables標簽配置url才會生效
	const loadBefore = (params) => {
		//訂單編號搜索
		params.wheres.push({
			name: "GoodsName",
			value: goodsName.value,
			displayType: 'like'
		})
		return true;
	}
	//表格加載后方法
	const loadAfter = (res) => {
		//注意：如果需要編輯，請按下面配置轉換數據格式
		res.rows.forEach(row => {
			//1、如果字段是圖片，請將圖片字符串調用getImg方法轉換為框架要求的格式
			row.Img = proxy.base.getImg(row.Img, proxy.http)

			//如果是多選或者下拉框多選、級聯，需要將值轉換為數组格式
			//row.字段 = (row.字段||'').split(',')
		})
		return true;
	}

	//加載數據，只有vol-table配置了url此方法才生效
	const loadData = () => {
		//刷新表格
		tableRef.value.load()
	}

	//添加行數據
	const addClick = () => {
		//注意，如果有圖上傳或者下拉框多選，這里需要给默認值數组
		tableData.value.push({
			Img: []
		})
		//也可以將數據添加到第一行
		//tableData.value.unshift({Img:[]})
	}

	const onChange = (field, value, row) => {
		proxy.$toast('選擇了字段[' + (field) + '],值[' + value + ']')
		//這里可以做其他操作。给其他字段設置值：row.字段=
	}

	//表格數據
	const tableData = ref([]);
	//示例配置的url自動調用接口獲取的數據，也可以手動調用接口给tableData設置值，
	// proxy.http.post(url,{参數},true).then(res=>{
	// 	  //注意返回的數據格式，如果有編輯圖片、下拉框多選、級聯，参照上面loadAfter方法說明轉換數據格式
	// 	   tableData.value.splice(0);
	// 	   tableData.value.push(...res);
	// })


	//<vol-table :readonly="false">上面vol-table標簽設置取消只讀才可以編輯
	//如果某個字段不需要編輯，請將設置下readonly:true
	const columns = ref([{
			field: 'GoodsCode',
			required: true,
			title: '商品編號',
			readonly: false,
			type: "input"
		}, {
			field: 'GoodsName',
			title: '商品名稱',
			//required: true,
			readonly: false,
			type: "input", // "textarea"
		},
		{
			field: 'CatalogId',
			title: '商品分類',
			type: "select",
			bind: {
				key: '分類級聯',
				data: []
			}
		},
		{
			field: 'Price',
			//required: true,
			title: '商品單價',
			type: 'decimal',
			extra: {
				text: "元",
				style: "font-size:28rpx;color:#5b5c5c;margin-left:10rpx"
			}
		},
		{
			field: 'CreateDate',
			title: '出廠時間',
			type: 'date'
		},
		{
			field: 'Img',
			title: '商品圖片',
			type: 'img'
		}
	]);
</script>

<style scoped lang="less">
	.table-demo {
		height: 100%;
		background: #fbfbfb;
	}

	.search {
		padding: 10px;
	}

	.scope-header {
		flex: 1;
		text-align: right;
		font-weight: 400;
		font-size: 26rpx;
		display: flex;
		align-items: center;
		justify-content: flex-end;

		.btns {
			margin-top: 12rpx;
			margin-bottom: 26rpx;
		}
	}

	.scope-price {
		font-weight: bolder;
		color: #df0000;
		font-size: 30rpx;
	}

	.add-btn {
		height: 50px;
		position: relative;

		.btn {
			padding: 20rpx;
			width: 100%;
			background: #fff;
			position: fixed;
			box-sizing: border-box;
			bottom: 4rpx;
			z-index: 99;
		}
	}
</style>
