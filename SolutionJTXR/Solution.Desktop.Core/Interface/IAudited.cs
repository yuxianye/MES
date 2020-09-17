namespace Solution.Desktop.Core
{
    /// <summary>
    /// 表示审计属性，包含<see cref="ICreatedAudited"/>与<see cref="IUpdateAudited"/>
    /// </summary>
    public interface IAudited : ICreatedAudited, IUpdateAudited
    {

    }
}
