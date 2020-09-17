namespace Solution.Desktop.Core
{
    /// <summary>
    /// Solution操作结果接口
    /// </summary>
    public interface ISolutionResult<TResultType, TData>
    {
        /// <summary>
        /// 获取或设置 结果类型
        /// </summary>
        TResultType ResultType { get; set; }

        /// <summary>
        /// 获取或设置 返回消息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// 获取或设置 结果数据
        /// </summary>
        TData Data { get; set; }

    }
}
