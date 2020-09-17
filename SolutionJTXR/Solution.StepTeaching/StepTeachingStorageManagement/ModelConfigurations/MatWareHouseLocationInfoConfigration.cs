using OSharp.Data.Entity;
using Solution.MatWarehouseStorageManagement.Models;
using System;

namespace Solution.MatWarehouseStorageManagement.ModelConfigurations
{
    public class MatWareHouseLocationInfoConfigration : EntityConfigurationBase<MatWareHouseLocationInfo, Guid>
    {
        public MatWareHouseLocationInfoConfigration()
        {
            HasRequired(m => m.MatWareHouse).WithMany();
            HasRequired(m => m.MatWareHouseArea).WithMany();
        }
    }
}
