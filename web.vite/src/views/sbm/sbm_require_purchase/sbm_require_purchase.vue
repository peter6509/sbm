<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/sbm/sbm_require_purchase/sbm_require_purchase.jsx或sbm_require_purchase.vue文件编写
 *新版本支持vue或【表.jsx]文件编写業務,文档见:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
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
     <!-- 引用其他頁面（默認隱藏） -->
    <div class="reqopen-ref">
       <ReqOpen class="reqopen-ref" ref="reqopenRef" />
     </div>
</template>
<script setup lang="jsx">
    import extend from "@/extension/sbm/sbm_require_purchase/sbm_require_purchase.jsx";
    import viewOptions from './sbm_require_purchase/options.js'
    import ReqOpen from '@/views/sbm/sbm_reqopen/sbm_reqopen.vue';
    import { ref, reactive, getCurrentInstance, watch, onMounted } from "vue";
    const grid = ref(null);
    const { proxy } = getCurrentInstance()
    const reqopenRef = ref();
    //http請求，proxy.http.post/get
    const { table, editFormFields, editFormOptions, searchFormFields, searchFormOptions, columns, detail, details } = reactive(viewOptions())

    let gridRef;//對應[表.jsx]文件中this.使用方式一样
    //生成對象屬性初始化
    const onInit = async ($vm) => {
        gridRef = $vm;
        gridRef.pagination.sortName = 'name';
        gridRef.pagination.order = 'desc';
        gridRef.columnIndex = true ;
        gridRef.details[0].pagination = {
            total : 0,
            size: 30,
            sizes:[30,60,90],
            sortName : "req_item",
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
         gridRef.buttons.splice(1, 0, {
            name: '詢價單創建',
            icon: 'el-icon-document',
            type: 'primary',
            plain: true,
            onClick: () => {
            // 打開新開案表單（不立即刷新，交由自動刷新或保存成功後刷新）
            reqopenRef.value?.$refs?.grid?.add?.();
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
        //與jsx中的this.xx使用一样，只需將this.xx改為gridRef.xx
        //更多屬性见：https://doc.volcore.xyz/docs/view-grid
    }
    const refreshTree = async () => {
        gridRef.search();
     };
    //生成對象屬性初始化后,操作明细表配置用到
    const onInited = async () => {
        gridRef.showCustom = false ;
        gridRef.showCustomSearch = false ;
        gridRef.buttons.forEach((x) => {
            if (x.name === '新建') {x.hidden = true};
            // if (x.name === '編輯') {x.hidden = true};
         });
    //gridRef.getTable().updateSummary(["price_sub"],false)

    gridRef.details[0].columns.forEach((x) => {
      if (x.field === 'prod_num' || x.field === 'price_unit') { 
            x.onKeyPress = (row, column, $event) => {
              row.price_sub = (row.prod_num ||0) * (row.price_unit ||0) ;
            }

        }
      }   
      )
       gridRef.details[0].columns.forEach(x => {
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
                editFormFields.untax_amount = val ;
                if (editFormFields.discount_amount > 0 ){
                    editFormFields.tax = Math.round(editFormFields.discount_amount * 0.05) ;
                    editFormFields.tot_amount = editFormFields.discount_amount + Math.round(val * 0.05) ;
                    
                } else {
                    editFormFields.tax = Math.round(val * 0.05) ;
                    editFormFields.tot_amount = val + Math.round(val * 0.05) ;
                    editFormFields.discount_amount = val  ;
                }
                
                return ( '$' + (val + '').replace(/\B(?=(\d{3})+(?!\d))/g, ',')
                );
             };

        }
    })

    let stageOption;
    details[1].columns.forEach((x) => {
         if (x.field == 'partner_company') {
            //给下拉框绑定onChange事件
            x.onChange = (row, column) => {
                //设置第二个下拉框的数据源
                companyChange(row, column)
                //这里也可以给其他的字段设置值
                // row.字段=值;
                //也可以给上面的主表表单设置值
                // this.editFormFields.xxx=row.xx;
            }
        }
      });
    const companyChange = async (row, column) => {
      // 1) 取得第二個下拉欄位定義
       const col = details?.[1]?.columns?.find(c => c.field === 'partner_contact');
       if (!col?.bind?.data || !Array.isArray(col.bind.data)) {
           proxy.$message.warning('第一個下拉框尚未綁定資料源或資料格式不正確');
           return;
        }
  
       let url="api/sbm_partner/Getcontact?code=" + row.partner_company ;
     try {
        const res = await proxy.http.get(url, {}, false);

        // 3) 標準化回傳：支援兩種格式
        //    A) [{ key: 1, value: 'S01' }, ...]
        //    B) ['1', '2', ...]（純 key 列表）
       const list = Array.isArray(res?.data) ? res.data : (Array.isArray(res) ? res : []);
       const allowKeys = new Set(
          list.map(x => String(typeof x === 'object' ? x?.key : x))
        );

        // 4) 依 allowKeys 顯示/隱藏（或禁用）選項
        //    注意：不同元件可能是用 "hidden" 或 "disabled"；兩種都設保險
       const next = col.bind.data.map(opt => {
       const canShow = allowKeys.has(String(opt.key));
      return {
        ...opt,
        hidden: !canShow,
        disabled: !canShow
      };
    });


     // 5) 重新指派陣列以確保響應式更新
     col.bind.data = next;

      proxy.$forceUpdate?.();

    } catch (e) {
      proxy.$message.error(`載入專案任務數據失敗：${e?.message || e}`);
    }
   };
    }
    const searchBefore = async (param) => {
        //界面查詢前,可以给param.wheres添加查詢参數
        //返回false，则不會執行查詢
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
       const lines = Array.isArray(formData.sbm_require_purchase_line)
           ? formData.sbm_require_purchase_line
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
<style scoped lang="less">
/* 默認隐藏引用的頁面 */
.reqopen-ref ::v-deep(.layout-container) {
  display: none;
}
</style>
