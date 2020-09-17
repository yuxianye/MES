using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.MatPalletInfo.Model
{
    /// <summary>
    /// 托盘模型
    /// </summary>
    public class MatPalletInfoModel : ModelBase, IAudited
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

        #region 托盘编号
        private string palletCode;

        /// <summary>
        /// 托盘编号
        /// </summary>
        [Required(ErrorMessage = "托盘编号必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
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
        [Required(ErrorMessage = "托盘名称必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string PalletName
        {
            get { return palletName; }
            set { Set(ref palletName, value); }
        }
        #endregion

        #region 托盘最大承重
        private decimal? palletMaxWeight;

        /// <summary>
        /// 托盘最大承重
        /// </summary>
        public decimal? PalletMaxWeight
        {
            get { return palletMaxWeight; }
            set { Set(ref palletMaxWeight, value); }
        }
        #endregion

        #region 托盘规格
        private string palletSpecifications;

        /// <summary>
        /// 托盘规格
        /// </summary>
        [MaxLength(100, ErrorMessage = "长度大于100个字符")]
        public string PalletSpecifications
        {
            get { return palletSpecifications; }
            set { Set(ref palletSpecifications, value); }
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


        #region 入库单编制数量
        private decimal? materialinstorageQuantity;

        /// <summary>
        /// 入库单编制数量
        /// </summary>
        public decimal? MaterialInStorageQuantity
        {
            get { return materialinstorageQuantity; }
            set { Set(ref materialinstorageQuantity, value); }
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
            PalletCode = null;
            PalletName = null;
            Remark = null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
