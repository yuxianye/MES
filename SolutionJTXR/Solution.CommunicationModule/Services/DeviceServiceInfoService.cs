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
    /// <summary>
    /// 设备通讯服务器 服务
    /// </summary>
    public class DeviceServerInfoService : IDeviceServerInfoConract
    {
        /// <summary>
        /// 设备服务器仓储
        /// </summary>
        public IRepository<DeviceServerInfo, Guid> DeviceServerInfoRepository { get; set; }

        /// <summary>
        /// 设备点仓储
        /// </summary>
        public IRepository<DeviceNode, Guid> DeviceNodeRepository { get; set; }

        /// <summary>
        /// 获取设备通讯服务器信息查询数据集
        /// </summary>
        public IQueryable<DeviceServerInfo> DeviceServerInfos
        {
            get => DeviceServerInfoRepository.Entities;
        }

        /// <summary>
        /// 设备服务器数据集（可跟踪）
        /// </summary>
        public IQueryable<DeviceServerInfo> DeviceServerTrackInfos
        {
            get => DeviceServerInfoRepository.TrackEntities;
        }

        /// <summary>
        /// 添加设备通讯服务器信息
        /// </summary>
        /// <param name="inputDtos">要添加的设备通讯服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> AddDeviceServerInfos(params DeviceServerInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");
            foreach (var dtoData in inputDtos)
            {
                if (DeviceServerInfoRepository.CheckExists(x => x.DeviceServerName.Equals(dtoData.DeviceServerName)))
                    return new OperationResult(OperationResultType.Error, $"服务名称为{dtoData.DeviceServerName}的信息已存在，请使用其他名称。");
                if (DeviceServerInfoRepository.CheckExists(x => x.DeviceServerName.Equals(dtoData.DeviceUrl)))
                    return new OperationResult(OperationResultType.Error, $"服务链接地址为{dtoData.DeviceUrl}的信息已存在，请使用其他名称。");
            }
            var result = await DeviceServerInfoRepository.InsertAsync(inputDtos);
            return result;
        }

        /// <summary>
        /// 检查设备通讯服务器信息信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备通讯服务器信息编号</param>
        /// <returns>设备通讯服务器信息是否存在</returns>
        public bool CheckDeviceServerInfoExists(Expression<Func<DeviceServerInfo, bool>> predicate, Guid id) => DeviceServerInfoRepository.CheckExists(predicate, id);

        /// <summary>
        /// 删除设备通讯服务器信息
        /// </summary>
        /// <param name="ids">要删除的设备通讯服务器信息编号</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> DeleteDeviceServerInfos(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            foreach (var id in ids)
            {
                if (DeviceNodeRepository.CheckExists(x => x.DeviceServerInfo.Id.Equals(id)))
                    return new OperationResult(OperationResultType.Error, "服务器已经关联数据点，不能删除！");
            }
            var result = await DeviceServerInfoRepository.DeleteAsync(ids);

            return result;
        }

        /// <summary>
        /// 更新设备通讯服务器信息信息
        /// </summary>
        /// <param name="inputDtos">包含更新信息的设备通讯服务器信息DTO信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult> EditDeviceServerInfos(params DeviceServerInfoInputDto[] inputDtos)
        {
            inputDtos.CheckNotNull("inputDtos");

            var result = await DeviceServerInfoRepository.UpdateAsync(inputDtos);

            return result;
        }
    }
}
