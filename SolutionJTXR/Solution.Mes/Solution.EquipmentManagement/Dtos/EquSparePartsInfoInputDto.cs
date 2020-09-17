using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EquipmentManagement.Models;


namespace Solution.EquipmentManagement.Dtos
{
    public class EquSparePartsInfoInputDto : /* ILockable, IRecyclable,*/ IInputDto<Guid>, IAudited
    {


        public Guid Id { get; set; }

        /// <summary>
        /// 备件类别ID
        /// </summary>
        public Guid EquSparePartType_Id { get; set; }

        /// <summary>
        /// 备件名称
        /// </summary>

        public string SparePartName { get; set; }
        /// <summary>
        /// 备件编号
        /// </summary>

        public string SparePartCode { get; set; }
        /// <summary>
        /// 备件类别
        /// </summary>
        /// 
        public virtual EquSparePartTypeInfo Equspareparttypeinfo { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>

        public string Specifications { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
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
        /// 备注
        /// </summary>

        public string Remark { get; set; }
        ///// <summary>
        ///// 是否锁定
        ///// </summary>
        //public bool IsLocked { get; set; }
        /// <summary>
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
