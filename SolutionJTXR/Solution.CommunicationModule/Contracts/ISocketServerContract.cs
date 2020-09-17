using OSharp.Core.Dependency;
using OSharp.Utility.Data;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Contracts
{
    /// <summary>
    /// 实时通讯服务器契约接口
    /// </summary>
    public interface ISocketServerContract : IScopeDependency
    {
        #region 实时通讯服务器信息业务
        /// <summary>
        /// 获取实时通讯服务器信息查询数据集
        /// </summary>
        IQueryable<SocketServer> SocketServerInfos { get; }

        /// <summary>
        /// 检查实时通讯服务器信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新实时通讯服务器信息编号</param>
        /// <returns>实时通讯服务器信息是否存在</returns>
        bool CheckSocketServerExists(Expression<Func<SocketServer, bool>> predicate, Guid id);

        /// <summary>
        /// 添加实时通讯服务器信息
        /// </summary>
        /// <param name="inputDtos">要添加的实时通讯服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddSocketServers(params SocketServerInputDto[] inputDtos);

        /// <summary>
        /// 更新实时通讯服务器信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的实时通讯服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditSocketServers(params SocketServerInputDto[] inputDtos);

        /// <summary>
        /// 删除实时通讯服务器信息
        /// </summary>
        /// <param name="ids">要删除的实时通讯服务器信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteSocketServers(params Guid[] ids);

        #endregion
    }
}
