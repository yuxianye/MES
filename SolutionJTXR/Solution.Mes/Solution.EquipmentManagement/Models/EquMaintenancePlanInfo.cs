using OSharp.Core.Data;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.EquipmentManagement.Models
{
    public class EquMaintenancePlanInfo : EntityBase<Guid>, /*ILockable, IRecyclable, */IAudited
    {
        /// <summary>
        /// 设备维护计划信息
        /// </summary>
        public virtual EquEquipmentInfo EquipmentInfo { get; set; }

        /// <summary>
        /// 维护计划单号
        /// </summary>
        [StringLength(50)]
        public string MaintenanceCode { get; set; }

        /// <summary>
        /// 维护类别(保养,点检,检修)
        /// </summary>
        public MaintenanceType MaintenanceTypeInfo{get;set;}

        /// <summary>
        /// 计划制定人
        /// </summary>
        [StringLength(50)]
        public string Maker { get; set; }

        /// <summary>
        /// 计划停机时长(小时)
        /// </summary>
        public double PlanStopDuration { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime PlanFinishTime { get; set; }

        /// <summary>
        /// 维护实际开始时间
        /// </summary>
        public DateTime ActualStartTime { get; set; }

        /// <summary>
        /// 维护实际完成时间
        /// </summary>
        public DateTime ActualFinishTime { get; set; }

        /// <summary>
        /// 维护内容
        /// </summary>
        [StringLength(100)]
        public string MaintenanceContent { get; set; }

        /// <summary>
        /// 承担班组
        /// </summary>
        [StringLength(50)]
        public string UndertakeTeamID { get; set; }

        /// <summary>
        /// 结论
        /// </summary>
        [StringLength(100)]
        public string Result { get; set; }

        /// <summary>
        /// 验收部门ID
        /// </summary>
        public virtual EntDepartmentInfo CheckDepartment { get; set; }

        /// <summary>
        /// 维护人员
        /// </summary>
        [StringLength(50)]
        public string MaintenancePerson { get; set; }

        /// <summary>
        /// 维护计划状态(未开始,已完成)
        /// </summary>
        public MaintenancePlanState MaintenancePlanStateInfo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

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

        public DateTime? LastUpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
