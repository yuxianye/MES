using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.ProductManagement.Dtos;
using Solution.ProductManagement.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.ProductManagement.Contracts
{
    public interface IProductionProcessInfoContract : IScopeDependency
    {
        #region 工序信息业务
        /// <summary>
        /// 获取工序信息查询数据集
        /// </summary>
        IQueryable<ProductionProcessInfo> ProductionProcessInfos { get; }

        /// <summary>
        /// 检查组工序信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的工序信息编号</param>
        /// <returns>工序信息是否存在</returns>
        bool CheckExists(Expression<Func<ProductionProcessInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加工序信息
        /// </summary>
        /// <param name="inputDtos">要添加的工序信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params ProductionProcessInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新工序信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的工序信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params ProductionProcessInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除工序信息
        /// </summary>
        /// <param name="ids">要删除的工序信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        ///// <summary>
        ///// 逻辑删除工序信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params ProductionProcessInfo[] enterinfo);
        ///// <summary>
        ///// 逻辑还原工序信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params ProductionProcessInfo[] enterinfo);

        #endregion
    }
}
