using System.Collections.Generic;
using System.Threading.Tasks;
using VolPro.Core.BaseProvider;
using VolPro.Entity.DomainModels;

namespace VolPro.Sys.IServices
{
    public partial interface ISys_DictionaryService
    {
        /// <summary>
        /// 代碼生成器获取所有字典项(超级管理權限)
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetBuilderDictionary();
        object GetVueDictionary(string[] dicNos);
        object GetTableDictionary(Dictionary<string, object[]> keyData);
        object GetSearchDictionary(string dicNo, string value);

        /// <summary>
        /// 表單設置為远程查詢，重置或第一次添加表單時，获取字典的key、value
        /// </summary>
        /// <param name="dicNo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<object> GetRemoteDefaultKeyValue(string dicNo, string key);
    }
}

