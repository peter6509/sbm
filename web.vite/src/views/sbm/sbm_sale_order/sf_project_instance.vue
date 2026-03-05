<!--
 *Author：jxx
 *Date：{Date}
 *Contact：283591387@qq.com
 *業務請在@/extension/sf_project/project_instance/sf_project_instance.jsx或sf_project_instance.vue文件编寫
 *新版本支持vue或【表.jsx]文件编寫業務,文檔見:https://doc.volcore.xyz/docs/view-grid、https://doc.volcore.xyz/docs/web
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
  >
    <!-- 自定義组件數據槽擴展 -->
    <template #gridHeader>
      <div style="padding: 0 15px">
        <el-alert
          title="<1>.專案實例請完整配置好再進行發佈方能順利管理 <2>.專案發佈後,任務會發佈通知訊息予相關工程師 <3>.發佈後且未結案之專案,會於進度甘特圖表展現"
          type="success"
          :closable="false"
        />
      </div>
    </template>

    <template #modelHeader>
      <el-alert
        title="專案任務的負責人員/起訖時間應盡可能完整配置"
        type="success"
        :closable="false"
      />
    </template>
  </view-grid>

  <select-template ref="projtemplateRef" @onSelectTemplate="onSelectTemplate" />

  <!-- 引用其他頁面（默認隱藏） -->
  <div class="proj-open-ref">
    <ProjOpen class="proj-open-ref" ref="projopenRef" />
  </div>
  <div class="subins-ref">
    <SubOpen class="subins-ref" ref="subinstanceRef" />
  </div>
  <div class="openusercal-ref">
    <OpenUserCal class="openusercal-ref" ref="openusercalRef" />
  </div>
  <div class="openuserload-ref">
    <OpenUserLoad class="openuserload-ref" ref="openuserloadRef" />
  </div>
  <div class="openrework-ref">
    <OpenReWork class="openrework-ref" ref="openreworkRef"/> 
  </div>
  <div class="projclosed-ref">
    <ProjClosed class="projclosed-ref" ref="projclosedRef"/>
  </div>
</template>

<script setup lang="jsx">
import ProjOpen from '@/views/sf_project/project_open/sf_project_open.vue';
import SubOpen from '@/views/sf_project/project_instance_sub/sf_project_instance_sub.vue';
import OpenUserCal from '@/views/sf_project/project_openusercal/sf_project_openusercal.vue';
import OpenUserLoad from '@/views/sf_project/project_openuserload/sf_project_openuserload.vue';
import extend from '@/extension/sf_project/project_instance/sf_project_instance.jsx';
import viewOptions from './sf_project_instance/options.js';
import OpenReWork from '@/views/sf_project/project_rework/sf_project_rework.vue' ;
import ProjClosed from '@/views/sf_project/project_closed/sf_project_closed.vue' ;

import { ref, reactive, getCurrentInstance, nextTick, onBeforeUnmount } from 'vue';

const grid = ref(null);
const projopenRef = ref();
const subinstanceRef = ref();
const projtemplateRef = ref();
const openusercalRef = ref();
const openuserloadRef = ref();
const openreworkRef = ref() ;
const projclosedRef = ref() ;

const { proxy } = getCurrentInstance();
const { table, editFormFields, editFormOptions, searchFormFields, searchFormOptions, columns, detail, details } =
  reactive(viewOptions());

// 這個不是 ref，不要用 .value
let gridRef;
// 3 秒自動刷新計時器
let refreshTimer = null;
// 子表名
const subTableName = 'sf_project_instance_sub';

// 統一的刷新邏輯（主表 + 子表）
const refreshData = () => {
  // 若希望背景分頁不刷新，保留這行；若要無條件刷新，移除下行判斷
  if (document.visibilityState !== 'visible') return;

  // 主表刷新
  gridRef?.search?.();

  // 子表刷新（當前上下文存在時）
  gridRef?.reloadDetail?.(subTableName);

  // console.debug('✅ Auto refreshed at', new Date().toLocaleTimeString());
};

// 選擇模板回調
const onSelectTemplate = (rows) => {
  if (rows && rows.length) {
    editFormFields.template_id = rows[0].template_id;
  }
};

// 子表保存成功後的刷新（手動觸發時可用）
const onSubSaveSuccess = () => {
  gridRef?.reloadDetail?.(subTableName);
};

// 初始化：拿到 $vm/設定按鈕/欄位
const onInit = async ($vm) => {
  gridRef = $vm;
  
  // 主表排序
  gridRef.pagination.sortName = 'project_no';
  gridRef.pagination.order = 'asc';
  gridRef.columnIndex = true ;
  let deptOption,userOption ;
  gridRef.editFormOptions.forEach((options) => {
        options.forEach((item) => {
        if (item.field == 'dept_id'){
           deptOption = item ;
           }   
        if (item.field == 'user_id'){
           userOption = item ;
            }
        });
    });
   deptOption.onChange = (value,option) => {
      let url="api/Sys_User/GetDeptUser?code=" + value ;
      gridRef.http.get(url,{},true).then(result =>{
      userOption.data = result ;          
            })
        };
  columns.forEach(x=>{
        //设置Title列固定
        if (x.field=='project_no') {
            x.fixed=true //也可以设置为right,固定到最右边
            x.align="center"//设置文本居中
        }
    })
  // 子表分頁設定
  if (gridRef.details && gridRef.details[0]) {
    gridRef.details[0].pagination = {
      total: 0,
      size: 15,
      sizes: [10, 20, 30, 60],
      sortName: 'sub_no',
      order: 'asc',
    };
  }

  // // 主表工具列：專案新開案
  // gridRef.buttons.splice(1, 0, {
  //   name: '專案新開案',
  //   icon: 'el-icon-document',
  //   type: 'primary',
  //   plain: true,
  //   onClick: () => {
  //     // 打開新開案表單（不立即刷新，交由自動刷新或保存成功後刷新）
  //     projopenRef.value?.$refs?.grid?.add?.();
  //   },
  // });

   // 主表工具列：專案新開案
  gridRef.buttons.splice(1, 0, {
    name: '個人行事曆查詢',
    icon: 'el-icon-document',
    type: 'primary',
    title: '個人行事曆選單精靈',
    plain: true,
    onClick: () => {
      // 打開新開案表單（不立即刷新，交由自動刷新或保存成功後刷新）
      openusercalRef.value?.$refs?.grid?.add?.();
    },
  });

    // 主表工具列：專案新開案
  gridRef.buttons.splice(1, 0, {
    name: '個人工作負荷統計',
    icon: 'el-icon-document',
    type: 'primary',
    title: '工作負荷選單精靈',
    plain: true,
    onClick: () => {
      // 打開新開案表單（不立即刷新，交由自動刷新或保存成功後刷新）
      openuserloadRef.value?.$refs?.grid?.add?.();
    },
  });

  // 主表工具列：專案新開案
  gridRef.buttons.splice(1, 0, {
    name: '專案新開案',
    icon: 'el-icon-document',
    type: 'primary',
    plain: true,
    onClick: () => {
      // 打開新開案表單（不立即刷新，交由自動刷新或保存成功後刷新）
      projopenRef.value?.$refs?.grid?.add?.();
    },
  });


  // 主表工具列：資料重整（手動立即刷新）
  gridRef.buttons.splice(1, 0, {
    name: '畫面重整',
    icon: 'el-icon-refresh',
    type: 'primary',
    plain: true,
    onClick: () => {
      refreshData();
    },
  });

  // 已結案行底色
  columns.forEach((x) => {
    x.cellStyle = (row) => {
      if (row.is_closed === true) {
        return { background: '#CCD6DD' };
      }
      return {};
    };
  });
};

// Inited：此時 gridRef 已可用，啟動 3 秒輪詢
const onInited = async () => {
   // 隱藏「新建」按鈕
  gridRef.buttons.forEach((x) => {
    if (x.name === '新建') {x.hidden = true};
    if (x.name === '編輯') {x.hidden = true};
  });
  gridRef.showCustom = false ;
  gridRef.showCustomSearch = false ;
  gridRef.details[0].pagination = {
    sizes : [30,60,90],
    size: 60,
    sortName : "sub_item",
    order : "asc"
  } 
 details[0].columns.forEach(x=>{
  if (x.field == 'user_id'){
      x.filterData=true ;
     }
  if (x.field == 'dept_id'){
      x.filterData=true ;
     }   
  if (x.field=='sub_no'){
     x.filterData=true ;
     }
  if (x.field=='sub_name'){
     x.filterData=true ;
     }
  if (x.field=='description'){
     x.filterData=true ;
     }
  })

  gridRef.details[0].columnIndex = true ;
  gridRef.details[1].columnIndex = true ;
  // 避免重複啟動：先清除舊計時器
  if (refreshTimer) {
    clearInterval(refreshTimer);
    refreshTimer = null;
  }
  // 每 10 秒自動刷新一次
  // refreshTimer = setInterval(refreshData, 6000);

  // 子表工具列：子任務手動建立
  if (gridRef.details && gridRef.details[0]) {
    gridRef.details[0].buttons?.unshift({
      name: '子任務手動建立',
      icon: 'el-icon-document',
      type: 'primary',
      plain: true,
      onClick: () => {
        // 打開子表新增（不立刻 reload，交給自動刷新或保存成功事件）
        proxy.base.setItem("ins_subitem",'');
        proxy.base.setItem("ins_levelname",'');
        proxy.base.setItem("ins_projectid",'') ;
        proxy.base.setItem("ins_parentId",'');
        subinstanceRef.value?.$refs?.grid?.add?.();
        // 若你的 SubOpen 有自定義保存成功事件，可在那邊調用 onSubSaveSuccess()
      },
    });

    // 子表工具列：重整（手動）
    gridRef.details[0].buttons?.unshift({
      name: '明細表重整',
      icon: 'el-icon-refresh',
      type: 'success',
      plain: true,
      onClick: () => {
        refreshData();
      },
    });
  }

  // 在 status 欄位前插入操作按鈕欄
  const idx = columns.findIndex((x) => x.field === 'status');
  if (idx >= 0) {
    columns.splice(idx, 0, {
      title: '觸發動作',
      field: 'action_area',
      align: 'center',
      width: 250,
      render: (h, { row }) => {
        return (
          <div>
            <el-button
              onClick={($e) => {
                $e.stopPropagation();
                const pjid = Number(row.project_id);
                const typeid = 2;
                const url = `api/sf_project_instance/GenInsPublish?projid=${pjid}&mytypeid=${typeid}`;
                proxy.http.post(url, {}, true).then(() => {});
                proxy.$message.success('點擊了評估計劃');
                if (row.status=='' || row.status == null){
                  row.status=0;
                }
                
              }}
              style="height:23px;padding:0 8px !important;"
              size="small"
              type="success"
              plain
            >
              計劃評估
            </el-button>
            <el-button
              onClick={($e) => {
                $e.stopPropagation();
                const pjid = Number(row.project_id);
                const typeid = 0;
                const url = `api/sf_project_instance/GenInsPublish?projid=${pjid}&mytypeid=${typeid}`;
                proxy.http.post(url, {}, true).then(() => {});
                if (row.status==0){
                  row.status = '';
                }
                proxy.$message.success('點擊了取消評估');
              }}
              style="height:23px;padding:0 8px !important;"
              size="small"
              type="primary"
              plain
            >
              取消評估
            </el-button>

            <el-button
              onClick={($e) => {
                $e.stopPropagation();
                const pjid = Number(row.project_id);
                const typeid = 1;
                const url = `api/sf_project_instance/GenInsPublish?projid=${pjid}&mytypeid=${typeid}`;
                proxy.http.post(url, {}, true).then(() => {});
                // 立即刷新一次（亦可交給 3 秒輪詢）
                if (row.status === null || row.status === '') {
                    row.status = 1 ;
                  }

                // row.status=1 ;
               // refreshData();
              }}
              style="height:23px;padding:0 8px !important;"
              size="small"
              type="danger"
              plain
            >
              發佈專案
            </el-button>
          </div>
        );
      },
    });
  }
   
  details[0].columns.forEach((x) => {
        //设置进度条,
        //更多属性：https://element-plus.gitee.io/zh-CN/component/progress.html
        //这里只是举例,CreateDate换为你当前操作的字段
        if (x.field == 'progress') {
            x.width=120;
            x.title = '實際進度';
            x.render = (h, { row, column, index }) => {
            //  percentage={90} 实际应该修改为：row.字段
             if (row.progress == null) {
                return null;   // 不渲染任何東西
            }
            if (index % 2 === 1) {
                //90改为row.字段
                return <el-progress stroke-width={4} percentage={row.progress} />;
            }
            //10改为row.字段
            return (
                <el-progress
                stroke-width={4}
                color="#67c23a"
                show-text={true}
                percentage={row.progress}
                />
            );
            };
        }
      
          if (x.field == 'dept_id') {
            //给下拉框绑定onChange事件
            x.onChange = (row, column) => {
                //设置第二个下拉框的数据源
                selectChangedept(row, column)
                //这里也可以给其他的字段设置值
                // row.字段=值;
                //也可以给上面的主表表单设置值
                // this.editFormFields.xxx=row.xx;
            }
        }
      
      }
      
      
      )
     const selectChangedept = async (row, column) => {
      // 1) 取得第二個下拉欄位定義
       const col = details?.[0]?.columns?.find(c => c.field === 'user_id');
       if (!col?.bind?.data || !Array.isArray(col.bind.data)) {
           proxy.$message.warning('第二個下拉框尚未綁定資料源或資料格式不正確');
           return;
        }

       // 2) 呼叫 API（這支是 POST，但參數綁定在 querystring）
      //proxy.$message.success('<a>'+gridRef.editFormFields.user_id +'</a>') 
      const url = `api/Sys_User/getDeptUser?code=${row.dept_id}`

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

      // 6) 必要時強制刷新（視框架封裝而定）
      // gridRef?.refresh && gridRef.refresh();  // 若有這方法可打開
      proxy.$forceUpdate?.();

    } catch (e) {
      proxy.$message.error(`載入部門人員數據失敗：${e?.message || e}`);
    }
   };
   
    details[1].columns.forEach((x) => {
         if (x.field == 'project_id') {
            //给下拉框绑定onChange事件
            x.onChange = (row, column) => {
                //设置第二个下拉框的数据源
                selectChangeproj(row, column)
                //这里也可以给其他的字段设置值
                // row.字段=值;
                //也可以给上面的主表表单设置值
                // this.editFormFields.xxx=row.xx;
            }
        }
      });
    const selectChangeproj = async (row, column) => {
      // 1) 取得第二個下拉欄位定義
       const col = details?.[1]?.columns?.find(c => c.field === 'sub_id');
       if (!col?.bind?.data || !Array.isArray(col.bind.data)) {
           proxy.$message.warning('第一個下拉框尚未綁定資料源或資料格式不正確');
           return;
        }

        const col1 = details?.[1]?.columns?.find(c => c.field === 'before_task');
       if (!col1?.bind?.data || !Array.isArray(col1.bind.data)) {
           proxy.$message.warning('第二個下拉框尚未綁定資料源或資料格式不正確');
           return;
        }

       // 2) 呼叫 API（這支是 POST，但參數綁定在 querystring）
      //proxy.$message.success('<a>'+gridRef.editFormFields.user_id +'</a>') 
      const url = `api/sf_project_instance_sub/getprojtask?proj=${row.project_id}`

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

     const next1 = col1.bind.data.map(opt => {
     const canShow1 = allowKeys.has(String(opt.key));
      return {
        ...opt,
        hidden: !canShow1,
        disabled: !canShow1
      };
    });

     // 5) 重新指派陣列以確保響應式更新
     col.bind.data = next;
     col1.bind.data = next1 ; 
      // 6) 必要時強制刷新（視框架封裝而定）
      // gridRef?.refresh && gridRef.refresh();  // 若有這方法可打開
      proxy.$forceUpdate?.();

    } catch (e) {
      proxy.$message.error(`載入專案任務數據失敗：${e?.message || e}`);
    }
   };  
  details[0].columns.find((x) => {
        return x.field == 'sub_tno';
    }).render = (h, { row, column, index }) => {
        return (
            <div>
                <el-button
                    link
                    onClick={($e) => {
                        proxy.base.setItem("ins_subitem",row.sub_item);
                        
                        proxy.base.setItem("ins_projectid",row.project_id) ;
                        if (row.level_name == 4){
                          proxy.base.setItem("ins_parentId",row.parentId);
                          proxy.base.setItem("ins_levelname",row.level_name);
                        } else {
                          proxy.base.setItem("ins_parentId",row.sub_id);
                          proxy.base.setItem("ins_levelname",row.level_name + 1);
                        }
                        
                        subinstanceRef.value?.$refs?.grid?.add?.();
                       // priceBtnClick(1, row, column, index, $e);
                    }}
                    class="bi-plus-circle"
                    style="color: #2196F3;cursor: pointer;"
                ></el-button>
                <el-button
                    link
                    onClick={($e) => {
                        proxy.base.setItem("rework_proj",row.project_id) ;
                        proxy.base.setItem("rework_sub",row.sub_id) ;
                        proxy.base.setItem("rework_user",row.user_id) ;
                        proxy.base.setItem("rework_dept",row.dept_id) ;
                        openreworkRef.value?.$refs?.grid?.add?.();
                    }}
                    class="bi-wrench-adjustable"
                    style="margin-left:1px;color: #2196F3;cursor: pointer;"
                ></el-button>
                <span style="margin-left:5px">{row.sub_no}</span>
            </div>
        );
    };
    gridRef.columns.find((x) => {
        return x.field == 'name';
         }).render = (h, { row, column, index }) => {
        return (
            <div>
                <el-button
                    link
                    onClick={($e) => {
                        const nowday = new Date(); 
                         proxy.base.setItem("closed_projid",row.project_id) ;
                         proxy.base.setItem("closed_userid",row.user_id) ; 
                        projclosedRef.value?.$refs?.grid?.add?.();
                    }}
                    class="bi-c-square"
                    style="margin-left:1px;color: #2196F3;cursor: pointer;"
                ></el-button>
                <span style="margin-left:5px">{row.name}</span>
            </div>
        );
    };
} // oninited end
//触发第二个下拉框联动操作


// 釋放：離開頁面清理計時器
onBeforeUnmount(() => {
  if (refreshTimer) {
    clearInterval(refreshTimer);
    refreshTimer = null;
  }
});

// 其餘生命周期/鉤子保持原行為
const searchBefore = async () => true;
const searchAfter = async () => true;
const addBefore = async () => true;
const updateBefore = async () => true;
const rowClick = () => {};
const modelOpenBefore = async () => true;
const modelOpenAfter = async (row) => {
  // 隱藏明細的「添加行」按鈕
  if (gridRef?.details && gridRef.details[0]) {
    gridRef.details[0].buttons?.forEach((btn) => {
      if (btn.name === '添加行') {
        btn.hidden = true;
      }
    });
  }
  // 暫存專案資訊
  proxy.base.setItem('sf_projectid', row.project_id);
  proxy.base.setItem('sf_projname', row.name);
};

// 對外暴露
defineExpose({});
</script>

<style scoped lang="less">
/* 默認隐藏引用的頁面 */
.proj-open-ref ::v-deep(.layout-container) {
  display: none;
}
.subins-ref ::v-deep(.layout-container) {
  display: none;
}
.openusercal-ref ::v-deep(.layout-container) {
  display: none;
}
.openuserload-ref ::v-deep(.layout-container) {
  display: none;
}
.openrework-ref ::v-deep(.layout-container) {
  display: none;
}
.projclosed-ref ::v-deep(.layout-container) {
  display: none;
}

</style>