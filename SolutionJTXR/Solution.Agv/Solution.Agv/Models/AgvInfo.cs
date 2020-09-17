using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Agv.Models
{
    [Description("Agv小车信息")]
    public class AgvInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 车辆编号
        /// </summary>
        [StringLength(50)]
        public string CarNo { get; set; }

        /// <summary>
        /// 车辆名称
        /// </summary>
        [StringLength(50)]
        public string CarName { get; set; }

        /// <summary>
        /// 所属区域
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 所属产线
        /// </summary>
        public int ProductLineId { get; set; }

        /// <summary>
        /// 设定速度
        /// </summary>
        public float SettingSpeed { get; set; }

        /// <summary>
        /// 通行优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 报警电量
        /// </summary>
        public int AlarmPowerLevel { get; set; }

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
