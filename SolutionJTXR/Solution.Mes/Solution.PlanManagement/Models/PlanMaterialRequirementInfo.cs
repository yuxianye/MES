using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Solution.ProductManagement.Models;
using Solution.MatWarehouseStorageManagement.Models;

namespace Solution.PlanManagement.Models
{
    [Description("工序材料需求信息")]
    public class PlanMaterialRequirementInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 工序请求ID
        /// </summary>
        public virtual PlanProductionProcessRequirementInfo ProductionProcessRequirement { get; set; }


        /// <summary>
        /// 材料ID
        /// </summary>
        public virtual MaterialInfo Material { get; set; }

        /// <summary>
        /// 需要数量
        /// </summary>

        public decimal? RequireQuantity { get; set; }

        /// <summary>
        /// 实际数量
        /// </summary>

        public decimal? ActualQuantity { get; set; }


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
