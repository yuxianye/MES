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
    public interface IProductTypeInfoContract : IScopeDependency
    {

        #region 产品类别信息业务
        /// <summary>
        /// 获取产品类别信息查询数据集
        /// </summary>
        IQueryable<ProductTypeInfo> ProductTypeInfos { get; }

        /// <summary>
        /// 检查组产品类别信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的产品类别信息编号</param>
        /// <returns>产品类别信息是否存在</returns>
        bool CheckExists(Expression<Func<ProductTypeInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加产品类别信息
        /// </summary>
        /// <param name="inputDtos">要添加的产品类别信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params ProductTypeInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新产品类别信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的产品类别信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params ProductTypeInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除产品类别信息
        /// </summary>
        /// <param name="ids">要删除的产品类别信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        ///// <summary>
        ///// 逻辑删除产品类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params ProductTypeInfo[] enterinfo);
        ///// <summary>
        ///// 逻辑还原产品类别信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params ProductTypeInfo[] enterinfo);


        #endregion
    }
}
