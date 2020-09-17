namespace Solution.Desktop.Core
{
    /// <summary>
    /// Solution结果基类
    /// </summary>
    /// <typeparam name="TResultType">结果类型</typeparam>
    /// <typeparam name="TData">结果数据类型</typeparam>
    public abstract class SolutionResult<TResultType, TData> : ISolutionResult<TResultType, TData>
    {
        protected string _message;

        /// <summary>
        /// 初始化一个<see cref="SolutionResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected SolutionResult()
            : this(default(TResultType))
        { }

        /// <summary>
        /// 初始化一个<see cref="SolutionResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected SolutionResult(TResultType type)
            : this(type, null, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="SolutionResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected SolutionResult(TResultType type, string message)
            : this(type, message, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="SolutionResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected SolutionResult(TResultType type, string message, TData data)
        {
            ResultType = type;
            _message = message;
            Data = data;
        }

        /// <summary>
        /// 获取或设置 结果类型
        /// </summary>
        public TResultType ResultType { get; set; }

        /// <summary>
        /// 获取或设置 返回消息
        /// </summary>
        public virtual string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 获取或设置 结果数据
        /// </summary>
        public TData Data { get; set; }
    }
}
