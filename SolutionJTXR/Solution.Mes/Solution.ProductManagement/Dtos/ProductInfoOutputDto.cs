using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.ProductManagement.Dtos
{
    public class ProductInfoOutputDto : IOutputDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public Guid ProductType_Id { get; set; }
        /// <summary>
        /// 产品规格型号
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 产品描述
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
