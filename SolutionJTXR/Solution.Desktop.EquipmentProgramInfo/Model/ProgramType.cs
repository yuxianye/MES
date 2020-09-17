using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.EquipmentProgramInfo.Model
{
  public static class ProgramTypeEnum
    {
        public enum ProgramType
        {
            /// <summary>
            /// 件
            /// </summary>
            [Description("机床程序")]
            机床程序 = 1,
            /// <summary>
            /// 个
            /// </summary>
            [Description("机器人程序")]
            机器人程序 = 2,
            /// <summary>
            /// 打
            /// </summary>
            [Description("PLC程序")]
            PLC程序 = 3



        }
    }
}
