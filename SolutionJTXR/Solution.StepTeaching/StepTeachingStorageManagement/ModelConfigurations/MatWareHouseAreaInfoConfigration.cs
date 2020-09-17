using OSharp.Data.Entity;
using Solution.MatWarehouseStorageManagement.Models;
using System;

namespace Solution.MatWarehouseStorageManagement.ModelConfigurations
{
    public class MatWareHouseAreaInfoConfigration : EntityConfigurationBase<MatWareHouseAreaInfo, Guid>
    {
        public MatWareHouseAreaInfoConfigration()
        {
            HasRequired(m => m.MatWareHouse).WithMany();
        }
    }
}
