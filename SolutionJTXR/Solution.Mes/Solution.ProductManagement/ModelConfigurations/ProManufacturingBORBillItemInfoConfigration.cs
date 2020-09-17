using OSharp.Data.Entity;
using Solution.ProductManagement.Models;
using System;

namespace Solution.ProductManagement.ModelConfigurations
{
  public  class ProManufacturingBORBillItemInfoConfigration : EntityConfigurationBase<ProManufacturingBORBillItemInfo, Guid>
    {
        public ProManufacturingBORBillItemInfoConfigration()
        {
            HasRequired(m => m.ProManufacturingBill).WithMany();
            HasRequired(m => m.ProductionProcess).WithMany();
            HasRequired(m => m.Equipment).WithMany();
        }
    }
}
