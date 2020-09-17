using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.MatWarehouseStorageManagement.Models
{
    /// <summary>
    /// 托盘信息
    /// </summary>
    [Description("托盘信息")]
    public class MatPalletInfo : EntityBase<Guid>, IAudited
    {
        public MatPalletInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 托盘编号，不能重复
        /// </summary>
        [StringLength(50)]
        public string PalletCode { get; set; }

        /// <summary>
        /// 托盘名称，不能重复
        /// </summary>
        [StringLength(50)]
        public string PalletName { get; set; }

        /// <summary>
        /// 托盘最大承重
        /// </summary>
        public decimal? PalletMaxWeight { get; set; }

        /// <summary>
        /// 托盘规格
        /// </summary>
        [StringLength(100)]
        public string PalletSpecifications { get; set; }
    
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
