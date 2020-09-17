using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.PlanOrderManage.Model.PlanEnumModel;

namespace Solution.Desktop.PlanWorkListManage.Model
{
    /// <summary>
    /// 工序需求模型
    /// </summary>
    public class PlanProductionProcessRequirementInfoModel : ModelBase, IAudited
    {
        #region 工序需求Id
        private Guid id;

        /// <summary>
        /// 工序需求Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion
        #region 计划Id
        private Guid productionSchedule_Id;

        /// <summary>
        /// 计划Id
        /// </summary>

        public Guid ProductionSchedule_Id
        {
            get { return productionSchedule_Id; }
            set { Set(ref productionSchedule_Id, value); }
        }

        #endregion
        #region 计划名称
        private string scheduleName;

        /// <summary>
        /// 计划名称
        /// </summary>
        public string ScheduleName
        {
            get { return scheduleName; }
            set { Set(ref scheduleName, value); }
        }

        #endregion

        #region 计划编号
        private string scheduleCode;

        /// <summary>
        /// 计划编号
        /// </summary>

        public string ScheduleCode
        {
            get { return scheduleCode; }
            set { Set(ref scheduleCode, value); }
        }

        #endregion
      
        #region 工序Id
        private Guid productionProcess_Id;

        /// <summary>
        /// 工序Id
        /// </summary>

        public Guid ProductionProcess_Id
        {
            get { return productionProcess_Id; }
            set { Set(ref productionProcess_Id, value); }
        }

        #endregion
        #region 工序名称
        private string productionProcessName;

        /// <summary>
        /// 工序名称
        /// </summary>
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

        public string ProductionProcessCode
        {
            get { return productionProcessCode; }
            set { Set(ref productionProcessCode, value); }
        }

        #endregion
        #region 配方名称
        private string productionRuleName;

        /// <summary>
        /// 配方名称
        /// </summary>
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

        public string ProductionRuleVersion
        {
            get { return productionRuleVersion; }
            set { Set(ref productionRuleVersion, value); }
        }

        #endregion
      
        #region 工序时长
        private decimal? duration;

        /// <summary>
        /// 工序时长
        /// </summary>
        public decimal? Duration
        {
            get { return duration; }
            set { Set(ref duration, value); }
        }
        #endregion
        #region 工序时长
        private DurationUnit durationUnit;

        /// <summary>
        /// 工序时长
        /// </summary>
        public DurationUnit DurationUnit
        {
            get { return durationUnit; }
            set { Set(ref durationUnit, value); }
        }
        #endregion
        #region 工序排序
        private int? productionProcessOrder;

        /// <summary>
        /// 工序排序
        /// </summary>
        public int? ProductionProcessOrder
        {
            get { return productionProcessOrder; }
            set { Set(ref productionProcessOrder, value); }
        }
        #endregion
        #region 计划开始时间
        private DateTime? startTime;

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? StartTime
        {
            get { return startTime; }
            set { Set(ref startTime, value); }
        }
        #endregion
        #region 计划结束时间
        private DateTime? endTime;

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? EndTime
        {
            get { return endTime; }
            set { Set(ref endTime, value); }
        }
        #endregion
        #region 实际开始时间
        private DateTime? actualStartTime;

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualStartTime
        {
            get { return actualStartTime; }
            set { Set(ref actualStartTime, value); }
        }
        #endregion
        #region 实际结束时间
        private DateTime? actualFinishTime;

        /// <summary>
        /// 实际结束时间
        /// </summary>
        public DateTime? ActualFinishTime
        {
            get { return actualFinishTime; }
            set { Set(ref actualFinishTime, value); }
        }
        #endregion
      
        #region 描述
        private string description;

        /// <summary>
        /// 描述
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
            ProductionProcessCode = null;
            ProductionProcessName = null;
            ProductionRuleName = null;
            ProductionRuleVersion = null;
            ScheduleCode = null;
            ScheduleName = null;
            Duration = null;
            ProductionProcessOrder = null;
            StartTime = null;
            EndTime = null;
            ActualFinishTime = null;
            ActualFinishTime = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
