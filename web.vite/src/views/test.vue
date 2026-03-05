<template>
    <div>
        <iframe v-once src="https://www.163.com"></iframe>
    </div>
</template>
<script lang="ts">
export default{
    created(){
        console.log('111')
    },
    // watch:{
    //    '$route': { // $route可以用引號，也可以不用引號
    //         handler(to, from){
  
    //         console.log('當前頁面路由：' + to.params.id); //新路由信息

    //         },
    //         deep: true, // 深度观察監聽
    //         immediate: true, // 第一次初始化渲染就可以監聽到
    //     }
    // }
}
</script>
