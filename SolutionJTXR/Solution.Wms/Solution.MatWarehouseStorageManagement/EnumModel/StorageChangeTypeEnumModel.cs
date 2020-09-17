using System.ComponentModel;

namespace Solution.MatWarehouseStorageManagement.Dtos
{
    public static class StorageChangeTypeEnumModel
    {
        public enum StorageChangeType
        {
            /// <summary>
            /// 入库
            /// </summary>
            [Description("入库")]
            InStorageChangeType = 1,

            /// <summary>
            /// 出库
            /// </summary>
            [Description("出库")]
            OutStorageChangeType = 2,

            /// <summary>
            /// 移库
            /// </summary>
            [Description("移库")]
            MoveStorageChangeType = 3,

            /// <summary>
            /// 调整
            /// </summary>
            [Description("调整")]
            ModifyStorageChangeType = 4,


            /// <summary>
            /// 盘点
            /// </summary>
            [Description("盘点")]
            InventoryStorageChangeType = 5,
        }
    }
}
