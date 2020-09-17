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
    public interface IProductionRuleInfoContract : IScopeDependency
    {
        #region 配方信息业务
        /// <summary>
        /// 获取配方信息查询数据集
        /// </summary>
        IQueryable<ProductionRuleInfo> ProductionRuleInfos { get; }

        /// <summary>
        /// 检查组配方信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的配方信息编号</param>
        /// <returns>配方信息是否存在</returns>
        bool CheckExists(Expression<Func<ProductionRuleInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加配方信息
        /// </summary>
        /// <param name="inputDtos">要添加的配方信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params ProductionRuleInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新配方信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的配方DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params ProductionRuleInfoInputDto[] inputDtos);

        /// <summary>
        /// 审核配方信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Audit(params ProductionRuleInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除配方信息
        /// </summary>
        /// <param name="ids">要删除的配方信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        ///// <summary>
        ///// 逻辑删除配方信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params ProductionRuleInfo[] enterinfo);
        ///// <summary>
        ///// 逻辑还原配方信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params ProductionRuleInfo[] enterinfo);


        #endregion
    }
}
