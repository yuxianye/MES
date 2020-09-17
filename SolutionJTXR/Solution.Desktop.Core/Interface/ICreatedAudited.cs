namespace Solution.Desktop.Core
{
    /// <summary>
    /// 给信息添加 创建时间、创建者 属性，在实体创建时，将自动提取当前用户为创建者
    /// </summary>
    public interface ICreatedAudited : ICreatedTime
    {
        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        string CreatorUserId { get; set; }
    }

}
