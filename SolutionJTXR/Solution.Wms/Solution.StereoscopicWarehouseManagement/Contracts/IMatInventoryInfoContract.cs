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
    /// 盘点信息契约接口
    /// </summary>
    public interface IMatInventoryInfoContract : IScopeDependency
    {
        #region 盘点信息业务

        /// <summary>
        /// 获取盘点信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<MatInventoryInfo> MatInventoryInfos { get; }

        /// <summary>
        /// 检查组盘点信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的盘点信息编号</param>
        /// <returns>盘点信息是否存在</returns>
        bool CheckExists(Expression<Func<MatInventoryInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加盘点信息
        /// </summary>
        /// <param name="inputDtos">要添加的盘点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params MatInventoryInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新盘点信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的盘点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params MatInventoryInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除盘点信息
        /// </summary>
        /// <param name="ids">要删除的盘点信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);

        #endregion
    }
}
