using OSharp.Data.Entity;
using Solution.PlanManagement.Models;
using System;

namespace Solution.PlanManagement.ModelConfigurations
{
    public class PlanEquipmentRequirementInfoConfiguration : EntityConfigurationBase<PlanEquipmentRequirementInfo, Guid>
    {
        public PlanEquipmentRequirementInfoConfiguration()
        {
            HasRequired(m => m.ProductionProcessRequirement).WithMany();
            HasRequired(m => m.Equipment).WithMany();
        }
    }
}