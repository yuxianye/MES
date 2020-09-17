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
   public interface IEntProductionLineInfoContract : IScopeDependency
    {
        #region 生产线信息业务
        /// <summary>
        /// 获取生产线信息查询数据集
        /// </summary>
        IQueryable<EntProductionLineInfo> EntProductionLineInfo { get; }

        /// <summary>
        /// 检查组生产线信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的生产线信息编号</param>
        /// <returns>生产线信息是否存在</returns>
        bool CheckEntProductionLineInfoExists(Expression<Func<EntProductionLineInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 增加生产线信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params EntProductionLineInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除生产线信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        ///// <summary>
        ///// 逻辑删除生产线信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params EntProductionLineInfo[] enterinfos);

        ///// <summary>
        ///// 逻辑还原生产线信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params EntProductionLineInfo[] enterinfos);

        /// <summary>
        /// 更新生产线信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params EntProductionLineInfoInputDto[] inputDtos);

        #endregion
    }
}
