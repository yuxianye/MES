using System.ComponentModel;

namespace Solution.EquipmentManagement.Models
{
    public enum MaintenanceType
    {
        [Description("保养")]
        Maintain = 0,
        [Description("点检")]
        SpotCheck = 1,
        [Description("检修")]
        Overhaul = 2
    }
}
