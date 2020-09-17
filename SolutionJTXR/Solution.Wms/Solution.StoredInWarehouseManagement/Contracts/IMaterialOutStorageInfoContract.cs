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
    /// 物料出库信息契约接口
    /// </summary>
    public interface IMaterialOutStorageInfoContract : IScopeDependency
    {
        #region 物料出库信息业务

        /// <summary>
        /// 获取物料出库信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MaterialOutStorageInfo> MaterialOutStorageInfos { get; }

        /// <summary>
        /// 检查组物料出库信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的物料出库信息编号</param>
        /// <returns>物料出库信息是否存在</returns>
        bool CheckExists(Expression<Func<MaterialOutStorageInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加物料出库信息
        /// </summary>
        /// <param name="inputDtos">要添加的物料出库信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MaterialOutStorageInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新物料出库信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的物料出库信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params MaterialOutStorageInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除物料出库信息
        /// </summary>
        /// <param name="ids">要删除的物料出库信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        
        #endregion
    }
}
