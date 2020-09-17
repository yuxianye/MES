using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialUnitEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.WareHouseLocationTypeEnumModel;

namespace Solution.Desktop.MatWareHousAreaLocationInfo.Model
{
    /// <summary>
    /// 仓位图模型
    /// </summary>
    public class MatWareHousAreaLocationItemInfoModel : ModelBase, IAudited
    {
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

        #region 入库数量
        private decimal? quantity;

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal? Quantity
        {
            get { return quantity; }
            set { Set(ref quantity, value); }
        }
        #endregion


        #region 入库数量
        private decimal? fullpalletquantity;

        /// <summary>
        /// 入库数量
        /// </summary>
        public decimal? FullPalletQuantity
        {
            get { return fullpalletquantity; }
            set { Set(ref fullpalletquantity, value); }
        }
        #endregion

        #region 入库单号
        private string instoragebillCode;

        /// <summary>
        /// 入库单号
        /// </summary>
        public string InStorageBillCode
        {
            get { return instoragebillCode; }
            set { Set(ref instoragebillCode, value); }
        }

        #endregion


        #region 入库单号
        private string batchcode;

        /// <summary>
        /// 入库单号
        /// </summary>
        public string BatchCode
        {
            get { return batchcode; }
            set { Set(ref batchcode, value); }
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
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
