namespace Solution.Desktop.Core
{
    /// <summary>
    /// 消息令牌
    /// </summary>
    public enum MessengerToken
    {
        /// <summary>
        /// 登陆成功
        /// </summary>
        LoginSuccess,

        /// <summary>
        /// 登陆退出
        /// </summary>
        LoginExit,

        /// <summary>
        /// 页面导航跳转,包括DockerabledockerAbleView ,popup等都是用此导航
        /// </summary>
        Navigate,

        /// <summary>
        /// 关闭对话框
        /// </summary>
        ClosePopup,

        /// <summary>
        /// 数据已经变化
        /// </summary>
        DataChanged,
    }
}
