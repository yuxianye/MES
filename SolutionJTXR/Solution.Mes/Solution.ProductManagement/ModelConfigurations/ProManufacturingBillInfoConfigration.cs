using OSharp.Data.Entity;
using Solution.ProductManagement.Models;
using System;

namespace Solution.ProductManagement.ModelConfigurations
{
   public class ProManufacturingBillInfoConfigration : EntityConfigurationBase<ProManufacturingBillInfo, Guid>
    {
        public ProManufacturingBillInfoConfigration()
        {
            HasRequired(m => m.Product).WithMany();
            HasRequired(m => m.ProductionRule).WithMany();
        }
    }
}
