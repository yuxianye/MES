namespace Solution.Desktop.Core
{
    /// <summary>
    /// 页面类型
    /// </summary>
    public enum ViewType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 可停靠的页面
        /// </summary>
        DockableView = 1,

        /// <summary>
        /// 模式对话框弹出窗体
        /// </summary>
        Popup = 2,

        /// <summary>
        /// 没有标题栏的模式对话框弹出窗体
        /// </summary>
        PopupNoTitle = 3,

        /// <summary>
        /// 单独的窗体
        /// </summary>
        SingleWindow = 4,

    }
}
