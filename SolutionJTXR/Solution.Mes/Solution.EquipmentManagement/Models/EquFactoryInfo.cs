using OSharp.Core.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.EquipmentManagement.Models
{
    [Description("设备厂家信息")]
    public class EquFactoryInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 厂家名称
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string FactoryName { get; set; }

        /// <summary>
        /// 厂家编号
        /// </summary>
        [StringLength(50)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string FactoryCode { get; set; }
        /// <summary>
        /// 厂家地址
        /// </summary>
        [StringLength(50)]
        public string FactoryAddress { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [StringLength(50)]
        public string Contacts { get; set; }

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

        public DateTime? LastUpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
