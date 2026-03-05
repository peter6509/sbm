<template>
    <div class="form-content" :style="{ height: height + 'px' }">
        <div class="ep-text">
            <div><i class="el-icon-receiving"></i></div>
            移動端請在代碼生成器v2版本上配置(勾選App列)
        </div>
    </div>

</template>
<script lang="jsx" setup>
  const pros = defineProps({
    height: {
        type: Number,
        default: 100,
    }
});
</script>
<style lang="less" scoped>
.form-content {
    position: relative;
    // background: #fff;
    padding: 0 !important;
}

.ep-text {
    font-size: 15px;
    color: #737272;
    text-align: center;
    position: absolute;
    width: 100%;
    top: 150px;
    letter-spacing: 1px;

    i {
        margin-top: 10px;
        color: #e2e2e2;
        font-size: 35px;
    }
}

.custom-title {
    display: flex;
    height: 44px;
    align-items: center;

    button {
        margin-left: 15px;
    }

    .attr-item {
        align-items: center;
        display: flex;
        font-weight: 400;
    }

    .attr-item-title {
        color: #0247de;
        margin-right: 10px;
        font-size: 13px;
    }
}
</style>
