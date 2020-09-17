using OSharp.Core.Data;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Dtos
{
    public class CommOpcUaServerNodeMapOutputDto : IOutputDto
    {
        public Guid Id { get; set; }

        public CommOpcUaServer OpcUaServer { get; set; }

        public DeviceNode OpcUaNode { get; set; }
    }
}
