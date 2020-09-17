using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class OutStorageStatusEnumModel
    {
        public enum OutStorageStatus
        {
            /// <summary>
            /// 待组盘
            /// </summary>
            [Description("待组盘")]
            OutStorageUnFinishStatus = 1,

            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            OutStorageFinishStatus = 2,
        }
    }
}
