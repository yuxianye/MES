namespace Solution.Desktop.Core
{
    /// <summary>
    ///  数据分页信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T>
    {
        /// <summary>
        /// 获取或设置 分页数据
        /// </summary>
        public T[] Data { get; set; }

        /// <summary>
        /// 获取或设置 总记录数
        /// </summary>
        public int Total { get; set; }
    }
}
