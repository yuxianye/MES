using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class MaterialUnitEnumModel
    {
        public enum MaterialUnit
        {
            /// <summary>
            /// 千克
            /// </summary>
            [Description("千克")]
            KG = 1,

            /// <summary>
            /// 个
            /// </summary>
            [Description("个")]
            Number = 2,

            /// <summary>
            /// 立方米
            /// </summary>
            [Description("立方米")]
            CubicMeter = 3
        }
    }
}
