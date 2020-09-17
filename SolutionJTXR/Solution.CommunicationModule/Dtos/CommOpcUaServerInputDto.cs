using OSharp.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationModule.Dtos
{
    public class CommOpcUaServerInputDto : IInputDto<Guid>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// OPC UA服务器ID
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// OPC UA服务器名字
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// OPC UA服务器URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatorUserId { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string LastUpdatorUserId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
