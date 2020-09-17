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
    public interface IEntEmployeeInfoContract : IScopeDependency
    {
        #region 人员信息业务
        /// <summary>
        /// 获取人员信息查询数据集
        /// </summary>
        IQueryable<EntEmployeeInfo> EntEmployeeInfo { get; }

        /// <summary>
        /// 检查组车间信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的车间信息编号</param>
        /// <returns>人员信息是否存在</returns>
        bool CheckEntEmployeeInfoExists(Expression<Func<EntEmployeeInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 增加人员信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params EntEmployeeInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除人员信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        ///// <summary>
        ///// 逻辑删除人员信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params EntAreaInfo[] enterinfos);

        ///// <summary>
        ///// 逻辑还原车间信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params EntAreaInfo[] enterinfos);

        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params EntEmployeeInfoInputDto[] inputDtos);

        #endregion
    }
}
