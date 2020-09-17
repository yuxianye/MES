using OSharp.Data.Entity;
using Solution.PlanManagement.Models;
using System;

namespace Solution.PlanManagement.ModelConfigurations
{
    public class PlanProductionScheduleInfoConfiguration : EntityConfigurationBase<PlanProductionScheduleInfo, Guid>
    {
        public PlanProductionScheduleInfoConfiguration()
        {
            HasRequired(m => m.OrderItem).WithMany();
            HasRequired(m => m.ProductionRule).WithMany();
        }
    }
}