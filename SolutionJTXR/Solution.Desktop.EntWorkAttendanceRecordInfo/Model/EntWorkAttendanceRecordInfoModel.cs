using Solution.Desktop.Core;
using Solution.Desktop.EntWorkAttendanceRecordInfo.EnumType;
using Solution.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Desktop.EntWorkAttendanceRecordInfo.Model
{
    /// <summary>
    /// 区域模型
    /// </summary>
    public class EntWorkAttendanceRecordInfoModel : ModelBase, IAudited
    {
        #region 考勤记录ID
        private Guid id;

        /// <summary>
        /// 考勤记录ID
        /// </summary>

        public Guid Id
        {
            get { return id; }
            set { Set(ref id, value); }
        }

        #endregion

        #region 员工ID
        private Guid _entEmployeeId;

        /// <summary>
        /// 员工编号
        /// </summary>
        public Guid EntEmployeeId
        {
            get { return _entEmployeeId; }
            set { Set(ref _entEmployeeId, value); }
        }

        #endregion


        #region 员工编号
        private string _entEmployeeCode;

        /// <summary>
        /// 员工编号
        /// </summary>
        public string EntEmployeeCode
        {
            get { return _entEmployeeCode; }
            set { Set(ref _entEmployeeCode, value); }
        }

        #endregion

        #region 员工名称
        private string _entEmployeeName;

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EntEmployeeName
        {
            get { return _entEmployeeName; }
            set { Set(ref _entEmployeeName, value); }
        }

        #endregion

        #region 考勤日期
        private DateTime attendanceDate;

        public DateTime AttendanceDate
        {
            get { return attendanceDate; }
            set { Set(ref attendanceDate, value); }
        }

        #endregion

        #region 上班时间
        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { Set(ref startTime, value); }
        }

        #endregion

        #region 下班时间
        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set { Set(ref endTime, value); }
        }

        #endregion

        #region 请假类型
        private LeaveType leaveType;

        public LeaveType LeaveType
        {
            get { return leaveType; }
            set { Set(ref leaveType, value); }
        }

        #endregion

        #region 请假时长
        private decimal leaveDuration;

        public decimal LeaveDuration
        {
            get { return leaveDuration; }
            set { Set(ref leaveDuration, value); }
        }

        #endregion

        #region 班次
        private Shifts shifts;

        public Shifts Shifts
        {
            get { return shifts; }
            set { Set(ref shifts, value); }
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

        protected override void Disposing()
        {
            Remark = null;
            CreatorUserId = null;
            LastUpdatorUserId = null;
            Remark = null;
        }

    }

}
