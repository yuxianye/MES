using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.ProductInfo.Model
{
    /// <summary>
    /// 产品模型
    /// </summary>
    public class ProductInfoModel : ModelBase, IAudited
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
        #region 产品名称
        private string productName;

        /// <summary>
        /// 产品名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string ProductName
        {
            get { return productName; }
            set { Set(ref productName, value); }
        }

        #endregion

        #region 产品编号
        private string productCode;

        /// <summary>
        /// 产品编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]

        public string ProductCode
        {
            get { return productCode; }
            set { Set(ref productCode, value); }
        }

        #endregion

        #region 规格型号
        private string specification;

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return specification; }
            set { Set(ref specification, value); }
        }
        #endregion

        #region 产品类别Id
        private Guid productType_Id;

        /// <summary>
        /// 产品类别Id
        /// </summary>
        public Guid ProductType_Id
        {
            get { return productType_Id; }
            set { Set(ref productType_Id, value); }
        }
        #endregion

        #region 规格型号
        private string productTypeName;

        /// <summary>
        /// 产品类别名称
        /// </summary>
        public string ProductTypeName
        {
            get { return productTypeName; }
            set { Set(ref productTypeName, value); }
        }
        #endregion
        #region 规格型号
        private string productTypeCode;

        /// <summary>
        /// 产品类别编号
        /// </summary>
        public string ProductTypeCode
        {
            get { return productTypeCode; }
            set { Set(ref productTypeCode, value); }
        }
        #endregion

        #region 产品描述
        private string description;

        /// <summary>
        /// 产品描述
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

        //#region 是否锁定
        //private bool isLocked;

        ///// <summary>
        ///// 是否锁定
        ///// </summary>
        //public bool IsLocked
        //{
        //    get { return isLocked; }
        //    set { Set(ref isLocked, value); }
        //}
        //#endregion

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

        //#region 是否逻辑删除
        //private bool isDeleted;

        ///// <summary>
        ///// 是否逻辑删除
        ///// </summary>
        //public bool IsDeleted
        //{
        //    get { return isDeleted; }
        //    set { Set(ref isDeleted, value); }
        //}
        //#endregion

        protected override void Disposing()
        {
            ProductCode = null;
            ProductName = null;
            ProductType_Id = Guid.Empty;
            Description = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
