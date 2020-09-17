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
    public interface ICommOpcUaBusinessNodeMapContract : IScopeDependency
    {
        #region OpcUa业务数据关联数据点信息业务
        /// <summary>
        /// 获取OpcUa业务数据关联数据点信息查询数据集
        /// </summary>
        IQueryable<CommOpcUaBusinessNodeMap> CommOpcUaBusinessNodeMapInfos { get; }

        /// <summary>
        /// 检查OpcUa业务数据关联数据点信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的OpcUa业务数据关联数据点信息编号</param>
        /// <returns>OpcUa业务数据关联数据点信息是否存在</returns>
        bool CheckCommOpcUaBusinessNodeMapExists(Expression<Func<CommOpcUaBusinessNodeMap, bool>> predicate, Guid id);

        /// <summary>
        /// 添加OpcUa业务数据关联数据点信息
        /// </summary>
        /// <param name="inputDtos">要添加的OpcUa业务数据关联数据点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddCommOpcUaBusinessNodeMaps(params CommOpcUaBusinessNodeMapInputDto[] inputDtos);

        /// <summary>
        /// 更新OpcUa业务数据关联数据点信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的OpcUa业务数据关联数据点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditCommOpcUaBusinessNodeMaps(params CommOpcUaBusinessNodeMapInputDto[] inputDtos);

        /// <summary>
        /// 删除OpcUa业务数据关联数据点信息
        /// </summary>
        /// <param name="ids">要删除的OpcUa业务数据关联数据点信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCommOpcUaBusinessNodeMaps(params Guid[] ids);

        /// <summary>
        /// 逻辑删除OpcUa业务数据关联数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        Task<OperationResult> RecycleCommOpcUaBusinessNodeMap(params CommOpcUaBusinessNodeMap[] data);

        /// <summary>
        /// 恢复逻辑删除OpcUa业务数据关联数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> RestoreCommOpcUaBusinessNodeMap(params CommOpcUaBusinessNodeMap[] data);
        Task<OperationResult> Setting(params CommOpcUaBusinessManageInputDto[] inputDtos);
        #endregion
    }
}
