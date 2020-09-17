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
    /// <summary>
    /// 设备服务器信息契约接口
    /// </summary>
    public interface IDeviceServerInfoConract : IScopeDependency
    {
        #region DeviceServerInfo业务
        /// <summary>
        /// 获取设备通讯服务器数据集
        /// </summary>
        IQueryable<DeviceServerInfo> DeviceServerInfos { get; }

        /// <summary>
        /// 获取设备通讯服务器可跟踪的数据集
        /// </summary>
        IQueryable<DeviceServerInfo> DeviceServerTrackInfos { get; }

        /// <summary>
        /// 检查设备通讯服务信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">更新的设备通讯服务信息编号</param>
        /// <returns>设备通讯服务器信息是否存在</returns>
        bool CheckDeviceServerInfoExists(Expression<Func<DeviceServerInfo, bool>> predicate, Guid id);

        /// <summary>
        /// 添加设备通讯服务器信息
        /// </summary>
        /// <param name="inputDtos">要添加的设备通讯服务器DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddDeviceServerInfos(params DeviceServerInfoInputDto[] inputDtos);

        /// <summary>
        /// 更新设备通讯服务器信息
        /// </summary>
        /// <param name="inputDtos">包含更新的设备通讯服务器DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditDeviceServerInfos(params DeviceServerInfoInputDto[] inputDtos);

        /// <summary>
        /// 删除设备通讯服务器信息
        /// </summary>
        /// <param name="ids">要删除的设备通讯服务器信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteDeviceServerInfos(params Guid[] ids);

        #endregion
    }
}
