using OSharp.Data.Entity;
using Solution.Agv.Models;
using System;

namespace Solution.Agv.ModelConfigurations
{
    public class TaskInfoConfigration : EntityConfigurationBase<TaskInfo, Guid>
    {
        public TaskInfoConfigration()
        {
            HasRequired(m => m.TargetPointInfo).WithMany();
            HasRequired(m => m.TaskCar).WithMany();
            HasRequired(m => m.RouteInfo).WithMany();
        }
    }
}
