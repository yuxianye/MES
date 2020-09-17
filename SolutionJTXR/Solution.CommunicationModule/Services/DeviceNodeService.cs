using OSharp.Core.Data;
using OSharp.Core.Data.Extensions;
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
    /// <summary>
    /// 设备点表服务
    /// </summary>
    public class DeviceNodeService : IDeviceNodeContract
    {
        /// <summary>
        /// 设备点表仓储
        /// </summary>
        public IRepository<DeviceNode, Guid> DeviceNodeRepository { get; set; }

        /// <summary>
        /// 设备服务器仓储
        /// </summary>
        public IRepository<DeviceServerInfo, Guid> DeviceServerInfoRepository { get; set; }

        /// <summary>
        /// 业务点表map仓储
        /// </summary>
        public IRepository<ProductionProcessEquipmentBusinessNodeMap, Guid> ProductionProcessEquipmentBusinessNodeMapRepository { get; set; }

        /// <summary>
        /// 获取设备数据点信息查询数据集
        /// </summary>
        public IQueryable<DeviceNode> DeviceNodes => DeviceNodeRepository.Entities;

        /// <summary>
        /// 设备点表数据集（可跟踪）
        /// </summary>
        public IQueryable<DeviceNode> DeviceNodesTrack => DeviceNodeRepository.TrackEntities;

        /// <summary>
        /// 添加设备数据点信息
        /// </summary>
        /// <param name="inputDtos">要添加的设备数据点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddDeviceNodes(params DeviceNodeInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.NodeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写点表名称，不能为空。");
                if (string.IsNullOrEmpty(dtoData.NodeUrl))
                    return new OperationResult(OperationResultType.Error, "请正确填写通讯点Url，不能为空。");
                dtoData.DeviceServerInfo = DeviceServerInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.DeviceServerInfo_Id);
                if (Equals(dtoData.DeviceServerInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备通讯服务器不存在,数据添加失败。");
                }
                if (DeviceNodeRepository.CheckExists(x => x.NodeUrl == dtoData.NodeUrl && x.DeviceServerInfo.Id == dtoData.DeviceServerInfo_Id))
                    return new OperationResult(OperationResultType.Error, $"设备通讯服务器：{dtoData.DeviceServerInfo.DeviceServerName}，数据点URL：{dtoData.NodeUrl} 的数据已存在，请使用其他Url或选择其他设备服务器。");
            }
            var result = await DeviceNodeRepository.InsertAsync(inputDtos);
            return result;
        }

        /// <summary>
        /// 检查设备数据点信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备数据点信息编号</param>
        /// <returns>设备数据点信息是否存在</returns>
        public bool CheckDeviceNodeExists(Expression<Func<DeviceNode, bool>> predicate, Guid id) => DeviceNodeRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除设备数据点信息
        /// </summary>
        /// <param name="ids">要删除的设备数据点信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteDeviceNodes(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                if (ProductionProcessEquipmentBusinessNodeMapRepository.CheckExists(x => x.DeviceNode.Id.Equals(id)))
                {
                    return new OperationResult(OperationResultType.Error, "数据点关联了业务点数据，不能删除！");
                }
            }
            var result = await DeviceNodeRepository.DeleteAsync(ids);
            return result;
        }

        /// <summary>
        /// 更新设备数据点信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的设备数据点信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditDeviceNodes(params DeviceNodeInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (string.IsNullOrEmpty(dtoData.NodeName))
                    return new OperationResult(OperationResultType.Error, "请正确填写点表名称，不能为空。");
                if (string.IsNullOrEmpty(dtoData.NodeUrl))
                    return new OperationResult(OperationResultType.Error, "请正确填写通讯点Url，不能为空。");
                if (DeviceNodeRepository.CheckExists(x => x.NodeUrl == dtoData.NodeUrl && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"通讯URL：{dtoData.NodeUrl} 的数据已被使用，请使用其他Url。");
                dtoData.DeviceServerInfo = DeviceServerInfoRepository.TrackEntities.FirstOrDefault(m => m.Id == dtoData.DeviceServerInfo_Id);

                if (Equals(dtoData.DeviceServerInfo, null))
                {
                    return new OperationResult(OperationResultType.Error, $"对应的设备通讯服务器不存在,该组数据不被存储。");
                }

                if (DeviceNodeRepository.CheckExists(x => x.NodeUrl == dtoData.NodeUrl && x.DeviceServerInfo.Id == dtoData.DeviceServerInfo_Id && x.Id != dtoData.Id))
                    return new OperationResult(OperationResultType.Error, $"设备通讯服务器：{dtoData.DeviceServerInfo.DeviceServerName}，数据点URL：{dtoData.NodeUrl} 的数据已被使用，请使用其他名称。");

            }
            var result = await DeviceNodeRepository.UpdateAsync(inputDtos);
            return result;
        }
    }
}
