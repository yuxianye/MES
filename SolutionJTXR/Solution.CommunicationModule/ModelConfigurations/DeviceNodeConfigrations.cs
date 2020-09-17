using OSharp.Data.Entity;
using Solution.CommunicationModule.Models;
using Solution.Core.Data;
using System;

namespace Solution.CommunicationModule.ModelConfigurations
{
    public class DeviceNodeConfigrations : EntityConfigurationBase<DeviceNode, Guid>
    {
        public override Type DbContextType
        {
            get { return typeof(CommunicationDbContext); }
        }
        public DeviceNodeConfigrations()
        {
            HasRequired(m => m.DeviceServerInfo).WithMany();
        }
    }
}
