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
    public interface IEntSiteInfoContract : IScopeDependency
    {
        #region 厂区信息业务
        /// <summary>
        /// 获取厂区信息查询数据集
        /// </summary>
        IQueryable<EntSiteInfo> EntSiteInfo { get; }

        /// <summary>
        /// 检查厂区信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的厂区信息编号</param>
        /// <returns>厂区信息是否存在</returns>
        bool CheckEntSiteInfoExists(Expression<Func<EntSiteInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 增加厂区信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params EntSiteInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除厂区信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        ///// <summary>
        ///// 逻辑删除厂区信息
        ///// </summary>
        ///// <param name="entsiteinfos"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params EntSiteInfo[] entsiteinfos);

        ///// <summary>
        ///// 逻辑还原厂区信息
        ///// </summary>
        ///// <param name="entsiteinfos"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params EntSiteInfo[] entsiteinfos);

        /// <summary>
        /// 更新厂区信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params EntSiteInfoInputDto[] inputDtos);

        #endregion
    }
}
