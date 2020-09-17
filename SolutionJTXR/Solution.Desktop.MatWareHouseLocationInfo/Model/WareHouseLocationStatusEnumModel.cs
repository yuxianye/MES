using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Solution.Utility.Extensions;

namespace Solution.Desktop.MatWareHouseLocationInfo.Model
{
   public static class WareHouseLocationStatusEnumModel
    {
        public enum WareHouseLocationStatus
        {
            /// <summary>
            /// 空库位
            /// </summary>
            [Description("空库位")]
            空库位 = 1,

            /// <summary>
            /// 空托盘
            /// </summary>
            [Description("空托盘")]
            空托盘 = 2,

            /// <summary>
            /// 满库位
            /// </summary>
            [Description("满库位")]
            满库位 = 3,

            /// <summary>
            /// 部分库位
            /// </summary>
            [Description("部分库位")]
            部分库位 = 4,

            /// <summary>
            /// 异常库位
            /// </summary>
            [Description("异常库位")]
            异常库位 = 5
        }
    }
}
