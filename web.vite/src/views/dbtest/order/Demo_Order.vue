<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/dbtest/order/Demo_Order.jsx或Demo_Order.vue文件編寫
 *新版本支持vue或【表.jsx]文件編寫業務,文檔見:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
 -->
<template>
  <view-grid
    ref="grid"
    :columns="columns"
    :detail="detail"
    :details="details"
    :editFormFields="editFormFields"
    :editFormOptions="editFormOptions"
    :searchFormFields="searchFormFields"
    :searchFormOptions="searchFormOptions"
    :table="table"
    :extend="extend"
    :onInit="onInit"
    :onInited="onInited"
    :searchBefore="searchBefore"
    :searchAfter="searchAfter"
    :addBefore="addBefore"
    :updateBefore="updateBefore"
    :rowClick="rowClick"
    :modelOpenBefore="modelOpenBefore"
    :modelOpenAfter="modelOpenAfter"
    :detailSelectable="detailSelectable"
    :saveConfirm="saveConfirm"
    :printBefore="printBefore"
  >
    <!-- 自定義组件數據槽擴展，更多數據槽slot見文檔 -->
    <template #gridFooter>
      <gridFooter ref="footerRef"></gridFooter>
    </template>
    <template #gridBody>
      <el-alert
        show-icon
        style="margin-bottom: 10px"
        type="success"
        title="當前頁面示例：打印、多表頭；彈出框：編輯表單下拉框table搜索、明细表table搜索；"
      ></el-alert>
    </template>
    <template #modelBody>
      <el-alert
        show-icon
        :closable="false"
        type="success"
        title="點擊[客户]或明细表[商品名稱]可進行下拉框table搜索(代碼生成器生成后即可使用)"
      ></el-alert>
    </template>
    <template #detailContent>
      <!-- 自定義明细表内容 -->
      <DetailContent @detailContentOnChange="detailContentOnChange"></DetailContent>
    </template>
  </view-grid>

  <!-- 彈出框選擇數據 -->
  <SelectData
    ref="selectRef"
    @detailOnSelect="detailOnSelect"
    @customerOnSelect="customerOnSelect"
  ></SelectData>
</template>
<script setup lang="jsx">
import extend from "@/extension/dbtest/order/Demo_Order.jsx";
import viewOptions from "./Demo_Order/options.js";
import { ref, reactive, getCurrentInstance, watch, onMounted } from "vue";
//同一界面明细表格顯示
import gridFooter from "./Demo_Order/Demo_OrderGridFooter.vue";
//彈出框自定義輸入框
import DetailContent from "./Demo_Order/Demo_OrderDetailContent.vue";
//主表與明细表彈出框選擇數據
import SelectData from "./Demo_Order/SelectData.vue";
//下拉table配置
import { initFormSelectTable, initDetailTable } from "./Demo_Order/OrderSelectTable.jsx";
const grid = ref(null);
const { proxy } = getCurrentInstance();
//http請求，proxy.http.post/get
const {
  table,
  editFormFields,
  editFormOptions,
  searchFormFields,
  searchFormOptions,
  columns,
  detail,
  details,
} = reactive(viewOptions());
const footerRef = ref();
let gridRef;
//生成對象屬性初始化
const onInit = async ($vm) => {
  gridRef = $vm;
  //設置按鈕顯示數量，超出的顯示在更多里面
  gridRef.maxBtnLength = 6;
  initBtn();
};

const printBefore = (rows, param) => {
  param.test = 123;
  return true;
};

const saveConfirm = async (callback, formData, isAdd) => {
  proxy
    .$confirm("提示文字信息", "警告", {
      confirmButtonText: "確定",
      cancelButtonText: "取消",
      type: "warning",
      center: true,
    })
    .then(() => {
      //邏輯處理完成后調用保存操作
      callback(true);
    });
};

const detailSelectable = (row, index, item) => {
  console.log(index);
  return index == 1;
};

const selectRef = ref();
//生成對象屬性初始化后,操作明细表配置用到
const onInited = async () => {
  initFormSelectTable(gridRef);
  //明细初始化配置必須寫在onInited中
  initDetailTable(gridRef);

  //增加明细表選擇數據按鈕
  gridRef.detailOptions.buttons.unshift({
    name: "選擇數據", //按鈕名稱
    icon: "el-icon-plus", //按鈕圖標，参照iview圖標
    hidden: false, //是否隐藏按鈕(如果想要隐藏按鈕，在onInited方法中遍历buttons，設置hidden=true)
    onClick: () => {
      //觸發事件
      selectRef.value.openDetail();
    },
  });

  //這里可以重新設置主表表格高度
  if (gridRef.height > 300) {
    gridRef.height = gridRef.height - 160 - 50;
  }
  gridRef.height = gridRef.height - 40;
  //主表設置表格合計功能
  gridRef.summary = true;
  gridRef.columns.forEach((x) => {
    if (x.field == "TotalQty" || x.field == "TotalPrice") {
      x.summary = true;
    }
  });
  //明细表求和
  gridRef.detailOptions.summary = true;
  gridRef.detailOptions.columns.forEach((x) => {
    x.require = false;
    if (x.field == "Price" || x.field == "Qty") {
      x.summary = true;
      //按回车自動跳轉到下一行焦點
      // x.onKeyPress = (row, column, $e) => {
      //   if ($e && $e.keyCode == 13) {
      //     gridRef.$refs.detail.toNextCell(null, row, "Qty", true);
      //   }
      // };
    }
  });

  //設置明细表高度
  gridRef.detailOptions.height = 240;
  //固定彈出框宽度
  gridRef.boxOptions.width = 1200;
};
//明细表選擇數據回寫
const detailOnSelect = (rows) => {
  //獲取明细表rowData追加數據
  gridRef.getTable().rowData.push(...rows);
};
//表單選擇數據回調
const customerOnSelect = (row) => {
  editFormFields.Customer = row.Customer;
  editFormFields.CustomerId = row.CustomerId;
  editFormFields.PhoneNo = row.PhoneNo;
};

const searchBefore = async (param) => {
  //界面查詢前,可以给param.wheres添加查詢参數
  //返回false，則不會執行查詢
  return true;
};
const searchAfter = async (rows, result) => {
  return true;
};
const addBefore = async (formData) => {
  //新建保存前formData為對象，包括明细表，可以给给表單設置值，自己輸出看formData的值
  return true;
};
const updateBefore = async (formData) => {
  //編輯保存前formData為對象，包括明细表、删除行的Id
  return true;
};
const rowClick = ({ row, column, event }) => {
  //取消其他行選中
  gridRef.getTable(true).clearSelection();
  //設置選中當前行
  gridRef.getTable(true).toggleRowSelection(row);
  //調用Demo_OrderGridFooter.vue中明细表table的查詢方法
  footerRef.value?.gridRowClick(row);
};
const modelOpenBefore = async (row) => {
  //彈出框打開后方法
  return true; //返回false，不會打開彈出框
};
const modelOpenAfter = (row) => {
  //彈出框打開后方法,設置表單默認值,按鈕操作等
};
//監聽表單輸入，做實時計算
//watch(() => editFormFields.字段,(newValue, oldValue) => {	})

const detailContentOnChange = (val) => {
  proxy.$message.success("下選擇選擇了:" + val);
};

//配置表頭過滤
columns.forEach((x) => {
  if (x.field == "OrderNo" || x.field == "OrderType") {
    x.filterData = true;
  }
});

//自定義合計顯示格式
columns.forEach((x) => {
  if (x.field == "TotalPrice") {
    x.summary = true;
    x.align = "center";
    x.width = 80;
    x.summaryFormatter = (val, column, result, summaryData) => {
      if (!val) {
        return "0.00";
      }
      summaryData[0] = "汇總";
      return (
        "￥" + (val + "").replace(/\B(?=(\d{3})+(?!\d))/g, ",") //+ '元'
      );
    };
    //計算平均值
    //x.summary = 'avg';//2023.05.03更新voltable文件后才能使用
    //設置小數顯示位數(默認2位)
    // x.numberLength = 4;
  }
});

const initBtn = () => {
  //增加提交審批按鈕
  let index = gridRef.buttons.findIndex((x) => {
    return x.value == "Audit";
  });
  gridRef.buttons.splice(index + 1, 0, {
    name: "提交",
    icon: "el-icon-check",
    class: "",
    plain: true,
    type: "primary",
    onClick: () => {
      let rows = gridRef.getSelectRows();
      if (!rows.length) {
        return gridRef.$message.error("請選擇行數據");
      }
      let ids = rows.map((x) => {
        return x.Order_Id;
      });

      gridRef.http.post("api/Demo_Order/submitAudit", ids).then((result) => {
        if (!result.status) {
          gridRef.$message.error(result.message);
          return;
        }
        gridRef.$message.success(result.message);
        gridRef.search();
      });
    },
  });

  //自定義表單按鈕
  let countdown = 10;
  const btnValue = ref("發送短信");
  const phoneOption = gridRef.getFormOption("PhoneNo");
  phoneOption.extra = {
    render: (h, {}) => {
      return (
        <div>
          <el-button
            type="primary"
            link
            onClick={() => {
              selectRef.value.open();
            }}
          >
            <i class="el-icon-search">選擇數據</i>
          </el-button>
          <el-button
            type="primary"
            style="margin-left:0"
            link
            onClick={() => {
              //設置倒計時
              var timer = setInterval(function () {
                if (countdown > 0) {
                  btnValue.value = countdown + "(秒)";
                  countdown--;
                } else {
                  btnValue.value = "發送短信";
                  countdown = 10;
                  clearInterval(timer);
                }
              }, 1000);
            }}
          >
            <i class="el-icon-message">{btnValue.value}</i>
          </el-button>
          <el-popover placement="top-start" title="提示" width="200" trigger="hover">
            {{
              reference: () => {
                return (
                  <i
                    style="color:rgb(6 118 169);font-size:12px;margin-left:5px"
                    onClick={() => {
                      gridRef.$message.success("提示信息");
                    }}
                    class="el-icon-warning-outline"
                  ></i>
                );
              },
              default: () => {
                return (
                  <div style="width:300px">
                    <div style="font-size:12px">觸發 Popover 顯示的 HTML 元素</div>
                  </div>
                );
              },
            }}
          </el-popover>
        </div>
      );
    },
  };
};
//對外暴露數據
defineExpose({});
</script>
