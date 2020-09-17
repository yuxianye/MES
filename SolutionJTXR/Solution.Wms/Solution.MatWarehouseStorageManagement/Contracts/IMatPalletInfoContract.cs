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
    /// 托盘信息契约接口
    /// </summary>
    public interface IMatPalletInfoContract : IScopeDependency
    {
        #region 托盘信息业务

        /// <summary>
        /// 获取托盘信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MatPalletInfo> MatPalletInfos { get; }

        /// <summary>
        /// 检查组托盘信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的托盘信息编号</param>
        /// <returns>托盘信息是否存在</returns>
        bool CheckMatPalletExists(Expression<Func<MatPalletInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加托盘信息
        /// </summary>
        /// <param name="inputDtos">要添加的托盘信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MatPalletInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新托盘信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的托盘信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateMatPallets(params MatPalletInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除托盘信息
        /// </summary>
        /// <param name="ids">要删除的托盘信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteMatPallets(params Guid[] ids);
        
        #endregion
    }
}
