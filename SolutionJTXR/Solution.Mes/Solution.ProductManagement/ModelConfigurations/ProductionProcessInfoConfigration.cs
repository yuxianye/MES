using OSharp.Data.Entity;
using Solution.ProductManagement.Models;
using System;

namespace Solution.ProductManagement.ModelConfigurations
{
    public class ProductionProcessInfoConfigration : EntityConfigurationBase<ProductionProcessInfo, Guid>
    {
        public ProductionProcessInfoConfigration()
        {
            HasRequired(m => m.EntProductionLine).WithMany();
        }
    }
}
