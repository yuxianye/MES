using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Dtos;
using Solution.EnterpriseInformation.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.Contracts
{
    /// <summary>
    /// 班组信息契约接口
    /// </summary>
    public interface IEntTeamInfoContract : IScopeDependency
    {
        #region 班组信息业务

        /// <summary>
        /// 获取班组信息查询数据集 《注意拼写单复数。》
        /// </summary>
        IQueryable<EntTeamInfo> EntTeamInfos { get; }

        /// <summary>
        /// 检查班组信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的班组信息编号</param>
        /// <returns>班组信息是否存在</returns>
        bool CheckEnterpriseExists(Expression<Func<EntTeamInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加班组信息
        /// </summary>
        /// <param name="inputDtos">要添加的班组信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params EntTeamInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新班组信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的班组信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params EntTeamInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除班组信息
        /// </summary>
        /// <param name="ids">要删除的班组信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);


        #endregion
    }
}
