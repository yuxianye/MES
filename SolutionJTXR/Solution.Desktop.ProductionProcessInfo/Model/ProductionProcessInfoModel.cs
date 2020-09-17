using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.ProductionProcessInfo.Model
{
    /// <summary>
    /// 工序模型
    /// </summary>
    public class ProductionProcessInfoModel : ModelBase, IAudited
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

        #region 工序名称
        private string productionProcessName;

        /// <summary>
        /// 工序名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string ProductionProcessName
        {
            get { return productionProcessName; }
            set { Set(ref productionProcessName, value); }
        }

        #endregion

        #region 工序编号
        private string productionProcessCode;

        /// <summary>
        /// 工序编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string ProductionProcessCode
        {
            get { return productionProcessCode; }
            set { Set(ref productionProcessCode, value); }
        }

        #endregion
        #region 企业ID
        private Guid enterpriseId;
        public Guid Enterprise_Id
        {
            get { return enterpriseId; }
            set { Set(ref enterpriseId, value); }
        }

        #endregion

        #region 企业名称
        private string enterpriseName;
        public string EnterpriseName
        {
            get { return enterpriseName; }
            set { Set(ref enterpriseName, value); }
        }

        #endregion
        #region 厂区ID
        private Guid entsiteId;
        public Guid EntSite_Id
        {
            get { return entsiteId; }
            set { Set(ref entsiteId, value); }
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
        #region 区域ID
        private Guid entareaId;
        public Guid EntArea_Id
        {
            get { return entareaId; }
            set { Set(ref entareaId, value); }
        }

        #endregion
        #region 区域名称
        private string areaName;

        public string AreaName
        {
            get { return areaName; }
            set { Set(ref areaName, value); }
        }

        #endregion
        #region 生产线Id
        private Guid entProductionLine_Id;

        /// <summary>
        /// 生产线Id
        /// </summary>
        public Guid EntProductionLine_Id
        {
            get { return entProductionLine_Id; }
            set { Set(ref entProductionLine_Id, value); }
        }
        #endregion

        #region 生产线名称
        private string productionLineName;

        /// <summary>
        /// 生产线名称
        /// </summary>
        public string ProductionLineName
        {
            get { return productionLineName; }
            set { Set(ref productionLineName, value); }
        }

        #endregion

        #region 生产线编号
        private string productionLineCode;

        /// <summary>
        /// 生产线编号
        /// </summary>
        [Required(ErrorMessage = "工序编号必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string ProductionLineCode
        {
            get { return productionLineCode; }
            set { Set(ref productionLineCode, value); }
        }
        #endregion
        #region 工序描述
        private string description;

        /// <summary>
        /// 工序描述
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
        #region 是否被选中
        private bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                //Set(ref _isChecked, value);
                _isChecked = value;
                if (PropertyChangedHandler != null)
                    PropertyChangedHandler(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }
        #endregion
        protected override void Disposing()
        {
            ProductionLineCode = null;
            ProductionLineName = null;
            ProductionProcessName = null;
            ProductionProcessCode = null;
            EntProductionLine_Id = Guid.Empty;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
