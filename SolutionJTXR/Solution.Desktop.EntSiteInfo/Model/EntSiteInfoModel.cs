using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EntSiteInfo.Model
{
    /// <summary>
    /// 厂区模型
    /// </summary>
    public class EntSiteInfoModel : ModelBase, IAudited
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
        #region 企业ID
        private Guid _enterpriseId;
        public Guid Enterprise_Id
        {
            get { return _enterpriseId; }
            set { Set(ref _enterpriseId, value); }
        }

        #endregion

        #region 企业名称
        private string _enterpriseName;
        public string EnterpriseName
        {
            get { return _enterpriseName; }
            set { Set(ref _enterpriseName, value); }
        }

        #endregion
        #region 厂区名称
        private string siteName;

        /// <summary>
        /// 厂区名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string SiteName
        {
            get { return siteName; }
            set { Set(ref siteName, value); }
        }

        #endregion

        #region 厂区编号
        private string siteCode;

        /// <summary>
        /// 厂区编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string SiteCode
        {
            get { return siteCode; }
            set { Set(ref siteCode, value); }
        }

        #endregion

        #region 厂区负责人
        private string siteManager;

        /// <summary>
        /// 厂区负责人
        /// </summary>
        public string SiteManager
        {
            get { return siteManager; }
            set { Set(ref siteManager, value); }
        }
        #endregion

        #region 厂区电话
        private string sitePhone;

        /// <summary>
        /// 厂区电话
        /// </summary>
        [RegularExpression(@"^[1][3,4,5,6,7,8,9][0-9]{9}$", ErrorMessage = "无效的电话号码")]
        public string SitePhone
        {
            get { return sitePhone; }
            set { Set(ref sitePhone, value); }
        }
        #endregion

        #region 厂区描述
        private string description;

        /// <summary>
        /// 厂区描述
        /// </summary>
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

        protected override void Disposing()
        {
            SiteCode = null;
            SiteName = null;
            EnterpriseName = null;
            SiteManager = null;
            SitePhone = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }

}
