using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.PlanOrderManage.Model.PlanEnumModel;

namespace Solution.Desktop.PlanOrderManage.Model
{
    /// <summary>
    /// 订单模型
    /// </summary>
    public class PlanOrderInfoModel : ModelBase, IAudited
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
        #region 订单名称
        private string orderName;

        /// <summary>
        /// 订单名称
        /// </summary>
        [Required(ErrorMessage = "订单名称必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
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
        [Required(ErrorMessage = "订单编号必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string OrderCode
        {
            get { return orderCode; }
            set { Set(ref orderCode, value); }
        }

        #endregion

        #region 采购人
        private string procurement;

        /// <summary>
        /// 采购人
        /// </summary>
        public string Procurement
        {
            get { return procurement; }
            set { Set(ref procurement, value); }
        }
        #endregion

        #region 采购人电话
        private string procurePhone;

        /// <summary>
        /// 采购人电话
        /// </summary>
        [RegularExpression(@"^[1][3,4,5,6,7,8,9][0-9]{9}$", ErrorMessage = "无效的电话号码")]
        public string ProcurePhone
        {
            get { return procurePhone; }
            set { Set(ref procurePhone, value); }
        }
        #endregion
        #region 订单交货日期
        private DateTime? deliveryTime;

        /// <summary>
        /// 订单交货日期
        /// </summary>
        public DateTime? DeliveryTime
        {
            get { return deliveryTime; }
            set { Set(ref deliveryTime, value); }
        }
        #endregion
        #region 预期完成日期
        private DateTime? expectedFinishTime;

        /// <summary>
        /// 预期完成日期
        /// </summary>
        public DateTime? ExpectedFinishTime
        {
            get { return expectedFinishTime; }
            set { Set(ref expectedFinishTime, value); }
        }
        #endregion
        #region 实际开始时间
        private DateTime? actualStartTime;

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? ActualStartTime
        {
            get { return actualStartTime; }
            set { Set(ref actualStartTime, value); }
        }
        #endregion
        #region 实际完成时间
        private DateTime? actualFinishTime;

        /// <summary>
        /// 实际完成时间
        /// </summary>
        public DateTime? ActualFinishTime
        {
            get { return actualFinishTime; }
            set { Set(ref actualFinishTime, value); }
        }
        #endregion
        #region 订单状态
        private OrderState orderState;

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderState OrderState
        {
            get { return orderState; }
            set { Set(ref orderState, value); }
        }
        #endregion
        #region 发货地址
        private string deliverAddress;

        /// <summary>
        /// 发货地址
        /// </summary>
        public string DeliverAddress
        {
            get { return deliverAddress; }
            set { Set(ref deliverAddress, value); }
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
            Procurement = null;
            ProcurePhone = null;
            DeliveryTime = null;
            ExpectedFinishTime = null;
            ActualStartTime = null;
            ActualFinishTime = null;
            DeliverAddress = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
