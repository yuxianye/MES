using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using Solution.StepTeachingDispatchManagement.Models;

namespace Solution.StepTeachingDispatchManagement.Dtos
{
    /// <summary>
    /// 分步教学任务调度主信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class DisTaskDispatchInfoInputDto : IInputDto<Guid>, IAudited
    {

        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 任务编号，不能重复
        /// </summary>
        public string TaskCode { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public DisStepActionInfo DisStepAction { get; set; }


        /// <summary>
        /// 任务结果
        /// </summary>
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
