using OSharp.Data.Entity;
using Solution.EquipmentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.EquipmentManagement.ModelConfigurations
{
    public class EquRepairsInfoConfigration : EntityConfigurationBase<EquRepairsInfo, Guid>
    {
        public EquRepairsInfoConfigration()
        {
            HasRequired(m => m.EquipmentInfo).WithMany();
            HasRequired(m => m.SparePartsInfo).WithMany();
        }
    }
}
