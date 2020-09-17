using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Models
{
    /// <summary>
    /// 实体类-设备通讯服务信息DTO
    /// </summary>
    [Description("设备通讯服务信息")]
    public class DeviceServerInfoInputDto : IInputDto<Guid>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 设备服务名称，例如OpcUa、Opc。与类名的前缀一样。
        /// </summary>
        [Display(Description = "设备服务名称")]
        [Required, StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string DeviceServerName { get; set; }

        /// <summary>
        /// 设备连接地址、连接字符串例如"opc.tcp://127.0.0.1:49320/" "COM1" "192.168.1.235";
        /// </summary>
        public string DeviceUrl { get; set; }

        /// <summary>
        /// 设备连驱动程序集名称，程序集dll名称。例如Solution.Device.OpcUaDevice
        /// </summary>
        [Display(Description = "设备驱动程序集名称")]
        [Required, StringLength(100)]
        public string DeviceDriveAssemblyName { get; set; }

        /// <summary>
        /// 设备连驱动类名称。例如Solution.Device.OpcUaDevice.OpcUaDeviceHelper;
        /// </summary>
        [Display(Description = "设备驱动类名称")]
        [Required, StringLength(100)]
        public string DeviceDriveClassName { get; set; }

        [Display(Description = "备注")]
        [StringLength(200)]
        public string Remark { get; set; }

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

        [Display(Description = "是否锁定")]
        public bool IsLocked { get; set; }

    }
}
