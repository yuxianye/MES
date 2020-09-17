using OSharp.Data.Entity;
using Solution.StereoscopicWarehouseManagement.Models;
using System;

namespace Solution.StereoscopicWarehouseManagement.ModelConfigurations
{
    public class MatInventoryItemInfoConfigration : EntityConfigurationBase<MatInventoryItemInfo, Guid>
    {
        public MatInventoryItemInfoConfigration()
        {
            HasRequired(m => m.MatInventory).WithMany();
            HasRequired(m => m.MaterialBatch).WithMany();
        }
    }
}