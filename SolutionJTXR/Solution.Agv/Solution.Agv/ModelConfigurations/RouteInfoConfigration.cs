using OSharp.Data.Entity;
using Solution.Agv.Models;
using System;

namespace Solution.Agv.ModelConfigurations
{
    public class RouteInfoConfigration : EntityConfigurationBase<RouteInfo, Guid>
    {
        public RouteInfoConfigration()
        {
            HasRequired(m => m.StartMarkPointInfo).WithMany();
            HasRequired(m => m.EndMarkPointInfo).WithMany();
        }
    }
}
