<template>
  <div class="table-item">
    <div class="small-text">
      功能:自動绑定數據源、自定義合計格式、(行單擊、雙擊、複選框)事件、設置默認選中行、拖拽行排序、表頭篩選、固定列、文本居中、文本超出换行顯示、超出提示信息、自定義表頭、行號、複選框、拖動右下角改變高度
    </div>
    <div class="table-item-header">
      <vol-title icon="edit" title="基礎表格"></vol-title>
      <div class="table-item-buttons">
        <div>
          <el-button type="primary" @click="addRow" plain>添加行</el-button>
          <el-button type="primary" @click="delRow" color="#f89898" plain>删除行</el-button>
          <el-button type="primary" @click="getRow" plain>獲取選中行</el-button>
          <el-button type="primary" @click="clearRow" color="#95d475" plain>清空行</el-button>
          <el-button type="primary" @click="updateSummary" color="#95d475" plain
            >刷新合計</el-button
          >
        </div>
      </div>
    </div>

    <!-- 
        ck：顯示複選框
        column-index：顯示行號
        single:只否單選
        drag-position：表格可拖動改變高度
        table-data:表格數據
        columns：表格配置
        max-height:表格最大高(也可以設置為:height="maxHeight"固定高度)
        sortable：表格拖動排序
        onSortEnd:表格拖動排序结束事件
        text-inline：文本超出换行顯示
        pagination-hide：隐藏分頁
        rowClick：行點擊事件
        rowDbClick：行點雙擊事件
        selectionChange：複選框選擇事件
        selectable:複選框checkbox是否可以選中
     -->
    <vol-table
      ref="tableRef"
      ck
      column-index
      :single="false"
      drag-position="bottom"
      :table-data="tableData"
      :columns="columns"
      :max-height="maxHeight"
      :sortable="true"
      @onSortEnd="onSortEnd"
      :text-inline="false"
      :pagination-hide="true"
      @rowClick="rowClick"
      @rowDbClick="rowDbClick"
      @selectionChange="selectionChange"
      :selectable="selectable"
    ></vol-table>
  </div>
</template>

<script setup lang="jsx">
import VolTable from '@/components/basic/VolTable.vue'
import { ref, reactive, getCurrentInstance, nextTick, defineExpose } from 'vue'
const { proxy } = getCurrentInstance()
const tableRef = ref()
const maxHeight = ref(0)
//計算表格高度:屏幕高度减去其他標簽高度
maxHeight.value = document.body.clientHeight - 236

const getRow = () => {
  const rows = tableRef.value.getSelected()
  if (!rows.length) {
    proxy.$message.error('請選中行')
    return
  }
  proxy.$message.success(JSON.stringify(rows))
}

const delRow = () => {
  tableRef.value.delRow()
  proxy.$message.success('删除成功')
}
const clearRow = () => {
  tableData.splice(0)
  proxy.$message.success('數據已清空')
}

const addRow = () => {
  // tableData.push({ OrderNo: "D2022040600009" });
  // tableData = [{ OrderNo: "D2022040600009" }];
  // this.tableData = [];
  tableData.push(...[{ OrderNo: 'D2022040600009' }])
}

//表格拖動结束事件
const onSortEnd = (rows, newIndex, oldIndex) => {
  //rows表格數據,可循環rows修改行的值
  proxy.$message.info('拖動排序结束')
}

const rowClick = ({ row, column, event, index }) => {
  proxy.$message.info(`點擊了第[${index}行]`)
}
const rowDbClick = ({ row, column, event, index }) => {
  proxy.$message.info(`雙擊了第[${index}行]`)
}
const selectionChange = (rows) => {
  proxy.$message.info(`勾選事件`)
}
//刷新合計
const updateSummary = () => {
  tableRef.value.initSummary()
  //刷新指定字段的合計信息
  //tableRef.value.updateSummary(["字段1", "字段2"]);
  proxy.$message.info(`刷新合計成功`)
}
//禁用複選框不可選
const selectable = (row, index) => {
  //可根據row.字段判断
  return index !== 2
}

//設置默認選中第一行(如果是接口返回的數據，寫loadAfter方法后面，見绑定接口)
// nextTick(() => {
//   tableRef.value.toggleRowSelection(tableData[0]);
// });

const columns = reactive([
  { field: 'Order_Id', title: 'Order_Id', type: 'guid', width: 110, hidden: true },
  {
    field: 'OrderNo',
    title: '訂單編號',
    filterData: true, //開啟篩選
    showOverflowTooltip: true, //内容超出時鼠標放上去提示文本
    align: 'center', //居中顯示：center,居左顯示:left，靠右顯示:right
    width: 100
  },
  {
    field: 'OrderType',
    title: '訂單類型',
    titleStyle: 'color:#0085ee', //標題樣式
    tip: {
      //自定義表頭提示
      text: '自定表頭提示',
      icon: 'el-icon-edit-outline',
      color: '#0085ee', //圖標颜色
      click: () => {
        //圖標點擊事件
        proxy.$message.success('點擊了表頭')
      }
    },
    type: 'int',
    bind: { key: '訂單類型', data: [] },
    width: 70
  },
  {
    field: 'TotalPrice',
    title: '總價',
    type: 'decimal',
    summary: true,
    width: 60,
    sort: true, //排序
    align: 'center',
    summaryFormatter: (val, col, data, summaryData) => {
      //自定義格式化合計顯示
      if (!val) {
        return ''
      }
      summaryData[0] = '汇總+' //自定義合計名稱
      //自定義合計值的格式
      val = (val + '').replace(/\d{1,3}(?=(\d{3})+(\.\d*)?$)/g, '$&,')
      return '¥' + val
    }
  },
  {
    field: 'TotalQty',
    title: '總數量',
    type: 'int',
    summary: true, //開啟合計
    numberLength: 2, //保留兩位小數
    width: 80,
    align: 'center'
  },
  {
    field: 'OrderDate',
    title: '訂單日期',
    type: 'date',
    width: 95
  }, //如果要顯示日期時分秒改為type: "datetime"
  {
    field: 'Customer',
    title: '客户',
    type: 'string',
    width: 80,
    renderHeader: (h, { row, column, index }) => {
      return (
        <el-tag
          size="small"
          onClick={() => {
            proxy.$message.success('點擊了表頭')
          }}
        >
          {column.title}
          <span class="el-icon-search" style="margin-left:4px"></span>
        </el-tag>
      )
    }
  },
  { field: 'PhoneNo', title: '手機', type: 'string', width: 110 },
  { field: 'CreateDate', title: '創建時間', type: 'datetime', width: 150 }
])
const tableData = reactive([
  {
    OrderNo: 'D2023082400001',
    OrderType: 3,
    TotalPrice: 180,
    TotalQty: 2500.68,
    OrderDate: '2023-08-09 00:00:00',
    CustomerId: null,
    Customer: '阮星竹',
    PhoneNo: '18612009000',
    OrderStatus: 3,
    Remark: null,
    CreateDate: '2023-08-24 00:52:52'
  },
  {
    OrderNo: 'D2023082400002',
    OrderType: 2,
    TotalPrice: 450,
    TotalQty: 1500,
    OrderDate: '2023-09-19 00:00:00',
    CustomerId: null,
    Customer: '阮星竹',
    PhoneNo: '18612009000',
    OrderStatus: 2,
    Remark: null,
    CreateDate: '2023-08-24 00:52:52'
  }
])
</script>
<style lang="less" scoped>
.table-item-header {
  display: flex;
  padding: 6px 0;
  .table-item-buttons {
    margin-left: auto;
  }
}
.small-text {
  font-size: 12px;
  color: #2196f3;
  padding-top: 10px;
  position: relative;
  top: 2px;
}
</style>
