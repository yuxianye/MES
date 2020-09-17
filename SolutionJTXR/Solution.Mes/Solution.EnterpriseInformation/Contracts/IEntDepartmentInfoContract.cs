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
    public interface IEntDepartmentInfoContract : IScopeDependency
    {
        #region  部门信息业务

        /// <summary>
        /// 获取部门信息查询数据集 《注意拼写单复数。》
        /// </summary>
        IQueryable<EntDepartmentInfo> EntDepartmentInfos { get; }

        /// <summary>
        /// 检查部门信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的部门信息编号</param>
        /// <returns>部门信息是否存在</returns>
        bool CheckEntDepartmentInfosExists(Expression<Func<EntDepartmentInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="inputDtos">要添加的部门信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddEntDepartmentInfos(params EntDepartmentInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新部门信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的部门信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateEntDepartmentInfos(params EntDepartmentInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除部门信息
        /// </summary>
        /// <param name="ids">要删除的部门信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteEntDepartmentInfos(params Guid[] ids);

        #endregion
    }
}
