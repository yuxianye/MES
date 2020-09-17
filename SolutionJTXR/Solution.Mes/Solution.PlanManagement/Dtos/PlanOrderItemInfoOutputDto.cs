using OSharp.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using Solution.ProductManagement.Models;
using Solution.PlanManagement.Models;

namespace Solution.PlanManagement.Dtos
{
    public class PlanOrderItemInfoOutputDto : IOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 订单明细名称
        /// </summary>
        public string OrderItemName { get; set; }

        /// <summary>
        /// 订单明细编号
        /// </summary>
        public string OrderItemCode { get; set; }
        /// <summary>
        /// 订单产品数量
        /// </summary>
        public decimal? OrderQuantity { get; set; }

        /// <summary>
        /// 剩余可排产数量
        /// </summary>
        public decimal? RemainQuantity { get; set; }

        /// <summary>
        /// 单位(1:个,2:箱,3:件)
        /// </summary>
        public int? ProductUnit { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int? OrderState { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public Guid Order_Id { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public Guid Product_Id { get; set; }

        /// <summary>
        /// 订单
        /// </summary>
        public PlanOrderInfo Order { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public ProductInfo Product { get; set; }

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
