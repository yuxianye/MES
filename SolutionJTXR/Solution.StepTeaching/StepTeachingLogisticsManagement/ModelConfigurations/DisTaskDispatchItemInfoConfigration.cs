using OSharp.Data.Entity;
using Solution.MatWarehouseStorageManagement.Models;
using Solution.StepTeachingDispatchManagement.Models;
using System;

namespace Solution.StepTeachingDispatchManagement.ModelConfigurations
{
    public class DisTaskDispatchItemInfoConfigration : EntityConfigurationBase<DisTaskDispatchItemInfo, Guid>
    {
        public DisTaskDispatchItemInfoConfigration()
        {
            HasRequired(m => m.DisStepTeachingTaskDispatch).WithMany();
            HasRequired(m => m.Equipment).WithMany();
            HasRequired(m => m.EquipmentAction).WithMany();
        }
    }
}
