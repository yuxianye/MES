using Solution.Desktop.Core;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.MatWareHouseInfo.Model.InventoryTypeEnumModel;

namespace Solution.Desktop.MatInventoryInfo.Model
{
    /// <summary>
    /// 盘点信息模型
    /// </summary>
    public class MatInventoryInfoModel : ModelBase, IAudited
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

        #region 盘点编号
        private string inventoryCode;

        /// <summary>
        /// 盘点编号
        /// </summary>
        [Required(ErrorMessage = "盘点编号必填"), MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string InventoryCode
        {
            get { return inventoryCode; }
            set { Set(ref inventoryCode, value); }
        }

        #endregion

        #region 盘点类型
        private InventoryType inventoryType;

        /// <summary>
        /// 盘点类型
        /// </summary>
        [Required(ErrorMessage = "盘点类型必填")]
        public InventoryType InventoryType
        {
            get { return inventoryType; }
            set { Set(ref inventoryType, value); }
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


        #region 操作人员
        private string operatorPerson;

        /// <summary>
        /// 操作人员
        /// </summary>
        [MaxLength(50, ErrorMessage = "长度大于50个字符")]
        public string Operator
        {
            get { return operatorPerson; }
            set { Set(ref operatorPerson, value); }
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


        #region 明细数量
        private int? itemcount;

        /// <summary>
        /// 明细数量
        /// </summary>
        public int? ItemCount
        {
            get { return itemcount; }
            set { Set(ref itemcount, value); }
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
