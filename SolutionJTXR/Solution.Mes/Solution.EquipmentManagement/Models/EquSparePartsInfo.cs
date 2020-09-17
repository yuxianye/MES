using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Solution.EquipmentManagement.Models
{
    [Description("备件信息")]
    public class EquSparePartsInfo : EntityBase<Guid>, /*ILockable, IRecyclable, */IAudited
    {
        /// <summary>
        /// 备件名称
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]

        public string SparePartName { get; set; }
        /// <summary>
        /// 备件编号
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]

        public string SparePartCode { get; set; }
        /// <summary>
        /// 备件类别
        /// </summary>
        /// 
        public virtual EquSparePartTypeInfo Equspareparttypeinfo { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [StringLength(100)]
        public string Specifications { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        [StringLength(100)]
        public string ModelNumber { get; set; }

        /// <summary>
        /// 备件数量
        /// </summary>
        public int Quantity { get; set; }//EquRunningStateTypes
        /// <summary>
        ///数量单位(1:件,2:个,3:打))
        /// </summary>
        public int SparePartUnit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [StringLength(200)]

        public string Remark { get; set; }

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

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

        public DateTime? LastUpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion

    }
}
