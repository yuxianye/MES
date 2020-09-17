using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.CommunicationModule.Models
{
    /// <summary>
    /// 实体类——业务点表
    /// </summary>
    [Description("业务点表")]
    public class BusinessNode : EntityBase<Guid>, IAudited
    {
        [Display(Description = "业务名称")]
        [Required, StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string BusinessName { get; set; }

        [Display(Description = "描述")]
        [StringLength(200)]
        public string Description { get; set; }

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
