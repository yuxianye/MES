using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialUnitEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.WareHouseLocationTypeEnumModel;

namespace Solution.Desktop.MatWareHousAreaLocationInfo.Model
{
    /// <summary>
    /// 仓位图模型
    /// </summary>
    public class MatWareHousAreaLocationInfoModel : ModelBase, IAudited
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

        #region 货架层数
        private int? layerNumberQuantity;

        /// <summary>
        /// 货架层数
        /// </summary>
        public int? LayerNumberQuantity
        {
            get { return layerNumberQuantity; }
            set { Set(ref layerNumberQuantity, value); }
        }
        #endregion

        #region 货架列数
        private int? columnNumberQuantity;

        /// <summary>
        /// 货架列数
        /// </summary>
        public int? ColumnNumberQuantity
        {
            get { return columnNumberQuantity; }
            set { Set(ref columnNumberQuantity, value); }
        }
        #endregion

        #region 库位总数
        private int? locationQuantity;

        /// <summary>
        /// 库位总数
        /// </summary>
        public int? LocationQuantity
        {
            get { return locationQuantity; }
            set { Set(ref locationQuantity, value); }
        }
        #endregion

        #region 层数信息
        private string layerNumber;

        /// <summary>
        /// 层数信息
        /// </summary>
        public string LayerNumber
        {
            get { return layerNumber; }
            set { Set(ref layerNumber, value); }
        }
        #endregion


        #region 列数信息
        private int columnNumber;

        /// <summary>
        /// 列数信息
        /// </summary>
        public int ColumnNumber
        {
            get { return columnNumber; }
            set { Set(ref columnNumber, value); }
        }
        #endregion

        #region 列信息
        private List<string> warehousecolumns;

        /// <summary>
        /// 列信息
        /// </summary>
        public List<string> WareHouseColumns
        {
            get { return warehousecolumns; }
            set { Set(ref warehousecolumns, value); }
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
            WareHouseCode = null;
            WareHouseName = null;
            Remark = null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
