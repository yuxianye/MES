using OSharp.Data.Entity;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StepTeachingDispatchManagement.Models;
using System;

namespace Solution.StepTeachingDispatchManagement.ModelConfigurations
{
    public class DisTaskDispatchInfoConfigration : EntityConfigurationBase<DisTaskDispatchInfo, Guid>
    {
        public DisTaskDispatchInfoConfigration()
        {
            HasRequired(m => m.DisStepAction).WithMany();
        }
    }
}
