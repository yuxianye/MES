using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.EquipmentManagement.Dtos
{
    /// <summary>
    /// 设备厂家信息
    /// </summary>
    public class EquFactoryInfoInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 厂家名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 厂家编号
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 厂家地址
        /// </summary>
        public string FactoryAddress { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacts { get; set; }

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
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
