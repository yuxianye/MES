using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class StorageMoveStateEnumModel
    {
        public enum StorageMoveState
        {
            /// <summary>
            /// 未开始
            /// </summary>
            [Description("未开始")]
            StorageMoveUnFinishState = 1,

            /// <summary>
            /// 移库完成
            /// </summary>
            [Description("移库完成")]
            StorageMoveFinishState = 2,
          
        }
    }
}
