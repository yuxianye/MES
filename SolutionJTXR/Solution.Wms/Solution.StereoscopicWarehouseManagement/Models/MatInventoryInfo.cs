using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.MatWarehouseStorageManagement.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.StereoscopicWarehouseManagement.Models
{
    /// <summary>
    /// 盘点信息
    /// </summary>
    [Description("盘点信息")]
    public class MatInventoryInfo : EntityBase<Guid>, IAudited
    {
        public MatInventoryInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 仓库ID
        /// </summary>
        public virtual MatWareHouseInfo MatWareHouse { get; set; }

        /// <summary>
        /// 盘点类型
        /// </summary>
        public int? InventoryType { get; set; }

        /// <summary>
        /// 盘点编号
        /// </summary>
        [StringLength(50)]
        public string InventoryCode { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        [StringLength(50)]
        public string Operator { get; set; }


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
