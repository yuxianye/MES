using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.DisStepActionInfo.Model.SystemTypeEnumModel;


namespace Solution.Desktop.DisStepActionInfo.Model
{
    /// <summary>
    /// 盘点信息模型
    /// </summary>
    public class DisStepActionInfoModel : ModelBase, IAudited
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

        #region 分步操作编号
        private string stepactioncode;

        /// <summary>
        /// 分步操作编号
        /// </summary>

        public string StepActionCode
        {
            get { return stepactioncode; }
            set { Set(ref stepactioncode, value); }
        }

        #endregion

        #region 分步操作名称
        private string stepactionname;

        /// <summary>
        /// 分步操作名称
        /// </summary>
        public string StepActionName
        {
            get { return stepactionname; }
            set { Set(ref stepactionname, value); }
        }
        #endregion

        #region 描述
        private string description;

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度大于200个字符")]
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
        [MaxLength(200, ErrorMessage = "长度大于200个字符")]
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
        }
        #endregion

        #region 创建时间
        private DateTime createdTime;

        /// <summary>
        /// 创建时间
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
            StepActionCode = null;
            StepActionName = null;
            Remark = null;
            description = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
