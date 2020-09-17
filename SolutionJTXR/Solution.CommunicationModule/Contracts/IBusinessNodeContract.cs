using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Contracts
{
    /// <summary>
    /// 业务点表契约接口
    /// </summary>
    public interface IBusinessNodeContract : IScopeDependency
    {
        #region 业务点表信息
        /// <summary>
        /// 获取业务点表查询数据集
        /// </summary>
        IQueryable<BusinessNode> BusinessNodes { get; }

        /// <summary>
        /// 检查业务点表信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的业务点表数据编号</param>
        /// <returns>业务点表是否存在</returns>
        bool CheckBusinessNodeExists(Expression<Func<BusinessNode, bool>> predicate, Guid id);

        /// <summary>
        /// 添加业务点表
        /// </summary>
        /// <param name="inputDtos">要添加的业务点表DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params BusinessNodeInputDto[] inputDtos);

        /// <summary>
        /// 更新业务点表信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的业务点表DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params BusinessNodeInputDto[] inputDtos);

        /// <summary>
        /// 删除业务点表
        /// </summary>
        /// <param name="ids">要删除的业务点表编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteBusinessNodes(params Guid[] ids);

        #endregion
    }
}
