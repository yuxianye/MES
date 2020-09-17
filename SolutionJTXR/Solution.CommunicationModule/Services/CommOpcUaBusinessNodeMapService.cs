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
    public class CommOpcUaBusinessNodeMapService : ICommOpcUaBusinessNodeMapContract
    {
        public IRepository<CommOpcUaBusinessNodeMap, Guid> CommOpcUaBusinessNodeMapRepository { get; set; }
        public IRepository<DeviceNode, Guid> CommOpcUaNodeRepository { get; set; }
        public IRepository<CommOpcUaBusiness, Guid> CommOpcUaBusinessRepository { get; set; }
        /// <summary>
        /// 获取OpcUa业务数据关联数据点信息查询数据集
        /// </summary>
        public IQueryable<CommOpcUaBusinessNodeMap> CommOpcUaBusinessNodeMapInfos => CommOpcUaBusinessNodeMapRepository.Entities;

        /// <summary>
        /// 添加OpcUa业务数据关联数据点信息
        /// </summary>
        /// <param name="inputDtos">要添加的OpcUa业务数据关联数据点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddCommOpcUaBusinessNodeMaps(params CommOpcUaBusinessNodeMapInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            CommOpcUaBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaBusinessNodeMapRepository.InsertAsync(inputDtos);
            CommOpcUaBusinessNodeMapRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 检查OpcUa业务数据关联数据点信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的OpcUa业务数据关联数据点信息编号</param>
        /// <returns>OpcUa业务数据关联数据点信息是否存在</returns>
        public bool CheckCommOpcUaBusinessNodeMapExists(Expression<Func<CommOpcUaBusinessNodeMap, bool>> predicate, Guid id) => CommOpcUaBusinessNodeMapRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除OpcUa业务数据关联数据点信息
        /// </summary>
        /// <param name="ids">要删除的OpcUa业务数据关联数据点信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteCommOpcUaBusinessNodeMaps(params Guid[] ids)
        {
            ids.CheckNotNull("ids");

            CommOpcUaBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaBusinessNodeMapRepository.DeleteAsync(ids);
            CommOpcUaBusinessNodeMapRepository.UnitOfWork.Commit();

            return result;
        }

        /// <summary>
        /// 更新OpcUa业务数据关联数据点信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的OpcUa业务数据关联数据点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditCommOpcUaBusinessNodeMaps(params CommOpcUaBusinessNodeMapInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            CommOpcUaBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
            var result = await CommOpcUaBusinessNodeMapRepository.UpdateAsync(inputDtos);
            CommOpcUaBusinessNodeMapRepository.UnitOfWork.Commit();

            return result;
        }
        /// <summary>
        /// 设备业务数据点配置
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> Setting(params CommOpcUaBusinessManageInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");         
            OperationResult result = new OperationResult();
            OperationResult result2 = new OperationResult();
            foreach (var inputDto in inputDtos)
            {
                if (Equals(inputDto.OpcUaBusiness_Id, Guid.Empty))
                {
                    return new OperationResult(OperationResultType.Error, "请选择业务名称！");
                }
                if (Equals(inputDto.EquipmentID, Guid.Empty))
                {
                    return new OperationResult(OperationResultType.Error, "请选择设备名称！");
                }
                int count = inputDto.CommOpcUaNodeInfoList.Count();
                if (count >= 0)
                {
                    CommOpcUaBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
                    var oldmaplist = CommOpcUaBusinessNodeMapRepository.TrackEntities.Where(x => x.EquipmentID == inputDto.EquipmentID && x.OpcUaBusiness.Id == inputDto.OpcUaBusiness_Id);
                    int count0 = oldmaplist.Count();
                    if (count0 > 0)
                    {
                        Guid[] mapIds = new Guid[count0];
                        mapIds = oldmaplist.Select(x => x.Id).ToArray();
                        result2 = await CommOpcUaBusinessNodeMapRepository.DeleteAsync(mapIds);
                    }
                    if (count == 0 && count0 > 0)
                    {
                        result = result2;
                    }
                    if ((result2.Successed || count0 == 0) && count > 0)
                    {
                        CommOpcUaBusinessNodeMapInputDto[] map_dtos = new CommOpcUaBusinessNodeMapInputDto[count];
                        for (int i = 0; i < count; i++)
                        {
                            CommOpcUaBusinessNodeMapInputDto edto = new CommOpcUaBusinessNodeMapInputDto();
                            edto.EquipmentID = inputDto.EquipmentID;
                            var id = inputDto.CommOpcUaNodeInfoList[i].Id;
                            edto.OpcUaNode = CommOpcUaNodeRepository.TrackEntities.Where(m => m.Id == id).FirstOrDefault();
                            edto.OpcUaBusiness = CommOpcUaBusinessRepository.TrackEntities.Where(m => m.Id == inputDto.OpcUaBusiness_Id).FirstOrDefault();
                            edto.CreatorUserId = inputDto.CreatorUserId;
                            edto.CreatedTime = inputDto.CreatedTime;
                            edto.LastUpdatedTime = inputDto.LastUpdatedTime;
                            edto.LastUpdatorUserId = inputDto.LastUpdatorUserId;
                            if (Equals(edto.OpcUaBusiness, null))
                            {
                                return new OperationResult(OperationResultType.Error, $"对应的通讯业务信息不存在,该组数据不被存储。");
                            }
                            if (Equals(edto.OpcUaNode, null))
                            {
                                return new OperationResult(OperationResultType.Error, $"对应的数据点不存在,该组数据不被存储。");
                            }
                            map_dtos[i] = edto;
                        }
                        result = await CommOpcUaBusinessNodeMapRepository.InsertAsync(map_dtos);        
                    }
                    CommOpcUaBusinessNodeMapRepository.UnitOfWork.Commit();
                }
                else
                {
                    return new OperationResult(OperationResultType.Error, $"设备业务数据点配置数据异常,该组数据不被存储。");
                }
            }

            return result;
        }
        /// <summary>
        /// 逻辑删除OpcUa业务数据关联数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns>删除条数</returns>
        public async Task<OperationResult> RecycleCommOpcUaBusinessNodeMap(params CommOpcUaBusinessNodeMap[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                CommOpcUaBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
                count = await CommOpcUaBusinessNodeMapRepository.RecycleAsync(data);
                CommOpcUaBusinessNodeMapRepository.UnitOfWork.Commit();
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
                names.Add(entity.OpcUaBusiness.BusinessName);
            }
            return count > 0
                    ? new OperationResult(OperationResultType.Success,
                        names.Count > 0
                            ? "信息“{0}”逻辑删除成功".FormatWith(names.ExpandAndToString())
                            : "{0}个信息逻辑删除成功".FormatWith(count))
                    : new OperationResult(OperationResultType.NoChanged);

        }

        /// <summary>
        /// 恢复逻辑删除OpcUa业务数据关联数据
        /// </summary>
        /// <param name="inputDtos"></param>
        /// <returns></returns>
        public async Task<OperationResult> RestoreCommOpcUaBusinessNodeMap(params CommOpcUaBusinessNodeMap[] data)
        {
            data.CheckNotNull("data");
            int count = 0;
            try
            {
                CommOpcUaBusinessNodeMapRepository.UnitOfWork.BeginTransaction();
                count = await CommOpcUaBusinessNodeMapRepository.RestoreAsync(data);
                CommOpcUaBusinessNodeMapRepository.UnitOfWork.Commit();
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
                names.Add(entity.OpcUaBusiness.BusinessName);
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
