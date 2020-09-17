using OSharp.Data.Entity;
using Solution.EquipmentManagement.Models;
using System;

namespace Solution.EquipmentManagement.ModelConfigurations
{
    public class EquRunningStateInfoConfigration : EntityConfigurationBase<EquRunningStateInfo, Guid>
    {
        public EquRunningStateInfoConfigration()
        {
            HasRequired(m => m.Equipmentinfo).WithMany();
           
        }
    }
}
