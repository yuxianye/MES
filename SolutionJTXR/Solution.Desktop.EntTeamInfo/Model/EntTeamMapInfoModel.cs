using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.Desktop.EntEmployeeInfo.Model;
using System.Collections.ObjectModel;


namespace Solution.Desktop.EntTeamInfo.Model
{
    /// <summary>
    /// 班组人员配置模型
    /// </summary>
    public class EntTeamMapInfoModel : ModelBase, IAudited
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

        #region 人员ID

        private Guid? entEmployeeInfo_Id;

        public Guid? EntEmployeeInfo_Id
        {
            get { return entEmployeeInfo_Id; }
            set { Set(ref entEmployeeInfo_Id, value); }
        }
        #endregion

        #region 人员列表
        private ObservableCollection<EntEmployeeInfoModel> entEmployeeInfoList = new ObservableCollection<EntEmployeeInfoModel>();
        public ObservableCollection<EntEmployeeInfoModel> EntEmployeeInfoList
        {
            get { return entEmployeeInfoList; }
            set { Set(ref entEmployeeInfoList, value); }
        }
        #endregion

        #region 班组ID

        private Guid? entTeamInfo_Id;

        public Guid? EntTeamInfo_Id
        {
            get { return entTeamInfo_Id; }
            set { Set(ref entTeamInfo_Id, value); }
        }
        #endregion

        #region 班组

        private EntTeamInfoModel entTeamInfo;

        public EntTeamInfoModel EntTeamInfo
        {
            get { return entTeamInfo; }
            set { Set(ref entTeamInfo, value); }
        }
        #endregion

        #region 人员

        private EntEmployeeInfoModel entEmployeeInfo;

        public EntEmployeeInfoModel EntEmployeeInfo
        {
            get { return entEmployeeInfo; }
            set { Set(ref entEmployeeInfo, value); }
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
            EntEmployeeInfo_Id = Guid.Empty;
            EntTeamInfo_Id = Guid.Empty;
            EntEmployeeInfo = null;
            EntTeamInfo = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}

