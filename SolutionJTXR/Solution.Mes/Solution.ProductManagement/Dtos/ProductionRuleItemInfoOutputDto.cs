using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.ProductManagement.Dtos
{
    public class ProductionRuleItemInfoOutputDto : IOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 配方ID
        /// </summary>
        /// 
        public Guid ProductionRule_Id { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public Guid ProductionProcess_Id { get; set; }

        /// <summary>
        /// 工序排序
        /// </summary>
        public int? ProductionProcessOrder { get; set; }

        /// <summary>
        /// 工序时长
        /// </summary>
        public decimal? Duration { get; set; }

        /// <summary>
        /// 时长单位 （1:小时,2:分钟,3:秒）
        /// </summary>
        public int? DurationUnit { get; set; }

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
