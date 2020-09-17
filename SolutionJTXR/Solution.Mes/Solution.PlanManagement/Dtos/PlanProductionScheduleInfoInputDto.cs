using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.ProductManagement.Models;
using Solution.PlanManagement.Models;

namespace Solution.PlanManagement.Dtos
{
    public class PlanProductionScheduleInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 生产计划名称
        /// </summary>
        public string ScheduleName { get; set; }

        /// <summary>
        /// 生产计划编号
        /// </summary>
        public string ScheduleCode { get; set; }

        /// <summary>
        /// 订单明细ID
        /// </summary>
        public Guid OrderItem_Id { get; set; }

        /// <summary>
        /// 配方ID
        /// </summary>
        public Guid ProductionRule_Id { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public PlanOrderItemInfo OrderItem { get; set; }

        /// <summary>
        /// 配方
        /// </summary>
        public ProductionRuleInfo ProductionRule { get; set; }

        /// <summary>
        /// 生产计划状态
        /// </summary>
        public int? ScheduleStatus { get; set; }

        /// <summary>
        /// 计划生产数量
        /// </summary>
        public decimal? Quantity { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// 是否下达计划
        /// </summary>

        public bool? IsPublish { get; set; }

        /// <summary>
        ///发布时间
        /// </summary>
        public DateTime? PublishedDate { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualStartTime { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualFinishTime { get; set; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public decimal? FinishQuantity { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        #endregion

        #region Implementation of ICreatedAudited

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        public string CreatorUserId { get; set; }

        #endregion

        #region Implementation of IUpdateAutited

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
