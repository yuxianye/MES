using System.Windows;

namespace Solution.Desktop.Core
{
    /// <summary>
    /// 页变更事件参数
    /// </summary>
    public class PageChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 新页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
    }
}
