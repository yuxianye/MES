using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EnterpriseInformation.Models;

namespace Solution.MatWarehouseStorageManagement.Dtos
{
    /// <summary>
    /// 供应商信息输入Dto《属性大部分与models相同,直接复制即可》
    /// </summary>
    public class MatSupplierInfoInputDto : IInputDto<Guid>, IAudited
    {

        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id{ get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商电话
        /// </summary>
        public string SupplierPhone { get; set; }


        /// <summary>
        /// 供应商传真
        /// </summary>
        public string SupplierFax { get; set; }

        /// <summary>
        /// 供应商地址
        /// </summary>
        public string SupplierAddress { get; set; }

        /// <summary>
        /// 供应商邮箱
        /// </summary>
        public string SupplierEmail { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
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
