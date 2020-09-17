using OSharp.Data.Entity;
using Solution.CommunicationModule.Models;
using Solution.Core.Data;
using System;

namespace Solution.CommunicationModule.ModelConfigurations
{
    public class CommOpcUaServerNodeMapConfigrations : EntityConfigurationBase<CommOpcUaServerNodeMap, Guid>
    {
        public override Type DbContextType
        {
            get { return typeof(CommunicationDbContext); }
        }
        public CommOpcUaServerNodeMapConfigrations()
        {
            HasRequired(m => m.OpcUaServer).WithMany();
            HasRequired(m => m.OpcUaNode).WithMany();
        }
    }
}
