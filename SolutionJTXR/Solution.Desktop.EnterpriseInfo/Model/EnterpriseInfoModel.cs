using Solution.Desktop.Core;
using Solution.Utility;
using Solution.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Solution.Desktop.EnterpriseInfo.Model
{
    /// <summary>
    /// 企业模型
    /// </summary>
    public class EnterpriseModel : ModelBase, /*ILockable, IRecyclable,*/ IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// Id
        /// </summary>
        [DisplayName("编号")]
        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 企业编号
        private string enterpriseCode;

        /// <summary>
        /// 企业编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        [DisplayName("企业编号")]
        public string EnterpriseCode
        {
            get { return enterpriseCode; }
            set { Set(ref enterpriseCode, value); }
        }
        #endregion

        #region 企业名称
        private string enterpriseName;

        /// <summary>
        /// 企业名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        [DisplayName("企业名称")]
        public string EnterpriseName
        {
            get { return enterpriseName; }
            set { Set(ref enterpriseName, value); }
        }
        #endregion

        #region 企业地址
        private string enterpriseAddress;

        /// <summary>
        /// 企业地址
        /// </summary>
        [DisplayName("企业地址")]
        public string EnterpriseAddress
        {
            get { return enterpriseAddress; }
            set { Set(ref enterpriseAddress, value); }
        }
        #endregion

        #region 企业电话
        private string enterprisePhone;

        /// <summary>
        /// 企业电话
        /// </summary>
        [RegularExpression(@"^[1][3,4,5,6,7,8,9][0-9]{9}$", ErrorMessage = "无效的电话号码")]
        public string EnterprisePhone
        {
            get { return enterprisePhone; }
            set { Set(ref enterprisePhone, value); }
        }
        #endregion

        #region 企业描述
        private string description;

        /// <summary>
        /// 企业描述
        /// </summary>
        [DisplayName("企业描述")]
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
        }
        #endregion

        //#region 是否锁定
        //private bool isLocked;

        ///// <summary>
        ///// 是否锁定
        ///// </summary>
        //public bool IsLocked
        //{
        //    get { return isLocked; }
        //    set { Set(ref isLocked, value); }
        //}
        //#endregion

        //#region 是否逻辑删除
        //private bool isDeleted;

        ///// <summary>
        ///// 是否逻辑删除
        ///// </summary>
        //public bool IsDeleted
        //{
        //    get { return isDeleted; }
        //    set { Set(ref isDeleted, value); }
        //}
        //#endregion

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
        [DisplayName("创建者编号")]
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
        [DisplayName("最后更新者编号")]
        public string LastUpdatorUserId
        {
            get { return lastUpdatorUserId; }
            set { Set(ref lastUpdatorUserId, value); }
        }
        #endregion

        protected override void Disposing()
        {
            EnterpriseCode = null;
            EnterpriseName = null;
            EnterpriseAddress = null;
            EnterprisePhone = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
