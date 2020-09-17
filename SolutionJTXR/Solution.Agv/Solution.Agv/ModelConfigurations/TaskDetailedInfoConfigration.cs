using OSharp.Data.Entity;
using Solution.Agv.Models;
using System;

namespace Solution.Agv.ModelConfigurations
{
    public class TaskDetailedInfoConfigration : EntityConfigurationBase<TaskDetailedInfo, Guid>
    {
        public TaskDetailedInfoConfigration()
        {
            HasRequired(m => m.Material).WithMany();
            HasRequired(m => m.TaskInfo).WithMany();
        }
    }
}
