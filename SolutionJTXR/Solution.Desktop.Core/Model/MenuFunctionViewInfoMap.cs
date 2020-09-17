namespace Solution.Desktop.Core
{
    /// <summary>
    /// 功能和界面的映射模型，对应到配置文件FunctionViewInfoMap.json
    /// </summary>
    public class MenuFunctionViewInfoMap
    {
        /// <summary>
        /// 服务器对应的Controller,例如：Roles
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 获取或设置 功能名称 Delete
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 页面信息
        /// </summary>
        public ViewInfo ViewInfo { get; set; }

    }
}
