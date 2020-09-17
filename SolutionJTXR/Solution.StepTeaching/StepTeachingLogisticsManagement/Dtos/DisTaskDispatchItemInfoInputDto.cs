using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using Solution.StepTeachingDispatchManagement.Models;
using Solution.EquipmentManagement.Models;

namespace Solution.StepTeachingDispatchManagement.Dtos
{
    /// <summary>
    /// 分步教学任务调度明细信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class DisTaskDispatchItemInfoInputDto : IInputDto<Guid>, IAudited
    {

        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid DisStepTeachingTaskDispatch_Id { get; set; }

        public DisTaskDispatchInfo DisStepTeachingTaskDispatch { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public Guid Equipment_Id { get; set; }

        public EquEquipmentInfo Equipment { get; set; }

        /// <summary>
        /// 设备动作ID
        /// </summary>
        public Guid EquipmentAction_Id { get; set; }

        public EquFactoryInfo EquipmentAction { get; set; }

        /// <summary>
        /// 设备动作顺序
        /// </summary>
        public int ActionOrder { get; set; }

        /// <summary>
        /// 任务明细编号，不能重复
        /// </summary>
        public string TaskItemCode { get; set; }

        /// <summary>
        /// 任务结果
        /// </summary>
        public string TaskItemResult { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }

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
