using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Solution.Desktop.ProductionProcessInfo.Model;

namespace Solution.Desktop.DisStepActionInfo.Model
{
    /// <summary>
    /// 分步操作与工序关联数据模型
    /// </summary>
    public class DisStepActionProcessMapModel : ModelBase, IAudited
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

        #region 工序ID

        private Guid? productionProcessInfo_ID;

        public Guid? ProductionProcessInfo_ID
        {
            get { return productionProcessInfo_ID; }
            set { Set(ref productionProcessInfo_ID, value); }
        }
        #endregion

        #region 工序列表
        private ObservableCollection<ProductionProcessInfoModel> productionProcessInfoList = new ObservableCollection<ProductionProcessInfoModel>();
        public ObservableCollection<ProductionProcessInfoModel> ProductionProcessInfoList
        {
            get { return productionProcessInfoList; }
            set { Set(ref productionProcessInfoList, value); }
        }
        #endregion

        #region 分步操作ID

        private Guid? disStepActionInfo_ID;

        public Guid? DisStepActionInfo_ID
        {
            get { return disStepActionInfo_ID; }
            set { Set(ref disStepActionInfo_ID, value); }
        }
        #endregion

        #region 分步操作

        private DisStepActionInfoModel disStepActionInfo;

        public DisStepActionInfoModel DisStepActionInfo
        {
            get { return disStepActionInfo; }
            set { Set(ref disStepActionInfo, value); }
        }
        #endregion

        #region 工序

        private ProductionProcessInfoModel productionProcessInfo;

        public ProductionProcessInfoModel ProductionProcessInfo
        {
            get { return productionProcessInfo; }
            set { Set(ref productionProcessInfo, value); }
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
            ProductionProcessInfo_ID = Guid.Empty;
            DisStepActionInfo_ID = Guid.Empty;
        }

    }

}
