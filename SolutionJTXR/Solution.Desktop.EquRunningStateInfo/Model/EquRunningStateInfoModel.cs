using Solution.Desktop.Core;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using static Solution.Desktop.EquRunningStateInfo.Model.RunningStateTypeEnum;

namespace Solution.Desktop.EquRunningStateInfo.Model
{
    /// <summary>
    /// 设备运行信息模型
    /// </summary>
    public class EquRunningStateInfoModel : ModelBase, IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 设备运行ID
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 设备ID
        private Guid EquipmentInfoId;
        public Guid EquipmentInfo_Id//Equipmenttype_Id
        {
            get { return EquipmentInfoId; }
            set { Set(ref EquipmentInfoId, value); }
        }
        #endregion

        #region 设备名称
        private string EquipmentinfoName;
        public string EquipmentName
        {
            get { return EquipmentinfoName; }
            set { Set(ref EquipmentinfoName, value); }
        }
        #endregion
        #region 设备编号
        private string equipmentCode;

        public string EquipmentCode
        {
            get { return equipmentCode; }
            set { Set(ref equipmentCode, value); }
        }
        #endregion
        /// <summary>
        /// 设备运行运行状态
        /// </summary>
        //EquRunningStateTypes
        #region 设备运行状态
        private RunningStateType runningStateType;

        /// <summary>
        /// 状态类型
        /// </summary>
        [Required(ErrorMessage = "状态类型必填")]
        public RunningStateType EquRunningStateTypes
        {
            get { return runningStateType; }
            set { Set(ref runningStateType, value); }
        }
        #endregion

        /// //采集状态时间
        private DateTime? runningStateTime;
        /// <summary>
        ///  采集状态时间
        /// </summary>
        public DateTime? RunningStateTime
        {
            get { return runningStateTime; }
            set { Set(ref runningStateTime, value); }
        }
        /// <summary>
        /// 故障信息
        /// </summary>
        private string faultInfo;
        public string FaultInfo
        {
            get { return faultInfo; }
            set { Set(ref faultInfo, value); }
        }
        /// <summary>
        /////故障代码
        /// </summary>
        #region 故障代码
        private string faultCode;
        /// <summary>
        ///故障代码
        /// </summary>
        [Required(ErrorMessage = "故障代码必填"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string FaultCode
        {
            get { return faultCode; }
            set { Set(ref faultCode, value); }
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
            //EquRunningStateTypes = null;
            FaultInfo = null;
            FaultCode = null;

            Remark = null;

            CreatorUserId = null;
            LastUpdatorUserId = null;
        }

    }

}
