using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class InventoryTypeEnumModel
    {
        public enum InventoryType
        {
            /// <summary>
            /// 抽盘
            /// </summary>
            [Description("抽盘")]
            InventoryUnRegularType = 1,

            /// <summary>
            /// 定期盘点
            /// </summary>
            [Description("定期盘点")]
            InventoryRegularType = 2,
          
        }
    }
}
