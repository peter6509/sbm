<template>
  <div class="table-item">
    <div class="table-item-header">
      <vol-title icon="edit" title="自定義渲染+按鈕"
        >功能:自定義顯示内容、按鈕、數據源tag颜色</vol-title
      >
      <div class="table-item-buttons">
        <div>
          <el-input
            style="width: 140px; margin-right: 10px"
            v-model="orderNo"
            placeholder="訂單編號"
          ></el-input>
          <el-button type="primary" @click="search" color="#95d475" plain>查詢</el-button>
          <el-button type="primary" @click="addRow" plain>添加行</el-button>
          <el-button type="primary" @click="delRow" color="#f89898" plain
            >删除行</el-button
          >
          <el-button type="primary" @click="getSelected" plain>獲取選中行</el-button>
        </div>
      </div>
    </div>
    <!-- 
        loadBefore:表格數據加載前
        loadBefore:表格數據加載后
        ck：顯示複選框
        column-index：顯示行號
        columns：表格配置
        max-height:表格最大高(也可以設置為:height="maxHeight"固定高度)
     -->
    <vol-table
      @loadBefore="loadBefore"
      @loadAfter="loadAfter"
      ref="tableRef"
      :url="url"
      index
      :columns="columns"
      :max-height="maxHeight"
      :pagination-hide="false"
      :load-key="true"
      :column-index="false"
    ></vol-table>
  </div>
</template>

<script lang="jsx" setup>
import VolTable from "@/components/basic/VolTable.vue";
import { ref, reactive, getCurrentInstance } from "vue";
const { proxy } = getCurrentInstance();
const tableRef = ref();
//計算表格高度
const maxHeight = ref(0);
maxHeight.value = document.body.clientHeight - 250;
//使用生成的接口返回數據，也可以自定義接口返回，見接口代碼
const url = ref("api/demo_order/getPageData");
const orderNo = ref("");
//獲取選中行
const getSelected = () => {
  const rows = tableRef.value.getSelected();
  if (!rows.length) {
    return proxy.$message.error("請選中行");
  }
  proxy.$message.success(`共選中【${rows.length}】行`);
};
const addRow = () => {
  tableRef.value.addRow({ OrderNo: "D2022040600009" });
  //如果批量添加行。請使用：
  //tableRef.value.rowData.push(...[{ OrderNo: "D2022040600009" },{ OrderNo: "D2022040600009" }])
};
const delRow = () => {
  tableRef.value.delRow();
  proxy.$message.success("删除成功");
};
const search = () => {
  tableRef.value.load(null, true);
  proxy.$message.success("查詢成功");
};
//加載表格查詢前方法
const loadBefore = (params, callBack) => {
  //調用后台接口前處理
  //設置查詢條件,参數更多屬性说明見：【绑定接口+跨頁選擇】
  params.wheres.push({
    name: "OrderNo",
    value: orderNo.value,
    displayType: "like", //模糊查詢
  });

  //也可以给value設置值，后台自己解析
  // params.value=orderNo.value
  callBack(true); //false不會調用后台接口
};
//查詢后方法
const loadAfter = (rows, callBack, result) => {
  //這里Enable設置只是測試數據，不要複制過去了
  rows.forEach((x, index) => {
    x.Enable = 0;
    x.Enable = index < 2 ? 1 : 0;
  });

  callBack(true);
};
const getTagName = (row) => {
  // ["primary", "success", "info", "warning", "danger"]
  if (row.OrderType == 1) {
    return "primary";
  }
  if (row.OrderType == 2) {
    return "success";
  }
  if (row.OrderType == 3) {
    return "danger";
  }
  return "primary";
};

const columns = reactive([
  { field: "Order_Id", title: "Order_Id", type: "guid", width: 110, hidden: true },
  {
    field: "OrderNo",
    title: "自定義圖標、事件",
    type: "string",
    width: 130,
    render: (h, { row, column, index }) => {
      return (
        <div>
          <i
            onClick={() => {
              //row當前行數據
              proxy.$message.success("點擊了第1個圖標");
            }}
            class="el-icon-search"
            style="color: #2196F3;"
          ></i>
          <i
            onClick={() => {
              //row當前行數據
              proxy.$message.success("點擊了第3個圖標");
            }}
            class="el-icon-date"
            style="margin-left:10px;color: #2196F3;"
          ></i>
          <span style="margin-left:5px">{row.OrderNo}</span>
        </div>
      );
    },
  },
  {
    field: "OrderType",
    title: "字段合並",
    type: "int",
    bind: { key: "訂單狀態", data: [] },
    width: 150,
    tip: {
      //自定義表頭提示
      text: "多個字段合並顯示减少字段列數",
      icon: "el-icon-warning-outline",
      color: "#0085ee", //圖標颜色
      click: () => {
        //圖標點擊事件
        proxy.$message.success("點擊了表頭");
      },
    },
    render: (h, { row, column, index }) => {
      //proxy.base.getColumnDicItem轉换數據字典顯示文本
      return (
        <div style="line-height:1.5;padding:5px 0;">
          <div>訂單編號：{row.OrderNo}</div>
          <div>商品價格：{row.TotalPrice}</div>
          <div>
            訂單狀態：
            <el-tag size="small" type={getTagName(row)}>
              {proxy.base.getColumnDicItem(columns, "OrderType", row.OrderType).value ||
                row.OrderType}
            </el-tag>
          </div>
          <div>采购時間：{(row.OrderDate || "").substr(0, 10)}</div>
        </div>
      );
    },
  },
  {
    field: "OrderDate",
    title: "自定義提示",
    width: 80,
    tip: {
      //自定義表頭提示
      text: "鼠標放在表格上自動彈出提示",
      icon: "el-icon-warning-outline",
      color: "#0085ee", //圖標颜色
      click: () => {
        //圖標點擊事件
        proxy.$message.success("點擊了表頭");
      },
    },
    render: (h, { row, column, index }) => {
      return (
        <el-popover placement="right" title="提示信息" width={250} trigger="hover">
          {{
            reference: () => (
              <div>
                <i class="el-icon-warning-outline" style="color: #2196F3;">
                  {(row.OrderDate || "").substr(0, 7)}
                </i>
              </div>
            ),
            //自定義提示内容
            default: () => {
              return (
                <div style="line-height:1.5;">
                  <div>訂單編號：{row.OrderNo}</div>
                  <div>商品價格：{row.TotalPrice}</div>
                  <div>
                    訂單狀態：
                    <el-tag size="small" type={getTagName(row)}>
                      {proxy.base.getColumnDicItem(columns, "OrderType", row.OrderType)
                        .value || row.OrderType}
                    </el-tag>
                  </div>
                  <div>采购時間：{row.OrderDate}</div>
                </div>
              );
            },
          }}
        </el-popover>
      );
    },
  },
  {
    field: "Customer",
    title: "步骤條",
    align: "center",
    width: 120,
    render: (h, { row, column, index }) => {
      //量多步骤條屬性見：https://element-plus.org/zh-CN/component/steps.html
      return (
        //可以根據row的值動態生成步骤信息
        <el-steps style="max-width:200px;padding:10px 0" active={1} align-center>
          {[
            { title: "采购", desc: "采购描述" },
            { title: "生产", desc: "生产描述" },
            { title: "銷售", desc: "銷售描述" },
          ].map((c) => {
            return <el-step title={c.title} />;
          })}
        </el-steps>
      );
    },
  },
  {
    field: "PhoneNo",
    title: "進度條",
    align: "center",
    width: 100,
    render: (h, { row, column, index }) => {
      //  percentage={90} 實際應该修改為：row.字段
      if (index % 2 === 1) {
        //90改為row.字段
        return <el-progress stroke-width={4} percentage={90} />;
      }
      //10改為row.字段
      return (
        <el-progress stroke-width={4} color="#67c23a" show-text={true} percentage={10} />
      );
    },
  },
  {
    field: "Enable",
    title: "複選框",
    type: "int",
    width: 70,
    align: "center",
    render: (h, { row, column, index }) => {
      //注意:Enable表格數據的字段或者返回的字段，必須要有值，不能是null或者空，尽量是0/1,否則頁面加載就會觸發onChange
      //更多屬性配置：https://element-plus.org/zh-CN/component/checkbox.html
      return (
        <el-checkbox
          true-value={1}
          false-value={0}
          v-model={row.Enable}
          onChange={(val) => {
            proxy.$message.success("選中事件" + val);
          }}
        ></el-checkbox>
      );
    },
  },
  {
    field: "Enable",
    title: "開關",
    type: "int",
    width: 70,
    align: "center",
    render: (h, { row, column, index }) => {
      //注意:Enable表格數據的字段或者返回的字段，必須要有值，不能是null或者空，尽量是0/1,否則頁面加載就會觸發onChange
      //更多屬性配置：https://element-plus.org/zh-CN/component/c.html
      return (
        <el-switch
          active-value={1}
          inactive-value={0}
          v-model={row.Enable}
          inline-prompt
          active-text="是"
          inactive-text="否"
          onChange={(val) => {
            proxy.$message.success("選中事件" + val);
          }}
        ></el-switch>
      );
    },
  },
  {
    field: "Enable",
    title: "自定義",
    type: "int",
    width: 70,
    align: "center",
    render: (h, { row, column, index }) => {
      //注意:Enable表格數據的字段或者返回的字段，必須要有值，不能是null或者空，尽量是0/1
      //更多屬性配置：https://element-plus.org/zh-CN/component/c.html
      return (
        <div>
          <span
            style={{
              width: "8px",
              height: "8px",
              display: "inline-block",
              borderRadius: "50%",
              marginRight: "5px",
              background: row.Enable ? "#0ae413" : "#f82616",
            }}
          ></span>
          {row.Enable ? "正常" : "異常"}
        </div>
      );
    },
  },
  {
    title: "各種按鈕",
    field: "操作",
    width: 110,
    fixed: "right", //設置固定到右邊，也可以固定到左邊'left'
    align: "center", // 'center',
    render: (h, { row, column, index }) => {
      return (
        <div style="padding-right:10px;">
          <el-button
            onClick={($e) => {
              //row當前行數據
              proxy.$message.success("查看");
            }}
            type="primary"
            link
          >
            查看
          </el-button>

          {/* 通過條件判断,要顯示的按鈕 */}
          {/*  {
                                      index % 2 === 1
                                      ?<el-button>修改</el-button>
                                      : <el-button>設置</el-button>
                                  } */}

          {/* 通過v-show控制按鈕隐藏與顯示
                  下面的index % 2 === 1换成：row.字段==值 */}
          <el-button
            onClick={($e) => {
              //row當前行數據
              proxy.$message.success("修改");
            }}
            v-show={index % 2 === 1}
            type="success"
            link
          >
            修改
          </el-button>
          <el-button
            onClick={($e) => {
              //row當前行數據
              proxy.$message.success("設置");
            }}
            v-show={index % 2 === 0}
            type="warning"
            link
          >
            設置
          </el-button>

          <el-dropdown
            onClick={(value) => {
              this.dropdownClick(value);
            }}
            trigger="click"
            v-slots={{
              dropdown: () => (
                <el-dropdown-menu>
                  <el-dropdown-item>
                    <div
                      onClick={() => {
                        //row當前行數據
                        proxy.$message.success("京酱肉丝");
                      }}
                    >
                      京酱肉丝
                    </div>
                  </el-dropdown-item>
                  <el-dropdown-item>
                    <div
                      onClick={() => {
                        //row當前行數據
                        proxy.$message.success("驴肉火烧");
                      }}
                    >
                      驴肉火烧
                    </div>
                  </el-dropdown-item>
                </el-dropdown-menu>
              ),
            }}
          >
            <span
              style="font-size: 13px;color: #409eff;margin: 5px 0 0 10px;"
              class="el-dropdown-link"
            >
              更多<i class="el-icon-arrow-right"></i>
            </span>
          </el-dropdown>
        </div>
      );
    },
  },
]);
</script>
<style lang="less" scoped>
.table-item-header {
  display: flex;
  align-items: center;
  padding: 6px;
  .table-item-buttons {
    flex: 1;
    text-align: right;
  }
}
::v-deep(.el-step__title) {
  font-size: 13px !important;
}
</style>
