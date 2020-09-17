using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class WareHouseLocationTypeEnumModel
    {
        public enum WareHouseLocationType
        {
            /// <summary>
            /// 原料库位
            /// </summary>
            [Description("原料库位")]
            MaterialWareHouseLocationType = 1,

            /// <summary>
            /// 成品库位
            /// </summary>
            [Description("成品库位")]
            ProductWareHouseLocationType = 2,

            /// <summary>
            /// 半成品库位
            /// </summary>
            [Description("半成品库位")]
            SemiProductWareHouseLocationType = 3
        }
    }
}
