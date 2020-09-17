using OSharp.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Dtos
{
    public class CommOpcUaServerOutputDto:IOutputDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// OPC UA服务器ID
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// OPC UA服务器名字
        /// </summary>
        public int ServerName { get; set; }

        /// <summary>
        /// OPC UA服务器URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
