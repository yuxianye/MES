using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.ProductManagement.Models;
using Solution.EquipmentManagement.Models;
using Solution.PlanManagement.Models;

namespace Solution.PlanManagement.Dtos
{
    public class PlanEquipmentRequirementInfoInputDto : IInputDto<Guid>, IAudited
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 工序请求ID
        /// </summary>
        public Guid ProductionProcessRequirement_Id { get; set; }


        /// <summary>
        /// 设备ID
        /// </summary>
        public Guid Equipment_Id { get; set; }
        /// <summary>
        /// 工序请求
        /// </summary>
        public PlanProductionProcessRequirementInfo ProductionProcessRequirement { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public EquEquipmentInfo Equipment { get; set; }


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
