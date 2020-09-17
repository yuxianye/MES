namespace Solution.Desktop.Core
{
    /// <summary>
    /// 回收站操作类型
    /// </summary>
    public enum RecycleOperation
    {
        /// <summary>
        /// 逻辑删除
        /// </summary>
        LogicDelete,

        /// <summary>
        /// 还原
        /// </summary>
        Restore,

        /// <summary>
        /// 物理删除
        /// </summary>
        PhysicalDelete
    }
}
