using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VolPro.Core.DBManager;
using VolPro.Core.Extensions;
using VolPro.Core.Infrastructure;
using VolPro.Entity.DomainModels;

namespace VolPro.Core.WorkFlow
{
    public static class WorkFlowGeneric
    {
        /// <summary>
        /// 获取下一個节點
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workflow"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static List<Sys_WorkFlowTableStep> GetNextStep<T>(this Sys_WorkFlowTable workflow, Sys_WorkFlowTableStep current) where T : Sys_WorkFlowTable
        {
            var list = workflow.Sys_WorkFlowTableStep.Where(x => x.StepId == current.NextStepId).ToList();
            //下一個节點抄送
            if (list.Exists(x => x.StepAttrType == StepType.cc.ToString()))
            {
                list = workflow.Sys_WorkFlowTableStep.Where(x => list.Any(c => c.NextStepId == x.StepId)).ToList();
            }
            return list;
        }
        /// <summary>
        /// 获取上一個节點
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="workflow"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public static List<Sys_WorkFlowTableStep> GetPreStep<T>(this Sys_WorkFlowTable workflow, Sys_WorkFlowTableStep current) where T : Sys_WorkFlowTable
        {
            var list = workflow.Sys_WorkFlowTableStep.Where(x => x.NextStepId == current.StepId).ToList();
            //上一個节點抄送
            if (list.Exists(x => x.StepAttrType == StepType.cc.ToString()))
            {
                list = workflow.Sys_WorkFlowTableStep.Where(x => list.Any(c => c.StepId == x.NextStepId)).ToList();
            }
            return list;
        }
        public static string GetNextStepId<T>(this Sys_WorkFlowTable workflow, Sys_WorkFlowTableStep current) where T : Sys_WorkFlowTable
        {
            return workflow.GetNextStep<Sys_WorkFlowTable>(current).Select(x => x.StepId).FirstOrDefault();
        }


        public static void CreateTitleTemplate<Entity>(Entity entity, Sys_WorkFlowTable flowTable, Sys_WorkFlow workflow)
        {
            if (string.IsNullOrEmpty(workflow.TitleTemplate))
            {
                return;
            }

            var placeholders = new List<string>();
            var matches = Regex.Matches(workflow.TitleTemplate, @"\{#(\w+)\}");
            foreach (Match match in matches)
            {
                string placeholder = match.Groups[0].Value;
                string key = match.Groups[1].Value;

                placeholders.Add(key);
            }
            var tableOptions = DBServerProvider.DbContext.Set<Sys_TableColumn>()
                .Where(c => c.TableName == workflow.WorkTable && placeholders.Contains(c.ColumnName))
                           .Select(s => new
                           {
                               s.ColumnName,
                               s.ColumnCnName,
                               s.DropNo,
                               isDate = s.IsImage == 4,
                               s.ColumnType,
                               s.EditRowNo,
                               s.EditType,
                               s.IsNull
                           }).ToList();

            List<Sys_Dictionary> dictionaries = new List<Sys_Dictionary>();

            var dicNos = tableOptions.Select(s => s.DropNo).ToList();
            if (dicNos.Count > 0)
            {
                dictionaries = DictionaryManager.GetDictionaries(dicNos, true).ToList();
            }

            var properties = typeof(Entity).GetProperties().Where(x => placeholders.Contains(x.Name));

            string titleTemplate = workflow.TitleTemplate;
            foreach (var property in properties)
            {
                string field = property.Name;
                string placeholder = $"{{#{field}}}";
                string value = property.GetValue(entity)?.ToString();

                var option = tableOptions.Where(x => x.ColumnName == field).FirstOrDefault();
                if (option != null)
                {
                    if (option.isDate)
                    {
                        value = value.GetDateTime().Value.ToString("yyyy-MM-dd");
                    }
                    else if (option.ColumnType == "DateTime")
                    {
                        value = value.GetDateTime().Value.ToString("yyyy-MM-dd HH:mm:sss");
                    }
                    else if (!string.IsNullOrEmpty(option.DropNo))
                    {
                        string val = null;
                        if (option.EditType == "selectList" || option.EditType == "checkbox" || option.EditType == "treeSelect")
                        {
                            string[] arr = value.Split(",");
                            arr = dictionaries.Where(c => c.DicNo == option.DropNo).FirstOrDefault()
                                   ?.Sys_DictionaryList
                                   ?.Where(c => arr.Contains(c.DicValue))?.Select(s => s.DicName)
                                   .ToArray();
                            val = string.Join(",", arr);
                        }
                        else
                        {
                            val = dictionaries.Where(c => c.DicNo == option.DropNo).FirstOrDefault()
                             ?.Sys_DictionaryList
                             ?.Where(c => c.DicValue == value)?.Select(s => s.DicName)
                             .FirstOrDefault();
                        }
                        if (!string.IsNullOrEmpty(val))
                        {
                            value = val;
                        }
                    }
                }
                titleTemplate = titleTemplate.Replace(placeholder, value ?? "");
            }
            flowTable.TitleTemplate = titleTemplate;
        }
    }
}
