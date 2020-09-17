using OSharp.Data.Entity;
using Solution.Agv.Models;
using System;

namespace Solution.Agv.ModelConfigurations
{
    public class AlarmInfoConfigration : EntityConfigurationBase<AlarmInfo, Guid>
    {

        public AlarmInfoConfigration()
        {
            HasRequired(m => m.AgvInfo).WithMany();
        }
    }
}
