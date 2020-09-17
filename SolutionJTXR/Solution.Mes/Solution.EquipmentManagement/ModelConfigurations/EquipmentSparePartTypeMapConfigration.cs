using OSharp.Data.Entity;
using Solution.EquipmentManagement.Models;
using System;

namespace Solution.EquKnifeToolTypeInfo.ModelConfigurations
{
    public class EquipmentKnifeToolTypeMapConfigration : EntityConfigurationBase<EquipmentSparePartTypeMap, Guid>
    {
        public EquipmentKnifeToolTypeMapConfigration()
        {
            HasRequired(m => m.Equspareparttypeinfo).WithMany();
            HasRequired(m => m.Equipmentinfo).WithMany();
        }
    }
}
