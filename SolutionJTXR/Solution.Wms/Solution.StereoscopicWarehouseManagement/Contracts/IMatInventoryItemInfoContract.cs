using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.StereoscopicWarehouseManagement.Dtos;
using Solution.StereoscopicWarehouseManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.StereoscopicWarehouseManagement.Contracts
{
    /// <summary>
    /// 盘点明细信息契约接口
    /// </summary>
    public interface IMatInventoryItemInfoContract : IScopeDependency
    {
        #region 盘点明细信息业务

        /// <summary>
        /// 获取盘点明细信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MatInventoryItemInfo> MatInventoryItemInfos { get; }

        /// <summary>
        /// 检查组盘点明细信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的盘点明细信息编号</param>
        /// <returns>盘点明细信息是否存在</returns>
        bool CheckExists(Expression<Func<MatInventoryItemInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加盘点明细信息
        /// </summary>
        /// <param name="inputDtos">要添加的盘点明细信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MatInventoryItemInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新盘点明细信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的盘点明细信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params MatInventoryItemInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除盘点明细信息
        /// </summary>
        /// <param name="ids">要删除的盘点明细信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);

        /// <summary>
        ///作业任务
        /// </summary>
        /// <param name="dtos">包含更新信息的物料作业任务DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddTask(params MatInventoryItemInfoInputDto[] dtos);

        #endregion
    }
}
