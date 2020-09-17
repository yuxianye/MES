using OSharp.Core.Data;
using Solution.CommunicationModule.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.CommunicationModule.Dtos
{
    /// <summary>
    /// 历史数据输入Dto
    /// </summary>
    public class HistoryDataInputDto : IInputDto<long>
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备服务器ID
        /// </summary>
        public String DeviceServer_Id { get; set; }

        /// <summary>
        /// 设备服务器名称
        /// </summary>
        public String DeviceServerName { get; set; }

        /// <summary>
        /// 设备点表ID
        /// </summary>
        public String DeviceNode_Id { get; set; }

        /// <summary>
        /// 设备点名
        /// </summary>
        public String NodeName { get; set; }

        /// <summary>
        /// 数据结果
        /// </summary>
        public String DataValue { get; set; }

        /// <summary>
        /// 数据质量
        /// </summary>
        public int? Quality { get; set; }

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        [StringLength(50)]
        public string CreatorUserId { get; set; }
    }
}
