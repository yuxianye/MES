using OSharp.Data.Entity;
using Solution.MatWarehouseStorageManagement.Models;
using System;

namespace Solution.MatWarehouseStorageManagement.ModelConfigurations
{
    public class MatWareHouseInfoConfigration : EntityConfigurationBase<MatWareHouseInfo, Guid>
    {
        public MatWareHouseInfoConfigration()
        {
            HasRequired(m => m.EntArea).WithMany();
            HasRequired(m => m.MatWareHouseType).WithMany();
        }
    }
}
