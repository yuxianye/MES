using System.ComponentModel;

namespace Solution.MatWarehouseStorageManagement.Dtos
{
    public static class InventoryStateEnumModel
    {
        public enum InventoryState
        {
            /// <summary>
            /// 未开始
            /// </summary>
            [Description("未开始")]
            未开始 = 1,

            /// <summary>
            /// 进行中
            /// </summary>
            [Description("进行中")]
            进行中 = 2,

            /// <summary>
            /// 盘点结束
            /// </summary>
            [Description("盘点结束")]
            盘点结束 = 3,

        }
    }
}
