using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace VolPro.Core.Extensions
{

    public static class ObjectExtension
    {

        public static bool DicKeyIsNullOrEmpty(this Dictionary<string, object> dic, string key)
        {
            if (dic == null)
                return true;
            if (!dic.ContainsKey(key)) return true;
            object value = dic[key];
            if (value == null || value.ToString() == "")
            {
                return true;
            }
            return false;
        }
        public static Dictionary<string, object> ReaderToDictionary(this IDataReader Reader)
        {
            List<Dictionary<string, object>> rowList = Reader.ReaderToDictionaryList();
            return rowList.Count() > 0 ? rowList[0] : null;
        }
        /// <summary>
        /// IDataReader转換成DictionaryList
        /// </summary>
        /// <param name="Reader"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ReaderToDictionaryList(this IDataReader Reader)
        {
            List<Dictionary<string, object>> rowList = new List<Dictionary<string, object>>();
            try
            {
                while (Reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    for (var fieldCount = 0; fieldCount < Reader.FieldCount; fieldCount++)
                    {
                        row.Add(Reader.GetName(fieldCount), Reader[fieldCount]);
                    }
                    rowList.Add(row);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                Reader.Close();
                Reader.Dispose();
            }
            return rowList;
        }

        public static T DicToEntity<T>(this Dictionary<string, object> dic)
        {
            return new List<Dictionary<string, object>>() { dic }.DicToList<T>().ToList()[0];
        }
        public static List<T> DicToList<T>(this List<Dictionary<string, object>> dicList)
        {
            return dicList.DicToIEnumerable<T>().ToList();
        }
        public static object DicToList(this List<Dictionary<string, object>> dicList, Type type)
        {
            return typeof(ObjectExtension).GetMethod("DicToList")
               .MakeGenericMethod(new Type[] { type })
               .Invoke(typeof(ObjectExtension), new object[] { dicList });
        }

        public static IEnumerable<T> DicToIEnumerable<T>(this List<Dictionary<string, object>> dicList)
        {
            foreach (Dictionary<string, object> dic in dicList)
            {
                T model = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in model.GetType()
                    .GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!dic.TryGetValue(property.Name, out object value)) continue;
                    property.SetValue(model, value?.ToString().ChangeType(property.PropertyType), null);
                }
                yield return model;
            }
        }
        /// <summary>
        /// IDataReader转換成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Reader"></param>
        /// <returns></returns>
        public static List<T> ReaderToList<T>(this IDataReader Reader)
        {
            List<string> objectField = new List<string>(Reader.FieldCount);
            for (int i = 0; i < Reader.FieldCount; i++)
            {
                objectField.Add(Reader.GetName(i).ToLower());
            }
            List<T> objectList = new List<T>();
            try
            {
                while (Reader.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in model.GetType()
                        .GetProperties(BindingFlags.GetProperty
                        | BindingFlags.Public
                        | BindingFlags.Instance))
                    {
                        if (!objectField.Contains(property.Name.ToLower())) { continue; }
                        if (Reader[property.Name].IsNullOrEmpty()) { continue; }
                        property.SetValue(model, Reader[property.Name].ToString().ChangeType(property.PropertyType), null);
                    }
                    objectList.Add(model);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                Reader.Close();
                Reader.Dispose();
            }
            return objectList;
        }


        public static object ChangeType(this object value, Type type)
        {
            if (null == value) return null;

            try
            {
                if (type == typeof(Guid) || type == typeof(Guid?))
                {
                    string val = value.ToString();
                    if (val == "") return null;
                    return Guid.Parse(val);
                }
                if (type == typeof(bool) || type == typeof(bool?))
                {
                    string val = value.ToString();
                    return val == "1" || val == "True";
                }
                if (!type.IsGenericType) return Convert.ChangeType(value, type);
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        /// <summary>
        /// 將集合转換為數據集。
        /// </summary>
        /// <typeparam name="T">转換的元素類型。</typeparam>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否生成泛型數據集。</param>
        /// <returns>數據集。</returns>
        public static DataSet ToDataSet<T>(this IEnumerable<T> list, bool generic = true)
        {
            return ListToDataSet(list, generic);
        }

        /// <summary>
        /// 將集合转換為數據集。
        /// </summary>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否生成泛型數據集。</param>
        /// <returns>數據集。</returns>
        public static DataSet ToDataSet(this IEnumerable list, bool generic = true)
        {
            return ListToDataSet(list, generic);
        }

        /// <summary>
        /// 將集合转換為數據集。
        /// </summary>
        /// <typeparam name="T">转換的元素類型。</typeparam>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否生成泛型數據集。</param>
        /// <returns>數據集。</returns>
        public static DataSet ToDataSet<T>(this IEnumerable list, bool generic = true)
        {
            return ListToDataSet(list, typeof(T), generic);
        }

        /// <summary>
        /// 將實例转換為集合數據集。
        /// </summary>
        /// <typeparam name="T">實例類型。</typeparam>
        /// <param name="o">實例。</param>
        /// <param name="generic">是否生成泛型數據集。</param>
        /// <returns>數據集。</returns>
        public static DataSet ToListSet<T>(this T o, bool generic = true)
        {
            if (o is IEnumerable)
            {
                return ListToDataSet(o as IEnumerable, generic);
            }
            else
            {
                return ListToDataSet(new T[] { o }, generic);
            }
        }

        /// <summary>
        /// 將可序列化實例转換為XmlDocument。
        /// </summary>
        /// <typeparam name="T">實例類型。</typeparam>
        /// <param name="o">實例。</param>
        /// <returns>XmlDocument。</returns>
        public static XmlDocument ToXmlDocument<T>(this T o)
        {
            XmlDocument xmlDocument = new XmlDocument
            {
                InnerXml = o.ToListSet().GetXml()
            };
            return xmlDocument;
        }

        /// <summary>
        /// 將集合转換為數據集。
        /// </summary>
        /// <param name="list">集合。</param>
        /// <param name="t">转換的元素類型。</param>
        /// <param name="generic">是否生成泛型數據集。</param>
        /// <returns>转換后的數據集。</returns>
        private static DataSet ListToDataSet(IEnumerable list, Type t, bool generic)
        {
            DataSet ds = new DataSet("Data");
            if (t == null)
            {
                if (list != null)
                {
                    foreach (var i in list)
                    {
                        if (i == null)
                        {
                            continue;
                        }
                        t = i.GetType();
                        break;
                    }
                }
                if (t == null)
                {
                    return ds;
                }
            }
            ds.Tables.Add(t.Name);
            //如果集合中元素為DataSet扩展涉及到的基本類型時，進行特殊转換。
            if (t.IsValueType || t == typeof(string))
            {
                ds.Tables[0].TableName = "Info";
                ds.Tables[0].Columns.Add(t.Name);
                if (list != null)
                {
                    foreach (var i in list)
                    {
                        DataRow addRow = ds.Tables[0].NewRow();
                        addRow[t.Name] = i;
                        ds.Tables[0].Rows.Add(addRow);
                    }
                }
                return ds;
            }
            //處理模型的字段和屬性。
            var fields = t.GetFields();
            var properties = t.GetProperties();
            foreach (var j in fields)
            {
                if (!ds.Tables[0].Columns.Contains(j.Name))
                {
                    if (generic)
                    {
                        ds.Tables[0].Columns.Add(j.Name, j.FieldType);
                    }
                    else
                    {
                        ds.Tables[0].Columns.Add(j.Name);
                    }
                }
            }
            foreach (var j in properties)
            {
                if (!ds.Tables[0].Columns.Contains(j.Name))
                {
                    if (generic)
                    {
                        ds.Tables[0].Columns.Add(j.Name, j.PropertyType);
                    }
                    else
                    {
                        ds.Tables[0].Columns.Add(j.Name);
                    }
                }
            }
            if (list == null)
            {
                return ds;
            }
            //讀取list中元素的值。
            foreach (var i in list)
            {
                if (i == null)
                {
                    continue;
                }
                DataRow addRow = ds.Tables[0].NewRow();
                foreach (var j in fields)
                {
                    MemberExpression field = Expression.Field(Expression.Constant(i), j.Name);
                    LambdaExpression lambda = Expression.Lambda(field, new ParameterExpression[] { });
                    Delegate func = lambda.Compile();
                    object value = func.DynamicInvoke();
                    addRow[j.Name] = value;
                }
                foreach (var j in properties)
                {
                    MemberExpression property = Expression.Property(Expression.Constant(i), j);
                    LambdaExpression lambda = Expression.Lambda(property, new ParameterExpression[] { });
                    Delegate func = lambda.Compile();
                    object value = func.DynamicInvoke();
                    addRow[j.Name] = value;
                }
                ds.Tables[0].Rows.Add(addRow);
            }
            return ds;
        }

        /// <summary>
        /// 將集合转換為數據集。
        /// </summary>
        /// <typeparam name="T">转換的元素類型。</typeparam>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否生成泛型數據集。</param>
        /// <returns>數據集。</returns>
        private static DataSet ListToDataSet<T>(IEnumerable<T> list, bool generic)
        {
            return ListToDataSet(list, typeof(T), generic);
        }

        /// <summary>
        /// 將集合转換為數據集。
        /// </summary>
        /// <param name="list">集合。</param>
        /// <param name="generic">是否转換為字符串形式。</param>
        /// <returns>转換后的數據集。</returns>
        private static DataSet ListToDataSet(IEnumerable list, bool generic)
        {
            return ListToDataSet(list, null, generic);
        }

        /// <summary>
        /// 获取DataSet第一表，第一行，第一列的值。
        /// </summary>
        /// <param name="ds">DataSet數據集。</param>
        /// <returns>值。</returns>
        public static object GetData(this DataSet ds)
        {
            if (
                ds == null
                || ds.Tables.Count == 0
                )
            {
                return string.Empty;
            }
            else
            {
                return ds.Tables[0].GetData();
            }
        }

        /// <summary>
        /// 获取DataTable第一行，第一列的值。
        /// </summary>
        /// <param name="dt">DataTable數據集表。</param>
        /// <returns>值。</returns>
        public static object GetData(this DataTable dt)
        {
            if (
                dt.Columns.Count == 0
                || dt.Rows.Count == 0
                )
            {
                return string.Empty;
            }
            else
            {
                return dt.Rows[0][0];
            }
        }

        /// <summary>
        /// 获取DataSet第一個匹配columnName的值。
        /// </summary>
        /// <param name="ds">數據集。</param>
        /// <param name="columnName">列名。</param>
        /// <returns>值。</returns>
        public static object GetData(this DataSet ds, string columnName)
        {
            if (
                ds == null
                || ds.Tables.Count == 0
                )
            {
                return string.Empty;
            }
            foreach (DataTable dt in ds.Tables)
            {
                object o = dt.GetData(columnName);
                if (!string.IsNullOrEmpty(o.ToString()))
                {
                    return o;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取DataTable第一個匹配columnName的值。
        /// </summary>
        /// <param name="dt">數據表。</param>
        /// <param name="columnName">列名。</param>
        /// <returns>值。</returns>
        public static object GetData(this DataTable dt, string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return GetData(dt);
            }
            if (
                dt.Columns.Count == 0
                || dt.Columns.IndexOf(columnName) == -1
                || dt.Rows.Count == 0
                )
            {
                return string.Empty;
            }
            return dt.Rows[0][columnName];
        }

        /// <summary>
        /// 將object转換為string類型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <param name="t">默認值。</param>
        /// <returns>string。</returns>
        //public static string ToString(this object o, string t)
        //{
        //    string info = string.Empty;
        //    if (o == null)
        //    {
        //        info = t;
        //    }
        //    else
        //    {
        //        info = o.ToString(t);
        //    }
        //    return info;
        //}

        /// <summary>
        /// 將DateTime?转換為string類型信息。
        /// </summary>
        /// <param name="o">DateTime?。</param>
        /// <param name="format">標准或自定義日期和時间格式的字符串。</param>
        /// <param name="t">默認值。</param>
        /// <returns>string。</returns>
        public static string ToString(this DateTime? date, string format)
        {
            return date.Value.ToString(format);
        }











        private static Regex BoolRegex = new Regex("(?<info>(true|false))", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 从object中获取bool類型信息。
        /// </summary>
        /// <param name="o">object。</param>
        /// <returns>bool。</returns>
        public static bool GetBool(this string value)
        {
            bool.TryParse(value, out bool result);
            return result;
        }

        private static Regex IntRegex = new Regex("(?<info>-?\\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);



        private static Regex DecimalRegex = new Regex("(?<info>-?\\d+(\\.\\d+)?)", RegexOptions.IgnoreCase | RegexOptions.Singleline);



        /// <summary>
        /// 讀取XElement节點的文本内容。
        /// </summary>
        /// <param name="xElement">XElement节點。</param>
        /// <param name="t">默認值。</param>
        /// <returns>文本内容。</returns>
        public static string Value(this XElement xElement, string t = default(string))
        {
            if (xElement == null)
            {
                return t;
            }
            else
            {
                return xElement.Value;
            }
        }

        /// <summary>
        /// 获取與指定鍵相关的值。
        /// </summary>
        /// <typeparam name="TKey">鍵類型。</typeparam>
        /// <typeparam name="TValue">值類型。</typeparam>
        /// <param name="dictionary">表示鍵/值對象的泛型集合。</param>
        /// <param name="key">鍵。</param>
        /// <param name="t">默認值。</param>
        /// <returns>值。</returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue t = default(TValue))
        {
            TValue value = default(TValue);
            if (dictionary == null || key == null)
            {
                return t;
            }
            if (!dictionary.TryGetValue(key, out value))
            {
                value = t;
            }
            return value;
        }

        /// <summary>
        /// 获取與指定鍵相关或者第一個的值。
        /// </summary>
        /// <typeparam name="TKey">鍵類型。</typeparam>
        /// <typeparam name="TValue">值類型。</typeparam>
        /// <param name="dictionary">表示鍵/值對象的泛型集合。</param>
        /// <param name="key">鍵。</param>
        /// <param name="t">默認值。</param>
        /// <returns>值。</returns>
        public static TValue GetFirstOrDefaultValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue t = default(TValue))
        {
            TValue value = default(TValue);
            if (dictionary == null || key == null)
            {
                return t;
            }
            if (!dictionary.TryGetValue(key, out value))
            {
                if (dictionary.Count() == 0)
                {
                    value = t;
                }
                else
                {
                    value = dictionary.FirstOrDefault().Value;
                }
            }
            return value;
        }

        /// <summary>
        /// 获取具有指定 System.Xml.Linq.XName 的第一個（按文档顺序）子元素。
        /// </summary>
        /// <param name="xContainer">XContainer。</param>
        /// <param name="xName">要匹配的 System.Xml.Linq.XName。</param>
        /// <param name="t">是否返回同名默認值。</param>
        /// <returns>與指定 System.Xml.Linq.XName 匹配的 System.Xml.Linq.XElement，或者為 null。</returns>
        public static XElement Element(this XContainer xContainer, XName xName, bool t)
        {
            XElement info;
            if (xContainer == null)
            {
                info = null;
            }
            else
            {
                info = xContainer.Element(xName);
            }
            if (t && info == null)
            {
                info = new XElement(xName);
            }
            return info;
        }

        /// <summary>
        /// 按文档顺序返回此元素或文档的子元素集合。
        /// </summary>
        /// <param name="xContainer">XContainer。</param>
        /// <param name="t">是否返回非空默認值。</param>
        /// <returns>System.Xml.Linq.XElement 的按文档顺序包含此System.Xml.Linq.XContainer 的子元素，或者非空默認值。</returns>
        public static IEnumerable<XElement> Elements(this XContainer xContainer, bool t)
        {
            IEnumerable<XElement> info;
            if (xContainer == null)
            {
                info = null;
            }
            else
            {
                info = xContainer.Elements();
            }
            if (t && info == null)
            {
                info = new List<XElement>();
            }
            return info;
        }

        /// <summary>
        /// 按文档顺序返回此元素或文档的經過筛选的子元素集合。集合中只包括具有匹配 System.Xml.Linq.XName 的元素。
        /// </summary>
        /// <param name="xContainer">XContainer。</param>
        /// <param name="xName">要匹配的 System.Xml.Linq.XName。</param>
        /// <param name="t">是否返回非空默認值。</param>
        /// <returns>System.Xml.Linq.XElement 的按文档顺序包含具有匹配System.Xml.Linq.XName 的 System.Xml.Linq.XContainer 的子级，或者非空默認值。</returns>
        public static IEnumerable<XElement> Elements(this XContainer xContainer, XName xName, bool t)
        {
            IEnumerable<XElement> info;
            if (xContainer == null)
            {
                info = null;
            }
            else
            {
                info = xContainer.Elements(xName);
            }
            if (t && info == null)
            {
                info = new List<XElement>();
            }
            return info;
        }

        public static Guid? GetGuid(this object value)
        {
            if (Guid.TryParse(value?.ToString(), out Guid guid))
            {
                return guid;
            }
            return null;
        }


        public static string ToString(this int? value, string format)
        {
            return value?.ToString(format);
        }

        /// <summary>
        /// 获取默認非空字符串。
        /// </summary>
        /// <param name="s">首选默認非空字符串。</param>
        /// <param name="args">依次非空字符串可选项。</param>
        /// <returns>默認非空字符串。若无可选项则返回string.Empty。</returns>
        public static string DefaultStringIfEmpty(this string s, params string[] args)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            foreach (string i in args)
            {
                if (!string.IsNullOrEmpty(i) && !string.IsNullOrEmpty(i.Trim()))
                {
                    return i;
                }
            }

            return (s ?? string.Empty);
        }

        /// <summary>
        /// 對 URL 字符串進行编碼。
        /// </summary>
        /// <param name="s">要编碼的文本。</param>
        /// <param name="regex">匹配要编碼的文本。</param>
        /// <param name="encoding">指定编碼方案的 System.Text.Encoding 對象。</param>
        /// <returns>一個已编碼的字符串。</returns>
        public static string ToUrlEncodeString(this string s, Regex regex = default(Regex), Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            if (regex == null)
            {
                return HttpUtility.UrlEncode(s, encoding);
            }
            List<string> l = new List<string>();
            foreach (char i in s)
            {
                string t = i.ToString();
                l.Add(regex.IsMatch(t) ? HttpUtility.UrlEncode(t, encoding) : t);
            }
            return string.Join(string.Empty, l);
        }

        /// <summary>
        /// 對 URL 字符串進行编碼。
        /// </summary>
        /// <param name="s">要编碼的文本。</param>
        /// <param name="regex">匹配要编碼的文本。</param>
        /// <param name="encoding">指定编碼方案的 System.Text.Encoding 對象。</param>
        /// <returns>一個已编碼的字符串。</returns>
        public static string ToUrlEncodeString(this string s, string regex, Encoding encoding = null)
        {
            return ToUrlEncodeString(s, new Regex(regex), encoding);
        }

        /// <summary>
        /// 將日期转換為UNIX時间戳字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToUnixTimeStamp(this DateTime date)
        {
            DateTime startTime = TimeZoneInfo.ConvertTimeToUtc(new DateTime(1970, 1, 1));
            string timeStamp = date.Subtract(startTime).Ticks.ToString();
            return timeStamp.Substring(0, timeStamp.Length - 7);
        }

        private static readonly Regex MobileRegex = new Regex("^1[3-9][0-9]\\d{4,8}$");
        private static readonly Regex EmailRegex = new Regex("^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,3}){1,2})$");

        /// <summary>
        /// 判断當前字符串是否是移動电话號碼
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobile(this string mobile)
        {
            return MobileRegex.IsMatch(mobile);
        }

        /// <summary>
        /// 判断當前字符串是否為郵箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(this string email)
        {
            return EmailRegex.IsMatch(email);
        }

    }



    /// <summary>
    /// 標記。
    /// </summary>
    public enum Flag
    {
        /// <summary>
        /// 默認。
        /// </summary>
        Default,

        /// <summary>
        /// 真。
        /// </summary>
        True,

        /// <summary>
        /// 假。
        /// </summary>
        False
    }
}
