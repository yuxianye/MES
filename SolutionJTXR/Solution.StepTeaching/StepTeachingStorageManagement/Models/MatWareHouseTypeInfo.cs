using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.MatWarehouseStorageManagement.Models
{
    /// <summary>
    /// 仓库类型信息
    /// </summary>
    [Description("仓库类型信息")]
    public class MatWareHouseTypeInfo : EntityBase<Guid>, IAudited
    {
        public MatWareHouseTypeInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 仓库类别编号，不能重复
        /// </summary>
        [StringLength(50)]
        public string WareHouseTypeCode { get; set; }

        /// <summary>
        /// 仓库类别名称，不能重复
        /// </summary>
        [StringLength(50)]
        public string WareHouseTypeName { get; set; }
        
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
