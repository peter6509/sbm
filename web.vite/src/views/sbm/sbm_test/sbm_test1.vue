<template>
<div style="padding: 15px 20px 15px 5px">
  <div class="pre-text">{{ text }}</div>
  <vol-form ref="form"
            :labelWidth="80"
            :load-key="false"
            :formFields="fields"
            :formRules="formOptions">
  </vol-form>
 
 

  <div class="form-btns">
    <el-button type="primary"
               @click="submit"
               icon="el-icon-check"
               size="mini">提交</el-button>
    <el-button type="primary"
               @click="reset"
               plain
               icon="el-icon-refresh-right"
               size="mini">重置</el-button>
  </div>
</div>
</template>

<script>
// 使用方式：
// 1、新建一個vue頁面，把此頁面内容複制進去
// 2、router->index.js配置路由，頁面上輸入地址即可看到數據(也可以把菜單配置上)
// 3、或者参照表單設計頁面做動態頁面
//**表單設計器的table下載還在開發中
import extend from "@/extension/sbm/sbm_test/sbm_test.jsx";
import viewOptions from './sbm_test/options.js'
import VolForm from '@/components/basic/VolForm'
export default {
    components: {"vol-form": VolForm,},
    data () {
        return {
            text: "",
            tabsModel: "0",
            fields: {"test_no":null,"test_name":null,"test_type":null,"":null,"test_addr":null,"test_memo":null}, 
            formOptions: [[{"field":"test_no","title":"TEST單號","type":"text","required":false,"readonly":false,"colSize":null},
				{"field":"test_name","title":"TEST名稱","type":"text","required":false,"readonly":false,"colSize":null},
				{"field":"test_type","title":"TEST屬性","type":"select","required":false,"readonly":false,"colSize":null,"data":[{"key":"1","value":"公司"},{"key":"2","value":"個人"					}],"dataKey":"夥伴類型"}],
				[
                {
                    colSize: 12,
                    render: (h) => {
                      return  <div
                          style="display: flex;margin-bottom:-4px;line-height:20px;margin-top:5px;padding-bottom: 5px;border-bottom: 1px solid rgb(238, 238, 238)">
                          <div style="height: 19px; width: 9px; border-radius: 10px;"></div>
                          <div style="padding-left: 1px; font-weight: bold; font-size: 13px;">{this.$ts('TEST地址')}</div>
                        </div>
                    }
                  }
                ],
				[{"field":"test_addr","title":"","type":"textarea","required":false,"readonly":false,"colSize":12}],
				[
                {
                    colSize: 12,
                    render: (h) => {
                      return  <div
                          style="display: flex;margin-bottom:-4px;line-height:20px;margin-top:5px;padding-bottom: 5px;border-bottom: 1px solid rgb(238, 238, 238)">
                          <div style="height: 19px; width: 9px; border-radius: 10px;"></div>
                          <div style="padding-left: 1px; font-weight: bold; font-size: 13px;">{this.$ts('TEST地址')}</div>
                        </div>
                    }
                  }
                ],
				[{"field":"test_memo","title":"","type":"editor","required":false,"readonly":false,"height":200,"colSize":12,"url":""}]],
            tables: [],
            tabs: [] 
        };
    },
    created () { 

    },
    methods: {
        submit () {
            this.$Message.success("submit")
            return;
            this.http.post("url",this.fields,true).then(result=>{

            })
        },
        reset () {
            this.$refs.form.reset();
            this.$Message.success("表單已重置")
        },
        download () {
            this.$Message.info("111")
        }
    }
};

VolForm;
</script>
<style lang="less" scoped>
.form-btns {
text-align: center;
}
.tables {
padding-left: 15px;
.table-item {
  padding: 10px;
}
.table-header {
  display: flex;
  margin-bottom: 8px;
}
.header-text {
  position: relative;
  bottom: -9px;
  flex: 1;
  font-weight: bold;
}
.header-btns {
  text-align: right;
}
}
</style>