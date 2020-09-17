using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EquipmentManagement.Models;


namespace Solution.EquipmentManagement.Dtos
{
    public class EquipmentSparePartTypeMapInputDto : /* ILockable, IRecyclable,*/ IInputDto<Guid>, IAudited
    {


        public Guid Id { get; set; }

        /// <summary>
        /// 备件类别模型
        /// </summary>
        /// 
        public EquSparePartTypeInfo EquSparePartTypeInfo { get; set; }


        /// <summary>
        /// 设备ID
        /// </summary>
        /// 
        public EquEquipmentInfo EquipmentInfo { get; set; }

        //public string EquipmentName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>

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
