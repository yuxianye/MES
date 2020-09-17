using OSharp.Data.Entity;
using Solution.ProductManagement.Models;
using System;

namespace Solution.ProductManagement.ModelConfigurations
{
   public class ProductionRuleItemInfoConfigration : EntityConfigurationBase<ProductionRuleItemInfo, Guid>
    {
        public ProductionRuleItemInfoConfigration()
        {
            HasRequired(m => m.ProductionRule).WithMany();
            HasRequired(m => m.ProductionProcess).WithMany();
        }
    }
}
