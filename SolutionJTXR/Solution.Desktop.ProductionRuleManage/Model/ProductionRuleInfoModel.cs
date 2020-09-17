using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.ProductionRuleManage.Model
{
    /// <summary>
    /// 配方模型
    /// </summary>
    public class ProductionRuleInfoModel : ModelBase, IAudited
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
        #region 配方ID
        private Guid product_Id;
        public Guid Product_Id
        {
            get { return product_Id; }
            set { Set(ref product_Id, value); }
        }

        #endregion
        #region 配方名称
        private string productName;

        /// <summary>
        /// 配方名称
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { Set(ref productName, value); }
        }

        #endregion

        #region 配方编号
        private string productCode;

        /// <summary>
        /// 配方编号
        /// </summary>

        public string ProductCode
        {
            get { return productCode; }
            set { Set(ref productCode, value); }
        }

        #endregion
        #region 配方名称
        private string productionRuleName;

        /// <summary>
        /// 配方名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string ProductionRuleName
        {
            get { return productionRuleName; }
            set { Set(ref productionRuleName, value); }
        }

        #endregion

        #region 配方版本号
        private string productionRuleVersion;

        /// <summary>
        /// 配方版本号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string ProductionRuleVersion
        {
            get { return productionRuleVersion; }
            set { Set(ref productionRuleVersion, value); }
        }

        #endregion

        #region 审核时间
        private DateTime? approvalDate;

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ApprovalDate
        {
            get { return approvalDate; }
            set { Set(ref approvalDate, value); }
        }
        #endregion
        #region 生效时间
        private DateTime? effectiveDate;

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? EffectiveDate
        {
            get { return effectiveDate; }
            set { Set(ref effectiveDate, value); }
        }
        #endregion
        #region 过期时间
        private DateTime? expirationDate;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpirationDate
        {
            get { return expirationDate; }
            set { Set(ref expirationDate, value); }
        }
        #endregion

        #region 审核人
        private string approvedBy;

        /// <summary>
        /// 审核人
        /// </summary>
        public string ApprovedBy
        {
            get { return approvedBy; }
            set { Set(ref approvedBy, value); }
        }
        #endregion

        #region 配方状态ID
        private Guid productionRuleStatus_Id;
        public Guid ProductionRuleStatus_Id
        {
            get { return productionRuleStatus_Id; }
            set { Set(ref productionRuleStatus_Id, value); }
        }

        #endregion
        #region 配方状态编号
        private string productionRuleStatusCode;

        /// <summary>
        /// 配方状态编号
        /// </summary>

        public string ProductionRuleStatusCode
        {
            get { return productionRuleStatusCode; }
            set { Set(ref productionRuleStatusCode, value); }
        }

        #endregion
        #region 配方状态名称
        private string productionRuleStatusName;

        /// <summary>
        /// 配方状态名称
        /// </summary>

        public string ProductionRuleStatusName
        {
            get { return productionRuleStatusName; }
            set { Set(ref productionRuleStatusName, value); }
        }

        #endregion
        #region 配方时长
        private decimal? duration;

        /// <summary>
        /// 配方时长
        /// </summary>
        public decimal? Duration
        {
            get { return duration; }
            set { Set(ref duration, value); }
        }
        #endregion

        #region 时长单位
        private int? durationUnit;

        /// <summary>
        /// 时长单位
        /// </summary>
        public int? DurationUnit
        {
            get { return durationUnit; }
            set { Set(ref durationUnit, value); }
        }
        #endregion
        #region 时长单位名称
        private string durationUnitName;

        /// <summary>
        /// 时长单位名称
        /// </summary>
        public string DurationUnitName
        {
            get { return durationUnitName; }
            set { Set(ref durationUnitName, value); }
        }
        #endregion

        #region 配方描述
        private string description;

        /// <summary>
        /// 配方描述
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
            ProductionRuleName = null;
            ProductionRuleVersion = null;
            ApprovalDate = null;
            ApprovedBy = null;
            ProductionRuleStatus_Id = Guid.Empty;
            Duration = null;
            DurationUnit = null;
            DurationUnitName = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
