using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Models;

namespace Solution.StoredInWarehouseManagement.Dtos
{
    /// <summary>
    /// 物料批次信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class MaterialBatchInfoOutputDto : IOutputDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public MaterialInfo Material { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public MatSupplierInfo MatSupplier { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public Guid MatWareHouseLocation_Id { get; set; }

        public MatWareHouseLocationInfo MatWareHouseLocation { get; set; }

        /// <summary>
        /// 入库ID
        /// </summary>
        public MaterialInStorageInfo MaterialInStorage { get; set; }

        /// <summary>
        /// 批次编号
        /// </summary>
        [StringLength(50)]
        public string BatchCode { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Quantity { get; set; }

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
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
