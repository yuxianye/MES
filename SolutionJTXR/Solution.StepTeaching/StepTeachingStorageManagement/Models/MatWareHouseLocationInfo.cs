using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.MatWarehouseStorageManagement.Models
{
    /// <summary>
    /// 库位信息
    /// </summary>
    [Description("库位信息")]
    public class MatWareHouseLocationInfo : EntityBase<Guid>, IAudited
    {
        public MatWareHouseLocationInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 所属仓库ID
        /// </summary>
        public virtual MatWareHouseInfo MatWareHouse { get; set; }

        /// <summary>
        /// 所属货区ID
        /// </summary>
        public virtual MatWareHouseAreaInfo MatWareHouseArea { get; set; }

        /// <summary>
        /// 库位编号，不能重复
        /// </summary>
        [StringLength(50)]
        public string WareHouseLocationCode { get; set; }

        /// <summary>
        /// 库位名称，不能重复
        /// </summary>
        [StringLength(50)]
        public string WareHouseLocationName { get; set; }

        /// <summary>
        /// 库位类型
        /// </summary>
        public int? WareHouseLocationType { get; set; }

        /// <summary>
        /// 库位状态
        /// </summary>
        public int? WareHouseLocationStatus { get; set; }

        /// <summary>
        /// 托盘ID
        /// </summary>
        public Guid? PalletID { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public Guid? MaterialID { get; set; }

        /// <summary>
        /// 入库ID
        /// </summary>
        public Guid? InStorageID { get; set; }

        /// <summary>
        /// 物料批次ID
        /// </summary>
        public Guid? MaterialBatchID { get; set; }

        /// <summary>
        /// 库位存放物料数量
        /// </summary>
        public decimal? StorageQuantity { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }

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
