<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/mes/mes/MES_ProductionOrder.jsx或MES_ProductionOrder.vue文件編寫
 *新版本支持vue或【表.jsx]文件編寫業務,文檔見:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
 -->
<template>
  <div class="grid">
    <view-grid ref="grid" :columns="columns" :detail="detail" :details="details" :editFormFields="editFormFields"
      :editFormOptions="editFormOptions" :searchFormFields="searchFormFields" :searchFormOptions="searchFormOptions"
      :table="table" :extend="extend" :onInit="onInit" :onInited="onInited" :searchBefore="searchBefore"
      :searchAfter="searchAfter" :addBefore="addBefore" :updateBefore="updateBefore" :rowClick="rowClick"
      :modelOpenBefore="modelOpenBefore" :modelOpenAfter="modelOpenAfter">
      <!-- 自定義组件數據槽擴展，更多數據槽slot見文檔 -->
      <template #gridHeader>
      </template>
      <template #modelBody>
        <el-alert style="margin-bottom: -1px;" title="只需簡單配置，明细表訂單數量輸入即可實時計算表單的數量" class="alert-primary"
          :closable="false"></el-alert>
      </template>
      <template #gridFooter>
        <order-footer ref="footerRef"></order-footer>
      </template>
    </view-grid>
  </div>
  <!-- 表單選擇客户 -->
  <select-custom ref="customRef" @onSelectCustom="onSelectCustom"></select-custom>
  <!-- 明细表選擇物料 -->
  <select-material ref="materialRef" @onSelect="onSelect"></select-material>
</template>
<script setup lang="jsx">
import orderFooter from './MES_ProductionOrder/MES_ProductionOrderFooter.vue'
//明细表選擇物料頁面
import SelectMaterial from './MES_ProductionOrderSelectSelectMaterial.vue'
//表單選擇客户頁面
import SelectCustom from './MES_ProductionOrderSelectCustom.vue'
import extend from '@/extension/mes/mes/MES_ProductionOrder.jsx'
import viewOptions from './MES_ProductionOrder/options.js'
import { ref, reactive, getCurrentInstance, watch, onMounted } from 'vue'
const grid = ref(null)
const { proxy } = getCurrentInstance()
//http請求，proxy.http.post/get
const {
  table,
  editFormFields,
  editFormOptions,
  searchFormFields,
  searchFormOptions,
  columns,
  detail,
  details
} = reactive(viewOptions())

//方式1：明细表：物料編號下拉框選擇给其他字段設置值
detail.columns.forEach((item) => {
  if (item.field == 'MaterialCode') {
    item.onChange = (row) => {
      //查找數據源，數據字典維护的sql,已經返回了其他字段的值，或者這里通過proy.http調用接口返回數據
      const dic =
        item.bind.data.find((x) => {
          return x.key == row.MaterialCode
        }) || {}
      row.MaterialName = dic.value
      row.MaterialSpecification = dic.Specification
      row.Unit = dic.Unit
    }
  } else if (item.field == 'PlanQuantity') {//設置明细表輸時實時計算给表單設置值
    item.summary = true;
    //明细表訂單數量輸入時给主表的訂單數量設置合計值
    item.summaryFormatter = (qtyValue, column, rows, summaryArrData) => {
      //明细表輸入或者值變化后给表單字段設置值
      editFormFields.OrderQty = qtyValue
      //這里的return qtyValue一定要寫上,自定義返回格式,return qtyValue+'件'
      return qtyValue + '件'
    }
  }
})

//設置主表合計字段
columns.forEach(x => {
  if (x.field == 'OrderQty') {
    x.summary = true;
  }
})

columns.splice(7, 1, {
  field: "工序生产進度",
  title: "工序生产進度",
  align: "left",
  width: 200,
  render: (h, { row, column, index }) => {
    //量多步骤條屬性見：https://element-plus.org/zh-CN/component/steps.html
    return (
      //可以根據row的值動態生成步骤信息
      <el-steps style="padding:10px 0" active={index + 1}>
        {[
          { title: "建模", desc: "建模建模建模" },
          { title: "冲压", desc: "冲压冲压冲压" },
          { title: "焊接", desc: "焊接焊接焊接" },
          { title: "涂装", desc: "涂装涂装涂装" },
          { title: "總装", desc: "總装總装總装" },
        ].map((c) => {
          return <el-step>
            {{ title: () => { return <span style="font-size:12px">{c.title}</span> } }}
          </el-step>

        })}
      </el-steps>
    );
  },
})

columns.splice(8, 1, {
  field: "任務進度",
  title: "任務進度",
  align: "left",
  width: 150,
  render: (h, { row, column, index }) => {
    //更多步骤條屬性見：https://element-plus.org/zh-CN/component/steps.html
    return <div>
      <el-progress
        stroke-width={6}
        color="#409eff"
        show-text={false}
        percentage={80} />  <div>80%/({row.OrderQty})</div></div>

  }
})


const customRef = ref();

let gridRef //對應[表.jsx]文件中this.使用方式一樣
//生成對象屬性初始化
const onInit = async ($vm) => {
  gridRef = $vm

  const option = gridRef.getFormOption("CustomerName");
  option.readonly = true;
  option.extra = {
    icon: 'el-icon-zoom-out', //顯示圖標
    text: '選擇', //顯示文本
    style: 'color: #3a8ee6;font-size: 13px;cursor: pointer;',
    //觸發事件
    click: (item) => {
      customRef.value.open();
    }
  };
}
//選擇客户回調
const onSelectCustom = (rows) => {
  editFormFields.CustomerName = rows[0].CustomerName;
  //這里還可以给其他字段設置值editFormFields.字段=
}


const materialRef = ref()
//生成對象屬性初始化后,操作明细表配置用到
const onInited = async () => {
  //設置明细表高度
  gridRef.detailOptions.height = 300;
  //設置彈出框宽度
  gridRef.boxOptions.width = 1000
  //設置表格高度
  gridRef.height = gridRef.height - 280

  //通過彈出框選擇數據
  gridRef.detailOptions.buttons.unshift({
    name: '選擇物料', //按鈕名稱
    icon: 'el-icon-plus', //按鈕圖標，参照iview圖標
    hidden: false, //是否隐藏按鈕(如果想要隐藏按鈕，在onInited方法中遍历buttons，設置hidden=true)
    onClick: () => {
      //觸發事件
      materialRef.value.open()
    }
  })

  //给物料編碼設置彈出框選擇數據
  detail.columns.forEach((x) => {
    if (x.field == 'MaterialCode') {
      x.render = (h, { row, column, index }) => {
        x.edit = null;//設置當前字段不可編輯
        return (
          <div>
            <el-button
              link
              onClick={($e) => {
                $e.stopPropagation();
                //觸發事件
                materialRef.value.open(row)
              }}
              class="el-icon-search"
              style="color: #2196F3;cursor: pointer;"
            ></el-button>
            <span style="margin-left:5px">{row.MaterialCode}</span>
          </div>
        );
      };
    }
  })
}

//選擇物料回調
const onSelect = (rows) => {
  //轉换數據格式與當前明细表字段一致
  rows = rows.map((c) => {
    return {
      MaterialName: c.MaterialName,
      MaterialCode: c.MaterialCode,
      MaterialSpecification: c.Specification,
      Unit: c.Unit
    }
  })
  //寫入明细表
  gridRef.getTable().rowData.unshift(...rows)
}

const searchBefore = async (param) => {
  //界面查詢前,可以给param.wheres添加查詢参數
  //返回false，則不會執行查詢
  return true
}
const searchAfter = async (rows, result) => {
  return true
}
const addBefore = async (formData) => {
  //新建保存前formData為對象，包括明细表，可以给给表單設置值，自己輸出看formData的值
  return true
}
const updateBefore = async (formData) => {
  //編輯保存前formData為對象，包括明细表、删除行的Id
  return true
}
const footerRef = ref()
//查詢界面點擊行事件,加載明细表數據
const rowClick = ({ row, column, event }) => {
  //查詢界面點擊行事件
  // grid.value.toggleRowSelection(row); //單擊行時選中當前行;
  footerRef.value.gridRowClick(row)
}
const modelOpenBefore = async (row) => {
  //彈出框打開后方法
  return true //返回false，不會打開彈出框
}
const modelOpenAfter = (row) => {
  //彈出框打開后方法,設置表單默認值,按鈕操作等
}
//監聽表單輸入，做實時計算
//watch(() => editFormFields.字段,(newValue, oldValue) => {	})
//對外暴露數據
defineExpose({})
</script>
<style lang="less">
.grid .el-step__title {
  line-height: 22px !important;
}
</style>