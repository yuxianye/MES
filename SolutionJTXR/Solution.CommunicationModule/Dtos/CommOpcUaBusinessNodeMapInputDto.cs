using OSharp.Core.Data;
using Solution.CommunicationModule.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.CommunicationModule.Dtos
{
    public class CommOpcUaBusinessNodeMapInputDto : IInputDto<Guid>
    {
        public Guid Id { get; set; }

        public CommOpcUaBusiness OpcUaBusiness { get; set; }

        public DeviceNode OpcUaNode { get; set; }
        public Guid EquipmentID { get; set; }
        public Guid OpcUaBusiness_Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatorUserId { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string LastUpdatorUserId { get; set; }

    }
}
