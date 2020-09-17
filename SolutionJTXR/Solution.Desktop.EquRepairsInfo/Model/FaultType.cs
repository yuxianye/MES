using System.ComponentModel;

namespace Solution.Desktop.EquRepairsInfo.Model
{
    /// <summary>
    /// 故障类别
    /// </summary>
    public enum FaultType
    {
        [Description("一般")]
        Commonly = 0,
        [Description("轻微")]
        Slight = 1,
        [Description("严重")]
        Serious = 2,
        [Description("致命")]
        Deadly = 3
    }
}
