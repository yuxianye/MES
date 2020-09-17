using OSharp.Data.Entity;
using Solution.PlanManagement.Models;
using System;

namespace Solution.PlanManagement.ModelConfigurations
{
    public class PlanMaterialRequirementInfoConfiguration : EntityConfigurationBase<PlanMaterialRequirementInfo, Guid>
    {
        public PlanMaterialRequirementInfoConfiguration()
        {
            HasRequired(m => m.ProductionProcessRequirement).WithMany();
            HasRequired(m => m.Material).WithMany();
        }
    }
}