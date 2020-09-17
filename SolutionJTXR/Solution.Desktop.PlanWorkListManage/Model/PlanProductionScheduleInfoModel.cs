using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.PlanOrderManage.Model.PlanEnumModel;

namespace Solution.Desktop.PlanWorkListManage.Model
{
    /// <summary>
    /// 计划模型
    /// </summary>
    public class PlanProductionScheduleInfoModel : ModelBase, IAudited
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
        #region 产品Id
        private Guid product_Id;

        /// <summary>
        /// 产品Id
        /// </summary>

        public Guid Product_Id
        {
            get { return product_Id; }
            set { Set(ref product_Id, value); }
        }

        #endregion
        #region 产品名称
        private string productName;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { Set(ref productName, value); }
        }

        #endregion

        #region 产品编号
        private string productCode;

        /// <summary>
        /// 产品编号
        /// </summary>

        public string ProductCode
        {
            get { return productCode; }
            set { Set(ref productCode, value); }
        }

        #endregion
        #region 订单明细Id
        private Guid orderItem_Id;

        /// <summary>
        /// 订单明细Id
        /// </summary>

        public Guid OrderItem_Id
        {
            get { return orderItem_Id; }
            set { Set(ref orderItem_Id, value); }
        }

        #endregion
        #region 订单明细名称
        private string orderItemName;

        /// <summary>
        /// 订单明细名称
        /// </summary>
        public string OrderItemName
        {
            get { return orderItemName; }
            set { Set(ref orderItemName, value); }
        }

        #endregion

        #region 订单明细编号
        private string orderItemCode;

        /// <summary>
        /// 订单明细编号
        /// </summary>

        public string OrderItemCode
        {
            get { return orderItemCode; }
            set { Set(ref orderItemCode, value); }
        }

        #endregion
        #region 订单名称
        private string orderName;

        /// <summary>
        /// 订单名称
        /// </summary>
        public string OrderName
        {
            get { return orderName; }
            set { Set(ref orderName, value); }
        }

        #endregion

        #region 订单编号
        private string orderCode;

        /// <summary>
        /// 订单编号
        /// </summary>

        public string OrderCode
        {
            get { return orderCode; }
            set { Set(ref orderCode, value); }
        }

        #endregion
        #region 配方Id
        private Guid productionRule_Id;

        /// <summary>
        /// 配方Id
        /// </summary>

        public Guid ProductionRule_Id
        {
            get { return productionRule_Id; }
            set { Set(ref productionRule_Id, value); }
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
        #region 计划名称
        private string scheduleName;

        /// <summary>
        /// 计划名称
        /// </summary>
        [Required(ErrorMessage = "计划名称必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string ScheduleName
        {
            get { return scheduleName; }
            set { Set(ref scheduleName, value); }
        }

        #endregion

        #region 计划编号
        private string scheduleCode;

        /// <summary>
        /// 产品编号
        /// </summary>
        [Required(ErrorMessage = "计划编号必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string ScheduleCode
        {
            get { return scheduleCode; }
            set { Set(ref scheduleCode, value); }
        }

        #endregion

        #region 计划生产数量
        private string quantity;

        /// <summary>
        /// 计划生产数量
        /// </summary>
        public string Quantity
        {
            get { return quantity; }
            set { Set(ref quantity, value); }
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
        #region 发布时间
        private DateTime? publishedDate;

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublishedDate
        {
            get { return publishedDate; }
            set { Set(ref publishedDate, value); }
        }
        #endregion
        #region 优先级
        private Priority priority;

        /// <summary>
        /// 优先级
        /// </summary>
        public Priority Priority
        {
            get { return priority; }
            set { Set(ref priority, value); }
        }
        #endregion
        #region 是否下达计划
        private bool? isPublish;

        /// <summary>
        /// 是否下达计划
        /// </summary>
        public bool? IsPublish
        {
            get { return isPublish; }
            set { Set(ref isPublish, value); }
        }
        #endregion
        #region 计划状态
        private ScheduleStatus scheduleStatus;

        /// <summary>
        /// 计划状态
        /// </summary>
        public ScheduleStatus ScheduleStatus
        {
            get { return scheduleStatus; }
            set { Set(ref scheduleStatus, value); }
        }
        #endregion
        #region 已完成数量
        private decimal? finishQuantity;

        /// <summary>
        /// 已完成数量
        /// </summary>
        public decimal? FinishQuantity
        {
            get { return finishQuantity; }
            set { Set(ref finishQuantity, value); }
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
            OrderCode = null;
            OrderName = null;
            OrderItemCode = null;
            OrderItemName = null;
            ScheduleCode = null;
            ScheduleName = null;
            Quantity = null;
            FinishQuantity = null;
            StartTime = null;
            EndTime = null;
            ActualFinishTime = null;
            ActualFinishTime = null;
            ProductionRuleName = null;
            ProductionRuleVersion = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
