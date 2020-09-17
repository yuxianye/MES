using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using Solution.ProductManagement.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.StepTeachingDispatchManagement.Models
{
    [Description("分步操作与工序关联信息")]
    public class DisStepActionProcessMapInfo : EntityBase<Guid>, IAudited
    {

        /// <summary>
        /// 分步操作信息
        /// </summary>
        public virtual DisStepActionInfo DisStepActionInfo { get; set; }
        /// <summary>
        /// 工序信息
        /// </summary>
        public virtual ProductionProcessInfo ProductionProcessInfo { get; set; }

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
