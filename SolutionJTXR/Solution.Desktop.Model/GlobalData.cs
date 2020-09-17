using Solution.Desktop.Core;
using System.Collections.Generic;

namespace Solution.Desktop.Model
{
    /// <summary>
    /// 全局数据信息
    /// </summary>
    public class GlobalData
    {
        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        public static LoginUser CurrentLoginUser { get; set; }

        /// <summary>
        /// 当前用户可用的功能模块信息
        /// </summary>
        public static IEnumerable<MenuModule> CurrentUserModule { get; set; }

        /// <summary>
        /// 服务器根目录
        /// </summary>
        public static string ServerRootUri { get; set; } = Utility.ConfigHelper.GetAppSetting("ServerUri");

        /// <summary>
        /// 功能和界面的映射数据
        /// </summary>
        public static List<MenuFunctionViewInfoMap> MenuFunctionViewInfoMap { get; set; }

        /// <summary>
        /// Token字符串
        /// </summary>
        public static string AccessTocken { get; set; }

        public static string TokenUri { get; set; } = Utility.ConfigHelper.GetAppSetting("TokenUri");

        /// <summary>
        /// 查询时的排序字段
        /// </summary>
        public static readonly string SortField = "LastUpdatedTime,Id";

        /// <summary>
        /// 查询时的排序规则
        /// </summary>
        public static readonly string SortOrder = "desc,desc";
    }
}
