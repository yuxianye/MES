using OSharp.Data.Entity;
using Solution.PlanManagement.Models;
using System;

namespace Solution.PlanManagement.ModelConfigurations
{
    public class PlanProductionProcessRequirementInfoConfiguration : EntityConfigurationBase<PlanProductionProcessRequirementInfo, Guid>
    {
        public PlanProductionProcessRequirementInfoConfiguration()
        {
            HasRequired(m => m.ProductionSchedule).WithMany();
            HasRequired(m => m.ProductionProcess).WithMany();
        }
    }
}