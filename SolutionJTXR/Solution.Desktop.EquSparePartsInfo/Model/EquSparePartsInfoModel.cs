using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.EquSparePartsInfo.Model.SparePartUnitEnum;

namespace Solution.Desktop.EquSparePartsInfo.Model
{
    /// <summary>
    /// 设备备件信息模型
    /// </summary>
    public class EquSparePartsModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 设备ID
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 备件类别ID
        private Guid equSparePartTypeId;
        public Guid EquSparePartType_Id//Equipmenttype_Id
        {
            get { return equSparePartTypeId; }
            set { Set(ref equSparePartTypeId, value); }
        }
        #endregion

        #region 设备类别名称
        public string equSparePartTypeName;
        public string EquSparePartTypeName
        {
            get { return equSparePartTypeName; }
            set { Set(ref equSparePartTypeName, value); }
        }
        #endregion    

        #region 备件名称
        private string sparePartName;

        /// <summary>
        /// 备件名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string SparePartName
        {
            get { return sparePartName; }
            set { Set(ref sparePartName, value); }
        }

        #endregion

        #region 备件编号
        private string sparePartCode;

        /// <summary>
        /// 备件编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string SparePartCode
        {
            get { return sparePartCode; }
            set { Set(ref sparePartCode, value); }
        }

        #endregion

        #region 规格
        private string specifications;

        /// <summary>
        /// 规格
        /// </summary>
        public string Specifications
        {
            get { return specifications; }
            set { Set(ref specifications, value); }
        }
        #endregion

        #region 型号
        private string modelNumber;

        /// <summary>
        /// 型号
        /// </summary>
        public string ModelNumber
        {
            get { return modelNumber; }
            set { Set(ref modelNumber, value); }
        }
        #endregion

        #region 数量
        private int quantity;

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set { Set(ref quantity, value); }
        }
        #endregion

        /// <summary>
        ///数量单位
        /// </summary>
        //EquRunningStateTypes
        #region 数量单位
        private SparePartUnit sparePartUnit;

        /// <summary>
        /// 状态类型
        /// </summary>
        [Required(ErrorMessage = "数量单位类型必填")]
        public SparePartUnit SparePartUnit
        {
            get { return sparePartUnit; }
            set { Set(ref sparePartUnit, value); }
        }
        #endregion

        #region 单价
        private string price;

        /// <summary>
        /// 单价
        /// </summary>
        public string Price
        {
            get { return price; }
            set { Set(ref price, value); }
        }
        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度小于200个字符")]
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

        //#region 是否逻辑删除
        ////private bool isDeleted;

        /////// <summary>
        /////// 是否逻辑删除
        /////// </summary>
        ////public bool IsDeleted
        ////{
        ////    get { return isDeleted; }
        ////    set { Set(ref isDeleted, value); }
        ////}
        //#endregion

        protected override void Disposing()
        {
            EquSparePartTypeName = null;
            SparePartName = null;
            SparePartCode = null;
            Specifications = null;
            ModelNumber = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
