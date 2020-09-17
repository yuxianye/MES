using OSharp.Data.Entity;
using Solution.CommunicationModule.Models;
using Solution.Core.Data;
using System;

namespace Solution.CommunicationModule.ModelConfigurations
{
    public class EquipmentBusinessNodeMapConfigrations : EntityConfigurationBase<ProductionProcessEquipmentBusinessNodeMap, Guid>
    {
        public override Type DbContextType
        {
            get { return typeof(CommunicationDbContext); }
        }
        public EquipmentBusinessNodeMapConfigrations()
        {
            HasRequired(m => m.BusinessNode).WithMany();
            HasRequired(m => m.DeviceNode).WithMany();
        }
    }
}
