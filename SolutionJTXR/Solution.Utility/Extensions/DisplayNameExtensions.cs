using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Utility.Extensions
{
    public static class DisplayNameExtensions
    {
        /// <summary>
        /// 反射获取DisplayName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<String> GetDisplayName<T>(this T t)
        {
            List<String> list = new List<string>();
            Type type = t.GetType();
            PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo info in ps)
            {
                if (info.SetMethod != null)
                {
                    var name = info.GetCustomAttribute<DisplayNameAttribute>();
                    //list.Add((name == null ? info.Name : name.DisplayName) + ":" + (info.GetValue(t, null) == null ? string.Empty : info.GetValue(t, null).ToString()));
                    list.Add(name.DisplayName);
                }
            }
            return list;
        }

        public static Dictionary<String, Type> GetFiledNameAndType<T>(this T t)
        {
            Dictionary<String, Type> FileNameDic = new Dictionary<String, Type>();
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return null; }
            foreach (PropertyInfo item in properties)
            {
                if (item.SetMethod != null)
                {
                    string name = item.Name;
                    Type columnType = item.PropertyType;
                    if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        columnType = item.PropertyType.GetGenericArguments()[0];
                    }
                    FileNameDic.Add(name, columnType);
                }
            }
            return FileNameDic;
        }
    }
}
