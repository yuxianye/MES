using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Dtos;
using Solution.StoredInWarehouseManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.StoredInWarehouseManagement.Contracts
{
    /// <summary>
    /// 物料批次信息契约接口
    /// </summary>
    public interface IMaterialBatchInfoContract : IScopeDependency
    {
        #region 物料批次信息业务

        /// <summary>
        /// 获取物料批次信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MaterialBatchInfo> MaterialBatchInfos { get; }
        IQueryable<MaterialBatchInfo> MaterialTrackBatchInfos { get; }

        IQueryable<MaterialBatchInfoOutputDto> MaterialBatch2Infos { get; }

        /// <summary>
        /// 检查组物料批次信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的物料批次信息编号</param>
        /// <returns>物料批次信息是否存在</returns>
        bool CheckExists(Expression<Func<MaterialBatchInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加物料批次信息
        /// </summary>
        /// <param name="inputDtos">要添加的物料批次信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MaterialBatchInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新物料批次信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的物料批次信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params MaterialBatchInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除物料批次信息
        /// </summary>
        /// <param name="ids">要删除的物料批次信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);

        #endregion
    }
}
