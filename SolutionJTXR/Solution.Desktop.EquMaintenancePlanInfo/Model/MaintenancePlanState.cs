using System.ComponentModel;

namespace Solution.Desktop.EquMaintenancePlanInfo.Model
{
    public enum MaintenancePlanState
    {
        [Description("未开始")]
        NotBegain = 0,
        [Description("已完成")]
        Completed = 1
    }
}
