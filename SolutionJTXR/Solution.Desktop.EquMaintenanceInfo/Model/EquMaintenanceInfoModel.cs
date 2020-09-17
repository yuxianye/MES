using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquMaintenanceInfo.Model
{
    /// <summary>
    /// 设备维护信息模型
    /// </summary>
    public class EquMaintenanceModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 设备维护ID
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 设备维护ID
        public Guid EquipmentInfoId;
        [Required(ErrorMessage = "必选！")]
        public Guid EquipmentInfo_Id//Equipmenttype_Id
        {
            get { return EquipmentInfoId; }
            set { Set(ref EquipmentInfoId, value); }
        }
        #endregion

        #region 设备维护类别名称
        public string EquipmentinfoName;
        public string EquipmentName
        {
            get { return EquipmentinfoName; }
            set { Set(ref EquipmentinfoName, value); }
        }
        #endregion    

        #region 设备维护问题
        private string equipmentProblem;

        /// <summary>
        /// 设备维护存在问题
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string EquipmentProblem
        {
            get { return equipmentProblem; }
            set { Set(ref equipmentProblem, value); }
        }

        #endregion

        #region 存在问题的原因
        private string cause;

        /// <summary>
        /// 设备维护存在问题的原因
        /// </summary>

        public string Cause
        {
            get { return cause; }
            set { Set(ref cause, value); }
        }

        #endregion

        #region 解决方案
        private string correction;

        /// <summary>
        /// 解决方案
        /// </summary>
        public string Correction
        {
            get { return correction; }
            set { Set(ref correction, value); }
        }
        #endregion

        #region 维护实际开始时间
        private DateTime? actualStartTime;
        /// <summary>
        ///   维护实际开始时间
        /// </summary>
        public DateTime? ActualStartTime
        {
            get { return actualStartTime; }
            set { Set(ref actualStartTime, value); }
        }
        #endregion

        #region 维护实际开始时间
        private DateTime? actualFinishTime;

        /// <summary>
        ///  维护实际完成时间
        /// </summary>
        public DateTime? ActualFinishTime
        {
            get { return actualFinishTime; }
            set { Set(ref actualFinishTime, value); }
        }
        #endregion

        #region 维护消耗资源
        private string consumable;

        /// <summary>
        /// 维护消耗资源
        /// </summary>
        public string Consumable
        {
            get { return consumable; }
            set { Set(ref consumable, value); }
        }
        #endregion

        #region 维护负责人
        private string responsiblePerson;

        /// <summary>
        /// 负责人
        /// </summary>
        public string ResponsiblePerson
        {
            get { return responsiblePerson; }
            set { Set(ref responsiblePerson, value); }
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
            EquipmentinfoName = null;
            EquipmentProblem = null;
            Cause = null;
            Correction = null;
            Consumable = null;
            Remark = null;
            ResponsiblePerson = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
