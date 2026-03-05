<template>
  <div class="coder-v3">
    <!-- 左邊樹结構 -->
    <div class="coder-left">
      <div class="coder-left-title">
        <i class="el-icon-receiving title-icon"></i> 代碼生成配置
      </div>
      <el-scrollbar :width="1" class="coder-left-scrollbar">
        <el-tree ref="treeRef" highlight-current node-key="id" class="tree-contianer" :data="treeDataValue"
          :expand-on-click-node="false" @node-click="nodeClick" icon="ArrowRight">
        </el-tree>
      </el-scrollbar>
    </div>
    <div class="tree-content">
      <coder-content ref="contentRef" @loadTableAfter="loadTableAfter" :parent-data="data"></coder-content>
    </div>
  </div>
</template>

<script setup>
import coderContent from "./coderV3Content.vue";
const emit = defineEmits(["nodeClick"]);
import { ref, reactive, getCurrentInstance, nextTick, computed } from "vue";
const { proxy } = getCurrentInstance();
const contentRef = ref();
const data = ref([]);
const orginData = ref([]); //原始數據
const defaultProps = ref({
  children: "children",
  label: "name", //樹形结構顯示名稱的字段
});
const treeRef = ref();

const treeDataValue = computed(() => {
  return data.value.filter((x) => {
    return x.value != -999;
  });
});

const loadTableAfter = (item) => {
  data.value.push(item);
};

//節點點擊事件
const nodeClick = (data) => {
  //左側樹點擊事件
  let ids = [];
  //獲取分類的子節點
  //左邊樹節點的甩有子節點，用于查詢數據
  getAllChildrenId(data, ids);
  //獲取所有的父節點
  //左側樹選中節點的所有父節點,用于新建時設置级聯的默認值
  let nodes = proxy.base.getTreeAllParent(data.id, orginData.value);
  //獲取節點的id
  nodes = nodes.map((m) => {
    return m.id;
  });
  contentRef.value.nodeClick(data.id);
  // emit("nodeClick", ids, nodes);
};
const getAllChildrenId = (data, ids) => {
  //獲取分類的子節點
  ids.push(data.id);
  if (!data.children || !data.children.length) {
    return;
  }
  data.children.forEach((x) => {
    ids.push(x.id);
    getAllChildrenId(x, ids);
  });
};
// console.log(document.body.clientWidth, document.body.clientHeight);
if (document.body.clientWidth < 1500 || document.body.clientHeight < 700) {
  document.body.style = "zoom:0.9;";
}
//從商品MES_MaterialCatalogController加載左邊tree數據
proxy.http.post("api/builder/GetTableTree").then((result) => {
  var treeData = JSON.parse(result.list);
  const nameSpace = JSON.parse(result.nameSpace).map((x) => {
    return {
      key: x,
      value: x,
    };
  });

  //記錄原始數據
  orginData.value = treeData;
  //將返回的數據遞归轉换為tree结構，文檔：http://v2.volcore.xyz/document/vueDev【數组對象轉换為tree】
  data.value = proxy.base.convertTree(treeData, (node, data, isRoot) => {
    node.label = node.name;
    node.value = node.id;
    node.key = node.id;
  });

  data.value.unshift({
    parentId: null,
    value: -999,
    label: "無",
  });

  nextTick(() => {
    contentRef.value.initData(nameSpace, data.value, orginData.value);
  });
  // console.log(data.value);
  // if (data.value.length>1) {
  //   nodeClick(data.value[0]);
  // }
});
</script>
<style lang="less" scoped>
@import "./style/coderV3.less";
</style>
<style lang="less">
.tree-contianer {
  /* 覆盖 el-tree 的默認樣式 */
  .el-tree-node__content:hover {
    .el-button--text {
      color: #2F3133;
    }
  }

  .tree-node-opt {
    margin: 00010px;

    .el-button--text {
      color: #dddddd;
    }
  }

  .el-tree-node {
    position: relative;


    &::before {
      content: '';
      width: 1px;
      height: 100%;
      border-left: 1px solid #d9d9d9;
      position: absolute;
      left: -4px;
      top: -17px;
    }

    &::after {
      content: '';
      width: 20px;
      height: 0px;
      border-top: 1px solid #d9d9d9;
      position: absolute;
      top: 20px;
      left: -4px;
    }

    &:last-child:before {
      height: 38px;
    }

    .el-tree-node__children {
      padding-left: 16px;
    }

    .el-tree-node__expand-icon.is-leaf {
      display: none;
    }
  }

  &>.el-tree-node:before {
    border-left: none;
  }

  &>.el-tree-node:after {
    border-top: none;
  }
}
</style>