using System.ComponentModel;

namespace Solution.MatWarehouseStorageManagement.Dtos
{
    public static class StorageMoveStateEnumModel
    {
        public enum StorageMoveState
        {
            /// <summary>
            /// 未开始
            /// </summary>
            [Description("未开始")]
            未开始 = 1,

            /// <summary>
            /// 移库完成
            /// </summary>
            [Description("移库完成")]
            移库完成 = 2,
          
        }
    }
}
