using Solution.Desktop.Core;
using Solution.Desktop.EntDepartmentInfo.Model;
using Solution.Desktop.EquipmentInfo.Model;
using Solution.Utility;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EquMaintenancePlanInfo.Model
{
    /// <summary>
    /// 设备类别信息模型
    /// </summary>
    public class EquMaintenancePlanInfoModel : ModelBase, /*ILockable, IRecyclable,*/ IAudited
    {
        #region Id
        private Guid id;

        /// <summary>
        /// 设备类型ID
        /// </summary>
        [DisplayName("ID")]
        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 设备名称
        private string equipmentName;

        /// <summary>
        /// 设备名称
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string EquipmentName
        {
            get { return equipmentName; }
            set { Set(ref equipmentName, value); }
        }

        #endregion

        #region 设备编号
        private string equipmentInfo_Id;

        /// <summary>
        /// 设备编号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string EquipmentInfo_Id
        {
            get { return equipmentInfo_Id; }
            set { Set(ref equipmentInfo_Id, value); }
        }
        #endregion

        #region 设备
        private EquipmentInfoModel equipmentInfo;

        /// <summary>
        /// 设备
        /// </summary>
        public EquipmentInfoModel EquipmentInfo
        {
            get { return equipmentInfo; }
            set { Set(ref equipmentInfo, value); }
        }
        #endregion

        #region 维护计划单号
        private string maintenanceCode;

        /// <summary>
        /// 维护计划单号
        /// </summary>
        [Required(ErrorMessage = "必填项，唯一，限50个字符"), MaxLength(50, ErrorMessage = "长度小于50个字符")]
        public string MaintenanceCode
        {
            get { return maintenanceCode; }
            set { Set(ref maintenanceCode, value); }
        }

        #endregion

        #region 维护类别(保养,点检,检修)
        private MaintenanceType maintenanceType;

        /// <summary>
        /// 维护类别(保养,点检,检修)
        /// </summary>
        public MaintenanceType MaintenanceType
        {
            get { return maintenanceType; }
            set { Set(ref maintenanceType, value); }
        }

        #endregion

        #region 计划制定人
        private string maker;

        /// <summary>
        /// 计划制定人
        /// </summary>
        public string Maker
        {
            get { return maker; }
            set { Set(ref maker, value); }
        }

        #endregion

        #region 计划停机时长(小时)
        private double planStopDuration;

        /// <summary>
        /// 计划停机时长(小时)
        /// </summary>
        public double PlanStopDuration
        {
            get { return planStopDuration; }
            set { Set(ref planStopDuration, value); }
        }

        #endregion

        #region 计划开始时间
        private DateTime? planStartTime;

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanStartTime
        {
            get { return planStartTime; }
            set { Set(ref planStartTime, value); }
        }

        #endregion

        #region 计划完成时间
        private DateTime? planFinishTime;

        /// <summary>
        /// 计划完成时间
        /// </summary>
        public DateTime? PlanFinishTime
        {
            get { return planFinishTime; }
            set { Set(ref planFinishTime, value); }
        }

        #endregion

        #region 维护实际开始时间
        private DateTime? actualStartTime;

        /// <summary>
        /// 维护实际开始时间
        /// </summary>
        public DateTime? ActualStartTime
        {
            get { return actualStartTime; }
            set { Set(ref actualStartTime, value); }
        }

        #endregion

        #region 维护实际完成时间
        private DateTime? actualFinishTime;

        /// <summary>
        /// 维护实际完成时间
        /// </summary>
        public DateTime? ActualFinishTime
        {
            get { return actualFinishTime; }
            set { Set(ref actualFinishTime, value); }
        }

        #endregion

        #region 维护内容
        private string maintenanceContent;

        /// <summary>
        /// 维护内容
        /// </summary>
        public string MaintenanceContent
        {
            get { return maintenanceContent; }
            set { Set(ref maintenanceContent, value); }
        }

        #endregion

        #region 承担班组ID
        private string undertakeTeamID;

        /// <summary>
        /// 承担班组ID
        /// </summary>
        public string UndertakeTeamID
        {
            get { return undertakeTeamID; }
            set { Set(ref undertakeTeamID, value); }
        }

        #endregion

        #region 结论
        private string result;

        /// <summary>
        /// 结论
        /// </summary>
        public string Result
        {
            get { return result; }
            set { Set(ref result, value); }
        }

        #endregion

        #region 验收部门ID
        private string checkDepartmentID;

        /// <summary>
        /// 验收部门ID
        /// </summary>
        public string CheckDepartmentID
        {
            get { return checkDepartmentID; }
            set { Set(ref checkDepartmentID, value); }
        }

        #endregion

        #region 部门名称
        private string departmentInfo_Name;

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentInfo_Name
        {
            get { return departmentInfo_Name; }
            set { Set(ref departmentInfo_Name, value); }
        }
        #endregion

        #region 部门
        private EntDepartmentInfoModel entDepartmentInfo;

        /// <summary>
        /// 部门
        /// </summary>
        public EntDepartmentInfoModel CheckDepartment
        {
            get { return entDepartmentInfo; }
            set { Set(ref entDepartmentInfo, value); }
        }
        #endregion

        #region 维护人员
        private string maintenancePerson;

        /// <summary>
        /// 维护人员
        /// </summary>
        public string MaintenancePerson
        {
            get { return maintenancePerson; }
            set { Set(ref maintenancePerson, value); }
        }

        #endregion

        #region 维护计划状态(未开始,已完成)
        private MaintenancePlanState maintenancePlanState;

        /// <summary>
        /// 维护计划状态(未开始,已完成)
        /// </summary>
        public MaintenancePlanState MaintenancePlanState
        {
            get { return maintenancePlanState; }
            set { Set(ref maintenancePlanState, value); }
        }

        #endregion

        #region 备注
        private string remark;

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "长度小于200个字符")]
        [DisplayName("备注")]
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
        [DisplayName("记录创建时间")]
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
        [DisplayName("创建者")]
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
        [DisplayName("最后更新时间")]
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
        [DisplayName("最后更新者")]
        public string LastUpdatorUserId
        {
            get { return lastUpdatorUserId; }
            set { Set(ref lastUpdatorUserId, value); }
        }
        #endregion

        protected override void Disposing()
        {
            EquipmentName = null;
            EquipmentInfo_Id = null;
            EquipmentInfo = null;
            MaintenanceCode = null;
            Maker = null;
            PlanStartTime = null;
            PlanFinishTime = null;
            ActualStartTime = null;
            ActualFinishTime = null;
            MaintenanceContent = null;
            UndertakeTeamID = null;
            Result = null;
            CheckDepartmentID = null;
            MaintenancePerson = null;
            Remark = null;
            CreatorUserId = null;
            LastUpdatedTime = null;
            LastUpdatorUserId = null;
        }

    }

}
