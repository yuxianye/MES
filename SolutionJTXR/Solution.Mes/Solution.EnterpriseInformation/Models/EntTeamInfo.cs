using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.EnterpriseInformation.Models
{
    [Description("班组信息")]
    public class EntTeamInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 班组编号，不能重复
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string TeamCode { get; set; }

        /// <summary>
        /// 班组名称，不能重复
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string TeamName { get; set; }

        /// <summary>
        /// 人员个数
        /// </summary>
        public int? TeamNumber { get; set; }

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
