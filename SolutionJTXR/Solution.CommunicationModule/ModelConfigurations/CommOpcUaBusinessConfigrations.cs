using OSharp.Data.Entity;
using Solution.CommunicationModule.Models;
using Solution.Core.Data;
using System;

namespace Solution.CommunicationModule.ModelConfigurations
{
    public class CommOpcUaBusinessConfigrations : EntityConfigurationBase<CommOpcUaBusiness, Guid>
    {
        public override Type DbContextType
        {
            get { return typeof(CommunicationDbContext); }
        }
    }
}
