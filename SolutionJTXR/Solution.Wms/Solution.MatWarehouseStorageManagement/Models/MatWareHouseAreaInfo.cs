using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.MatWarehouseStorageManagement.Models
{
    /// <summary>
    /// 仓库货区信息
    /// </summary>
    [Description("仓库货区信息")]
    public class MatWareHouseAreaInfo : EntityBase<Guid>, IAudited
    {
        public MatWareHouseAreaInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 所属仓库ID
        /// </summary>
        public virtual MatWareHouseInfo MatWareHouse { get; set; }

        /// <summary>
        /// 仓库货区编号，不能重复
        /// </summary>
        [StringLength(50)]
        public string WareHouseAreaCode { get; set; }

        /// <summary>
        /// 仓库货区名称，不能重复
        /// </summary>
        [StringLength(50)]
        public string WareHouseAreaName { get; set; }

        /// <summary>
        /// 库位类型
        /// </summary>
        public int? WareHouseLocationType { get; set; }

        /// <summary>
        /// 货架层数
        /// </summary>
        public int? LayerNumber { get; set; }

        /// <summary>
        /// 货架列数
        /// </summary>
        public int? ColumnNumber { get; set; }

        /// <summary>
        /// 库位总数
        /// </summary>
        public int? LocationQuantity { get; set; }

        /// <summary>
        /// 货架规格
        /// </summary>
        [StringLength(100)]
        public string StorageRackSpecifications { get; set; }

        /// <summary>
        /// 库位规格
        /// </summary>
        [StringLength(100)]
        public string LocationSpecifications { get; set; }

        /// <summary>
        /// 库位承载(KG)
        /// </summary>
        public decimal? LocationLoadBearing { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// 是否生成库位
        /// </summary>
        public bool IsGenerageLocation { get; set; }

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
