using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Contracts
{
    public interface ICommOpcUaServerConract : IScopeDependency
    {
        #region OpcUa服务器信息业务
        /// <summary>
        /// 获取OpcUa服务器信息查询数据集
        /// </summary>
        IQueryable<CommOpcUaServer> CommOpcUaServerInfos { get; }

        IQueryable<CommOpcUaServer> CommOpcUaServerTrackInfos { get; }

        /// <summary>
        /// 检查组OpcUa服务器信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的OpcUa服务器信息编号</param>
        /// <returns>OpcUa服务器信息是否存在</returns>
        bool CheckCommOpcUaServerExists(Expression<Func<CommOpcUaServer, bool>> predicate, Guid id);

        /// <summary>
        /// 添加OpcUa服务器信息
        /// </summary>
        /// <param name="inputDtos">要添加的OpcUa服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddCommOpcUaServers(params CommOpcUaServerInputDto[] inputDtos);

        /// <summary>
        /// 更新OpcUa服务器信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的OpcUa服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditCommOpcUaServers(params CommOpcUaServerInputDto[] inputDtos);

        /// <summary>
        /// 删除OpcUa服务器信息
        /// </summary>
        /// <param name="ids">要删除的OpcUa服务器信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteCommOpcUaServers(params Guid[] ids);

        /// <summary>
        /// 逻辑删除OpcUa服务器数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        Task<OperationResult> RecycleCommOpcUaServer(params CommOpcUaServer[] data);

        /// <summary>
        /// 恢复逻辑删除OpcUa服务器数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        Task<OperationResult> RestoreCommOpcUaServer(params CommOpcUaServer[] data);



        #endregion
    }
}
