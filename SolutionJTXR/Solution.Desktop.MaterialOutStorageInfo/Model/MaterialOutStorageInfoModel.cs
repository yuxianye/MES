using Solution.Desktop.Core;
using Solution.Desktop.MaterialBatchInfo.Model;
using Solution.Utility;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.AuditStatusEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.OutStorageStatusEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.OutStorageTypeEnumModel;

namespace Solution.Desktop.MaterialOutStorageInfo.Model
{
    /// <summary>
    /// 物料出库模型
    /// </summary>
    public class MaterialOutStorageInfoModel : ModelBase, IAudited
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

        #region 出库单编号
        private string outstoragebillCode;

        /// <summary>
        /// 出库单编号
        /// </summary>
        [Required(ErrorMessage = "出库单编号必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]

        public string OutStorageBillCode
        {
            get { return outstoragebillCode; }
            set { Set(ref outstoragebillCode, value); }
        }

        #endregion

        #region 出库类型
        private OutStorageType outstorageType;

        /// <summary>
        /// 出库类型
        /// </summary>
        [Required(ErrorMessage = "出库类型必填")]
        public OutStorageType OutStorageType
        {
            get { return outstorageType; }
            set { Set(ref outstorageType, value); }
        }
        #endregion

        #region 生产排产ID
        private Guid? planscheduleID;

        /// <summary>
        /// 生产排产ID
        /// </summary>
        public Guid? PlanScheduleID
        {
            get { return planscheduleID; }
            set { Set(ref planscheduleID, value); }
        }
        #endregion

        #region 物料ID
        private Guid? material_Id;

        /// <summary>
        /// 物料ID
        /// </summary>
        //[Required(ErrorMessage = "物料信息必填")]
        public Guid? Material_Id
        {
            get { return material_Id; }
            set { Set(ref material_Id, value); }
        }
        #endregion

        #region 物料ID
        private Guid? materialID;

        /// <summary>
        /// 物料ID
        /// </summary>
        public Guid? MaterialID
        {
            get { return materialID; }
            set { Set(ref materialID, value); }
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

        #region 出库数量
        private decimal? quantity;

        /// <summary>
        /// 出库数量
        /// </summary>
        //[Required(ErrorMessage = "出库数量必填")]
        public decimal? Quantity
        {
            get { return quantity; }
            set { Set(ref quantity, value); }
        }
        #endregion

        #region 配盘数量
        private decimal? palletQuantity;

        /// <summary>
        /// 配盘数量
        /// </summary>
        public decimal? PalletQuantity
        {
            get { return palletQuantity; }
            set { Set(ref palletQuantity, value); }
        }
        #endregion


        #region 托盘ID
        private Guid? palletid;

        /// <summary>
        /// 托盘ID
        /// </summary>
        public Guid? PalletID
        {
            get { return palletid; }
            set { Set(ref palletid, value); }
        }
        #endregion

        #region 托盘编号
        private string palletCode;

        /// <summary>
        /// 托盘编号
        /// </summary>
        public string PalletCode
        {
            get { return palletCode; }
            set { Set(ref palletCode, value); }
        }

        #endregion

        #region 托盘名称
        private string palletName;

        /// <summary>
        /// 托盘名称
        /// </summary>
        public string PalletName
        {
            get { return palletName; }
            set { Set(ref palletName, value); }
        }
        #endregion

        #region 出库时间
        private DateTime? outstorageTime;

        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime? OutStorageTime
        {
            get { return outstorageTime; }
            set { Set(ref outstorageTime, value); }
        }
        #endregion

        #region 出库完成时间
        private DateTime? finishTime;

        /// <summary>
        /// 出库完成时间
        /// </summary>
        public DateTime? FinishTime
        {
            get { return finishTime; }
            set { Set(ref finishTime, value); }
        }
        #endregion

        #region 出库状态
        private OutStorageStatus outstorageStatus;

        /// <summary>
        /// 出库状态
        /// </summary>
        public OutStorageStatus OutStorageStatus
        {
            get { return outstorageStatus; }
            set { Set(ref outstorageStatus, value); }
        }
        #endregion

        #region 制单人
        private string createUser;

        /// <summary>
        /// 制单人
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string CreateUser
        {
            get { return createUser; }
            set { Set(ref createUser, value); }
        }
        #endregion

        #region 审核人
        private string auditPerson;

        /// <summary>
        /// 审核人
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string AuditPerson
        {
            get { return auditPerson; }
            set { Set(ref auditPerson, value); }
        }
        #endregion

        #region 审核状态
        private AuditStatus auditStatus;

        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditStatus AuditStatus
        {
            get { return auditStatus; }
            set { Set(ref auditStatus, value); }
        }
        #endregion

        #region 描述
        private string description;

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度大于100个字符")]
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
        [MaxLength(200, ErrorMessage = "长度大于200个字符")]
        public string Remark
        {
            get { return remark; }
            set { Set(ref remark, value); }
        }
        #endregion

        #region 库位批次信息列表
        private ObservableCollection<MaterialBatchInfoModel> materialBatchs = new ObservableCollection<MaterialBatchInfoModel>();

        /// <summary>
        /// 库位批次信息数据
        /// </summary>
        public ObservableCollection<MaterialBatchInfoModel> MaterialBatchs
        {
            get { return materialBatchs; }
            set { Set(ref materialBatchs, value); }
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


        #region 库位ID
        private Guid? warehouselocationID;

        /// <summary>
        /// 库位ID
        /// </summary>
        public Guid? WareHouseLocationID
        {
            get { return warehouselocationID; }
            set { Set(ref warehouselocationID, value); }
        }
        #endregion

        #region 库位编号
        private string warehouselocationCode;

        /// <summary>
        /// 库位编号
        /// </summary>
        public string WareHouseLocationCode
        {
            get { return warehouselocationCode; }
            set { Set(ref warehouselocationCode, value); }
        }

        #endregion

        #region 库位名称
        private string warehouselocationName;

        /// <summary>
        /// 库位名称
        /// </summary>
        public string WareHouseLocationName
        {
            get { return warehouselocationName; }
            set { Set(ref warehouselocationName, value); }
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
