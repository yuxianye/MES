using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Solution.EquipmentManagement.Models;


namespace Solution.EquipmentManagement.Models
{
    [Description("备件类别信息")]
    public class EquipmentSparePartTypeMap : EntityBase<Guid>, /*ILockable, IRecyclable, */IAudited
    {
        //public string EquipmentName { get; set; }
        /// <summary>
        /// 备件类别
        /// </summary>
        public virtual EquSparePartTypeInfo Equspareparttypeinfo { get; set; }
        public virtual EquEquipmentInfo Equipmentinfo { get; set; }

        /// <summary>
        /// 备注
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
        public DateTime? LastUpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion

    }
}
