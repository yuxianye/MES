using OSharp.Data.Entity;
using Solution.ProductManagement.Models;
using System;

namespace Solution.ProductManagement.ModelConfigurations
{
   public class ProManufacturingBOMBillItemInfoConfigration : EntityConfigurationBase<ProManufacturingBOMBillItemInfo, Guid>
    {
        public ProManufacturingBOMBillItemInfoConfigration()
        {
            HasRequired(m => m.ProManufacturingBill).WithMany();
            HasRequired(m => m.ProductionProcess).WithMany();
            HasRequired(m => m.Material).WithMany();
        }
    }
}
