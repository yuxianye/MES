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
    public interface IEntAreaInfoContract : IScopeDependency
    {
        #region 车间信息业务
        /// <summary>
        /// 获取车间信息查询数据集
        /// </summary>
        IQueryable<EntAreaInfo> EntAreaInfo { get; }

        /// <summary>
        /// 检查组车间信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的车间信息编号</param>
        /// <returns>车间信息是否存在</returns>
        bool CheckEntAreaInfoExists(Expression<Func<EntAreaInfo, bool>> predicate, Guid id );

        /// <summary>
        /// 增加车间信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Add(params EntAreaInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除车间信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<OperationResult> Delete(params Guid[] ids);

        ///// <summary>
        ///// 逻辑删除车间信息
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
        /// 更新车间信息
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> Update(params EntAreaInfoInputDto[] inputDtos);

        #endregion
    }
}
