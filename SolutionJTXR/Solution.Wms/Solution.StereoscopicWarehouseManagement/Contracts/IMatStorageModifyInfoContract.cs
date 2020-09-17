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
    /// 库存调整信息契约接口
    /// </summary>
    public interface IMatStorageModifyInfoContract : IScopeDependency
    {
        #region 库存调整信息业务

        /// <summary>
        /// 获取库存调整信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MatStorageModifyInfo> MatStorageModifyInfos { get; }

        /// <summary>
        /// 检查组库存调整信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的库存调整信息编号</param>
        /// <returns>库存调整信息是否存在</returns>
        bool CheckExists(Expression<Func<MatStorageModifyInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加库存调整信息
        /// </summary>
        /// <param name="inputDtos">要添加的库存调整信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MatStorageModifyInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新库存调整信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的库存调整信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params MatStorageModifyInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除库存调整信息
        /// </summary>
        /// <param name="ids">要删除的库存调整信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);

        /// <summary>
        ///作业任务
        /// </summary>
        /// <param name="dtos">包含更新信息的物料作业任务DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddTask(params MatStorageModifyInfoInputDto[] dtos);

        #endregion
    }
}
