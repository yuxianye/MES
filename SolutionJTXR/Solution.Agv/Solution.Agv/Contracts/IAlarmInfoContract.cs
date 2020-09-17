using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.Agv.Dtos;
using Solution.Agv.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.Agv.Contracts
{
    /// <summary>
    /// 报警契约接口
    /// </summary>
    public interface IAlarmInfoContract : IScopeDependency
    {
        #region 报警信息业务
        /// <summary>
        /// 获取报警信息查询数据集
        /// </summary>
        IQueryable<AlarmInfo> AlarmInfos { get; }

        /// <summary>
        /// 检查组报警信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的报警信息编号</param>
        /// <returns>报警信息是否存在</returns>
        bool CheckAlarmInfoExists(Expression<Func<AlarmInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 增加报警信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>    AlarmInfoIntputDto
        Task<OperationResult> Add(params AlarmInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新报警信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params AlarmInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除报警信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        #endregion
    }
}
