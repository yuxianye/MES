using OSharp.Data.Entity;
using Solution.StoredInWarehouseManagement.Models;
using System;

namespace Solution.StoredInWarehouseManagement.ModelConfigurations
{
    public class MaterialBatchInfoConfigration : EntityConfigurationBase<MaterialBatchInfo, Guid>
    {
        public MaterialBatchInfoConfigration()
        {
            HasRequired(m => m.MaterialInStorage).WithMany();
            HasRequired(m => m.Material).WithMany();
            HasRequired(m => m.MatWareHouseLocation).WithMany();
        }
    }
}
