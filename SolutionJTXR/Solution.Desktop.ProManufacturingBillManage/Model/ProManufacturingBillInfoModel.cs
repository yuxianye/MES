using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.ProManufacturingBillManage.Model.ProductEnumModel;

namespace Solution.Desktop.ProManufacturingBillManage.Model
{
    /// <summary>
    /// 制造清单模型
    /// </summary>
    public class ProManufacturingBillInfoModel : ModelBase, IAudited
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
        #region 产品ID
        private Guid product_Id;
        public Guid Product_Id
        {
            get { return product_Id; }
            set { Set(ref product_Id, value); }
        }

        #endregion

        #region 产品名称
        private string productName;
        public string ProductName
        {
            get { return productName; }
            set { Set(ref productName, value); }
        }

        #endregion
        #region 产品编号
        private string productCode;
        public string ProductCode
        {
            get { return productCode; }
            set { Set(ref productCode, value); }
        }

        #endregion
        #region 配方ID
        private Guid productionRule_Id;
        public Guid ProductionRule_Id
        {
            get { return productionRule_Id; }
            set { Set(ref productionRule_Id, value); }
        }

        #endregion
        #region 配方名称
        private string productionRuleName;
        public string ProductionRuleName
        {
            get { return productionRuleName; }
            set { Set(ref productionRuleName, value); }
        }

        #endregion
        #region 配方版本号
        private string productionRuleVersion;
        public string ProductionRuleVersion
        {
            get { return productionRuleVersion; }
            set { Set(ref productionRuleVersion, value); }
        }

        #endregion

        #region 制造清单名称
        private string billName;

        /// <summary>
        /// 制造清单名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string BillName
        {
            get { return billName; }
            set { Set(ref billName, value); }
        }

        #endregion

        #region 制造清单编号
        private string billCode;

        /// <summary>
        /// 制造清单编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string BillCode
        {
            get { return billCode; }
            set { Set(ref billCode, value); }
        }

        #endregion

        #region 制造清单类型名称
        private string billTypeName;

        /// <summary>
        /// 制造清单类型名称
        /// </summary>
        public string BillTypeName
        {
            get { return billTypeName; }
            set { Set(ref billTypeName, value); }
        }
        #endregion
        #region 物料类型
        private BillType billType;

        /// <summary>
        /// 物料类型
        /// </summary>
        [Required(ErrorMessage = "清单类型必填")]
        public BillType BillType
        {
            get { return billType; }
            set { Set(ref billType, value); }
        }
        #endregion
        //#region 制造清单类型
        //private int? billType;

        ///// <summary>
        ///// 制造清单类型
        ///// </summary>
        //public int? BillType
        //{
        //    get { return billType; }
        //    set { Set(ref billType, value); }
        //}
        //#endregion
        #region 配方状态ID
        private Guid productionRuleStatus_Id;
        public Guid ProductionRuleStatus_Id
        {
            get { return productionRuleStatus_Id; }
            set { Set(ref productionRuleStatus_Id, value); }
        }

        #endregion
        #region 配方状态编号
        private string productionRuleStatusCode;

        /// <summary>
        /// 配方状态编号
        /// </summary>

        public string ProductionRuleStatusCode
        {
            get { return productionRuleStatusCode; }
            set { Set(ref productionRuleStatusCode, value); }
        }

        #endregion
        #region 配方状态名称
        private string productionRuleStatusName;

        /// <summary>
        /// 配方状态名称
        /// </summary>

        public string ProductionRuleStatusName
        {
            get { return productionRuleStatusName; }
            set { Set(ref productionRuleStatusName, value); }
        }

        #endregion
        #region 制造清单描述
        private string description;

        /// <summary>
        /// 制造清单描述
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
            BillName = null;
            BillCode = null;
            Product_Id = Guid.Empty;
            ProductionRule_Id = Guid.Empty;
            ProductCode = null;
            ProductName = null;
            ProductionRuleName = null;
            ProductionRuleStatusCode = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
