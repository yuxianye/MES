using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Desktop.EquipmentInfo.Model
{
    public enum EquEquipmentState
    {
        [Description("启动")]
        Start,
        [Description("调拨")]
        Allocation,
        [Description("封存")]
        SealUp,
        [Description("报废")]
        Scrap,
        [Description("闲置")]
        Idle,
        [Description("移装")]
        Shifting
    }
}
