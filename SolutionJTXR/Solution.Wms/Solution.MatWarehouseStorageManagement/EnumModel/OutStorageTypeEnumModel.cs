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
            MaterialAutoOutStorageType = 1,

            /// <summary>
            /// 成品手动出库
            /// </summary>
            [Description("成品手动出库")]
            ProductManuallyOutStorageType = 2,

            /// <summary>
            /// 空托盘出库
            /// </summary>
            [Description("空托盘出库")]
            PalletOutStorageType = 3,

            /// <summary>
            /// 原料自动出库演示
            /// </summary>
            [Description("原料自动出库演示")]
            MaterialAutoShowOutStorageType = 4,
        }
    }
}
