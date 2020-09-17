using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.StepTeachingDispatchManagement.Models;
using Solution.ProductManagement.Models;
using System.Collections.Generic;

namespace Solution.StepTeachingDispatchManagement.Dtos
{
    /// <summary>
    /// 分步操作与工序关联信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class DisStepActionProcessMapInfoInputDto : IInputDto<Guid>, IAudited
    {

        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public Guid ProductionProcessInfo_ID { get; set; }

        /// <summary>
        /// 工序信息
        /// </summary>
        public virtual ProductionProcessInfo ProductionProcessInfo { get; set; }
        /// <summary>
        /// 分步操作信息ID
        /// </summary>
        public Guid DisStepActionInfo_ID { get; set; }
        /// <summary>
        /// 分步操作信息
        /// </summary>
        public virtual DisStepActionInfo DisStepActionInfo { get; set; }

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
