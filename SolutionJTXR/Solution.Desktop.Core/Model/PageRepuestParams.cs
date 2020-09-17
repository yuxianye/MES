using Solution.Desktop.Core.Model;

namespace Solution.Desktop.Core
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageRepuestParams
    {
        public FilterGroup FilterGroup { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 200;

        /// <summary>
        /// 查询列表(,分割组成字符串)
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 排序参数(,分割组成字符串)
        /// </summary>
        public string SortOrder { get; set; }
    }
}
