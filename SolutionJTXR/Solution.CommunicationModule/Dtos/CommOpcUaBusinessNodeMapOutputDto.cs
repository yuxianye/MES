using OSharp.Core.Data;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Dtos
{
    public class CommOpcUaBusinessNodeMapOutputDto : IOutputDto
    {
        public Guid Id { get; set; }
        public CommOpcUaBusiness OpcUaBusiness{get;set;}

        public DeviceNode OpcUaNode { get; set; }
    }
}
