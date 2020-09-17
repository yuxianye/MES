using System.ComponentModel;

namespace Solution.MatWarehouseStorageManagement.Dtos
{
    public static class InStorageTypeEnumModel
    {
        public enum InStorageType
        {
            /// <summary>
            /// 原料手动入库
            /// </summary>
            [Description("原料手动入库")]
            MaterialManuallyInStorageType = 1,

            /// <summary>
            /// 原料自动入库
            /// </summary>
            [Description("原料自动入库")]
            MaterialAutoInStorageType = 2,

            /// <summary>
            /// 成品自动入库
            /// </summary>
            [Description("成品自动入库")]
            ProductAutoInStorageType = 3,

            /// <summary>
            /// 空托盘入库
            /// </summary>
            [Description("空托盘入库")]
            PalletInStorageType = 4,
        }
    }
}
