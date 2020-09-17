using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Utility.Extensions
{
    public static class DataTableExtensions
    {
        public static List<T> DataTableToList<T>(this DataTable dt) where T : new()
        {
            // 定义集合    
            List<T> ts = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 将List转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data, List<string> displayNameList)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();

            for (int i = 0; i < displayNameList.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                Type columnType = property.PropertyType;

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = property.PropertyType.GetGenericArguments()[0];
                }
                if (columnType == typeof(System.Guid))
                {
                    columnType = typeof(System.String);
                }
                dt.Columns.Add(displayNameList[i], columnType);
            }
            object[] values = new object[displayNameList.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}
