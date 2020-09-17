using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.CommunicationModule.Models
{
    /// <summary>
    /// 实体类——OpcUa通信历史
    /// </summary>
    [Description("历史数据")]
    public class HistoryData : EntityBase<long>
    {
        /// <summary>
        /// 设备服务器ID
        /// </summary>
        [StringLength(36)]
        public string DeviceServer_Id { get; set; }

        /// <summary>
        /// 设备服务器名称
        /// </summary>
        [StringLength(50)]
        public String DeviceServerName { get; set; }

        /// <summary>
        /// 设备点表ID
        /// </summary>
        [StringLength(36)]
        public String DeviceNode_Id { get; set; }

        [Display(Description = "点名")]
        [StringLength(100)]
        public String NodeName { get; set; }

        [Display(Description = "点值")]
        [StringLength(100)]
        public String DataValue { get; set; }

        [Display(Description = "数据质量")]
        public int? Quality { get; set; }

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public Nullable<DateTime> CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        [StringLength(50)]
        public string CreatorUserId { get; set; }
    }
}
