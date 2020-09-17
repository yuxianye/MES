using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
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
    public class SocketServerService : ISocketServerContract
    {
        public IRepository<SocketServer, Guid> SocketServerRepository { get; set; }

        /// <summary>
        /// 获取实时通讯服务器信息查询数据集
        /// </summary>
        public IQueryable<SocketServer> SocketServerInfos => SocketServerRepository.Entities;

        /// <summary>
        /// 添加实时通讯服务器信息
        /// </summary>
        /// <param name="inputDtos">要添加的实时通讯服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddSocketServers(params SocketServerInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            if (SocketServerRepository.Entities.Count() > 0)
            {
                return new OperationResult(OperationResultType.Error, $"已经存在Socket服务器信息，不能再添加!");
            }

            SocketServerRepository.UnitOfWork.BeginTransaction();
            var result = await SocketServerRepository.InsertAsync(inputDtos);
            SocketServerRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 检查组实时通讯服务器信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的实时通讯服务器信息编号</param>
        /// <returns>实时通讯服务器信息是否存在</returns>
        public bool CheckSocketServerExists(Expression<Func<SocketServer, bool>> predicate, Guid id) => SocketServerRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除实时通讯服务器信息
        /// </summary>
        /// <param name="ids">要删除的实时通讯服务器信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteSocketServers(params Guid[] ids)
        {
            ids.CheckNotNull("ids");

            SocketServerRepository.UnitOfWork.BeginTransaction();
            var result = await SocketServerRepository.DeleteAsync(ids);
            SocketServerRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 更新实时通讯服务器信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的实时通讯服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditSocketServers(params SocketServerInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            SocketServerRepository.UnitOfWork.BeginTransaction();
            var result = await SocketServerRepository.UpdateAsync(inputDtos);
            SocketServerRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 逻辑删除实时通讯服务器数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        public async Task<OperationResult> RecycleSocketServer(params SocketServer[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                SocketServerRepository.UnitOfWork.BeginTransaction();
                count = await SocketServerRepository.RecycleAsync(data);
                SocketServerRepository.UnitOfWork.Commit();
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
        /// 恢复逻辑删除实时通讯服务器数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> RestoreSocketServer(params SocketServer[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                SocketServerRepository.UnitOfWork.BeginTransaction();
                count = await SocketServerRepository.RestoreAsync(data);
                SocketServerRepository.UnitOfWork.Commit();
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
