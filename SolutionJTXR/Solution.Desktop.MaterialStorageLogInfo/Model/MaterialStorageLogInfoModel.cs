using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.AuditStatusEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.InStorageStatusEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.InStorageTypeEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialTypeEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialUnitEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.StorageChangeTypeEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.WareHouseLocationTypeEnumModel;

namespace Solution.Desktop.MaterialStorageLogInfo.Model
{
    /// <summary>
    /// 物料库存日志模型
    /// </summary>
    public class MaterialStorageLogInfoModel : ModelBase, IAudited
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

        #region 物料ID
        private Guid? material_ID;

        /// <summary>
        /// 物料ID
        /// </summary>
        public Guid? Material_ID
        {
            get { return material_ID; }
            set { Set(ref material_ID, value); }
        }
        #endregion

        #region 物料编号
        private string materialCode;

        /// <summary>
        /// 物料编号
        /// </summary>
        [Required(ErrorMessage = "物料编号必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]

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
        [Required(ErrorMessage = "物料名称必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
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
        [Required(ErrorMessage = "物料类型必填")]
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
        [Required(ErrorMessage = "物料单位必填")]
        public MaterialUnit MaterialUnit
        {
            get { return materialUnit; }
            set { Set(ref materialUnit, value); }
        }
        #endregion

        #region 物料批次ID
        private Guid? materialbatch_ID;

        /// <summary>
        /// 物料批次ID
        /// </summary>
        public Guid? MaterialBatch_ID
        {
            get { return materialbatch_ID; }
            set { Set(ref materialbatch_ID, value); }
        }
        #endregion

        #region 批次编号
        private string batchCode;

        /// <summary>
        /// 批次编号
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string BatchCode
        {
            get { return batchCode; }
            set { Set(ref batchCode, value); }
        }

        #endregion


        #region 入库单号
        private string instoragebillCode;

        /// <summary>
        /// 入库单号
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string InStorageBillCode
        {
            get { return instoragebillCode; }
            set { Set(ref instoragebillCode, value); }
        }

        #endregion


        #region 出库单号
        private string outstoragebillCode;

        /// <summary>
        /// 出库单号
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string OutStorageBillCode
        {
            get { return outstoragebillCode; }
            set { Set(ref outstoragebillCode, value); }
        }

        #endregion


        #region 库位改变类型
        private StorageChangeType storagechangeType;

        /// <summary>
        /// 库位改变类型
        /// </summary>
        public StorageChangeType StorageChangeType
        {
            get { return storagechangeType; }
            set { Set(ref storagechangeType, value); }
        }
        #endregion        

        #region 入库ID
        private Guid? instorageID;

        /// <summary>
        /// 入库ID
        /// </summary>
        public Guid? InStorageID
        {
            get { return instorageID; }
            set { Set(ref instorageID, value); }
        }
        #endregion

        #region 出库ID
        private Guid? outstorageID;

        /// <summary>
        /// 出库ID
        /// </summary>
        public Guid? OutStorageID
        {
            get { return outstorageID; }
            set { Set(ref outstorageID, value); }
        }
        #endregion

        #region 改变数量
        private decimal? changedAmount;

        /// <summary>
        /// 改变数量
        /// </summary>
        public decimal? ChangedAmount
        {
            get { return changedAmount; }
            set { Set(ref changedAmount, value); }
        }
        #endregion

        #region 原数量
        private decimal? originalAmount;

        /// <summary>
        /// 原数量
        /// </summary>
        public decimal? OriginalAmount
        {
            get { return originalAmount; }
            set { Set(ref originalAmount, value); }
        }
        #endregion

        #region 当前数量
        private decimal? currentAmount;

        /// <summary>
        /// 当前数量
        /// </summary>
        public decimal? CurrentAmount
        {
            get { return currentAmount; }
            set { Set(ref currentAmount, value); }
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

        #region 所属仓库ID
        private Guid matwarehouse_Id;
        public Guid MatWareHouse_Id
        {
            get { return matwarehouse_Id; }
            set { Set(ref matwarehouse_Id, value); }
        }
        #endregion

        #region 所属仓库编号
        private string warehouseCode;

        // <summary>
        // 所属仓库编号
        // </summary>
        public string WareHouseCode
        {
            get { return warehouseCode; }
            set { Set(ref warehouseCode, value); }
        }
        #endregion

        #region 所属仓库名称
        private string warehouseName;

        // <summary>
        // 所属仓库名称
        // </summary>
        public string WareHouseName
        {
            get { return warehouseName; }
            set { Set(ref warehouseName, value); }
        }
        #endregion      

        /// ////////////////////////////////////////////////////////

        #region 仓库区域编号
        private string wareHouseareaCode;

        /// <summary>
        /// 仓库区域编号
        /// </summary>
        [Required(ErrorMessage = "仓库区域编号必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]

        public string WareHouseAreaCode
        {
            get { return wareHouseareaCode; }
            set { Set(ref wareHouseareaCode, value); }
        }

        #endregion

        #region 仓库区域名称
        private string wareHouseareaName;

        /// <summary>
        /// 仓库区域名称
        /// </summary>
        [Required(ErrorMessage = "仓库区域名称必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string WareHouseAreaName
        {
            get { return wareHouseareaName; }
            set { Set(ref wareHouseareaName, value); }
        }
        #endregion

        #region 库位类型
        private WareHouseLocationType warehouselocationType;

        /// <summary>
        /// 库位类型
        /// </summary>
        public WareHouseLocationType WareHouseLocationType
        {
            get { return warehouselocationType; }
            set { Set(ref warehouselocationType, value); }
        }
        #endregion

        #region 库位ID
        private Guid? matwarehouselocation_ID;

        /// <summary>
        /// 库位ID
        /// </summary>
        public Guid? MatWareHouseLocation_ID
        {
            get { return matwarehouselocation_ID; }
            set { Set(ref matwarehouselocation_ID, value); }
        }
        #endregion

        #region 库位编号
        private string warehouselocationCode;

        /// <summary>
        /// 库位编号
        /// </summary>
        [Required(ErrorMessage = "库位编号必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]

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
        [Required(ErrorMessage = "库位名称必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
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
