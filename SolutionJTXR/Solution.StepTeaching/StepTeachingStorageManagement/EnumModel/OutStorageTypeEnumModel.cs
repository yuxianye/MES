using System.ComponentModel;

namespace Solution.MatWarehouseStorageManagement.Dtos
{
    public static class OutStorageTypeEnumModel
    {
        public enum OutStorageType
        {
            /// <summary>
            /// 原料自动出库
            /// </summary>
            [Description("原料自动出库")]
            原料自动出库 = 1,

            /// <summary>
            /// 成品手动出库
            /// </summary>
            [Description("成品手动出库")]
            成品手动出库 = 2,

            /// <summary>
            /// 空托盘出库
            /// </summary>
            [Description("空托盘出库")]
            空托盘出库 = 3,

            /// <summary>
            /// 原料自动出库演示
            /// </summary>
            [Description("原料自动出库演示")]
            原料自动出库演示 = 4,
        }
    }
}
