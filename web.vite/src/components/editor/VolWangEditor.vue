<template>
  <div ref="editorRef"></div>
</template>

<script setup>
const props = defineProps({
  url: {
    // 上傳圖片的url
    type: String,
    default: "",
  },
  upload: {
    // 上傳方法
    type: Function,
    // (file, insertImgFn) => {}
    default: null,
  },
  uploadCount: {
    // 最多可以上傳(圖片)的數量
    type: Number,
    default: 3,
  },
  modelValue: {
    type: String,
    default: "",
  },
  width: {
    type: String,
    default: "100%",
  },
  height: {
    type: Number,
    default: 250,
  },
  minWidth: {
    type: Number,
    default: 650,
  },
  minHeight: {
    type: Number,
    default: 100,
  },
});
import E from "wangeditor";
import {
  ref,
  getCurrentInstance,
  watchEffect,
  onMounted,
  onUnmounted,
  nextTick,
} from "vue";

const { proxy } = getCurrentInstance();

const emits = defineEmits(["update:modelValue"]);

const editorRef = ref(null);
const editor = ref(null);
const lastHtml = ref("");
const isEditorReady = ref(false);

// 創建編輯器實例
const createEditor = () => {
  editor.value = new E(editorRef.value);

  // 編輯器基本配置
  editor.value.config.zIndex = 500;
  editor.value.config.height = props.height;

  // 内容變化時觸發事件
  editor.value.config.onchange = (html) => {
    // 避免初始渲染時觸發不必要的更新
    if (!isEditorReady.value) {
      isEditorReady.value = true;
      lastHtml.value = html;
      return;
    }

    // 只有當内容真正變化時才更新
    if (html !== lastHtml.value) {
      lastHtml.value = html;
      emits("update:modelValue", html);
    }
  };

  // 自定義圖片上傳
  editor.value.config.customUploadImg = function (resultFiles, insertImgFn) {
    if (props.upload) {
      // 使用自定義上傳方法
      resultFiles.map((item) => {
        props.upload(item, insertImgFn);
      });
    } else {
      // 使用默認上傳邏輯
      const formData = new FormData();
      const nameArr = [];

      resultFiles.forEach(function (file) {
        formData.append("fileInput", file, file.name);
        nameArr.push(file.name);
      });

      if (!props.url) {
        console.error("未配置上傳URL");
        return;
      }

      proxy.http
        .post(props.url, formData, true, {
          headers: { "Content-Type": "multipart/form-data" },
        })
        .then((x) => {
          if (!x.status) {
            console.error(x.message);
            return;
          }

          nameArr.forEach((m) => {
            insertImgFn(proxy.http.ipAddress + x.data + m);
          });
        });
    }
  };

  // 創建編輯器
  editor.value.create();

  // 初始化内容
  nextTick(() => {
    if (props.modelValue) {
      editor.value.txt.html(props.modelValue);
      lastHtml.value = props.modelValue;
    }
    isEditorReady.value = true;
  });
};

// 監聽外部傳入的 modelValue 變化
watchEffect(() => {
  if (isEditorReady.value && props.modelValue !== lastHtml.value) {
    // 使用 nextTick 確保 DOM 更新完成
    nextTick(() => {
      editor.value.txt.html(props.modelValue);
      lastHtml.value = props.modelValue;
    });
  }
});

// 生命周期钩子
onMounted(() => {
  createEditor();
});

onUnmounted(() => {
  if (editor.value) {
    editor.value.destroy();
    editor.value = null;
  }
});
</script>

<style scoped></style>
