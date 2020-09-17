using OSharp.Data.Entity;
using Solution.Agv.Models;
using System;

namespace Solution.Agv.ModelConfigurations
{
    public class RoadInfoConfigration : EntityConfigurationBase<RoadInfo, Guid>
    {
        public RoadInfoConfigration()
        {
            HasRequired(m => m.StartMarkPointInfo).WithMany();
            HasRequired(m => m.EndMarkPointInfo).WithMany();
        }
    }
}
