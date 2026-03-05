import gridHeader from './Sys_RoleGridHeade.vue'
import auth from './Sys_RoleAuthDataOptions.js'
let extension = {
  components: {
    //動態擴充组件或组件路径
    //表單header、content、footer對應位置擴充的组件
    gridHeader: gridHeader,
    gridBody: '',
    gridFooter: '',
    //彈出框(修改、編輯、查看)header、content、footer對應位置擴充的组件
    modelHeader: '',
    modelBody: '',
    modelFooter: ''
  },
  buttons: [], //擴展的按鈕
  tableAction: 'Sys_Role',
  methods: {
    //事件擴展
    onInited() {
      let authData = auth.map((x) => {
        const data = Object.assign({}, x)
        data.key = data.key + ''
        return data
      })

      // this.height = this.height - 80;
      this.editFormOptions.forEach((x) => {
        x.forEach((item) => {
          if (item.field == 'ParentId') {
            item.title = '上级角色'
            //設置任意節點都能選中(默認只能選中最后一個節點)
            item.changeOnSelect = true
          } else if (item.field == 'DatAuth') {
            //設置任意節點都能選中(默認只能選中最后一個節點)
            item.type = 'select'
            item.data = authData
          }
        })
      })
      this.columns.forEach((x) => {
        if (x.field == 'DatAuth') {
          x.bind = { data: authData }
        }
      })
    },
    initService() {
      if (!this.$global.db) {
        return
      }
      try {
        let list = this.$store.getters.getServiceList().map((x) => {
          return {
            key: x.id,
            value: x.name
          }
        })
        if (!list.length) {
          return
        }
        this.editFormFields.DbServiceId = null
        this.editFormOptions.push([
          { field: 'DbServiceId', title: '租户', data: list, type: 'select' }
        ])
      } catch (error) {}
    },
    onInit() {
      this.initService()
      //設置treetable的唯一值字段(這個字段的值在表里面必須是唯一的)
      this.rowKey = 'Role_Id'
      this.lazy = false
      this.rowParentField = 'ParentId' //ParentId為表的父级id字段

      this.columns.find((x) => {
        return x.field == 'ParentId'
      }).hidden = true

      if (
        this.buttons.some((x) => {
          return x.value == 'Add' || x.value == 'Update'
        })
      ) {
        this.columns.push({
          title: '權限',
          field: '權限',
          width: 80,
          fixed: 'right',
          align: 'center',
          render: (h, { row, column, index }) => {
            return (
              <div>
                <el-tooltip
                  class="box-item"
                  effect="dark"
                  content={this.$ts('權限設置')}
                  placement="top"
                >
                  <el-button
                    link
                    onClick={($e) => {
                      this.openModel(row)
                    }}
                    type="primary"
                    plain
                    size="small"
                    style="padding: 5px !important;margin: 0;"
                  >
                    <i style="font-size:13px" class="el-icon-user"></i>
                    {this.$ts('權限')}
                  </el-button>
                </el-tooltip>
              </div>
            )
          }
        })
      }
    },
    openModel(row) {
      this.$refs.gridHeader.open(row)
    },
    /***加載后台數據見Sys_RoleController.cs文件***/
    loadTreeChildren(tree, treeNode, resolve) {
      //加載子節點
      let url = `api/role/getTreeTableChildrenData?roleId=${tree.Role_Id}`
      this.http.post(url, {}).then((result) => {
        resolve(result.rows)
      })
    },
    /***加載后台數據見Sys_RoleController.cs文件***/
    searchBefore(params) {
      //判断加載根節點或子節點
      //没有查詢條件，默認查詢返回所有根節點數據
      if (!params.wheres.length) {
        params.value = 1
      }
      return true
    },
    addAfter() {
      this.initDicKeys()
      return true
    },
    updateAfter() {
      this.initDicKeys()
      return true
    },
    delAfter() {
      this.initDicKeys()
      return true
    }
  }
}
export default extension
