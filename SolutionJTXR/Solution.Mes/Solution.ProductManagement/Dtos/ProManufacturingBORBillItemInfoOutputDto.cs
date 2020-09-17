using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.ProductManagement.Models;
using Solution.EquipmentManagement.Models;

namespace Solution.ProductManagement.Dtos
{
    public class ProManufacturingBORBillItemInfoOutputDto : IOutputDto
    {

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// BORID
        /// </summary>
        public Guid ProManufacturingBill_Id { get; set; }
        /// <summary>
        /// 制造清单
        /// </summary>
        public ProManufacturingBillInfo ProManufacturingBill { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public Guid ProductionProcess_Id { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public ProductionProcessInfo ProductionProcess { get; set; }
        /// <summary>
        /// 设备ID
        /// </summary>
        /// 
        public Guid Equipment_Id { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public EquEquipmentInfo Equipment { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Quantity { get; set; }

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
