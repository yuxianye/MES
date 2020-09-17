using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EntAreaInfo.Model
{
    /// <summary>
    /// 区域模型
    /// </summary>
    public class EntAreaInfoModel : ModelBase, IAudited
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
        #region 厂区ID
        private Guid _entsiteId;
        public Guid EntSite_Id
        {
            get { return _entsiteId; }
            set { Set(ref _entsiteId, value); }
        }

        #endregion
        #region 厂区名称
        private string siteName;

        public string SiteName
        {
            get { return siteName; }
            set { Set(ref siteName, value); }
        }

        #endregion
        #region 区域名称
        private string areaName;

        /// <summary>
        /// 区域名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string AreaName
        {
            get { return areaName; }
            set { Set(ref areaName, value); }
        }

        #endregion

        #region 区域编号
        private string areaCode;

        /// <summary>
        /// 区域编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string AreaCode
        {
            get { return areaCode; }
            set { Set(ref areaCode, value); }
        }

        #endregion

        #region 区域负责人
        private string areaManager;

        /// <summary>
        /// 区域负责人
        /// </summary>
        public string AreaManager
        {
            get { return areaManager; }
            set { Set(ref areaManager, value); }
        }
        #endregion

        #region 区域电话
        private string areaPhone;

        /// <summary>
        /// 区域电话
        /// </summary>
        [RegularExpression(@"^[1][3,4,5,6,7,8,9][0-9]{9}$", ErrorMessage = "无效的电话号码")]
        public string AreaPhone
        {
            get { return areaPhone; }
            set { Set(ref areaPhone, value); }
        }
        #endregion

        #region 区域描述
        private string description;

        /// <summary>
        /// 区域描述
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
            AreaCode = null;
            AreaName = null;
            EnterpriseName = null;
            SiteName = null;
            AreaManager = null;
            AreaPhone = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
