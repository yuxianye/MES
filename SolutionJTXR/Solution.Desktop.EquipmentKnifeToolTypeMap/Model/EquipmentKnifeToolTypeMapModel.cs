using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquipmentKnifeToolTypeMap.Model
{
    /// <summary>
    /// 设备信息模型
    /// </summary>
    public class EquipmentKnifeToolTypeMapModel : ModelBase, IAudited
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

        #region 刀具类别ID
        public Guid KnifeToolTypeInfoId;
        [Required(ErrorMessage = "必选！")]
        public Guid KnifeToolTypeInfo_Id//Equipmenttype_Id
        {
            get { return KnifeToolTypeInfoId; }
            set { Set(ref KnifeToolTypeInfoId, value); }
        }
        #endregion
        #region 设备ID
        public Guid EquipmentInfoId;
        [Required(ErrorMessage = "必选！")]
        public Guid EquipmentInfo_Id//Entproductionline_Id
        {
            get { return EquipmentInfoId; }
            set { Set(ref EquipmentInfoId, value); }
        }
        #endregion

        #region 刀具类别名称
        public string knifeToolTypeName;
        public string KnifeToolTypeName
        {
            get { return knifeToolTypeName; }
            set { Set(ref knifeToolTypeName, value); }
        }
        #endregion
      
        #region 设备名称
        private string equipmentName;

        public string EquipmentName
        {
            get { return equipmentName; }
            set { Set(ref equipmentName, value); }
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
            EquipmentName = null;
            KnifeToolTypeName = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
            Remark = null;
        }

    }

}
