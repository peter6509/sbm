/*****************************************************************************************
 **  Author:jxx 2022
 **  QQ:283591387
 **完整文檔見：http://v2.volcore.xyz/document/api 【代碼生成頁面ViewGrid】
 **常用示例見：http://v2.volcore.xyz/document/vueDev
 **后台操作見：http://v2.volcore.xyz/document/netCoreDev
 *****************************************************************************************/
//此js文件是用来自定義擴展業務代碼，可以擴展一些自定義頁面或者重新配置生成的代碼

import modelHeader from './orderModelHeader'
let extension = {
  components: {
    //查詢界面擴展组件
    gridHeader: '',
    //自定義列表頁面
    gridBody: '',
    gridFooter: '',
    //新建、編輯彈出框擴展组件
    modelHeader: modelHeader,
    modelBody: '',
    modelFooter: ''
  },
  text: '', //界面上的提示文字
  tableAction: '', //指定某張表的權限(這里填寫表名,默認不用填寫)
  buttons: { view: [], box: [], detail: [] }, //擴展的按鈕
  methods: {
    searchAfter(rows) {
      return true
    },
    //下面這些方法可以保留也可以删除
    onInit() {
      // console.log('oninit')
      // //設置表格篩選
      // this.maxBtnLength = 5
      // this.queryFields = ['CreateDate']
    },
    onInited() {
     
    },
    saveConfirm(callback,formData,isAdd){
      callback()
    }
  }
}
export default extension
