using System.ComponentModel;

namespace Solution.Desktop.DisTaskDispatchInfo.Model
{
    public static class SystemTypeEnumModel
    {
        public enum SystemType
        {
            /// <summary>
            /// 机器人搬运系统
            /// </summary>
            [Description("机器人搬运系统")]
            机器人搬运系统 = 1,

            /// <summary>
            /// 智能仓储系统
            /// </summary>
            [Description("智能仓储系统")]
            智能仓储系统 = 2,

            /// <summary>
            /// 物流系统
            /// </summary>
            [Description("物流系统")]
            物流系统 = 3,
        }
    }
}
