using OSharp.Data.Entity;
using Solution.EquipmentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EquipmentManagement.ModelConfigurations
{
    public class EquMaintenancePlanInfoConfigration : EntityConfigurationBase<EquMaintenancePlanInfo, Guid>
    {
        public EquMaintenancePlanInfoConfigration()
        {
            HasRequired(m => m.EquipmentInfo).WithMany();
            HasRequired(m => m.CheckDepartment).WithMany();
        }
    }
}
