<template>
  <vol-box :lazy="true" v-model="model" title="選擇物料" :width="900" :padding="0">
    <div>
      <!-- 搜索配置 -->
      <div class="search-form">
        <label>物料名稱：</label>
        <el-input style="width: 120px" v-model="MaterialName"></el-input>
        <label style="margin-left: 10px">物料編號：</label>
        <el-input style="width: 120px" v-model="MaterialCode"></el-input>
        <el-button size="small" type="primary" @click="search">搜索</el-button>
      </div>
      <!-- 表格數據 -->
      <vol-table ref="tableRef" :loadKey="true" :columns="columns" :pagination-hide="false" :height="370" :url="url"
        :single="single" @loadBefore="loadBefore"></vol-table>
    </div>
    <template #footer>
      <el-button type="primary" @click="detailSelectClick" size="small">選擇數據</el-button>
    </template>
  </vol-box>
</template>
<script setup>
import { ref, nextTick, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()
const tableRef = ref()
const model = ref(false)
//調用的接口地址,這里使用的生成頁面的接口，也可以参照：组件示例->table组件中自定義配置
const url = ref('api/MES_Material/getPageData')

//是否單選(明细表格選擇數據)
const single = ref(false);
const detailRow = ref({})

const MaterialCode = ref('') //物料編碼
const MaterialName = ref('') //物料名稱

//表格配置(可以將生成vue文件中columns配置複制過来)
const columns = ref([
  { field: 'MaterialCode', title: '物料編碼', type: 'string', width: 120 },
  { field: 'MaterialName', title: '物料名稱', type: 'string', width: 90 },
  { field: 'Img', title: '物料圖片', type: 'img', width: 80 },
  { field: 'Specification', title: '規格型號', type: 'string', width: 110 },
  {
    field: 'Unit',
    title: '單位',
    type: 'string',
    bind: { key: '物料單位', data: [] },
    width: 60
  },
  { field: 'Creator', title: '創建人', type: 'string', width: 80 },
  { field: 'CreateDate', title: '創建時間', type: 'datetime', width: 130 }
])
//彈出框打開選擇數據
const open = (row) => {
  //明细表表格選擇數據傳進来的當前行
  detailRow.value = row;
  //如果是明细表表格傳進来的按鈕，表格設置為單選
  single.value = !!row;
  model.value = true
  //刷新表格的數據
  nextTick(() => {
    tableRef.value?.load(null, true)
  })
}

const emit = defineEmits(['onSelect'])
//選擇數據按鈕
const detailSelectClick = () => {
  //回寫數據到明细表
  let rows = tableRef.value.getSelected()
  if (!rows.length) {
    return proxy.$message.error('請選擇數據')
  }
  //如果是明细表表格傳進来的按鈕,直接给表格行數據設置值
  if (detailRow.value) {
    detailRow.value.MaterialName = rows[0].MaterialName;
    detailRow.value.MaterialCode = rows[0].MaterialCode;
    detailRow.value.MaterialSpecification = rows[0].Specification;
    detailRow.value.Unit = rows[0].Unit;
  } else {
    //選擇數據回調
    emit('onSelect', rows)
  }
  model.value = false;
}
//查詢點擊事件
const search = () => {
  tableRef.value.load(null, true)
}
//表數據加載設置查詢條件
const loadBefore = (param, callBack) => {
  param.wheres = [
    { name: 'MaterialCode', value: MaterialCode.value, displayType: 'like' },
    { name: 'MaterialName', value: MaterialName.value, displayType: 'like' }
  ]
  callBack(true)
}
//暴露方法给【選擇物料】按鈕使用
defineExpose({
  open
})
</script>
<style lang="less" scoped>
.search-form {
  display: flex;
  padding: 10px;
  line-height: 34px;

  button {
    margin-left: 10px;
  }
}
</style>