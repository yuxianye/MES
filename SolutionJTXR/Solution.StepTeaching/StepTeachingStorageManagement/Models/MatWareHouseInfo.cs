using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.MatWarehouseStorageManagement.Models
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    [Description("仓库信息")]
    public class MatWareHouseInfo : EntityBase<Guid>, IAudited
    {
        public MatWareHouseInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 所属区域ID
        /// </summary>
        public virtual EntAreaInfo EntArea { get; set; }

        /// <summary>
        /// 仓库类型ID
        /// </summary>
        public virtual MatWareHouseTypeInfo MatWareHouseType { get; set; }

        /// <summary>
        /// 仓库编号，不能重复
        /// </summary>
        [StringLength(50)]
        public string WareHouseCode { get; set; }

        /// <summary>
        /// 仓库名称，不能重复
        /// </summary>
        [StringLength(50)]
        public string WareHouseName { get; set; }

        /// <summary>
        /// 仓库描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// 仓库负责人
        /// </summary>
        [StringLength(50)]
        public string Manager { get; set; }

        /// <summary>
        /// 仓库电话
        /// </summary>
        [StringLength(50)]
        public string WareHousePhone { get; set; }

        
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
