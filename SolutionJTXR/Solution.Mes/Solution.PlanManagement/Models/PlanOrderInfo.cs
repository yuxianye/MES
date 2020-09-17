using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.PlanManagement.Models
{
    [Description("订单信息")]
    public class PlanOrderInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 订单名称
        /// </summary>
        [StringLength(100)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string OrderName { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [StringLength(100)]
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public string OrderCode { get; set; }

        /// <summary>
        /// 订单状态ID
        /// </summary>
        public int? OrderState { get; set; }
        /// <summary>
        /// 采购人
        /// </summary>
        [StringLength(50)]
        public string Procurement { get; set; }

        /// <summary>
        /// 采购人电话
        /// </summary>
        [StringLength(50)]
        public string ProcurePhone { get; set; }


        /// <summary>
        /// 订单交货日期
        /// </summary>
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// 预期完成时间
        /// </summary>
        public DateTime? ExpectedFinishTime { get; set; }

        /// <summary>
        /// 实际开始生产时间
        /// </summary>
        public DateTime? ActualStartTime { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualFinishTime { get; set; }

        /// <summary>
        /// 发货地址
        /// </summary>
        [StringLength(200)]
        public string DeliverAddress { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        [StringLength(200)]
        public string Description { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
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
