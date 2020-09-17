using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Solution.Utility
{
    public static class NumberHelper
    {
        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }
    }
}
