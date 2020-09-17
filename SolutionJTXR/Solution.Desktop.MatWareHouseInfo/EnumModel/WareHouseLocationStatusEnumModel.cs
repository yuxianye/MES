using System.ComponentModel;

namespace Solution.Desktop.MatWareHouseInfo.Model
{
    public static class WareHouseLocationStatusEnumModel
    {
        public enum WareHouseLocationStatus
        {
            /// <summary>
            /// 空库位
            /// </summary>
            [Description("空库位")]
            WareHouseLocationEmptyStatus = 1,

            /// <summary>
            /// 空托盘
            /// </summary>
            [Description("空托盘")]
            WareHouseLocationPalletStatus = 2,

            /// <summary>
            /// 满库位
            /// </summary>
            [Description("满库位")]
            WareHouseLocationFullStatus = 3,

            /// <summary>
            /// 部分库位
            /// </summary>
            [Description("部分库位")]
            WareHouseLocationPartStatus = 4,

            /// <summary>
            /// 异常库位
            /// </summary>
            [Description("异常库位")]
            WareHouseLocationErrorStatus = 5
        }
    }
}
