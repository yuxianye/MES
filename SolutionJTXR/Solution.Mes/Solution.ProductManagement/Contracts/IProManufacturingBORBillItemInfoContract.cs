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
    public interface IProManufacturingBORBillItemInfoContract : IScopeDependency
    {
        #region BOR清单明细信息业务
        /// <summary>
        /// 获取BOR清单明细信息查询数据集
        /// </summary>
        IQueryable<ProManufacturingBORBillItemInfo> ProManufacturingBORBillItemInfos { get; }

        /// <summary>
        /// 检查组BOR清单明细信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的BOR清单明细信息编号</param>
        /// <returns>BOR清单明细信息是否存在</returns>
        bool CheckExists(Expression<Func<ProManufacturingBORBillItemInfo, bool>> predicate, Guid id);


        /// <summary>
        /// 添加BOR清单明细信息
        /// </summary>
        /// <param name="inputDtos">要添加的BOR清单明细信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params ProManufacturingBORBillItemInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新BOR清单明细信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的BOR清单明细信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params ProManufacturingBORBillItemInfoInputDto[] inputDtos);

        /// <summary>
        /// 物理删除BOR清单明细信息
        /// </summary>
        /// <param name="ids">要删除的BOR清单明细信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Delete(params Guid[] ids);
        ///// <summary>
        ///// 逻辑删除BOR清单明细信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicDelete(params ProManufacturingBORBillItemInfo[] enterinfo);
        ///// <summary>
        ///// 逻辑还原BOR清单明细信息
        ///// </summary>
        ///// <param name="enterinfo"></param>
        ///// <returns></returns>
        //Task<OperationResult> LogicRestore(params ProManufacturingBORBillItemInfo[] enterinfo);


        #endregion
    }
}
