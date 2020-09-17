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
    public interface ICommOpcUaBusinessContract : IScopeDependency
    {
        #region Opc Ua 业务信息
        /// <summary>
        /// 获取Opc Ua 业务数据查询数据集
        /// </summary>
        IQueryable<CommOpcUaBusiness> CommOpcUaBusinessInfos { get; }

        /// <summary>
        /// 检查组Opc Ua 业务数据信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的Opc Ua 业务数据编号</param>
        /// <returns>Opc Ua 业务数据是否存在</returns>
        bool CheckCommOpcUaBusinessExists(Expression<Func<CommOpcUaBusiness, bool>> predicate, Guid id);

        /// <summary>
        /// 添加Opc Ua 业务数据
        /// </summary>
        /// <param name="inputDtos">要添加的Opc Ua 业务数据DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Add(params CommOpcUaBusinessInputDto[] inputDtos);

        /// <summary>
        /// 更新Opc Ua 业务数据信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的Opc Ua 业务数据DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> Update(params CommOpcUaBusinessInputDto[] inputDtos);

        /// <summary>
        /// 删除Opc Ua 业务数据
        /// </summary>
        /// <param name="ids">要删除的Opc Ua 业务数据编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCommOpcUaBusinesss(params Guid[] ids);

        /// <summary>
        /// 逻辑删除Opc Ua 业务数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        Task<OperationResult> RecycleCommOpcUaBusinesss(params CommOpcUaBusiness[] data);
        /// <summary>
        /// 恢复逻辑删除Opc Ua 业务数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> RestoreCommOpcUaBusinesss(params CommOpcUaBusiness[] inputDtos);

        #endregion
    }
}
