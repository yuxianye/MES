using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class InventoryStateEnumModel
    {
        public enum InventoryState
        {
            /// <summary>
            /// 未开始
            /// </summary>
            [Description("未开始")]
            InventoryUnFinishState = 1,

            /// <summary>
            /// 进行中
            /// </summary>
            [Description("进行中")]
            InventoryingState = 2,

            /// <summary>
            /// 盘点结束
            /// </summary>
            [Description("盘点结束")]
            InventoryFinishState = 3,

        }
    }
}
