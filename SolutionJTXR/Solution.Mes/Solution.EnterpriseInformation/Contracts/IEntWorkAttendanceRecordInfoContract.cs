using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.Contracts
{
    public interface IEntWorkAttendanceRecordInfoContract : IScopeDependency
    {
        #region 考勤信息业务
        /// <summary>
        /// 获取人员信息查询数据集
        /// </summary>
        IQueryable<EntWorkAttendanceRecordInfo> EntWorkAttendanceRecordInfos { get; }

        /// <summary>
        /// 检查组车间信息信息是否存在
        /// </summary>
        bool CheckEntEmployeeInfoExists(Expression<Func<EntWorkAttendanceRecordInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 增加考勤信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params EntWorkAttendanceRecordInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除考勤信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        /// <summary>
        /// 更新考勤信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params EntWorkAttendanceRecordInfoInputDto[] inputDtos);

        #endregion
    }
}
