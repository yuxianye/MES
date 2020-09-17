using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.ProductManagement.Models
{
    [Description("产品类别信息")]
    public  class ProductTypeInfo : EntityBase<Guid>, IAudited/*, IRecyclable, ILockable*/
    {
        //public ProductTypeInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 产品类别名称
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string ProductTypeName { get; set; }

        /// <summary>
        /// 产品类别编号
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string ProductTypeCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }
        //#region Implementation of ILockable
        ///// <summary>
        ///// 是否锁定
        ///// </summary>
        //public bool IsLocked { get; set; }
        //#endregion

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
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public DateTime? LastUpdatedTime { get; set; } 

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion

        //#region Implementation of IRecyclable
        ///// <summary>
        ///// 是否逻辑删除
        ///// </summary>
        //public bool IsDeleted { get; set; }
        //#endregion
    }
}
