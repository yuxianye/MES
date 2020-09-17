using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.Agv.Dtos;
using Solution.Agv.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Solution.Agv.Contracts
{
    public interface ITaskDetailedInfoContract : IScopeDependency
    {
        #region 任务信息业务
        /// <summary>
        /// 获取任务信息查询数据集
        /// </summary>
        IQueryable<TaskDetailedInfo> TaskDetailedInfos { get; }

        /// <summary>
        /// 检查任务信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的任务信息编号</param>
        /// <returns>任务信息是否存在</returns>
        bool CheckTaskDetailedInfoExists(Expression<Func<TaskDetailedInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 增加任务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params TaskDetailedInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新任务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params TaskDetailedInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除任务信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        #endregion
    }
}
