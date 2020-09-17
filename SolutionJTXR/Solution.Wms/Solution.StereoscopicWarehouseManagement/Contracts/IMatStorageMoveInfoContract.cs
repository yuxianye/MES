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
    /// 移库信息契约接口
    /// </summary>
    public interface IMatStorageMoveInfoContract : IScopeDependency
    {
        #region 移库信息业务

        /// <summary>
        /// 获取移库信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MatStorageMoveInfo> MatStorageMoveInfos { get; }
        IQueryable<MatStorageMoveInfo> MatStorageTrackMoveInfos { get; }

        /// <summary>
        /// 检查组移库信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的移库信息编号</param>
        /// <returns>移库信息是否存在</returns>
        bool CheckExists(Expression<Func<MatStorageMoveInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加移库信息
        /// </summary>
        /// <param name="inputDtos">要添加的移库信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MatStorageMoveInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新移库信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的移库信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params MatStorageMoveInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除移库信息
        /// </summary>
        /// <param name="ids">要删除的移库信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);

        /// <summary>
        ///作业任务
        /// </summary>
        /// <param name="dtos">包含更新信息的物料作业任务DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddTask(params MatStorageMoveInfoInputDto[] dtos);


        #endregion
    }
}
