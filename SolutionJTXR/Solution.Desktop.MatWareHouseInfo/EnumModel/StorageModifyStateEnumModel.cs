using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class StorageModifyStateEnumModel
    {
        public enum StorageModifyState
        {
            /// <summary>
            /// 未开始
            /// </summary>
            [Description("未开始")]
            StorageModifyUnFinishState = 1,

            /// <summary>
            /// 调整结束
            /// </summary>
            [Description("调整结束")]
            StorageModifyFinishState = 2,
          
        }
    }
}
