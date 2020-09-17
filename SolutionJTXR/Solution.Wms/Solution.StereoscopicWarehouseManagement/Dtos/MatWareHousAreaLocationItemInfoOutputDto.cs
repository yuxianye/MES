using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.MatWarehouseStorageManagement.Models;


namespace Solution.StereoscopicWarehouseManagement.Dtos
{
    /// <summary>
    /// 仓位图模型
    /// </summary>
    public class MatWareHousAreaLocationItemInfoOutputDto : IInputDto<Guid>, IAudited
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// 托盘编号
        /// </summary>
        public string PalletCode { get; set; }


        /// <summary>
        /// 托盘名称
        /// </summary>
        public string PalletName { get; set; }


        /// <summary>
        /// 库位编号
        /// </summary>
        public string WareHouseLocationCode { get; set; }


        /// <summary>
        /// 库位名称
        /// </summary>
        public string WareHouseLocationName { get; set; }


        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialCode { get; set; }


        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }


        /// <summary>
        /// 物料单位
        /// </summary>
        public int MaterialUnit { get; set; }


        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal? FullPalletQuantity { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        public string InStorageBillCode { get; set; }


        /// <summary>
        /// 入库单号
        /// </summary>
        public string BatchCode { get; set; }


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
