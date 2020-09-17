using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.StepTeachingDispatchManagement.Models
{
    /// <summary>
    /// 分步教学任务调度主信息
    /// </summary>
    [Description("分步教学任务调度主信息")]
    public class DisTaskDispatchInfo : EntityBase<Guid>, IAudited
    {
        public DisTaskDispatchInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 任务编号，不能重复
        /// </summary>
        [StringLength(100)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string TaskCode { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public virtual DisStepActionInfo DisStepAction { get; set; }

        /// <summary>
        /// 任务结果
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string TaskResult { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// 入库ID
        /// </summary>
        public Guid? InStorage_Id { get; set; }
        /// <summary>
        /// 出库ID
        /// </summary>
        public Guid? OutStorage_Id { get; set; }
        /// <summary>
        /// 移库ID 
        /// </summary>
        public Guid? MoveStorage_Id { get; set; }

        /// <summary>
        /// 描述
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
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
