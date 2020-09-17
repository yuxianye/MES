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
    public interface IProductInfoContract : IScopeDependency
    {
        #region 产品信息业务
        /// <summary>
        /// 获取产品信息查询数据集
        /// </summary>
        IQueryable<ProductInfo> ProductInfos { get; }

        /// <summary>
        /// 检查组产品信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的产品信息编号</param>
        /// <returns>产品信息是否存在</returns>
        bool CheckExists(Expression<Func<ProductInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加产品信息
        /// </summary>
        /// <param name="inputDtos">要添加的产品信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params ProductInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新产品信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的产品信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params ProductInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除产品信息
        /// </summary>
        /// <param name="ids">要删除的产品信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        ///// <summary>
        ///// 逻辑删除产品信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params ProductInfo[] enterinfo);
        ///// <summary>
        ///// 逻辑还原产品信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params ProductInfo[] enterinfo);


        #endregion
    }
}
