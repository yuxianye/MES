using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.PlanManagement.Dtos;
using Solution.PlanManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.PlanManagement.Contracts
{
    public interface IPlanProductionScheduleInfoContract : IScopeDependency
    {
        #region 生产计划信息业务
        /// <summary>
        /// 获取生产计划查询数据集
        /// </summary>
        IQueryable<PlanProductionScheduleInfo> PlanProductionScheduleInfos { get; }

        /// <summary>
        /// 检查组生产计划信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的生产计划编号</param>
        /// <returns>生产计划是否存在</returns>
        bool CheckExists(Expression<Func<PlanProductionScheduleInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加生产计划
        /// </summary>
        /// <param name="inputDtos">要添加的生产计划DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params PlanProductionScheduleInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新生产计划信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的生产计划DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params PlanProductionScheduleInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除生产计划
        /// </summary>
        /// <param name="ids">要删除的生产计划Id</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);

        /// <summary>
        /// 增加工单
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> AddWorkList(params PlanProductionScheduleInfoInputDto[] inputDtos);
        /// <summary>
        /// 工单下达
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> DistributeWorkOrder(params PlanProductionScheduleInfoInputDto[] inputDtos);
        #endregion
    }
}
