using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Solution.Utility.Extensions;

namespace Solution.Desktop.MatWareHouseAreaInfo.Model
{
   public static class WareHouseLocationTypeEnumModel
    {
        public enum WareHouseLocationType
        {
            /// <summary>
            /// 原料库位
            /// </summary>
            [Description("原料库位")]
            原料库位 = 1,

            /// <summary>
            /// 成品库位
            /// </summary>
            [Description("成品库位")]
            成品库位 = 2,

            /// <summary>
            /// 半成品库位
            /// </summary>
            [Description("半成品库位")]
            半成品库位 = 3
        }
    }
}
