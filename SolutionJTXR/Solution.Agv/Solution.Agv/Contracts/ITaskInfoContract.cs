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
    public interface ITaskInfoContract : IScopeDependency
    {
        #region 任务信息业务
        /// <summary>
        /// 获取任务信息查询数据集
        /// </summary>
        IQueryable<TaskInfo> TaskInfos { get; }

        /// <summary>
        /// 检查任务信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的任务信息编号</param>
        /// <returns>任务信息是否存在</returns>
        bool CheckTaskInfoExists(Expression<Func<TaskInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 增加任务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params TaskInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新任务信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params TaskInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除任务信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        #endregion
    }
}
