using OSharp.Data.Entity;
using Solution.StereoscopicWarehouseManagement.Models;
using System;

namespace Solution.StereoscopicWarehouseManagement.ModelConfigurations
{
    public class MatStorageModifyInfoConfigration : EntityConfigurationBase<MatStorageModifyInfo, Guid>
    {
        public MatStorageModifyInfoConfigration()
        {
            HasRequired(m => m.MaterialBatch).WithMany();
        }
    }
}