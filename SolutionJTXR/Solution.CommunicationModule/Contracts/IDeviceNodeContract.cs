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
    /// 设备点表契约接口
    /// </summary>
    public interface IDeviceNodeContract : IScopeDependency
    {
        #region 设备数据点信息业务
        /// <summary>
        /// 获取设备数据点信息查询数据集
        /// </summary>
        IQueryable<DeviceNode> DeviceNodes { get; }

        /// <summary>
        /// 获取设备数据点信息修改数据集
        /// </summary>
        IQueryable<DeviceNode> DeviceNodesTrack { get; }

        /// <summary>
        /// 检查设备数据点信息是否存在
        /// </summary>
        /// <param name="predicate">检查谓语表达式</param>
        /// <param name="id">设备数据点编号</param>
        /// <returns>设备数据点是否存在</returns>
        bool CheckDeviceNodeExists(Expression<Func<DeviceNode, bool>> predicate, Guid id);

        /// <summary>
        /// 添加设备数据点
        /// </summary>
        /// <param name="inputDtos">要添加的设备数据点DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> AddDeviceNodes(params DeviceNodeInputDto[] inputDtos);

        /// <summary>
        /// 更新设备数据点信息
        /// </summary>
        /// <param name="inputDtos">包含更新的设备数据点DTO信息</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> EditDeviceNodes(params DeviceNodeInputDto[] inputDtos);

        /// <summary>
        /// 删除设备数据点信息
        /// </summary>
        /// <param name="ids">要删除的设备数据点编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteDeviceNodes(params Guid[] ids);

        #endregion
    }
}
