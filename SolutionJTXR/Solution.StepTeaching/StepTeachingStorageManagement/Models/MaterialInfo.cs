using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.MatWarehouseStorageManagement.Models
{
    /// <summary>
    /// 物料信息
    /// </summary>
    [Description("物料信息")]
    public class MaterialInfo : EntityBase<Guid>, IAudited
    {
        public MaterialInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 物料编号，不能重复
        /// </summary>
        [StringLength(50)]
        public string MaterialCode { get; set; }

        /// <summary>
        /// 物料名称，不能重复
        /// </summary>
        [StringLength(50)]
        public string MaterialName { get; set; }
        
        /// <summary>
        /// 物料类型
        /// </summary>
        public int? MaterialType { get; set; }


        /// <summary>
        /// 物料单位
        /// </summary>
        public int? MaterialUnit { get; set; }

        /// <summary>
        /// 满盘数量
        /// </summary>
        public int? FullPalletQuantity { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [StringLength(200)]
        public string Specification { get; set; }

        /// <summary>
        /// 单位重量
        /// </summary>
        public decimal? UnitWeight { get; set; }

        /// <summary>
        /// 当前库存
        /// </summary>
        public decimal? CurrentStock { get; set; }

        /// <summary>
        /// 最大库存
        /// </summary>
        public decimal? MaxStock { get; set; }

        /// <summary>
        /// 最小库存
        /// </summary>
        public decimal? MinStock { get; set; }

        /// <summary>
        /// 物料描述
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
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}