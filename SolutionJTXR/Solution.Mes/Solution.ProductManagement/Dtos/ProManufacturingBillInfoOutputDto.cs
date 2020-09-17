using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.ProductManagement.Dtos
{
    public class ProManufacturingBillInfoOutputDto : IOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        /// 
        public Guid Product_Id { get; set; }

        /// <summary>
        /// 配方ID
        /// </summary>
        /// 
        public Guid ProductionRule_Id { get; set; }


        /// <summary>
        /// 清单名称
        /// </summary>
        public string BillName { get; set; }

        /// <summary>
        /// 清单编号
        /// </summary>
        public string BillCode { get; set; }

        /// <summary>
        /// 清单类型(1:BOM,2:BOR)
        /// </summary>
        public int? BillType { get; set; }

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
