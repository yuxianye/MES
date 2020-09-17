using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.DisTaskDispatchInfo.Model.SystemTypeEnumModel;


namespace Solution.Desktop.DisTaskDispatchInfo.Model
{
    /// <summary>
    /// 盘点信息模型
    /// </summary>
    public class DisTaskDispatchInfoModel : ModelBase, IAudited
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

        #region 任务编号
        private string taskCode;

        /// <summary>
        /// 任务编号
        /// </summary>
        [Required(ErrorMessage = "任务编号必填"), MaxLength(100, ErrorMessage = "长度大于100个字符")]
        public string TaskCode
        {
            get { return taskCode; }
            set { Set(ref taskCode, value); }
        }

        #endregion

        #region 任务结果
        private string taskResult;

        /// <summary>
        /// 任务编号
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string TaskResult
        {
            get { return taskResult; }
            set { Set(ref taskResult, value); }
        }

        #endregion

        #region 分步教学任务类别ID
        private Guid disStepAction_Id;
        public Guid DisStepAction_Id
        {
            get { return disStepAction_Id; }
            set { Set(ref disStepAction_Id, value); }
        }
        #endregion

        #region 分步教学任务类别编号
        private string disStepActioncode;

        /// <summary>
        /// 分步教学任务类别编号
        /// </summary>

        public string DisStepActionCode
        {
            get { return disStepActioncode; }
            set { Set(ref disStepActioncode, value); }
        }

        #endregion

        #region 分步教学任务类别名称
        private string disStepActionname;

        /// <summary>
        /// 分步教学任务类别名称
        /// </summary>
        public string DisStepActionName
        {
            get { return disStepActionname; }
            set { Set(ref disStepActionname, value); }
        }
        #endregion

        #region 系统类别
        private SystemType systemtype;

        /// <summary>
        /// 系统类别
        /// </summary>
        [Required(ErrorMessage = "分步教学任务类别名称必填")]
        public SystemType SystemType
        {
            get { return systemtype; }
            set { Set(ref systemtype, value); }
        }
        #endregion

        #region 完成时间
        private DateTime? finishTime;

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? FinishTime
        {
            get { return finishTime; }
            set { Set(ref finishTime, value); }
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

        #region 任务描述
        private string description;

        /// <summary>
        /// 任务描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度大于200个字符")]
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
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
            TaskCode = null;
            TaskResult = null;
            DisStepActionCode = null;
            DisStepActionName = null;
            FinishTime = null;
            Remark = null;
            description = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
