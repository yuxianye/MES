using Solution.Desktop.Core;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialTypeEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialUnitEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.WareHouseLocationStatusEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.WareHouseLocationTypeEnumModel;

namespace Solution.Desktop.MaterialBatchInfo.Model
{
    /// <summary>
    /// 物料批次模型
    /// </summary>
    public class MaterialBatchInfoModel : ModelBase, IAudited
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

        #region 供应商ID
        private Guid? matsupplierID;

        /// <summary>
        /// 供应商ID
        /// </summary>
        public Guid? MatSupplierID
        {
            get { return matsupplierID; }
            set { Set(ref matsupplierID, value); }
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

        #region 入库ID
        private Guid? materialinstorage_ID;

        /// <summary>
        /// 入库ID
        /// </summary>
        public Guid? MaterialInStorage_ID
        {
            get { return materialinstorage_ID; }
            set { Set(ref materialinstorage_ID, value); }
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

        #region 描述
        private string description;

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度大于200个字符")]
        public string Description
        {
            get { return description; }
            set { Set(ref description, value); }
        }
        #endregion

        #region 数量
        private decimal? quantity;

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Quantity
        {
            get { return quantity; }
            set { Set(ref quantity, value); }
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

        #region 库位状态
        private WareHouseLocationStatus warehouselocationStatus;

        /// <summary>
        /// 库位状态
        /// </summary>
        public WareHouseLocationStatus WareHouseLocationStatus
        {
            get { return warehouselocationStatus; }
            set { Set(ref warehouselocationStatus, value); }
        }
        #endregion
        #region 托盘ID
        private Guid? pallet_Id;
        public Guid? Pallet_Id
        {
            get { return pallet_Id; }
            set { Set(ref pallet_Id, value); }
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

        #region 是否被选中

        private bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                //Set(ref _isChecked, value);
                _isChecked = value;
                if (PropertyChangedHandler != null)
                    PropertyChangedHandler(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

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
