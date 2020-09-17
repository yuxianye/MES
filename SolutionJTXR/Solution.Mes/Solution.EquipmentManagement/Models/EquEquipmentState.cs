using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EquipmentManagement.Models
{
    public enum EquEquipmentState
    {
        Start, //启动
        Allocation,//调拨
        SealUp,//封存
        Scrap,//报废
        Idle,//闲置
        Shifting//移装
    }
}
