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
            入库 = 1,

            /// <summary>
            /// 出库
            /// </summary>
            [Description("出库")]
            出库 = 2,

            /// <summary>
            /// 移库
            /// </summary>
            [Description("移库")]
            移库 = 3,

            /// <summary>
            /// 调整
            /// </summary>
            [Description("调整")]
            调整 = 4,


            /// <summary>
            /// 盘点
            /// </summary>
            [Description("盘点")]
            盘点 = 5,
        }
    }
}
