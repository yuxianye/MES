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
    /// 企业信息契约接口
    /// </summary>
    public interface IEnterpriseInfoContract : IScopeDependency
    {
        #region 企业信息业务

        /// <summary>
        /// 获取企业信息查询数据集 《注意拼写单复数。》
        /// </summary>
        IQueryable<EnterpriseInfo> EnterpriseInfos { get; }

        /// <summary>
        /// 检查企业信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的企业信息编号</param>
        /// <returns>企业信息是否存在</returns>
        bool CheckEnterpriseExists(Expression<Func<EnterpriseInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加企业信息
        /// </summary>
        /// <param name="inputDtos">要添加的企业信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddEnterprises(params EnterpriseInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新企业信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的企业信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> UpdateEnterprises(params EnterpriseInfoInputDto[] inputDtos);

        ///// <summary>
        ///// 逻辑删除企业信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDeleteEnterprises(params EnterpriseInfo[] enterinfo);

        /// <summary>
        /// 物理删除企业信息
        /// </summary>
        /// <param name="ids">要删除的企业信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteEnterprises(params Guid[] ids);

        ///// <summary>
        ///// 逻辑还原企业信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestoreEnterprises(params EnterpriseInfo[] enterinfo);

        #endregion

    }
}
