using OSharp.Data.Entity;
using Solution.ProductManagement.Models;
using System;

namespace Solution.ProductManagement.ModelConfigurations
{
    public class ProductionRuleInfoConfigration : EntityConfigurationBase<ProductionRuleInfo, Guid>
    {
        public ProductionRuleInfoConfigration()
        {
            HasRequired(m => m.Product).WithMany();
            HasRequired(m => m.ProductionRuleStatus).WithMany();
        }
    }
}
