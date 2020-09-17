using OSharp.Data.Entity;
using Solution.PlanManagement.Models;
using System;

namespace Solution.PlanManagement.ModelConfigurations
{
    public class PlanOrderItemInfoConfiguration : EntityConfigurationBase<PlanOrderItemInfo, Guid>
    {
        public PlanOrderItemInfoConfiguration()
        {
            HasRequired(m => m.Order).WithMany();
            HasRequired(m => m.Product).WithMany();
        }
    }
}