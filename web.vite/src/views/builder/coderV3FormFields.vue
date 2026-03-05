<template>
  <div class="ep-text" v-if="!fieldOptions.length">
       <div><i class="el-icon-receiving"></i></div>
    請點擊左邊樹形配置</div>
  <draggable v-model="fieldOptions" @end="end" class="left-draggable-item"
    :group="{ name: 'componentsGroup', pull: 'clone', put: false }" animation="300" @start="onStart" :sort="false"
    :move="onMove">
    <transition-group>
      <div :class="item.field == 1 ? 'item forbid' : 'item'" v-for="item in fieldOptions" :key="item.field">
        <i :class="item.icon"></i> {{ $ts(item.title) }}
      </div>
    </transition-group>
  </draggable>
</template>
<script setup lang="jsx">
import { VueDraggableNext as draggable } from "vue-draggable-next";
import { ref, reactive } from "vue";

const props = defineProps({
  checkEnd: {
    type: Function,
    default: (field) => {
      return true;
    },
  },
});

const emit = defineEmits(["end"]);

const fieldOptions = ref([])

//左邊往右邊拖動時的事件
const end = (e) => {
  if (1 == 1 && e.from !== e.to) {
    if (props.randomField) {
      let obj = Object.assign({}, fieldOptions.value[e.oldIndex]);
      obj.field = new Date().valueOf();
      obj.width = 25;

      obj.readonly = false;
      obj.required = false;
      emit("end", obj);
      return
    }
    emit("end", fieldOptions.value[e.oldIndex]);
  }
};
const onMove = (e, originalEvent) => {
  // this.moveId = e.relatedContext.element.id;
  return props.checkEnd(fieldOptions.value[e.draggedContext.index].field);
};
const drag = ref(null);
const onStart = (e, e1) => {
  return false;
};
const initDraggableFields = (options) => {

  //  fieldOptions.value.splice(0)
  fieldOptions.value = options
  // console.log(  fieldOptions.value)
}
defineExpose({
  initDraggableFields
})
</script>
<style lang="less" scoped>
.drag-center {
  display: flex;
  flex-direction: column;
  flex: 1;
}

.left-draggable-item {
  //   flex: 1;
  display: inline-block;
  padding: 5px;
  //   border-right: 1px solid #eee;
}

.left-draggable-item .item {
  cursor: move;
  float: left;
  width: 97px;
  text-align: center;
  border: 1px dashed #d9daff;
  padding: 2px 6px;
  text-align: left;
  line-height: 28px;
  margin: 4px;
  border-radius: 3px;
  background: #ebedff;
  font-size: 13px;
  color: #000000;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.item2:hover,
.actived {
  background: #f0f9eb;
}

.item:hover {
  border: 1px dashed #787be8;
  color: #787be8;
  background: #daddfc;
}

.ep-text {
  font-size: 15px;
  color: #737272;
  text-align: center;
  margin-top: 150px;
  letter-spacing: 1px;
  i{
    margin-top: 10px;
       color: #e2e2e2;
        font-size: 35px;
  }
}
</style>
