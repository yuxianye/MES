using Solution.Desktop.Core;
using Solution.Utility;
using System;

namespace Solution.Desktop.ProManufacturingBillManage.Model
{
    public class ProManufacturingBORBillItemInfoModel : ModelBase, IAudited
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
        #region 制造清单ID
        private Guid proManufacturingBill_Id;
        public Guid ProManufacturingBill_Id
        {
            get { return proManufacturingBill_Id; }
            set { Set(ref proManufacturingBill_Id, value); }
        }

        #endregion
        #region 制造清单名称
        private string billName;

        /// <summary>
        ///  制造清单名称
        /// </summary>
        public string BillName
        {
            get { return billName; }
            set { Set(ref billName, value); }
        }

        #endregion

        #region 制造清单编号
        private string billCode;

        /// <summary>
        ///  制造清单编号
        /// </summary>

        public string BillCode
        {
            get { return billCode; }
            set { Set(ref billCode, value); }
        }

        #endregion
        #region 工序ID
        private Guid productionProcess_Id;
        public Guid ProductionProcess_Id
        {
            get { return productionProcess_Id; }
            set { Set(ref productionProcess_Id, value); }
        }

        #endregion
        #region 工序名称
        private string productionProcessName;

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProductionProcessName
        {
            get { return productionProcessName; }
            set { Set(ref productionProcessName, value); }
        }

        #endregion

        #region 工序编号
        private string productionProcessCode;

        /// <summary>
        /// 工序编号
        /// </summary>

        public string ProductionProcessCode
        {
            get { return productionProcessCode; }
            set { Set(ref productionProcessCode, value); }
        }

        #endregion
        #region 设备ID
        private Guid equipment_Id;
        public Guid Equipment_Id
        {
            get { return equipment_Id; }
            set { Set(ref equipment_Id, value); }
        }

        #endregion
        #region 设备名称
        private string equipmentlName;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return equipmentlName; }
            set { Set(ref equipmentlName, value); }
        }

        #endregion

        #region 设备编号
        private string equipmentCode;

        /// <summary>
        /// 设备编号
        /// </summary>

        public string EquipmentCode
        {
            get { return equipmentCode; }
            set { Set(ref equipmentCode, value); }
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
        #region 数量
        private decimal? quantity;

        /// <summary>
        /// BOM描述
        /// </summary>
        public decimal? Quantity
        {
            get { return quantity; }
            set { Set(ref quantity, value); }
        }
        #endregion
        #region BOM描述
        private string description;

        /// <summary>
        /// BOM描述
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
            ProManufacturingBill_Id = Guid.Empty;
            ProductionProcess_Id = Guid.Empty;
            ProductionRule_Id = Guid.Empty;
            ProductionProcessCode = null;
            ProductionProcessName = null;
            ProductionRuleName = null;
            ProductionRuleStatusCode = null;
            ProductionRuleVersion = null;
            BillCode = null;
            BillName = null;
            Quantity = null;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
