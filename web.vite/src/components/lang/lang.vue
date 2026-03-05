<template>
  <div v-if="$global.lang">
    <el-dropdown trigger="hover">
      <el-button link class="el-dropdown-link" style="outline: none" :style="{ color: color }">
        {{ getText }}
        <i class="el-icon-arrow-down"></i>
      </el-button>
      <template #dropdown>
        <el-dropdown-menu>
          <el-dropdown-item :disabled="lang == item.value" v-for="(item, index) in list" :key="index">
            <div @click="onClick(item.value)">
              {{ item.text }}
            </div>
          </el-dropdown-item>
        </el-dropdown-menu>
      </template>
    </el-dropdown>
  </div>
</template>
<script setup>
import { ref, computed } from 'vue'
import { currentLang } from './index.js'
const props = defineProps({
  direction: {
    type: String,
    default: 'horizontal'
  },
  color: {
    type: String,
    default: 'white'
  }
})
const lang = ref({})
lang.value = currentLang()
console.log(lang.value)
const list = ref([
  { name: '繁體中文', text: '繁體中文', value: 'zh-tw' },
  { name: '簡體中文', text: '簡體中文', value: 'zh-cn' },
  { name: '英語', text: 'English', value: 'en' },
  { name: '越南語', text: 'Tiếng Việt', value: 'vi' },
  { name: '法語', text: 'Français', value: 'fr' },
  { name: '西班牙語', text: 'Español', value: 'es' }
  // { name: "阿拉伯語", text: " الصينية التقليدية", value: "ar" },
  // { name: "俄語", text: " русский язык", value: "ru" },
])
const getText = computed(() => {
  //const data = list.value.find(c => c.value === lang.value)
  const data = list.value.find(c => c.value === 'zh_tw')
  return data ? data.text : '繁體中文'
})
const onClick = (key) => {
  lang.value = key
  localStorage.setItem(lang_storage_key, key)
  location.reload()
}

defineExpose({
  onClick
})
</script>

<style scoped>
.el-dropdown-link {
  color: white;
  font-size: 14px !important;
  line-height: 60px;
  height: 60px;
  /* padding: 10px 0; */
}
</style>
