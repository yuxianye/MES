using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquipmentTypeInfo.Model
{
    /// <summary>
    /// 设备类别信息模型
    /// </summary>
    public class EquipmentTypeModel : ModelBase, /*ILockable, IRecyclable,*/ IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 设备类型ID
        /// </summary>
        [DisplayName("ID")]
        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 设备类别名称
        private string equipmentTypeName;

        /// <summary>
        /// 设备类型名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        [DisplayName("设备类别名称")]
        public string EquipmentTypeName
        {
            get { return equipmentTypeName; }
            set { Set(ref equipmentTypeName, value); }
        }

        #endregion

        #region 设备类别编号
        private string equipmentTypeCode;

        /// <summary>
        /// 设备类别编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        [DisplayName("设备类别编号")]
        public string EquipmentTypeCode
        {
            get { return equipmentTypeCode; }
            set { Set(ref equipmentTypeCode, value); }
        }

        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度小于200个字符")]
        [DisplayName("备注")]
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
        [DisplayName("记录创建时间")]
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
        [DisplayName("创建者")]
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
        [DisplayName("最后更新时间")]
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
        [DisplayName("最后更新者")]
        public string LastUpdatorUserId
        {
            get { return lastUpdatorUserId; }
            set { Set(ref lastUpdatorUserId, value); }
        }
        #endregion

        protected override void Disposing()
        {
            EquipmentTypeName = null;
            EquipmentTypeCode = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
