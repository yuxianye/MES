using OSharp.Core.Data;
using Solution.CommunicationModule.Models;
using System;

namespace Solution.CommunicationModule.Dtos
{
    public class CommOpcUaHistoryOutputDto : IOutputDto
    {
        public Guid Id { get; set; }


        /// <summary>
        /// OpcUa服务器
        /// </summary>
        public CommOpcUaServer OpcUaServer { get; set; }

        /// <summary>
        /// 数据点
        /// </summary>
        public DeviceNode OpcUaNode { get; set; }

        /// <summary>
        /// 数据结果
        /// </summary>
        public String DataValue { get; set; }
    }
}
