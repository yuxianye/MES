using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Solution.Utility.Extensions
{
    /// <summary>
    /// 类型<see cref="Type"/>辅助扩展方法类
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Point转Path Data
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToPathDataString(this List<Point> source)
        {
            StringBuilder sb = new StringBuilder();
            if (source.Any())
            {
                sb.Append("M ");
                foreach (var v in source)
                {
                    sb.Append(v.ToString());
                    sb.Append(" ");
                }
                //sb.Append(" Z");
            }

            return sb.ToString();
        }


    }
}
