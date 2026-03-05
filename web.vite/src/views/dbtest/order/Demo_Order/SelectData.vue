<template>
  <vol-box :lazy="true" v-model="model" title="選擇客户" :width="1200" :padding="0">
    <div>
      <customer ref="customerRef"></customer>
    </div>
    <template #footer>
      <div>
        <el-button type="primary" size="small" @click="onSelect">確認</el-button>
      </div>
    </template>
  </vol-box>
  <!-- 
     批量選擇明细表數據 -->
  <vol-box
    :lazy="true"
    v-model="detailModel"
    title="批量選擇明细表數據"
    :height="440"
    :width="1000"
    :padding="0"
  >
    <div>
      <!-- 搜索配置 -->
      <div class="search-form">
        <label>商品名稱：</label>
        <el-input style="width: 200px" v-model="GoodsName"></el-input>
        <el-button size="small" type="primary" plain @click="search"
          ><i class="el-icon-search"></i>搜索</el-button
        >
      </div>
      <!-- 表格數據 -->
      <vol-table
        ref="detailTableRef"
        :loadKey="true"
        :columns="columns"
        :pagination-hide="false"
        :height="340"
        :url="url"
        @loadBefore="loadBefore"
      ></vol-table>
    </div>

    <template #footer>
      <div>
        <el-button type="primary" @click="detailSelectClick" size="default"
          >選擇數據</el-button
        >
      </div>
    </template>
  </vol-box>
</template>

<script setup lang="jsx">
import customer from "@/views/dbtest/customer/Demo_Customer";
import {
  ref,
  reactive,
  getCurrentInstance,
  
  
  
  nextTick,
} from "vue";

//創建emit回調方法
const emit = defineEmits(["customerOnSelect", "detailOnSelect"]);

const { proxy } = getCurrentInstance();

const model = ref(false); //彈出框
const detailModel = ref(false); //批量選擇明细表數據model
const GoodsName = ref(""); //商品名稱搜索
//明细的查詢url
const url = "api/Demo_Order/getGoods";
//明细表字段配置
const columns = reactive([
  { field: "GoodsName", title: "商品名稱", width: 120 },
  { field: "GoodsCode", title: "商品編號", type: "string", width: 100 },
  { field: "Img", title: "商品圖片", type: "img", width: 80 },
  { field: "Specs", title: "商品規格", type: "string", width: 90 },
  { field: "Price", title: "單價", type: "decimal", width: 80 },
  { field: "Remark", title: "備註", width: 100 },
  { field: "CreateDate", title: "創建時間", width: 140 },
]);

const customerRef = ref();
//打開主表選擇數據
const open = () => {
  model.value = true;
  //打開時刷新界面數據
  customerRef.value && customerRef.value.$refs.grid.search();
};
//打明细表批量選擇
const openDetail = () => {
  GoodsName.value = "";
  detailModel.value = true;
  //刷新表格的數據
  nextTick(() => {
    detailTableRef.value?.load(null, true);
  });
};
const onSelect = () => {
  //主表的選擇數據回寫到主表的表單上
  let rows = customerRef.value.$refs.grid.getSelectRows();
  if (!rows.length) {
    return proxy.$message.error("請選擇數據");
  }
  emit("customerOnSelect", rows[0]);
  model.value = false;
  // this.$emit("customerOnSelect", ($parent) => {
  //   //如：調用頁面查詢
  //   $parent.editFormFields.Customer = rows[0].Customer;
  //   $parent.editFormFields.CustomerId = rows[0].CustomerId;
  //   $parent.editFormFields.PhoneNo = rows[0].PhoneNo;
  //   this.model = false;
  // });
};
//明细表選擇數據, 回寫數據到明细表
const detailSelectClick = () => {
  //獲取選中的行數據
  let rows = detailTableRef.value.getSelected();
  if (!rows.length) {
    return proxy.$message.error("請選擇數據");
  }
  //獲取回寫到明细表的字段
  rows = rows.map((row) => {
    return {
      GoodsId: row.GoodsId,
      GoodsCode: row.GoodsCode,
      GoodsName: row.GoodsName,
      Img: row.Img,
      Specs: row.Specs,
      Price: row.Price,
    };
  });
  //調用demo_order.vue的detailOnSelect方法回寫數據
  emit("detailOnSelect", rows);
  detailModel.value = false;
};
const detailTableRef = ref();
//明细表點擊搜索
const search = () => {
  //搜索
  detailTableRef.value.load(null, true);
};
//明细表查詢前方法設置查詢條件
const loadBefore = (param, callBack) => {
  //批量選擇設置查詢條件
  param.wheres = [{ name: "GoodsName", value: GoodsName.value, displayType: "like" }];
  callBack(true);
};
//對外暴露打開彈出框方法，否則demo_oder.vue中點擊按鈕訪問不到方法
defineExpose({
  open,
  openDetail,
});
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
