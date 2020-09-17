using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.ProductManagement.Dtos
{
    public class ProductionProcessInfoOutputDto : IOutputDto
    {

        /// <summary>
        /// 工序ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 生产线ID
        /// </summary>
        /// 
        public Guid EntProductionLine_Id { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProductionProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public string ProductionProcessCode { get; set; }

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
