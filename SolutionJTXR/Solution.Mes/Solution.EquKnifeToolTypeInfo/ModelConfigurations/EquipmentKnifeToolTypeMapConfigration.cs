using OSharp.Data.Entity;
using Solution.EquipKnifeToolInfo.Models;
using System;

namespace Solution.EquipKnifeToolInfo.ModelConfigurations
{
    public class EquipmentKnifeToolTypeMapConfigration : EntityConfigurationBase<EquipmentKnifeToolTypeMap, Guid>
    {
        public EquipmentKnifeToolTypeMapConfigration()
        {
            HasRequired(m => m.Knifetooltypeinfo).WithMany();
            HasRequired(m => m.Equipmentinfo).WithMany();
        }
    }
}
