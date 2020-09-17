using Solution.CommunicationModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Solution.CommunicationModule.Dtos
{
    public class CommOpcUaBusinessManageInputDto : CommOpcUaBusinessNodeMapInputDto
    {
        public List<CommOpcUaNode> CommOpcUaNodeInfoList { get; set; }
    }
}
