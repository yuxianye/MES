using OSharp.Core.Data;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Dtos
{
    /// <summary>
    /// 设备点表输入Dto
    /// </summary>
    public class DeviceNodeInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 设备服务器信息
        /// </summary>
        public virtual DeviceServerInfo DeviceServerInfo { get; set; }

        /// <summary>
        /// 设备服务器ID
        /// </summary>
        public Guid DeviceServerInfo_Id { get; set; }

        /// <summary>
        /// 设备通讯服务器名称
        /// </summary>
        public string DeviceServerInfoName { get; set; }

        /// <summary>
        /// 数据点ID
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// 数据点名字
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 数据点URL
        /// </summary>
        public string NodeUrl { get; set; }

        /// <summary>
        /// 数据点订阅时间间隔
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 数据点类型
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastUpdatorUserId { get; set; }

    }
}
