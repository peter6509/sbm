<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/sbm/sbm_sale_order/sbm_sale_order.jsx或sbm_sale_order.vue文件編写
 *新版本支持vue或【表.jsx]文件編写業務,文档见:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
 -->
<template>
    <view-grid ref="grid"
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
               :modelOpenAfter="modelOpenAfter">
        <!-- 自定义组件數據槽扩展，更多數據槽slot见文档 -->
        <template #gridHeader>
        </template>
    </view-grid>
</template>
<script setup lang="jsx">
    import extend from "@/extension/sbm/sbm_sale_order/sbm_sale_order.jsx";
    import viewOptions from './sbm_sale_order/options.js'
    import { ref, reactive, getCurrentInstance, watch, onMounted } from "vue";
    import { hiprint } from 'vue-plugin-hiprint'
    const grid = ref(null);
    const { proxy } = getCurrentInstance()
    //http請求，proxy.http.post/get
    const { table, editFormFields, editFormOptions, searchFormFields, searchFormOptions, columns, detail, details } = reactive(viewOptions())

    let gridRef;//對應[表.jsx]文件中this.使用方式一样
   

    //生成對象屬性初始化
    const onInit = async ($vm) => {
        gridRef = $vm;
        //與jsx中的this.xx使用一样，只需將this.xx改為gridRef.xx
        //更多屬性见：https://doc.volcore.xyz/docs/view-grid
      
        gridRef.pagination.sortName = 'name';
        gridRef.pagination.order = 'desc';
        gridRef.columnIndex = true ;
        gridRef.detailOptions.pagination = {
            total : 0,
            size: 30,
            sizes:[30,60,90],
            sortName : "sale_item",
            order : 'asc'
        }
        let compOption,contactOption ;
        gridRef.editFormOptions.forEach((options) => {
        options.forEach((item) => {
        if (item.field == 'partner_company'){
           compOption = item ;
           }   
        if (item.field == 'partner_contact'){
           contactOption = item ;
            }
        });
        });
       compOption.onChange = (value,option) => {
       let url="api/sbm_partner/Getcontact?code=" + value ;
       gridRef.http.get(url,{},true).then(result =>{
       contactOption.data = result ;          
             })
         };
       columns.forEach(x=>{
        //设置Title列固定
        if (x.field=='name') {
            x.fixed=true //也可以设置为right,固定到最右边
            x.align="center"//设置文本居中
        }
        })
        gridRef.buttons.splice(1, 0, {
            name: '報價單EMail',
            icon: 'el-icon-document',
            type: 'primary',
            plain: true,
            onClick: () => {
            // 打開新開案表單（不立即刷新，交由自動刷新或保存成功後刷新）
            printAndSend();
            },
         });
         gridRef.buttons.splice(1, 0, {
            name: '重整畫面',
            icon: 'el-icon-document',
            type: 'primary',
            plain: true,
            onClick: () => {
            // 打開新開案表單（不立即刷新，交由自動刷新或保存成功後刷新）
               refreshTree();
            },
         });
       
    }
     const refreshTree = async () => {
        gridRef.search();
     };
    //生成對象屬性初始化后,操作明细表配置用到
    const onInited = async () => {
       gridRef.showCustom = false;
       gridRef.showCustomSearch = false;


    gridRef.showCustomSearch = false ;
    //gridRef.getTable().updateSummary(["price_sub"],false)

    gridRef.detailOptions.columns.forEach((x) => {
      if (x.field === 'prod_num' || x.field === 'price_unit') { 
            x.onKeyPress = (row, column, $event) => {
              row.price_sub = (row.prod_num ||0) * (row.price_unit ||0) ;
              let sum = 0 ;
              gridRef.detailOptions.data.forEach(r => {
                sum += (r.price_sub || 0);
              })

              // 3. 回寫主檔（header table / editForm）
              gridRef.editFormFields.untax_amount = sum ;
              gridRef.editFormFields.tax = Math.round(sum * 0.05) ;
              gridRef.editFormFields.tot_amount = sum + Math.round(sum * 0.05) ;
              gridRef.editFormFields.discount_amount = sum  ;
            }
        }
      }   
      )
       gridRef.detailOptions.columns.forEach(x => {
        if (x.field == 'price_sub') {
            x.summary = true;
            //计算平均值
            //x.summary = 'avg';
            //设置小数显示位数(默认2位)
            // x.numberLength = 4;

            //这里也可以自定义返回合计的格式、文本显示
            x.summaryFormatter = (val, column, rows, summaryData) => {
                    if (!val) return '0.00';
                    summaryData[0] = '合計';
                    return ( '$' + (val + '').replace(/\B(?=(\d{3})+(?!\d))/g, ',')
                    );
             };

        }
    })
   
    }

    const printAndSend = async () => {
      const template = hiprint.getTemplate('sbm_quotation')

      // 產生 PDF
     const pdf = await template.toPdf({
      data: printData
      })

     // 轉成 blob
      const blob = new Blob([pdf], { type: 'application/pdf' })

      const formData = new FormData()
      formData.append('file', blob, 'quotation.pdf')
      formData.append('email', 'peter@lansir.com.tw')

      await api.post('/api/email/send', formData)
     }
    const searchBefore = async (param) => {
        //界面查詢前,可以给param.wheres添加查詢参數
        //返回false，則不會執行查詢
        return true;
    }
    const searchAfter = async (rows, result) => {
        return true;
    }
    const addBefore = async (formData) => {
        //新建保存前formData為對象，包括明细表，可以给给表單設置值，自己输出看formData的值
        return true;
    }
    const updateBefore = async (formData) => {
       // formData 包含 editFormFields 和明細 rows
       // 計算明細小計

       // 計算主檔金額
       const lines = Array.isArray(formData.sbm_sale_order_line)
           ? formData.sbm_sale_order_line
             : []

         const sum = lines.reduce((t, r) => {
           return t + Number(r.price_sub || 0)
               }, 0)
       const round2 = v => Math.round(v * 100) / 100

       formData.untax_amount = round2(sum)
       formData.discount_amount = round2(sum)
       formData.tax = round2(formData.discount_amount * 0.05)
       formData.tot_amount = round2(formData.discount_amount + formData.tax)
       
       
       return true
      }

    const rowClick = ({ row, column, event }) => {
        //查詢界面點击行事件
        // grid.value.toggleRowSelection(row); //單击行時选中當前行;
    }
    const modelOpenBefore = async (row) => {//弹出框打開后方法
        return true;//返回false，不會打開弹出框
    }
    const modelOpenAfter = (row) => {
        //弹出框打開后方法,設置表單默認值,按钮操作等
    }
    //監聽表單输入，做實時计算
    //watch(() => editFormFields.字段,(newValue, oldValue) => {	})
    //對外暴露數據
    defineExpose({})
</script>
<style scoped>
.detail-header-center .vxe-cell--title {
  text-align: center !important;
}
</style>
