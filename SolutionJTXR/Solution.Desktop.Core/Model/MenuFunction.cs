namespace Solution.Desktop.Core
{
    /// <summary>
    /// 菜单功能，对应后端的Function，最小功能信息
    /// </summary>
    public class MenuFunction
    {
        /// <summary>
        /// 获取或设置 功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 区域名称
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 获取或设置 控制器名称
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 获取或设置 功能名称
        /// </summary>
        public string Action { get; set; }
    }
}
