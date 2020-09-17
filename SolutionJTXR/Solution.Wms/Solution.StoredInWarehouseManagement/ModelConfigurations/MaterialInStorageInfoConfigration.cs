using OSharp.Data.Entity;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StoredInWarehouseManagement.Models;
using System;

namespace Solution.StoredInWarehouseManagement.ModelConfigurations
{
    public class MaterialInStorageInfoConfigration : EntityConfigurationBase<MaterialInStorageInfo, Guid>
    {
        public MaterialInStorageInfoConfigration()
        {
            //HasRequired(m => m.Material).WithMany();
        }
    }
}