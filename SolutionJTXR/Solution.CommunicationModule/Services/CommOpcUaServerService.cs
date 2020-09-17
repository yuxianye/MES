using OSharp.Core.Data;
using OSharp.Utility;
using OSharp.Utility.Data;
using OSharp.Utility.Exceptions;
using OSharp.Utility.Extensions;
using Solution.CommunicationModule.Contracts;
using Solution.CommunicationModule.Dtos;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Services
{
    public class CommOpcUaServerService : ICommOpcUaServerConract
    {
        public IRepository<CommOpcUaServer, Guid> CommOpcUaServerRepository { get; set; }
        public IRepository<CommOpcUaServerNodeMap, Guid> CommOpcUaServerNodeMapRepository { get; set; }
        /// <summary>
        /// 获取OpcUa服务器信息查询数据集
        /// </summary>
        public IQueryable<CommOpcUaServer> CommOpcUaServerInfos
        {
            get => CommOpcUaServerRepository.Entities;
        }

        public IQueryable<CommOpcUaServer> CommOpcUaServerTrackInfos
        {
            get => CommOpcUaServerRepository.TrackEntities;
        }

        /// <summary>
        /// 添加OpcUa服务器信息
        /// </summary>
        /// <param name="inputDtos">要添加的OpcUa服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddCommOpcUaServers(params CommOpcUaServerInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (CommOpcUaServerRepository.CheckExists(x => x.ServerName.Equals(dtoData.ServerName)))
                    return new OperationResult(OperationResultType.Error, $"服务名称为{dtoData.ServerName}的信息已存在，数据不合法，该组数据不被存储");
                if (CommOpcUaServerRepository.CheckExists(x => x.ServerName.Equals(dtoData.Url)))
                    return new OperationResult(OperationResultType.Error, $"服务链接为{dtoData.Url}的信息已存在，数据不合法，该组数据不被存储");
            }
            CommOpcUaServerRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaServerRepository.InsertAsync(inputDtos);
            CommOpcUaServerRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 检查组OpcUa服务器信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的OpcUa服务器信息编号</param>
        /// <returns>OpcUa服务器信息是否存在</returns>
        public bool CheckCommOpcUaServerExists(Expression<Func<CommOpcUaServer, bool>> predicate, Guid id) => CommOpcUaServerRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除OpcUa服务器信息
        /// </summary>
        /// <param name="ids">要删除的OpcUa服务器信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteCommOpcUaServers(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                if (CommOpcUaServerNodeMapRepository.CheckExists(x => x.OpcUaServer.Id.Equals(id)))
                    return new OperationResult(OperationResultType.Error, "服务器已经关联数据点，不能删除！");
            }
            CommOpcUaServerRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaServerRepository.DeleteAsync(ids);
            CommOpcUaServerRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 更新OpcUa服务器信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的OpcUa服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditCommOpcUaServers(params CommOpcUaServerInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            CommOpcUaServerRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaServerRepository.UpdateAsync(inputDtos);
            CommOpcUaServerRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 逻辑删除OpcUa服务器数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        public async Task<OperationResult> RecycleCommOpcUaServer(params CommOpcUaServer[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                CommOpcUaServerRepository.UnitOfWork.BeginTransaction();
                count = await CommOpcUaServerRepository.RecycleAsync(data);
                CommOpcUaServerRepository.UnitOfWork.Commit();
            }
            catch (DataException dataException)
            {
                return new OperationResult(OperationResultType.Error, dataException.Message);
            }
            catch (OSharpException osharpException)
            {
                return new OperationResult(OperationResultType.Error, osharpException.Message);
            }

            List<string> names = new List<string>();
            foreach (var entity in data)
            {
                names.Add(entity.ServerName);
            }
            return count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑删除成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 恢复逻辑删除OpcUa服务器数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> RestoreCommOpcUaServer(params CommOpcUaServer[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                CommOpcUaServerRepository.UnitOfWork.BeginTransaction();
                count = await CommOpcUaServerRepository.RestoreAsync(data);
                CommOpcUaServerRepository.UnitOfWork.Commit();
            }
            catch (DataException dataException)
            {
                return new OperationResult(OperationResultType.Error, dataException.Message);
            }
            catch (OSharpException osharpException)
            {
                return new OperationResult(OperationResultType.Error, osharpException.Message);
            }

            List<string> names = new List<string>();
            foreach (var entity in data)
            {
                names.Add(entity.ServerName);
            }
            return count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑还原成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);
        }
    }
}
