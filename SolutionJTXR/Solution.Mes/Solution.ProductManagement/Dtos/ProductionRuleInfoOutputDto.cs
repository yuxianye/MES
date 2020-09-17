using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.ProductManagement.Dtos
{
    class ProductionRuleInfoOutputDto : IOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        /// 
        public Guid Product_Id { get; set; }


        /// <summary>
        /// 配方名称
        /// </summary>
        public string ProductionRuleName { get; set; }

        /// <summary>
        /// 配方版本号
        /// </summary>
        public string ProductionRuleVersion { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? ApprovalDate { get; set; }


        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? EffectiveDate { get; set; }


        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        /// 
        public string ApprovedBy { get; set; }

        /// <summary>
        /// 配方状态(未审核,应用于生产,应用于测试,已废弃)
        /// </summary>
        public Guid ProductionRuleStatus_Id { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public decimal? Duration { get; set; }

        /// <summary>
        /// 时长单位（1:小时,2:分钟,3:秒）
        /// </summary>
        public int? DurationUnit { get; set; }

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
