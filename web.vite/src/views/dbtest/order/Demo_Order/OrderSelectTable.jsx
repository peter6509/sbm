//初始化主表數據選擇
export const initFormSelectTable = (gridRef) => {
  //配置編輯表單下拉框table搜索選項
  const item = gridRef.getFormOption('Customer')
  item.disabled = false
  //配置請求的接口地址
  //可以使用生成的頁面接口，注意接口權限問題，如果提示没有權限,参照后台后開發文檔上的重寫權限示例
  //item.url = 'api/Demo_Customer/getPageData';

  //尽量自定義接口，見下面的文檔描述，或者Demo_CustomerController類的方法Search
  item.url = 'api/Demo_Customer/search'
  item.labelRender = (h, {}) => {
    return (
      <div>
        <el-popover placement="top-start" title="下拉table搜索" width="600" trigger="hover">
          {{
            default: () => {
              return (
                <div style="font-size:13px;line-height:2">
                  快速搜索自定義表格内容，支持數據自绑定、分頁、上下鍵+回车選擇數據
                </div>
              )
            },
            reference: () => {
              return (
                <div style="color:#1598ff">
                  <span class="el-icon-warning-outline" style="font-size:13px"></span> 客户
                </div>
              )
            }
          }}
        </el-popover>
      </div>
    )
  }
  //設置顯示的字段
  item.columns = [
    {
      field: 'Customer_Id',
      title: 'Customer_Id',
      width: 110,
      hidden: true
    },
    //設置search:true,則字段可以搜索
    {
      field: 'Customer',
      title: '客户',
      width: 80,
      search: true
    }, //search是否開啟表格上方的字段搜索
    {
      field: 'PhoneNo',
      title: '手機',
      width: 110,
      search: true
    },
    {
      field: 'Province',
      title: '省',
      type: 'select',
      bind: { key: '省', data: [] },
      width: 80,
      search: true
    },
    {
      field: 'DetailAddress',
      title: '詳细地址',
      width: 120
    }
  ]

  //選中table數據后，回寫到表單
  item.onSelect = (rows) => {
    gridRef.editFormFields.Customer = rows[0].Customer
    gridRef.editFormFields.PhoneNo = rows[0].PhoneNo
  }

  /****下面的這些都是可以選配置，上面的是必填的******/

  //(輸入框搜索)表格數據加載前處理
  item.loadBefore = (param, callback) => {
    //方式1、手動設置查詢條件
    // param.wheres.push({
    //       name:"Customer",
    //       value:this.editFormFields.Customer,
    //       displayType:"like"
    // })
    //方式2、给param.value設置值，后台手動處理查詢條件
    param.value = gridRef.editFormFields.Customer
    callback(true)
  }

  /****************下面這些配置不是必須的**************/
  //表格數據加載后處理
  item.loadAfter = (rows, callback, result) => {
    callback(true)
  }

  //設置彈出框高度(默認200)
  item.height = 200
  //設置彈出框宽度(默認500)
  //item.width = 400;
  // item.textInline = false; //設置表格超出自動换行顯示
  //設置表格是否單選
  item.single = true
  //設置是否顯示分頁
  item.paginationHide = false
}

//初始化明细數據選擇
export const initDetailTable = (gridRef) => {
  //明细表合計時表單多個字段設置值(與上面的示例區别在于這里)
  gridRef.detailOptions.columns.forEach((col) => {
    //给數量Qty字段合計自定義顯示格式、同時與表單聯動顯示
    if (col.field == 'Qty') {
      //value:Qty字段合計的结果
      //rows:明细表的全部數據
      //summaryArrData:所有合計的全部對象
      col.summaryFormatter = (qtyValue, column, rows, summaryArrData) => {
        //明细表輸入或者值變化后给表單字段設置值
        gridRef.editFormFields.TotalQty = qtyValue

        //從明细表rows找到價格字段，手動計算合計
        let priceValue = 0
        rows.forEach((x) => {
          priceValue += x.Price || 0
        })

        //明细表數量字段+價格字段計算结果给總價設置值
        gridRef.editFormFields.TotalPrice =( qtyValue * priceValue).toFixed(2)*1.0

        //這里的return qtyValue一定要寫上,自定義返回格式,return qtyValue+'件'
        return qtyValue + '件'
      }
    }
  })

  //配置編輯表單下拉框table搜索選項
  gridRef.detailOptions.columns.forEach((item) => {
    if (item.field == 'GoodsName') {
      item.readonly = false
      //配置請求的接口地址
      //可以使用生成的頁面接口，注意接口權限問題，如果提示没有權限,参照后台后開發文檔上的重寫權限示例
      //item.url = 'api/Demo_Goods/getPageData';

      //尽量自定義接口，見下面的文檔描述，或者Demo_GoodsController類的方法Search
      item.url = 'api/Demo_Goods/search'

      //設置顯示的字段
      item.columns = [
        {
          field: 'GoodsName',
          title: '商品名稱',
          type: 'string',
          width: 120
        },
        {
          field: 'GoodsCode',
          title: '商品編號',
          type: 'string',
          width: 100
        },
        {
          field: 'Specs',
          title: '規格',
          type: 'string',
          width: 60,
          align: 'left'
        },
        { field: 'Price', title: '單價', type: 'decimal', width: 60 },
        { field: 'Remark', title: '備註', type: 'string', width: 100 }
      ]

      //選中table數據后，回寫到表單
      //editRow:當前正在編輯的行
      //rows:選中的行
      item.onSelect = (editRow, rows) => {
        editRow.GoodsName = rows[0].GoodsName
        editRow.GoodsCode = rows[0].GoodsCode
        editRow.Price = rows[0].Price
      }

      /****下面的這些都是可以選配置，上面的是必填的******/

      //(輸入框搜索)表格數據加載前處理
      //editRow:當前正在編輯的行
      //param:請求的参數
      item.loadBefore = (editRow, param, callback) => {
        //方式1、手動設置查詢條件
        // param.wheres.push({
        //       name:"GoodsName",
        //       value:row.GoodsName,
        //       displayType:"like"
        // })
        //方式2、给param.value設置值，后台手動處理查詢條件
        param.value = editRow.GoodsName
        callback(true)
      }

      /****************下面這些配置不是必須的**************/
      //表格數據加載后處理
      //editRow:當前正在編輯的行
      //rows:后台返回的數據
      item.loadAfter = (editRow, rows, callback, result) => {
        callback(true)
      }

      //設置彈出框高度(默認200)
      item.height = 200
      //設置彈出框宽度(默認500)
      item.selectWidth = 500
      item.textInline = true //設置表格超出自動换行顯示
      //設置表格是否單選
      item.single = true
      //設置是否顯示分頁
      item.paginationHide = true
    }
  })
}
