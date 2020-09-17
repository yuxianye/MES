using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.CommunicationModule.Models
{
    /// <summary>
    /// 实体类——通讯数据点配置
    /// </summary>
    [Description("工序设备业务点表配置")]
    public class ProductionProcessEquipmentBusinessNodeMap : EntityBase<Guid>, IAudited/*, IRecyclable, ILockable*/
    {
        /// <summary>
        /// 工序编号
        /// </summary>
        [Display(Description = "工序")]
        [Required]
        public Guid? ProductionProcessInfo_Id { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        [Display(Description = "设备ID")]
        public Guid? Equipment_Id { get; set; }

        /// <summary>
        /// 设备数据点
        /// </summary>
        public virtual DeviceNode DeviceNode { get; set; }

        /// <summary>
        /// 业务数据点
        /// </summary>
        public virtual BusinessNode BusinessNode { get; set; }

        [Display(Description = "描述")]
        [StringLength(100)]
        public string Description { get; set; }

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

    }
}
