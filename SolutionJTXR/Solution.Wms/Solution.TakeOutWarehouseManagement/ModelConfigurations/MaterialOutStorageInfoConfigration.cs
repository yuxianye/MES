using OSharp.Data.Entity;
using Solution.TakeOutWarehouseManagement.Models;
using System;

namespace Solution.TakeOutWarehouseManagement.ModelConfigurations
{
    public class MaterialOutStorageInfoConfigration : EntityConfigurationBase<MaterialOutStorageInfo, Guid>
    {
        public MaterialOutStorageInfoConfigration()
        {
            //HasRequired(m => m.Material).WithMany();
        }
    }
}
