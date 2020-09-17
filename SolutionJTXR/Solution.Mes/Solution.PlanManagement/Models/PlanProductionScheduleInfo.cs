﻿using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Solution.ProductManagement.Models;

namespace Solution.PlanManagement.Models
{

    [Description("生产计划信息")]
    public class PlanProductionScheduleInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 生产计划名称
        /// </summary>
        [StringLength(100)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string ScheduleName { get; set; }

        /// <summary>
        /// 生产计划编号
        /// </summary>
        [StringLength(100)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string ScheduleCode { get; set; }

        /// <summary>
        /// 订单明细ID
        /// </summary>
        public virtual PlanOrderItemInfo OrderItem { get; set; }

        /// <summary>
        /// 配方ID
        /// </summary>
        public virtual ProductionRuleInfo ProductionRule { get; set; }

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
        [StringLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
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
        [StringLength(50)]
        public string CreatorUserId { get; set; }

        #endregion

        #region Implementation of IUpdateAutited

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion

    }
}
