using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Solution.Utility.Extensions;

namespace Solution.Desktop.ProManufacturingBillManage.Model
{
    public static class ProductEnumModel
    {
        public enum BillType
        {
            /// <summary>
            /// 物料清单
            /// </summary>
            [Description("物料清单BOM")]
            BOM = 1,

            /// <summary>
            /// 资源清单
            /// </summary>
            [Description("资源清单BOR")]
            BOR = 2
        }
    }
}
