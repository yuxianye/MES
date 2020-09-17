using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.MatSupplierInfo.Model
{
    /// <summary>
    /// 供货商模型
    /// </summary>
    public class MatSupplierInfoModel : ModelBase, IAudited
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

        #region 供应商编号
        private string supplierCode;

        /// <summary>
        /// 供应商编号
        /// </summary>
        [Required(ErrorMessage = "供应商编号必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string SupplierCode
        {
            get { return supplierCode; }
            set { Set(ref supplierCode, value); }
        }

        #endregion

        #region 供应商名称
        private string supplierName;

        /// <summary>
        /// 供应商名称
        /// </summary>
        [Required(ErrorMessage = "供应商名称必填"), MaxLength(100, ErrorMessage = "长度小于100个字符")]
        public string SupplierName
        {
            get { return supplierName; }
            set { Set(ref supplierName, value); }
        }
        #endregion


        #region 供应商电话
        private string supplierPhone;

        /// <summary>
        /// 供应商电话
        /// </summary>
        //[ MaxLength(50, ErrorMessage = "长度小于50个字符")]
        [RegularExpression(@"^[1][3,4,5,6,7,8,9][0-9]{9}$", ErrorMessage = "无效的电话号码")]
        public string SupplierPhone
        {
            get { return supplierPhone; }
            set { Set(ref supplierPhone, value); }
        }
        #endregion

        #region 供应商传真
        private string supplierFax;

        /// <summary>
        /// 供应商传真
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string SupplierFax
        {
            get { return supplierFax; }
            set { Set(ref supplierFax, value); }
        }
        #endregion


        #region 供应商地址
        private string supplierAddress;

        /// <summary>
        /// 供应商地址
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string SupplierAddress
        {
            get { return supplierAddress; }
            set { Set(ref supplierAddress, value); }
        }
        #endregion

        #region 供应商邮箱
        private string supplierEmail;

        /// <summary>
        /// 供应商邮箱
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string SupplierEmail
        {
            get { return supplierEmail; }
            set { Set(ref supplierEmail, value); }
        }
        #endregion


        #region 联系人
        private string contact;

        /// <summary>
        /// 联系人
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Contact
        {
            get { return contact; }
            set { Set(ref contact, value); }
        }
        #endregion


        #region 描述
        private string description;

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }
        #endregion


        #region 是否启用
        private bool isUse;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse
        {
            get { return isUse; }
            set { Set(ref isUse, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度小于200个字符")]
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

        protected override void Disposing()
        {
            SupplierCode = null;
            SupplierName = null;
            Remark = null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
