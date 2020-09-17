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
    /// 供应商信息契约接口
    /// </summary>
    public interface IMatSupplierInfoContract : IScopeDependency
    {
        #region 供应商信息业务

        /// <summary>
        /// 获取供应商信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MatSupplierInfo> MatSupplierInfos { get; }

        /// <summary>
        /// 检查组供应商信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的供应商信息编号</param>
        /// <returns>供应商信息是否存在</returns>
        bool CheckExists(Expression<Func<MatSupplierInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加供应商信息1
        /// </summary>
        /// <param name="inputDtos">要添加的供应商信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MatSupplierInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新供应商信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的供应商信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params MatSupplierInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除供应商信息
        /// </summary>
        /// <param name="ids">要删除的供应商信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
         
        #endregion
    }
}
