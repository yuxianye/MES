using OSharp.Data.Entity;
using Solution.StereoscopicWarehouseManagement.Models;
using System;

namespace Solution.StereoscopicWarehouseManagement.ModelConfigurations
{
    public class MatInventoryInfoConfigration : EntityConfigurationBase<MatInventoryInfo, Guid>
    {
        public MatInventoryInfoConfigration()
        {
            HasRequired(m => m.MatWareHouse).WithMany();
        }
    }
}