using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.ProductManagement.Models
{
    [Description("配方信息")]
    public class ProductionRuleInfo : EntityBase<Guid>, IAudited/*, IRecyclable, ILockable*/
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        /// 
        public virtual ProductInfo Product { get; set; }


        /// <summary>
        /// 配方名称
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string ProductionRuleName { get; set; }

        /// <summary>
        /// 配方版本号
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
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

        ///// <summary>
        ///// 作者 （有创建者字段，不需要该字段）
        ///// </summary>
        //public Guid? Author { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        /// 
        [StringLength(50)]
        public string ApprovedBy { get; set; }

        /// <summary>
        /// 配方状态(未审核,应用于生产,应用于测试,已废弃)
        /// </summary>
        public virtual ProductionRuleStatusInfo ProductionRuleStatus { get; set; }

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
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        //#region Implementation of ILockable
        ///// <summary>
        ///// 是否锁定
        ///// </summary>
        //public bool IsLocked { get; set; }
        //#endregion

        //#region Implementation of IRecyclable
        ///// <summary>
        ///// 是否逻辑删除
        ///// </summary>
        //public bool IsDeleted { get; set; }
        //#endregion
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
