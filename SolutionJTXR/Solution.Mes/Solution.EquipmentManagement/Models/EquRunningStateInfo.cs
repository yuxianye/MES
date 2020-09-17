using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Solution.EquipmentManagement.Models
{
    [Description("设备运行状态信息")]
    public class EquRunningStateInfo : EntityBase<Guid>, /*ILockable, IRecyclable, */IAudited
    {
        /// <summary>
        /// 关联的设备  //设备ID
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
        [StringLength(200)]
        public string FaultInfo { get; set; }
        /// <summary>
        /////故障代码
        /// </summary>
        [StringLength(50)]
        public string FaultCode { get; set; }

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
