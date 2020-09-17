using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StereoscopicWarehouseManagement.Models;
using Solution.StoredInWarehouseManagement.Models;

namespace Solution.StereoscopicWarehouseManagement.Dtos
{
    /// <summary>
    /// 盘点明细信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class MatInventoryItemInfoInputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 盘点ID
        /// </summary>
        public Guid MatInventory_Id { get; set; }
        public virtual MatInventoryInfo MatInventory { get; set; }

        /// <summary>
        /// 库位ID
        /// </summary>
        public Guid MatWareHouseLocation_Id { get; set; }
        public MatWareHouseLocationInfo MatWareHouseLocation { get; set; }

        /// <summary>
        /// 物料批次ID
        /// </summary>
        public Guid MaterialBatch_Id { get; set; }
        public MaterialBatchInfo MaterialBatch { get; set; }

        /// <summary>
        /// 满盘数量
        /// </summary>
        public int FullPalletQuantity { get; set; }

        /// <summary>
        /// 账面数量
        /// </summary>
        public decimal? AccuntAmount { get; set; }

        /// <summary>
        /// 实际数量
        /// </summary>
        public decimal? ActualAmount { get; set; }

        /// <summary>
        /// 差异数量=账面数量-实际数量
        /// </summary>
        public decimal? DifferenceAmount { get; set; }

        /// <summary>
        /// 盘点时间
        /// </summary>
        public DateTime? InventoryTime { get; set; }

        /// <summary>
        /// 盘点状态
        /// </summary>
        public int? InventoryState { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 当前系统登录用户
        /// </summary>
        public string UserName { get; set; }

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
