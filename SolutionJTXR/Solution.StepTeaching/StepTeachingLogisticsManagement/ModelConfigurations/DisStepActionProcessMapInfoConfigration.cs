using OSharp.Data.Entity;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StepTeachingDispatchManagement.Models;
using Solution.ProductManagement.Models;

using System;

namespace Solution.StepTeachingDispatchManagement.ModelConfigurations
{
    public class DisStepActionProcessMapInfoConfigration : EntityConfigurationBase<DisStepActionProcessMapInfo, Guid>
    {
        public DisStepActionProcessMapInfoConfigration()
        {
            HasRequired(m => m.DisStepActionInfo).WithMany();
            HasRequired(m => m.ProductionProcessInfo).WithMany();
        }
    }
}

