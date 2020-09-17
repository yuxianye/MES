using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.EntWorkAttendanceRecordInfo.EnumType
{
    public enum LeaveType : int
    {
        /// <summary>
        ///  请假类型
        /// </summary>
        /// 
        [Description("事假")]
        PersonalLeave = 1,

        [Description("病假")]
        SickLeave = 2,

        [Description("年假")]
        YearLeave = 3,
    }

    public enum Shifts : int
    {
        /// <summary>
        ///  班次
        /// </summary>
        /// 
        [Description("早班")]
        MorningShift = 1,

        [Description("中班")]
        MiddleShift = 2,

        [Description("晚班")]
        NightShift = 3,
    }
}
