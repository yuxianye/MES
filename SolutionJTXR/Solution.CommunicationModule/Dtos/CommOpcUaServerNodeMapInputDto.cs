using OSharp.Core.Data;
using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Dtos
{
    public class CommOpcUaServerNodeMapInputDto : IInputDto<Guid>
    {
        public Guid Id { get; set; }

        public CommOpcUaServer OpcUaServer { get; set; }

        public DeviceNode OpcUaNode { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatorUserId { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string LastUpdatorUserId { get; set; }

    }
}
