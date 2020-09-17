using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.PlanOrderManage.Model.PlanEnumModel;

namespace Solution.Desktop.PlanOrderManage.Model
{
    public class PlanOrderItemInfoModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// Id
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion
        #region 订单Id
        private Guid order_Id;

        /// <summary>
        /// 订单Id
        /// </summary>

        public Guid Order_Id
        {
            get { return order_Id; }
            set { Set(ref order_Id, value); }
        }

        #endregion
        #region 订单名称
        private string orderName;

        /// <summary>
        /// 订单名称
        /// </summary>
        public string OrderName
        {
            get { return orderName; }
            set { Set(ref orderName, value); }
        }
        #endregion
        #region 订单编号
        private string orderCode;

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode
        {
            get { return orderCode; }
            set { Set(ref orderCode, value); }
        }
        #endregion
        #region 产品Id
        private Guid product_Id;

        /// <summary>
        /// 产品Id
        /// </summary>

        public Guid Product_Id
        {
            get { return product_Id; }
            set { Set(ref product_Id, value); }
        }

        #endregion
        #region 产品名称
        private string productName;

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { Set(ref productName, value); }
        }
        #endregion
        #region 产品编号
        private string productCode;

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductCode
        {
            get { return productCode; }
            set { Set(ref productCode, value); }
        }
        #endregion
        #region 订单明细名称
        private string orderItemName;

        /// <summary>
        /// 订单明细名称
        /// </summary>
        [Required(ErrorMessage = "订单明细名称必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string OrderItemName
        {
            get { return orderItemName; }
            set { Set(ref orderItemName, value); }
        }

        #endregion

        #region 订单明细编号
        private string orderItemCode;

        /// <summary>
        /// 订单明细编号
        /// </summary>
        [Required(ErrorMessage = "订单明细编号必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string OrderItemCode
        {
            get { return orderItemCode; }
            set { Set(ref orderItemCode, value); }
        }

        #endregion

        #region 订单数量
        private decimal? orderQuantity;

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal? OrderQuantity
        {
            get { return orderQuantity; }
            set { Set(ref orderQuantity, value); }
        }
        #endregion

        #region 剩余可排产数量
        private decimal? remainQuantity;

        /// <summary>
        /// 剩余可排产数量
        /// </summary>
        public decimal? RemainQuantity
        {
            get { return remainQuantity; }
            set { Set(ref remainQuantity, value); }
        }
        #endregion
        #region 产品单位
        private ProductUnit productUnit;

        /// <summary>
        /// 产品单位
        /// </summary>
        public ProductUnit ProductUnit
        {
            get { return productUnit; }
            set { Set(ref productUnit, value); }
        }
        #endregion

        #region 订单明细状态
        private OrderState orderState;

        /// <summary>
        /// 订单明细状态
        /// </summary>
        public OrderState OrderState
        {
            get { return orderState; }
            set { Set(ref orderState, value); }
        }
        #endregion
        #region 描述
        private string description;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }
        #endregion
        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
        }
        #endregion
        #region 记录创建时间
        private DateTime createdTime;

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { Set(ref createdTime, value); }
        }
        #endregion

        #region 创建者编号
        private string creatorUserId;

        /// <summary>
        /// 创建者编号
        /// </summary>
        public string CreatorUserId
        {
            get { return creatorUserId; }
            set { Set(ref creatorUserId, value); }
        }
        #endregion

        #region 最后更新时间
        private DateTime? lastUpdatedTime;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdatedTime
        {
            get { return lastUpdatedTime; }
            set { Set(ref lastUpdatedTime, value); }
        }
        #endregion

        #region 最后更新者编号
        private string lastUpdatorUserId;

        /// <summary>
        /// 最后更新者编号
        /// </summary>
        public string LastUpdatorUserId
        {
            get { return lastUpdatorUserId; }
            set { Set(ref lastUpdatorUserId, value); }
        }
        #endregion
        protected override void Disposing()
        {
            OrderCode = null;
            OrderName = null;
            OrderItemName = null;
            OrderItemCode = null;
            OrderQuantity = null;
            RemainQuantity = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
