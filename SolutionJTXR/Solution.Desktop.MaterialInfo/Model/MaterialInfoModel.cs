using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialTypeEnumModel;
using static Solution.Desktop.MatWareHouseInfo.Model.MaterialUnitEnumModel;

namespace Solution.Desktop.MaterialInfo.Model
{
    /// <summary>
    /// 物料模型
    /// </summary>
    public class MaterialInfoModel : ModelBase, IAudited
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

        /// ////////////////////////////////////////////////////////////

        #region 满盘数量
        private int? fullpalletQuantity;

        /// <summary>
        /// 满盘数量
        /// </summary>
        [Required(ErrorMessage = "满盘数量必填")]
        public int? FullPalletQuantity
        {
            get { return fullpalletQuantity; }
            set { Set(ref fullpalletQuantity, value); }
        }
        #endregion

        #region 规格型号
        private string specification;

        /// <summary>
        /// 规格型号
        /// </summary>
        [Required(ErrorMessage = "规格型号必填"), MaxLength(200, ErrorMessage = "长度大于50个字符")]
        public string Specification
        {
            get { return specification; }
            set { Set(ref specification, value); }
        }
        #endregion

        #region 单位重量
        private decimal? unitWeight;

        /// <summary>
        /// 单位重量
        /// </summary>
        public decimal? UnitWeight
        {
            get { return unitWeight; }
            set { Set(ref unitWeight, value); }
        }
        #endregion

        #region 当前库存
        private decimal? currentStock;

        /// <summary>
        /// 当前库存
        /// </summary>
        public decimal? CurrentStock
        {
            get { return currentStock; }
            set { Set(ref currentStock, value); }
        }
        #endregion

        #region 最大库存
        private decimal? maxStock;

        /// <summary>
        /// 最大库存
        /// </summary>
        public decimal? MaxStock
        {
            get { return maxStock; }
            set { Set(ref maxStock, value); }
        }
        #endregion

        #region 最小库存
        private decimal? minStock;

        /// <summary>
        /// 最小库存
        /// </summary>
        public decimal? MinStock
        {
            get { return minStock; }
            set { Set(ref minStock, value); }
        }
        #endregion

        #region 物料描述
        private string description;

        /// <summary>
        /// 物料描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度大于200个字符")]
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
            MaterialCode= null;
            MaterialName= null;
            Specification= null;
            Description= null;
            Remark= null;
            //
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }
    }
}
