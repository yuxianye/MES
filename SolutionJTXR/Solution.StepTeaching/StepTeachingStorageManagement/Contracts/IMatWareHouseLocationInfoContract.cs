using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.MatWarehouseStorageManagement.Contracts
{
    /// <summary>
    /// 库位信息契约接口
    /// </summary>
    public interface IMatWareHouseLocationInfoContract : IScopeDependency
    {
        #region 库位信息业务

        /// <summary>
        /// 获取库位信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MatWareHouseLocationInfo> MatWareHouseLocationInfos { get; }

        /// <summary>
        /// 检查组库位信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的库位信息编号</param>
        /// <returns>库位信息是否存在</returns>
        bool CheckMatWareHouseLocationExists(Expression<Func<MatWareHouseLocationInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加库位信息
        /// </summary>
        /// <param name="inputDtos">要添加的库位信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MatWareHouseLocationInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新库位信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的库位信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateMatWareHouseLocations(params MatWareHouseLocationInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除库位信息
        /// </summary>
        /// <param name="ids">要删除的库位信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteMatWareHouseLocations(params Guid[] ids);
        
        #endregion
    }
}
