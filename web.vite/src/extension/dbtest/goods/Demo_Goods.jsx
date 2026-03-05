let extension = {
  components: {
    //查詢界面擴展组件
    gridHeader: '',
    gridBody: '',
    gridFooter: '',
    //新建、編輯彈出框擴展组件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  text:"點擊新建或編輯跳轉到新頁面，同樣由代碼生成器生成",
  tableAction: '', //指定某張表的權限(這里填寫表名,默認不用填寫)
  buttons: { view: [], box: [], detail: [] }, //擴展的按鈕
  methods: {
    //下面這些方法可以保留也可以删除
    onInit() {
      //如果是商品信息tree頁面，默認不加載數據
      if (this.$route.path == '/Demo_GoodsTree') {
        this.load = false;
      }
      //設置table表格文字超出后换行顯示
      this.textInline = false;

      let column = this.columns.find((x) => {
        return x.field == 'Enable';
      });
      column.edit = {
        type: 'switch',
        keep: true
      };
      //是否可用字段設置切换事件並保存到數據庫
      column.onChange = (value, row, tableData) => {
        let url = `api/Demo_Goods/updateStatus?goodsId=${row.GoodsId}&enable=${row.Enable}`;
        this.http.get(url, {}, true).then((result) => {
         // this.$Message.success(result);
        });
      };
    },
    onInited() {
      //自定義彈出框的高度
      this.boxOptions.height = this.boxOptions.height + 80;
    },
    nodeClick(catalogIds, nodes) {      //左邊樹節點點擊事件
      //左邊樹節點的甩有子節點，用于查詢數據
      this.catalogIds = catalogIds.join(',');
      //左側樹選中節點的所有父節點,用于新建時設置级聯的默認值
      this.nodes = nodes;
      console.log(this.nodes);

      this.search();
    },
    searchBefore(param) {
      //查詢前方法，如果是左邊樹選擇了商品分類，直接查詢商品分類
      if (this.catalogIds) {
        param.wheres.push({
          name: 'CatalogId',
          value: this.catalogIds,
          displayType: 'selectList'
        });
      }
      return true;
    },
    modelOpenAfter(row) {
      //點擊編輯/新建按鈕彈出框后，可以在此處寫邏輯，如，從后台獲取數據
      if (this.currentAction == 'Add') {
        //新建時設置左邊樹選中的節點
        this.editFormFields.CatalogId = this.nodes;
      }
    }
  }
};
export default extension;
