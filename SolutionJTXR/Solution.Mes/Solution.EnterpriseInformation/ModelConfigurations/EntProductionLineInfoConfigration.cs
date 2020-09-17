using OSharp.Data.Entity;
using Solution.EnterpriseInformation.Models;
using System;

namespace Solution.EnterpriseInformation.ModelConfigurations
{
   public class EntProductionLineInfoConfigration : EntityConfigurationBase<EntProductionLineInfo, Guid>
    {
        public EntProductionLineInfoConfigration()
        {
            HasRequired(m => m.EntArea).WithMany();
        }
    }
}
