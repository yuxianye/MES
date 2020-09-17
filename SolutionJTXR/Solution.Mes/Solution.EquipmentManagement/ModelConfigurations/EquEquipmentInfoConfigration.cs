using OSharp.Data.Entity;
using Solution.EquipmentManagement.Models;
using System;

namespace Solution.EquipmentManagement.ModelConfigurations
{
    public class EquEquipmentInfoConfigration : EntityConfigurationBase<EquEquipmentInfo, Guid>
    {
        public EquEquipmentInfoConfigration()
        {
            HasRequired(m => m.Equipmenttype).WithMany();
            HasRequired(m => m.DepartmentInfo).WithMany();
            HasRequired(m => m.EquFactoryInfo).WithMany();
        }
    }
}
