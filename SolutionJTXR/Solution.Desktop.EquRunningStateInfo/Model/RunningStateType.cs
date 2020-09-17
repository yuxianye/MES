using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.EquRunningStateInfo.Model
{
    public static class RunningStateTypeEnum
    {
        public enum RunningStateType
        {
            /// <summary>
            /// 开机状态
            /// </summary>
            [Description("开机")]
            Open = 1,
            /// <summary>
            /// 关机状态
            /// </summary>
            [Description("关机")]
            Close = 2,
            /// <summary>
            /// 故障状态
            /// </summary>
            [Description("故障")]
            Fault = 3



        }
    }
}
