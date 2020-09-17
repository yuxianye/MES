using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.WareHouseLocationStatusEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.WareHouseLocationTypeEnumModel;

namespace Solution.Desktop.MatWareHouseLocationInfo.Model
{
    /// <summary>
    /// 库位模型
    /// </summary>
    public class MatWareHouseLocationInfoModel : ModelBase, IAudited
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

        /// ////////////////////////////////////////////////////////

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

        #region 所属货区ID
        private Guid matwarehousearea_Id;
        public Guid MatWareHouseArea_Id
        {
            get { return matwarehousearea_Id; }
            set { Set(ref matwarehousearea_Id, value); }
        }
        #endregion

        #region 所属货区编号
        private string warehouseareaCode;

        // <summary>
        // 所属货区编号
        // </summary>
        public string WareHouseAreaCode
        {
            get { return warehouseareaCode; }
            set { Set(ref warehouseareaCode, value); }
        }
        #endregion

        #region 所属货区名称
        private string warehouseareaName;

        // <summary>
        // 所属货区名称
        // </summary>
        public string WareHouseAreaName
        {
            get { return warehouseareaName; }
            set { Set(ref warehouseareaName, value); }
        }
        #endregion      

        /// ////////////////////////////////////////////////////////

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
        private Guid? palletID;

        public Guid? PalletID
        {
            get { return palletID; }
            set { Set(ref palletID, value); }
        }
        #endregion

        #region 托盘编号
        private string palletCode;

        /// <summary>
        /// 托盘编号
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
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
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string PalletName
        {
            get { return palletName; }
            set { Set(ref palletName, value); }
        }
        #endregion

        #region 物料信息
        private string materialName;

        /// <summary>
        /// 物料信息
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string MaterialName
        {
            get { return materialName; }
            set { Set(ref materialName, value); }
        }
        #endregion

        #region 入库信息
        private string instorageName;

        /// <summary>
        /// 入库信息
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string InStorageName
        {
            get { return instorageName; }
            set { Set(ref instorageName, value); }
        }
        #endregion

        #region 物料批次信息
        private string materialbatchName;

        /// <summary>
        /// 物料批次信息
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string MaterialBatchName
        {
            get { return materialbatchName; }
            set { Set(ref materialbatchName, value); }
        }
        #endregion

        #region 库位存放物料数量
        private decimal? storageQuantity;

        /// <summary>
        /// 库位存放物料数量
        /// </summary>
        public decimal? StorageQuantity
        {
            get { return storageQuantity; }
            set { Set(ref storageQuantity, value); }
        }
        #endregion


        #region 是否启用
        private bool isUse;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsUse
        {
            get { return isUse; }
            set { Set(ref isUse, value); }
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
        #endregion

        //public event PropertyChangedEventHandler PropertyChanged; 

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
            WareHouseLocationCode = null;
            WareHouseLocationName = null;
            Remark = null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
