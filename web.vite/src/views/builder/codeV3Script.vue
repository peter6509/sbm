<template>
    <div class="sc-content" :style="{ height: height + 'px' }">

        <!-- <div style="padding:0px;margin-bottom:10px">
            <el-alert style="margin-bottom: -1px" title="配置js脚本處理頁面事件邏輯,不需要發布編譯實時生效,更多方法配置見生成頁面文檔" class="alert-primary"
                :closable="false"></el-alert>
        </div> -->
        <div class="code-editor">
            <div class="editor-toolbar">
                <!-- <select v-model="selectedLanguage" @change="handleLanguageChange">
                    <option value="javascript">JavaScript</option>
                    <option value="html">HTML</option>
                    <option value="css">CSS</option>
                    <option value="python">Python</option>
                    <option value="json">JSON</option>
                </select> -->
                <el-button link icon="Document" @click="setDefaultScript">默認脚本</el-button>
                <el-button link icon="Delete" @click="clearDefaultCode">清除代碼</el-button>
            </div>
            <MonacoEditor v-model:value="code" :language="language" :options="editorOptions"
                @editor-mounted="handleEditorMounted" :height="height + 'px'" />
        </div>
    </div>
</template>
<script lang="jsx" setup>
import MonacoEditor from '@guolao/vue-monaco-editor'
import * as monaco from 'monaco-editor'
import { ref, reactive, getCurrentInstance, watch, defineAsyncComponent } from "vue";
const pros = defineProps({
    height: {
        type: Number,
        default: 100,
    },
    fieldOptions: {
        type: Array,
        default: () => {
            return [];
        },
    }
});



const defaultScript = `/*
 * @Description:配置js脚本處理頁面事件邏輯,不需要編譯發布,修改后實時生效,更多方法配置見框架文檔
 */

function extension() {
    const onInit = async () => {
        //框架初始化配置前
    }
    const onInited = async () => {
        //框架初始化配置后
        //如果要配置明细表,在此方法操作
        //this.detailOptions.columns.forEach(column=>{ });
    }
    const searchBefore = async (param) => {
        //界面查詢前,可以给param.wheres添加查詢参數,返回false,則不會執行查詢
        return true;
    }
    const searchAfter = async (result) => {
        //查詢后result返回的查詢數據
        return true;
    }
    const addBefore = async (formData) => {
        //新建保存前formData為對象,包括明细表
        return true;
    }
    const updateBefore = async (formData) => {//編輯保存前formData為對象，包括明细表、删除行的Id
        return true;
    }
    const rowClick = ({ row, column, event }) => {
        //查詢界面點擊行事件
    }
    const modelOpenAfter = (row, currentAction, isCopyClick) => {
        //點擊編輯、新建按鈕彈出框后，可以在此處寫邏輯，如，從后台獲取數據
    }
    return { onInit, onInited, searchBefore, searchAfter, addBefore, updateBefore, rowClick, modelOpenAfter}
}
   `;

// 編輯器内容
const code = ref('')


const setDefaultScript = () => {
    code.value = defaultScript;
}

const clearDefaultCode = () => {
    code.value = ''
}
// 當前語言
const language = ref('javascript')
const selectedLanguage = ref('javascript')

// 保存編輯器實例
let editorInstance = null

// 編輯器配置
const editorOptions = {
    fontSize: 14,
    tabSize: 2,
    autoIndent: true,
    minimap: { enabled: false }, // 隐藏迷你地圖
    scrollBeyondLastLine: false,
    'javascript.validate.enable': false,
    // 額外關閉 TypeScript 對 JS 的類型檢查（如果有）
    'typescript.validate.enable': false,
    // 禁用内置诊断（错误提示）
    'diagnosticsOptions': {
        'javascript': {
            'validate': false
        }
    },
    roundedSelection: false,
    readOnly: false, // 是否只讀
    lineNumbers: 'on', // 顯示行號
    lineDecorationsWidth: 10,
    folding: true, // 支持代碼折叠
    theme: 'vs-dark' // 主題：vs, vs-dark, hc-black
}

// 編輯器挂載完成
const handleEditorMounted = (editor) => {
    editorInstance = editor
    console.log('編輯器已初始化')
    // 彻底關閉當前編輯器實例的語法校驗
    monaco.languages.typescript.javascriptDefaults.setDiagnosticsOptions({
        noSemanticValidation: true,
        noSyntaxValidation: true
    })
}

// 切换語言
const handleLanguageChange = () => {
    language.value = selectedLanguage.value
    // 根據語言重置示例代碼
    resetSampleCode()
}

// 重置示例代碼
const resetSampleCode = () => {

    code.value = `// JavaScript 示例
function calculateSum(a, b) {
  return a + b;
}

const result = calculateSum(5, 3);
console.log(\`計算结果: \${result}\`);`

}

// 格式化代碼
const formatCode = () => {
    if (!editorInstance) return

    editorInstance.executeEdits('', [
        {
            range: editorInstance.getModel().getFullModelRange(),
            text: code.value
        }
    ])

    // 觸發格式化
    monaco.editor.executeEdits('', [])
    editorInstance.getAction('editor.action.formatDocument').run()
}

// 清空代碼
const clearCode = () => {
    code.value = ''
}

// 監聽語言變化，動態更新編輯器語言
watch(language, (newLang) => {
    if (editorInstance) {
        monaco.editor.setModelLanguage(editorInstance.getModel(), newLang)
    }
})
</script>
<style lang="less" scoped>
.sc-content {
    background: #fff;
}

.code-editor {
    border: 1px solid #e0e0e0;
    border-radius: 6px;
    overflow: hidden;
}

.editor-toolbar {
    padding: 6px;
    background-color: #ffffffc7;
    display: flex;
    // gap: 10px;
    align-items: center;
}

.editor-toolbar {
    position: absolute;
    z-index: 99;
    right: 11px;
    background: #fff;
    border-bottom-left-radius: 5px;
}
</style>