using OSharp.Data.Entity;
using Solution.CommunicationModule.Models;
using Solution.Core.Data;
using System;

namespace Solution.CommunicationModule.ModelConfigurations
{
    public class CommOpcUaBusinessNodeMapConfigrations : EntityConfigurationBase<CommOpcUaBusinessNodeMap, Guid>
    {
        public override Type DbContextType
        {
            get { return typeof(CommunicationDbContext); }
        }
        public CommOpcUaBusinessNodeMapConfigrations()
        {
            HasRequired(m => m.OpcUaBusiness).WithMany();
            HasRequired(m => m.OpcUaNode).WithMany();
        }
    }
}
