using OSharp.Core.Data;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.MatWarehouseStorageManagement.Models
{
    /// <summary>
    /// 供应商信息
    /// </summary>
    [Description("供应商信息")]
    public class MatSupplierInfo : EntityBase<Guid>, IAudited
    {                 
        public MatSupplierInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 供应商编号，不能重复
        /// </summary>
        [StringLength(50)]
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称，不能重复
        /// </summary>
        [StringLength(100)]
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商电话
        /// </summary>
        [StringLength(50)]
        public string SupplierPhone { get; set; }


        /// <summary>
        /// 供应商传真
        /// </summary>
        [StringLength(50)]
        public string SupplierFax { get; set; }

        /// <summary>
        /// 供应商地址
        /// </summary>
        [StringLength(100)]
        public string SupplierAddress { get; set; }

        /// <summary>
        /// 供应商邮箱
        /// </summary>
        [StringLength(50)]
        public string SupplierEmail { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [StringLength(50)]
        public string Contact { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(100)]
        public string Description { get; set; }

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
