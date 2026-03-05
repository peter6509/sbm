<template>
  <div class="vol-dialog">
    <el-dialog
      v-model="vmodel"
      :destroy-on-close="destroyOnClose"
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      :width="width"
      :fullscreen="fullscreen"
      :draggable="draggable||$global.boxDraggable"
      :modal="modal"
      :before-close="handleClose"
    >
      <template #header>
        <i :class="icon"></i> {{ $ts(title) }}
        <slot name="header"></slot>

        <button
          v-if="showFullscreen && showFull"
          class="el-dialog__headerbtn"
          type="button"
          style="right: 65px; color: var(--el-color-info)"
          @click="handleFullScreen"
        >
          <i class="el-icon el-icon-full-screen" :title="$ts('最大化')"></i>
        </button>
        <button
          class="el-dialog__headerbtn"
          type="button"
          style="right: 35px; color: var(--el-color-info)"
          @click="minimizeClick"
        >
          <i class="el-icon-minus" :title="$ts('最小化')"></i>
        </button>
      </template>
      <el-scrollbar :max-height="contentHeight">
        <div
          v-if="inited"
          class="srcoll-content"
          :style="{ padding: padding + 'px', height: height ? height + 'px' : 'auto' }"
        >
          <slot name="content"></slot>
          <slot></slot>
        </div>
      </el-scrollbar>
      <template #footer>
        <div class="dia-footer" v-if="footer">
          <slot name="footer"></slot>
          <el-button type="primary" v-if="!footer" size="mini" @click="handleClose()"
            ><i class="el-icon-close"></i>{{ $ts("關閉") }}</el-button
          >
        </div>
      </template>
    </el-dialog>
    <div class="minimize" v-if="minimize">
      <div class="fx-1"><i class="el-icon-warning-outline"></i>{{ $ts(title) }}</div>
      <div>
        <el-button type="default" text @click="restoreDialog">
          <i class="el-icon-copy-document" size="small" :title="$ts('還原')"></i
        ></el-button>
        <el-button type="default" text @click="minimizeCloseClick">
          <i class="el-icon-close" :title="$ts('關閉')"></i
        ></el-button>
      </div>
    </div>
  </div>
</template>

<script>
import { defineComponent, getCurrentInstance, ref, watch, watchEffect } from "vue";

export default defineComponent({
  props: {
    modelValue: false,
    lazy: {
      //是否開啟懒加載2020.12.06
      type: Boolean,
      default: false,
    },
    icon: {
      type: String,
      default: "el-icon-warning-outline",
    },
    title: {
      type: String,
      default: "基本信息",
    },
    height: {
      type: Number,
      default: 0,
    },
    width: {
      type: Number,
      default: 650,
    },
    padding: {
      type: Number,
      default: 16,
    },
    modal: {
      //是否需要遮罩層
      type: Boolean,
      default: true,
    },
    draggable: {
      //啟用可拖拽功能
      type: Boolean,
      default: false,
    },
    mask: {
      type: Boolean,
      default: true,
    },
    onModelClose: {
      //2021.07.11增加彈出框關閉事件
      type: Function,
      default: (iconClick) => {
        return true;
      },
    },
    footer: {
      //是否顯示底部按鈕
      type: Boolean,
      default: true,
    },
    full: {
      type: Boolean,
      default: false,
    },
    showFull: {
      type: Boolean,
      default: true,
    },
    destroyOnClose: {
      //當關閉 Dialog 時，銷毁其中的元素
      type: Boolean,
      default: false,
    },
  },
  setup(props, context) {
    const clientHeight = document.body.clientHeight * 0.95 - 60;
    const minimize = ref(false);
    const inited = ref(true);
    const vmodel = ref(false);
    const footer = ref(false);
    const top = ref(100);
    vmodel.value = props.modelValue;
    footer.value = !!context.slots.footer;
    const contentHeight = ref(200);
    contentHeight.value = props.height;
    const handleClose = (done, iconClose) => {
      let result = props.onModelClose(!!iconClose);
      if (result === false) return;
      vmodel.value = false;
      context.emit("update:modelValue", false);
      done && done();
    };
    const calcHeight = (val) => {
      //  if (props.height > clientHeight) {
      //  if(true){
      contentHeight.value = clientHeight - 30;
      return clientHeight / -2 + "px";
      //  }
      // contentHeight.value = val || props.height;
      // return (props.height + 56) / -2 + 'px';
    };
    top.value = calcHeight();
    watch(
      () => props.modelValue,
      (newVal, oldVal) => {
        vmodel.value = newVal;
        if (newVal) {
          minimize.value = false;
        }
      }
    );
    watch(
      () => props.height,
      (newVal, oldVal) => {
        top.value = calcHeight();
      }
    );
    const { proxy } = getCurrentInstance();
    const fullscreen = ref(false);
    const showFullscreen = ref(true);

    if (typeof proxy.$global.fullscreen == "boolean") {
      showFullscreen.value = proxy.$global.fullscreen;
    }
    fullscreen.value = props.full;

    const handleFullScreen = () => {
      fullscreen.value = !fullscreen.value;
      context.emit("fullscreen", fullscreen.value);
    };
    //通過全局計算minimize開啟的數量来處理最小化的位置
    const minimizeClick = () => {
      vmodel.value = false;
      minimize.value = true;
      context.emit("update:modelValue", false);
    };

    const restoreDialog = () => {
      minimize.value = false;
      vmodel.value = true;
      context.emit("update:modelValue", true);
    };
    const minimizeCloseClick = () => {
      minimize.value = false;
      handleClose(null, true);
    };

    return {
      handleClose,
      minimize,
      inited,
      vmodel,
      footer,
      top,
      calcHeight,
      contentHeight,
      fullscreen,
      showFullscreen,
      handleFullScreen,
      minimizeClick,
      restoreDialog,
      minimizeCloseClick,
    };
  },
});
</script>

<style lang="less" scoped>
.dia-footer {
  text-align: right;
  width: 100%;
  border-top: 1px solid #f1f1f1;
  text-align: right;
  padding: 6px 8px;
}
</style>

<style scoped lang="less">
.vol-dialog ::v-deep(.el-overlay-dialog) {
  display: flex !important;
}

.vol-dialog ::v-deep(.el-dialog) {
  margin: auto;
}

.vol-dialog ::v-deep(.el-dialog) {
  border-top-left-radius: 4px;
  border-top-right-radius: 4px;
}

.vol-dialog ::v-deep(.el-dialog__header) {
  border-top-left-radius: 4px;
  border-top-right-radius: 4px;
  padding: 0px 13px;
  line-height: 53px;
  border-bottom: 1px solid #f1f1f1;
  height: 50px;
  color: rgb(79, 79, 79);
  font-weight: bold;
  font-size: 14px;
  margin: 0;
  // background-image: linear-gradient(135deg, #0cd7bd 10%, #50c3f7);
}

.vol-dialog ::v-deep(.el-dialog__footer),
.vol-dialog ::v-deep(.el-dialog__body) {
  padding: 0;
}

.vol-dialog ::v-deep(.el-dialog__headerbtn) {
  top: 0;
  padding-top: 8px;
  height: 50px;
  width: 0;
  padding-right: 30px;
  padding-left: 5px;
}
// .vol-dialog ::v-deep(.el-dialog__headerbtn .el-dialog__close) {
//   color: #fff;
// }
.minimize {
  z-index: 9999;
  background: #ffff;
  min-width: 200px;
  display: flex;
  padding: 10px;
  font-size: 13px;
  bottom: 15px;
  right: 15px;
  border: 1px solid var(--el-notification-border-color);
  position: fixed;
  background-color: var(--el-bg-color-overlay);
  box-shadow: 0px 2px 8px 3px #eee;
  border-radius: 5px;
  align-items: center;
  .fx-1 {
    flex: 1;
    padding-right: 10px;
    i {
      margin-right: 3px;
    }
  }
  ::v-deep(button) {
    color: #000;
    padding: 0 !important;
    font-size: 14px !important;
    font-weight: bolder;
    height: 22px;
  }
}
</style>
