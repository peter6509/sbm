export default function(){
        const editFormFields = {"Name":"","Address":"","OrderCode":"","Age":""};
        const editFormOptions = [[{"title":"名字","field":"Name"}],
                              [{"title":"地址","field":"Address"}],
                              [{"dataKey":"訂單狀態","data":[],"title":"編號","field":"OrderCode","type":"select"}],
                              [{"title":"Age","field":"Age","type":"number"}]];
        const tableName="Test2025";
        const tableCNName="測試表2025";
        const newTabEdit=true;
        const key='Id';
        const detail = {
            cnName: "",
            table: "",
            url:"api/Test2025/getDetailPage",
            columns: [],
            sortName: "",
            key: ""
        };

       const details=[]

    return {
        key,
        tableName,
        tableCNName,
        editFormFields,
        editFormOptions,
        detail,
        details,
        newTabEdit
    }
}