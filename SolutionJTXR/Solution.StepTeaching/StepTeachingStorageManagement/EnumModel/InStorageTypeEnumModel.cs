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
            原料手动入库 = 1,

            /// <summary>
            /// 原料自动入库
            /// </summary>
            [Description("原料自动入库")]
            原料自动入库 = 2,

            /// <summary>
            /// 成品自动入库
            /// </summary>
            [Description("成品自动入库")]
            成品自动入库 = 3,

            /// <summary>
            /// 空托盘入库
            /// </summary>
            [Description("空托盘入库")]
            空托盘入库 = 4,
        }
    }
}
