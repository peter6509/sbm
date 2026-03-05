<template>
  <div class="goods-tree">
    <!-- 左邊樹结構 -->
    <div class="tree-left">
      <div class="tree-left-title">物料分類</div>
      <el-scrollbar style="flex: 1; border-right: 1px solid #eee">
        <!-- el-tree文檔https://element-plus.gitee.io/zh-CN/component/tree.html#%E6%96%B9%E6%B3%95 -->
        <el-tree
          ref="treeRef"
          highlight-current
          node-key="id"
          :default-expanded-keys="defaultExpandedKeys"
          :data="data"
          :props="defaultProps"
          :expand-on-click-node="false"
          @node-click="nodeClick"
        />
      </el-scrollbar>
    </div>
  </div>
</template>

<script setup>
const emit = defineEmits(['nodeClick'])
import { ref, reactive, getCurrentInstance, nextTick } from 'vue'
const { proxy } = getCurrentInstance()
const data = ref([])
const orginData = ref([]) //原始數據
const defaultProps = ref({
  children: 'children',
  label: 'name' //樹形结構顯示名稱的字段
})
const defaultExpandedKeys = ref([]) //默認展開的tree節點

const treeRef = ref()
//節點點擊事件
const nodeClick = (data) => {
  //左側樹點擊事件
  let ids = []
  //獲取分類的子節點
  //左邊樹節點的甩有子節點，用于查詢數據
  getAllChildrenId(data, ids)
  //獲取所有的父節點
  //左側樹選中節點的所有父節點,用于新建時設置级聯的默認值
  let nodes = proxy.base.getTreeAllParent(data.id, orginData.value)
  //獲取節點的id
  nodes = nodes.map((m) => {
    return m.id
  })
  emit('nodeClick', ids, nodes)
}
const getAllChildrenId = (data, ids) => {
  //獲取分類的子節點
  ids.push(data.id)
  if (!data.children || !data.children.length) {
    return
  }
  data.children.forEach((x) => {
    ids.push(x.id)
    getAllChildrenId(x, ids)
  })
}

//從商品MES_MaterialCatalogController加載左邊tree數據
proxy.http.get('api/MES_MaterialCatalog/getList').then((result) => {
  //記錄原始數據
  orginData.value = result
  //將返回的數據遞归轉换為tree结構，文檔：http://v2.volcore.xyz/document/vueDev【數组對象轉换為tree】
  data.value = proxy.base.convertTree(result, (node, data, isRoot) => {})
  //商品分類有數據時加載右邊商品信息
  if (data.value.length) {
    //默認展開經一個樹節點
    defaultExpandedKeys.value = [data.value[0].id]
    nodeClick(data.value[0])
  }
})
</script>
<style lang="less" scoped>
.goods-tree {
  position: absolute;
  display: flex;
  height: 100%;
  width: 100%;
  .tree-left {
    display: flex;
    flex-direction: column;
    width: 200px;
    .tree-left-title {
      line-height: 25px;
      font-size: 13px;
      font-weight: bolder;;
      background: rgba(102, 177, 255, 0.058823529411764705);
      padding: 6px 16px;
      border: 1px solid #ececec;
    }
  }
  .tree-right {
    flex: 1;
    width: 0;
  }
}
</style>
