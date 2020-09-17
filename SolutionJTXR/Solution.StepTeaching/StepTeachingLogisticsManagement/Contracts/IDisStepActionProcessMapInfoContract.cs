using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.StepTeachingDispatchManagement.Dtos;
using Solution.StepTeachingDispatchManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.StepTeachingDispatchManagement.Contracts
{
    /// <summary>
    /// 分步操作与工序关联信息契约接口
    /// </summary>
    public interface IDisStepActionProcessMapInfoContract : IScopeDependency
    {
        #region 分步操作与工序关联信息业务
        /// <summary>
        /// 获取分步操作与工序关联信息查询数据集
        /// </summary>
        IQueryable<DisStepActionProcessMapInfo> DisStepActionProcessMapInfos { get; }

        /// <summary>
        /// 检查分步操作与工序关联信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的分步操作与工序关联信息编号</param>
        /// <returns>分步操作与工序关联信息是否存在</returns>
        bool CheckDisStepActionProcessMapExists(Expression<Func<DisStepActionProcessMapInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加分步操作与工序关联信息
        /// </summary>
        /// <param name="inputDtos">要添加的分步操作与工序关联DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddDisStepActionProcessMaps(params DisStepActionProcessMapInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新分步操作与工序关联信息
        /// </summary>
        /// <param name="inputDtos">包含更新的业务点表map信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditDisStepActionProcessMaps(params DisStepActionProcessMapInfoInputDto[] inputDtos);

        /// <summary>
        /// 删除分步操作与工序关联信息
        /// </summary>
        /// <param name="ids">要删除的分步操作与工序关联信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteDisStepActionProcessMaps(params Guid[] ids);

        /// <summary>
        /// 配置分步操作与工序关联信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Setting(params DisStepActionProcessMapManageInputDto[] inputDtos);

        #endregion

    }
}
