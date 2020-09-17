using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.EnterpriseInformation.Dtos
{
    public class EntProductionLineInfoOutputDto : IOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public Guid EntArea_Id { get; set; }

        /// <summary>
        /// 生产线名称
        /// </summary>
        public string ProductionLineName { get; set; }

        /// <summary>
        /// 生产线编号
        /// </summary>
        public string ProductionLineCode { get; set; }

        /// <summary>
        /// 生产线时长
        /// </summary>
        public decimal? Duration { get; set; }

        /// <summary>
        /// 时长单位
        /// </summary>
        public int? DurationUnit { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
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
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
