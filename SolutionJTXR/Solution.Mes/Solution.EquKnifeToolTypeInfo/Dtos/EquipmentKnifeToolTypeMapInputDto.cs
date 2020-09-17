using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EquipKnifeToolInfo.Models;
using Solution.EquipmentManagement.Models;


namespace Solution.EquipKnifeToolInfo.Dtos
{
    public class EquipmentKnifeToolTypeMapInputDto : /* ILockable, IRecyclable,*/ IInputDto<Guid>, IAudited
    {

  
        public Guid Id{ get; set; }

        /// <summary>
        /// 刀具ID
        /// </summary>
        public Guid KnifeToolTypeInfo_Id { get; set; }

        /// <summary>
        /// 刀具ID
        /// </summary>
        /// 
        public virtual KnifeToolTypeInfo Knifetooltypeinfo { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public Guid EquipmentInfo_Id { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        /// 
        public virtual EquipmentInfo Equipmentinfo { get; set; }

        public string KnifeToolTypeName { get; set; }
        public string EquipmentName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]

        public string Remark { get; set; }
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
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
        //#region Implementation of IRecyclable
        //public bool IsDeleted { get; set; }
        //#endregion
    }
}
