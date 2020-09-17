using OSharp.Data.Entity;
using Solution.EquipmentManagement.Models;
using System;

namespace Solution.EquipmentManagement.ModelConfigurations
{
    public class EquSparePartsInfoConfigration : EntityConfigurationBase<EquSparePartsInfo, Guid>
    {
        public EquSparePartsInfoConfigration()
        {
            HasRequired(m => m.Equspareparttypeinfo).WithMany();
           
        }
    }
}
