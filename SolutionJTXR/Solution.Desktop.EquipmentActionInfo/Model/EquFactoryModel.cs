using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquFactoryInfo.Model
{
    /// <summary>
    /// 设备厂家信息模型
    /// </summary>
    public class EquFactoryModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 厂家名称
        private string factoryName;

        /// <summary>
        /// 厂家名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string FactoryName
        {
            get { return factoryName; }
            set { Set(ref factoryName, value); }
        }
        #endregion

        #region 厂家编号
        private string factoryCode;

        /// <summary>
        /// 厂家编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string FactoryCode
        {
            get { return factoryCode; }
            set { Set(ref factoryCode, value); }
        }
        #endregion

        #region 厂家地址
        private string factoryAddress;

        /// <summary>
        /// 厂家地址
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string FactoryAddress
        {
            get { return factoryAddress; }
            set { Set(ref factoryAddress, value); }
        }
        #endregion

        #region 联系人
        private string contacts;

        /// <summary>
        /// 联系人
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Contacts
        {
            get { return contacts; }
            set { Set(ref contacts, value); }
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

        protected override void Disposing()
        {
            FactoryName = null;
            FactoryCode = null;
            FactoryAddress = null;
            Contacts = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }
    }
}
