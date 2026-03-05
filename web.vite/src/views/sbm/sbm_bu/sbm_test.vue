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

import VolForm from '@/components/basic/VolForm'
export default {
    components: {"vol-form": VolForm,},
    data () {
        return {
            text: "",
            tabsModel: "0",
            fields: {"field1630258884671":null,"field1630258891760":null,"field1630258904862":null,"field1630258924442":[],"field1630259518082":[],"field1630259033241":[],"field1630259538490":[],"field1630259172794":null,"field1630259600186":null,"field1630259295154":null,"field1630259610476":null,"field1630258969346":[]}, 
            formOptions: [[{"field":"field1630258884671","title":"輸入框","type":"text","required":false,"readonly":false,"colSize":null},
				{"field":"field1630258891760","title":"日期","type":"date","required":false,"readonly":false,"colSize":null},
				{"field":"field1630258904862","title":"下拉框","type":"select","required":false,"readonly":false,"colSize":null,"data":[{"key":"請設置數據源","value":"請設置數據源"					}],"dataKey":""},
				{"field":"field1630258924442","title":"下拉多選","type":"selectList","required":false,"readonly":false,"colSize":null,"data":[{"key":"請設置數據源","value":"請設置數據源"}],"dataKey":""}],
				[{"field":"field1630259518082","title":"级聯","type":"cascader","required":false,"readonly":false,"colSize":null,"data":[{"key":"請配置數據源","value":"請配置數據源"}],"dataKey":""},
				{"field":"field1630259033241","title":"多選","type":"checkbox","required":false,"readonly":false,"colSize":null,"data":[{"key":"1","value":"發货"},{"key":"2","value":"退货"},{"key":"3","value":"返單"}],"dataKey":"ordertype"},
				{"field":"field1630259538490","title":"單選","type":"radio","required":false,"readonly":false,"colSize":null,"data":[{"key":"0","value":"否"},{"key":"2","value":"xx11"},{"key":"1","value":"是"}],"dataKey":"enable"},
				{"field":"field1630259172794","title":"switch","type":"switch","required":false,"readonly":false,"colSize":null}],
				[
                {
                    colSize: 12,
                    render: (h) => {
                      return  <div
                          style="display: flex;margin-bottom:-4px;line-height:20px;margin-top:5px;padding-bottom: 5px;border-bottom: 1px solid rgb(238, 238, 238)">
                          <div style="height: 19px; width: 9px; border-radius: 10px;"></div>
                          <div style="padding-left: 1px; font-weight: bold; font-size: 13px;">{this.$ts('分段信息')}</div>
                        </div>
                    }
                  }
                ],
				[{"field":"field1630259295154","title":"圖片","type":"img","required":false,"readonly":false,"colSize":12,"maxSize":3,"fileInfo":[],"multiple":false,"autoUpload":false,"maxFile":5,"url":"api/SellOrder/upload"}],
				[{"field":"field1630259610476","title":"excel","type":"excel","required":false,"readonly":false,"colSize":12,"maxSize":3,"fileInfo":[],"multiple":false,"autoUpload":true,"maxFile":5,"url":"api/SellOrder/upload"}]],
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