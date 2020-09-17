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
    public interface IPlanOrderItemInfoContract : IScopeDependency
    {
        #region 订单明细信息业务
        /// <summary>
        /// 获取订单明细查询数据集
        /// </summary>
        IQueryable<PlanOrderItemInfo> PlanOrderItemInfos { get; }

        /// <summary>
        /// 检查组订单明细信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的订单明细编号</param>
        /// <returns>订单明细是否存在</returns>
        bool CheckExists(Expression<Func<PlanOrderItemInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加订单明细
        /// </summary>
        /// <param name="inputDtos">要添加的订单明细DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params PlanOrderItemInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新订单明细信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的订单明细DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params PlanOrderItemInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除订单明细
        /// </summary>
        /// <param name="ids">要删除的订单明细Id</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        ///// <summary>
        ///// 逻辑删除订单明细
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params PlanOrderItemInfo[] enterinfo);
        ///// <summary>
        ///// 逻辑还原订单明细
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params PlanOrderItemInfo[] enterinfo);


        #endregion
    }
}
