<template>
  <div class="table-item">
    <div class="table-item-header">
      <div class="table-item-border"></div>
      <vol-title icon="edit" title="樹形结構"> 功能：樹形结構</vol-title>
      <div class="table-item-buttons">
        <div>
          <el-button type="primary" @click="search" color="#95d475" plain>查詢</el-button>
        </div>
      </div>
    </div>
    <!-- 
         lazy：是否懒加載，如果需要顯示所有數據不延迟加載，請將lazy設置為false,去掉loadTreeChildren,並且后台返回全部數據
         rowKey：主鍵字段
         rowParentField：父级id字段
         defaultExpandAll：默認展開所有節點，lazy=false時生效
        expandRowKeys：默認展開的行數據主鍵id，lazy=false時生效
     -->
    <vol-table
      :lazy="true"
      rowKey="Role_Id"
      rowParentField="ParentId"
      @loadBefore="loadBefore"
      @loadAfter="loadAfter"
      :defaultExpandAll="false"
      :expandRowKeys="[1]"
      :loadTreeChildren="loadTreeChildren"
      ref="tableRef"
      :url="url"
      index
      :columns="columns"
      :height="maxHeight"
      :pagination-hide="false"
      :ck="true"
      :column-index="true"
    ></vol-table>
  </div>
</template>

<script lang="jsx" setup>
import VolTable from "@/components/basic/VolTable.vue";
import { ref, reactive, getCurrentInstance } from "vue";
const { proxy } = getCurrentInstance();
const tableRef = ref();
//接口返回數據，可以框架生成的接口getPageData
//如果是自定義的接口，需要返回的數據格式：{total:100,rows:[]}
const url = ref("api/sys_role/getPageData");
//計算表格高度
const maxHeight = ref(0);
maxHeight.value = document.body.clientHeight - 250;
/***延迟加載子節點數據，見Sys_RoleController.cs文件***/
const loadTreeChildren = (tree, treeNode, resolve) => {
  //加載子節點
  let url = `api/Sys_Role/getTreeTableChildrenData?roleId=${tree.Role_Id}`; //Role_Id换為表的主鍵字段
  proxy.http.post(url, {}).then((result) => {
    resolve(result.rows);
  });
};

const loadBefore = (params, callBack) => {
  //調用后台接口前處理
  //設置查詢條件
  callBack(true); //false不會調用后台接口
};
//查詢后方法
const loadAfter = (rows, callBack, result) => {
  callBack(true);
};
const search = () => {
  tableRef.value.load(null, true);
  proxy.$message.success("查詢成功");
};

const columns = reactive([
  { field: "RoleName", title: "角色名稱", width: 140 },
  { field: "Role_Id", title: "角色ID", width: 70 },
  {
    field: "Enable",
    title: "是否啟用",
    bind: { key: "enable", data: [] },
    width: 90,
  },
  { field: "Creator", title: "創建人", width: 130 },
  {
    field: "CreateDate",
    title: "創建時間",
    width: 150,
  },
  {
    field: "Modifier",
    title: "修改人",
    width: 100,
  },
  {
    field: "ModifyDate",
    title: "修改時間",
    width: 180,
  },
]);
</script>
<style lang="less" scoped>
.table-item-header {
  display: flex;
  align-items: center;
  padding: 6px 0;
  .table-item-buttons {
    flex: 1;
    text-align: right;
  }
}
</style>
