using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using OSharp.Utility.Data;
using Solution.EquipmentManagement.Models;


namespace Solution.EquipmentManagement.Dtos
{
    public class EquRunningStateInfoInputDto : /* ILockable, IRecyclable,*/ IInputDto<Guid>, IAudited
    {


        public Guid Id { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public Guid EquipmentInfo_Id { get; set; }

        /// <summary>
        /// 设备模型
        /// </summary>
        /// 
        public virtual EquEquipmentInfo Equipmentinfo { get; set; }

        /// <summary>
        ///运行状态类型(1:开机,2:关机,3:故障)
        /// </summary>
        public int EquRunningStateTypes { get; set; }//EquRunningStateTypes


        /// <summary>
        /// //采集状态时间
        /// </summary>
        public DateTime RunningStateTime { get; set; }
        /// <summary>
        /// //故障信息
        /// </summary>
        public string FaultInfo { get; set; }
        /// <summary>
        /////故障代码
        /// </summary>
        public string FaultCode { get; set; }

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
