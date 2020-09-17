using OSharp.Core.Data;
using OSharp.Utility.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EnterpriseInformation.Models
{
    [Description("考勤信息")]
    public class EntWorkAttendanceRecordInfo : EntityBase<Guid>, IAudited
    {
        /// <summary>
        /// 考勤记录ID
        /// </summary>
        public EntWorkAttendanceRecordInfo() => Id = CombHelper.NewComb();

        /// <summary>
        /// 员工ID
        /// </summary>
        public virtual EntEmployeeInfo EntEmployeeInfo { get; set; }

        /// <summary>
        /// 员工Code
        /// </summary>
        public string EntEmployeeCode { get; set; }
        /// <summary>
        /// 考勤日期
        /// </summary>
        public DateTime AttendanceDate { get; set; }

        /// <summary>
        /// 上班时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 下班时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        public int LeaveType { get; set; }

        /// <summary>
        /// 请假时长
        /// </summary>
        public decimal LeaveDuration { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        public int Shifts { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Remark { get; set; }

        #region Implementation of ICreatedTime

        /// <summary>
        /// 获取设置 信息创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        #endregion

        #region Implementation of ICreatedAudited

        /// <summary>
        /// 获取或设置 创建者编号
        /// </summary>
        [StringLength(50)]
        public string CreatorUserId { get; set; }

        #endregion

        #region Implementation of IUpdateAutited

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Index]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 获取或设置 最后更新者编号
        /// </summary>
        [StringLength(50)]
        public string LastUpdatorUserId { get; set; }

        #endregion
    }
}
