using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.CommunicationModule.Models
{
    /// <summary>
    /// 实体类——通讯数据点配置
    /// </summary>
    [Description("通讯数据点配置")]
    public class DeviceNode : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 设备服务器信息
        /// </summary>
        public virtual DeviceServerInfo DeviceServerInfo { get; set; }

        /// <summary>
        /// 数据点名称
        /// </summary>
        [Display(Description = "数据点名称")]
        [Required, StringLength(100)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string NodeName { get; set; }

        /// <summary>
        /// 数据点URL
        /// </summary>
        [Display(Description = "数据点URL")]
        [Required]
        public string NodeUrl { get; set; }

        /// <summary>
        /// 订阅间隔
        /// </summary>
        [Display(Description = "订阅间隔")]
        [Required]
        [Range(100, 10000, ErrorMessage = "100毫秒-10秒之间")]
        public int Interval { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [Display(Description = "数据类型")]
        [Required]
        public int DataType { get; set; }

        [Display(Description = "描述")]
        [StringLength(100)]
        public string Description { get; set; }

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        #endregion

        #region Implementation of ICreatedAudited

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        [StringLength(50)]
        public string CreatorUserId { get; set; }

        #endregion

        #region Implementation of IUpdateAutited

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
