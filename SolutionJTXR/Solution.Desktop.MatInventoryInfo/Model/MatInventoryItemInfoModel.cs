using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.InventoryStateEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialTypeEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialUnitEnumModel;

namespace Solution.Desktop.MatInventoryInfo.Model
{
    /// <summary>
    /// 盘点明细信息模型
    /// </summary>
    public class MatInventoryItemInfoModel : ModelBase, IAudited
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

        #region 盘点ID
        private Guid? matinventory_Id;

        /// <summary>
        /// 盘点ID
        /// </summary>
        public Guid? MatInventory_Id
        {
            get { return matinventory_Id; }
            set { Set(ref matinventory_Id, value); }
        }
        #endregion


        #region 所属库位ID
        private Guid matwarehouselocation_Id;
        public Guid MatWareHouseLocation_Id
        {
            get { return matwarehouselocation_Id; }
            set { Set(ref matwarehouselocation_Id, value); }
        }
        #endregion

        #region 所属库位编号
        private string warehouselocationCode;

        // <summary>
        // 所属库位编号
        // </summary>
        public string WareHouseLocationCode
        {
            get { return warehouselocationCode; }
            set { Set(ref warehouselocationCode, value); }
        }
        #endregion

        #region 所属库位名称
        private string warehouselocationName;

        // <summary>
        // 所属库位名称
        // </summary>
        public string WareHouseLocationName
        {
            get { return warehouselocationName; }
            set { Set(ref warehouselocationName, value); }
        }
        #endregion      


        #region 物料批次ID
        private Guid? materialbatch_Id;

        /// <summary>
        /// 物料批次ID
        /// </summary>
        public Guid? MaterialBatch_Id
        {
            get { return materialbatch_Id; }
            set { Set(ref materialbatch_Id, value); }
        }
        #endregion

        #region 批次编号
        private string batchCode;

        /// <summary>
        /// 批次编号
        /// </summary>
        public string BatchCode
        {
            get { return batchCode; }
            set { Set(ref batchCode, value); }
        }

        #endregion

        #region 物料编号
        private string materialCode;

        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialCode
        {
            get { return materialCode; }
            set { Set(ref materialCode, value); }
        }

        #endregion

        #region 物料名称
        private string materialName;

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName
        {
            get { return materialName; }
            set { Set(ref materialName, value); }
        }
        #endregion


        #region 物料类型
        private MaterialType materialType;

        /// <summary>
        /// 物料类型
        /// </summary>
        public MaterialType MaterialType
        {
            get { return materialType; }
            set { Set(ref materialType, value); }
        }
        #endregion

        /// ////////////////////////////////////////////////////////////

        #region 物料单位
        private MaterialUnit materialUnit;

        /// <summary>
        /// 物料单位
        /// </summary>
        public MaterialUnit MaterialUnit
        {
            get { return materialUnit; }
            set { Set(ref materialUnit, value); }
        }
        #endregion

        #region 满盘数量
        private int? fullpalletQuantity;

        /// <summary>
        /// 满盘数量
        /// </summary>
        public int? FullPalletQuantity
        {
            get { return fullpalletQuantity; }
            set { Set(ref fullpalletQuantity, value); }
        }
        #endregion

        #region 账面数量
        private decimal? accuntAmount;

        /// <summary>
        /// 账面数量
        /// </summary>
        [Required(ErrorMessage = "账面数量必填")]
        public decimal? AccuntAmount
        {
            get { return accuntAmount; }
            set { Set(ref accuntAmount, value); }
        }
        #endregion


        #region 实际数量
        private decimal? actualAmount;

        /// <summary>
        /// 实际数量
        /// </summary>
        [Required(ErrorMessage = "实际数量必填")]
        public decimal? ActualAmount
        {
            get { return actualAmount; }
            set { Set(ref actualAmount, value); }
        }
        #endregion

        #region 差异数量
        private decimal? differenceAmount;

        /// <summary>
        /// 差异数量
        /// </summary>
        public decimal? DifferenceAmount
        {
            get { return differenceAmount; }
            set { Set(ref differenceAmount, value); }
        }
        #endregion    

        #region 操作人员
        private string operatorPerson;

        /// <summary>
        /// 操作人员
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string Operator
        {
            get { return operatorPerson; }
            set { Set(ref operatorPerson, value); }
        }
        #endregion


        #region 盘点时间
        private DateTime? inventoryTime;

        /// <summary>
        /// 盘点时间
        /// </summary>
        public DateTime? InventoryTime
        {
            get { return inventoryTime; }
            set { Set(ref inventoryTime, value); }
        }
        #endregion


        #region 盘点状态
        private InventoryState inventoryState;

        /// <summary>
        /// 盘点状态
        /// </summary>
        public InventoryState InventoryState
        {
            get { return inventoryState; }
            set { Set(ref inventoryState, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度大于200个字符")]
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
            Remark = null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
