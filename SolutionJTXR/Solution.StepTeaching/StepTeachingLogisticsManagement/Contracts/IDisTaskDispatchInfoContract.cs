using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Dtos;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StepTeachingDispatchManagement.Dtos;
using Solution.StepTeachingDispatchManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.StepTeachingDispatchManagement.Contracts
{
    /// <summary>
    /// 分步教学任务调度主信息契约接口
    /// </summary>
    public interface IDisTaskDispatchInfoContract : IScopeDependency
    {
        #region 分步教学任务调度主信息业务

        /// <summary>
        /// 获取分步教学任务调度主信息查询数据集《注意拼写单复数》
        /// </summary>
        IQueryable<DisTaskDispatchInfo> DisTaskDispatchInfos { get; }

        /// <summary>
        /// 检查组分步教学任务调度主信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的分步教学任务调度主信息编号</param>
        /// <returns>分步教学任务调度主信息是否存在</returns>
        bool CheckExists(Expression<Func<DisTaskDispatchInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加分步教学任务调度主信息1
        /// </summary>
        /// <param name="inputDtos">要添加的分步教学任务调度主信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params DisTaskDispatchInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新分步教学任务调度主信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的分步教学任务调度主信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params DisTaskDispatchInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除分步教学任务调度主信息
        /// </summary>
        /// <param name="ids">要删除的分步教学任务调度主信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
         
        #endregion
    }
}
