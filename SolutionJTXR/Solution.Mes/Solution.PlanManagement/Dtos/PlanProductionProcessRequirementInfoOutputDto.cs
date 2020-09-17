using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.ProductManagement.Models;
using Solution.PlanManagement.Models;

namespace Solution.PlanManagement.Dtos
{
    public class PlanProductionProcessRequirementInfoOutputDto : IOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 计划ID
        /// </summary>
        public Guid ProductionSchedule_Id { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public Guid ProductionProcess_Id { get; set; }
        /// <summary>
        /// 计划
        /// </summary>
        public PlanProductionScheduleInfo ProductionSchedule { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public ProductionProcessInfo ProductionProcess { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 实际开始生产时间
        /// </summary>
        public DateTime? ActualStartTime { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualFinishTime { get; set; }

        /// <summary>
        /// 工序时长
        /// </summary>
        public decimal? Duration { get; set; }

        /// <summary>
        /// 时长单位
        /// </summary>
        public int? DurationUnit { get; set; }


        /// <summary>
        /// 工序排序
        /// </summary>
        public int? ProductionProcessOrder { get; set; }

        /// <summary>
        /// 描述
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
