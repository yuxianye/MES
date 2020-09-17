using OSharp.Data.Entity;
using Solution.EquipKnifeToolInfo.Models;
using System;

namespace Solution.EquipKnifeToolInfo.ModelConfigurations
{
    public class EquKnifeToolInfoConfigration : EntityConfigurationBase<EquKnifeToolInfo, Guid>
    {
        public EquKnifeToolInfoConfigration()
        {
            HasRequired(m => m.Knifetooltypeinfo).WithMany();
           
        }
    }
}
