using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.EquSparePartsInfo.Model
{
    public static class SparePartUnitEnum
    {
        public enum SparePartUnit
        {
            /// <summary>
            /// 件
            /// </summary>
            [Description("件")]
            Piece = 1,
            /// <summary>
            /// 个
            /// </summary>
            [Description("个")]
            Number = 2,
            /// <summary>
            /// 打
            /// </summary>
            [Description("打")]
            Dozen = 3



        }
    }
}
