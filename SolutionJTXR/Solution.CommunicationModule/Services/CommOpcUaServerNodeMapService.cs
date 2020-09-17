using OSharp.Core.Data;
using OSharp.Core.Mapping;
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
    public class CommOpcUaServerNodeMapService : ICommOpcUaServerNodeMapContract
    {
        public IRepository<CommOpcUaServerNodeMap, Guid> CommOpcUaServerNodeMapRepository { get; set; }

        //public IRepository<CommOpcUaServer, Guid> CommOpcUaServerRepository { get; set; }

        //public IRepository<CommOpcUaNode, Guid> CommOpcUaNodeRepository { get; set; } 

        /// <summary>
        /// 获取OpcUa服务器数据点关联信息查询数据集
        /// </summary>
        public IQueryable<CommOpcUaServerNodeMap> CommOpcUaServerNodeMapInfos => CommOpcUaServerNodeMapRepository.Entities;

        /// <summary>
        /// 添加OpcUa服务器数据点关联信息
        /// </summary>
        /// <param name="inputDtos">要添加的OpcUa服务器数据点关联信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddCommOpcUaServerNodeMaps(params CommOpcUaServerNodeMapInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            CommOpcUaServerNodeMapRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaServerNodeMapRepository.InsertAsync(inputDtos);
            CommOpcUaServerNodeMapRepository.UnitOfWork.Commit();
            return result;
        }

        /// <summary>
        /// 检查组OpcUa服务器数据点关联信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的OpcUa服务器数据点关联信息编号</param>
        /// <returns>OpcUa服务器数据点关联信息是否存在</returns>
        public bool CheckCommOpcUaServerNodeMapExists(Expression<Func<CommOpcUaServerNodeMap, bool>> predicate, Guid id) => CommOpcUaServerNodeMapRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除OpcUa服务器数据点关联信息
        /// </summary>
        /// <param name="ids">要删除的OpcUa服务器数据点关联信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteCommOpcUaServerNodeMaps(params Guid[] ids)
        {
            ids.CheckNotNull("ids");

            CommOpcUaServerNodeMapRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaServerNodeMapRepository.DeleteAsync(ids);
            CommOpcUaServerNodeMapRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 更新OpcUa服务器数据点关联信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的OpcUa服务器数据点关联信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditCommOpcUaServerNodeMaps(params CommOpcUaServerNodeMapInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            CommOpcUaServerNodeMapRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaServerNodeMapRepository.UpdateAsync(inputDtos);
            CommOpcUaServerNodeMapRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 逻辑删除OpcUa服务器数据点关联数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        public async Task<OperationResult> RecycleCommOpcUaServerNodeMaps(params CommOpcUaServerNodeMap[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                CommOpcUaServerNodeMapRepository.UnitOfWork.BeginTransaction();
                count = await CommOpcUaServerNodeMapRepository.RecycleAsync(data);
                CommOpcUaServerNodeMapRepository.UnitOfWork.Commit();
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
                names.Add(entity.OpcUaNode.NodeName);
            }
            return  count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑删除成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);
        }

        /// <summary>
        /// 恢复逻辑删除OpcUa服务器数据点关联数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> RestoreCommOpcUaServerNodeMaps(params CommOpcUaServerNodeMap[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                CommOpcUaServerNodeMapRepository.UnitOfWork.BeginTransaction();
                count = await CommOpcUaServerNodeMapRepository.RestoreAsync(data);
                CommOpcUaServerNodeMapRepository.UnitOfWork.Commit();
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
                names.Add(entity.OpcUaNode.NodeName);
            }
            return  count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑还原成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑还原成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);
        }
    }
}
