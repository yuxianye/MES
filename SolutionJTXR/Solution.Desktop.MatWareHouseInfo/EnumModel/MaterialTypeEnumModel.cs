using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class MaterialTypeEnumModel
    {
        public enum MaterialType
        {
            /// <summary>
            /// 原料
            /// </summary>
            [Description("原料")]
            Material = 1,

            /// <summary>
            /// 半成品
            /// </summary>
            [Description("半成品")]
            SemiProduct = 2,

            /// <summary>
            /// 成品
            /// </summary>
            [Description("成品")]
            Product = 3
        }
    }
}
