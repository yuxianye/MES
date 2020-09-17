using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.PlanManagement.Dtos;
using Solution.PlanManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.PlanManagement.Contracts
{
   public interface IPlanOrderInfoContract : IScopeDependency
    {
        #region 订单信息业务
        /// <summary>
        /// 获取订单查询数据集
        /// </summary>
        IQueryable<PlanOrderInfo> PlanOrderInfos { get; }

        /// <summary>
        /// 检查组订单信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的订单编号</param>
        /// <returns>订单是否存在</returns>
        bool CheckExists(Expression<Func<PlanOrderInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="inputDtos">要添加的订单DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params PlanOrderInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新订单信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的订单DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params PlanOrderInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除订单
        /// </summary>
        /// <param name="ids">要删除的订单Id</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        ///// <summary>
        ///// 逻辑删除订单
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params PlanOrderInfo[] enterinfo);
        ///// <summary>
        ///// 逻辑还原订单
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params PlanOrderInfo[] enterinfo);


        #endregion
    }
}
