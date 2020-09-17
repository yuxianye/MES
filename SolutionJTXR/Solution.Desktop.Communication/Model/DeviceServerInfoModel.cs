using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.Communication.Model
{
    /// <summary>
    /// DeviceServerInfo数据模型
    /// </summary>
    public class DeviceServerInfoModel : ModelBase, IAudited/*, IRecyclable, ILockable*/
    {
        #region Id
        private Guid id = CombHelper.NewComb();

        /// <summary>
        /// Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 服务名称

        private string deviceServerName;

        /// <summary>
        /// 设备服务名称，例如OpcUa、Opc。与类名的前缀一样。
        /// </summary>
        [Required(ErrorMessage = "必填项，限20个字符"), MaxLength(20, ErrorMessage = "长度小于20个字符")]
        public string DeviceServerName
        {
            get { return deviceServerName; }
            set { Set(ref deviceServerName, value); }
        }
        #endregion

        #region 服务地址

        private string deviceUrl;

        /// <summary>
        /// 设备连接地址、连接字符串/连接参数。例如"opc.tcp://127.0.0.1:49320/" "COM1,115200" "192.168.1.235" 或者json字符串;
        /// </summary>
        [Required(ErrorMessage = "必填项，限200个字符"), MaxLength(200, ErrorMessage = "长度小于200个字符")]
        public string DeviceUrl
        {
            get { return deviceUrl; }
            set { Set(ref deviceUrl, value); }
        }
        #endregion

        #region 驱动程序集名称

        private string deviceDriveAssemblyName = "Solution.Device.OpcUaDevice";
        /// <summary>
        /// 设备连驱动程序集名称，程序集dll名称。例如Solution.Device.OpcUaDevice
        /// </summary>
        [Required(ErrorMessage = "必填项，限200个字符"), MaxLength(200, ErrorMessage = "长度小于200个字符")]
        public string DeviceDriveAssemblyName
        {
            get { return deviceDriveAssemblyName; }
            set { Set(ref deviceDriveAssemblyName, value); }
        }
        #endregion

        #region 驱动程序集名称

        private string deviceDriveClassName = "Solution.Device.OpcUaDevice.OpcUaDeviceHelper";
        /// <summary>
        /// 设备连驱动类名称。例如Solution.Device.OpcUaDevice.OpcUaDeviceHelper;
        /// </summary>
        [Required(ErrorMessage = "必填项，限200个字符"), MaxLength(200, ErrorMessage = "长度小于200个字符")]
        public string DeviceDriveClassName
        {
            get { return deviceDriveClassName; }
            set { Set(ref deviceDriveClassName, value); }
        }
        #endregion

        #region 备注 

        private string remark;
        [MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
        }
        #endregion

        #region 记录创建时间
        private DateTime createdTime;

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { Set(ref createdTime, value); }
        }
        #endregion

        #region 创建者编号
        private string creatorUserId;

        /// <summary>
        /// 创建者编号
        /// </summary>
        public string CreatorUserId
        {
            get { return creatorUserId; }
            set { Set(ref creatorUserId, value); }
        }
        #endregion

        #region 最后更新时间
        private DateTime? lastUpdatedTime;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdatedTime
        {
            get { return lastUpdatedTime; }
            set { Set(ref lastUpdatedTime, value); }
        }
        #endregion

        #region 最后更新者编号
        private string lastUpdatorUserId;

        /// <summary>
        /// 最后更新者编号
        /// </summary>
        public string LastUpdatorUserId
        {
            get { return lastUpdatorUserId; }
            set { Set(ref lastUpdatorUserId, value); }
        }
        #endregion


        #region 是否锁定
        private bool isLocked;

        /// <summary>
        /// 是否锁定
        /// </summary>
        [Display(Description = "是否锁定")]
        public bool IsLocked
        {
            get { return isLocked; }
            set { Set(ref isLocked, value); }
        }
        #endregion

        protected override void Disposing()
        {
        }

    }

}
