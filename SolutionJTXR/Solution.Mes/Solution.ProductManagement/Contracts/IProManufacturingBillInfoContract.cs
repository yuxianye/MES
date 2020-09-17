using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Solution.ProductManagement.Contracts
{
    public interface IProManufacturingBillInfoContract : IScopeDependency
    {
        #region 制造清单信息业务
        /// <summary>
        /// 获取制造清单信息查询数据集
        /// </summary>
        IQueryable<ProManufacturingBillInfo> ProManufacturingBillInfos { get; }

        /// <summary>
        /// 检查组制造清单信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的制造清单信息编号</param>
        /// <returns>制造清单信息是否存在</returns>
        bool CheckExists(Expression<Func<ProManufacturingBillInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加制造清单信息
        /// </summary>
        /// <param name="inputDtos">要添加的制造清单信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params ProManufacturingBillInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新制造清单信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的制造清单信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params ProManufacturingBillInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除制造清单信息
        /// </summary>
        /// <param name="ids">要删除的制造清单信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        ///// <summary>
        ///// 逻辑删除制造清单信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params ProManufacturingBillInfo[] enterinfo);
        ///// <summary>
        ///// 逻辑还原制造清单信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params ProManufacturingBillInfo[] enterinfo);


        #endregion
    }
}
