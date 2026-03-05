
const loadTableOptions = (proxy, props) => {
    const opsUrl = `api/builder/loadOptions?tableId=${props.coderTableId}&table=${props.coderTableId ? '' : props.table.name}`
    proxy.http.ajax({
        url: opsUrl,
        async: false,
        success: (res) => {
            if (!res.status) return proxy.$message.error(res.content)
              //  console.log(res.content)
            const wrappedCode = `
            (function() {
                ${res.content}
                return defaultExport;
            })() `;
            const getDefaultExport = new Function(`
            let defaultExport;
            ${wrappedCode.replace('export default', 'defaultExport =')}
            return defaultExport;
           `);
            const options = getDefaultExport()();
            Object.assign(props.table, options.table)

            props.columns.splice(0);
            props.columns.push(...options.columns)


            Object.assign(props.detail, options.detail)

            props.details.splice(0);
            props.details.push(...options.details)

            Object.assign(props.editFormFields, options.editFormFields)
            props.editFormOptions.splice(0);
            props.editFormOptions.push(...options.editFormOptions)
            //console.log(options.editFormOptions)

            Object.assign(props.searchFormFields, options.searchFormFields)
            props.searchFormOptions.splice(0);
            props.searchFormOptions.push(...options.searchFormOptions)
        }
    })
}
const dyKey = 'dyTableData';
//動態頁面
const isDyPage = (proxy, props) => {
    const data = proxy.base.getItem(dyKey);
    if (!data||!data.length) {
        return false;
    }
    return data.some(x => { return x.table == props.table.name && x.dyPage })
}

//動態脚本
const loadDyScript = (proxy, props, dataConfig) => {
    const data = proxy.base.getItem(dyKey);
    if (!data || !data.dyScript) {
        return;
    }
    proxy.http.ajax({
        url: 'api/builder/getDyScript?table=' + props.table.name,
        async: false,
        success: (res) => {
            if (!res.data) {
                return;
            }
            // dataConfig.dyScript=res.data;
        }
    })
}
//獲取有動態頁面加載、動態脚本的表
const loadDyTable = (proxy) => {
    const data = proxy.base.getItem(dyKey);
    if (data) {
        return;
    }
    proxy.http.ajax({
        url: 'api/builder/getDyTable',
        async: false,
        success: (res) => {
            proxy.base.setItem(dyKey, res);
        }
    })
}
export { loadTableOptions, loadDyTable, loadDyScript, isDyPage }